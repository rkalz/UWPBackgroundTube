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
        }

        private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var client = new YoutubePuller();
            var results = await client.GetSearchResults(SearchBox.Text);
            SearchResults.ItemsSource = results;
        }

        private async void SearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get data from Youtube
            var client = new YoutubePuller();
            VideoMetadata metadata = await client.GetVideoMetadata(SearchResults.SelectedItem.ToString());

            // Get stream url to make playback item
            string streamUrl = await client.GetStreamUrl(metadata.ID);
            var playbackItem = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(streamUrl)));

            // Add metadata to playback item
            MetadataConfig.AddMetadata(metadata, playbackItem);

            // Set media source to playback item
            MediaElement.Source = playbackItem;
        }
    }
}
