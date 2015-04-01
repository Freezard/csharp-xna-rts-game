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
    class Smoke2 : ParticleSystem
    {
        public Smoke2(GraphicsDevice graphics)
            : base(graphics)
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "PFXsmoke";

            settings.MaxParticles = 80;
            settings.Particles = 80;

            settings.Duration = TimeSpan.FromSeconds(1.5f);

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 1.1f;

            settings.MinVerticalVelocity = 1.5f;
            settings.MaxVerticalVelocity = 1.8f;

            settings.Gravity = new Vector3(-0.4f, -0.1f, 0);

            settings.EndVelocity = 0.015f;

            settings.MinRotateSpeed = 0;
            settings.MaxRotateSpeed = 0;

            settings.MinStartSize = 0.01f;
            settings.MaxStartSize = 0.02f;

            settings.MinEndSize = 0.1f;
            settings.MaxEndSize = 0.2f;
        }
    }
}
