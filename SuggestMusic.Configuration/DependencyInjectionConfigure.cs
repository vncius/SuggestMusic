using Microsoft.Extensions.DependencyInjection;
using SuggestMusic.Interfaces.OpenWeather;
using SuggestMusic.Interfaces.Spotify;
using SuggestMusic.Service.OpenWeather;
using SuggestMusic.Service.Spotify;

namespace SuggestMusic.Configuration
{
    public static class DependencyInjectionConfigure
    {
        public static void ConfigureDependencias(IServiceCollection service)
        {
            service.ConfigureSpotifyService();
            service.ConfigureOpenWeatherService();
        }

        private static void ConfigureSpotifyService(this IServiceCollection service)
        {
            service.AddScoped<ISpotifyService, SpotifyService>();
        }

        private static void ConfigureOpenWeatherService(this IServiceCollection service)
        {
            service.AddScoped<IOpenWeatherService, OpenWeatherService>();
        }
    }
}
