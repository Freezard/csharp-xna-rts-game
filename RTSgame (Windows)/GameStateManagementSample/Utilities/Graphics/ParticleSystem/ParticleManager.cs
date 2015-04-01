using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.Utilities;

namespace RTSgame.Utilities.Graphics.ParticleSystem
{
    class ParticleManager
    {
        private ParticleSystem smoke1;
        private ParticleSystem smoke2;
        public ParticleSystem explosionSmoke;
        public ParticleSystem explosion;
        public ParticleSystem smokeTrail;
        public ParticleSystem smokeWW;
        private List<Vector3> smoke1Positions;
        private List<Vector3> smoke2Positions;
        private List<ParticleSystem> particleSystems;

        private static ParticleManager instance;

        private ParticleManager()
        {
        }

        public void Initialize(GraphicsDevice graphics)
        {
            smoke1Positions = new List<Vector3>();
            smoke2Positions = new List<Vector3>();
            particleSystems = new List<ParticleSystem>();

            smoke1 = new Smoke1(graphics);
            smoke2 = new Smoke2(graphics);
            explosionSmoke = new ExplosionSmoke(graphics);
            explosion = new Explosion(graphics);
            smokeTrail = new SmokeTrail(graphics);
            smokeWW = new Special(graphics);

            particleSystems.Add(smoke1);
            particleSystems.Add(smoke2);
            particleSystems.Add(explosionSmoke);
            particleSystems.Add(explosion);
            particleSystems.Add(smokeTrail);
            particleSystems.Add(smokeWW);
        }

        public static ParticleManager GetInstance()
        {
            if (instance == null)
                instance = new ParticleManager();
            return instance;
        }

        public void SetCamera(Matrix view, Matrix projection)
        {
            foreach (ParticleSystem sys in particleSystems)
                sys.SetCamera(view, projection);
        }

        public void AddSmoke1(Vector3 position)
        {
            smoke1Positions.Add(position);
            smoke1.settings.MaxParticles = smoke1.settings.Particles * smoke1Positions.Count;
            smoke1.UpdateMaxParticles();
        }

        public void AddSmoke2(Vector3 position)
        {
            smoke2Positions.Add(position);
            smoke2.settings.MaxParticles = smoke2.settings.Particles * smoke2Positions.Count;
            smoke2.UpdateMaxParticles();
        }

        public void RemoveSmoke1(Vector3 position)
        {
            smoke1.settings.MaxParticles -= smoke1.settings.Particles;
            smoke1Positions.Remove(position);
        }

        public void RemoveSmoke2(Vector3 position)
        {
            smoke2.settings.MaxParticles -= smoke2.settings.Particles;
            smoke2Positions.Remove(position);
        }

        public void UpdateSmoke1()
        {     
            for (int i = 0; i < smoke1Positions.Count; i++)
                smoke1.AddParticle(smoke1Positions[i], Vector3.Zero);
        }

        public void UpdateSmoke2()
        {        
            for (int i = 0; i < smoke2Positions.Count; i++)
                smoke2.AddParticle(smoke2Positions[i], Vector3.Zero);
        }

        public void AddExplosion(Vector3 position, Vector3 velocity, float scale)
        {
            explosion.ChangeSize(scale);
            explosionSmoke.ChangeSize(scale);

            for (int i = 0; i < 20; i++)
                explosion.AddParticle(position + new Vector3(0, 0.5f, 0), velocity);
            for (int i = 0; i < 5; i++)
                explosionSmoke.AddParticle(position + new Vector3(0, 0.5f, 0), velocity);
            
            Shaders.GetInstance().AddPointLight(position, new Vector4(1, 1, 0, 0), 2.5f, 4, 0.35f);
        }

        public void AddSpecial(Vector3 position, Vector3 velocity, float scale)
        {
            smokeWW.ChangeSize(scale);

            for (int i = 0; i < 30; i++)
                smokeWW.AddParticle(position, velocity);

            Shaders.GetInstance().AddPointLight(position, new Vector4(1, 0, 0, 0), 7f, 4, 0.8f);
            
        }

        public void AddLight(Vector3 position, Vector4 color, float power, float range, float duration, bool isPrioritized = false)
        {
            Shaders.GetInstance().AddPointLight(position, color, power, range, duration, isPrioritized);
        }

        public void Update(GameTime gameTime)
        {
            UpdateSmoke1();

            foreach (ParticleSystem sys in particleSystems)
                sys.Update(gameTime);
        }

        public void Draw()
        {           
            smoke1.Draw();
            explosionSmoke.Draw();
            smokeTrail.Draw();
            explosion.Draw();
            smokeWW.Draw();
        }
    }
}
