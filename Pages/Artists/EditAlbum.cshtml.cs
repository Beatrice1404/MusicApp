using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Albums
{
    public class EditAlbumModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty]
        public Album Album { get; set; }

        public string ArtistId { get; set; }

        public EditAlbumModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<IActionResult> OnGetAsync(string artistId, string albumId)
        {
            ArtistId = artistId;
            var artist = await _mongoDBService.GetArtistAsync(artistId);
            Album = artist?.albums.FirstOrDefault(a => a.Id == albumId);

            if (Album == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string artistId, string albumId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _mongoDBService.UpdateAlbumAsync(artistId, Album);

            return RedirectToPage("/Artists/Details", new { id = artistId });
        }
    }
}
