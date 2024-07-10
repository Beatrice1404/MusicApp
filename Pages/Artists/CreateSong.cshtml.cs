using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Songs
{
    public class CreateSongModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty]
        public Song Song { get; set; }

        public string ArtistId { get; set; }
        public string AlbumId { get; set; }

        public CreateSongModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public void OnGet(string artistId, string albumId)
        {
            ArtistId = artistId;
            AlbumId = albumId;
        }

        public async Task<IActionResult> OnPostAsync(string artistId, string albumId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _mongoDBService.AddSongAsync(artistId, albumId, Song);

            return RedirectToPage("/Artists/Details", new { id = artistId });
        }
    }
}
