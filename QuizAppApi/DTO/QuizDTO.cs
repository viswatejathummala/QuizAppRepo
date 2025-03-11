using System.ComponentModel.DataAnnotations;

namespace QuizAppApi.DTO
{
    public class QuizDTO
    {
        [Required]
        public int QuizId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        [Required]
        public int CreatedBy { get; set; }
    }
}
