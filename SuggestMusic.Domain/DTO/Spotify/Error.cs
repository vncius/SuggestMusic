using Newtonsoft.Json;
using System;

namespace SuggestMusic.Domain.DTO.Spotify
{
    [Serializable]
    public class Error
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
