using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouTubeApp.Services.ViewModel
{
    public abstract class BaseSearchResultViewModel : ISearchResultViewModel
    {
        public string YoutubeVideoId { get; set; }
        public string YoutubeChannelId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string ImagemUrl { get; set; }
        public ResultType Tipo { get; protected set; }
        public DateTime? PublicadoEm { get; set; }
    }
}
