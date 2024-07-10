using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class DeleteArtistModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty]
        public Artist Artist { get; set; }

        public DeleteArtistModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Artist = await _mongoDBService.GetArtistAsync(id);

            if (Artist == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            await _mongoDBService.DeleteArtistAsync(id);

            return RedirectToPage("/Artists/Index");
        }
    }
}
