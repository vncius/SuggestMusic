using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SuggestMusic.API.Controllers;
using SuggestMusic.Service.OpenWeather;
using SuggestMusic.Service.Spotify;
using SuggestMusic.UnitTests.Helpers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SuggestMusic.UnitTests
{
    [TestFixture]
    public class TestCaseSuggestMusic
    {
        #region PROPERTIES

        private IHttpClientFactory _httpClientFactory;
        private SpotifyController _controllerSpotify;
        private SpotifyService _spotifyService;
        private OpenWeatherService _openWeatherService;
        private IConfiguration _configuration;

        #endregion PROPERTIES

        #region PUBLIC METHODS

        [SetUp]
        public void Setup()
        {
            var logger = HelperTests.GetMockLogger<SpotifyController>();

            _configuration = GetMemoryConfig();
            _httpClientFactory = HelperTests.GetMockHttpClientFactory();
            _spotifyService = new SpotifyService(_httpClientFactory, _configuration);
            _openWeatherService = new OpenWeatherService(_httpClientFactory, _configuration);
            _controllerSpotify = new SpotifyController(_spotifyService, _openWeatherService, logger);
        }

        [Test]
        public async Task CaseGetPlayList()
        {
            var result = await _controllerSpotify.GetCityPlaylist("Goiânia");

            Assert.IsNotNull(result, "Falha ao obter retorno da API");
            Assert.IsTrue(result is OkObjectResult, "Retorno da API é diferente de 200 OK");

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult, "O resultado retornado é nulo");
            Assert.IsNotNull(okResult.Value, "O valor retornado para playlists é nulo");
        }

        [Test]
        public async Task CaseGetTheCoordinatesPlaylist()
        {
            var result = await _controllerSpotify.GetThecoordinatesPlaylist(-16.6786, -49.2539);

            Assert.IsNotNull(result, "Falha ao obter retorno da API");
            Assert.IsTrue(result is OkObjectResult, "Retorno da API é diferente de 200 OK");

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult, "O resultado retornado é nulo");
            Assert.IsNotNull(okResult.Value, "O valor retornado para playlists é nulo");
        }

        [Test]
        public async Task CaseGetRecomendations()
        {
            var temp = await _spotifyService.GetTracks("pop");

            if (temp.Playlist == null)
            {
                Assert.Fail("O Objeto playlist retornado pelo SpotifyService é nulo");
            }

            Assert.IsFalse(temp.Playlist.Length <= 0, "Nenhuma musica foi retornada pelo SpotifyService");
        }

        [Test]
        public async Task CaseGetTemperatureByCoordinates()
        {
            var temp = await _openWeatherService.GetTemperature(-16.6786, -49.2539);

            Assert.IsFalse(temp < -90 || temp > 70, "A temperatura retornada pelo serviço é inválida");
        }

        [Test]
        public async Task CaseGetTemperatureByCity()
        {
            var temp = await _openWeatherService.GetTemperature("Goiânia");

            Assert.IsFalse(temp == -273.15, "Não foi obtido a tempetura da cidade passada por parâmetro");
        }

        #endregion PUBLIC METHODS

        #region PRIVATE METHODS

        /// <summary>
        /// Simula a configuração do arquivo appsettings.json
        /// </summary>
        /// <returns>Retorna as configurações necessária para execução dos testes</returns>
        private IConfiguration GetMemoryConfig()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"ApiOpenWeather:UrlAPI", "https://api.openweathermap.org/data/2.5"},
                {"ApiOpenWeather:ClientId", "eca2468742496c6bbe593d5ae121b0fd"},
                {"ApiSpotify:UrlAPI", "https://api.spotify.com/v1"},
                {"ApiSpotify:UrlGetToken", "https://accounts.spotify.com/api"},
                {"ApiSpotify:ClientId", "ab53b7bacd3b43cfb8626a0f2095922b"},
                {"ApiSpotify:ClientSecret", "030b138df827455a855d604fa147bbd6"}
            };

            return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        }

        #endregion
    }
}