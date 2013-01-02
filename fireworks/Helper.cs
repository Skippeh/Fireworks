using System;
using SFML.Graphics;
using SFML.Window;

namespace Fireworks
{
	public static class Helper
	{
		private static Random random;

		static Helper()
		{
			random = new Random();
		}

		public static Vector2f GetRandomVector2f(int minX, int minY, int maxX, int maxY)
		{
			return GetRandomVector2f(minX, minY, maxX, maxY, random);
		}

		public static Vector2f GetRandomVector2f(int minX, int minY, int maxX, int maxY, Random rand)
		{
			var x = rand.Next(minX, maxX + 1);
			var y = rand.Next(minY, maxY + 1);

			return new Vector2f(x, y);
		}

		public static Vector2f GetVector2FromAngle(float angle)
		{
			var direction = new Vector2f();

			direction.X = (float) Math.Cos(angle);
			direction.Y = (float) Math.Sin(angle);

			return direction;
		}

		public static float GetRandomNumber(int min, int max)
		{
			return random.Next(min, max + 1);
		}

		public static Color GetRandomColor()
		{
			var r = (byte) GetRandomNumber(0, 255);
			var g = (byte) GetRandomNumber(0, 255);
			var b = (byte) GetRandomNumber(0, 255);

			return new Color(r, g, b);
		}

		#region Vector2f extensions

		public static float Distance(this Vector2f v1, Vector2f v2)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}