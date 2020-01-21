using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularSPA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YouTubeApp.Services;
using YouTubeApp.Services.ViewModel;

namespace YouTubeApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class YoutubeSearchController : ControllerBase
    {
        private int pageSize = 5;
        private readonly IYouTubeService _youTubeService;
        private readonly ICanalService _canalService;
        private readonly IVideoService _videoService;

        public YoutubeSearchController(IYouTubeService youTubeService, ICanalService canalService, IVideoService videoService)
        {
            _youTubeService = youTubeService;
            _canalService = canalService;
            _videoService = videoService;
        }

        // GET: api/YoutubeSearch
        [HttpGet]
        [Route("{query}/{pageToken}")]
        public async Task<IActionResult> Get(string query, string pageToken)
        {
            YouTubeResponseViewModel result = await _youTubeService.SearchAsync(query, pageToken, pageSize);
            await GravarHistoricoCanal(result);
            await GravarHistoricoVideos(result);
            return Ok(new ResponseRoot<YouTubeResponseViewModel>()
            {
                Data = result
            });
        }

        [HttpGet]
        [Route("{query}")]
        public async Task<IActionResult> Get(string query)
        {
            YouTubeResponseViewModel result =  await _youTubeService.SearchAsync(query, pageSize);
            await GravarHistoricoCanal(result);
            await GravarHistoricoVideos(result);
            return Ok(new ResponseRoot<YouTubeResponseViewModel>()
            {
                Data = result
            });
        }
        
        private async Task GravarHistoricoCanal(YouTubeResponseViewModel result)
        {
            var lista = result.Items
                .Where(w => w.Tipo == ResultType.Canal)
                .Select(s => (CanalSearchResultViewModel)s)
                .ToList();
            await _canalService.SincronizarCanaisAsync(lista);
            var listRemoved = lista.Where(x => x.IncludeInList == false);
            foreach (var item in listRemoved)
            {
                result.Items.Remove(item);
            }
        }

        private async Task GravarHistoricoVideos(YouTubeResponseViewModel result)
        {
            var lista = result.Items
                .Where(w => w.Tipo == ResultType.Video)
                .Select(s => s.YoutubeVideoId)
                .ToList();
            var videosDetails = await _youTubeService.GetVideosByIdsAsync(lista);
            await _videoService.SincronizarVideosAsync(videosDetails);

        }
    }
}
