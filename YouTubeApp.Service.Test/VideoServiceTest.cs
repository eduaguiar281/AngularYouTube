using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YouTube.Services;
using YouTubeApp.Core.Models;
using YouTubeApp.Data;
using YouTubeApp.Services;
using YouTubeApp.Services.ViewModel;

namespace YouTubeApp.Service.Test
{
    [TestFixture]
    public class VideoServiceTest
    {
        const string _VIDEO_ID_PARAM_NAME = "id";
        const string _VIDEO_LIST_PARAM_NAME = "videos";
        const string _VIDEO_PREDICATE_PARAM_NAME = "predicate";


        private IVideoService _service;
        
        [SetUp]
        public void Setup()
        {
            var mockCanal = new Mock<ICanalService>();
            mockCanal.Setup(x => x.GetCanalByYoutubeId(It.IsAny<string>()))
                .Returns(Task.FromResult(new Canal 
                                        { 
                                           Id = "CanalIdSomeValue", 
                                           ChannelId = "ChannelIdSomeValue", 
                                           Title = "TitleSomeValue", 
                                           Descricao = "DescricaoSomeValue" 
                                        }));

            mockCanal.Setup(x => x.GetCanalByYoutubeId(It.Is<string>(s => s == "IncorrectYoutubeChannelId")))
                .Returns(Task.FromResult(default(Canal)));

            ICanalService canalService = mockCanal.Object;

            Video video = new Video
            {
                Id = "VideoIdSomeValue",
                VideoId = "YoutubeVideoIdSomeValue",
                CanalId = "CanalIdSomeValue",
                Descricao = "DescricaoSomeValue",
                Titulo = "TituloSomeValue",

            };
            IQueryable<Video> listaVideo = new List<Video>() { video }.AsQueryable();

            var mockRepository = new Mock<IRepository<Video>>();
            mockRepository.Setup(r => r.GetSingleAsync(It.IsAny<Expression<Func<Video, bool>>>()))
                .Returns((Expression<Func<Video, bool>> predicate) 
                          => Task.FromResult(listaVideo.FirstOrDefault(predicate)));
            _service = new VideoService(mockRepository.Object, canalService);
        }


        #region SincronizrVideosAsync
        [Test]
        public async Task SincronizarVideosAsync_videos_Is_Null()
        {
            try
            {
                await _service.SincronizarVideosAsync(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == _VIDEO_LIST_PARAM_NAME);
            }

        }

        [Test]
        public async Task SincronizarVideosAsync_videos_Eq_Zero()
        {
            try
            {
                await _service.SincronizarVideosAsync(new List<VideoSearchResultViewModel>());
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ParamName == _VIDEO_LIST_PARAM_NAME);
            }

        }

        [TestCase(null, null, null, null)]
        [TestCase("YouTubeVideoIdSomeValue", null, null, null)]
        [TestCase("YouTubeVideoIdSomeValue", "YoutubeChannelId", null, null)]
        [TestCase("YouTubeVideoIdSomeValue", "YoutubeChannelId", "TituloSomeValue", null)]
        [TestCase("YouTubeVideoIdSomeValue", "IncorrectYoutubeChannelId", "TituloSomeValue", "DescricaoSomeValue")]
        public async Task SincronizarVideosAsync_videos_Validation_Errors(string youTubeVideoId, string youtubeChannelId, string titulo, string descricao)
        {
            try
            {
                var lista = new List<VideoSearchResultViewModel>()
                {
                    new VideoSearchResultViewModel
                    {
                        YoutubeVideoId = youTubeVideoId,
                        YoutubeChannelId = youtubeChannelId,
                        Titulo = titulo,
                        Descricao = descricao
                    }
                };
                await _service.SincronizarVideosAsync(lista);
                Assert.Fail();
            }
            catch (ServiceException)
            {
                Assert.Pass();
            }

        }


        [TestCase("YouTubeVideoIdNewSomeValue", "ChannelIdSomeValue", "TituloSomeValue", "DescricaoSomeValue")]
        [TestCase("YoutubeVideoIdSomeValue", "ChannelIdSomeValue", "TituloSomeValue", "DescricaoSomeValue")]
        public async Task SincronizarVideosAsync_videos_Insert_Update_Success(string youTubeVideoId, string youtubeChannelId, string titulo, string descricao)
        {
            var lista = new List<VideoSearchResultViewModel>()
                {
                    new VideoSearchResultViewModel
                    {
                        YoutubeVideoId = youTubeVideoId,
                        YoutubeChannelId = youtubeChannelId,
                        Titulo = titulo,
                        Descricao = descricao
                    }
                };
            await _service.SincronizarVideosAsync(lista);
            Assert.Pass();

        }

        #endregion

        #region GetVideoByYoutubeId

        [Test]
        public async Task GetVideoByYoutubeId_Id_Is_Null()
        {
            try
            {
                await _service.GetVideoByYoutubeId(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == _VIDEO_ID_PARAM_NAME);
            }

        }

        #endregion

        #region GetVideosAsync
        [Test]
        public async Task GetVideosAsync_Predicate_Is_Null()
        {
            try
            {
                await _service.GetVideosAsync(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == _VIDEO_PREDICATE_PARAM_NAME);
            }
        }

        #endregion

        #region GetVideoByIdAsync
        [Test]
        public async Task GetVideoByIdAsync_Id_Is_Null()
        {
            try
            {
                await _service.GetVideoByIdAsync(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == _VIDEO_ID_PARAM_NAME);
            }
        }

        #endregion

    }
}
