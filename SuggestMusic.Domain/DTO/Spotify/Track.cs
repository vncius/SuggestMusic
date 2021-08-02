using Newtonsoft.Json;
using System;

namespace SuggestMusic.Domain.DTO.Spotify
{
    [Serializable]
    public class Track
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("popularity")]
        public short Popularity { get; set; }
    }
}
