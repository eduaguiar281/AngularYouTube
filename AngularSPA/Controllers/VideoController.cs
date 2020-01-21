using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularSPA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YouTubeApp.Core.Infrastructure;
using YouTubeApp.Core.Models;
using YouTubeApp.Services;

namespace YouTubeApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;
        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        // GET: api/Video
        [HttpGet]
        public async Task<IActionResult> Get(int? pageSize, int? pageIndex)
        {
            if (pageSize == null)
            {
                var result = new ResponseRoot<IList<Video>>
                {
                    Data = await _videoService.GetVideosAsync(),
                    Success = true
                };
                return Ok(result);
            }
            else
            {
                if (pageIndex == null)
                    pageIndex = 0;
                var query = _videoService.GetQuery();
                var pagedList = await PagedList<Video>.Create(query, pageIndex.Value, pageSize.Value);
                var result = new ResponseRoot<PagedList<Video>>
                {
                    Data = pagedList,
                    PageIndex = pagedList.PageIndex,
                    PageSize = pagedList.PageSize,
                    TotalCount = pagedList.TotalCount,
                    TotalPages = pagedList.TotalPages,
                    Success = true

                };
                return Ok(result);
            }
        }

        // GET: api/Video/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = new ResponseRoot<Video>
            {
                Data = await _videoService.GetVideoByIdAsync(id),
                Success = true

            };
            return Ok(result);
        }
    }
}
