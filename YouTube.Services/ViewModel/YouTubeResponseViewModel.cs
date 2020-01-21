﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouTubeApp.Services.ViewModel
{
    public class YouTubeResponseViewModel  
    {
        public IList<ISearchResultViewModel> Items { get; set; }

        public string Query { get; set; }
        
        public string NextPageToken { get; set; }

        public string PriorPageToken { get; set; }
    }
}
