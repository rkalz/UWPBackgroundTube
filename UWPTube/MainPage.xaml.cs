using System;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

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
            VideoData videoInfo = client.GetVideoDataFromUrl("https://www.youtube.com/watch?v=hCCtaw7URFA");

            // Set media element source and metadata
            var playbackItem = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(videoInfo.StreamUrl)));
            var metadata = playbackItem.GetDisplayProperties();
            metadata.Type = Windows.Media.MediaPlaybackType.Music;
            metadata.MusicProperties.Title = videoInfo.Title;
            metadata.MusicProperties.Artist = videoInfo.Uploader;
            metadata.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(videoInfo.ThumbnailStandardUrl));
            playbackItem.ApplyDisplayProperties(metadata);
            mediaElement.Source = playbackItem;
        } 
    }
}
