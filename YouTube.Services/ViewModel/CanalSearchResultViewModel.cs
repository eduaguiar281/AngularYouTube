using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouTubeApp.Services.ViewModel
{
    public class CanalSearchResultViewModel: BaseSearchResultViewModel
    {
        public CanalSearchResultViewModel()
        {
            Tipo = ResultType.Canal;
            IncludeInList = true;
        }

        public bool IncludeInList { get; set; }
    }
}
