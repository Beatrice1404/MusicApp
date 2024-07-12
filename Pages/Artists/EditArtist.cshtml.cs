using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class EditArtistModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty]
        public Artist Artist { get; set; }

        public EditArtistModel(MongoDBService mongoDBService)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingArtist = await _mongoDBService.GetArtistAsync(id);
            if (existingArtist == null)
            {
                return NotFound();
            }

            existingArtist.name = Artist.name; 

            await _mongoDBService.UpdateArtistAsync(id, existingArtist);

            return RedirectToPage("/Artists/Index");
        }
    }
}
