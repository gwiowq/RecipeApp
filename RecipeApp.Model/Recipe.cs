using System;

namespace RecipeApp.Model
{
    /// <summary>
    /// Сущность "Рецепт".
    /// </summary>
    public class Recipe : IDomainObject
    {
        /// <summary>Уникальный идентификатор.</summary>
        public int Id { get; set; }

        /// <summary>Название блюда.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Категория блюда.</summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>Основной ингредиент.</summary>
        public string MainIngredient { get; set; } = string.Empty;

        /// <summary>Время приготовления в минутах.</summary>
        public int CookingMinutes { get; set; }

        /// <summary>Количество порций.</summary>
        public int Servings { get; set; }

        /// <summary>Калорийность.</summary>
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