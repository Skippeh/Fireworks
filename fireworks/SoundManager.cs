using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SFML.Audio;

namespace Fireworks
{
	public static class SoundManager
	{
		private static Dictionary<string, SoundBuffer> sounds = new Dictionary<string, SoundBuffer>(); 

		static SoundManager()
		{
			if (!Directory.Exists(@"Data\Sounds")) Directory.CreateDirectory(@"Data\Sounds");
			var files = Directory.GetFiles(@"Data\Sounds");

			foreach (var fileName in files)
			{
				sounds.Add(Path.GetFileNameWithoutExtension(fileName), new SoundBuffer(fileName));
			}

			Listener.Direction = new Vector3f(0, 0, -1);
			Listener.Position = new Vector3f(400, 0, 0);
			Listener.GlobalVolume = 100f;
		}

		public static Sound Play(string soundName, Vector3f position)
		{
			if (sounds.ContainsKey(soundName))
			{
				var sound = new Sound(sounds[soundName])
								{
									RelativeToListener = false, // Don't follow the listener around.
									Position = position,
									MinDistance = 400f,
									Attenuation = 1f,
								};

				sound.Play();

				return sound;
			}
			else
			{
				Console.WriteLine("Unable to play sound, file not found: " + soundName);

				return null;
			}
		}
	}
}