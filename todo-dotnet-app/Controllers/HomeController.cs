using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Controllers
{
    public class HomeController : Controller
    {
        // Action to display the main todo list page
        public IActionResult Index()
        {
            return View();
        }
    }
}
