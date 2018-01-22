using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YoutubeExplode;

namespace UWPTube.Utils.Youtube
{

    struct VideoData
    {
        public string Title;
        public string Uploader;
        public string ThumbnailStandardUrl;
        public string ThumbnailMaxUrl;

        public string StreamUrl;
    }

    class YoutubePuller
    {
        YoutubeClient client;

        public YoutubePuller()
        {
            client = new YoutubeClient();
        }

        public VideoData GetVideoDataFromUrl(String url)
        {
            var id = YoutubeClient.ParseVideoId(url);
            return GetVideoData(id);
        }

        public VideoData GetVideoData(string id)
        {
            VideoData data = new VideoData();

            var rawVideoData = client.GetVideoAsync(id).Result;
            data.Title = rawVideoData.Title;
            data.Uploader = rawVideoData.Author;
            data.ThumbnailStandardUrl = rawVideoData.Thumbnails.StandardResUrl;
            data.ThumbnailMaxUrl = rawVideoData.Thumbnails.MaxResUrl;

            var streamData = client.GetVideoMediaStreamInfosAsync(id).Result;
            long bitrate = 0;
            foreach (var candidate in streamData.Audio)
            {
                if (candidate.Bitrate > bitrate)
                {
                    data.StreamUrl = candidate.Url;
                    bitrate = candidate.Bitrate;
                }
            }

            return data;
        }


    }
}
