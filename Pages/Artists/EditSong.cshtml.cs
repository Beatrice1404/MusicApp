using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Songs
{
    public class EditSongModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty]
        public Song Song { get; set; }

        public string ArtistId { get; set; }
        public string AlbumId { get; set; }
        public string SongId { get; set; } // Adaug? aceast? proprietate pentru a p?stra songId

        public EditSongModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<IActionResult> OnGetAsync(string artistId, string albumId, string songId)
        {
            ArtistId = artistId;
            AlbumId = albumId;
            SongId = songId; // P?streaz? songId

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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _mongoDBService.UpdateSongAsync(artistId, albumId, songId); // Trimite songId

            return RedirectToPage("/Artists/Details", new { id = artistId });
        }
    }
}
