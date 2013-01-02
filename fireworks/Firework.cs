using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace Fireworks
{
	public abstract class Firework : Emitter
	{
		protected bool exploded = false;

		protected Firework(Vector2f position, Vector2f initialVelocity, Vector2f gravity, float particleLifeTime)
			: base(position, initialVelocity, gravity, 0.01f, particleLifeTime)
		{
			Position = position;
		}

		public override Particle AddParticle(Vector2f position, Vector2f initialVelocity, Vector2f gravity, float lifeTime, Color color)
		{
			var p = base.AddParticle(position, initialVelocity, gravity, lifeTime, color);
			p.Modifiers.AddRange(Modifiers);

			return p;
		}

		protected abstract void Explode();
	}
}