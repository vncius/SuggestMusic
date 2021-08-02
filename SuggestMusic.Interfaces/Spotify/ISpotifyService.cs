using SuggestMusic.Domain.Model;
using System.Threading.Tasks;

namespace SuggestMusic.Interfaces.Spotify
{
    public interface ISpotifyService
    {
        Task<ModelPlaylist> GetTracks(string search);
    }
}
