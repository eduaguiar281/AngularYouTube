using Google;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouTubeApp.Services.ViewModel;

namespace YouTubeApp.Services
{
    public class YouTubeService : IYouTubeService
    {
        private YouTubeClient _client;

        public YouTubeService(IConfiguration configuration)
        {
            _client = new YouTubeClient(configuration);
        }

        public async Task<YouTubeResponseViewModel> SearchAsync(string query, int maxResults)
        {
            if (string.IsNullOrEmpty(query))
                throw new ArgumentNullException(nameof(query));

            if (maxResults == 0)
                throw new ArgumentException("Valor informado não pode ser zero!", nameof(maxResults));

            try
            {
                var response = await _client.SearchAsync(query, maxResults);
                return await ReadResponse(response);
            }
            catch (GoogleApiException)
            {
                return new YouTubeResponseViewModel()
                {
                    IsSuccess = false
                };
            }
            
        }

        public async Task<YouTubeResponseViewModel> SearchAsync(string query, string pageToken, int maxResults)
        {
            if (string.IsNullOrEmpty(query))
                throw new ArgumentNullException(nameof(query));

            if (maxResults == 0)
                throw new ArgumentException("Valor informado não pode ser zero!", nameof(maxResults));

            if (string.IsNullOrEmpty(pageToken))
                throw new ArgumentNullException(nameof(pageToken));
            try
            {
                var response = await _client.SearchAsync(query, pageToken, maxResults);
                return await ReadResponse(response);
            }
            catch (GoogleApiException)
            {
                return new YouTubeResponseViewModel()
                {
                    IsSuccess = false
                };
            }
        }

        public async Task<IList<VideoSearchResultViewModel>> GetVideosByIdsAsync(List<string> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            if (ids.Count == 0)
                throw new ArgumentException("Nenhum id foi encontrado na lista", nameof(ids));

            var result = new List<VideoSearchResultViewModel>();
            var videoIds = String.Join(", ", ids.ToArray());
            var response = await _client.GetVideoListAsync(videoIds);
            if (response == null || response.Items.Count == 0)
                return result;

            foreach (Google.Apis.YouTube.v3.Data.Video video in response.Items)
                result.Add(CreateVideoViewModel(video));

            return result;
        }


        private CanalSearchResultViewModel CreateCanalViewModel(Channel channel)
        {
            return new CanalSearchResultViewModel
            {
                YoutubeChannelId = channel.Id,
                Titulo = channel.Snippet.Title,
                ImagemUrl = channel.Snippet.Thumbnails.Default__.Url.Replace("https", "http"),
                Descricao = channel.Snippet.Description,
                PublicadoEm = channel.Snippet.PublishedAt
            };
        }

        private VideoSearchResultViewModel CreateVideoViewModel(Google.Apis.YouTube.v3.Data.Video video)
        {
            return new VideoSearchResultViewModel
            {
                YoutubeVideoId = video.Id,
                YoutubeChannelId = video.Snippet.ChannelId,
                Titulo = video.Snippet.Title,
                ImagemUrl = video.Snippet.Thumbnails.Default__.Url.Replace("https", "http"),
                Descricao = video.Snippet.Description,
                PublicadoEm = video.Snippet.PublishedAt,
                Definicao = video.ContentDetails.Definition,
                Idioma = video.Snippet.DefaultLanguage,
                QuantidadeLike = video.Statistics?.LikeCount,
                QuantidadeDeslike = video.Statistics?.DislikeCount,
                QuantidadeComentario = video.Statistics?.CommentCount,
                QuantidadeVisualizacao = video.Statistics?.ViewCount
            };
        }

        private async Task<CanalSearchResultViewModel> CompleteData(SearchResult searchResult)
        {
            var response = await _client.GetChannelListAsync(searchResult.Snippet.ChannelId);
            var channel = response.Items.FirstOrDefault();
            if (channel != null)
            {
                var result = CreateCanalViewModel(channel);
                result.IncludeInList = false;
                return result;
            }
            else
            {
                return default;
            }
        }

        private async Task<YouTubeResponseViewModel> ReadResponse(SearchListResponse response)
        {
            var lista = new List<ISearchResultViewModel>();
            foreach (SearchResult searchResult in response.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        if (!lista.Any(r=> r.YoutubeChannelId == searchResult.Snippet.ChannelId))
                        {
                            var canal = await CompleteData(searchResult);
                            if (canal != null)
                                lista.Add(canal);
                        }
                        lista.Add(new VideoSearchResultViewModel
                        {
                            YoutubeChannelId = searchResult.Snippet.ChannelId,
                            YoutubeVideoId = searchResult.Id.VideoId,
                            Descricao = searchResult.Snippet.Description,
                            Titulo = searchResult.Snippet.Title,
                            ImagemUrl = searchResult.Snippet.Thumbnails?.Default__?.Url.Replace("https", "http"),
                            PublicadoEm = searchResult.Snippet.PublishedAt 
                        });
                        break;

                    case "youtube#channel":
                        lista.Add(new CanalSearchResultViewModel
                        {
                            YoutubeChannelId = searchResult.Id.ChannelId,
                            Descricao = searchResult.Snippet.Description,
                            Titulo = searchResult.Snippet.Title,
                            ImagemUrl = searchResult.Snippet.Thumbnails?.Default__?.Url.Replace("https", "http"),
                            PublicadoEm = searchResult.Snippet.PublishedAt
                        });
                        break;
                }
            }

            return new YouTubeResponseViewModel()
            {
                NextPageToken = response.NextPageToken,
                PriorPageToken = response.PrevPageToken,
                Items = lista
            };
        }
    }
}
