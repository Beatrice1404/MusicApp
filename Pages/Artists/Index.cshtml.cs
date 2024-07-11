using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;

namespace MusicApp.Pages.Artists
{
    public class IndexModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        public List<ISearchableItem> SearchResults { get; set; } = new List<ISearchableItem>();
        public string CurrentFilter { get; set; }

        public IndexModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task OnGetAsync(string currentFilter)
        {
            CurrentFilter = currentFilter;

            if (string.IsNullOrEmpty(CurrentFilter))
            {
                var artists = await _mongoDBService.GetAsync();
                SearchResults = new List<ISearchableItem>(artists.Select(a => new ArtistSearchResult { Artist = a }));
                return;
            }

            SearchResults = await _mongoDBService.SearchAsync(CurrentFilter);
        }
    }
}
