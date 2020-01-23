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

namespace YouTubeApp.Service.Test
{
    [TestFixture]
    public class YouTubeClientTest
    {
        private IYouTubeClient _youtubeClient;
        private IYouTubeClient _youtubeClientComChaveErrada;

        const string _SEARCH_QUERY = "Pop Musics";
        const string _QUERY_PARAM_NAME = "query";
        const string _MAXRESULTS_PARAM_NAME = "maxResults";
        const string _VIDEOS_IDS_PARAM_NAME = "videosIds";
        const string _CHANNEL_IDS_PARAM_NAME = "channelIds";

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

            _youtubeClient = new YouTubeClient(configuration.Object);
            
            apiKeySection.Setup(a => a.Value).Returns("CHAVE_ERRADA");
            youTubeConfigSection.Setup(a => a.GetSection("APIKEY")).Returns(apiKeySection.Object);
            configuration.Setup(a => a.GetSection("YouTubeConfig")).Returns(youTubeConfigSection.Object);
            _youtubeClientComChaveErrada = new YouTubeClient(configuration.Object);
        }

        #region SearchAsync()

        [Test]
        public async Task SearchAsync_Query_Is_Null_Test()
        {
            try
            {
                await _youtubeClient.SearchAsync(null, 10);
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
                await _youtubeClient.SearchAsync(_SEARCH_QUERY, 0);
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
            try
            {
                await _youtubeClientComChaveErrada.SearchAsync(_SEARCH_QUERY, 5);
                Assert.Fail();
            }
            catch (GoogleApiException)
            {

                Assert.Pass();
            }
        }

        [Test]
        public async Task SearchAsync_Paging_Query_Is_Null_Test()
        {
            try
            {
                await _youtubeClient.SearchAsync(null, "CSDFEX", 10);
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
                await _youtubeClient.SearchAsync(_SEARCH_QUERY, "CSDFEX", 0);
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
            try
            {
                await _youtubeClientComChaveErrada.SearchAsync(_SEARCH_QUERY, "CSDFEX", 5);
                Assert.Fail();
            }
            catch (GoogleApiException)
            {

                Assert.Pass();
            }
        }

        #endregion

        #region GetVideoListAsync

        [Test]
        public async Task GetVideoListAsync_VideoIds_Is_Null()
        {
            try
            {
                await _youtubeClient.GetVideoListAsync(null);
                Assert.Fail();
            }
            catch(ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == _VIDEOS_IDS_PARAM_NAME);
            }
        }

        [Test]
        public async Task GetVideoListAsync_Falha_Requisicao()
        {
            try
            {
                await _youtubeClientComChaveErrada.GetVideoListAsync("PYRbC7vDxio, jOx0JEC_3P4");
                Assert.Fail();
            }
            catch (GoogleApiException)
            {
                Assert.Pass();
            }
        }

        #endregion

        #region GetChannelListAsync

        [Test]
        public async Task GetChannelListAsync_ChannelIds_Is_Null()
        {
            try
            {
                await _youtubeClient.GetChannelListAsync(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == _CHANNEL_IDS_PARAM_NAME);
            }
        }

        [Test]
        public async Task GetChannelListAsync_Falha_Requisicao()
        {
            try
            {
                await _youtubeClientComChaveErrada.GetChannelListAsync("UCmqofm6gSYnyOG1KLRDLULA, UCJB5n0L-Iu8OGwh1anOt8Uw");
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
