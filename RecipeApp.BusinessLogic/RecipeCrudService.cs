using System;
using System.Collections.Generic;
using DataAccessLayer;
using RecipeApp.Model;

namespace RecipeApp.BusinessLogic
{
    /// <summary>
    /// Сервис для выполнения CRUD-операций над рецептами.
    /// </summary>
    public class RecipeCrudService
    {
        
        private readonly IReadRepository<Recipe> _readRepository;
        private readonly IWriteRepository<Recipe> _writeRepository;

        
        /// <summary>Инициализирует сервис с внедренными репозиториями.</summary>
        public RecipeCrudService(IReadRepository<Recipe> readRepo, IWriteRepository<Recipe> writeRepo)
        {
            _readRepository = readRepo;
            _writeRepository = writeRepo;
        }

        /// <summary>Создаёт новый рецепт.</summary>
        public Recipe Create(string name, string category, string mainIngredient, int cookingMinutes, int servings, int calories)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Название не может быть пустым");
            if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Категория не может быть пустой");

            var recipe = new Recipe
            {
                Name = name,
                Category = category,
                MainIngredient = mainIngredient,
                CookingMinutes = cookingMinutes,
                Servings = servings,
                Calories = calories
            };

            
            _writeRepository.Add(recipe);
            return recipe;
        }

        /// <summary>Удаляет рецепт по Id.</summary>
        public bool Delete(int id)
        {
            
            var recipe = _readRepository.ReadById(id);
            if (recipe == null) return false;

            _writeRepository.Delete(id);
            return true;
        }

        /// <summary>Возвращает рецепт по Id.</summary>
        public Recipe? Read(int id) => _readRepository.ReadById(id);

        /// <summary>Возвращает все рецепты.</summary>
        public List<Recipe> ReadAll() => _readRepository.ReadAll();

        /// <summary>Обновляет рецепт.</summary>
        public bool Update(int id, string name, string category, string mainIngredient, int cookingMinutes, int servings, int calories)
        {
            var recipe = _readRepository.ReadById(id);
            if (recipe == null) return false;

            recipe.Name = name;
            recipe.Category = category;
            recipe.MainIngredient = mainIngredient;
            recipe.CookingMinutes = cookingMinutes;
            recipe.Servings = servings;
            recipe.Calories = calories;
            _writeRepository.Update(recipe);
            return true;
        }
    }
}