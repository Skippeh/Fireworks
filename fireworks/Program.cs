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

		private static Vector2i mouseDownPosition = new Vector2i();
		private static bool drawAimLine = false;
		private static bool isMouseDown = false;

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
			wnd.MouseButtonReleased += OnMouseButtonReleased;
			wnd.MouseMoved += OnMouseMoved;

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

		private static void OnMouseMoved(object sender, MouseMoveEventArgs e)
		{
			if (isMouseDown)
				mouseDownPosition = new Vector2i(e.X, e.Y);
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
			{
				//NetworkManager.SpawnFirework(e.X, e.Y);
				drawAimLine = true;
				mouseDownPosition = new Vector2i(e.X, e.Y);
			}
		}

		private static void OnMouseButtonReleased(object sender, MouseButtonEventArgs e)
		{
			drawAimLine = false;

			// Spawn firework with appropriate velocity
			var mouseDownPositionF = new Vector2f(mouseDownPosition.X, mouseDownPosition.Y);

			var mousePosition = Mouse.GetPosition(wnd);
			var mousePositionF = new Vector2f(mousePosition.X, mousePosition.Y);

			var distance = mouseDownPositionF.Distance(mousePositionF);
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

			if (drawAimLine)
			{
				var mouseDownPositionF = new Vector2f(mouseDownPosition.X, mouseDownPosition.Y);

				var mousePosition = Mouse.GetPosition(wnd);
				var mousePositionF = new Vector2f(mousePosition.X, mousePosition.Y);

				Console.WriteLine("{" + mouseDownPositionF.X + "," + mouseDownPositionF.Y + "} - {" + mousePositionF.X + ", " + mousePositionF.Y + "}");

				var vertices = new Vertex[2];
				vertices[0] = new Vertex(mouseDownPositionF, Color.White);
				vertices[1] = new Vertex(mousePositionF, Color.White);

				wnd.Draw(vertices, PrimitiveType.Lines);
			}

			wnd.Display();
		}
	}
}