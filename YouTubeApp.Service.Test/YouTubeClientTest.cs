using Google;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YouTubeApp.Services;
using YouTubeApp.Services.ViewModel;

namespace YouTubeApp.Service.Test
{
    [TestFixture]
    public class YouTubeClientTest
    {
        private IYouTubeService _youtubeService;
        private IYouTubeService _youtubeClientComChaveErrada;

        const string _SEARCH_QUERY = "Pop Musics";
        const string _QUERY_PARAM_NAME = "query";
        const string _MAXRESULTS_PARAM_NAME = "maxResults";
        const string _VIDEOS_IDS_PARAM_NAME = "ids";

        [SetUp]
        public void Setup()
        {
            var configuration = new Mock<IConfiguration>();

            var apiKeySection = new Mock<IConfigurationSection>();
            apiKeySection.Setup(a => a.Value).Returns("AIzaSyAy_0a0bVHWj60mHEmPIK-lS_Ip0dAmQcE");

            var clientApplicationNameSection = new Mock<IConfigurationSection>();
            clientApplicationNameSection.Setup(a => a.Value).Returns("YouTubeApp");

            var youTubeConfigSection = new Mock<IConfigurationSection>();
            youTubeConfigSection.Setup(a => a.GetSection("APIKEY")).Returns(apiKeySection.Object);
            youTubeConfigSection.Setup(a => a.GetSection("ClientApplicationName")).Returns(clientApplicationNameSection.Object);

            configuration.Setup(a => a.GetSection("YouTubeConfig")).Returns(youTubeConfigSection.Object);

            _youtubeService = new YouTubeService(configuration.Object);
            
            apiKeySection.Setup(a => a.Value).Returns("CHAVE_ERRADA");
            youTubeConfigSection.Setup(a => a.GetSection("APIKEY")).Returns(apiKeySection.Object);
            configuration.Setup(a => a.GetSection("YouTubeConfig")).Returns(youTubeConfigSection.Object);
            _youtubeClientComChaveErrada = new YouTubeService(configuration.Object);
        }

        #region SearchAsync()

        [Test]
        public async Task SearchAsync_Query_Is_Null_Test()
        {
            try
            {
                await _youtubeService.SearchAsync(null, 10);
                Assert.Fail();
            }
            catch(ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == _QUERY_PARAM_NAME);
            }
        }

        [Test]
        public async Task SearchAsync_MaxResults_Is_Zero_Test()
        {
            try
            {
                await _youtubeService.SearchAsync(_SEARCH_QUERY, 0);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ParamName == _MAXRESULTS_PARAM_NAME);
            }
        }

        [Test]
        public async Task SearchAsync_Falha_Requisicao()
        {
            YouTubeResponseViewModel result = await _youtubeClientComChaveErrada.SearchAsync(_SEARCH_QUERY, 5);
            Assert.AreEqual(false, result.IsSuccess);
        }

        [Test]
        public async Task SearchAsync_Paging_Query_Is_Null_Test()
        {
            try
            {
                await _youtubeService.SearchAsync(null, "SomeValue", 10);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == _QUERY_PARAM_NAME);
            }
        }

        [Test]
        public async Task SearchAsync_Paging_MaxResults_Is_Zero_Test()
        {
            try
            {
                await _youtubeService.SearchAsync(_SEARCH_QUERY, "SomeValue", 0);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ParamName == _MAXRESULTS_PARAM_NAME);
            }
        }

        [Test]
        public async Task SearchAsync_Paging_Falha_Requisicao()
        {
            YouTubeResponseViewModel result = await _youtubeClientComChaveErrada.SearchAsync(_SEARCH_QUERY, "SomeValue", 5);
            Assert.AreEqual(false, result.IsSuccess);
        }

        #endregion

        #region GetVideoListAsync

        [Test]
        public async Task GetVideoListAsync_VideoIds_Is_Null()
        {
            try
            {
                await _youtubeService.GetVideosByIdsAsync(null);
                Assert.Fail();
            }
            catch(ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == _VIDEOS_IDS_PARAM_NAME);
            }
        }

        [Test]
        public async Task GetVideoListAsync_VideoIds_Sem_elementos()
        {
            var ids = new List<string>();
            try
            {
                await _youtubeService.GetVideosByIdsAsync(ids);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ParamName == _VIDEOS_IDS_PARAM_NAME);
            }
        }

        [Test]
        public async Task GetVideoListAsync_Falha_Requisicao()
        {
            try
            {
                await _youtubeClientComChaveErrada.GetVideosByIdsAsync(new List<string> { "PYRbC7vDxio", "jOx0JEC_3P4" });
                Assert.Fail();
            }
            catch (GoogleApiException)
            {
                Assert.Pass();
            }
        }

        #endregion

    }
}
