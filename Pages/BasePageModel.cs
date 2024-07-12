using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MusicApp.Pages {
    public class BasePageModel : PageModel
    {
        public string CurrentFilter { get; set; }
    }
}