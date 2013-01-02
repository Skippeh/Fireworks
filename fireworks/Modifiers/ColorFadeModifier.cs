using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Fireworks.Modifiers
{
	public class ColorFadeModifier : Modifier
	{
		public Color StartColor { get; set; }
		public Color EndColor { get; set; }

		public ColorFadeModifier(Color startColor, Color endColor)
		{
			StartColor = startColor;
			EndColor = endColor;
		}

		public override void ModifyParticle(Particle particle, float dt)
		{
			var diffR = (float) EndColor.R - (float) StartColor.R;
			var diffG = (float) EndColor.G - (float) StartColor.G;
			var diffB = (float) EndColor.B - (float) StartColor.B;
			var lifeTime = 1f - (particle.CurrentLifeTime / particle.MaxLifeTime);

			var r = StartColor.R + (diffR * lifeTime);
			var g = StartColor.G + (diffG * lifeTime);
			var b = StartColor.B + (diffB * lifeTime);

			var color = new Color((byte) r,
			                      (byte) g,
			                      (byte) b,
			                      particle.Color.A);

			particle.Color = color;
		}
	}
}