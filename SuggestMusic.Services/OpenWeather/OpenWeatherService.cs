using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SuggestMusic.Domain.Convertions;
using SuggestMusic.Domain.DTO.OpenWeather;
using SuggestMusic.Infrastructure.Exceptions.HttpExceptions;
using SuggestMusic.Interfaces.Helpers;
using SuggestMusic.Interfaces.OpenWeather;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SuggestMusic.Service.OpenWeather
{
    /// <summary>
    /// Classe de serviço referente ao consumo da API (OpenWeather)
    /// </summary>
    public class OpenWeatherService : IOpenWeatherService
    {
        #region PROPERTIES

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        private string _urlAPI;
        private string _clientId;

        #endregion PROPERTIES

        #region CONSTRUCTOR

        /// <summary>
        /// Construtor da classe OpenWeatherService
        /// </summary>
        /// <param name="clientFactory">Injeção de dependência para IHttpClientFactory</param>
        /// <param name="configuration">Injeção de dependência para IConfiguration</param>
        /// <returns>Objeto de retorno da API, contém uma lista de string (Músicas)</returns>
        public OpenWeatherService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            ConfigureService();

        }

        #endregion CONSTRUCTOR

        #region PUBLIC METHODS

        /// <summary>
        /// Obtém a temperatura atual de uma cidade consumindo a API OpenWeather
        /// </summary>
        /// <param name="latitude">Latitude da localização desejada</param>
        /// <param name="longitude">Longitude da localização desejada</param>
        /// <returns>Temperatura atual da cidade passada por parâmetro</returns>
        public async Task<short> GetTemperature(double latitude, double longitude)
        {
            var endpoint = _urlAPI.CombineUrl("/weather");
            HttpClient client = _clientFactory.CreateClient();

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"lat", latitude.ToString() },
                {"lon", longitude.ToString() },
                {"appid", _clientId }
            };

            HttpResponseMessage response = await client.GetAsync(endpoint.AddQueryString(parameters));

            return await HandleTempetureResponse(response);
        }

        /// <summary>
        /// Obtém a temperatura atual de uma cidade consumindo a API OpenWeather
        /// </summary>
        /// <param name="city">Cidade a ser pesquisada</param>
        /// <returns>Temperatura atual da cidade passada por parâmetro</returns>
        public async Task<short> GetTemperature(string city)
        {
            var endpoint = _urlAPI.CombineUrl("/weather");
            HttpClient client = _clientFactory.CreateClient();

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"q", city },
                {"appid", _clientId }
            };

            HttpResponseMessage response = await client.GetAsync(endpoint.AddQueryString(parameters));

            return await HandleTempetureResponse(response);
        }

        #endregion PUBLIC METHODS

        #region PRIVATE METHODS
        /// <summary>
        /// Trata o response obtido da API OpenWeather
        /// </summary>
        private async Task<short> HandleTempetureResponse(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<DTOReturnOpenWeather>(content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (data == null || !data.IsValid())
                {
                    throw new NotFoundException("It was not possible to obtain the temperature of the location informed");
                }

                return Convertions.ConvertKelvinToCelsius(data.Main.Temp);
            }
            else
            {
                throw new BadGatewayException(data?.Message);
            }
        }

        /// <summary>
        /// Obtém configurações necessárias para o serviço através do arquivo de configuração appsettings.json
        /// </summary>
        private void ConfigureService()
        {
            _urlAPI = _configuration["ApiOpenWeather:UrlAPI"];
            _clientId = _configuration["ApiOpenWeather:ClientId"];
        }

        #endregion
    }
}
