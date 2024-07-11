using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class AlbumDetailsModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        public Album Album { get; set; }

        public string ArtistId { get; set; } // Ad?ug?m ArtistId ca proprietate

        public AlbumDetailsModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task OnGetAsync(string albumTitle)
        {
            var artist = await _mongoDBService.GetArtistByAlbumTitleAsync(albumTitle);
            if (artist != null)
            {
                ArtistId = artist.Id;
                Album = artist.albums.FirstOrDefault(a => a.title == albumTitle);
            }
        }
    }
}
