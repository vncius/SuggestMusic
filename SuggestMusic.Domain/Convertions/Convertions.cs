using System;

namespace SuggestMusic.Domain.Convertions
{
    public static class Convertions
    {
        /// <summary>
        /// Converte kelvin em Celsius
        /// </summary>
        /// <param name="temperature">Temperatura no formato kelvin</param>
        /// <returns>Temperatura no formato Celsius</returns>
        public static short ConvertKelvinToCelsius(double temperature)
        {
            return Convert.ToInt16(temperature - 273.15);
        }

        /// <summary>
        /// Converte temperatura em estilo musical
        /// </summary>
        /// <param name="temperature">Temperatura no formato kelvin</param>
        /// <returns>Temperatura no formato Celsius</returns>
        public static string ConvertCelsiusToMusicStyle(double tempetureInCelsius)
        {
            if (tempetureInCelsius > 30)
            {
                return "party";
            }
            else if (tempetureInCelsius >= 15)
            {
                return "pop";
            }
            else if (tempetureInCelsius > 14)
            {
                return "rock";
            }

            return "classical";
        }
    }
}
