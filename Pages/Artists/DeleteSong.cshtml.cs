using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class DeleteSongModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty]
        public Song Song { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SongId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string AlbumId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ArtistId { get; set; }

        public DeleteSongModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<IActionResult> OnGetAsync(string artistId, string albumId, string songId)
        {
            // Ob?ine song-ul pe care dore?ti s?-l ?tergi
            Song = await _mongoDBService.GetSongByTitleAsync(songId);

            if (Song == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string artistId, string albumId, string songId)
        {
            await _mongoDBService.DeleteSongAsync(artistId, albumId, songId);

            return RedirectToPage("/Artists/AlbumDetails", new { artistId = artistId, albumTitle = Song.title });
        }

    }
}
