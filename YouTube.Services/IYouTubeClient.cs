using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouTubeApp.Services
{
    public interface IYouTubeClient
    {
        Task<SearchListResponse> SearchAsync(string query, int maxResusts);
        Task<SearchListResponse> SearchAsync(string query, string pageToken, int maxResults);

        Task<ChannelListResponse> GetChannelListAsync(string channelIds);
        Task<VideoListResponse> GetVideoListAsync(string videosIds);

    }
}
