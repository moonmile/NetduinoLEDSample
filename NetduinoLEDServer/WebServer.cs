using System;
using Microsoft.SPOT;
using System.Net.Sockets;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Net;
using System.Text;
using System.Threading;

namespace NetduinoLEDServer
{

	public class WebServer : IDisposable
	{
		public delegate void OnRequestDelegate(string request);
		public event OnRequestDelegate OnRequest;

		private Socket socket = null;
		//open connection to onbaord led so we can blink it with every request
		// private OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);
		public WebServer()
		{
			//Initialize Socket class
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//Request and bind to an IP from DHCP server
			socket.Bind(new IPEndPoint(IPAddress.Any, 80));
			//Debug print our IP address
			Debug.Print(Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress);
		}
		


		public void ListenForRequest()
		{
			//Start listen for web requests
			socket.Listen(10);

			while (true)
			{
				using (Socket clientSocket = socket.Accept())
				{
					//Get clients IP
					IPEndPoint clientIP = clientSocket.RemoteEndPoint as IPEndPoint;
					EndPoint clientEndPoint = clientSocket.RemoteEndPoint;
					//int byteCount = cSocket.Available;
					int bytesReceived = clientSocket.Available;
					if (bytesReceived > 0)
					{
						//Get request
						byte[] buffer = new byte[bytesReceived];
						int byteCount = clientSocket.Receive(buffer, bytesReceived, SocketFlags.None);
						string request = new string(Encoding.UTF8.GetChars(buffer));
						Debug.Print(request);
						if (OnRequest != null)
						{
							OnRequest(request);
						}
						//Compose a response
						string response = "now: " + DateTime.Now.ToString();
						string header = "HTTP/1.0 200 OK\r\nContent-Type: text; charset=utf-8\r\nContent-Length: " + response.Length.ToString() + "\r\nConnection: close\r\n\r\n";
						clientSocket.Send(Encoding.UTF8.GetBytes(header), header.Length, SocketFlags.None);
						clientSocket.Send(Encoding.UTF8.GetBytes(response), response.Length, SocketFlags.None);
						//Blink the onboard LED
						// led.Write(true);
						Thread.Sleep(150);
						// led.Write(false);
					}
				}
			}
		}
		#region IDisposable Members
		~WebServer()
		{
			Dispose();
		}
		public void Dispose()
		{
			if (socket != null)
				socket.Close();
		}
		#endregion
	}
}
