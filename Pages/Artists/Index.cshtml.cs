using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicApp.Models;
using MusicApp.Services;

namespace MusicApp.Pages.Artists
{
    public class IndexModel : BasePageModel
    {
        private readonly MongoDBService _mongoDBService;

        public List<ISearchableItem> SearchResults { get; set; } = new List<ISearchableItem>();

        public IndexModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task OnGetAsync(string currentFilter)
        {
            CurrentFilter = currentFilter;

            if (string.IsNullOrEmpty(CurrentFilter))
            {
                // If there's no filter, you can decide to show all artists or nothing.
                var artists = await _mongoDBService.GetAsync();
                SearchResults = new List<ISearchableItem>(artists.Select(a => new ArtistSearchResult { Artist = a }));
                return;
            }

            SearchResults = await _mongoDBService.SearchAsync(CurrentFilter);
        }
    }
}
