using System;
using System.Threading.Tasks;

using MoreLinq;
using YoutubeExplode;

namespace UWPTube.Utils.Youtube
{

    // Struct containing strings needed for playback, metadata
    public struct VideoMetadata
    {
        public string Title;
        public string ID;
        public string Uploader;
        public string ThumbnailStandardUrl;
        public string ThumbnailMaxUrl;
    }

    class YoutubePuller
    {
        YoutubeClient client;

        public YoutubePuller()
        {
            client = new YoutubeClient();
        }

        public async Task<VideoMetadata> GetVideoMetadataFromUrl(String url)
        {
            var id = YoutubeClient.ParseVideoId(url);
            return await GetVideoMetadata(id);
        }

        public async Task<VideoMetadata> GetVideoMetadata(string id)
        {
            VideoMetadata data;
            data.ID = id;

            // Populates struct with desired metadata
            var rawVideoData = await client.GetVideoAsync(id);
            data.Title = rawVideoData.Title;
            data.Uploader = rawVideoData.Author;
            data.ThumbnailStandardUrl = rawVideoData.Thumbnails.StandardResUrl;
            data.ThumbnailMaxUrl = rawVideoData.Thumbnails.MaxResUrl;

            return data;
        }

        public async Task<string> GetStreamUrl(string id)
        {
            // Get a list of all the streams and return the one with the highest bitrate
            var streamData = await client.GetVideoMediaStreamInfosAsync(id);
            return streamData.Audio.MaxBy(stream => stream.Bitrate).Url;
        }
    }
}
