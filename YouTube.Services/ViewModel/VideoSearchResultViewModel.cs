using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouTubeApp.Services.ViewModel
{
    public class VideoSearchResultViewModel :BaseSearchResultViewModel
    {
        public VideoSearchResultViewModel()
        {
            Tipo = ResultType.Video;
        }

        public string Idioma { get; set; }
        public string Definicao { get; set; }
        public ulong? QuantidadeLike { get; set; }
        public ulong? QuantidadeDeslike { get; set; }
        public ulong? QuantidadeComentario { get; set; }
        public ulong? QuantidadeVisualizacao { get; set; }
    }
}
