using System;

namespace RecipeApp.Model
{
    /// <summary>
    /// Сущность "Рецепт" — описывает блюдо и его характеристики.
    /// </summary>
    public class Recipe
    {
        /// <summary>Уникальный идентификатор рецепта.</summary>
        public int Id { get; set; }

        /// <summary>Название блюда.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Категория блюда (суп, салат, десерт и т.д.).</summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>Основной ингредиент блюда.</summary>
        public string MainIngredient { get; set; } = string.Empty;

        /// <summary>Время приготовления в минутах.</summary>
        public int CookingMinutes { get; set; }

        /// <summary>Количество порций.</summary>
        public int Servings { get; set; }

        /// <summary>Калорийность одной порции (ккал).</summary>
        public int Calories { get; set; }

        /// <summary>
        /// Возвращает строковое представление рецепта.
        /// </summary>
        public override string ToString()
        {
            return $"[{Id}] \"{Name}\" — категория: {Category}, осн. ингредиент: {MainIngredient}, " +
                   $"{CookingMinutes} мин, {Servings} порц., {Calories} ккал";
        }
    }
}