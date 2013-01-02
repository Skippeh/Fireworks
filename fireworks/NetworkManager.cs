using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Lidgren.Network;
using SFML.Graphics;
using shared;

namespace Fireworks
{
	public static class NetworkManager
	{
		private static NetClient client;

		public static void Initialize(string ip)
		{
			client = new NetClient(new NetPeerConfiguration("Fireworks")
				                       {
					                       AcceptIncomingConnections = false,
					                       ConnectionTimeout = 5
				                       });

			client.Start();
			client.Connect(ip, 7007);

			new Thread(ListenForData).Start();
		}

		private static void ListenForData()
		{
			while (Program.Running)
			{
				NetIncomingMessage message;
				while ((message = client.ReadMessage()) != null)
				{
					switch (message.MessageType)
					{
						case NetIncomingMessageType.Data:
							{
								var protocolType = (Protocol)message.ReadByte();

								HandleIncomingData(protocolType, message.SenderConnection, message);

								break;
							}
						case NetIncomingMessageType.StatusChanged:
							{
								var status = (NetConnectionStatus) message.ReadByte();
								var msg = message.ReadString();

								Console.WriteLine(msg);

								if (status == NetConnectionStatus.Disconnected)
								{
									Console.WriteLine("Disconnected from server, exiting after key press.");
									Console.ReadKey(true);
									Program.Running = false;
								}

								break;
							}
					}
				}

				Thread.Sleep(1);
			}
		}

		private static void HandleIncomingData(Protocol protocolType, NetConnection serverConnection, NetIncomingMessage message)
		{
			switch (protocolType)
			{
				case Protocol.SpawnFirework:
					{
						int x = message.ReadInt32();
						int y = message.ReadInt32();
						int seed = message.ReadInt32();

						Console.WriteLine("Spawning local firework at {" + x + ", " + y + "}.");

						// TODO: Spawn a local firework.
						FireworkManager.SpawnFirework(x, y, seed);

						break;
					}
			}
		}

		public static void SpawnFirework(int x, int y)
		{
			var message = client.CreateMessage();

			message.Write((byte) Protocol.SpawnFirework);
			message.Write(x);
			message.Write(y);

			client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
			Console.WriteLine("Sent SpawnFirework at {" + x + ", " + y + "} to server.");
		}

		public static void Disconnect()
		{
			client.Disconnect("Client disconnecting");
		}
	}
}