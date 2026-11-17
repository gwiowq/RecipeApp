using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using RecipeApp.Model;

namespace RecipeApp.BusinessLogic
{
    /// <summary>
    /// Бизнес-логика: поиск и группировка.
    /// </summary>
    public class Logic
    {
        
        private readonly IReadRepository<Recipe> _readRepository;

        /// <summary>Инициализирует логику репозиторием для чтения.</summary>
        public Logic(IReadRepository<Recipe> readRepo)
        {
            _readRepository = readRepo;
        }

        /// <summary>Группирует рецепты по категориям.</summary>
        public Dictionary<string, List<Recipe>> GroupByCategory()
        {
            var allRecipes = _readRepository.ReadAll();
            return allRecipes.GroupBy(r => r.Category).ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>Ищет рецепты по ингредиенту.</summary>
        public List<Recipe> FindByIngredient(string ingredient)
        {
            if (string.IsNullOrWhiteSpace(ingredient)) return new List<Recipe>();
            var allRecipes = _readRepository.ReadAll();
            return allRecipes.Where(r => r.MainIngredient.IndexOf(ingredient, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        /// <summary>Возвращает допустимые категории.</summary>
        public string[] GetAllowedCategories() => new[] { "Суп", "Салат", "Десерт", "Основное", "Напиток", "Закуска" };
    }
}