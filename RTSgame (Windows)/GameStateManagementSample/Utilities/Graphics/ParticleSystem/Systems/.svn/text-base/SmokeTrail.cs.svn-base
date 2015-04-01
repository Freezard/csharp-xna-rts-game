#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace RTSgame.Utilities.Graphics.ParticleSystem
{
    class SmokeTrail : ParticleSystem
    {
        public SmokeTrail(GraphicsDevice graphics)
            : base(graphics)
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "rr1";

            settings.MaxParticles = 2000;

            settings.Duration = TimeSpan.FromSeconds(0.5f);
            settings.DurationRandomness = 0;
            settings.EmitterVelocitySensitivity = 0;
            settings.BaseEmitterVelocitySensitivity = 0;

            settings.MinHorizontalVelocity = 0;
            settings.BaseMinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0.5f;
            settings.BaseMaxHorizontalVelocity = 0.5f;

            settings.MinVerticalVelocity = -0.1f;
            settings.BaseMinVerticalVelocity = -0.1f;
            settings.MaxVerticalVelocity = 0.1f;
            settings.BaseMaxVerticalVelocity = 0.1f;

            settings.MinColor = new Color(0.5f, 0, 0);
            settings.MaxColor = new Color(1, 1, 1);

            settings.MinStartSize = 0.01f;
            settings.BaseMinStartSize = 0.01f;
            settings.MaxStartSize = 0.01f;
            settings.BaseMaxStartSize = 0.01f;

            settings.MinEndSize = 0.1f;
            settings.BaseMinEndSize = 0.1f;
            settings.MaxEndSize = 0.2f;
            settings.BaseMaxEndSize = 0.2f;
        }
    }
}
