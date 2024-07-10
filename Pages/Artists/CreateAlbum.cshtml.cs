using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class CreateAlbumModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty]
        public Album Album { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public Artist Artist { get; set; }

        public CreateAlbumModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task OnGetAsync()
        {
            Artist = await _mongoDBService.GetArtistAsync(Id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _mongoDBService.AddAlbumAsync(Id, Album);

            return RedirectToPage("/Artists/Albums", new { id = Id });
        }
    }
}
