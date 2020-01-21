using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YouTubeApp.Core.Models;
using YouTubeApp.Services.ViewModel;

namespace YouTubeApp.Services
{
    public interface IVideoService
    {
        Task SincronizarVideosAsync(IList<VideoSearchResultViewModel> videos);

        Task<Video> GetVideoByYoutubeId(string id);

        Task<IList<Video>> GetVideosAsync();
        Task<IList<Video>> GetVideosAsync(Expression<Func<Video, bool>> predicate);

        Task<Video> GetVideoByIdAsync(string id);

        IMongoQueryable<Video> GetQuery();

        IMongoCollection<Video> GetCollection();

    }
}
