using Microsoft.EntityFrameworkCore;
using RecipeApp.Model;

namespace DataAccessLayer
{
    /// <summary>
    /// Контекст базы данных для работы с Entity Framework.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Таблица рецептов в базе данных.
        /// </summary>
        public DbSet<Recipe> Recipes { get; set; }

        /// <summary>
        /// Настраивает подключение к базе данных (в данном случае — InMemory).
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("RecipeDb");
        }
    }
}