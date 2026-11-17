using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using RecipeApp.Model;

namespace DataAccessLayer
{
    // <summary>
    /// Репозиторий для работы с базой данных через Dapper.
    /// </summary>
    public class DapperRepository<Recipe> : IRepository<Recipe> where Recipe : class, IDomainObject
    {
        private readonly string _connectionString = @"Server=(localdb)\mssqllocaldb;Database=RecipeAppDb;Trusted_Connection=True;";

        /// <inheritdoc/>
        public void Add(Recipe entity)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"INSERT INTO Recipes (Name, Category, MainIngredient, CookingMinutes, Servings, Calories) 
                           VALUES (@Name, @Category, @MainIngredient, @CookingMinutes, @Servings, @Calories)";
            connection.Execute(sql, entity);
        }

        /// <inheritdoc/>
        public void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "DELETE FROM Recipes WHERE Id = @Id";
            connection.Execute(sql, new { Id = id });
        }

        /// <inheritdoc/>
        public List<Recipe> ReadAll()
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT * FROM Recipes";
            return connection.Query<Recipe>(sql).ToList();
        }

        /// <inheritdoc/>
        public Recipe? ReadById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT * FROM Recipes WHERE Id = @Id";
            return connection.QueryFirstOrDefault<Recipe>(sql, new { Id = id });
        }

        /// <inheritdoc/>
        public void Update(Recipe entity)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"UPDATE Recipes 
                           SET Name = @Name, Category = @Category, MainIngredient = @MainIngredient, 
                               CookingMinutes = @CookingMinutes, Servings = @Servings, Calories = @Calories 
                           WHERE Id = @Id";
            connection.Execute(sql, entity);
        }
    }
}