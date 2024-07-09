using Microsoft.AspNetCore.Mvc;
using MusicApp.Models;
using MusicApp.Services;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApp.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly MongoDBService _mongoDBService;

        public ArtistsController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<IActionResult> Index()
        {
            var artists = await _mongoDBService.GetAsync();
            return View(artists);
        }

        [HttpGet] 
        public async Task<IActionResult> Albums(string id)
        {
            var artist = await _mongoDBService.GetAsync(id);
            return View(artist);
        }

        public async Task<IActionResult> AlbumDetails(string artistId, string albumId)
        {
            var artist = await _mongoDBService.GetAsync(artistId);
            var album = artist.albums.FirstOrDefault(a => a.Id == albumId);
            return View(album);
        }
    }

}
