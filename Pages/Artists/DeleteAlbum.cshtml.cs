using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class DeleteAlbumModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty]
        public Album Album { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ArtistId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string AlbumId { get; set; }

        public DeleteAlbumModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task OnGetAsync(string artistId, string albumId)
        {
            ArtistId = artistId;
            AlbumId = albumId;
            Album = await _mongoDBService.GetAlbumByIdAsync(albumId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _mongoDBService.DeleteAlbumAsync(ArtistId, AlbumId);
            return RedirectToPage("/Artists/Albums", new { id = ArtistId });
        }
    }
}
