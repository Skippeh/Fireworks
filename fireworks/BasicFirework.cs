using Fireworks.Modifiers;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace Fireworks
{
	internal class BasicFirework : Firework
	{
		private readonly Sound whistleSound;
		private bool exploded;

		public BasicFirework(Vector2f position, Vector2f initialVelocity, Vector2f gravity)
			: base(position, initialVelocity, gravity, 0.5f, 0.01f)
		{
			whistleSound = SoundManager.Play("whistlingrocket_" + Helper.GetRandomNumber(1, 4), new Vector3f(Position.X, 0, Position.Y));
			Modifiers.Add(new FadeModifier());
		}

		public override void Update(float dt)
		{
			base.Update(dt);

			if (!exploded)
			{
				if (Velocity.Y > 50)
				{
					Enabled = false;
					Explode();
				}
			}

			whistleSound.Position = new Vector3f(Position.X, 0, Position.Y);
		}

		protected override void Explode()
		{
			exploded = true;
			for (int i = 0; i < 16; ++i)
			{
				AddParticle(Position, Helper.GetVector2FromAngle(Helper.GetRandomNumber(0, 360)) * Helper.GetRandomNumber(0, 200),
				            Gravity * (Helper.GetRandomNumber(1, 3) / 10f), Helper.GetRandomNumber(4, 8), Helper.GetRandomColor());
			}

			SoundManager.Play("explode_" + Helper.GetRandomNumber(1, 2), new Vector3f(Position.X, 0, Position.Y));
		}

		public override Vector2f GetInitialParticleVelocity()
		{
			return new Vector2f(Helper.GetRandomNumber(-50, 50), 1);
		}
	}
}