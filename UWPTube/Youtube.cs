using System;
using System.Threading.Tasks;

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

        public async Task<VideoMetadata> GetVideoDataFromUrl(String url)
        {
            var id = YoutubeClient.ParseVideoId(url);
            return await GetVideoData(id).ConfigureAwait(false);
        }

        public async Task<VideoMetadata> GetVideoData(string id)
        {
            VideoMetadata data;
            data.ID = id;

            // Populates struct with desired video data
            var rawVideoData = await client.GetVideoAsync(id).ConfigureAwait(false);
            data.Title = rawVideoData.Title;
            data.Uploader = rawVideoData.Author;
            data.ThumbnailStandardUrl = rawVideoData.Thumbnails.StandardResUrl;
            data.ThumbnailMaxUrl = rawVideoData.Thumbnails.MaxResUrl;

            return data;
        }

        public async Task<string> GetStreamUrl(string id)
        {
            string url = null;
            // Selects stream with highest bitrate
            var streamData = await client.GetVideoMediaStreamInfosAsync(id).ConfigureAwait(false);
            long bitrate = 0;
            foreach (var candidate in streamData.Audio)
            {
                if (candidate.Bitrate > bitrate)
                {
                    url = candidate.Url;
                    bitrate = candidate.Bitrate;
                }
            }

            return url;
        }


    }
}
