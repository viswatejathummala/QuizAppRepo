using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using QuizAppWeb.Helpers;
using QuizAppWeb.Models;
using QuizAppWeb.Service;
using System.Globalization;

namespace QuizAppWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly IApiService _apiService;

        public AdminController(IApiService apiService) {
            _apiService = apiService;
        }
        public async Task<IActionResult> AdminDashboard()
        {
            // Assume languagePreference is set somewhere in your logic
            var languagePreference = HttpContext.Session.GetString("LanguagePreference") ?? "en";

            // Set the culture dynamically
            CultureInfo culture = new CultureInfo(languagePreference);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
    );

            var response =  await _apiService.GetAsync(ApiEndpoints.GetQuizzes);
            if (!response.IsSuccessStatusCode)
            {
                // Handle error response
                ModelState.AddModelError("", "Failed to load quizzes.");
                return View(); 
            }

            var quizzes = await response.Content.ReadFromJsonAsync<List<QuizViewModel>>();
            var viewModel = new AdminDashboardViewModel { Quizzes = quizzes };

            return View(viewModel);
        }
        public IActionResult UserDashboard() => View();

        public async Task<IActionResult> UserManagement()
        {
            var response = await _apiService.GetAsync(ApiEndpoints.GetUsers);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Failed to load users.");
                return View();
            }

            var users = await response.Content.ReadFromJsonAsync<List<UserModel>>();
            return View(users);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
