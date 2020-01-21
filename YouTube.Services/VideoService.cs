using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YouTubeApp.Core.Models;
using YouTubeApp.Data;
using YouTubeApp.Services.ViewModel;

namespace YouTubeApp.Services
{
    public class VideoService : IVideoService
    {
        private readonly IRepository<Video> _repository;
        private readonly ICanalService _canalService;
        public VideoService(IRepository<Video> repository, ICanalService canalService)
        {
            _repository = repository;
            _canalService = canalService;
        }

        public async Task SincronizarVideosAsync(IList<VideoSearchResultViewModel> videos)
        {
            foreach(VideoSearchResultViewModel video in videos)
            {
                var videoDb = await _repository.Table.FirstOrDefaultAsync(w => w.VideoId == video.YoutubeVideoId);
                if (videoDb == null)
                {
                    var canalDb = await _canalService.GetCanalByYoutubeId(video.YoutubeChannelId);
                    videoDb = new Video()
                    {
                        CriadoEm = DateTime.Now,
                        AtualizadoEm = DateTime.Now,
                        PublicadoEm = video.PublicadoEm,
                        Definicao = video.Definicao,
                        Descricao = video.Descricao,
                        Idioma = video.Idioma,
                        ImagemUrl = video.ImagemUrl,
                        QuantidadeComentario = video.QuantidadeComentario,
                        QuantidadeDeslike = video.QuantidadeDeslike,
                        QuantidadeLike = video.QuantidadeLike,
                        QuantidadeVisualizacao = video.QuantidadeVisualizacao,
                        Titulo = video.Titulo,
                        VideoId = video.YoutubeVideoId,
                        CanalId = canalDb.Id

                    };
                    await _repository.InsertAsync(videoDb);
                }
                else
                {
                    videoDb.AtualizadoEm = DateTime.Now;
                    videoDb.PublicadoEm = video.PublicadoEm;
                    videoDb.Definicao = video.Definicao;
                    videoDb.Descricao = video.Descricao;
                    videoDb.Idioma = video.Idioma;
                    videoDb.ImagemUrl = video.ImagemUrl;
                    videoDb.QuantidadeComentario = video.QuantidadeComentario;
                    videoDb.QuantidadeDeslike = video.QuantidadeDeslike;
                    videoDb.QuantidadeLike = video.QuantidadeLike;
                    videoDb.QuantidadeVisualizacao = video.QuantidadeVisualizacao;
                    videoDb.Titulo = video.Titulo;
                    videoDb.VideoId = video.YoutubeVideoId;
                    await _repository.UpdateAsync(videoDb);
                }
            }
        }

        public async Task<Video> GetVideoByYoutubeId(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IList<Video>> GetVideosAsync()
        {
            return await _repository.Table.ToListAsync();
        }
        public async Task<IList<Video>> GetVideosAsync(Expression<Func<Video, bool>> predicate)
        {
            return await _repository.Table.Where(predicate).ToListAsync();
        }

        public async Task<Video> GetVideoByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public IMongoQueryable<Video> GetQuery()
        {
            return _repository.Table;
        }

        public IMongoCollection<Video> GetCollection()
        {
            return _repository.Collection;
        }
    }
}
