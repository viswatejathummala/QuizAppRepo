using Microsoft.AspNetCore.Mvc;

namespace QuizAppWeb.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login() => View();
        public IActionResult Register() => View();
        public IActionResult ForgotPassword() => View();
        public IActionResult ResetPassword() => View();

        [HttpPost]
        public IActionResult Login(User user)
        {
            // Authentication logic here
            return RedirectToAction("Dashboard", "Admin");
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            // Registration logic here
            return RedirectToAction("Login");
        }
        public class User
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; } // Admin/User
        }

        public class Quiz
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public List<Question> Questions { get; set; }
        }

        public class Question
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public List<string> Options { get; set; }
            public string CorrectAnswer { get; set; }
        }

    }
}
