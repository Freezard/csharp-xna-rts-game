#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace RTSgame.Utilities.Graphics.ParticleSystem
{
    class Explosion : ParticleSystem
    {
        public Explosion(GraphicsDevice graphics)
            : base(graphics)
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "PFXexplosion";

            settings.MaxParticles = 6000;

            settings.Duration = TimeSpan.FromSeconds(2);
            settings.DurationRandomness = 1;
            settings.EmitterVelocitySensitivity = 0.2f;
            settings.BaseEmitterVelocitySensitivity = 0.2f;

            settings.MinHorizontalVelocity = 0.4f;
            settings.BaseMinHorizontalVelocity = 0.4f;
            settings.MaxHorizontalVelocity = 0.8f;
            settings.BaseMaxHorizontalVelocity = 0.8f;

            settings.MinVerticalVelocity = -0.8f;
            settings.BaseMinVerticalVelocity = -0.8f;
            settings.MaxVerticalVelocity = 0.8f;
            settings.BaseMaxVerticalVelocity = 0.8f;

            settings.EndVelocity = 0;

            settings.MinColor = Color.DarkGray;
            settings.MaxColor = Color.Gray;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 0.1f;
            settings.BaseMinStartSize = 0.1f;
            settings.MaxStartSize = 0.2f;
            settings.BaseMaxStartSize = 0.2f;

            settings.MinEndSize = 0.4f;
            settings.BaseMinEndSize = 0.4f;
            settings.MaxEndSize = 0.8f;
            settings.BaseMaxEndSize = 0.8f;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
