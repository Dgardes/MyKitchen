using MyKitchen_Core.Entities;
using MyKitchen_Core.Services;
using MyKitchen_Core.Tools;
using MyKitchen_Core.DTO.SearchDTOs;
using MyKitchen_UI.Properties;
using MyKitchen_UI.Interfaces;

namespace MyKitchen_UI
{
    public partial class MyRecipesControl : UserControl, IResizableContainer, IRefreshableTab
    {
        public event EventHandler? controlResizing;
        public event EventHandler? controlLoaded;
        public Size containerSize { get { return this.Size; } }

        

        private bool _isInitialized = false;
        private Recipe? _currentViewingRecipe = null;

        public MyRecipesControl()
        {
            InitializeComponent();
   
        }

        #region Initialization and Load

        public void OnTabActivated()
        {
            LoadFilterData();
            ExecuteSearch();
        }

        private void MyRecipesControl_Load(object? sender, EventArgs e)
        {
            controlLoaded?.Invoke(this, EventArgs.Empty);

            RP_FilterBorder.updatePanelSize(true);
            CheckedCB_FoodCategoryPanel.updatePanelSize(true);
            CheckedCB_CookBooksPanel.updatePanelSize(true);
            ShowRecipePanel.Visible = false;

            LoadFilterData();
            RegisterFilters();
            _isInitialized = true;

            ExecuteSearch();
        }

        private void LoadFilterData()
        {
            Response<List<Category>> categoriesRes = DataService.Instance.GetCategories(DataService.DataScope.Local);
            if (categoriesRes.status == Response<List<Category>>.responseStatus.success && categoriesRes.data != null)
            {
                ChackedLB_FoodCategories.Items.Clear();
                ChackedLB_FoodCategories.DisplayMember = "Name"; 
                foreach (Category cat in categoriesRes.data)
                {
                    ChackedLB_FoodCategories.Items.Add(cat);
                }
            }

            // 2. Завантаження кулінарних книг
            Response<List<CookBook>> cookBooksRes = DataService.Instance.GetCookBooks(DataService.DataScope.Local);
            if (cookBooksRes.status == Response<List<CookBook>>.responseStatus.success && cookBooksRes.data != null)
            {
                ChackedLB_CookBooks.Items.Clear();
                ChackedLB_CookBooks.DisplayMember = "Name";
                foreach (CookBook book in cookBooksRes.data)
                {
                    ChackedLB_CookBooks.Items.Add(book);
                }
            }
        }
        #endregion

        #region Search Logic

        private void RegisterFilters()
        {
            txtbox_RecipeName.TextChanged += OnFilterChanged;
            txtbox_ProductNames.TextChanged += OnFilterChanged;

            combox_FullPreparationTime.SelectedIndexChanged += OnFilterChanged;
            combox_ActivePreparationTime.SelectedIndexChanged += OnFilterChanged;

            combox_NumberOfServings.SelectedIndexChanged += OnFilterChanged;
            combox_CreationDate.SelectedIndexChanged += OnFilterChanged;
            combox_LastEdited.SelectedIndexChanged += OnFilterChanged;

            combox_CaloriesPerDish.SelectedIndexChanged += OnFilterChanged;
            combox_CaloriesPerServing.SelectedIndexChanged += OnFilterChanged;
            combox_DishProteins.SelectedIndexChanged += OnFilterChanged;
            combox_ServingProteins.SelectedIndexChanged += OnFilterChanged;
            combox_DishFats.SelectedIndexChanged += OnFilterChanged;
            combox_ServingFats.SelectedIndexChanged += OnFilterChanged;
            combox_DishCarbohydrates.SelectedIndexChanged += OnFilterChanged;
            combox_ServingCarbohydrates.SelectedIndexChanged += OnFilterChanged;

            ChackedLB_FoodCategories.ItemCheck += (s, e) => this.BeginInvoke(new MethodInvoker(ExecuteSearch));
            ChackedLB_CookBooks.ItemCheck += (s, e) => this.BeginInvoke(new MethodInvoker(ExecuteSearch));
        }

        private void OnFilterChanged(object? sender, EventArgs e)
        {
            if (!_isInitialized) return;
            ExecuteSearch();
        }

        private void ExecuteSearch()
        {
            SearchRecipeCriteria_dto criteria = new SearchRecipeCriteria_dto();
            
            // Назва
            if (!string.IsNullOrEmpty(txtbox_RecipeName.Text))
            { criteria.Name = txtbox_RecipeName.Text.Trim(); }


            if (combox_FullPreparationTime.SelectedIndex > 0 && !combox_FullPreparationTime.Text.Contains("Не вибрано"))
            {
                criteria.FullPreparationTime = GatherCookingTimeRange(combox_FullPreparationTime.Text); 
            }
            
            if (combox_ActivePreparationTime.SelectedIndex > 0 && !combox_ActivePreparationTime.Text.Contains("Не вибрано"))
            {
                criteria.ActivePreparationTime = GatherCookingTimeRange(combox_ActivePreparationTime.Text); 
            }

            // Дати
            if (combox_CreationDate.SelectedIndex > 0 && !combox_CreationDate.Text.Contains("Не вибрано"))
            {
                criteria.CreationDate = ParseDateComboBoxRange(combox_CreationDate.Text);
            }
            
            if (combox_LastEdited.SelectedIndex > 0 && !combox_LastEdited.Text.Contains("Не вибрано"))
            {
                criteria.LastEdited = ParseDateComboBoxRange(combox_LastEdited.Text);
            }

            // Порції
            if (combox_NumberOfServings.SelectedIndex > 0 && int.TryParse(combox_NumberOfServings.Text, out int servingsVal))
            {
                ValueRange<int> servingsRange = new ValueRange<int>();
                servingsRange.Min = servingsVal;
                servingsRange.Max = servingsVal == 10 ? int.MaxValue : servingsVal;
                criteria.Servings = servingsRange;
            }

            List<Guid> selectedCategories = new List<Guid>();
            foreach (object item in ChackedLB_FoodCategories.CheckedItems)
            {
                if (item is Category cat) selectedCategories.Add(cat.ID);
            }
            criteria.ReferencedCategories = selectedCategories;

            List<Guid> selectedCookBooks = new List<Guid>();
            foreach (object item in ChackedLB_CookBooks.CheckedItems)
            {
                if (item is CookBook book) selectedCookBooks.Add(book.ID);
            }
            
            //
            // TODO
            //
            //criteria.ReferencedCookBooks = selectedCookBooks;
            //

            if (combox_CaloriesPerDish.SelectedIndex > 0)
                criteria.DishCalories = ParseNutrientsComboBoxRange(combox_CaloriesPerDish.Text);
                
            if (combox_DishProteins.SelectedIndex > 0)
                criteria.DishProteins = ParseNutrientsComboBoxRange(combox_DishProteins.Text);

            if (combox_DishFats.SelectedIndex > 0)
                criteria.DishFats = ParseNutrientsComboBoxRange(combox_DishFats.Text);

            if (combox_DishCarbohydrates.SelectedIndex > 0)
                criteria.DishCarbohydrates = ParseNutrientsComboBoxRange(combox_DishCarbohydrates.Text);


            if (combox_CaloriesPerServing.SelectedIndex > 0)
                criteria.ServingCalories = ParseNutrientsComboBoxRange(combox_CaloriesPerServing.Text);
                
            if (combox_ServingProteins.SelectedIndex > 0)
                criteria.ServingProteins = ParseNutrientsComboBoxRange(combox_ServingProteins.Text);

            if (combox_ServingFats.SelectedIndex > 0)
                criteria.ServingFats = ParseNutrientsComboBoxRange(combox_ServingFats.Text);

            if (combox_ServingCarbohydrates.SelectedIndex > 0)
                criteria.ServingCarbohydrates = ParseNutrientsComboBoxRange(combox_ServingCarbohydrates.Text);


            // Запуск пошуку
            Response<List<Recipe>> response = DataService.Instance.SearchRecipes(criteria, DataService.DataScope.Local);
            if (response.status != Response<List<Recipe>>.responseStatus.error && response.data != null)
            {
                FLP_Recipes.Controls.Clear();

                Response<List<Product>> productsRes = DataService.Instance.GetProducts(DataService.DataScope.Local);
                List<Product> allProducts = productsRes.data ?? new List<Product>();

                foreach (Recipe recipe in response.data)
                {
                    double totalCalories = 0, totalProteins = 0, totalFats = 0, totalCarbs = 0;

                    if (recipe.Ingredients != null)
                    {
                        foreach (Ingredient ing in recipe.Ingredients)
                        {
                            if (ing.ReferencedProduct == Guid.Empty) continue;

                            Product? product = allProducts.FirstOrDefault(p => p.ID == ing.ReferencedProduct);
                            if (product != null)
                            {
                                double factor = ing.Weight / 100.0;
                                totalProteins += product.Proteins * factor;
                                totalFats += product.Fats * factor;
                                totalCarbs += product.Carbohydrates * factor;

                                Response<double> calRes = CaloriesCalculator.calculateIngredientCalories(product, ing.Weight);
                                if (calRes.status == Response<double>.responseStatus.success)
                                    totalCalories += calRes.data;
                            }
                        }
                    }

                    RecipeCardControl card = new RecipeCardControl(recipe, totalCalories, totalProteins, totalFats, totalCarbs);
                    
                    card.OpenClicked += (s, ev) =>
                    {
                        OpenRecipeView(recipe, totalCalories, totalProteins, totalFats, totalCarbs, allProducts);
                    };

                    FLP_Recipes.Controls.Add(card);
                }
            }
        }

        #endregion

        #region Recipe View Logic

        private void OpenRecipeView(Recipe recipe, double totalCal, double totalProt, double totalFat, double totalCarb, List<Product> allProducts)
        {
            _currentViewingRecipe = recipe;

            LB_RecipeName.Text = recipe.Name ?? "Без назви";
            LB_Description.Text = recipe.Description ?? "Опис відсутній.";
            LB_FullPreparationTime.Text = "Повний час приготування: " + (recipe.FullPreparationTime.TotalMinutes > 0 ? recipe.FullPreparationTime.TotalMinutes + " хв." : "Не вказано");
            LB_ActivePreparationTime.Text = "Активний час приготування: " + (recipe.ActivePreparationTime.TotalMinutes > 0 ? recipe.ActivePreparationTime.TotalMinutes + " хв." : "Не вказано");
            LB_Servings.Text = "Кількість порцій: " + recipe.Servings;
            label_CreationDate.Text = "Дата створення: " + recipe.CreationDate.ToString("dd.MM.yyyy HH:mm");
            label_LastEdited.Text = "Останнє редагування: " + recipe.LastEdited.ToString("dd.MM.yyyy HH:mm");

            Response<List<Category>> catRes = DataService.Instance.GetCategories(DataService.DataScope.Local);
            if (catRes.status == Response<List<Category>>.responseStatus.success && catRes.data != null && recipe.ReferencedCategories != null)
            {
                var recipeCategories = catRes.data.Where(c => recipe.ReferencedCategories.Contains(c.ID)).Select(c => c.Name).ToList();
                Label_Categories.Text = "Категорії: " + (recipeCategories.Count > 0 ? string.Join(", ", recipeCategories) : "Не вказано");
            }
            else
            {
                Label_Categories.Text = "Категорії: Не вказано";
            }

            if (!string.IsNullOrEmpty(recipe.MainPhotoPath) && File.Exists(recipe.MainPhotoPath))
            {
                try
                {
                    byte[] imageBytes = File.ReadAllBytes(recipe.MainPhotoPath);
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        MainPicBox.Image = Image.FromStream(ms);
                    }
                }
                catch { MainPicBox.Image = Resources.StockPhoto; }
            }
            else { MainPicBox.Image = Resources.StockPhoto; }

            label_CaloriesPerDish.Text = "Калорії на страву: " + totalCal.ToString("F0");
            label_DishProteins.Text = "Білки: " + totalProt.ToString("F1") + "г";
            label_DishFats.Text = "Жири: " + totalFat.ToString("F1") + "г";
            label_DishCarbohydrates.Text = "Вуглеводи: " + totalCarb.ToString("F1") + "г";

            double servings = recipe.Servings > 0 ? recipe.Servings : 1;
            label_CaloriesPerServing.Text = "Калорії на порцію: " + (totalCal / servings).ToString("F0");
            label_ServingProteins.Text = "Білки: " + (totalProt / servings).ToString("F1") + "г";
            label_ServingFats.Text = "Жири: " + (totalFat / servings).ToString("F1") + "г";
            label_ServingCarbohydrates.Text = "Вуглеводи: " + (totalCarb / servings).ToString("F1") + "г";

            Label_Link.Text = !string.IsNullOrEmpty(recipe.SourceLink) ? recipe.SourceLink : "Не вказано";

            DGV_Ingredients.Rows.Clear();
            int rowHeight = 25;
            int totalGridHeight = DGV_Ingredients.ColumnHeadersHeight;

            if (recipe.Ingredients != null)
            {
                foreach (Ingredient ing in recipe.Ingredients)
                {
                    string displayName = !string.IsNullOrEmpty(ing.RawName) ? ing.RawName : "Без назви";
                    double cal = 0, prot = 0, fat = 0, carb = 0;

                    if (ing.ReferencedProduct != Guid.Empty)
                    {
                        Product? prod = allProducts.FirstOrDefault(p => p.ID == ing.ReferencedProduct);
                        if (prod != null)
                        {
                            double factor = ing.Weight / 100.0;
                            prot = prod.Proteins * factor;
                            fat = prod.Fats * factor;
                            carb = prod.Carbohydrates * factor;
                            var calRes = CaloriesCalculator.calculateIngredientCalories(prod, ing.Weight);
                            if (calRes.status == Response<double>.responseStatus.success) cal = calRes.data;
                        }
                    }

                    DGV_Ingredients.Rows.Add(displayName, ing.Weight, cal.ToString("F1"), prot.ToString("F1"), fat.ToString("F1"), carb.ToString("F1"));
                    totalGridHeight += rowHeight;
                }
            }
            
            DGV_Ingredients.Height = totalGridHeight;
            IngredientsPanel.Height = totalGridHeight + CaloriesValPanel.Height + 20;

            FLP_RecipeSteps.Controls.Clear();
            if (recipe.PreparationSteps != null && recipe.PreparationSteps.Count > 0)
            {
                int stepNum = 1;
                foreach (Step step in recipe.PreparationSteps)
                {
                    StepViewControl sv = new StepViewControl(stepNum, step.Description, step.PhotoPath);
                    FLP_RecipeSteps.Controls.Add(sv);
                    stepNum++;
                }
            }

            // 2. Налаштовуємо Label_Author
            if (recipe.AuthorID == AppSession.Instance.CurrentUser?.ID)
            {
                Label_Author.Text = $"Автор: {AppSession.Instance.CurrentUser.Name} (Ви)";
            }
            else
            {
                // Рецепт було запозичено у хмарі, дізнаємося ім'я оригінального розробника
                var authorRes = DataService.Instance.GetAuthorById(recipe.AuthorID);
                Label_Author.Text = "Автор: " + ((authorRes.status == Response<UserProfile>.responseStatus.success && authorRes.data != null) 
                                    ? authorRes.data.Name 
                                    : "Невідомий автор");
            }

            // 3. Налаштовуємо ланцюжок натхнення (Label_InspiredBy)
            if (recipe.InspiredBy != Guid.Empty)
            {
                // Робимо точковий запит до глобальної бази, щоб знайти оригінальний рецепт-першоджерело
                var originalRecipeRes = DataService.Instance.GetRecipe(recipe.InspiredBy, DataService.DataScope.Global);

                if (originalRecipeRes.status == Response<Recipe>.responseStatus.success && originalRecipeRes.data != null)
                {
                    Recipe originalRecipe = originalRecipeRes.data;

                    // Зчитуємо ім'я автора оригінальної страви
                    var origAuthorRes = DataService.Instance.GetAuthorById(originalRecipe.AuthorID);
                    string origAuthorName = (origAuthorRes.status == Response<UserProfile>.responseStatus.success && origAuthorRes.data != null) 
                                            ? origAuthorRes.data.Name 
                                            : "Невідомий автор";

                    Label_InspiredBy.Text = $"За мотивами рецепта: {origAuthorName} – {originalRecipe.Name}";
                    Label_InspiredBy.Visible = true;
                }
                else
                {
                    // Якщо оригінальну страву повністю видалили з глобального хмарного сховища
                    Label_InspiredBy.Text = "";
                    Label_InspiredBy.Visible = false;
                }
            }
            else
            {
                // Якщо локальний рецепт є повністю автентичним
                Label_InspiredBy.Text = "";
                Label_InspiredBy.Visible = false;
            }

            ShowRecipePanel.Visible = true;
            if (ShowRecipePanel.isClosed)
            {
                ShowRecipePanel.updatePanelSizeOnClick();
            }

            ShowRecipePanel.BringToFront();
        }

        public void ViewRecipeExternal(Recipe recipe)
        {
            if (recipe == null) return;

            Response<List<Product>> productsRes = DataService.Instance.GetProducts(DataService.DataScope.Local);
            List<Product> allProducts = productsRes.data ?? new List<Product>();

            double totalCalories = 0, totalProteins = 0, totalFats = 0, totalCarbs = 0;

            if (recipe.Ingredients != null)
            {
                foreach (Ingredient ing in recipe.Ingredients)
                {
                    if (ing.ReferencedProduct == Guid.Empty) continue;

                    Product? product = allProducts.FirstOrDefault(p => p.ID == ing.ReferencedProduct);
                    if (product != null)
                    {
                        double factor = ing.Weight / 100.0;
                        totalProteins += product.Proteins * factor;
                        totalFats += product.Fats * factor;
                        totalCarbs += product.Carbohydrates * factor;

                        var calRes = CaloriesCalculator.calculateIngredientCalories(product, ing.Weight);
                        if (calRes.status == Response<double>.responseStatus.success)
                            totalCalories += calRes.data;
                    }
                }
            }

            OpenRecipeView(recipe, totalCalories, totalProteins, totalFats, totalCarbs, allProducts);
        }

        private void BTN_CloseRecipe_Click(object? sender, EventArgs e)
        {
            ShowRecipePanel.Visible = false;
        }

        private void BTN_Edit_Click(object? sender, EventArgs e)
        {
            if(_currentViewingRecipe != null && MainForm.mainForm != null)
            {
                MainForm.mainForm.OpenRecipeInEditMode(_currentViewingRecipe);
                ShowRecipePanel.Visible = false;
            }
        }

        #endregion

        #region combobox parsers
        private ValueRange<int> ParseNutrientsComboBoxRange(string comboBoxText)
        {
            ValueRange<int> range = new ValueRange<int>();

            if (string.IsNullOrEmpty(comboBoxText) || comboBoxText.Contains("Не вибрано"))
            {
                range.Min = 0;
                range.Max = int.MaxValue;
                return range;
            }

            string clean = comboBoxText.Replace("ккал", "").Replace("г.", "").Replace("г", "").Trim();

            if (clean.Contains("-"))
            {
                string[] parts = clean.Split('-');
                int.TryParse(parts[0].Trim(), out int minVal);
                int.TryParse(parts[1].Trim(), out int maxVal);

                range.Min = minVal;
                range.Max = maxVal;
            }
            else if (clean.Contains("+"))
            {
                int.TryParse(clean.Replace("+", "").Trim(), out int minVal);

                range.Min = minVal;
                range.Max = int.MaxValue;
            }

            return range;
        }

        private ValueRange<TimeSpan> GatherCookingTimeRange(string text)
        {
            ValueRange<TimeSpan> range = new ValueRange<TimeSpan>();

            if (string.IsNullOrEmpty(text) || text.Contains("Не вибрано"))
            {
                range.Min = TimeSpan.Zero;
                range.Max = TimeSpan.MaxValue;
                return range;
            }

            if (text.Contains("до 15 хв."))
            {
                range.Min = TimeSpan.Zero;
                range.Max = TimeSpan.FromMinutes(15);
            }
            else if (text.Contains("15 - 30"))
            {
                range.Min = TimeSpan.FromMinutes(15);
                range.Max = TimeSpan.FromMinutes(30);
            }
            else if (text.Contains("30 - 60 хв."))
            {
                range.Min = TimeSpan.FromMinutes(30);
                range.Max = TimeSpan.FromMinutes(60);
            }
            else if (text.Contains("1 - 1.5"))
            {
                range.Min = TimeSpan.FromHours(1);
                range.Max = TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(30));
            }
            else if (text.Contains("1.5 - 2"))
            {
                range.Min = TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(30));
                range.Max = TimeSpan.FromHours(2);
            }
            else if (text.Contains("2 - 3"))
            {
                range.Min = TimeSpan.FromHours(2);
                range.Max = TimeSpan.FromHours(3);
            }
            else if (text.Contains("3 - 5"))
            {
                range.Min = TimeSpan.FromHours(3);
                range.Max = TimeSpan.FromHours(5);
            }
            else if (text.Contains("5 - 7"))
            {
                range.Min = TimeSpan.FromHours(5);
                range.Max = TimeSpan.FromHours(7);
            }
            else if (text.Contains("8 - 10"))
            {
                range.Min = TimeSpan.FromHours(8);
                range.Max = TimeSpan.FromHours(10);
            }
            else if (text.Contains("10+"))
            {
                range.Min = TimeSpan.FromHours(10);
                range.Max = TimeSpan.MaxValue;
            }

            return range;
        }

        private ValueRange<DateTime> ParseDateComboBoxRange(string text)
        {
            ValueRange<DateTime> range = new ValueRange<DateTime>();
            DateTime now = DateTime.Now;

            if (string.IsNullOrEmpty(text) || text.Contains("Не вибрано"))
            {
                range.Min = DateTime.MinValue;
                range.Max = DateTime.MaxValue;
                return range;
            }

            if (text.Contains("Сьогодні"))
            {
                range.Min = now.Date;
                range.Max = now;
            }
            else if (text.Contains("Учора"))
            {
                range.Min = now.Date.AddDays(-1);
                range.Max = now.Date.AddTicks(-1);
            }
            else if (text.Contains("Цього тижня"))
            {
                range.Min = now.AddDays(-7);
                range.Max = now;
            }
            else if (text.Contains("Цього місяця"))
            {
                range.Min = now.AddMonths(-1);
                range.Max = now;
            }
            else if (text.Contains("Більше місяця тому"))
            {
                range.Min = DateTime.MinValue;
                range.Max = now.AddMonths(-1);
            }
            else if (text.Contains("Більше 3 місяців тому"))
            {
                range.Min = DateTime.MinValue;
                range.Max = now.AddMonths(-3);
            }
            else if (text.Contains("Більше 6 місяців тому"))
            {
                range.Min = DateTime.MinValue;
                range.Max = now.AddMonths(-6);
            }
            else if (text.Contains("Більше року тому"))
            {
                range.Min = DateTime.MinValue;
                range.Max = now.AddYears(-1);
            }
            else if (text.Contains("Більше 2 років тому"))
            {
                range.Min = DateTime.MinValue;
                range.Max = now.AddYears(-2);
            }

            return range;
        }

        #endregion

        #region Guna Panel Management

        private void MyRecipesControl_Resize(object sender, EventArgs e) { controlResizing?.Invoke(this, EventArgs.Empty); }

        private void BTN_OpenFilter_Click(object sender, EventArgs e)
        {
            RP_FilterBorder.updatePanelSizeOnClick();
            BTN_OpenFilter.Image = RP_FilterBorder.isClosed ? Properties.Resources.OpenFilter_Icon_Down : Properties.Resources.OpenFilter_Icon_Up;
        }

        private void BTN_OpenCategories_Click(object sender, EventArgs e)
        {
            CheckedCB_FoodCategoryPanel.updatePanelSizeOnClick();
            BTN_OpenCategories.Image = CheckedCB_FoodCategoryPanel.isClosed ? Properties.Resources.OpenFilter_Icon_Down : Properties.Resources.OpenFilter_Icon_Up;
        }

        private void BTN_OpenCookBooks_Click(object sender, EventArgs e)
        {
            CheckedCB_CookBooksPanel.updatePanelSizeOnClick();
            BTN_OpenCookBooks.Image = CheckedCB_CookBooksPanel.isClosed ? Properties.Resources.OpenFilter_Icon_Down : Properties.Resources.OpenFilter_Icon_Up;
        }

        private void ShowRecipePanel_Paint(object sender, PaintEventArgs e) { }

        #endregion
    }
}