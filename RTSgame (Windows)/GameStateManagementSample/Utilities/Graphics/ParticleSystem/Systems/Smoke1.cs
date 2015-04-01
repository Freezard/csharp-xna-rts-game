#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace RTSgame.Utilities.Graphics.ParticleSystem
{
    /// <summary>
    /// 
    /// </summary>
    class Smoke1 : ParticleSystem
    {
        public Smoke1(GraphicsDevice graphics)
            : base(graphics)
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "PFXsmoke";

            settings.MaxParticles = 420;
            settings.Particles = 420;

            settings.Duration = TimeSpan.FromSeconds(7);

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0.1f;

            settings.MinVerticalVelocity = 0.5f;
            settings.MaxVerticalVelocity = 0.8f;

            // Create a wind effect by tilting the gravity vector sideways.
            settings.Gravity = new Vector3(-0.4f, -0.1f, 0);

            settings.EndVelocity = 0.015f;

            settings.MinRotateSpeed = -0.2f;
            settings.MaxRotateSpeed = 0.2f;

            settings.MinStartSize = 0.01f;
            settings.MaxStartSize = 0.02f;

            settings.MinEndSize = 0.0175f;
            settings.MaxEndSize = 0.2f;
        }
    }
}
