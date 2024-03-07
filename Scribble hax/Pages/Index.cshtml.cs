using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scribble_hax.Controllers;

namespace Scribble_hax.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public readonly GameManager Game;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Game = new GameManager();
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost() {
            string response = Request.Form["userResponse"];

            Game.HandleUserInput(response);

            return RedirectToPage();
        }
    }
}
