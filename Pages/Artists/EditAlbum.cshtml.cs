using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class EditAlbumModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty]
        public Album Album { get; set; }

        [BindProperty(SupportsGet = true)]
        public string AlbumId { get; set; }

        public EditAlbumModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Album = await _mongoDBService.GetAlbumByIdAsync(AlbumId);

            if (Album == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _mongoDBService.UpdateAlbumAsync(Album.Id, Album);

            return RedirectToPage("/Artists/Albums", new { id = Album.Id });
        }
    }
}
