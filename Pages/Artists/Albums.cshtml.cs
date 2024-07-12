using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class AlbumsModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public Artist Artist { get; set; }

        public AlbumsModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task OnGetAsync()
        {
            Artist = await _mongoDBService.GetArtistAsync(Id);
        }
    }
}