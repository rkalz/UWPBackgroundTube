using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

using YoutubeExplode;

namespace UWPTube
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var client = new YoutubeClient();

            string link = "https://www.youtube.com/watch?v=hCCtaw7URFA";
            var id = YoutubeClient.ParseVideoId(link);

            var video = client.GetVideoAsync(id).Result;
            videoTitle.Text = video.Title;
            videoUploader.Text = video.Author;
            videoThumbnail.Source = new BitmapImage(new Uri(video.Thumbnails.StandardResUrl));

            var streamData = client.GetVideoMediaStreamInfosAsync(id).Result;
            long bitrate = 0;
            string stream = null;
            foreach (var candidate in streamData.Audio)
            {
                if (candidate.Bitrate > bitrate)
                {
                    stream = candidate.Url;
                    bitrate = candidate.Bitrate;
                }
            }
            mediaElement.Source = stream;
            mediaElement.Background = new ImageBrush {
                ImageSource = new BitmapImage(new Uri(video.Thumbnails.MaxResUrl)),
                Opacity = 0.5
            };
        }
    }
}
