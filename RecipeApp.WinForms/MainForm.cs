using System;
using System.Linq;
using System.Windows.Forms;
using RecipeApp.Model;

namespace RecipeApp.WinForms
{
    /// <summary>
    /// Главная форма приложения "Книга рецептов".
    /// Представление WindowsForms, использующее слой модели RecipeApp.Model.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly Logic _logic = new Logic();

        private DataGridView _grid = null!;
        private TextBox _txtName = null!;
        private TextBox _txtCategory = null!;
        private TextBox _txtIngredient = null!;
        private TextBox _txtTime = null!;
        private TextBox _txtServings = null!;
        private TextBox _txtCalories = null!;
        private TextBox _txtSearch = null!;

        /// <summary>Инициализирует главную форму.</summary>
        public MainForm()
        {
            InitializeComponent();
            SeedData();
            BuildUi();
            RefreshGrid();
        }

        /// <summary>Заполняет хранилище начальными данными.</summary>
        private void SeedData()
        {
            _logic.Create("Борщ", "Суп", "Свёкла", 90, 4, 120);
            _logic.Create("Цезарь", "Салат", "Курица", 20, 2, 250);
            _logic.Create("Тирамису", "Десерт", "Маскарпоне", 60, 6, 350);
            _logic.Create("Паста Карбонара", "Основное", "Спагетти", 25, 2, 480);
        }

        /// <summary>Создаёт элементы интерфейса формы.</summary>
        private void BuildUi()
        {
            Text = "Книга рецептов (WinForms)";
            Width = 1000;
            Height = 650;

            // Таблица
            _grid = new DataGridView
            {
                Left = 10,
                Top = 10,
                Width = 960,
                Height = 300,
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            _grid.SelectionChanged += (s, e) => FillInputsFromSelection();
            Controls.Add(_grid);

            // Поля ввода
            int y = 330;

            AddLabel("Название:", 10, y);
            _txtName = AddTextBox(120, y);

            AddLabel("Категория:", 10, y + 30);
            _txtCategory = AddTextBox(120, y + 30);

            AddLabel("Ингредиент:", 10, y + 60);
            _txtIngredient = AddTextBox(120, y + 60);

            AddLabel("Время (мин):", 10, y + 90);
            _txtTime = AddTextBox(120, y + 90);

            AddLabel("Порций:", 10, y + 120);
            _txtServings = AddTextBox(120, y + 120);

            AddLabel("Калорий:", 10, y + 150);
            _txtCalories = AddTextBox(120, y + 150);

            // Кнопки CRUD
            AddButton("Добавить", 340, y, 130, (s, e) => AddRecipe());
            AddButton("Редактировать", 340, y + 35, 130, (s, e) => EditRecipe());
            AddButton("Удалить", 340, y + 70, 130, (s, e) => DeleteRecipe());
            AddButton("По категориям", 340, y + 105, 130, (s, e) => ShowGrouped());

            // Поиск
            AddLabel("Поиск по ингредиенту:", 500, y + 105);
            _txtSearch = new TextBox { Left = 660, Top = y + 105, Width = 150 };
            Controls.Add(_txtSearch);

            AddButton("Найти", 820, y + 105, 80, (s, e) => SearchByIngredient());
            AddButton("Сброс", 820, y + 140, 80, (s, e) => RefreshGrid());

            // Подсказка про категории
            var lblHint = new Label
            {
                Left = 10,
                Top = y + 190,
                Width = 900,
                Text = "Допустимые категории: " + string.Join(", ", _logic.GetAllowedCategories())
            };
            Controls.Add(lblHint);
        }

        /// <summary>Добавляет Label на форму.</summary>
        private Label AddLabel(string text, int x, int y)
        {
            var lbl = new Label { Text = text, Left = x, Top = y, Width = 110 };
            Controls.Add(lbl);
            return lbl;
        }

        /// <summary>Добавляет TextBox на форму.</summary>
        private TextBox AddTextBox(int x, int y)
        {
            var tb = new TextBox { Left = x, Top = y, Width = 200 };
            Controls.Add(tb);
            return tb;
        }

        /// <summary>Добавляет кнопку на форму.</summary>
        private Button AddButton(string text, int x, int y, int width, EventHandler onClick)
        {
            var btn = new Button { Text = text, Left = x, Top = y, Width = width };
            btn.Click += onClick;
            Controls.Add(btn);
            return btn;
        }

        /// <summary>Обновляет таблицу данных.</summary>
        private void RefreshGrid()
        {
            _grid.DataSource = null;
            _grid.DataSource = _logic.ReadAll();
        }

        /// <summary>Заполняет поля ввода данными выбранной строки.</summary>
        private void FillInputsFromSelection()
        {
            if (_grid.SelectedRows.Count == 0) return;
            if (_grid.SelectedRows[0].DataBoundItem is not Recipe r) return;

            _txtName.Text = r.Name;
            _txtCategory.Text = r.Category;
            _txtIngredient.Text = r.MainIngredient;
            _txtTime.Text = r.CookingMinutes.ToString();
            _txtServings.Text = r.Servings.ToString();
            _txtCalories.Text = r.Calories.ToString();
        }

        /// <summary>Проверяет, что в поле введено корректное целое число.</summary>
        private bool TryParseInt(string text, string fieldName, out int value)
        {
            if (!int.TryParse(text, out value))
            {
                MessageBox.Show($"Поле «{fieldName}» должно содержать целое число.",
                    "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>Добавляет новый рецепт из полей ввода.</summary>
        private void AddRecipe()
        {
            if (!TryParseInt(_txtTime.Text, "Время", out int time)) return;
            if (!TryParseInt(_txtServings.Text, "Порций", out int servings)) return;
            if (!TryParseInt(_txtCalories.Text, "Калорий", out int calories)) return;

            try
            {
                _logic.Create(_txtName.Text, _txtCategory.Text, _txtIngredient.Text,
                    time, servings, calories);
                RefreshGrid();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Редактирует выбранный рецепт.</summary>
        private void EditRecipe()
        {
            if (_grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите рецепт в таблице.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var recipe = (Recipe)_grid.SelectedRows[0].DataBoundItem;

            if (!TryParseInt(_txtTime.Text, "Время", out int time)) return;
            if (!TryParseInt(_txtServings.Text, "Порций", out int servings)) return;
            if (!TryParseInt(_txtCalories.Text, "Калорий", out int calories)) return;

            try
            {
                _logic.Update(recipe.Id, _txtName.Text, _txtCategory.Text, _txtIngredient.Text,
                    time, servings, calories);
                RefreshGrid();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Удаляет выбранный рецепт.</summary>
        private void DeleteRecipe()
        {
            if (_grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите рецепт в таблице.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var recipe = (Recipe)_grid.SelectedRows[0].DataBoundItem;
            if (MessageBox.Show($"Удалить \"{recipe.Name}\"?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _logic.Delete(recipe.Id);
                RefreshGrid();
                ClearInputs();
            }
        }

        /// <summary>Показывает рецепты, сгруппированные по категориям.</summary>
        private void ShowGrouped()
        {
            var groups = _logic.GroupByCategory();
            if (groups.Count == 0)
            {
                MessageBox.Show("Список рецептов пуст.", "Группировка");
                return;
            }

            string result = string.Join("\n\n", groups.Select(g =>
                $"Категория: {g.Key}\n" +
                string.Join("\n", g.Value.Select(r => "  " + r))));

            MessageBox.Show(result, "Группировка по категориям");
        }

        /// <summary>Ищет рецепты по основному ингредиенту.</summary>
        private void SearchByIngredient()
        {
            var found = _logic.FindByIngredient(_txtSearch.Text);
            _grid.DataSource = null;
            _grid.DataSource = found;

            if (found.Count == 0)
                MessageBox.Show("Ничего не найдено.", "Поиск");
        }

        /// <summary>Очищает поля ввода.</summary>
        private void ClearInputs()
        {
            _txtName.Clear();
            _txtCategory.Clear();
            _txtIngredient.Clear();
            _txtTime.Clear();
            _txtServings.Clear();
            _txtCalories.Clear();
        }
    }
}