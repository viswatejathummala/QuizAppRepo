using Microsoft.AspNetCore.Mvc;

namespace QuizAppWeb.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
