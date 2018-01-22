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

            // Get data from Youtube
            var client = new YoutubePuller();
            VideoMetadata metadata = client.GetVideoDataFromUrl("https://www.youtube.com/watch?v=vQJGlhWVaic").Result;

            // Get stream url to make playback item
            string streamUrl = client.GetStreamUrl(metadata.ID).Result;
            var playbackItem = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(streamUrl)));

            // Add metadata to playback item
            MetadataConfig.AddMetadata(metadata, playbackItem);

            // Set media source to playback item
            mediaElement.Source = playbackItem;
        }
    }
}
