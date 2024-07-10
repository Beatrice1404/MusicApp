using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Songs
{
    public class DeleteSongModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty]
        public Song Song { get; set; }

        public string ArtistId { get; set; }
        public string AlbumId { get; set; }

        public DeleteSongModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<IActionResult> OnGetAsync(string artistId, string albumId, string songId)
        {
            ArtistId = artistId;
            AlbumId = albumId;
            var artist = await _mongoDBService.GetArtistAsync(artistId);
            var album = artist?.albums.FirstOrDefault(a => a.Id == albumId);
            Song = album?.songs.FirstOrDefault(s => s.Id == songId);

            if (Song == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string artistId, string albumId, string songId)
        {
            await _mongoDBService.DeleteSongAsync(artistId, albumId, songId);

            return RedirectToPage("/Artists/Details", new { id = artistId });
        }
    }
}
