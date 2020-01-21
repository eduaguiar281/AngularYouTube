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
    public interface ICanalService
    {
        Task SincronizarCanaisAsync(IList<CanalSearchResultViewModel> canais);
        Task<Canal> GetCanalByYoutubeId(string id);

        Task<IList<Canal>> GetCanaisAsync();
        Task<IList<Canal>> GetCanaisAsync(Expression<Func<Canal, bool>> predicate);

        Task<Canal> GetCanalByIdAsync(string id);

        IMongoQueryable<Canal> GetQuery();
        IMongoCollection<Canal> GetCollection();

    }
}
