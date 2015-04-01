using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.Utilities.Graphics
{
    class Shaders
    {
        private Effect effect;
        private Vector3 sunlight;
        private Matrix lightViewProjectionMatrix;
        private PointLight[] pointLights;
        private int currentLights = 0;

        private static Shaders instance;

        public static Shaders GetInstance()
        {
            if (instance == null)
            {
                instance = new Shaders();
            }
            return instance;
        }

        public void Initialize(Matrix worldMatrix, Matrix viewMatrix, Matrix projMatrix)
        {
            effect = AssetBank.GetInstance().GetShader("Shader");

            pointLights = new PointLight[Constants.MaxLights];

            for (int i = 0; i < Constants.MaxLights; i++)
                pointLights[i] = new PointLight(i);

            effect.Parameters["world"].SetValue(worldMatrix);
            effect.Parameters["view"].SetValue(viewMatrix);
            effect.Parameters["projection"].SetValue(projMatrix);
            sunlight = new Vector3(0.4f, 0.4f, 0.6f);
            sunlight.Normalize();
            effect.Parameters["lightDirection"].SetValue(sunlight);
            effect.Parameters["ambient"].SetValue(0.1f);
            effect.Parameters["enableLighting"].SetValue(true);
            effect.Parameters["currentLights"].SetValue(currentLights);    
        }

        public void Reset()
        {
            sunlight = new Vector3(0.4f, 0.4f, 0.6f);
            sunlight.Normalize();
            effect.Parameters["lightDirection"].SetValue(sunlight);
            effect.Parameters["ambient"].SetValue(0.1f);
            effect.Parameters["currentLights"].SetValue(currentLights);
        }

        public void AddPointLight(Vector3 position, Vector4 color, float power, float range, float duration)
        {
            for (int i = 0; i < Constants.MaxLights; i++)
                if (!pointLights[i].IsActive())
                {
                    pointLights[i].SetAttributes(position, color, power, range, duration);
                    currentLights++;
                    return;
                }
        }

        public void UpdateLights(GameTime gameTime, BoundingFrustum viewFrustum)
        {
            UpdateDirectionalLight(viewFrustum);
            UpdatePointLights(gameTime);
        }

        private void UpdateDirectionalLight(BoundingFrustum viewFrustum)
        {
            // Matrix with that will rotate in points the direction of the light
            Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero,
                                                       sunlight,
                                                       Vector3.Up);

            // Get the corners of the frustum           
            Vector3[] frustumCorners = viewFrustum.GetCorners();

            // Transform the positions of the corners into the direction of the light
            for (int i = 0; i < frustumCorners.Length; i++)
            {
                frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);
            }

            // Find the smallest box around the points
            BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCorners);

            Vector3 boxSize = lightBox.Max - lightBox.Min;
            Vector3 halfBoxSize = boxSize * 0.5f;

            // The position of the light should be in the center of the back
            // pannel of the box. 
            Vector3 lightPosition = lightBox.Min + halfBoxSize;
            lightPosition.Z = lightBox.Min.Z;

            // We need the position back in world coordinates so we transform 
            // the light position by the inverse of the lights rotation
            lightPosition = Vector3.Transform(lightPosition,
                                              Matrix.Invert(lightRotation));

            // Create the view matrix for the light
            Matrix lightView = Matrix.CreateLookAt(lightPosition,
                                                   lightPosition - sunlight,
                                                   Vector3.Up);

            // Create the projection matrix for the light
            // The projection is orthographic since we are using a directional light
            Matrix lightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y,
                                                               -boxSize.Z, boxSize.Z);

            lightViewProjectionMatrix = lightView * lightProjection;

            effect.Parameters["lightViewProjection"].SetValue(lightViewProjectionMatrix);
        }

        public void UpdatePointLights(GameTime gameTime)
        {
            for (int i = 0; i < Constants.MaxLights; i++)
                if (pointLights[i].IsActive())
                    if (!pointLights[i].Update(gameTime))
                    {
                        effect.Parameters["lights"].Elements[i].StructureMembers["position"].SetValue(pointLights[i].GetPosition());
                        effect.Parameters["lights"].Elements[i].StructureMembers["color"].SetValue(pointLights[i].GetColor());
                        effect.Parameters["lights"].Elements[i].StructureMembers["power"].SetValue(pointLights[i].GetPower());
                        effect.Parameters["lights"].Elements[i].StructureMembers["range"].SetValue(pointLights[i].GetRange());
                    }
                    else
                    {
                        effect.Parameters["lights"].Elements[i].StructureMembers["position"].SetValue(new Vector3(0, 0, 0));
                        effect.Parameters["lights"].Elements[i].StructureMembers["color"].SetValue(new Vector4(0, 0, 0, 0));
                        effect.Parameters["lights"].Elements[i].StructureMembers["power"].SetValue(0);
                        effect.Parameters["lights"].Elements[i].StructureMembers["range"].SetValue(0);
                        currentLights--;
                    }
            effect.Parameters["currentLights"].SetValue(currentLights);
        }

        public void SetTechnique(string technique)
        {
            effect.CurrentTechnique = effect.Techniques[technique];
        }

        public void SetTexture(Texture2D texture)
        {
            effect.Parameters["texture1"].SetValue(texture);
        }

        public void SetViewMatrix(Matrix viewMatrix)
        {
            effect.Parameters["view"].SetValue(viewMatrix);
        }

        public void SetWorldMatrix(Matrix worldMatrix)
        {
            effect.Parameters["world"].SetValue(worldMatrix);
        }

        public void EarthRotation()
        {
            sunlight = Vector3.Transform(sunlight, Matrix.CreateRotationY(0.003f));
            effect.Parameters["lightDirection"].SetValue(sunlight);
        }

        public void Sunrise()
        {
            sunlight = Vector3.Transform(sunlight, Matrix.CreateRotationX(0.005f));
            effect.Parameters["lightDirection"].SetValue(sunlight);
        }

        public Effect GetEffect()
        {
            return effect;
        }
    }
}
