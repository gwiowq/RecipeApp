using System;
using RecipeApp.Model;

namespace RecipeApp.ConsoleApp
{
    /// <summary>
    /// Консольное представление приложения "Книга рецептов".
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var logic = new Logic();
            SeedData(logic);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== КНИГА РЕЦЕПТОВ (Консоль) ===");
                Console.WriteLine("1. Показать все рецепты");
                Console.WriteLine("2. Добавить рецепт");
                Console.WriteLine("3. Удалить рецепт");
                Console.WriteLine("4. Редактировать рецепт");
                Console.WriteLine("5. Группировка по категориям");
                Console.WriteLine("6. Поиск по ингредиенту");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");

                switch (Console.ReadLine())
                {
                    case "1": ShowAll(logic); break;
                    case "2": AddRecipe(logic); break;
                    case "3": DeleteRecipe(logic); break;
                    case "4": EditRecipe(logic); break;
                    case "5": GroupByCategory(logic); break;
                    case "6": FindByIngredient(logic); break;
                    case "0": return;
                }
                Console.WriteLine("\nНажмите любую клавишу...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Запрашивает у пользователя целое число. Повторяет ввод, пока не получит корректное значение.
        /// </summary>
        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (int.TryParse(input, out int value))
                    return value;
                Console.WriteLine("Ошибка: нужно ввести целое число. Попробуйте ещё раз.");
            }
        }

        /// <summary>Заполняет хранилище начальными данными.</summary>
        static void SeedData(Logic logic)
        {
            logic.Create("Борщ", "Суп", "Свёкла", 90, 4, 120);
            logic.Create("Цезарь", "Салат", "Курица", 20, 2, 250);
            logic.Create("Тирамису", "Десерт", "Маскарпоне", 60, 6, 350);
            logic.Create("Паста Карбонара", "Основное", "Спагетти", 25, 2, 480);
        }

        /// <summary>Выводит все рецепты в консоль.</summary>
        static void ShowAll(Logic logic)
        {
            Console.WriteLine("\n--- Все рецепты ---");
            foreach (var r in logic.ReadAll())
                Console.WriteLine(r);
        }

        /// <summary>Запрашивает данные и создаёт новый рецепт.</summary>
        static void AddRecipe(Logic logic)
        {
            Console.WriteLine("\n--- Добавление рецепта ---");
            Console.WriteLine($"Допустимые категории: {string.Join(", ", logic.GetAllowedCategories())}");

            Console.Write("Название: "); string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Категория: "); string category = Console.ReadLine() ?? string.Empty;
            Console.Write("Основной ингредиент: "); string ingredient = Console.ReadLine() ?? string.Empty;
            int time = ReadInt("Время (мин): ");
            int servings = ReadInt("Порций: ");
            int calories = ReadInt("Калорий: ");

            try
            {
                var recipe = logic.Create(name, category, ingredient, time, servings, calories);
                Console.WriteLine($"Создан рецепт: {recipe}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>Удаляет рецепт по Id.</summary>
        static void DeleteRecipe(Logic logic)
        {
            int id = ReadInt("Введите Id рецепта для удаления: ");
            Console.WriteLine(logic.Delete(id) ? "Удалено" : "Не найдено");
        }

        /// <summary>Редактирует рецепт по Id.</summary>
        static void EditRecipe(Logic logic)
        {
            int id = ReadInt("Введите Id рецепта для редактирования: ");
            var recipe = logic.Read(id);
            if (recipe == null) { Console.WriteLine("Не найдено"); return; }

            Console.WriteLine($"Допустимые категории: {string.Join(", ", logic.GetAllowedCategories())}");

            Console.Write($"Название ({recipe.Name}): "); string name = Console.ReadLine() ?? string.Empty;
            Console.Write($"Категория ({recipe.Category}): "); string category = Console.ReadLine() ?? string.Empty;
            Console.Write($"Осн. ингредиент ({recipe.MainIngredient}): "); string ingredient = Console.ReadLine() ?? string.Empty;
            int time = ReadInt($"Время ({recipe.CookingMinutes}): ");
            int servings = ReadInt($"Порций ({recipe.Servings}): ");
            int calories = ReadInt($"Калорий ({recipe.Calories}): ");

            try
            {
                logic.Update(id, name, category, ingredient, time, servings, calories);
                Console.WriteLine("Обновлено");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>Выводит рецепты, сгруппированные по категориям.</summary>
        static void GroupByCategory(Logic logic)
        {
            Console.WriteLine("\n--- Рецепты по категориям ---");
            foreach (var group in logic.GroupByCategory())
            {
                Console.WriteLine($"\nКатегория: {group.Key}");
                foreach (var r in group.Value)
                    Console.WriteLine($"  {r}");
            }
        }

        /// <summary>Ищет рецепты по основному ингредиенту.</summary>
        static void FindByIngredient(Logic logic)
        {
            Console.Write("Введите ингредиент: ");
            string ingredient = Console.ReadLine() ?? string.Empty;
            var found = logic.FindByIngredient(ingredient);
            Console.WriteLine($"\nНайдено: {found.Count}");
            foreach (var r in found)
                Console.WriteLine(r);
        }
    }
}