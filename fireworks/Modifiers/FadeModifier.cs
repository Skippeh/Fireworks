using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Fireworks.Modifiers
{
	public class FadeModifier : Modifier
	{
		public override void ModifyParticle(Particle particle, float dt)
		{
			var maxLifeTime = particle.MaxLifeTime;
			var curLifeTime = particle.CurrentLifeTime;
			var alpha = 255f - ((curLifeTime / maxLifeTime) * 255f);

			if (particle.Color.A > 0)
			{
				particle.Color = new Color(particle.Color.R, particle.Color.G, particle.Color.B, (byte) alpha);
			}
		}
	}
}