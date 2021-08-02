using Newtonsoft.Json;
using System;

namespace SuggestMusic.Domain.DTO.OpenWeather
{
    [Serializable]
    public class DTOReturnOpenWeather
    {
        [JsonProperty("cod")]
        public int Cod { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("main")]
        public Main Main { get; set; }

        public bool IsValid()
        {
            if (Main == null)
            {
                return false;
            }

            return Main.Temp != 0;
        }
    }
}
