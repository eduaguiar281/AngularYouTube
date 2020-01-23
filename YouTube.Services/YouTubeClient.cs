using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Configuration;
using YouTube.Services;

namespace YouTubeApp.Services
{
    public class YouTubeClient : IYouTubeClient
    {
        private readonly Google.Apis.YouTube.v3.YouTubeService _service;

        public YouTubeClient(IConfiguration configuration)
        {
            _service = new Google.Apis.YouTube.v3.YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = configuration.GetSection("YouTubeConfig").GetSection("APIKEY").Value,
                ApplicationName = configuration.GetSection("YouTubeConfig").GetSection("ClientApplicationName").Value
            });
        }

        public async Task<SearchListResponse> SearchAsync(string query, int maxResults)
        {
            if (string.IsNullOrEmpty(query))
                throw new ArgumentNullException(nameof(query));

            if (maxResults == 0)
                throw new ArgumentException("Valor informado não pode ser zero!", nameof(maxResults));

            var searchListRequest = _service.Search.List("snippet");
            searchListRequest.Q = query;
            searchListRequest.MaxResults = maxResults;
            return await searchListRequest.ExecuteAsync();
        }

        public async Task<ChannelListResponse> GetChannelListAsync(string channelIds)
        {
            if (string.IsNullOrEmpty(channelIds))
                throw new ArgumentNullException(nameof(channelIds));
            var listRequest = _service.Channels.List("id, snippet, brandingSettings, contentDetails, invideoPromotion, statistics, topicDetails");
            listRequest.Id = channelIds;
            return await listRequest.ExecuteAsync();
        }

        public async Task<VideoListResponse> GetVideoListAsync(string videosIds)
        {
            if (string.IsNullOrEmpty(videosIds))
                throw new ArgumentNullException(nameof(videosIds));

            var listRequest = _service.Videos.List("id, snippet, contentDetails, recordingDetails, statistics, status");
            listRequest.Id = videosIds;
            return await listRequest.ExecuteAsync();
        }

        public async Task<SearchListResponse> SearchAsync(string query, string pageToken, int maxResults)
        {
            if (string.IsNullOrEmpty(query))
                throw new ArgumentNullException(nameof(query));

            if (maxResults == 0)
                throw new ArgumentException("Valor informado não pode ser zero!", nameof(maxResults));

            if (string.IsNullOrEmpty(pageToken))
                throw new ArgumentNullException(nameof(pageToken));


            var searchListRequest = _service.Search.List("snippet");
            searchListRequest.Q = query;
            searchListRequest.PageToken = pageToken;
            searchListRequest.MaxResults = maxResults;
            return await searchListRequest.ExecuteAsync();
        }
    }
}
