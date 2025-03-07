using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizAppWeb.Helpers;
using QuizAppWeb.Models;
using QuizAppWeb.Service;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace QuizAppWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApiService _apiService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IApiService apiService, ILogger<AccountController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public IActionResult Login() => View();
        public IActionResult Register() {
            ViewBag.Languages = GetSupportedLanguages();
            return View();
        }

        private List<SelectListItem> GetSupportedLanguages()
        {
            return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(c => new SelectListItem
                {
                    Value = c.Name, // e.g., "en-US", "fr-FR"
                    Text = c.EnglishName // e.g., "English", "French"
                })
                .OrderBy(c => c.Text)
                .ToList();
        }
        public IActionResult ForgotPassword() => View();
        public IActionResult ResetPassword() => View();
        

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginModel loginModel)
        {
            try
            {
                var response = await _apiService.LoginAsync(ApiEndpoints.Login, loginModel);
                bool isSuccess = (response.IsSuccessStatusCode) ? true : false;
                if (isSuccess)
                {
                    string actionName = string.Concat(_apiService.UserRole, "Dashboard");
                    string redirectUrl = Url.Action(actionName, "Admin");
                    return Json(new { success = true, redirectUrl });
                }

                ModelState.AddModelError("", "Invalid login credentials.");
                return BadRequest(new { success = false, message = "Invalid login credentials." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Register([FromBody]UserModel userModel)
        {
            try
            {
                var response = await _apiService.RegisterAsync(ApiEndpoints.Register, userModel);
                bool isSuccess = (response.IsSuccessStatusCode) ? true : false;

                if (isSuccess)
                {
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError("", "Registration failed.");
                return View(userModel);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }

        }
    }
}
