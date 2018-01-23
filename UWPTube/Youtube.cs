using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
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
        YouTubeService service;

        public YoutubePuller()
        {
            client = new YoutubeClient();
            service = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = ApiKeys.Youtube,
                ApplicationName = this.GetType().ToString()
            });
        }

        public async Task<VideoMetadata> GetVideoMetadataFromUrl(string url)
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
            data.ThumbnailStandardUrl = rawVideoData.Thumbnails.MediumResUrl;
            data.ThumbnailMaxUrl = rawVideoData.Thumbnails.MaxResUrl;

            return data;
        }

        public async Task<string> GetStreamUrl(string id)
        {
            // Get a list of all the streams and return the one with the highest bitrate
            var streamData = await client.GetVideoMediaStreamInfosAsync(id);
            return streamData.Audio.MaxBy(stream => stream.Bitrate).Url;
        }

        public async Task<List<string>> GetSearchResults(string query)
        {
            List<string> results = new List<string>(); ;
            var searchRequest = service.Search.List("snippet");
            searchRequest.Q = query;
            searchRequest.MaxResults = 50;

            var searchResult = await searchRequest.ExecuteAsync();
            searchResult.Items.ForEach(result =>
            {
                if (result.Id.Kind == "youtube#video")
                {
                    results.Add(String.Format("{0}", result.Id.VideoId));
                }
            });

            return results;
        }
    }
}
