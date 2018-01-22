using System;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;

using UWPTube.Utils.MediaPlayer;
using UWPTube.Utils.Youtube;


namespace UWPTube
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // Get data from Youtube
            var client = new YoutubePuller();
            VideoMetadata metadata = await client.GetVideoMetadataFromUrl("https://www.youtube.com/watch?v=vQJGlhWVaic");

            // Get stream url to make playback item
            string streamUrl = await client.GetStreamUrl(metadata.ID);
            var playbackItem = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(streamUrl)));

            // Add metadata to playback item
            MetadataConfig.AddMetadata(metadata, playbackItem);

            // Set media source to playback item
            mediaElement.Source = playbackItem;
        }
    }
}
