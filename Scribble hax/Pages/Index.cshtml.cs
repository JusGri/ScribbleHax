using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scribble_hax.Controllers.Board;

namespace Scribble_hax.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public readonly Board GameBoard;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            GameBoard = new Board();
        }

        public void OnGet()
        {

        }
    }
}
