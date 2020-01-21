using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouTubeApp.Core.Models
{
    public class Video: BaseEntity
    {
        public string VideoId { get; set; }
        public string CanalId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime? PublicadoEm { get; set; }
        public string Idioma { get; set; }
        public string ImagemUrl { get; set; }
        public string Definicao { get; set; }
        public ulong? QuantidadeLike { get; set; }
        public ulong? QuantidadeDeslike { get; set; }
        public ulong? QuantidadeComentario { get; set; }
        public ulong? QuantidadeVisualizacao { get; set; }

    }
}
