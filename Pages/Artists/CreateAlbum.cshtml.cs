using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Albums
{
    public class CreateAlbumModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty]
        public Album Album { get; set; }

        public string ArtistId { get; set; }

        public CreateAlbumModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public void OnGet(string artistId)
        {
            ArtistId = artistId;
        }

        public async Task<IActionResult> OnPostAsync(string artistId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _mongoDBService.AddAlbumAsync(artistId, Album);

            return RedirectToPage("/Artists/Details", new { id = artistId });
        }
    }
}
