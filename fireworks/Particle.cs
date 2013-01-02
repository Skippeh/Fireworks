using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace Fireworks
{
	public class Particle
	{
		public static Texture Texture;

		public Emitter ParentEmitter { get; private set; }

		public Vector2f Position { get; set; }

		private Vector2f oldPosition;

		public Vector2f Velocity { get; set; }

		public Vector2f Gravity { get; set; }

		public List<Modifier> Modifiers { get; private set; } 

		public float Rotation
		{
			get { return drawable.Rotation; }
			set { drawable.Rotation = value; }
		}

		public Vector2f Scale
		{
			get { return drawable.Scale; }
			set { drawable.Scale = value; }
		}

		public Color Color
		{
			get { return drawable.Color; }
			set { drawable.Color = value; }
		}

		public float MaxLifeTime { get; set; }

		public float CurrentLifeTime { get; set; }

		private readonly Sprite drawable;

		private readonly ConvexShape line = new ConvexShape(2);

		public Particle(Vector2f position, Vector2f initialVelocity, Vector2f gravity, Emitter parentEmitter, float lifeTime, Color color)
		{
			Position = position;
			oldPosition = Position;
			Velocity = initialVelocity;
			Gravity = gravity;
			ParentEmitter = parentEmitter;
			CurrentLifeTime = 0;
			MaxLifeTime = lifeTime;

			drawable = new Sprite(Texture)
				           {
					           Color = color,
					           Origin = new Vector2f(Texture.Size.X / 2f, Texture.Size.Y / 2f)
						   };

			line.FillColor = Color;

			Modifiers = new List<Modifier>();
		}

		public virtual void Update(float dt)
		{
			Modifiers.ForEach(mod => mod.ModifyParticle(this, dt));

			Velocity += Gravity * dt;
			Position += Velocity * dt;

			drawable.Position = Position;

			if (ParentEmitter != null)
			{
				CurrentLifeTime += dt;

				if (CurrentLifeTime >= MaxLifeTime)
				{
					ParentEmitter.RemoveParticle(this);
				}
			}
		}

		public virtual void Draw(RenderWindow window, float dt)
		{
			window.Draw(drawable);

			line.SetPoint(0, oldPosition);
			line.SetPoint(1, Position);

			window.Draw(line);
		}
	}
}