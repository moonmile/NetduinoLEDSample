using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NetduinoLEDOnly
{
	public class Program
	{
		OutputPort pin0, pin1, pin2, pin3, pin4;
		public static void Main()
		{
			new Program().main();
		}

		delegate void Action();

		public void main()
		{
			// オンボードLEDの点滅
			OutputPort boardLed = new OutputPort(Pins.ONBOARD_LED, false);
			boardLed.Write(true);
			InputPort btn = new InputPort( Pins.ONBOARD_BTN, false, Port.ResistorMode.Disabled);

			int pat = 0;


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

			Action[] patterns = new Action[] { 
				pattern01, pattern02,pattern03,
				pattern11, pattern12,pattern13,
				pattern2, pattern3, pattern4 };


			// LEDを光らせるスレッド
			new Thread(() => {

				pin0.Write(true);
				Thread.Sleep(500);

				while (true)
				{
					patterns[pat]();
				}
			}).Start();
			// ボタン待ちのスレッド
			bool preBtn = false;
			new Thread(() => {
				while (true)
				{
					if (btn.Read()) {
						if (preBtn == false)
						{
							pat++;
							if (pat >= patterns.Length) pat = 0;
							// リレーのために pin0 だけ付けておく
							pin0.Write(true);
						}
						preBtn = true;
					}
					else
					{
						preBtn = false;
					}
					Thread.Sleep(50);
				}
			}).Start();

			// メインスレッドを終了させるとプログラムが終了するので無限に停止
			Thread.Sleep(Timeout.Infinite);
		}

		void pattern01() { pattern0(500); }
		void pattern02() { pattern0(100); }
		void pattern03() { pattern0(50); }
		void pattern11() { pattern1(500); }
		void pattern12() { pattern1(100); }
		void pattern13() { pattern1(50); }

		void pattern0(int msec)
		{
			if (pin0.Read())
			{
				pin0.Write(false);
				pin1.Write(true);
			}
			else if (pin1.Read())
			{
				pin1.Write(false);
				pin2.Write(true);
			}
			else if (pin2.Read())
			{
				pin2.Write(false);
				pin3.Write(true);
			}
			else if (pin3.Read())
			{
				pin3.Write(false);
				pin4.Write(true);
			}
			else if (pin4.Read())
			{
				pin4.Write(false);
				pin0.Write(true);
			}
			Thread.Sleep(msec);
		}
		void pattern1(int msec)
		{
			if (pin0.Read())
			{
				pin0.Write(false);
				pin4.Write(true);
			}
			else if (pin1.Read())
			{
				pin1.Write(false);
				pin0.Write(true);
			}
			else if (pin2.Read())
			{
				pin2.Write(false);
				pin1.Write(true);
			}
			else if (pin3.Read())
			{
				pin3.Write(false);
				pin2.Write(true);
			}
			else if (pin4.Read())
			{
				pin4.Write(false);
				pin3.Write(true);
			}
			Thread.Sleep(msec);
		}
		/// <summary>
		///  全て消す
		/// </summary>
		void pattern2()
		{
			pin0.Write(false);
			pin1.Write(false);
			pin2.Write(false);
			pin3.Write(false);
			pin4.Write(false);
			Thread.Sleep(100);
		}
		/// <summary>
		///  全て付ける
		/// </summary>
		void pattern4()
		{
			pin0.Write(true);
			pin1.Write(true);
			pin2.Write(true);
			pin3.Write(true);
			pin4.Write(true);
			Thread.Sleep(100);
		}
		/// <summary>
		/// ゲラゲラボー
		/// </summary>
		void pattern3()
		{
			pin0.Write(true);
			pin1.Write(true);
			pin2.Write(true);
			pin3.Write(true);
			pin4.Write(true);
			Thread.Sleep(200);
			pin0.Write(false);
			pin1.Write(false);
			pin2.Write(false);
			pin3.Write(false);
			pin4.Write(false);
			Thread.Sleep(200);
			pin0.Write(true);
			pin1.Write(true);
			pin2.Write(true);
			pin3.Write(true);
			pin4.Write(true);
			Thread.Sleep(200);
			pin0.Write(false);
			pin1.Write(false);
			pin2.Write(false);
			pin3.Write(false);
			pin4.Write(false);
			Thread.Sleep(200);
			pin0.Write(true);
			pin1.Write(true);
			pin2.Write(true);
			pin3.Write(true);
			pin4.Write(true);
			Thread.Sleep(500);
			pin0.Write(false);
			pin1.Write(false);
			pin2.Write(false);
			pin3.Write(false);
			pin4.Write(false);
			Thread.Sleep(200);
		}
	}
}

