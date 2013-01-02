using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace Fireworks
{
	public static class Program
	{
		public static bool Running { get; set; }
		private static RenderWindow wnd;

		private static Font font;
		private static Text debugText;

		static void Main(string[] args)
		{
			Running = true;

			IPAddress ipAddress;
			while (true)
			{
				Console.Clear();
				Console.Write("IP to server: ");
				string line = Console.ReadLine();

				if (IPAddress.TryParse(line, out ipAddress))
				{
					break;
				}
				else
				{
					Console.WriteLine("Invalid IP address, press enter to try again.");
					Console.ReadKey(true);
				}
			}

			Console.Clear();
			Console.WriteLine("Got IP: " + ipAddress.ToString());

			NetworkManager.Initialize(ipAddress.ToString());

			wnd = new RenderWindow(new VideoMode(800, 480), "HAPPY NEW CHRISTMAS", Styles.Close);
			
			wnd.SetMouseCursorVisible(true);
			wnd.SetFramerateLimit(120);
			wnd.Closed += (sender, eventArgs) =>
				              {
					              wnd.Close();
					              NetworkManager.Disconnect();
				              };

			wnd.KeyPressed += OnKeyPressed;
			wnd.MouseButtonPressed += OnMouseButtonPressed;

			Initialize();

			Stopwatch stopwatch = Stopwatch.StartNew();
			while (wnd.IsOpen() && Running)
			{
				var frameTime = (float) stopwatch.Elapsed.TotalSeconds;
				stopwatch.Restart();

				wnd.DispatchEvents();
				Update(frameTime);
				Draw(frameTime);
			}

			Running = false;

			wnd.Close();
		}

		private static void Initialize()
		{
			Particle.Texture = new Texture(@"Data\Textures\Particle.png");

			font = new Font(@"Data\Fonts\Minecraftia.ttf");
			debugText = new Text("Hjello", font, 8)
			{
				Position = new Vector2f(10, 10)
			};
		}

		private static void OnKeyPressed(object sender, KeyEventArgs e)
		{

		}

		private static void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
		{
			if (e.Button == Mouse.Button.Left)
				NetworkManager.SpawnFirework(e.X, e.Y);
		}

		private const float maxWaitTime = 0.4f;
		private static float currentWaitTime = 0f;

		private static void Update(float dt)
		{
			if (Mouse.IsButtonPressed(Mouse.Button.Right))
			{
				currentWaitTime += dt;

				if (currentWaitTime >= maxWaitTime)
				{
					currentWaitTime = 0;

					var pos = new Vector2i((int) Helper.GetRandomNumber(100, 700), 480);
					NetworkManager.SpawnFirework(pos.X, pos.Y);
				}
			}
			else
			{
				currentWaitTime = maxWaitTime;
			}

			FireworkManager.Update(dt);
		}

		private static void Draw(float dt)
		{
			wnd.Clear();

			FireworkManager.Draw(wnd, dt);

			wnd.Draw(debugText);

			wnd.Display();
		}
	}
}