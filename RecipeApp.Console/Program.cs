using System;
using Ninject;
using RecipeApp.Model;
using RecipeApp.BusinessLogic;

namespace RecipeApp.ConsoleApp
{
    /// <summary>
    /// Консольное представление приложения.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Точка входа в приложение. Запускает меню.
        /// </summary>
        static void Main(string[] args)
        {
            var kernel = new StandardKernel(new SimpleConfigModule());
            var logic = kernel.Get<Logic>();
            var crudService = kernel.Get<RecipeCrudService>();

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
                    case "1": ShowAll(crudService); break;
                    case "2": AddRecipe(crudService); break;
                    case "3": DeleteRecipe(crudService); break;
                    case "4": EditRecipe(crudService); break;
                    case "5": GroupByCategory(logic); break;
                    case "6": FindByIngredient(logic); break;
                    case "0": return;
                }
                Console.WriteLine("\nНажмите любую клавишу...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Запрашивает у пользователя целое число с проверкой ввода.
        /// </summary>
        /// <param name="prompt">Приглашение к вводу.</param>
        /// <returns>Введенное целое число.</returns>
        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (int.TryParse(input, out int value)) return value;
                Console.WriteLine("Ошибка: нужно ввести целое число.");
            }
        }

        /// <summary>
        /// Выводит список всех рецептов.
        /// </summary>
        /// <param name="crudService">Сервис для работы с данными.</param>
        static void ShowAll(RecipeCrudService crudService)
        {
            Console.WriteLine("\n--- Все рецепты ---");
            foreach (var r in crudService.ReadAll())
                Console.WriteLine(r);
        }

        /// <summary>
        /// Запрашивает данные у пользователя и создает новый рецепт.
        /// </summary>
        /// <param name="crudService">Сервис для работы с данными.</param>
        static void AddRecipe(RecipeCrudService crudService)
        {
            Console.WriteLine("\n--- Добавление рецепта ---");
            Console.Write("Название: "); string name = Console.ReadLine() ?? "";
            Console.Write("Категория: "); string category = Console.ReadLine() ?? "";
            Console.Write("Ингредиент: "); string ingredient = Console.ReadLine() ?? "";
            int time = ReadInt("Время (мин): ");
            int servings = ReadInt("Порций: ");
            int calories = ReadInt("Калорий: ");

            try
            {
                crudService.Create(name, category, ingredient, time, servings, calories);
                Console.WriteLine("Рецепт успешно добавлен!");
            }
            catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
        }

        /// <summary>
        /// Удаляет рецепт по выбранному Id.
        /// </summary>
        /// <param name="crudService">Сервис для работы с данными.</param>
        static void DeleteRecipe(RecipeCrudService crudService)
        {
            ShowAll(crudService);
            int id = ReadInt("Введите Id рецепта для удаления: ");
            Console.WriteLine(crudService.Delete(id) ? "Удалено" : "Не найдено");
        }

        /// <summary>
        /// Редактирует существующий рецепт по Id.
        /// </summary>
        /// <param name="crudService">Сервис для работы с данными.</param>
        static void EditRecipe(RecipeCrudService crudService)
        {
            ShowAll(crudService);
            int id = ReadInt("Введите Id рецепта для редактирования: ");
            var recipe = crudService.Read(id);
            if (recipe == null) { Console.WriteLine("Не найдено"); return; }

            Console.Write($"Название ({recipe.Name}): "); string name = Console.ReadLine() ?? "";
            Console.Write($"Категория ({recipe.Category}): "); string category = Console.ReadLine() ?? "";
            Console.Write($"Ингредиент ({recipe.MainIngredient}): "); string ingredient = Console.ReadLine() ?? "";
            int time = ReadInt($"Время ({recipe.CookingMinutes}): ");
            int servings = ReadInt($"Порций ({recipe.Servings}): ");
            int calories = ReadInt($"Калорий ({recipe.Calories}): ");

            try
            {
                crudService.Update(id, name, category, ingredient, time, servings, calories);
                Console.WriteLine("Обновлено");
            }
            catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
        }

        /// <summary>
        /// Выводит рецепты, сгруппированные по категориям.
        /// </summary>
        /// <param name="logic">Бизнес-логика для группировки.</param>
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

        /// <summary>
        /// Ищет рецепты по указанному ингредиенту.
        /// </summary>
        /// <param name="logic">Бизнес-логика для поиска.</param>
        static void FindByIngredient(Logic logic)
        {
            Console.Write("Введите ингредиент: ");
            string ingredient = Console.ReadLine() ?? "";
            var found = logic.FindByIngredient(ingredient);
            Console.WriteLine($"\nНайдено: {found.Count}");
            foreach (var r in found) Console.WriteLine(r);
        }
    }
}