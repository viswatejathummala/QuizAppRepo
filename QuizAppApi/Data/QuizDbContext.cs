using Microsoft.EntityFrameworkCore;
using QuizAppApi.DTO;

namespace QuizAppApi.Data
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options)
           : base(options)
        {
        }

        public DbSet<Quiz> Quizzes { get; set; }
    }
}
