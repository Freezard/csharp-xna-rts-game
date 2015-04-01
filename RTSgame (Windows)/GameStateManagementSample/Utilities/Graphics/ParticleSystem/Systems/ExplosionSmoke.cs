#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace RTSgame.Utilities.Graphics.ParticleSystem
{
    class ExplosionSmoke : ParticleSystem
    {
        public ExplosionSmoke(GraphicsDevice graphics)
            : base(graphics)
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "PFXsmoke";

            settings.MaxParticles = 2000;

            settings.Duration = TimeSpan.FromSeconds(2.5);
            settings.EmitterVelocitySensitivity = 0;
            settings.BaseEmitterVelocitySensitivity = 0;

            settings.MinHorizontalVelocity = 0;
            settings.BaseMinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 1.1f;
            settings.BaseMaxHorizontalVelocity = 1.1f;
            
            settings.MinVerticalVelocity = -1.1f;
            settings.BaseMinVerticalVelocity = -1.1f;
            settings.MaxVerticalVelocity = 1.1f;
            settings.BaseMaxVerticalVelocity = 1.1f;

            settings.Gravity = new Vector3(0, -0.1f, 0);

            settings.EndVelocity = 0;

            settings.MinColor = Color.LightGray;
            settings.MaxColor = Color.White;

            settings.MinRotateSpeed = -2;
            settings.MaxRotateSpeed = 2;

            settings.MinStartSize = 0.1f;
            settings.BaseMinStartSize = 0.1f;
            settings.MaxStartSize = 0.2f;
            settings.BaseMaxStartSize = 0.2f;

            settings.MinEndSize = 0.8f;
            settings.BaseMinEndSize = 0.8f;
            settings.MaxEndSize = 0.16f;
            settings.BaseMaxEndSize = 0.16f;
        }
    }
}
