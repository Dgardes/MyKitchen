using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MyKitchen_Core.Entities;
using MyKitchen_Core.Services;
using MyKitchen_Core.Tools;
using MyKitchen_Core.DTO.CreateAndUpdateDTOs;
using MyKitchen_Core.DTO.SearchDTOs;
using MyKitchen_UI.Interfaces;
using MyKitchen_Core.Interfaces;

namespace MyKitchen_UI
{
    public partial class CookBooksControl :UserControl, IResizableContainer, IRefreshableTab
    {
        public event EventHandler? controlResizing;
        public event EventHandler? controlLoaded;
        public Size containerSize { get { return this.Size; } }

        private bool _isInitialized = false;
        private bool _isModified = false;
        private bool _isLoadingData = false;

        private CookBook? _currentCookBook = null;
        private List<Recipe> _localRecipesCache = new List<Recipe>();

        public CookBooksControl()
        {
        InitializeComponent();
        this.Load += CookBooksControl_Load;
        this.Resize += CookBooksControl_Resize;
        }

        #region Initialization

        public void OnTabActivated()
        {
        if(_isLoadingData) return;
        LoadLocalRecipesCache();
        ExecuteSearch();
        }

        private void CookBooksControl_Load(object sender, EventArgs e)
        {
        controlLoaded?.Invoke(this, EventArgs.Empty);

        RP_FilterBorder.updatePanelSize(true);

        BTN_SaveCookBook.Visible = false;
        BTN_DeleteCookBook.Visible = false;
        BTN_DeleteRecipeFromCB.Visible = false;
        BTN_ShowRecipe.Visible = false;

        RegisterFilters();
        RegisterEditEvents();
        LoadLocalRecipesCache();

        _isInitialized = true;
        ExecuteSearch();
        }

        private void LoadLocalRecipesCache()
        {
        // Використовуємо стабільний SearchRecipes із заповненим DTO, щоб уникнути внутрішніх валідацій репозиторію
        SearchRecipeCriteria_dto stableCriteria = new SearchRecipeCriteria_dto
        {
            ReferencedCategories = new List<Guid>()
        };

        Response<List<Recipe>> res = DataService.Instance.SearchRecipes(stableCriteria, DataService.DataScope.Local);
        if(res.status != Response<List<Recipe>>.responseStatus.error && res.data != null)
        {
        _localRecipesCache = res.data;
        }
        }

        private void CookBooksControl_Resize(object sender, EventArgs e)
        {
        controlResizing?.Invoke(this, EventArgs.Empty);
        }

        private void BTN_OpenFilter_Click(object sender, EventArgs e)
        {
        RP_FilterBorder.updatePanelSizeOnClick();
        BTN_OpenFilter.Image = RP_FilterBorder.isClosed ? Properties.Resources.OpenFilter_Icon_Down : Properties.Resources.OpenFilter_Icon_Up;
        }

        #endregion

        #region Search Logic

        private void RegisterFilters()
        {
        txtbox_CookBookName.TextChanged += OnFilterChanged;
        txtbox_FindByRecipeName.TextChanged += OnFilterChanged;
        txtbox_Author.TextChanged += OnFilterChanged;

        combox_CreationDate.SelectedIndexChanged += OnFilterChanged;
        combox_LastEdited.SelectedIndexChanged += OnFilterChanged;
        }

        private void OnFilterChanged(object? sender, EventArgs e)
        {
        if(!_isInitialized) return;
        ExecuteSearch();
        }

        private void ExecuteSearch()
        {
        LoadLocalRecipesCache();

        SearchCookBookCriteria_dto criteria = new SearchCookBookCriteria_dto();

        if(!string.IsNullOrEmpty(txtbox_CookBookName.Text))
            criteria.Name = txtbox_CookBookName.Text.Trim();

        if(!string.IsNullOrEmpty(txtbox_Author.Text))
            criteria.AuthorName = txtbox_Author.Text.Trim();

        if(combox_CreationDate.SelectedIndex > 0 && !combox_CreationDate.Text.Contains("Не вибрано"))
            criteria.CreationDate = ParseDateComboBoxRange(combox_CreationDate.Text);

        if(combox_LastEdited.SelectedIndex > 0 && !combox_LastEdited.Text.Contains("Не вибрано"))
            criteria.LastEdited = ParseDateComboBoxRange(combox_LastEdited.Text);

        Response<List<CookBook>> response = DataService.Instance.SearchCookBooks(criteria, DataService.DataScope.Local);

        if(response.status != Response<List<CookBook>>.responseStatus.error && response.data != null)
        {
        ListBox_CookBooks.Items.Clear();
        ListBox_CookBooks.DisplayMember = "Name";

        string recipeFilter = txtbox_FindByRecipeName.Text.Trim().ToLower();
        var filteredBooks = response.data;

        if(!string.IsNullOrEmpty(recipeFilter))
        {
        var matchingRecipeIds = _localRecipesCache
            .Where(r => r.Name != null && r.Name.ToLower().Contains(recipeFilter))
            .Select(r => r.ID).ToList();

        filteredBooks = filteredBooks
            .Where(cb => cb.ReferencedRecipes != null && cb.ReferencedRecipes.Any(id => matchingRecipeIds.Contains(id)))
            .ToList();
        }

        foreach(CookBook book in filteredBooks)
        {
        ListBox_CookBooks.Items.Add(book);
        }
        }
        }

        private ValueRange<DateTime> ParseDateComboBoxRange(string text)
        {
        ValueRange<DateTime> range = new ValueRange<DateTime>();
        DateTime now = DateTime.Now;

        if(string.IsNullOrEmpty(text) || text.Contains("Не вибрано"))
        {
        range.Min = DateTime.MinValue;
        range.Max = DateTime.MaxValue;
        return range;
        }

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

        #region Master-Detail logic

        private void ListBox_CookBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
        if(ListBox_CookBooks.SelectedItem is CookBook selectedBook)
        {
        if(_isModified && CheckForUnsavedChanges() == false)
        {
        ListBox_CookBooks.SelectedItem = _currentCookBook;
        return;
        }

        LoadCookBookData(selectedBook);
        }
        }

        private void LoadCookBookData(CookBook book)
        {
        _isLoadingData = true;
        _currentCookBook = book;

        txtbox_CookBName.Text = book.Name;
        txtbox_Description.Text = book.Description;
        CB_IsPublic.Checked = book.IsPublic;

        label_CreationDate.Text = "Створено: " + book.CreationDate.ToString("g");
        label_LastEdited.Text = "Змінено: " + book.LastEdited.ToString("g");

        ListBox_RecipesRefs.Items.Clear();
        ListBox_RecipesRefs.DisplayMember = "Name";

        LoadLocalRecipesCache();

        if(book.ReferencedRecipes != null)
        {
        foreach(Guid recipeId in book.ReferencedRecipes)
        {
        Recipe? recipe = _localRecipesCache.FirstOrDefault(r => r.ID == recipeId);

        if(recipe != null)
        {
        if(!ListBox_RecipesRefs.Items.Contains(recipe))
        {
        ListBox_RecipesRefs.Items.Add(recipe);
        }
        }
        }
        }

        _isLoadingData = false;
        _isModified = false;

        BTN_DeleteCookBook.Visible = true;
        BTN_SaveCookBook.Visible = false;

        BTN_DeleteRecipeFromCB.Visible = false;
        BTN_ShowRecipe.Visible = false;
        }

        private void ListBox_RecipesRefs_SelectedIndexChanged(object sender, EventArgs e)
        {
        if(ListBox_RecipesRefs.SelectedItem is Recipe selectedRecipe)
        {
        BTN_DeleteRecipeFromCB.Visible = true;
        BTN_ShowRecipe.Visible = true;
        }
        }
        #endregion

        #region Modification Tracking

        private void RegisterEditEvents()
        {
        txtbox_CookBName.TextChanged += OnCookBookModified;
        txtbox_Description.TextChanged += OnCookBookModified;
        CB_IsPublic.CheckedChanged += OnCookBookModified;
        }

        private void OnCookBookModified(object? sender, EventArgs e)
        {
        if(_isLoadingData) return;

        _isModified = true;
        BTN_SaveCookBook.Visible = true;
        }

        public bool CheckForUnsavedChanges()
        {
        if(!_isModified) return true;

        DialogResult result = MessageBox.Show("У вас є незбережені зміни в книзі рецептів. Зберегти їх?",
            "Увага", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

        if(result == DialogResult.Yes)
        {
        BTN_SaveCookBook_Click(null, null);
        return true;
        }
        else if(result == DialogResult.No)
        {
        return true;
        }
        return false;
        }

        #endregion

        #region Button Handlers (Create, Update, Delete)

        private void BTN_CreateCookBook_Click(object sender, EventArgs e)
        {
        if(AppSession.Instance.CurrentUser == null)
        {
        MessageBox.Show("Створювати книги можуть лише авторизовані користувачі.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
        }

        if(_isModified && CheckForUnsavedChanges() == false) return;

        _isLoadingData = true;
        _currentCookBook = null;

        ListBox_CookBooks.ClearSelected();
        ListBox_RecipesRefs.Items.Clear();
        txtbox_CookBName.Clear();
        txtbox_Description.Clear();
        CB_IsPublic.Checked = false;

        label_CreationDate.Text = "Створено: -";
        label_LastEdited.Text = "Змінено: -";

        _isLoadingData = false;
        _isModified = false;

        BTN_DeleteCookBook.Visible = false;
        BTN_SaveCookBook.Visible = true;

        txtbox_CookBName.Focus();
        }

        private void BTN_SaveCookBook_Click(object? sender, EventArgs e)
        {
        string name = txtbox_CookBName.Text.Trim();
        if(string.IsNullOrEmpty(name))
        {
        MessageBox.Show("Назва книги не може бути порожньою!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
        }

        List<Guid> currentRecipes = new List<Guid>();
        foreach(Recipe r in ListBox_RecipesRefs.Items)
        {
        currentRecipes.Add(r.ID);
        }

        if(_currentCookBook == null)
        {
        CreateCookBook_dto createDto = new CreateCookBook_dto
        {
            Name = name,
            Description = txtbox_Description.Text.Trim(),
            IsPublic = CB_IsPublic.Checked,
            ReferencedRecipes = currentRecipes
        };

        Response<CookBook> res = DataService.Instance.AddCookBook(createDto);
        if(res.status == Response<CookBook>.responseStatus.success && res.data != null)
        {
        MessageBox.Show("Книгу успішно створено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        _isModified = false;
        ExecuteSearch();
        LoadCookBookData(res.data);
        }
        else
        {
        MessageBox.Show("Помилка створення: " + res.message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        }
        else
        {
        UpdateCookBook_dto updateDto = new UpdateCookBook_dto
        {
            ID = _currentCookBook.ID,
            Name = name,
            Description = txtbox_Description.Text.Trim(),
            IsPublic = CB_IsPublic.Checked,
            ReferencedRecipes = currentRecipes
        };

        Response<CookBook> res = DataService.Instance.UpdateCookBook(updateDto);
        if(res.status == Response<CookBook>.responseStatus.success && res.data != null)
        {
        MessageBox.Show("Книгу успішно оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        _isModified = false;
        ExecuteSearch();
        LoadCookBookData(res.data);
        }
        else
        {
        MessageBox.Show("Помилка оновлення: " + res.message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        }
        }

        private void BTN_DeleteCookBook_Click(object sender, EventArgs e)
        {
        if(_currentCookBook == null) return;

        DialogResult result = MessageBox.Show($"Ви впевнені, що хочете видалити книгу '{_currentCookBook.Name}'?", "Видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if(result == DialogResult.Yes)
        {
        Response<bool> delRes = DataService.Instance.RemoveObject(ILocalRepository.dataType.cookBook, _currentCookBook.ID, DataService.DataScope.Local);
        if(delRes.status == Response<bool>.responseStatus.success)
        {
        _currentCookBook = null;
        _isModified = false;
        ExecuteSearch();
        ListBox_RecipesRefs.Items.Clear();
        txtbox_CookBName.Clear();
        txtbox_Description.Clear();
        BTN_DeleteCookBook.Visible = false;
        BTN_SaveCookBook.Visible = false;
        }
        else
        {
        MessageBox.Show("Помилка видалення: " + delRes.message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        }
        }

        private void BTN_DeleteRecipeFromCB_Click(object sender, EventArgs e)
        {
        if(ListBox_RecipesRefs.SelectedItem is Recipe selectedRecipe)
        {
        ListBox_RecipesRefs.Items.Remove(selectedRecipe);
        OnCookBookModified(null, EventArgs.Empty);
        BTN_DeleteRecipeFromCB.Visible = false;
        BTN_ShowRecipe.Visible = false;
        }
        }

        private void BTN_ShowRecipe_Click(object sender, EventArgs e)
        {
        if(ListBox_RecipesRefs.SelectedItem is Recipe selectedRecipe && MainForm.mainForm != null)
        {
        MainForm.mainForm.OpenRecipeInViewMode(selectedRecipe);
        }
        }

        #endregion
    }
}