using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YouTube.Services;
using YouTubeApp.Core.Models;
using YouTubeApp.Data;
using YouTubeApp.Services.ViewModel;

namespace YouTubeApp.Services
{
    public class CanalService : ICanalService
    {
        private readonly IRepository<Canal> _repository;

        public CanalService(IRepository<Canal> repository)
        {
            _repository = repository;
        }

        public async Task<Canal> GetCanalByYoutubeId(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            return await _repository.GetSingleAsync(w => w.ChannelId == id);
        }

        public async Task SincronizarCanaisAsync(IList<CanalSearchResultViewModel> canais)
        {
            if (canais == null)
                throw new ArgumentNullException(nameof(canais));

            if (canais.Count == 0)
                throw new ArgumentException("Lista de canais não possui elementos", nameof(canais));


            foreach (CanalSearchResultViewModel canal in canais)
            {
                if (string.IsNullOrEmpty(canal.YoutubeChannelId))
                    throw new ServiceException("Canal informado não possui Id do YouTube!");

                if (string.IsNullOrEmpty(canal.Titulo))
                    throw new ServiceException("Canal informado não possui Título!");

                if (string.IsNullOrEmpty(canal.Descricao))
                    throw new ServiceException("Canal informado não possui Descricao!");

                var canalDb = await _repository.GetSingleAsync(w => w.ChannelId == canal.YoutubeChannelId);
                if (canalDb == null)
                {
                    canalDb = new Canal
                    {
                        ChannelId = canal.YoutubeChannelId,
                        PublicadoEm = canal.PublicadoEm,
                        Descricao = canal.Descricao,
                        Imagem = canal.ImagemUrl,
                        Title = canal.Titulo,
                        CriadoEm = DateTime.Now,
                        AtualizadoEm = DateTime.Now
                    };
                    await _repository.InsertAsync(canalDb);
                }
                else
                {
                    canalDb.ChannelId = canal.YoutubeChannelId;
                    canalDb.PublicadoEm = canal.PublicadoEm;
                    canalDb.Descricao = canal.Descricao;
                    canalDb.Imagem = canal.ImagemUrl;
                    canalDb.Title = canal.Titulo;
                    canalDb.AtualizadoEm = DateTime.Now;
                    await _repository.UpdateAsync(canalDb);
                }
            }
        }

        public async Task<IList<Canal>> GetCanaisAsync()
        {
            return await _repository.Table.ToListAsync();
        }
        public async Task<IList<Canal>> GetCanaisAsync(Expression<Func<Canal, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            
            return await _repository.Table.Where(predicate).ToListAsync();
        }

        public async Task<Canal> GetCanalByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            return await _repository.GetByIdAsync(id);
        }

        public IMongoQueryable<Canal> GetQuery()
        {
            return _repository.Table;
        }

        public IMongoCollection<Canal> GetCollection()
        {
            return _repository.Collection;
        }


    }
}
