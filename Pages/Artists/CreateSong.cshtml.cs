using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class CreateSongModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty]
        public Song Song { get; set; }

        [BindProperty(SupportsGet = true)]
        public string AlbumId { get; set; }

        public Album Album { get; set; }

        public string ArtistId { get; set; } // Ad?ug?m ArtistId ca proprietate

        public CreateSongModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task OnGetAsync(string artistId, string albumId)
        {
            Album = await _mongoDBService.GetAlbumByIdAsync(albumId);
        }

        public async Task<IActionResult> OnPostAsync(string artistId, string albumId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _mongoDBService.AddSongAsync(artistId, albumId, Song);

            return RedirectToPage("/Artists/AlbumDetails", new { artistId = artistId, albumTitle = Album.title });
        }


    }
}
