using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class AlbumsModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        public Artist Artist { get; set; }

        public AlbumsModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task OnGetAsync(string id)
        {
            Artist = await _mongoDBService.GetAsync(id);
        }
    }
}
