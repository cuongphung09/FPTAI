using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
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

namespace FPT_AI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        class ReturnData
        {
            public string async { get; set; }
            public string error { get; set; }
            public string message { get; set; }
            public string request_id { get; set; }
        }
        private void convertButton_Click(object sender, RoutedEventArgs e)
        {
            Title = "Processing";
            String json = Task.Run(async () =>
            {
                String payload = "Tôi tên là Phùng Trí Cường";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("api-key", "12wzrse2wXjrEUuDEbj7TRUCH6012jSl");
                client.DefaultRequestHeaders.Add("speed", "");
                ComboBoxItem typeItem = (ComboBoxItem)combobox.SelectedItem;
                string value = typeItem.Content.ToString();
                Console.WriteLine(value);
                client.DefaultRequestHeaders.Add("voice",value );
                var response = await client.PostAsync("https://api.fpt.ai/hmi/tts/v5", new StringContent(payload));
                return await response.Content.ReadAsStringAsync();
            }).GetAwaiter().GetResult();

            var data = JsonConvert.DeserializeObject<ReturnData>(json);
            Debug.WriteLine("Error: ");
            Debug.WriteLine(data.error);
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var fileName = $"{folder}{Guid.NewGuid()}.mp3";
            Thread.Sleep(2000);
            using (var client = new WebClient())
            {
                client.DownloadFile(data.async, fileName);
            }
            Title = "Download Finished!";
            var player = new MediaPlayer();
            player.Open(new Uri(fileName, UriKind.Absolute));
            Title = "Playing audio file";
            player.MediaEnded += Player_MediaEnded;
            player.Play();
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            Title = "Player Ended!";
        }

        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
