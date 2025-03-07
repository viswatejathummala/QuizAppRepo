using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuizAppWeb.Controllers
{
    public class AdminController : Controller
    {
        
        public IActionResult AdminDashboard()
        {
            return View();
        }
        public IActionResult UserDashboard() => View();
        public IActionResult Index()
        {
            return View();
        }
    }
}
