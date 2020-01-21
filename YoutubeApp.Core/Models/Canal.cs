using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouTubeApp.Core.Models
{
    public class Canal: BaseEntity
    {
        public string ChannelId { get; set; }
        public string Title { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public DateTime? PublicadoEm { get; set; }
    }
}
