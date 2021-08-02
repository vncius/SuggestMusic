using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SuggestMusic.Domain.DTO.Spotify
{
    [Serializable]
    public class DTOReturnSpotifyRecomendations
    {
        [JsonProperty("tracks")]
        public List<Track> Tracks { get; set; }

        [JsonProperty("error")]
        public Error Error { get; set; }

        public bool IsValid()
        {
            if (Tracks == null)
            {
                return false;
            }

            return Tracks.Count > 0;
        }
    }
}
