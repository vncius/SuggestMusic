using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuggestMusic.Domain.Convertions;
using SuggestMusic.Domain.Model;
using SuggestMusic.Interfaces.OpenWeather;
using SuggestMusic.Interfaces.Spotify;
using System.Threading.Tasks;

namespace SuggestMusic.API.Controllers
{
    /// <summary>
    /// Controller da API, responsável por integrar com o Spotify e OpenWeather
    /// para retornar uma playlist de músicas recomendadas
    /// </summary>
    [ApiController]
    [Route("api/suggest")]
    public class SpotifyController : ControllerBase
    {
        private readonly ILogger<SpotifyController> _logger;
        private readonly ISpotifyService _spotifyService;
        private readonly IOpenWeatherService _openWeatherService;

        public SpotifyController(ISpotifyService spotifyService, IOpenWeatherService openWeatherService, ILogger<SpotifyController> logger)
        {
            _openWeatherService = openWeatherService;
            _spotifyService = spotifyService;
            _logger = logger;
        }

        /// <summary>
        /// Obtém músicas de acordo com a temperatura da latitude e longitude informada
        /// </summary>
        /// <param name="latitude">Latitude do usuário</param>
        /// <param name="longitude">Longitude do usuário</param>
        /// <returns>Retorno no formato de Json contendo uma lista de string (Músicas)</returns>
        [HttpGet]
        [Route("{latitude?}/{longitude?}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ModelPlaylist))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetThecoordinatesPlaylist(double latitude, double longitude)
        {
            _logger.LogInformation($"Executando api/suggest/latitude/longitude -> GetThecoordinatesPlaylist({latitude},{longitude})");
            var temperature = await _openWeatherService.GetTemperature(latitude, longitude);
            var styleMusical = Convertions.ConvertCelsiusToMusicStyle(temperature);
            var playlists = await _spotifyService.GetTracks(styleMusical);
            return Ok(playlists);
        }

        /// <summary>
        /// Obtém playlist de acordo com a temperatura da cidade informada
        /// </summary>
        /// <param name="city">Cidade usada para buscar temperatura</param>
        /// <returns>Retorno no formato de Json contendo uma lista de string (Músicas)</returns>
        [HttpGet]
        [Route("{city?}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ModelPlaylist))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCityPlaylist(string city)
        {
            _logger.LogInformation($"Executando api/suggest/city -> GetCityPlaylist({city})");
            var temperature = await _openWeatherService.GetTemperature(city);
            var styleMusical = Convertions.ConvertCelsiusToMusicStyle(temperature);
            var playlists = await _spotifyService.GetTracks(styleMusical);
            return Ok(playlists);
        }
    }
}
