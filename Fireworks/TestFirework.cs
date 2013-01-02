using System;
using Fireworks.Modifiers;
using SFML.Graphics;
using SFML.Window;

namespace Fireworks
{
	class TestFirework : Firework
	{
		private bool exploded = false;

		public TestFirework(Vector2f position, Random random)
			: base(position, Helper.GetRandomVector2f(-50, -500, 50, -450, random), new Vector2f(0, 300), 0.8f, 0.01f)
		{
			Modifiers.Add(new FadeModifier());
		}

		public override void Update(float dt)
		{
			base.Update(dt);

			if (Velocity.Y > 0 && !exploded)
			{
				Explode();
				Enabled = false;
				exploded = true;
			}
		}

		public override Vector2f GetInitialParticleVelocity()
		{
			return Helper.GetRandomVector2f(-50, 0, 50, 0);
		}

		protected override void Explode()
		{
			Enabled = false;

			for (int i = 0; i < 20; ++i)
			{
				var p = AddParticle(Position, Helper.GetRandomVector2f(-50, -5, 50, 5), new Vector2f(0, 0), 4, new Color(0, 0, 0)); // Color will be modified by modifier.
				p.Modifiers.Clear(); // Clear any modifiers the parent emitter might've had.
				p.Modifiers.Add(new ColorFadeModifier(Color.White, Color.Red));

				p.Dead += (sender, args) =>
					          {
								  for (int j = 0; j < 50; ++j)
								  {
									  var p2 = AddParticle(p.Position, Helper.GetRandomVector2f(-10, -10, 10, 10), new Vector2f(0, 5), 8f, Color.Blue);
									  p2.Modifiers.Add(new FadeModifier());
								  }
					          };
			}
		}
	}
}