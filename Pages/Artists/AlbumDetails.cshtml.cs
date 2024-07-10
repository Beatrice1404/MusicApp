using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class AlbumDetailsModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        public AlbumDetailsModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [BindProperty(SupportsGet = true)]
        public string AlbumTitle { get; set; }

        public Album Album { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Album = await _mongoDBService.GetAlbumByTitleAsync(AlbumTitle);

            if (Album == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
