using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouTubeApp.Services.ViewModel;

namespace YouTubeApp.Services
{
    public interface IYouTubeService
    {
        Task<YouTubeResponseViewModel> SearchAsync(string query, int maxResults);
        Task<YouTubeResponseViewModel> SearchAsync(string query, string pageToken, int maxResults);
        //Task<IList<CanalSearchResultViewModel>> GetChannelsByIdsAsync(List<string> ids);
        Task<IList<VideoSearchResultViewModel>> GetVideosByIdsAsync(List<string> ids);
    }
}
