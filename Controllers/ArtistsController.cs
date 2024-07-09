using Microsoft.AspNetCore.Mvc;
using MusicApp.Models;
using MusicApp.Services;

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
}
