using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Linq;
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

        public async Task OnGetAsync(string artistId, string albumId)
        {
            var artist = await _mongoDBService.GetAsync(artistId);
            Album = artist.albums.FirstOrDefault(a => a.Id == albumId);
        }
    }
}
