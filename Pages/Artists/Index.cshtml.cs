using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class IndexModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        public List<Artist> Artists { get; set; } = new List<Artist>();

        public IndexModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task OnGetAsync()
        {
            Artists = await _mongoDBService.GetAsync();
        }
    }
}
