using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicApp.Models;
using MusicApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicApp.Pages.Artists
{
    public class CreateArtistModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;

        [BindProperty]
        public Artist Artist { get; set; } = new Artist();

        public CreateArtistModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public void OnGet()
        {
            // Nu este nevoie de logica suplimentar? pentru metoda OnGet în acest exemplu
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Artist.albums = new List<Album>(); // Ini?ializeaz? lista de albume

            try
            {
                // Încearc? s? creezi artistul în MongoDB
                await _mongoDBService.CreateArtistAsync(Artist);

                // Log pentru debug (op?ional)
                Console.WriteLine($"Artistul cu numele '{Artist.name}' a fost creat cu succes.");
            }
            catch (Exception ex)
            {
                // Trateaz? excep?ia (afi?eaz? mesaj, înregistreaz? în log-uri, etc.)
                Console.WriteLine($"Eroare la operarea cu MongoDB: {ex.Message}");

                // Întoarce o pagin? de eroare sau afi?eaz? un mesaj de eroare utilizatorului
                // În acest exemplu, doar redirect?m înapoi la aceea?i pagin? (dar po?i gestiona asta cum dore?ti)
                return RedirectToPage("/Error");
            }

            // Dac? totul a func?ionat corect, redirecteaz? utilizatorul c?tre pagina de index a arti?tilor
            return RedirectToPage("/Artists/Index");
        }
    }
}
