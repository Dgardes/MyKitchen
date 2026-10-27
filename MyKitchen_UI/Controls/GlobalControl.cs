using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MyKitchen_Core.Entities;
using MyKitchen_Core.Services;
using MyKitchen_Core.Tools;
using MyKitchen_Core.DTO.CreateAndUpdateDTOs;
using MyKitchen_Core.DTO.SearchDTOs;
using MyKitchen_UI.Properties;
using MyKitchen_UI.Interfaces;

namespace MyKitchen_UI.Controls
{
    public partial class GlobalControl :UserControl, IResizableContainer, IRefreshableTab
    {
        #region Interfaces & Variables

        public event EventHandler? controlResizing;
        public event EventHandler? controlLoaded;
        public Size containerSize { get { return this.Size; } }

        private bool _isInitialized = false;
        private bool _isLoadingData = false;

        private Recipe? _currentViewingRecipe = null;
        private CookBook? _currentCookBook = null;

        // Кеші для швидкої роботи UI
        private List<Recipe> _globalRecipesCache = new List<Recipe>();
        private List<Product> _allProducts = new List<Product>();
        private List<UserProfile> _allUsersCache = new List<UserProfile>();

        #endregion

        public GlobalControl()
        {
        InitializeComponent();
        this.DoubleBuffered = true;
        this.Load += GlobalControl_Load;
        this.Resize += GlobalControl_Resize;
        }

        #region Initialization & Data Loading

        public void OnTabActivated()
        {
        LoadGlobalDataCaches();
        LoadFilterData();
        ExecuteActiveSearch();
        if(RecipesPanel.Visible)
        {
        ExecuteRecipeSearch();
        }
        else
        {
        ExecuteBooksSearch();
        }
        }

        private void GlobalControl_Load(object sender, EventArgs e)
        {
        controlLoaded?.Invoke(this, EventArgs.Empty);

        RP_FilterBorder_R.updatePanelSize(true);
        RP_FilterBorder_K.updatePanelSize(true);
        CheckedCB_FoodCategoryPanel.updatePanelSize(true);

        ShowRecipePanel.Visible = false;
        BTN_ShowRecipe.Visible = false;
        BTN_DownloadCookBook.Visible = false;

        LoadGlobalDataCaches();
        LoadFilterData();
        RegisterFilters();

        _isInitialized = true;
        SwitchTab(true); // За замовчуванням відкриваємо рецепти
        }

        // Завантаження масивів даних для розрахунків і відображення списків
        private void LoadGlobalDataCaches()
        {
        _isLoadingData = true;

        // 🔥 ФІКС АРХІТЕКТУРИ: Замість падучого GetRecipes(Global) використовуємо стабільний SearchRecipes
        // Шукаємо лише публічні рецепти з хмари, ініціалізуючи списки для захисту від NullReference всередині API
        SearchRecipeCriteria_dto stableCriteria = new SearchRecipeCriteria_dto
        {
            ReferencedCategories = new List<Guid>(),
            IsPublic = true
        };

        Response<List<Recipe>> recipesRes = DataService.Instance.SearchRecipes(stableCriteria, DataService.DataScope.Global);
        if(recipesRes.status != Response<List<Recipe>>.responseStatus.error && recipesRes.data != null)
        {
        _globalRecipesCache = recipesRes.data;
        }

        // 2. Всі локальні продукти (для розрахунку КБЖУ глобальних рецептів)
        Response<List<Product>> productsRes = DataService.Instance.GetProducts(DataService.DataScope.Local);
        if(productsRes.status == Response<List<Product>>.responseStatus.success && productsRes.data != null)
        {
        _allProducts = productsRes.data;
        }

        _isLoadingData = false;
        }

        // Завантаження даних для комбобоксів (Категорії)
        private void LoadFilterData()
        {
        Response<List<Category>> categoriesRes = DataService.Instance.GetCategories(DataService.DataScope.Local);
        if(categoriesRes.status == Response<List<Category>>.responseStatus.success && categoriesRes.data != null)
        {
        ChackedLB_FoodCategories.Items.Clear();
        ChackedLB_FoodCategories.DisplayMember = "Name";
        foreach(Category cat in categoriesRes.data)
        {
        ChackedLB_FoodCategories.Items.Add(cat);
        }
        }
        }

        #endregion

        #region Tab Switching (Recipes / Cookbooks)

        private void BTN_RecipesPage_Click(object? sender, EventArgs e) => SwitchTab(true);
        private void BTN_CookBooksPage_Click(object? sender, EventArgs e) => SwitchTab(false);

        private void SwitchTab(bool isRecipesTab)
        {
        RecipesPanel.Visible = isRecipesTab;
        CookBooksPanel.Visible = !isRecipesTab;
        ShowRecipePanel.Visible = false; // При перемиканні ховаємо перегляд рецепту

        Color activeBg = Color.White;
        Color activeText = Color.FromArgb(240, 112, 22);
        Color inactiveBg = Color.FromArgb(240, 112, 22);
        Color inactiveText = Color.White;

        if(isRecipesTab)
        {
        BTN_RecipesPage.FillColor = activeBg;
        BTN_RecipesPage.ForeColor = activeText;
        BTN_CookBooksPage.FillColor = inactiveBg;
        BTN_CookBooksPage.ForeColor = inactiveText;
        }
        else
        {
        BTN_CookBooksPage.FillColor = activeBg;
        BTN_CookBooksPage.ForeColor = activeText;
        BTN_RecipesPage.FillColor = inactiveBg;
        BTN_RecipesPage.ForeColor = inactiveText;
        }

        if(_isInitialized) ExecuteActiveSearch();
        }

        private void ExecuteActiveSearch()
        {
        if(RecipesPanel.Visible) ExecuteRecipeSearch();
        else ExecuteBooksSearch();
        }

        #endregion

        #region Recipes Search & Filters

        private void RegisterFilters()
        {
        // Фільтри рецептів
        txtbox_RecipeName.TextChanged += OnFilterChanged;
        txtbox_ProductNames.TextChanged += OnFilterChanged;
        combox_FullPreparationTime.SelectedIndexChanged += OnFilterChanged;
        combox_ActivePreparationTime.SelectedIndexChanged += OnFilterChanged;
        combox_NumberOfServings.SelectedIndexChanged += OnFilterChanged;
        combox_CreationDate_R.SelectedIndexChanged += OnFilterChanged;
        combox_LastEdited_R.SelectedIndexChanged += OnFilterChanged;
        combox_CaloriesPerDish.SelectedIndexChanged += OnFilterChanged;
        combox_CaloriesPerServing.SelectedIndexChanged += OnFilterChanged;
        combox_DishProteins.SelectedIndexChanged += OnFilterChanged;
        combox_ServingProteins.SelectedIndexChanged += OnFilterChanged;
        combox_DishFats.SelectedIndexChanged += OnFilterChanged;
        combox_ServingFats.SelectedIndexChanged += OnFilterChanged;
        combox_DishCarbohydrates.SelectedIndexChanged += OnFilterChanged;
        combox_ServingCarbohydrates.SelectedIndexChanged += OnFilterChanged;

        ChackedLB_FoodCategories.ItemCheck += (s, e) => this.BeginInvoke(new MethodInvoker(ExecuteRecipeSearch));

        // Фільтри книг
        txtbox_CookBookName.TextChanged += OnFilterChanged;
        txtbox_FindByRecipeName.TextChanged += OnFilterChanged;
        txtbox_Author.TextChanged += OnFilterChanged;
        combox_CreationDate_K.SelectedIndexChanged += OnFilterChanged;
        combox_LastEdited_K.SelectedIndexChanged += OnFilterChanged;
        }

        private void OnFilterChanged(object? sender, EventArgs e)
        {
        if(!_isInitialized) return;
        ExecuteActiveSearch();
        }

        private void ExecuteRecipeSearch()
        {
        SearchRecipeCriteria_dto criteria = new SearchRecipeCriteria_dto();

        if(!string.IsNullOrEmpty(txtbox_RecipeName.Text)) criteria.Name = txtbox_RecipeName.Text.Trim();

        if(combox_FullPreparationTime.SelectedIndex > 0 && !combox_FullPreparationTime.Text.Contains("Не вибрано"))
            criteria.FullPreparationTime = GatherCookingTimeRange(combox_FullPreparationTime.Text);
        if(combox_ActivePreparationTime.SelectedIndex > 0 && !combox_ActivePreparationTime.Text.Contains("Не вибрано"))
            criteria.ActivePreparationTime = GatherCookingTimeRange(combox_ActivePreparationTime.Text);

        if(combox_CreationDate_R.SelectedIndex > 0 && !combox_CreationDate_R.Text.Contains("Не вибрано"))
            criteria.CreationDate = ParseDateComboBoxRange(combox_CreationDate_R.Text);
        if(combox_LastEdited_R.SelectedIndex > 0 && !combox_LastEdited_R.Text.Contains("Не вибрано"))
            criteria.LastEdited = ParseDateComboBoxRange(combox_LastEdited_R.Text);

        if(combox_NumberOfServings.SelectedIndex > 0 && int.TryParse(combox_NumberOfServings.Text, out int servingsVal))
        {
        ValueRange<int> servingsRange = new ValueRange<int>();
        servingsRange.Min = servingsVal;
        servingsRange.Max = servingsVal == 10 ? int.MaxValue : servingsVal;
        criteria.Servings = servingsRange;
        }

        List<Guid> selectedCategories = new List<Guid>();
        foreach(object item in ChackedLB_FoodCategories.CheckedItems)
        {
        if(item is Category cat) selectedCategories.Add(cat.ID);
        }
        criteria.ReferencedCategories = selectedCategories;

        if(combox_CaloriesPerDish.SelectedIndex > 0) criteria.DishCalories = ParseNutrientsComboBoxRange(combox_CaloriesPerDish.Text);
        if(combox_DishProteins.SelectedIndex > 0) criteria.DishProteins = ParseNutrientsComboBoxRange(combox_DishProteins.Text);
        if(combox_DishFats.SelectedIndex > 0) criteria.DishFats = ParseNutrientsComboBoxRange(combox_DishFats.Text);
        if(combox_DishCarbohydrates.SelectedIndex > 0) criteria.DishCarbohydrates = ParseNutrientsComboBoxRange(combox_DishCarbohydrates.Text);
        if(combox_CaloriesPerServing.SelectedIndex > 0) criteria.ServingCalories = ParseNutrientsComboBoxRange(combox_CaloriesPerServing.Text);
        if(combox_ServingProteins.SelectedIndex > 0) criteria.ServingProteins = ParseNutrientsComboBoxRange(combox_ServingProteins.Text);
        if(combox_ServingFats.SelectedIndex > 0) criteria.ServingFats = ParseNutrientsComboBoxRange(combox_ServingFats.Text);
        if(combox_ServingCarbohydrates.SelectedIndex > 0) criteria.ServingCarbohydrates = ParseNutrientsComboBoxRange(combox_ServingCarbohydrates.Text);
        if(!string.IsNullOrWhiteSpace(txtbox_ProductNames.Text))
        {
        criteria.ProductsAliases = txtbox_ProductNames.Text
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim().ToLower())
            .ToList();
        }

        criteria.IsPublic = true; // Глобальний пошук лише по паблікам

        Response<List<Recipe>> response = DataService.Instance.SearchRecipes(criteria, DataService.DataScope.Global);

        if(response.status != Response<List<Recipe>>.responseStatus.error && response.data != null)
        {
        FLP_Recipes.Controls.Clear();

        foreach(Recipe recipe in response.data)
        {
        double totalCal = 0, totalProt = 0, totalFat = 0, totalCarb = 0;
        CalculateMacrosForRecipe(recipe, out totalCal, out totalProt, out totalFat, out totalCarb);

        RecipeCardControl card = new RecipeCardControl(recipe, totalCal, totalProt, totalFat, totalCarb);

        card.OpenClicked += (s, ev) =>
        {
        OpenRecipeView(recipe, totalCal, totalProt, totalFat, totalCarb);
        };

        FLP_Recipes.Controls.Add(card);
        }
        }
        }

        #endregion

        #region CookBooks Search & Master-Detail

        private void ExecuteBooksSearch()
        {
        SearchCookBookCriteria_dto criteria = new SearchCookBookCriteria_dto();

        if(!string.IsNullOrEmpty(txtbox_CookBookName.Text)) criteria.Name = txtbox_CookBookName.Text.Trim();
        if(!string.IsNullOrEmpty(txtbox_FindByRecipeName.Text)) criteria.RecipeName = txtbox_FindByRecipeName.Text.Trim();
        if(!string.IsNullOrEmpty(txtbox_Author.Text)) criteria.AuthorName = txtbox_Author.Text.Trim();

        if(combox_CreationDate_K.SelectedIndex > 0 && !combox_CreationDate_K.Text.Contains("Не вибрано"))
            criteria.CreationDate = ParseDateComboBoxRange(combox_CreationDate_K.Text);
        if(combox_LastEdited_K.SelectedIndex > 0 && !combox_LastEdited_K.Text.Contains("Не вибрано"))
            criteria.LastEdited = ParseDateComboBoxRange(combox_LastEdited_K.Text);

        criteria.IsPublic = true;

        Response<List<CookBook>> response = DataService.Instance.SearchCookBooks(criteria, DataService.DataScope.Global);

        if(response.status != Response<List<CookBook>>.responseStatus.error && response.data != null)
        {
        ListBox_CookBooks.Items.Clear();
        ListBox_CookBooks.DisplayMember = "Name";

        foreach(CookBook book in response.data)
        {
        ListBox_CookBooks.Items.Add(book);
        }
        }
        }

        private void ListBox_CookBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
        if(ListBox_CookBooks.SelectedItem is CookBook selectedBook)
        {
        _currentCookBook = selectedBook;

        label_CookBookName.Text = "Назва книги: " + selectedBook.Name;
        label_CookBookDescription.Text = selectedBook.Description ?? "Опис відсутній.";
        Label_CreatedDate_CookBook.Text = "Створено: " + selectedBook.CreationDate.ToString("g");
        Labal_LastEditedCookBook.Text = "Змінено: " + selectedBook.LastEdited.ToString("g");

        ListBox_RecipesRefs.Items.Clear();
        ListBox_RecipesRefs.DisplayMember = "Name";

        // Примусово оновлюємо глобальний кеш перед мапінгом списку страв у книзі
        LoadGlobalDataCaches();

        if(selectedBook.ReferencedRecipes != null)
        {
        foreach(Guid recipeId in selectedBook.ReferencedRecipes)
        {
        Recipe? recipe = _globalRecipesCache.FirstOrDefault(r => r.ID == recipeId);
        if(recipe != null)
        {
        ListBox_RecipesRefs.Items.Add(recipe);
        }
        }
        }

        BTN_ShowRecipe.Visible = false;
        BTN_DownloadCookBook.Visible = true;
        }
        }

        private void ListBox_RecipesRefs_SelectedIndexChanged(object sender, EventArgs e)
        {
        if(ListBox_RecipesRefs.SelectedItem is Recipe)
        {
        BTN_ShowRecipe.Visible = true;
        }
        }

        private void BTN_ShowRecipe_Click(object sender, EventArgs e)
        {
        if(ListBox_RecipesRefs.SelectedItem is Recipe selectedRecipe)
        {
        double totalCal = 0, totalProt = 0, totalFat = 0, totalCarb = 0;
        CalculateMacrosForRecipe(selectedRecipe, out totalCal, out totalProt, out totalFat, out totalCarb);
        OpenRecipeView(selectedRecipe, totalCal, totalProt, totalFat, totalCarb);
        }
        }

        #endregion

        #region Recipe View Logic (ShowRecipePanel)

        private void CalculateMacrosForRecipe(Recipe recipe, out double cal, out double prot, out double fat, out double carb)
        {
        cal = 0; prot = 0; fat = 0; carb = 0;
        if(recipe.Ingredients == null) return;

        foreach(Ingredient ing in recipe.Ingredients)
        {
        if(ing.ReferencedProduct == Guid.Empty) continue;
        Product? product = _allProducts.FirstOrDefault(p => p.ID == ing.ReferencedProduct);
        if(product != null)
        {
        double factor = ing.Weight / 100.0;
        prot += product.Proteins * factor;
        fat += product.Fats * factor;
        carb += product.Carbohydrates * factor;

        var calRes = CaloriesCalculator.calculateIngredientCalories(product, ing.Weight);
        if(calRes.status == Response<double>.responseStatus.success) cal += calRes.data;
        }
        }
        }

        private void OpenRecipeView(Recipe recipe, double totalCal, double totalProt, double totalFat, double totalCarb)
        {
        _currentViewingRecipe = recipe;

        LB_RecipeName.Text = recipe.Name ?? "Без назви";
        LB_Description.Text = recipe.Description ?? "Опис відсутній.";
        LB_FullPreparationTime.Text = "Повний час: " + (recipe.FullPreparationTime.TotalMinutes > 0 ? recipe.FullPreparationTime.TotalMinutes + " хв." : "Не вказано");
        LB_ActivePreparationTime.Text = "Активний час: " + (recipe.ActivePreparationTime.TotalMinutes > 0 ? recipe.ActivePreparationTime.TotalMinutes + " хв." : "Не вказано");
        LB_Servings.Text = "Порцій: " + recipe.Servings;
        label_CreationDate.Text = "Створено: " + recipe.CreationDate.ToString("dd.MM.yyyy HH:mm");
        label_LastEdited.Text = "Змінено: " + recipe.LastEdited.ToString("dd.MM.yyyy HH:mm");
        Label_Link.Text = !string.IsNullOrEmpty(recipe.SourceLink) ? recipe.SourceLink : "Не вказано";

        Response<List<Category>> catRes = DataService.Instance.GetCategories(DataService.DataScope.Local);
        if(catRes.status == Response<List<Category>>.responseStatus.success && catRes.data != null && recipe.ReferencedCategories != null)
        {
        var recipeCategories = catRes.data.Where(c => recipe.ReferencedCategories.Contains(c.ID)).Select(c => c.Name).ToList();
        Label_Categories.Text = "Категорії: " + (recipeCategories.Count > 0 ? string.Join(", ", recipeCategories) : "Не вказано");
        }
        else
        {
        Label_Categories.Text = "Категорії: Не вказано";
        }

        if(!string.IsNullOrEmpty(recipe.MainPhotoPath) && File.Exists(recipe.MainPhotoPath))
        {
        try
        {
        byte[] imageBytes = File.ReadAllBytes(recipe.MainPhotoPath);
        using(MemoryStream ms = new MemoryStream(imageBytes))
            MainPicBox.Image = Image.FromStream(ms);
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

        DGV_Ingredients.Rows.Clear();
        int rowHeight = 25;
        int totalGridHeight = DGV_Ingredients.ColumnHeadersHeight;

        if(recipe.Ingredients != null)
        {
        foreach(Ingredient ing in recipe.Ingredients)
        {
        string displayName = !string.IsNullOrEmpty(ing.RawName) ? ing.RawName : "Без назви";
        double iCal = 0, iProt = 0, iFat = 0, iCarb = 0;

        if(ing.ReferencedProduct != Guid.Empty)
        {
        Product? prod = _allProducts.FirstOrDefault(p => p.ID == ing.ReferencedProduct);
        if(prod != null)
        {
        double factor = ing.Weight / 100.0;
        iProt = prod.Proteins * factor; iFat = prod.Fats * factor; iCarb = prod.Carbohydrates * factor;
        var calRes = CaloriesCalculator.calculateIngredientCalories(prod, ing.Weight);
        if(calRes.status == Response<double>.responseStatus.success) iCal = calRes.data;
        }
        }

        DGV_Ingredients.Rows.Add(displayName, ing.Weight, iCal.ToString("F1"), iProt.ToString("F1"), iFat.ToString("F1"), iCarb.ToString("F1"));
        totalGridHeight += rowHeight;
        }
        }

        DGV_Ingredients.Height = totalGridHeight;
        IngredientsPanel.Height = totalGridHeight + CaloriesValPanel.Height + 20;

        FLP_RecipeSteps.Controls.Clear();
        if(recipe.PreparationSteps != null)
        {
        int stepNum = 1;
        foreach(Step step in recipe.PreparationSteps)
        {
        StepViewControl sv = new StepViewControl(stepNum, step.Description, step.PhotoPath);
        FLP_RecipeSteps.Controls.Add(sv);
        stepNum++;
        }
        }

        if(recipe.AuthorID == AppSession.Instance.CurrentUser?.ID)
        {
        Label_Author.Text = $"Автор: {AppSession.Instance.CurrentUser.Name} (Ви)";
        }
        else
        {
        var authorRes = DataService.Instance.GetAuthorById(recipe.AuthorID);
        Label_Author.Text = "Автор: " + ((authorRes.status == Response<UserProfile>.responseStatus.success && authorRes.data != null)
                            ? authorRes.data.Name
                            : "Невідомий автор");
        }

        if(recipe.InspiredBy != Guid.Empty)
        {
        var originalRecipeRes = DataService.Instance.GetRecipe(recipe.InspiredBy, DataService.DataScope.Global);

        if(originalRecipeRes.status == Response<Recipe>.responseStatus.success && originalRecipeRes.data != null)
        {
        Recipe originalRecipe = originalRecipeRes.data;

        var origAuthorRes = DataService.Instance.GetAuthorById(originalRecipe.AuthorID);
        string origAuthorName = (origAuthorRes.status == Response<UserProfile>.responseStatus.success && origAuthorRes.data != null)
                                ? origAuthorRes.data.Name
                                : "Невідомий автор";

        Label_InspiredBy.Text = $"За мотивами рецепта: {origAuthorName} – {originalRecipe.Name}";
        Label_InspiredBy.Visible = true;
        }
        else
        {
        Label_InspiredBy.Text = "";
        Label_InspiredBy.Visible = false;
        }
        }
        else
        {
        Label_InspiredBy.Text = "";
        Label_InspiredBy.Visible = false;
        }

        ShowRecipePanel.Visible = true;
        if(ShowRecipePanel.isClosed) ShowRecipePanel.updatePanelSizeOnClick();
        ShowRecipePanel.BringToFront();
        }

        private void BTN_CloseRecipe_Click(object sender, EventArgs e)
        {
        ShowRecipePanel.Visible = false;
        }

        #endregion

        #region Parsers (Macros, Time, Dates)

        private ValueRange<int> ParseNutrientsComboBoxRange(string comboBoxText)
        {
        ValueRange<int> range = new ValueRange<int>();
        if(string.IsNullOrEmpty(comboBoxText) || comboBoxText.Contains("Не вибрано"))
        { range.Min = 0; range.Max = int.MaxValue; return range; }

        string clean = comboBoxText.Replace("ккал", "").Replace("г.", "").Replace("г", "").Trim();
        if(clean.Contains("-"))
        {
        string[] parts = clean.Split('-');
        int.TryParse(parts[0].Trim(), out int minVal);
        int.TryParse(parts[1].Trim(), out int maxVal);
        range.Min = minVal; range.Max = maxVal;
        }
        else if(clean.Contains("+"))
        {
        int.TryParse(clean.Replace("+", "").Trim(), out int minVal);
        range.Min = minVal; range.Max = int.MaxValue;
        }
        return range;
        }

        private ValueRange<TimeSpan> GatherCookingTimeRange(string text)
        {
        ValueRange<TimeSpan> range = new ValueRange<TimeSpan>();
        if(string.IsNullOrEmpty(text) || text.Contains("Не вибрано"))
        { range.Min = TimeSpan.Zero; range.Max = TimeSpan.MaxValue; return range; }

        if(text.Contains("до 15 хв.")) { range.Min = TimeSpan.Zero; range.Max = TimeSpan.FromMinutes(15); }
        else if(text.Contains("15 - 30")) { range.Min = TimeSpan.FromMinutes(15); range.Max = TimeSpan.FromMinutes(30); }
        else if(text.Contains("30 - 60 хв.")) { range.Min = TimeSpan.FromMinutes(30); range.Max = TimeSpan.FromMinutes(60); }
        else if(text.Contains("1 - 1.5")) { range.Min = TimeSpan.FromHours(1); range.Max = TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(30)); }
        else if(text.Contains("1.5 - 2")) { range.Min = TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(30)); range.Max = TimeSpan.FromHours(2); }
        else if(text.Contains("2 - 3")) { range.Min = TimeSpan.FromHours(2); range.Max = TimeSpan.FromHours(3); }
        else if(text.Contains("3 - 5")) { range.Min = TimeSpan.FromHours(3); range.Max = TimeSpan.FromHours(5); }
        else if(text.Contains("5 - 7")) { range.Min = TimeSpan.FromHours(5); range.Max = TimeSpan.FromHours(7); }
        else if(text.Contains("8 - 10")) { range.Min = TimeSpan.FromHours(8); range.Max = TimeSpan.FromHours(10); }
        else if(text.Contains("10+")) { range.Min = TimeSpan.FromHours(10); range.Max = TimeSpan.MaxValue; }
        return range;
        }

        private ValueRange<DateTime> ParseDateComboBoxRange(string text)
        {
        ValueRange<DateTime> range = new ValueRange<DateTime>();
        DateTime now = DateTime.Now;
        if(string.IsNullOrEmpty(text) || text.Contains("Не вибрано"))
        { range.Min = DateTime.MinValue; range.Max = DateTime.MaxValue; return range; }

        if(text.Contains("Сьогодні")) { range.Min = now.Date; range.Max = now; }
        else if(text.Contains("Учора")) { range.Min = now.Date.AddDays(-1); range.Max = now.Date.AddTicks(-1); }
        else if(text.Contains("Цього тижня")) { range.Min = now.AddDays(-7); range.Max = now; }
        else if(text.Contains("Цього місяця")) { range.Min = now.AddMonths(-1); range.Max = now; }
        else if(text.Contains("Більше місяця тому")) { range.Min = DateTime.MinValue; range.Max = now.AddMonths(-1); }
        else if(text.Contains("Більше 3 місяців тому")) { range.Min = DateTime.MinValue; range.Max = now.AddMonths(-3); }
        else if(text.Contains("Більше 6 місяців тому")) { range.Min = DateTime.MinValue; range.Max = now.AddMonths(-6); }
        else if(text.Contains("Більше року тому")) { range.Min = DateTime.MinValue; range.Max = now.AddYears(-1); }
        else if(text.Contains("Більше 2 років тому")) { range.Min = DateTime.MinValue; range.Max = now.AddYears(-2); }
        return range;
        }

        #endregion

        #region Resizable Panels Management

        private void GlobalControl_Resize(object sender, EventArgs e) { controlResizing?.Invoke(this, EventArgs.Empty); }

        private void BTN_OpenFilter_R_Click(object sender, EventArgs e)
        {
        RP_FilterBorder_R.updatePanelSizeOnClick();
        BTN_OpenFilter_R.Image = RP_FilterBorder_R.isClosed ? Properties.Resources.OpenFilter_Icon_Down : Properties.Resources.OpenFilter_Icon_Up;
        }

        private void BTN_OpenFilter_K_Click(object sender, EventArgs e)
        {
        RP_FilterBorder_K.updatePanelSizeOnClick();
        BTN_OpenFilter_K.Image = RP_FilterBorder_K.isClosed ? Properties.Resources.OpenFilter_Icon_Down : Properties.Resources.OpenFilter_Icon_Up;
        }

        private void BTN_OpenCategories_Click(object sender, EventArgs e)
        {
        CheckedCB_FoodCategoryPanel.updatePanelSizeOnClick();
        BTN_OpenCategories.Image = CheckedCB_FoodCategoryPanel.isClosed ? Properties.Resources.OpenFilter_Icon_Down : Properties.Resources.OpenFilter_Icon_Up;
        }

        #endregion

        #region Download & Import Logic

        private void BTN_DownloadRecipe_Click(object sender, EventArgs e)
        {
        if(_currentViewingRecipe == null) return;

        Response<List<Recipe>> localRecipesRes = DataService.Instance.GetRecipes(DataService.DataScope.Local);
        List<Recipe> localRecipes = localRecipesRes.data ?? new List<Recipe>();

        if(localRecipes.Any(r => r.InspiredBy == _currentViewingRecipe.ID))
        {
        MessageBox.Show("Цей рецепт уже завантажено до вашої локальної бази!",
            "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
        }

        Response<Recipe> importRes = DataService.Instance.ImportGlobalRecipe(_currentViewingRecipe.ID);

        if(importRes.status != Response<Recipe>.responseStatus.error && importRes.data != null)
        {
        ShowRecipePanel.Visible = false;
        LoadGlobalDataCaches();

        if(importRes.status == Response<Recipe>.responseStatus.warning)
        {
        MessageBox.Show(importRes.message, "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        else
        {
        MessageBox.Show(importRes.message, "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        }
        else
        {
        MessageBox.Show("Помилка імпорту рецепта: " + importRes.message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        }

        private void BTN_DownloadCookBook_Click(object sender, EventArgs e)
        {
        if(_currentCookBook == null) return;

        DialogResult confirm = MessageBox.Show($"Завантажити кулінарну книгу '{_currentCookBook.Name}' та всі її рецепти?",
            "Підтвердження імпорту", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if(confirm != DialogResult.Yes) return;

        Response<List<Recipe>> localRecipesRes = DataService.Instance.GetRecipes(DataService.DataScope.Local);
        List<Recipe> localRecipes = localRecipesRes.data ?? new List<Recipe>();

        List<Guid> recipesToImportClean = new List<Guid>();
        List<Guid> existingLocalRecipeIdsToLink = new List<Guid>();

        if(_currentCookBook.ReferencedRecipes != null)
        {
        foreach(Guid globalRecipeId in _currentCookBook.ReferencedRecipes)
        {
        Recipe? existingLocalCopy = localRecipes.FirstOrDefault(r => r.InspiredBy == globalRecipeId);

        if(existingLocalCopy != null)
        {
        DialogResult duplicateResult = MessageBox.Show(
            $"Рецепт '{existingLocalCopy.Name}' уже завантажений у вашу локальну базу.\n\n" +
            $"Натисніть 'Yes', щоб прив'язати вже існуючий lokalний рецепт до цієї книги.\n" +
            $"Натисніть 'No', щоб проігнорувати і створити ще одну незалежну копію.",
            "Виявлено дублікат рецепта",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if(duplicateResult == DialogResult.Yes)
        {
        existingLocalRecipeIdsToLink.Add(existingLocalCopy.ID);
        continue;
        }
        }

        recipesToImportClean.Add(globalRecipeId);
        }
        }

        List<Guid> originalGlobalRefs = _currentCookBook.ReferencedRecipes != null ?
            new List<Guid>(_currentCookBook.ReferencedRecipes) : new List<Guid>();

        _currentCookBook.ReferencedRecipes = recipesToImportClean;

        Response<CookBook> bookImportRes = DataService.Instance.ImportGlobalCookBook(_currentCookBook.ID);

        _currentCookBook.ReferencedRecipes = originalGlobalRefs;

        if(bookImportRes.status != Response<CookBook>.responseStatus.error && bookImportRes.data != null)
        {
        CookBook importedLocalBook = bookImportRes.data;

        if(existingLocalRecipeIdsToLink.Count > 0)
        {
        foreach(Guid existingId in existingLocalRecipeIdsToLink)
        {
        if(!importedLocalBook.ReferencedRecipes.Contains(existingId))
        {
        importedLocalBook.ReferencedRecipes.Add(existingId);
        }
        }

        UpdateCookBook_dto updateDto = new UpdateCookBook_dto
        {
            ID = importedLocalBook.ID,
            Name = importedLocalBook.Name,
            Description = importedLocalBook.Description,
            IsPublic = importedLocalBook.IsPublic,
            ReferencedRecipes = importedLocalBook.ReferencedRecipes
        };

        var updateRes = DataService.Instance.UpdateCookBook(updateDto);
        if(updateRes.status == Response<CookBook>.responseStatus.error)
        {
        MessageBox.Show("Збірку імпортовано, але виникла помилка прив'язки локальних дублікатів: " + updateRes.message,
                        "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        }

        if(bookImportRes.status == Response<CookBook>.responseStatus.warning)
        {
        MessageBox.Show(bookImportRes.message, "Імпортовано частково", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        else
        {
        MessageBox.Show("Кулінарну книгу успішно імпортовано без дублювання наявних рецептів!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        LoadGlobalDataCaches();
        ExecuteBooksSearch();
        }
        else
        {
        MessageBox.Show("Помилка імпорту збірки: " + bookImportRes.message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        }

        #endregion

        private void label_LastEdited_Click(object sender, EventArgs e)
        {

        }
    }
}