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

namespace AngularSPA.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CanalController : ControllerBase
    {
        private readonly ICanalService _canalService; 
        public CanalController(ICanalService canalService)
        {
            _canalService = canalService;
        }

        // GET: api/Canal
        [HttpGet]
        public async Task<IActionResult> Get(int? pageSize, int? pageIndex, string queryString)
        {
            if (pageSize == null)
            {
                IList<Canal> list = null;
                if (string.IsNullOrEmpty(queryString))
                    list = await _canalService.GetCanaisAsync();
                else
                    list = await _canalService.GetCanaisAsync(w => w.Title.Contains(queryString));
                var result = new ResponseRoot<IList<Canal>>
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
                IMongoQueryable<Canal> query = null;
                ResponseRoot<PagedList<Canal>> result = null;
                if (string.IsNullOrEmpty(queryString))
                {
                    query = _canalService.GetQuery();
                    var pagedList = await PagedList<Canal>.Create(query, pageIndex.Value, pageSize.Value);
                    result = new ResponseRoot<PagedList<Canal>>
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
                    var builder = Builders<Canal>.Filter;
                    var builderSort = Builders<Canal>.Sort;
                    var filter = builder.Where(x => x.Title.Contains(queryString));
                    var sort = builderSort.Ascending(f => f.Title);
                    var pagedList = await PagedList<Canal>.Create(_canalService.GetCollection(), filter,sort, pageIndex.Value, pageSize.Value);
                    result = new ResponseRoot<PagedList<Canal>>
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

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(string id)
        {
            var result = new ResponseRoot<Canal>
            {
                Data = await _canalService.GetCanalByIdAsync(id),
                Success = true

            };
            return Ok(result);
        }
    }
}
