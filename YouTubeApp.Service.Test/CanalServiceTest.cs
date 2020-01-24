using MongoDB.Driver;
using MongoDB.Driver.Linq;
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
    public class CanalServiceTest
    {
        const string _CANAL_ID_PARAM_NAME = "id";
        const string _CANAL_LIST_PARAM_NAME = "canais";
        const string _CANAL_PREDICATE_PARAM_NAME = "predicate";

        private ICanalService _service;


        [SetUp]
        public void Setup()
        {
            var mockMongoCollection = new Mock<IMongoCollection<Canal>>().Object;
            var canal = new Canal() { ChannelId = "SomeValue", Title = "SomeValue", Descricao = "SomeValue" };
            var lista = new List<Canal>() { canal }.AsQueryable();

            Mock<IRepository<Canal>> mock = new Mock<IRepository<Canal>>();

            mock.Setup(r => r.Table).Returns(mockMongoCollection.AsQueryable());
            mock.Setup(r => r.GetSingleAsync()).Returns(Task.FromResult(canal));

            mock.Setup(r => r.GetSingleAsync(It.IsAny<Expression<Func<Canal, bool>>>()))
                .Returns((Expression<Func<Canal, bool>> predicate) => Task.FromResult(lista.FirstOrDefault(predicate)));

            _service = new CanalService(mock.Object);
        }

        #region GetCanalByYoutubeId

        [Test]
        public async Task GetCanalByYoutubeId_Id_Is_Null()
        {
            try
            {
                await _service.GetCanalByYoutubeId(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == _CANAL_ID_PARAM_NAME);
            }

        }
        #endregion

        #region SincronizarCanaisAsync
        [Test]
        public async Task SincronizarCanaisAsync_canais_Is_Null()
        {
            try
            {
                await _service.SincronizarCanaisAsync(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == _CANAL_LIST_PARAM_NAME);
            }
        }

        [Test]
        public async Task SincronizarCanaisAsync_canais_Is_Zero()
        {
            try
            {
                await _service.SincronizarCanaisAsync(new List<CanalSearchResultViewModel>());
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ParamName == _CANAL_LIST_PARAM_NAME);
            }
        }

        [TestCase(null, null, null)]
        [TestCase("channelIdSomeValue", null, null)]
        [TestCase("channelIdSomeValue", "tituloSomeValue", null)]
        public async Task SincronizarCanaisAsync_canais_Validation_Error(string channelId, string titulo, string descricao)
        {
            var canais = new List<CanalSearchResultViewModel>() { new CanalSearchResultViewModel() { YoutubeChannelId = channelId, Titulo = titulo, Descricao = descricao } };
            try
            {
                await _service.SincronizarCanaisAsync(canais);
                Assert.Fail();
            }
            catch (ServiceException)
            {
                Assert.Pass();
            }
        }

        [TestCase("channelIdSomeValue", "tituloSomeValue", "descricaoSomeValue")]
        [TestCase("SomeValue", "tituloSomeValue", "descricaoSomeValue")]
        public async Task SincronizarCanaisAsync_canais_Insert_Upadate_Success(string channelId, string titulo, string descricao)
        {
            var canais = new List<CanalSearchResultViewModel>() { new CanalSearchResultViewModel() { YoutubeChannelId = channelId, Titulo = titulo, Descricao = descricao } };
            await _service.SincronizarCanaisAsync(canais);
            Assert.Pass();
        }

        #endregion

        #region GetCanaisAsync    
        public async Task GetCanaisAsync_predicate_Is_Null()
        {
            try
            {
                await _service.GetCanaisAsync(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == _CANAL_PREDICATE_PARAM_NAME);
            }
        }

        #endregion

        #region GetCanalByIdAsync

        public async Task GetCanalByIdAsync_predicate_Is_Null()
        {
            try
            {
                await _service.GetCanalByIdAsync(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == _CANAL_ID_PARAM_NAME);
            }
        }

        #endregion
    }
}
