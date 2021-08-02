using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SuggestMusic.Domain.DTO.Spotify;
using SuggestMusic.Domain.Model;
using SuggestMusic.Infrastructure.Exceptions.HttpExceptions;
using SuggestMusic.Interfaces.Helpers;
using SuggestMusic.Interfaces.Spotify;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SuggestMusic.Service.Spotify
{
    /// <summary>
    /// Classe de serviço referente ao consumo da API (Spotify)
    /// </summary>
    public class SpotifyService : ISpotifyService
    {
        #region PROPERTIES

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        private string _urlGetToken;
        private string _urlAPI;
        private string _clientId;
        private string _clientSecret;

        #endregion PROPERTIES

        #region CONSTRUCTOR

        /// <summary>
        /// Construtor da classe SpotifyService
        /// </summary>
        /// <param name="clientFactory">Injeção de dependência para IHttpClientFactory</param>
        /// <param name="configuration">Injeção de dependência para IConfiguration</param>
        /// <returns>Objeto de retorno da API, contém uma lista de string (Músicas)</returns>
        public SpotifyService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            ConfigureService();
        }

        #endregion CONSTRUCTOR

        #region PUBLIC METHODS

        /// <summary>
        /// Obtém recomendações de músicas consumindo a API do Spotify
        /// </summary>
        /// <param name="genre">Gênero musical</param>
        /// <returns>Objeto de retorno da API, contém uma lista de string (Músicas)</returns>
        public async Task<ModelPlaylist> GetTracks(string genre)
        {
            string token = await GetToken();
            HttpClient client = _clientFactory.CreateClient();
            string endpoint = _urlAPI.CombineUrl("/recommendations");

            InsertHeader(ref client, $"Bearer {token}");

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"seed_genres", genre }
            };

            HttpResponseMessage response = await client.GetAsync(endpoint.AddQueryString(parameters));
            string content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<DTOReturnSpotifyRecomendations>(content);

            if (response.StatusCode == HttpStatusCode.OK)
            {

                if (data.Tracks == null || !data.IsValid())
                {
                    throw new NotFoundException("Unable to get songs for requested weather condition");
                }

                var dataOrdered = data.Tracks.OrderByDescending(x => x.Popularity).ToList();
                return new ModelPlaylist() { Playlist = dataOrdered.Select(x => x.Name).ToArray() };
            }
            else
            {
                throw new BadGatewayException(data.Error?.Message);
            }
        }

        #endregion PUBLIC METHODS

        #region PRIVATE METHODS

        /// <summary>
        /// Obtém o token de acesso para consumir recursos da API do (Spotify)
        /// </summary>
        /// <returns>Token para consumir serviços do Spotify</returns>
        private async Task<string> GetToken()
        {
            string endpoint = _urlGetToken.CombineUrl("/token");
            HttpClient client = _clientFactory.CreateClient();

            InsertHeader(ref client, $"Basic {$"{_clientId}:{_clientSecret}".ConvertToBase64()}");

            Dictionary<string, string> body = new Dictionary<string, string>()
            {
                {"token_type","bearer" },
                {"grant_type","client_credentials" }
            };

            HttpResponseMessage response = await client.PostAsync(endpoint, new FormUrlEncodedContent(body));
            string content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<DTOReturnSpotifyToken>(content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (!data.IsValid())
                {
                    throw new NotFoundException("Token returned by Spotify API is invalid");
                }

                return data.Access_token;
            }
            else
            {
                throw new BadGatewayException(data.Error);
            }
        }

        /// <summary>
        /// Obtém o token de acesso para consumir recursos da API do (Spotify)
        /// </summary>
        /// <param name="client">Cliente Http referenciado para incluir header</param>
        /// <param name="authorization">Autorização a ser incluida no header</param>
        private void InsertHeader(ref HttpClient client, string authorization)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "SuggestMusic");
            client.DefaultRequestHeaders.Add("Authorization", authorization);
        }

        /// <summary>
        /// Obtém configurações necessárias para o serviço através do arquivo de configuração appsettings.json
        /// </summary>
        private void ConfigureService()
        {
            _urlAPI = _configuration["ApiSpotify:UrlAPI"];
            _urlGetToken = _configuration["ApiSpotify:UrlGetToken"];
            _clientId = _configuration["ApiSpotify:ClientId"];
            _clientSecret = _configuration["ApiSpotify:ClientSecret"];
        }

        #endregion PRIVATE METHODS
    }
}
