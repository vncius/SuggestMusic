using Newtonsoft.Json;
using System;

namespace SuggestMusic.Domain.DTO.Spotify
{
    [Serializable]
    public class DTOReturnSpotifyToken
    {
        [JsonProperty("access_token")]
        public string Access_token { get; set; }

        [JsonProperty("token_type")]
        public string Token_type { get; set; }

        [JsonProperty("expires_in")]
        public int Expires_in { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Access_token);
        }
    }
}
