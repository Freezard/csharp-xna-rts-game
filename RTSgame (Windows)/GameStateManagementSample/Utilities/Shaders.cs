using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RTSgame.Utilities
{
    class Shaders
    {
        Effect effect;
        Vector3 sunlight;

        public Shaders()
        {
            effect = AssetBank.GetInstance().GetShader("Shader");
        }

        public void initialize(Matrix worldMatrix, Matrix viewMatrix, Matrix projMatrix)
        {
            effect.Parameters["world"].SetValue(worldMatrix);
            effect.Parameters["view"].SetValue(viewMatrix);
            effect.Parameters["projection"].SetValue(projMatrix);
            sunlight = new Vector3(-1.0f, 0.4f, 1.0f);
            sunlight.Normalize();
            effect.Parameters["lightDirection"].SetValue(sunlight);
            effect.Parameters["ambient"].SetValue(0.0f);
            effect.Parameters["enableLighting"].SetValue(true);    
        }

        public void setTechnique(string technique)
        {
            effect.CurrentTechnique = effect.Techniques[technique];  
        }

        public void setTexture(Texture2D texture)
        {
            effect.Parameters["texture1"].SetValue(texture);
        }

        public void setViewMatrix(Matrix viewMatrix)
        {
            effect.Parameters["view"].SetValue(viewMatrix);
        }

        public void setWorldMatrix(Matrix worldMatrix)
        {
            effect.Parameters["world"].SetValue(worldMatrix);
        }

        public void earthRotation()
        {
            sunlight = Vector3.Transform(sunlight, Matrix.CreateRotationY(0.007f));
            effect.Parameters["lightDirection"].SetValue(sunlight);
        }

        public void sunrise()
        {
            sunlight = Vector3.Transform(sunlight, Matrix.CreateRotationX(0.005f));
            effect.Parameters["lightDirection"].SetValue(sunlight);
        }

        public Effect getEffect()
        {
            return effect;
        }

        //// BasicEffect ////

        static public BasicEffect GetHeightMapEffect(GraphicsDevice graphics)
        {
            BasicEffect HeightMapEffect = new BasicEffect(graphics);

            HeightMapEffect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);
            HeightMapEffect.VertexColorEnabled = false;
            HeightMapEffect.LightingEnabled = true;
            if (HeightMapEffect.LightingEnabled)
            {

                HeightMapEffect.DirectionalLight0.Enabled = true; // enable each light individually
                if (HeightMapEffect.DirectionalLight0.Enabled)
                {
                    // x direction
                    HeightMapEffect.DirectionalLight0.DiffuseColor = new Vector3(0.8f, 0.8f, 0.6f);
                    HeightMapEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(0.3f, -1.0f, 0.5f));
                    // points from the light to the origin of the scene
                    HeightMapEffect.DirectionalLight0.SpecularColor = new Vector3(0.0f, 0.0f, 0.0f);
                }
            }

            return HeightMapEffect;
        }

        public static void SetTexture(ref BasicEffect effect, Texture2D texture)
        {
            effect.TextureEnabled = true;
            effect.Texture = texture;
        }
    }
}
