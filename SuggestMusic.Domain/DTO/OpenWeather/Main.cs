using Newtonsoft.Json;
using System;

namespace SuggestMusic.Domain.DTO.OpenWeather
{
    [Serializable]
    public class Main
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }
    }
}
