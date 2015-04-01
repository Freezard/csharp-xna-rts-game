#region using.*
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
        private static Matrix currentView;

        //Graphical constants
        GraphicsDevice graphics;
        private Shaders shaders = Shaders.GetInstance();
        BasicEffect basicEffect;
        float aspectRatio;
        float fov;
        public CullMode CullModeUsed = CullMode.CullCounterClockwiseFace;
        ParticleManager particleSystems = ParticleManager.GetInstance();
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
        private const float frustumMin = 1f; // Will not draw triangles closer than 1 unit
        private const float frustumMax = 50; // Draw distance
        BoundingFrustum viewFrustum;

        //SpriteBatch
        private SpriteBatch spriteBatch;
        private SpriteFont font;

        //Shadows
        private RenderTarget2D renderTarget;
        private bool depthMap = false;
        private bool shadowsEnabled = true;

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
                            MathHelper.ToRadians(fov), aspectRatio/2,
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
            particleSystems.Initialize(graphics);


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

        #region DrawAll
        public void DrawAll(GameTime gameTime)
        {
            //Render can prepare objects for drawing,
            //But it is DrawManager that should draw them.
            
            for (int i = drawListModels.Count - 1; i >= 0; i--)
                {

                    if (drawListModels[i].ToBeRemoved())
                    {
                        drawListModels.RemoveAt(i);
                    }
                }

            //Resets some graphicsdevice options after drawing 2D
            graphics.BlendState = BlendState.Opaque;
            graphics.DepthStencilState = DepthStencilState.Default;
            graphics.RasterizerState = RasterizerState.CullCounterClockwise;

            //Only one camera
            if (!multiPlayer)
            {
                graphics.Viewport = mainView;
                shaders.SetViewMatrix(leftCamera.GetViewMatrix());
                shaders.GetEffect().Parameters["cameraPosition"].SetValue(leftCamera.GetPosition());
                viewFrustum.Matrix = leftCamera.GetViewMatrix() * currentProjection;

                shaders.UpdateLights(gameTime, viewFrustum);
                if (shadowsEnabled)
                    CreateShadowMap();
                RenderScene();
             //   DrawShadowMapToScreen();

                graphics.Textures[0] = null;
                graphics.SamplerStates[0] = SamplerState.LinearWrap;
            }
                //Two cameras
            else
            {
                //Draw left camera
                graphics.Viewport = leftView;
                shaders.SetViewMatrix(leftCamera.GetViewMatrix());
                viewFrustum.Matrix = leftCamera.GetViewMatrix() * currentProjection;
                DrawHeightMap(leftCamera);
                DrawModels(leftCamera);
                DrawDoodads(leftCamera);
                DrawParticles(leftCamera);
                DrawOverlays(leftCamera, leftUI);

                //Resets some graphicsdevice options after drawing 2D
                graphics.BlendState = BlendState.Opaque;
                graphics.DepthStencilState = DepthStencilState.Default;
                graphics.RasterizerState = RasterizerState.CullCounterClockwise;
                graphics.SamplerStates[0] = SamplerState.LinearWrap;

                //Draw right camera
                graphics.Viewport = rightView;
                shaders.SetViewMatrix(rightCamera.GetViewMatrix());
                viewFrustum.Matrix = rightCamera.GetViewMatrix() * currentProjection;
                DrawHeightMap(rightCamera);
                DrawModels(rightCamera);
                DrawDoodads(rightCamera);
                DrawParticles(rightCamera);
                DrawOverlays(rightCamera, rightUI);
            }
        }
        #endregion




        #region DrawModel
        private void DrawModel(ModelObject drawable, Matrix view) // view is never used
        {
            drawable.UpdateCollisionBox();

            Model model = drawable.GetModel();
            Texture2D texture = drawable.GetTexture();
            BoundingBox collisionBox = drawable.GetCollisionBox();
            BoundingSphere collisionSphere = drawable.GetCollisionSphere();

            // Copy any parent transforms.
            Matrix[] bones = drawable.GetSkinTransforms();
            
            // Draw collision boxes (used for debugging)
            //DrawCollisionBox.Draw(graphics, collisionBox, leftCamera.GetViewMatrix(), currentProjection);
            //DrawCollisionSphere.Draw(graphics, collisionSphere, leftCamera.GetViewMatrix(), currentProjection, new Color(1.0f, 1.0f, 1.0f));

            // Only draw if model within view frustum
            if (!viewFrustum.Intersects(collisionSphere))
            {
                return;
            }
            
            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in model.Meshes)
            {
                Matrix worldMatrix = bones[mesh.ParentBone.Index] *
                Matrix.CreateScale(drawable.GetScale()) *
                Matrix.CreateRotationY(drawable.GetFacingAngleOnXZPlane()) *
                Matrix.CreateTranslation(drawable.GetPositionV3());
                
                // Apply custom effect to this model.
                // Should use Clone()...
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = shaders.GetEffect();

                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["world"].SetValue(worldMatrix);
                    if (drawable is TestUnit)
                    {
                        effect.Parameters["shininess"].SetValue(10);
                        effect.Parameters["specularIntensity"].SetValue(1);
                    }
                    else if (drawable is PlayerCharacter)
                    {
                        effect.Parameters["shininess"].SetValue(200);
                        effect.Parameters["specularIntensity"].SetValue(0.7f);
                    }
                    else effect.Parameters["specularIntensity"].SetValue(0);

                    if (shadowsEnabled)
                        if (depthMap)
                        {
                            if (drawable is SkinnedModelObject)
                            {
                                effect.Parameters["bones"].SetValue(bones);
                                effect.CurrentTechnique = effect.Techniques["AnimatedShadowMap"];
                            }
                            else effect.CurrentTechnique = effect.Techniques["ShadowMap"];
                        }
                        else
                        {
                            if (texture != null)
                            {
                                effect.Parameters["textured"].SetValue(true);
                                effect.Parameters["texture1"].SetValue(texture);
                            }
                            else effect.Parameters["textured"].SetValue(false);

                            if (drawable is SkinnedModelObject)
                            {
                                effect.Parameters["bones"].SetValue(bones);
                                effect.CurrentTechnique = effect.Techniques["AnimatedShadowed"];
                            }
                            else effect.CurrentTechnique = effect.Techniques["Shadowed"];
                            effect.Parameters["shadowMap"].SetValue(renderTarget);
                        }
                    else
                    {
                        if (drawable is SkinnedModelObject)
                        {
                            effect.Parameters["texture1"].SetValue(texture);
                            effect.Parameters["bones"].SetValue(bones);
                            effect.CurrentTechnique = effect.Techniques["Animated"];
                        }
                        else if (texture != null)
                        {
                            effect.Parameters["texture1"].SetValue(texture);
                            effect.CurrentTechnique = effect.Techniques["Textured"];
                        }
                        else
                        {
                            effect.CurrentTechnique = effect.Techniques["DiffuseLighting"];
                        }
                    }
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }
        #endregion

        private void DrawParticles(Camera camera)
        {
            particleSystems.SetCamera(camera.GetViewMatrix(), currentProjection);
            particleSystems.Draw();
        }

        private void DrawOverlays(Camera camera, GameUserInterface ui)
        {
            
            /*spriteBatch.Begin();
            
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
            */
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

        private void CreateShadowMap()
        {
            graphics.SetRenderTarget(renderTarget);
            graphics.Clear(Color.White);

            depthMap = true;
            DrawHeightMap(leftCamera);
            DrawModels(leftCamera);
            DrawDoodads(leftCamera);

            graphics.SetRenderTarget(null);

          //  FileStream stream = File.OpenWrite("screenshot.jpeg");
          //  renderTarget.SaveAsJpeg(stream, graphics.Viewport.Width, graphics.Viewport.Height);
          //  stream.Close();
        }

        private void RenderScene()
        {
            graphics.Clear(Color.Pink);
            graphics.SamplerStates[1] = SamplerState.PointClamp;

            depthMap = false;
            DrawHeightMap(leftCamera);
            DrawModels(leftCamera);
            DrawDoodads(leftCamera);
            DrawParticles(leftCamera);
            DrawOverlays(leftCamera, leftUI);
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
                    shaders.GetEffect().Parameters["shadowMap"].SetValue(renderTarget);
                    shaders.SetTechnique("Shadowed");
                }
            else shaders.SetTechnique("Textured");
            shaders.GetEffect().Parameters["specularIntensity"].SetValue(0);
            //shaders.EarthRotation(); // Rotates sunlight

            heightMap.DrawHeightMaps(DrawSubHeightMap, GetDrawCenter(camera));
        }
        #endregion

        void DrawShadowMapToScreen()
        {
            spriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, 128, 128), Color.White);
            spriteBatch.End();
        }

        public void addDrawable(ModelObject drawable)
        {
            drawListModels.Add(drawable);
            drawList3D.AddLast((IDrawableWorld)drawable);
        }
        public void addDrawable(SpriteObject drawable)
        {
            drawListOverlays.Add((SpriteObject)drawable);
            drawList3D.AddLast((IDrawableWorld)drawable);
        }
        private void DrawModels(Camera camera)
        {
            Matrix view = camera.GetViewMatrix();
            foreach (ModelObject drawable in drawListModels)
            {
                DrawModel(drawable, view);
            }

        }

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

        private void DrawDoodads(Camera camera)
        {
            currentView = camera.GetViewMatrix();

            World.GetData().ChangeValuesViaCenteredBox(
                DrawDoodad,
                GetDrawCenter(camera) + new Vector2(0, -5.0f),
                25.0f);
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

                DrawModel(StaticDoodads.StaticDoodadObject, currentView);
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
    }
}
