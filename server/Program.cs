using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Lidgren.Network;
using shared;

namespace server
{
	public static class Program
	{
		private static NetServer server;

		private static Random random = new Random();

		static void Main(string[] args)
		{
			server = new NetServer(new NetPeerConfiguration("Fireworks")
				                       {
					                       AcceptIncomingConnections = true,
					                       Port = 7007,
										   ConnectionTimeout =  5
				                       });

			server.Start();

			Console.WriteLine("Started server");

			while (true)
			{
				NetIncomingMessage message;
				while ((message = server.ReadMessage()) != null)
				{
					switch (message.MessageType)
					{
						case NetIncomingMessageType.Data:
							{
								var type = (Protocol)message.ReadByte();

								HandleIncomingData(type, message.SenderConnection, message);

								break;
							}
						case NetIncomingMessageType.StatusChanged:
							{
								var status = (NetConnectionStatus) message.ReadByte();
								var msg = message.ReadString();

								Console.WriteLine(status);
								Console.WriteLine(msg);

								if (status == NetConnectionStatus.Connected)
								{
									Console.WriteLine("Client connected from " + message.SenderConnection.ToString());
								}
								else if (status == NetConnectionStatus.Disconnected)
								{
									Console.WriteLine("Client disconnected from " + message.SenderConnection.ToString());
								}

								break;
							}
					}
				}

				Thread.Sleep(1);
			}
		}

		private static void HandleIncomingData(Protocol type, NetConnection connection, NetIncomingMessage message)
		{
			switch (type)
			{
				case Protocol.SpawnFirework:
					{
						var x = message.ReadInt32();
						var y = message.ReadInt32();

						var outgoingMessage = server.CreateMessage();
						outgoingMessage.Write((byte) Protocol.SpawnFirework);
						outgoingMessage.Write(x);
						outgoingMessage.Write(y);
						outgoingMessage.Write(random.Next());

						Console.WriteLine("Sending SpawnFirework at {" + x + ", " + y + "} to all clients.");

						server.SendToAll(outgoingMessage, NetDeliveryMethod.ReliableOrdered);

						break;
					}
			}
		}
	}
}