#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace RTSgame.Utilities.Graphics.ParticleSystem
{
    class Special : ParticleSystem
    {
        public Special(GraphicsDevice graphics)
            : base(graphics)
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "wwsmoke";

            settings.MaxParticles = 300;

            settings.Duration = TimeSpan.FromSeconds(1);
            settings.DurationRandomness = 0.5f;
            settings.EmitterVelocitySensitivity = 0;
            settings.BaseEmitterVelocitySensitivity = 0;

            settings.MinHorizontalVelocity = 0.1f;
            settings.BaseMinHorizontalVelocity = 0.1f;
            settings.MaxHorizontalVelocity = 2.0f;
            settings.BaseMaxHorizontalVelocity = 2.0f;

            settings.MinVerticalVelocity = 0.1f;
            settings.BaseMinVerticalVelocity = 0.1f;
            settings.MaxVerticalVelocity = 7.0f;
            settings.BaseMaxVerticalVelocity = 7.0f;

            settings.MinColor = new Color(0.5f, 0, 0);
            settings.MaxColor = new Color(1, 1, 1);

            settings.MinRotateSpeed = -10;
            settings.MaxRotateSpeed = 10;

            settings.MinStartSize = 0.1f;
            settings.BaseMinStartSize = 0.1f;
            settings.MaxStartSize = 0.2f;
            settings.BaseMaxStartSize = 0.2f;

            settings.MinEndSize = 0.2f;
            settings.BaseMinEndSize = 0.2f;
            settings.MaxEndSize = 0.4f;
            settings.BaseMaxEndSize = 0.4f;
        }
    }
}
