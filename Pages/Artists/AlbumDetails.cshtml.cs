using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class AlbumDetailsModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        public Album Album { get; set; }

        public AlbumDetailsModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task OnGetAsync(string albumTitle)
        {
            Album = await _mongoDBService.GetAlbumByTitleAsync(albumTitle);
        }
    }
}
