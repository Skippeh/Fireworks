using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace Fireworks
{
	public class Emitter : Particle
	{
		public bool Enabled { get; set; }

		public float ParticleLifeTime { get; set; }

		private List<Particle> particles;

		private float maxWaitTime;
		private float currentWaitTime;

		public Emitter(Vector2f position, Vector2f initialVelocity, Vector2f gravity, float waitTime, float particleLifeTime) : base(position, initialVelocity, gravity, null, 0, Color.White)
		{
			particles = new List<Particle>();

			maxWaitTime = waitTime;
			currentWaitTime = 0;
			ParticleLifeTime = particleLifeTime;

			Enabled = true;
		}

		public override void Update(float dt)
		{
			base.Update(dt);

			if (Enabled)
			{
				currentWaitTime += dt;

				if (currentWaitTime >= maxWaitTime)
				{
					currentWaitTime = 0;

					AddParticle(Position, GetInitialParticleVelocity(), Gravity, ParticleLifeTime, new Color(128, 128, 128));
				}
			}

			particles.ForEach(p => p.Update(dt));
		}

		public override void Draw(RenderWindow window, float dt)
		{
			particles.ForEach(p => p.Draw(window, dt));
		}

		public void AddParticle(Particle particle)
		{
			if(particle == this) throw new ArgumentException("Can't add an emitter to itself.");

			particles.Add(particle);
		}

		public virtual Particle AddParticle(Vector2f position, Vector2f initialVelocity, Vector2f gravity, float lifeTime, Color color)
		{
			var particle = new Particle(position, initialVelocity, gravity, this, lifeTime, color);
			AddParticle(particle);

			return particle;
		}

		public void RemoveParticle(Particle particle)
		{
			particles.Remove(particle);
		}

		public virtual Vector2f GetInitialParticleVelocity()
		{
			return new Vector2f(0, 0);
		}
	}
}