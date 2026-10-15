using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeApp.Model
{
    /// <summary>
    /// Класс бизнес-логики приложения "Книга рецептов".
    /// Содержит CRUD-операции и две бизнес-функции, зависящие от сущности Recipe.
    /// </summary>
    public class Logic
    {
        private readonly List<Recipe> _recipes = new List<Recipe>();
        private int _nextId = 1;

        /// <summary>
        /// Справочник допустимых категорий блюд.
        /// </summary>
        private static readonly string[] AllowedCategories =
        {
            "Суп", "Салат", "Десерт", "Основное", "Напиток", "Закуска"
        };

        /// <summary>
        /// Создаёт новый рецепт и добавляет его в коллекцию.
        /// </summary>
        public Recipe Create(string name, string category, string mainIngredient,
                             int cookingMinutes, int servings, int calories)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название не может быть пустым");
            if (name.Length > 100)
                throw new ArgumentException("Название не должно превышать 100 символов");

            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Категория не может быть пустой");
            if (!AllowedCategories.Contains(category, StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException(
                    $"Категория должна быть одной из: {string.Join(", ", AllowedCategories)}");

            if (string.IsNullOrWhiteSpace(mainIngredient))
                throw new ArgumentException("Основной ингредиент не может быть пустым");

            if (cookingMinutes <= 0)
                throw new ArgumentException("Время приготовления должно быть больше 0");
            if (cookingMinutes > 60 * 24)
                throw new ArgumentException("Время приготовления не может превышать 24 часа");

            if (servings <= 0)
                throw new ArgumentException("Количество порций должно быть больше 0");
            if (servings > 100)
                throw new ArgumentException("Количество порций не может превышать 100");

            if (calories < 0)
                throw new ArgumentException("Калорийность не может быть отрицательной");
            if (calories > 5000)
                throw new ArgumentException("Калорийность не может превышать 5000 ккал");

            var recipe = new Recipe
            {
                Id = _nextId++,
                Name = name,
                Category = category,
                MainIngredient = mainIngredient,
                CookingMinutes = cookingMinutes,
                Servings = servings,
                Calories = calories
            };
            _recipes.Add(recipe);
            return recipe;
        }

        /// <summary>
        /// Удаляет рецепт по идентификатору.
        /// </summary>
        public bool Delete(int id)
        {
            var recipe = _recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null) return false;
            _recipes.Remove(recipe);
            return true;
        }

        /// <summary>
        /// Возвращает рецепт по идентификатору или null, если не найден.
        /// </summary>
        public Recipe? Read(int id)
        {
            return _recipes.FirstOrDefault(r => r.Id == id);
        }

        /// <summary>
        /// Возвращает копию списка всех рецептов.
        /// </summary>
        public List<Recipe> ReadAll()
        {
            return new List<Recipe>(_recipes);
        }

        /// <summary>
        /// Обновляет данные существующего рецепта.
        /// </summary>
        public bool Update(int id, string name, string category, string mainIngredient,
                           int cookingMinutes, int servings, int calories)
        {
            var recipe = _recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null) return false;

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название не может быть пустым");
            if (name.Length > 100)
                throw new ArgumentException("Название не должно превышать 100 символов");

            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Категория не может быть пустой");
            if (!AllowedCategories.Contains(category, StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException(
                    $"Категория должна быть одной из: {string.Join(", ", AllowedCategories)}");

            if (string.IsNullOrWhiteSpace(mainIngredient))
                throw new ArgumentException("Основной ингредиент не может быть пустым");

            if (cookingMinutes <= 0)
                throw new ArgumentException("Время приготовления должно быть больше 0");
            if (cookingMinutes > 60 * 24)
                throw new ArgumentException("Время приготовления не может превышать 24 часа");

            if (servings <= 0)
                throw new ArgumentException("Количество порций должно быть больше 0");
            if (servings > 100)
                throw new ArgumentException("Количество порций не может превышать 100");

            if (calories < 0)
                throw new ArgumentException("Калорийность не может быть отрицательной");
            if (calories > 5000)
                throw new ArgumentException("Калорийность не может превышать 5000 ккал");

            recipe.Name = name;
            recipe.Category = category;
            recipe.MainIngredient = mainIngredient;
            recipe.CookingMinutes = cookingMinutes;
            recipe.Servings = servings;
            recipe.Calories = calories;
            return true;
        }

        /// <summary>
        /// Бизнес-функция №1: группирует рецепты по категориям.
        /// </summary>
        public Dictionary<string, List<Recipe>> GroupByCategory()
        {
            return _recipes
                .GroupBy(r => r.Category)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        /// Бизнес-функция №2: находит рецепты по основному ингредиенту.
        /// </summary>
        public List<Recipe> FindByIngredient(string ingredient)
        {
            if (string.IsNullOrWhiteSpace(ingredient))
                return new List<Recipe>();

            return _recipes
                .Where(r => r.MainIngredient.IndexOf(ingredient, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        /// <summary>
        /// Возвращает список допустимых категорий (для отображения пользователю).
        /// </summary>
        public string[] GetAllowedCategories()
        {
            return (string[])AllowedCategories.Clone();
        }
    }
}