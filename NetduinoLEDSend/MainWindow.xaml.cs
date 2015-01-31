using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetduinoLEDSend
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		async void SendCommand(string pin, bool sw)
		{
			string host = text1.Text;
			int port = 80;

			Uri uri = new Uri(string.Format("http://{0}:{1}/{2}/{3}", host, port, pin, sw? "on":"off"));
			HttpClient cl = new HttpClient();
			var res = await cl.GetAsync(uri);
			var result = await res.Content.ReadAsStringAsync();
			text2.Text = result;
		}


		private void clickPin0on(object sender, RoutedEventArgs e)
		{
			SendCommand( "pin0", true);
		}
		private void clickPin0off(object sender, RoutedEventArgs e)
		{
			SendCommand("pin0", false);
		}
		private void clickPin1on(object sender, RoutedEventArgs e)
		{
			SendCommand("pin1", true);
		}
		private void clickPin1off(object sender, RoutedEventArgs e)
		{
			SendCommand("pin1", false);
		}
		private void clickPin2on(object sender, RoutedEventArgs e)
		{
			SendCommand("pin2", true);
		}
		private void clickPin2off(object sender, RoutedEventArgs e)
		{
			SendCommand("pin2", false);
		}
		private void clickPin3on(object sender, RoutedEventArgs e)
		{
			SendCommand("pin3", true);
		}
		private void clickPin3off(object sender, RoutedEventArgs e)
		{
			SendCommand("pin3", false);
		}
		private void clickPin4on(object sender, RoutedEventArgs e)
		{
			SendCommand("pin4", true);
		}
		private void clickPin4off(object sender, RoutedEventArgs e)
		{
			SendCommand("pin4", false);
		}

	
	}
}
