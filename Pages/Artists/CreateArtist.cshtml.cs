using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class CreateArtistModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty]
        public Artist Artist { get; set; }

        public CreateArtistModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _mongoDBService.CreateArtistAsync(Artist);

            return RedirectToPage("/Artists/Index");
        }
    }
}
