using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NetduinoLEDServer
{
	public class Program
	{
		OutputPort pin0, pin1, pin2, pin3, pin4;
		public static void Main()
		{
			new Program().main();
		}

		public void main()
		{
			// オンボードLEDの点滅
			OutputPort boardLed = new OutputPort(Pins.ONBOARD_LED, false);
			boardLed.Write(true);

			pin0 = new OutputPort(Pins.GPIO_PIN_D0, false);
			pin1 = new OutputPort(Pins.GPIO_PIN_D1, false);
			pin2 = new OutputPort(Pins.GPIO_PIN_D2, false);
			pin3 = new OutputPort(Pins.GPIO_PIN_D3, false);
			pin4 = new OutputPort(Pins.GPIO_PIN_D4, false);

			pin0.Write(false);
			pin1.Write(false);
			pin2.Write(false);
			pin3.Write(false);
			pin4.Write(false);


			// Webサーバーを起動
			WebServer webServer = new WebServer();
			webServer.OnRequest += webServer_OnRequest;
			webServer.ListenForRequest();

			// メインスレッドを終了させるとプログラムが終了するので無限に停止
			Thread.Sleep(Timeout.Infinite);
		}


		void webServer_OnRequest(string request)
		{
			// GET /pin1/on HTTP/1.1
			string path = request.Split(new char[] { ' ' })[1];
			Debug.Print(path);

			string pin = path.Split(new char[] { '/' })[1];
			string sw  = path.Split(new char[] { '/' })[2];
			OutputPort port = null;
			switch (pin)
			{
				case "pin0": port = pin0; break;
				case "pin1": port = pin1; break;
				case "pin2": port = pin2; break;
				case "pin3": port = pin3; break;
				case "pin4": port = pin4; break;
			}
			if (port != null)
			{
				switch ( sw ) {
					case "on": port.Write(true);break;
					case "off": port.Write(false);break;
				}
			}
		}
	}
}
