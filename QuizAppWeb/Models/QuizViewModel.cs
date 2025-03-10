using System.ComponentModel.DataAnnotations;

namespace QuizAppWeb.Models
{
    public class QuizViewModel
    {
        public int QuizId { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public int DurationMinutes { get; set; }
    }
}
