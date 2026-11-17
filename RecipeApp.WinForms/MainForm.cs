using System;
using System.Linq;
using System.Windows.Forms;
using Ninject;
using RecipeApp.BusinessLogic;
using RecipeApp.Model;

namespace RecipeApp.WinForms
{
    public partial class MainForm : Form
    {
        private readonly Logic _logic;
        private readonly RecipeCrudService _crudService;

        private DataGridView _grid = null!;
        private TextBox _txtName = null!, _txtCategory = null!, _txtIngredient = null!, _txtTime = null!, _txtServings = null!, _txtCalories = null!, _txtSearch = null!;

        public MainForm()
        {
            InitializeComponent();

            var kernel = new StandardKernel(new SimpleConfigModule());
            _logic = kernel.Get<Logic>();
            _crudService = kernel.Get<RecipeCrudService>();

            SeedData();
            BuildUi();
            RefreshGrid();
        }

        private void SeedData()
        {
            try
            {
                _crudService.Create("Борщ", "Суп", "Свёкла", 90, 4, 120);
                _crudService.Create("Цезарь", "Салат", "Курица", 20, 2, 250);
                _crudService.Create("Тирамису", "Десерт", "Маскарпоне", 60, 6, 350);
                _crudService.Create("Паста Карбонара", "Основное", "Спагетти", 25, 2, 480);
            }
            catch { }
        }

        private void BuildUi()
        {
            Text = "Книга рецептов (WinForms)";
            Width = 1000; Height = 650;

            
            _grid = new DataGridView
            {
                Left = 10,
                Top = 10,
                Width = 960,
                Height = 300,
                AutoGenerateColumns = false, 
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 40 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Название", Width = 150 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Category", HeaderText = "Категория", Width = 100 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MainIngredient", HeaderText = "Ингредиент", Width = 120 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CookingMinutes", HeaderText = "Время (мин)", Width = 90 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Servings", HeaderText = "Порций", Width = 70 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Calories", HeaderText = "Калорий", Width = 80 });

            _grid.SelectionChanged += (s, e) => FillInputsFromSelection();
            Controls.Add(_grid);

            int y = 330;
            AddLabel("Название:", 10, y); _txtName = AddTextBox(120, y);
            AddLabel("Категория:", 10, y + 30); _txtCategory = AddTextBox(120, y + 30);
            AddLabel("Ингредиент:", 10, y + 60); _txtIngredient = AddTextBox(120, y + 60);
            AddLabel("Время (мин):", 10, y + 90); _txtTime = AddTextBox(120, y + 90);
            AddLabel("Порций:", 10, y + 120); _txtServings = AddTextBox(120, y + 120);
            AddLabel("Калорий:", 10, y + 150); _txtCalories = AddTextBox(120, y + 150);

            AddButton("Добавить", 340, y, 130, (s, e) => AddRecipe());
            AddButton("Редактировать", 340, y + 35, 130, (s, e) => EditRecipe());
            AddButton("Удалить", 340, y + 70, 130, (s, e) => DeleteRecipe());
            AddButton("По категориям", 340, y + 105, 130, (s, e) => ShowGrouped());

            AddLabel("Поиск по ингредиенту:", 500, y + 105);
            _txtSearch = new TextBox { Left = 660, Top = y + 105, Width = 150 };
            Controls.Add(_txtSearch);

            AddButton("Найти", 820, y + 105, 80, (s, e) => SearchByIngredient());
            AddButton("Сброс", 820, y + 140, 80, (s, e) => RefreshGrid());

            Controls.Add(new Label { Left = 10, Top = y + 190, Width = 900, Text = "Допустимые категории: " + string.Join(", ", _logic.GetAllowedCategories()) });
        }

        private Label AddLabel(string text, int x, int y)
        {
            var lbl = new Label { Text = text, Left = x, Top = y, Width = 110 };
            Controls.Add(lbl); return lbl;
        }

        private TextBox AddTextBox(int x, int y)
        {
            var tb = new TextBox { Left = x, Top = y, Width = 200 };
            Controls.Add(tb); return tb;
        }

        private void AddButton(string text, int x, int y, int width, EventHandler onClick)
        {
            var btn = new Button { Text = text, Left = x, Top = y, Width = width };
            btn.Click += onClick;
            Controls.Add(btn);
        }

        private void RefreshGrid()
        {
            _grid.DataSource = null;
            _grid.DataSource = _crudService.ReadAll();
        }

        private void FillInputsFromSelection()
        {
            if (_grid.SelectedRows.Count == 0 || _grid.SelectedRows[0].DataBoundItem is not Recipe r) return;
            _txtName.Text = r.Name; _txtCategory.Text = r.Category; _txtIngredient.Text = r.MainIngredient;
            _txtTime.Text = r.CookingMinutes.ToString(); _txtServings.Text = r.Servings.ToString(); _txtCalories.Text = r.Calories.ToString();
        }

        private bool TryParseInt(string text, string fieldName, out int value)
        {
            if (!int.TryParse(text, out value))
            {
                MessageBox.Show($"Поле «{fieldName}» должно содержать целое число.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void AddRecipe()
        {
            if (!TryParseInt(_txtTime.Text, "Время", out int time) || !TryParseInt(_txtServings.Text, "Порций", out int servings) || !TryParseInt(_txtCalories.Text, "Калорий", out int calories)) return;
            try
            {
                _crudService.Create(_txtName.Text, _txtCategory.Text, _txtIngredient.Text, time, servings, calories);
                RefreshGrid(); ClearInputs();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void EditRecipe()
        {
            if (_grid.SelectedRows.Count == 0) { MessageBox.Show("Выберите рецепт."); return; }
            var recipe = (Recipe)_grid.SelectedRows[0].DataBoundItem;
            if (!TryParseInt(_txtTime.Text, "Время", out int time) || !TryParseInt(_txtServings.Text, "Порций", out int servings) || !TryParseInt(_txtCalories.Text, "Калорий", out int calories)) return;
            try
            {
                _crudService.Update(recipe.Id, _txtName.Text, _txtCategory.Text, _txtIngredient.Text, time, servings, calories);
                RefreshGrid(); ClearInputs();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void DeleteRecipe()
        {
            if (_grid.SelectedRows.Count == 0) { MessageBox.Show("Выберите рецепт."); return; }
            var recipe = (Recipe)_grid.SelectedRows[0].DataBoundItem;
            if (MessageBox.Show($"Удалить \"{recipe.Name}\"?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _crudService.Delete(recipe.Id);
                RefreshGrid(); ClearInputs();
            }
        }

        private void ShowGrouped()
        {
            var groups = _logic.GroupByCategory();
            string result = string.Join("\n\n", groups.Select(g => $"Категория: {g.Key}\n" + string.Join("\n", g.Value.Select(r => "  " + r))));
            MessageBox.Show(result, "Группировка");
        }

        private void SearchByIngredient()
        {
            var found = _logic.FindByIngredient(_txtSearch.Text);
            _grid.DataSource = null; _grid.DataSource = found;
            if (found.Count == 0) MessageBox.Show("Ничего не найдено.");
        }

        private void ClearInputs()
        {
            _txtName.Clear(); _txtCategory.Clear(); _txtIngredient.Clear(); _txtTime.Clear(); _txtServings.Clear(); _txtCalories.Clear();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}