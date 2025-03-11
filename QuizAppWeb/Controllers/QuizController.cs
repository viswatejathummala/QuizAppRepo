using Microsoft.AspNetCore.Mvc;
using QuizAppWeb.Helpers;
using QuizAppWeb.Models;
using QuizAppWeb.Service;

namespace QuizAppWeb.Controllers
{
    public class QuizController : Controller
    {
        private readonly IApiService _apiService;

        public QuizController(IApiService apiService) {
            _apiService = apiService;
        }
        public async Task<IActionResult> Create(QuizViewModel quizViewModel)
        {
            if (ModelState.IsValid)
            {
                quizViewModel.CreatedBy = 1;
               var response = await _apiService.PostAsync(ApiEndpoints.CreateQuiz, quizViewModel);
                return RedirectToAction("Create");
            }

            return View();
        }
    }
}
