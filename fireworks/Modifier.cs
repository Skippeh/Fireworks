using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fireworks
{
	public abstract class Modifier
	{
		protected Modifier()
		{
			
		}

		public abstract void ModifyParticle(Particle particle, float dt);
	}
}