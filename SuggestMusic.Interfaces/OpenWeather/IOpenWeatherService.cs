using System.Threading.Tasks;

namespace SuggestMusic.Interfaces.OpenWeather
{
    public interface IOpenWeatherService
    {
        public Task<short> GetTemperature(double latitude, double longitude);
        public Task<short> GetTemperature(string city);
    }
}
