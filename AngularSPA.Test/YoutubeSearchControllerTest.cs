using AngularSPA.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YouTube.Services;
using YouTubeApp.Controllers;
using YouTubeApp.Services;
using YouTubeApp.Services.ViewModel;

namespace AngularSPA.Test
{
    [TestFixture]
    public class YoutubeSearchControllerTest
    {

        enum TestScope { GoogleApiNotResponseScope, ArgumentNullExcepionScope, ArgumentExceptionScope, ServiceExceptionScope };
        private YoutubeSearchController GetController(TestScope scope)
        {
            switch(scope)
            {
                case TestScope.GoogleApiNotResponseScope:
                    return new YoutubeSearchController(GetMockYoutubeServiceError(), null, null);
                case TestScope.ArgumentNullExcepionScope:
                    return new YoutubeSearchController(GetMockYoutubeService(), GetMockCanalServiceArgumentNull(), null);
                case TestScope.ArgumentExceptionScope:
                    return new YoutubeSearchController(GetMockYoutubeService(), GetMockCanalServiceArgument(), null);
                case TestScope.ServiceExceptionScope:
                    return new YoutubeSearchController(GetMockYoutubeService(), GetMockCanalServiceException(), null);
                default:
                    return new YoutubeSearchController(GetMockYoutubeServiceError(), null, null);
            }
        }

        #region MockServices

        private IYouTubeService GetMockYoutubeServiceError()
        {
            var result = new Mock<IYouTubeService>();
            result.Setup(x => x.SearchAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new YouTubeResponseViewModel { IsSuccess = false }));
            result.Setup(x => x.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new YouTubeResponseViewModel { IsSuccess = false }));

            return result.Object;
        }

        private IYouTubeService GetMockYoutubeService()
        {
            var result = new Mock<IYouTubeService>();

            result.Setup(x => x.SearchAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new YouTubeResponseViewModel { IsSuccess = true, Items = new List<ISearchResultViewModel>() }));
            result.Setup(x => x.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new YouTubeResponseViewModel { IsSuccess = true, Items = new List<ISearchResultViewModel>() }));

            return result.Object;
        }

        private IYouTubeService GetMockYoutubeServiceSuccess()
        {
            var lista = new List<ISearchResultViewModel>
            {
                new VideoSearchResultViewModel
                {
                    YoutubeChannelId = "SomeCahnnel1",
                    YoutubeVideoId = "SomeVideo1",
                    Idioma = "pt",
                    Definicao =  "4k",
                    Titulo = "SomeTitulo1",
                    Descricao = "SomeDescricao1",
                    ImagemUrl = "SomeUrl1",
                    PublicadoEm = DateTime.Now,
                    QuantidadeComentario = 100,
                    QuantidadeDeslike = 1,
                    QuantidadeLike = 1000,
                    QuantidadeVisualizacao = 5600
                },
                new VideoSearchResultViewModel
                {
                    YoutubeChannelId = "SomeCahnnel3",
                    YoutubeVideoId = "SomeVideo3",
                    Idioma = "pt",
                    Definicao =  "4k",
                    Titulo = "SomeTitulo3",
                    Descricao = "SomeDescricao3",
                    ImagemUrl = "SomeUrl3",
                    PublicadoEm = DateTime.Now,
                    QuantidadeComentario = 100,
                    QuantidadeDeslike = 1,
                    QuantidadeLike = 1000,
                    QuantidadeVisualizacao = 5600
                },
                new VideoSearchResultViewModel
                {
                    YoutubeChannelId = "SomeCahnnel2",
                    YoutubeVideoId = "SomeVideo2",
                    Idioma = "pt",
                    Definicao =  "4k",
                    Titulo = "SomeTitulo2",
                    Descricao = "SomeDescricao2",
                    ImagemUrl = "SomeUrl2",
                    PublicadoEm = DateTime.Now,
                    QuantidadeComentario = 100,
                    QuantidadeDeslike = 1,
                    QuantidadeLike = 1000,
                    QuantidadeVisualizacao = 5600
                },

                new CanalSearchResultViewModel
                {
                    YoutubeChannelId = "SomeChannel1",
                    ImagemUrl = "SomeUrl1",
                    Titulo = "SomeTitulo1",
                    Descricao = "SomeDescricao1",
                    IncludeInList = true,
                    PublicadoEm = DateTime.Now
                },
                new CanalSearchResultViewModel
                {
                    YoutubeChannelId = "SomeChannel2",
                    ImagemUrl = "SomeUrl2",
                    Titulo = "SomeTitulo2",
                    Descricao = "SomeDescricao2",
                    IncludeInList = true,
                    PublicadoEm = DateTime.Now
                },
                new CanalSearchResultViewModel
                {
                    YoutubeChannelId = "SomeChannel3",
                    ImagemUrl = "SomeUrl3",
                    Titulo = "SomeTitulo3",
                    Descricao = "SomeDescricao3",
                    IncludeInList = false,
                    PublicadoEm = DateTime.Now
                },
            };

            var result = new Mock<IYouTubeService>();

            result.Setup(x => x.SearchAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new YouTubeResponseViewModel { IsSuccess = true, Items = lista }));
            result.Setup(x => x.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new YouTubeResponseViewModel { IsSuccess = true, Items = lista }));

            return result.Object;
        }


        private ICanalService GetMockCanalServiceArgumentNull()
        {
            var result = new Mock<ICanalService>();
            result.Setup(x => x.SincronizarCanaisAsync(It.IsAny<List<CanalSearchResultViewModel>>()))
                .Throws(new ArgumentNullException(null, "ArgumentNullException"));
            return result.Object;
        }

        private ICanalService GetMockCanalServiceArgument()
        {
            var result = new Mock<ICanalService>();
            result.Setup(x => x.SincronizarCanaisAsync(It.IsAny<List<CanalSearchResultViewModel>>()))
                .Throws(new ArgumentException("ArgumentException", paramName: null));
            return result.Object;
        }

        private ICanalService GetMockCanalServiceException()
        {
            var result = new Mock<ICanalService>();
            result.Setup(x => x.SincronizarCanaisAsync(It.IsAny<List<CanalSearchResultViewModel>>()))
                .Throws(new ServiceException("ServiceException"));
            return result.Object;
        }

        private ICanalService GetMockCanalServiceSuccess()
        {
            var result = new Mock<ICanalService>();
            result.Setup(x => x.SincronizarCanaisAsync(It.IsAny<List<CanalSearchResultViewModel>>()));
            return result.Object;
        }
        private IVideoService GetMockVideoServiceSuccess()
        {
            var result = new Mock<IVideoService>();
            result.Setup(x => x.SincronizarCanaisAsync(It.IsAny<List<CanalSearchResultViewModel>>()))
                .Throws(new ServiceException("ServiceException"));
            return result.Object;
        }
        #endregion

        [SetUp]
        public void Setup()
        {
            
        }

        [TestCase("queryError", null)]
        [TestCase("queryError", "XYZ")]
        public async Task Get_Youtube_Service_Not_Response(string query, string pageToken)
        {
            var controller = GetController(TestScope.GoogleApiNotResponseScope);
            IActionResult result = null;
            if (string.IsNullOrEmpty(pageToken))
                result = await controller.Get(query);
            else
                result = await controller.Get(query, pageToken);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase("query", null)]
        [TestCase("query", "XYZ")]
        public async Task Get_ArgumentNullException(string query, string pageToken)
        {
            var controller = GetController(TestScope.ArgumentNullExcepionScope);
            IActionResult result;
            if (string.IsNullOrEmpty(pageToken))
                result = await controller.Get(query);
            else
                result = await controller.Get(query, pageToken);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var resposta = (result as BadRequestObjectResult);
            if (resposta == null)
                return;
            Assert.IsInstanceOf<ResponseRoot<YouTubeResponseViewModel>>(resposta.Value);
            var value = (resposta.Value as ResponseRoot<YouTubeResponseViewModel>);
            if (value == null)
                return;
            Assert.AreEqual("ArgumentNullException", value.Message);
        }

        [TestCase("query", null)]
        [TestCase("query", "XYZ")]
        public async Task Get_ArgumentException(string query, string pageToken)
        {
            var controller = GetController(TestScope.ArgumentExceptionScope);
            IActionResult result;
            if (string.IsNullOrEmpty(pageToken))
                result = await controller.Get(query);
            else
                result = await controller.Get(query, pageToken);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var resposta = (result as BadRequestObjectResult);
            if (resposta == null)
                return;
            Assert.IsInstanceOf<ResponseRoot<YouTubeResponseViewModel>>(resposta.Value);
            var value = (resposta.Value as ResponseRoot<YouTubeResponseViewModel>);
            if (value == null)
                return;
            Assert.AreEqual("ArgumentException", value.Message);
        }

        [TestCase("query", null)]
        [TestCase("query", "XYZ")]
        public async Task Get_ServiceException(string query, string pageToken)
        {
            var controller = GetController(TestScope.ServiceExceptionScope);
            IActionResult result;
            if (string.IsNullOrEmpty(pageToken))
                result = await controller.Get(query);
            else
                result = await controller.Get(query, pageToken);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var resposta = (result as BadRequestObjectResult);
            if (resposta == null)
                return;
            Assert.IsInstanceOf<ResponseRoot<YouTubeResponseViewModel>>(resposta.Value);
            var value = (resposta.Value as ResponseRoot<YouTubeResponseViewModel>);
            if (value == null)
                return;
            Assert.AreEqual("ServiceException", value.Message);
        }

    }
}