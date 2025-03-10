using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace QuizAppWeb.Helpers
{
    public class ApiEndpoints
    {
        public static string Login => "Auth/login";
        public static string Register => "Auth/register";
        public static string GetQuizzes => "Quiz";
        public static string GetQuizById(int id) => $"Quiz/{id}";
        public static string CreateQuiz => "Quiz/create";
        public static string UpdateQuiz(int id) => $"Quiz/update/{id}";
        public static string DeleteQuiz(int id) => $"Quiz/delete/{id}";
        public static string GetUsers => "Auth/users";
        //public static string CreateUser => "users/create";
        //public static string GetUserById(int id) => $"users/{id}";
        //public static string UpdateUser(int id) => $"users/update/{id}";
        //public static string DeleteUser(int id) => $"users/delete/{id}";
    }
}
