﻿#region using.*
using System.Collections.Generic;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.GameObjects;
using RTSgame.Utilities.Graphics;
using RTSgame.Utilities.Graphics.ParticleSystem;
using RTSgame.Collision;
using RTSgame.Collision.Debug;
using System;
using System.IO;
using RTSgame.GameObjects.Doodads;
using RTSgame.Utilities.IO;
using RTSgame.Utilities.World;
using RTSgame.GameObjects.Units;
using RTSgame.GameObjects.Components;
#endregion

namespace RTSgame.Utilities
{
    //The DrawManager is an object which handles the drawing of the game (to avoid writing code in GameplayScreen)
    class DrawManager
    {
        //Objects
        //LinkedList<?> drawList2D = new LinkedList<?>();
        LinkedList<IDrawableWorld> drawList3D = new LinkedList<IDrawableWorld>();
        List<ModelObject> drawListModels = new List<ModelObject>();
        List<SpriteObject> drawListOverlays = new List<SpriteObject>();
        private Boolean initShaders = false;

        //HeightMap
        HeightMap heightMap;

        //Doodads
        IWorld World;

        //Graphical constants
        GraphicsDevice graphics;
        private Shaders shaders = Shaders.GetInstance();
        BasicEffect basicEffect;
        float aspectRatio;
        float fov;
        public CullMode CullModeUsed = CullMode.CullCounterClockwiseFace;
        ParticleManager particleManager = ParticleManager.GetInstance();
        Boolean multiPlayer; //TODO Hittade inget bra ställe att lägga denna? :D

        //Rasterizer
        RasterizerState rasterizerState;

        //Camera
        public Camera leftCamera;
        private Camera rightCamera;
        private Viewport leftView, rightView, mainView;
        private GameUserInterface leftUI;
        private GameUserInterface rightUI;
        private Matrix currentProjection;
        private const float frustumMin = 1.0f; // Will not draw triangles closer than 1 unit
        private const float frustumMax = 60; // Draw distance
        BoundingFrustum viewFrustum;

        // draw techniques for doodads
        private const bool doodadInstancing = true; // false == batching
        private const bool doodadFrustrumCheck = true;
        private const float doodadDrawRadius = 30; // default 30

        //SpriteBatch
        private SpriteBatch spriteBatch;
        private SpriteFont font;

        //Shadows
        private RenderTarget2D renderTarget;
        private RenderTarget2D renderTarget2;
        private bool depthMap = false;
        private bool shadowsEnabled = true;
        private BoundingFrustum lightFrustum;

        private static DrawManager instance;
        #region Constructor
        private DrawManager()
        {


        }
        public void SetupDrawManager(Camera[] cameras, Player[] players, float fov, GraphicsDevice graphics, bool multiPlayer)
        {

            this.leftCamera = cameras[0];
            this.multiPlayer = multiPlayer;
            if (!multiPlayer)
                rightCamera = null;
            else
                rightCamera = cameras[1];
            aspectRatio = graphics.Viewport.AspectRatio;
            this.fov = fov;
            this.graphics = graphics;

            this.drawListDictionary = new Dictionary<Type, List<ModelObject>>();
            DoodadDataStructuresInit();

            spriteBatch = new SpriteBatch(graphics);
            font = AssetBank.GetInstance().GetFont("gamefont");

            if (!multiPlayer)
            {

                currentProjection = Matrix.CreatePerspectiveFieldOfView(
                            MathHelper.ToRadians(fov), aspectRatio,
                            frustumMin, frustumMax);
            }
            else
            {
                currentProjection = Matrix.CreatePerspectiveFieldOfView(
                            MathHelper.ToRadians(fov), aspectRatio / 2,
                            frustumMin, frustumMax);
            }

            //viewfrustum is a boundingfrustum, but will not always use the same matrix
            viewFrustum = new BoundingFrustum(Matrix.Identity);
            this.rasterizerState = new RasterizerState();
            this.rasterizerState.CullMode = CullModeUsed;
            graphics.RasterizerState = rasterizerState;
            if (!initShaders)
            {
                initShaders = true;
                shaders.Initialize(Matrix.Identity, leftCamera.GetViewMatrix(), //TODO Not sure if this needs to be changed for multiplayer?
                currentProjection);
            }
            basicEffect = new BasicEffect(graphics)
            {
                TextureEnabled = true,
                VertexColorEnabled = true,
            };
            renderTarget = new RenderTarget2D(graphics, 2048,
                2048, false, SurfaceFormat.Single, DepthFormat.Depth24);
            renderTarget2 = new RenderTarget2D(graphics, 2048,
                2048, false, SurfaceFormat.Single, DepthFormat.Depth24);
            particleManager.Initialize(graphics);


            //Init viewports




            if (!multiPlayer)
            {
                mainView = new Viewport();
                mainView.X = 0;
                mainView.Y = 0;
                mainView.Width = graphics.Viewport.Width;
                mainView.Height = graphics.Viewport.Height;

                this.leftUI = new GameUserInterface(players[0], mainView, leftCamera);

                //Need to add uis to gamestate to update them properly
                GameState.GetInstance().addUserInterface(leftUI);

                //TODO Maybe use leftView = mainView in one-player 

            }

            else
            {
                leftView = new Viewport();
                leftView.X = 0;
                leftView.Y = 0;
                leftView.Width = graphics.Viewport.Width / 2;
                leftView.Height = graphics.Viewport.Height;

                rightView = new Viewport();
                rightView.X = graphics.Viewport.Width / 2;
                rightView.Y = 0;
                rightView.Width = graphics.Viewport.Width / 2;
                rightView.Height = graphics.Viewport.Height;

                this.leftUI = new GameUserInterface(players[0], leftView, leftCamera);
                this.rightUI = new GameUserInterface(players[1], rightView, rightCamera);
                GameState.GetInstance().addUserInterface(leftUI);
                GameState.GetInstance().addUserInterface(rightUI);
            }
        }


        public static DrawManager GetInstance()
        {
            if (instance == null)
            {
                instance = new DrawManager();
            }
            return instance;
        }
        #endregion

        bool renderTarget1 = true;

        #region DrawAll
        public void DrawAll(GameTime gameTime)
        {
            CalculateFPS(gameTime);

            //Render can prepare objects for drawing,
            //But it is DrawManager that should draw them.

            for (int i = drawListModels.Count - 1; i >= 0; i--)
            {
                if (drawListModels[i].ToBeRemoved())
                {
                    // Remove from the dictionary
                    List<ModelObject> specificObjectList;
                    drawListDictionary.TryGetValue(drawListModels[i].GetType(), out specificObjectList);
                    specificObjectList.Remove(drawListModels[i]);

                    // Remove from the general list
                    drawListModels.RemoveAt(i);
                }
            }

            //Resets some graphicsdevice options after drawing 2D
            graphics.BlendState = BlendState.Opaque;
            graphics.DepthStencilState = DepthStencilState.Default;
            graphics.RasterizerState = RasterizerState.CullCounterClockwise;

            //graphics.Clear(Color.Pink);
            graphics.SamplerStates[1] = SamplerState.PointClamp;

            bool dayCycle = true;
            if (dayCycle)
            {
                shaders.DayRotation(gameTime); // Rotates sunlight
                shaders.SetEffectToDayEffect();
            }

            //Only one camera
            if (!multiPlayer)
            {
                graphics.Viewport = mainView;
                shaders.SetViewMatrix(leftCamera.GetViewMatrix());
                shaders.GetEffect().Parameters["cameraPosition"].SetValue(leftCamera.GetPosition());
                viewFrustum.Matrix = leftCamera.GetViewMatrix() * currentProjection;

                shaders.UpdateLights(gameTime, viewFrustum, shadowsEnabled);
                if (shadowsEnabled)
                {
                    lightFrustum = shaders.GetLightFrustum();
                    CreateShadowMap(leftCamera);
                }
                RenderScene(leftCamera, leftUI);
                //DrawShadowMapToScreen();

                graphics.Textures[0] = null;
                graphics.SamplerStates[0] = SamplerState.LinearWrap;
            }
            //Two cameras
            else
            {
                if (shadowsEnabled)
                {
                    // CREATE SHADOWMAP1
                    renderTarget1 = true;
                    graphics.Viewport = leftView;
                    shaders.SetViewMatrix(leftCamera.GetViewMatrix());
                    viewFrustum.Matrix = leftCamera.GetViewMatrix() * currentProjection;
                    shaders.UpdateLightFrustum(viewFrustum);
                    lightFrustum = shaders.GetLightFrustum();
                    CreateShadowMap(leftCamera);

                    // CREATE SHADOWMAP2
                    renderTarget1 = false;
                    graphics.Viewport = rightView;
                    shaders.SetViewMatrix(rightCamera.GetViewMatrix());
                    viewFrustum.Matrix = rightCamera.GetViewMatrix() * currentProjection;
                    shaders.UpdateLightFrustum(viewFrustum);
                    lightFrustum = shaders.GetLightFrustum();
                    CreateShadowMap(rightCamera);
                }

                // UPDATE THE POINT LIGHTS... fel ställe typ
                shaders.PreparePointLights(gameTime);

                // DRAW LEFT SCREEN
                renderTarget1 = true;
                graphics.Viewport = leftView;
                shaders.SetViewMatrix(leftCamera.GetViewMatrix());
                viewFrustum.Matrix = leftCamera.GetViewMatrix() * currentProjection;
                shaders.GetEffect().Parameters["cameraPosition"].SetValue(leftCamera.GetPosition());
                shaders.UpdateLightFrustum(viewFrustum);
                RenderScene(leftCamera, leftUI);

                //Resets some graphicsdevice options after drawing 2D
                graphics.BlendState = BlendState.Opaque;
                graphics.DepthStencilState = DepthStencilState.Default;
                graphics.RasterizerState = RasterizerState.CullCounterClockwise;
                graphics.SamplerStates[0] = SamplerState.LinearWrap;

                // DRAW RIGHT SCREEN
                renderTarget1 = false;
                graphics.Viewport = rightView;
                shaders.SetViewMatrix(rightCamera.GetViewMatrix());
                shaders.GetEffect().Parameters["cameraPosition"].SetValue(rightCamera.GetPosition());
                viewFrustum.Matrix = rightCamera.GetViewMatrix() * currentProjection;
                shaders.UpdateLightFrustum(viewFrustum);
                RenderScene(rightCamera, rightUI);
            }

            // Clean up
            shaders.CleanUpPointLights(gameTime);

        }
        #endregion

        #region DrawModel
        private void DrawModel(ModelObject drawable)
        {
            drawable.UpdateCollisionBox();
            ModelComponent comp = drawable.ModelComp;
            Model model = comp.Model;
            Texture2D texture = comp.Texture;
            BoundingBox collisionBox = drawable.GetCollisionBox();
            BoundingSphere collisionSphere = drawable.GetCollisionSphere();

            // Copy any parent transforms.
            Matrix[] bones = comp.GetSkinTransforms();

            // Draw collision boxes (used for debugging)
            //DrawCollisionBox.Draw(graphics, collisionBox, leftCamera.GetViewMatrix(), currentProjection);
            //DrawCollisionSphere.Draw(graphics, collisionSphere, leftCamera.GetViewMatrix(), currentProjection, new Color(1.0f, 1.0f, 1.0f));

            // Only draw if model within view frustum
            if (!IsInsideViewFrustrum(drawable))
                return;

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in model.Meshes)
            {
                Matrix worldMatrix = bones[mesh.ParentBone.Index] *
                Matrix.CreateScale(drawable.GetScale()) *
               Matrix.CreateRotationY(drawable.GetAngles().Y) *
               Matrix.CreateRotationX(drawable.GetAngles().X) *
               Matrix.CreateRotationZ(drawable.GetAngles().Z) *
               
                Matrix.CreateTranslation(drawable.GetPositionV3());

                // Apply custom effect to this model.
                // Should use Clone()...
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = shaders.GetEffect();

                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["world"].SetValue(worldMatrix);
                    SetUpEffectsForDrawing(
                        drawable,
                        effect,
                        ref bones,
                        texture);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }

        private void SetUpEffectsForDrawing(ModelObject drawable, Effect effect, ref Matrix[] bones, Texture2D texture)
        {

            if (drawable is TestUnit)
            {
                effect.Parameters["shininess"].SetValue(10);
                effect.Parameters["specularIntensity"].SetValue(1);
            }
            else if (drawable is PlayerCharacter)
            {
                effect.Parameters["shininess"].SetValue(200);
                //effect.Parameters["specularIntensity"].SetValue(0.7f);
                effect.Parameters["specularIntensity"].SetValue(shaders.modelSpecularIntensity);
            }
            else
                shaders.SetEffectToDayEffect();

            if (shadowsEnabled)
                if (depthMap)
                {
                    if (drawable.ModelComp is SkinnedModelComponent)
                    {
                        effect.Parameters["bones"].SetValue(bones);
                        effect.CurrentTechnique = effect.Techniques["AnimatedShadowMap"];
                    }
                    else
                        effect.CurrentTechnique = effect.Techniques["ShadowMap"];

                }
                else
                {
                    if (texture != null)
                    {
                        effect.Parameters["textured"].SetValue(true);
                        effect.Parameters["texture1"].SetValue(texture);
                    }
                    else
                        effect.Parameters["textured"].SetValue(false);

                    if (drawable.ModelComp is SkinnedModelComponent)
                    {
                        effect.Parameters["bones"].SetValue(bones);
                        effect.CurrentTechnique = effect.Techniques["AnimatedShadowed"];
                    }
                    else
                        effect.CurrentTechnique = effect.Techniques["Shadowed"];


                    if (renderTarget1)
                        effect.Parameters["shadowMap"].SetValue(renderTarget);
                    else effect.Parameters["shadowMap"].SetValue(renderTarget2);

                }
            else // no shadows
            {
                if (texture != null)
                {
                    effect.Parameters["textured"].SetValue(true);
                    effect.Parameters["texture1"].SetValue(texture);                                        
                }
                else effect.Parameters["textured"].SetValue(false);
                    
                if (drawable.ModelComp is SkinnedModelComponent)
                {
                    effect.Parameters["bones"].SetValue(bones);
                    effect.CurrentTechnique = effect.Techniques["Animated"];
                }
                else effect.CurrentTechnique = effect.Techniques["Textured"];
            }
        }

        private bool IsInsideViewFrustrum(ModelObject drawable)
        {
            if (depthMap)
            {
                if (!lightFrustum.Intersects(drawable.GetCollisionSphere()))
                {
                    return false;
                }
            }
            else if (!viewFrustum.Intersects(drawable.GetCollisionSphere()))
            {
                return false;
            }

            return true;
        }

        #endregion

        private void DrawParticles(Camera camera)
        {
            particleManager.SetCamera(camera.GetViewMatrix(), currentProjection);
            particleManager.Draw();
        }

        private void DrawOverlays(Camera camera, GameUserInterface ui)
        {

            //DrawTestTextOverlay(camera, ui);

            // Draw all billboards
            Matrix invertY = Matrix.CreateScale(1, -1, 1);
            basicEffect.World = invertY;
            basicEffect.View = Matrix.Identity;
            basicEffect.Projection = currentProjection;

            spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, basicEffect);

            foreach (SpriteObject so in drawListOverlays)
            {
                Texture2D img = so.GetImage();
                Vector3 position = so.GetPositionV3();
                Vector3 viewSpacePosition = Vector3.Transform(position, camera.GetViewMatrix() * invertY);

                spriteBatch.Draw(img, new Vector2(viewSpacePosition.X, viewSpacePosition.Y),
                    null, Color.White, 0, new Vector2(0, img.Height), new Vector2(0.01f, 0.01f), 0, viewSpacePosition.Z);
            }

            spriteBatch.End();


            spriteBatch.Begin();
            if (ui != null)
            {
                ui.Draw(spriteBatch, Vector2.Zero);
            }
            spriteBatch.End();
        }

        private void DrawTestTextOverlay(Camera camera, GameUserInterface ui)
        {

            spriteBatch.Begin();

            //////// TESTING SPRITES ////////
            string text = "= " + GameState.GetInstance().enemies;
            Texture2D hud = AssetBank.GetInstance().GetTexture("UIenemyIcon");

            // Draw text
            spriteBatch.DrawString(font, text, new Vector2(
                50, graphics.Viewport.Height - 39), Color.Black,
                0, Vector2.Zero, 0.5f, 0, 0);
            spriteBatch.DrawString(font, text, new Vector2(
                49, graphics.Viewport.Height - 40), Color.Red,
                0, Vector2.Zero, 0.5f, 0, 0);

            // Draw images
            spriteBatch.Draw(hud, new Vector2(
                10, graphics.Viewport.Height - 45), Color.White);

            spriteBatch.End();

            text = "Enemies = " + GameState.GetInstance().enemies;
            Vector3 textPosition = Constants.GetCenterOfTheWorldV3() +
                new Vector3(10, 2, 0);
            Vector2 textOrigin = font.MeasureString(text) / 2;
            float textSize = 0.025f;

            basicEffect.World = Matrix.CreateConstrainedBillboard(textPosition,
                camera.GetPosition(), Vector3.Down, null, null);
            basicEffect.View = camera.GetViewMatrix();
            basicEffect.Projection = currentProjection;

            // Draw text as billboard
            spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead,
                RasterizerState.CullNone, basicEffect);

            spriteBatch.DrawString(font, text, Vector2.Zero, Color.White,
                0, textOrigin, textSize, 0, 0);
            spriteBatch.End();
        }

        private void CreateShadowMap(Camera camera)
        {
            if (renderTarget1)
                graphics.SetRenderTarget(renderTarget);
            else
                graphics.SetRenderTarget(renderTarget2);
            graphics.Clear(Color.White);
            

            depthMap = true;

            shaders.GetEffect().Parameters["depthBias"].SetValue(Constants.Graphics_ShadowMap_DefaultDepthBias);
            DrawHeightMap(camera);
            shaders.GetEffect().Parameters["depthBias"].SetValue(Constants.Graphics_ShadowMap_ModelDepthBias);
            DrawModels(camera);
            DrawDoodads(camera);

            graphics.SetRenderTarget(null);

            //  FileStream stream = File.OpenWrite("screenshot.jpeg");
            //  renderTarget.SaveAsJpeg(stream, graphics.Viewport.Width, graphics.Viewport.Height);
            //  stream.Close();
        }

        private void RenderScene(Camera camera, GameUserInterface gameUI)
        {
            //graphics.Clear(Color.Pink);

            // mine
            graphics.SamplerStates[1] = SamplerState.PointClamp;

            shaders.GetEffect().Parameters["depthBias"].SetValue(Constants.Graphics_DefaultDepthBias);

            depthMap = false;
            DrawHeightMap(camera);
            DrawModels(camera);
            DrawDoodads(camera);
            DrawParticles(camera);
            DrawOverlays(camera, gameUI);
        }

        #region Draw HeightMap
        private void DrawHeightMap(Camera camera)
        {
            shaders.SetWorldMatrix(Matrix.Identity);
            //shaders.SetTexture(AssetBank.GetInstance().GetTexture("grass4_3"));
            //shaders.SetTexture(AssetBank.GetInstance().GetTexture("MultiTexture1"));
            shaders.SetTexture(World.GetTexture());
            if (shadowsEnabled)
                if (depthMap)
                    shaders.SetTechnique("ShadowMap");
                else
                {
                    shaders.GetEffect().Parameters["textured"].SetValue(true);
                    if (renderTarget1)
                        shaders.GetEffect().Parameters["shadowMap"].SetValue(renderTarget);
                    else shaders.GetEffect().Parameters["shadowMap"].SetValue(renderTarget2);
                    shaders.SetTechnique("Shadowed");
                }
            else shaders.SetTechnique("Textured");
            //shaders.GetEffect().Parameters["specularIntensity"].SetValue(1);

            heightMap.DrawHeightMaps(DrawSubHeightMap, GetDrawCenter(camera));

        }
        #endregion

        void DrawShadowMapToScreen()
        {
            spriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, 128, 128), Color.White);
            spriteBatch.End();
        }

        private Dictionary<Type, List<ModelObject>> drawListDictionary;

        public void addDrawable(ModelObject drawable)
        {
            drawListModels.Add(drawable);
            drawList3D.AddLast((IDrawableWorld)drawable);

            List<ModelObject> specificObjectList;
            bool listExisted = drawListDictionary.TryGetValue(drawable.GetType(), out specificObjectList);
            if (!listExisted)
            {
                specificObjectList = new List<ModelObject>();
                drawListDictionary.Add(drawable.GetType(), specificObjectList);
            }
            specificObjectList.Add(drawable);
        }
        public void addDrawable(SpriteObject drawable)
        {
            drawListOverlays.Add((SpriteObject)drawable);
            drawList3D.AddLast((IDrawableWorld)drawable);
        }

        private void DrawModels(Camera camera)
        {

            Dictionary<Type, List<ModelObject>>.ValueCollection listOfLists = drawListDictionary.Values;

            foreach (List<ModelObject> modelList in listOfLists)
            {
                if (modelList.Count > 0)
                {
                    ModelObject firstModel = modelList[0];

                    if (firstModel.ModelComp is SkinnedModelComponent)
                        for (int i = 0; i < modelList.Count; i++)
                        {
                            DrawModel(modelList[i]);
                        }
                    else
                        DrawModelList(modelList, camera);
                }
            }
        }

        public void DrawModelList(List<ModelObject> modelList, Camera camera)
        {
            if (modelList.Count > 0)
            {
                ModelObject firstModel = modelList[0];

                Array.Resize(ref instanceTransforms, modelList.Count);

                for (int i = 0; i < modelList.Count; i++)
                {
                    Matrix worldMatrix =
                    Matrix.CreateScale(modelList[i].GetScale()) *
                    
                    Matrix.CreateRotationY(modelList[i].GetAngles().Y) *
               Matrix.CreateRotationX(modelList[i].GetAngles().X) *
               Matrix.CreateRotationZ(modelList[i].GetAngles().Z) *
                    Matrix.CreateTranslation(modelList[i].GetPositionV3());

                    instanceTransforms[i] = (worldMatrix);
                }

                Matrix[] instancedModelBones = new Matrix[firstModel.ModelComp.Model.Bones.Count];
                firstModel.ModelComp.Model.CopyAbsoluteBoneTransformsTo(instancedModelBones);
                /*
                DrawModelBatching(
                    firstModel,
                    instancedModelBones,
                    instanceTransforms,
                    camera.GetViewMatrix(),
                    currentProjection);
                */
                DrawModelInstancing(
                    firstModel,
                    instancedModelBones,
                    instanceTransforms,
                    camera.GetViewMatrix(),
                    currentProjection);
                
            }
            else
                if (modelList.Count > 0)
                {
                    DrawModel(modelList[0]);
                }
        }

        Matrix[] instanceTransforms;

        #region InitHeightMap
        public void InitHeightMap(IWorld world)
        {

            // Create HeightMap
            heightMap = new HeightMap(world);

            // !! Do not move vertexbuffer and indexbuffer into heightmap quite yet,
            // !! in the future there will be several such buffers just to draw a
            // !! single heightmap, so it might not be optimal to have these in the heightmap.

            heightMap.HeightMaps.ChangeEveryPoint(InitSubHeightMap);

        }
        #endregion

        #region Init SubHeightMap
        private void InitSubHeightMap(ref SubHeightMap subHeightMap)
        {
            // Create and set VertexBuffer
            subHeightMap.VertexBuffer = new VertexBuffer(
                graphics,
                VertexPositionNormalTexture.VertexDeclaration,
                //subHeightMap.XDimension * subHeightMap.YDimension,
                subHeightMap.GetNumOfVertices(),
                BufferUsage.None);

            subHeightMap.VertexBuffer.SetData<VertexPositionNormalTexture>(subHeightMap.HeightMapData);

            // Create and set IndexBuffer
            subHeightMap.IndexBuffer = new IndexBuffer(
                graphics,
                IndexElementSize.SixteenBits,
                subHeightMap.GetNumOfIndices(),
                BufferUsage.WriteOnly);

            //HeightMapIndices = subHeightMap.CreateMapShortIndices();
            //subHeightMap.IndexBuffer.SetData<short>(HeightMapIndices);

            subHeightMap.IndexBuffer.SetData<short>(subHeightMap.Indexes);

        }
        #endregion

        #region Draw SubHeightMap
        private void DrawSubHeightMap(ref SubHeightMap subHeightMap)
        {
            // THESE MUST BE HERE
            // DONT MOVE THEM
            // ESPECIALLY NOT AWAY FROM DRAW
            graphics.Indices = subHeightMap.IndexBuffer;
            graphics.SetVertexBuffer(subHeightMap.VertexBuffer);
            // OK!

            foreach (EffectPass pass in shaders.GetEffect().CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    0,
                    0,
                    subHeightMap.GetNumOfVertices(),
                    0,
                    subHeightMap.GetNumOfPrimitives());
            }
        }
        #endregion

        #region Draw Doodads
        public void SetDoodads(IWorld world)
        {
            World = world;
        }


        List<List<Matrix>> instanceTransformsDynamic;
        Matrix[] instancedModelBones;

        private void DoodadDataStructuresInit()
        {
            instanceTransformsDynamic = new List<List<Matrix>>(4);
            for (int i = 0; i < Enum.GetValues(typeof(DoodadType)).Length; i++)
            {
                instanceTransformsDynamic.Add(new List<Matrix>(128));
            }
            instancedModelBones = new Matrix[1];
        }

        private void DrawDoodads(Camera camera)
        {

            for (int i = 0; i < Enum.GetValues(typeof(DoodadType)).Length; i++)
            {
                instanceTransformsDynamic[i].Clear();
            }

            instanceTransforms = new Matrix[128];

            // Populate the dynamic instance transform lists
            World.GetData().ChangeValuesViaCenteredBox(
                AddDoodadToInstanceDynamicList,
                GetDrawCenter(camera) + new Vector2(0f, 0f),
                doodadDrawRadius);


            for (int i = 1; i < Enum.GetValues(typeof(DoodadType)).Length; i++)
            {
                StaticDoodads.SetStaticModelObjectToCorrectType(i);

                //instancedModelBones = new Matrix[StaticDoodads.StaticDoodadObject.GetModel().Bones.Count];
                Array.Resize(ref instancedModelBones, StaticDoodads.StaticDoodadObject.ModelComp.Model.Bones.Count);
                StaticDoodads.StaticDoodadObject.ModelComp.Model.CopyAbsoluteBoneTransformsTo(instancedModelBones);

                Array.Resize(ref instanceTransforms, instanceTransformsDynamic[i].Count);
                instanceTransformsDynamic[i].CopyTo(instanceTransforms);

                if (!doodadInstancing)
                {
                    DrawModelBatching(
                        StaticDoodads.StaticDoodadObject,
                        instancedModelBones,
                        instanceTransforms,
                        camera.GetViewMatrix(),
                        currentProjection);
                }
                else
                {                  
                    DrawModelInstancing(
                        StaticDoodads.StaticDoodadObject,
                        instancedModelBones,
                        instanceTransforms,
                        camera.GetViewMatrix(),
                        currentProjection);
                }
            }
        }

        private void AddDoodadToInstanceDynamicList(ref WorldObject Wo, Point P)
        {
            if (Wo.HasDoodad())
            {
                StaticDoodads.SetStaticModelObject(ref Wo.Doodad, P);

                if (doodadFrustrumCheck && !IsInsideViewFrustrum(StaticDoodads.StaticDoodadObject))
                    return;

                Matrix worldMatrix =
                Matrix.CreateScale(StaticDoodads.StaticDoodadObject.GetScale()) *
                
                 Matrix.CreateRotationY(StaticDoodads.StaticDoodadObject.GetAngles().Y) *
               Matrix.CreateRotationX(StaticDoodads.StaticDoodadObject.GetAngles().X) *
               Matrix.CreateRotationZ(StaticDoodads.StaticDoodadObject.GetAngles().Z) *
                Matrix.CreateTranslation(StaticDoodads.StaticDoodadObject.GetPositionV3());

                instanceTransformsDynamic[(int)Wo.Doodad.DoodadType].Add(worldMatrix);
            }
        }


        /// <summary>
        /// Draws several copies of a piece of geometry without using any
        /// special GPU instancing techniques at all. This just does a
        /// regular loop and issues several draw calls one after another.
        /// </summary>
        void DrawModelBatching(ModelObject modelObject, Matrix[] modelBones,
                                   Matrix[] instances, Matrix view, Matrix projection)
        {
            ModelComponent comp = modelObject.ModelComp;
            Model model = comp.Model;
            Texture2D texture = comp.Texture;

            foreach (ModelMesh mesh in model.Meshes)
            {
                //foreach (ModelMeshPart meshPart in mesh.MeshParts)
                //    meshPart.Effect = shaders.GetEffect();

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    graphics.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);

                    graphics.Indices = meshPart.IndexBuffer;

                    meshPart.Effect = shaders.GetEffect();

                    // Set up the rendering effect.
                    Effect effect = meshPart.Effect;

                    SetUpEffectsForDrawing(modelObject,
                        effect, ref modelBones, texture);

                    effect.Parameters["view"].SetValue(view);
                    effect.Parameters["projection"].SetValue(projection);

                    EffectParameter transformParameter = effect.Parameters["world"];

                    // Draw a single instance copy each time around this loop.
                    for (int i = 0; i < instances.Length; i++)
                    {

                        transformParameter.SetValue(modelBones[mesh.ParentBone.Index] * instances[i]);

                        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                        {

                            pass.Apply();

                            graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                                                                    meshPart.NumVertices, meshPart.StartIndex,
                                                                    meshPart.PrimitiveCount);
                        }
                    }
                }
            }
        }

        
        static VertexDeclaration instanceVertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
            new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 1),
            new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 2),
            new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 3)
        );
        

        DynamicVertexBuffer instanceVertexBuffer;

        /// <summary>
        /// Draws several copies of a piece of geometry without using any
        /// special GPU instancing techniques at all. This just does a
        /// regular loop and issues several draw calls one after another.
        /// </summary>
        void DrawModelInstancing(ModelObject modelObject, Matrix[] modelBones,
                                   Matrix[] instances, Matrix view, Matrix projection)
        {
            ModelComponent comp = modelObject.ModelComp;
            Model model = comp.Model;
            Texture2D texture = comp.Texture;

            if (instances.Length == 0)
                return;

            // If we have more instances than room in our vertex buffer, grow it to the neccessary size.
            if ((instanceVertexBuffer == null) ||
                (instances.Length > instanceVertexBuffer.VertexCount))
            {
                if (instanceVertexBuffer != null)
                    instanceVertexBuffer.Dispose();

                instanceVertexBuffer = new DynamicVertexBuffer(graphics, instanceVertexDeclaration,
                                                               instances.Length, BufferUsage.WriteOnly);
            }

            // Transfer the latest instance transform matrices into the instanceVertexBuffer.
            instanceVertexBuffer.SetData(instances, 0, instances.Length, SetDataOptions.Discard);

            foreach (ModelMesh mesh in model.Meshes)
            {
                //foreach (ModelMeshPart meshPart in mesh.MeshParts)
                //    meshPart.Effect = shaders.GetEffect();

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    //graphics.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);
                    //graphics.SetVertexBuffer(meshPart.VertexBuffer);

                    //graphics.Indices = meshPart.IndexBuffer;

                    graphics.SetVertexBuffers(
                        new VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset, 0),
                        new VertexBufferBinding(instanceVertexBuffer, 0, 1)
                    );

                    graphics.Indices = meshPart.IndexBuffer;

                    meshPart.Effect = shaders.GetEffect();

                    // Set up the rendering effect.
                    Effect effect = meshPart.Effect;

                    SetUpEffectsForDrawing(modelObject,
                        effect, ref modelBones, texture);

                    effect.Parameters["world"].SetValue(modelBones[mesh.ParentBone.Index]);
                    effect.Parameters["view"].SetValue(view);
                    effect.Parameters["projection"].SetValue(projection);

                    if (shadowsEnabled)
                    {
                        if (depthMap)
                        {
                            effect.CurrentTechnique = effect.Techniques["ShadowMapHardware"];

                        }
                        else
                        {
                            effect.CurrentTechnique = effect.Techniques["ShadowedHardware"];
                            if (renderTarget1)
                                effect.Parameters["shadowMap"].SetValue(renderTarget);
                            else
                                effect.Parameters["shadowMap"].SetValue(renderTarget2);
                        }
                    }
                    else
                        effect.CurrentTechnique = effect.Techniques["HardwareInstancing"];




                    //EffectParameter transformParameter = effect.Parameters["world"];  

                    //transformParameter.SetValue(modelBones[mesh.ParentBone.Index] * instances[i]);
                    
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {

                        pass.Apply();

                        graphics.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0,
                                                                meshPart.NumVertices, meshPart.StartIndex,
                                                                meshPart.PrimitiveCount, instances.Length);
                    }
                }
            }
        }

        // This Method is used as a Array2D delegate,
        // and thus it's parameters and return type must be similar
        // to any of the delegates declared at the top of Array2D.
        // You are free to add new delegate types in Array2D if you like...
        private void DrawDoodad(ref WorldObject WorldObject, Point P)
        {
            if (WorldObject.Doodad.DoodadType != DoodadType.None)
            {
                StaticDoodads.SetStaticModelObject(ref WorldObject.Doodad, P);

                DrawModel(StaticDoodads.StaticDoodadObject);
            }
        }

        private Vector2 GetDrawCenter(Camera camera)
        {
            Vector2 drawCenter;

            if (camera.Following != null)
                drawCenter = camera.Following.GetPosition();
            else
                drawCenter = Calculations.V3ToV2(camera.GetPosition());

            return drawCenter;
        }

        #endregion

        internal void clearAll()
        {
            shaders.Reset();
            initShaders = false;

            //Clear all lists
            drawList3D.Clear();
            drawListModels.Clear();
            drawListOverlays.Clear();
        }

        internal Matrix GetCurrentProjection()
        {
            return currentProjection;
        }

        private int frames = 0;
        private int milliSecondsSinceStart = 0;
        const int milliSecondsBetweenUpdate = 5000;
        public void CalculateFPS(GameTime gameTime)
        {
            milliSecondsSinceStart += gameTime.ElapsedGameTime.Milliseconds;
            frames++;
            if (milliSecondsSinceStart > milliSecondsBetweenUpdate)
            {
                float framerate = frames / (milliSecondsBetweenUpdate / 1000.0f);
                DebugPrinter.Write("FPS: " + framerate);
                frames = 0;
                milliSecondsSinceStart = 0;
            }
        }
    }
}
