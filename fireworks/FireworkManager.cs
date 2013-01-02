using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace Fireworks
{
	class FireworkManager
	{
		private static List<Firework> fireworks = new List<Firework>();

		public static void Update(float dt)
		{
			fireworks.ForEach(firework => firework.Update(dt));
		}

		public static void Draw(RenderWindow window, float dt)
		{
			fireworks.ForEach(firework => firework.Draw(window, dt));
		}

		public static void SpawnFirework(int x, int y, int seed)
		{
			var random = new Random(seed);
			fireworks.Add(new BasicFirework(new Vector2f(x, y), Helper.GetRandomVector2f(-50, -500, 50, -450, random), new Vector2f(0, 300)));
		}
	}
}