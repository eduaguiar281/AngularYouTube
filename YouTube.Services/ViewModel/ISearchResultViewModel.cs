using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouTubeApp.Services.ViewModel
{
    public enum ResultType { Video, Canal }
    public interface ISearchResultViewModel
    {
        string YoutubeVideoId { get; set; }
        string YoutubeChannelId { get; set; }
        string Titulo { get; set; }
        string Descricao { get; set; }
        string ImagemUrl { get; set; }
        ResultType Tipo { get; }
        DateTime? PublicadoEm { get; set; }
    }
}
