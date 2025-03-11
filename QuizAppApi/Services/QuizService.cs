using QuizAppApi.Data;
using QuizAppApi.DTO;
using QuizAppApi.Repositories;

namespace QuizAppApi.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _repository;
        private readonly IUserRepository _userRepository;

        public QuizService(IQuizRepository repository,IUserRepository userrepository)
        {
            _repository = repository;
            _userRepository = userrepository;
        }

        public Task<IEnumerable<Quiz>> GetQuizzesAsync() => _repository.GetQuizzesAsync();

        public Task<Quiz> GetQuizByIdAsync(int id) => _repository.GetQuizByIdAsync(id);

        public async Task AddQuizAsync(QuizDTO quizDTO)
        {
            var creator = await _userRepository.GetUserByEmailAsync("viswaajet@gmail.com");
            if (creator == null)
            {
                throw new Exception("User not found."); // Handle user not found scenario
            }

            var quiz = new Quiz
            {
                Title = quizDTO.Title,
                Description = quizDTO.Description,
                DurationMinutes = quizDTO.DurationMinutes,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = quizDTO.CreatedBy, 
                Creator = creator             
            };

            await _repository.AddQuizAsync(quiz);

        }
        public async Task UpdateQuizAsync(QuizDTO quizDTO)
        {
            var creator = await _userRepository.GetUserByEmailAsync("viswaajet@gmail.com");
            if (creator == null)
            {
                throw new Exception("User not found."); // Handle user not found scenario
            }

            var quiz = new Quiz
            {
                Title = quizDTO.Title,
                Description = quizDTO.Description,
                DurationMinutes = quizDTO.DurationMinutes,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = quizDTO.CreatedBy,
                Creator = creator
            };

            await _repository.AddQuizAsync(quiz);
        }

        public Task DeleteQuizAsync(int id) => _repository.DeleteQuizAsync(id);
    }
}
