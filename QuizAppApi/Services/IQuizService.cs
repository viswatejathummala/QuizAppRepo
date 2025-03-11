using QuizAppApi.Data;
using QuizAppApi.DTO;

namespace QuizAppApi.Services
{
    public interface IQuizService
    {
        Task<IEnumerable<Quiz>> GetQuizzesAsync();
        Task<Quiz> GetQuizByIdAsync(int id);
        Task AddQuizAsync(QuizDTO quiz);
        Task UpdateQuizAsync(QuizDTO quiz);
        Task DeleteQuizAsync(int id);
    }
}
