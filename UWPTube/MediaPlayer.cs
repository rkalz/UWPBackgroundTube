using System;
using Windows.Media.Playback;
using Windows.Storage.Streams;

using UWPTube.Utils.Youtube;

namespace UWPTube.Utils.MediaPlayer
{
    public class MetadataConfig
    {
        // Adds specified metadata to the specified playback item
        public static void AddMetadata(VideoMetadata metadata, MediaPlaybackItem playbackItem)
        {
            var itemProps = playbackItem.GetDisplayProperties();
            itemProps.Type = Windows.Media.MediaPlaybackType.Music;
            itemProps.MusicProperties.Title = metadata.Title;
            itemProps.MusicProperties.Artist = metadata.Uploader;
            itemProps.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(metadata.ThumbnailStandardUrl));
            playbackItem.ApplyDisplayProperties(itemProps);
        }
    }
}