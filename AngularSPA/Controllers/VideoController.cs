using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularSPA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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
        public async Task<IActionResult> Get(int? pageSize, int? pageIndex, string queryString)
        {
            if (pageSize == null)
            {
                IList<Video> list = null;
                if (string.IsNullOrEmpty(queryString))
                    list = await _videoService.GetVideosAsync();
                else
                    list = await _videoService.GetVideosAsync(w => w.Titulo.Contains(queryString));

                var result = new ResponseRoot<IList<Video>>
                {
                    Data = list,
                    Success = true
                };
                return Ok(result);
            }
            else
            {
                if (pageIndex == null)
                    pageIndex = 0;

                IMongoQueryable<Video> query = null;
                ResponseRoot<PagedList<Video>> result = null;
                if (string.IsNullOrEmpty(queryString))
                {
                    query = _videoService.GetQuery();
                    var pagedList = await PagedList<Video>.Create(query, pageIndex.Value, pageSize.Value);
                    result = new ResponseRoot<PagedList<Video>>
                    {
                        Data = pagedList,
                        PageIndex = pagedList.PageIndex,
                        PageSize = pagedList.PageSize,
                        TotalCount = pagedList.TotalCount,
                        TotalPages = pagedList.TotalPages,
                        Success = true
                    };
                }
                else
                {
                    var builder = Builders<Video>.Filter;
                    var builderSort = Builders<Video>.Sort;
                    var filter = builder.Where(x => x.Titulo.Contains(queryString));
                    var sort = builderSort.Ascending(f => f.Titulo);
                    var pagedList = await PagedList<Video>.Create(_videoService.GetCollection(), filter, sort, pageIndex.Value, pageSize.Value);
                    result = new ResponseRoot<PagedList<Video>>
                    {
                        Data = pagedList,
                        PageIndex = pagedList.PageIndex,
                        PageSize = pagedList.PageSize,
                        TotalCount = pagedList.TotalCount,
                        TotalPages = pagedList.TotalPages,
                        Success = true
                    };
                }
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
