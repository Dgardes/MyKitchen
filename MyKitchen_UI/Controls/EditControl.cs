using MyKitchen_Core.Entities;
using MyKitchen_Core.Services;
using MyKitchen_Core.Tools;
using MyKitchen_Core.DTO.CreateAndUpdateDTOs;
using MyKitchen_Core.Interfaces;
using MyKitchen_UI.Properties;
using MyKitchen_UI.Interfaces;
using Guna.UI2.WinForms;


namespace MyKitchen_UI
{
    public partial class EditControl : UserControl, IResizableContainer, IRefreshableTab
    {
        private AutoCompleteStringCollection autoComplete = new AutoCompleteStringCollection();
        private List<ProductAlias> productAliases = new List<ProductAlias>();
        private Dictionary<string, Product> aliasToProductMap = new Dictionary<string, Product>(StringComparer.OrdinalIgnoreCase);
        
        private Recipe _currentRecipe = null;
        private string _currentPhotoPath = string.Empty;

        private bool _isModified = false;
        private bool _isLoadingData = false;

        public event EventHandler controlResizing;
        public event EventHandler controlLoaded;

        public Guna2Panel P_ContentPanel = null;

        public Size containerSize
        {
            get { return this.Size; }
        }

        public EditControl()
        {
            InitializeComponent();
            LoadDataForAutocomplete();
            P_ContentPanel = ContentPanel;
            ContentPanel.Visible = false;
        }

        #region DataGridView Event Handlers

        private void btn_AddRow_Click(object sender, EventArgs e)
        {
            DGV_Ingredients.Rows.Add();
            int rowheight = 29;
            DGV_Ingredients.Height += rowheight;
            IngredientsPanel.Height += rowheight;
        }

        private void DGV_Ingredients_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = e.Control as TextBox;
            if(tb == null) return;
            tb.KeyPress -= Numeric_KeyPress;

            string columnName = DGV_Ingredients.CurrentCell.OwningColumn.Name;

            if(columnName == "IngredientColumn")
            {
                tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                tb.AutoCompleteCustomSource = autoComplete;
            }
            else if(columnName == "Weight")
            {
                tb.AutoCompleteMode = AutoCompleteMode.None;
                tb.KeyPress += Numeric_KeyPress;
            }
        }

        private void Numeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void DGV_Ingredients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && DGV_Ingredients.Columns[e.ColumnIndex].Name == "DeleteButton")
            {
                DGV_Ingredients.Rows.RemoveAt(e.RowIndex);
                int rowheight = 29;
                DGV_Ingredients.Height -= rowheight;
                IngredientsPanel.Height -= rowheight;
            }
        }

        private void DGV_Ingredients_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string columnName = DGV_Ingredients.Columns[e.ColumnIndex].Name;
            if (columnName == "IngredientColumn" || columnName == "Weight")
            {
                UpdateRowCalculations(e.RowIndex);
                AnyControl_ValueChanged(sender, e); 
            }
        }

        #endregion

        #region Calculations
        private void UpdateRowCalculations(int rowIndex)
        {
            DataGridViewRow row = DGV_Ingredients.Rows[rowIndex];
            string aliasName = row.Cells["IngredientColumn"].Value?.ToString();
            string weightStr = row.Cells["Weight"].Value?.ToString();

            if (!string.IsNullOrEmpty(aliasName) && aliasToProductMap.TryGetValue(aliasName, out Product product))
            {
                if (double.TryParse(weightStr, out double weight))
                {
                    double proteins = (weight / 100) * product.Proteins;
                    double fats = (weight / 100) * product.Fats;
                    double carbo = (weight / 100) * product.Carbohydrates;

                    Response<double> calResponse = CaloriesCalculator.calculateCalories(proteins, carbo, fats);
            
                    if (calResponse.status == Response<double>.responseStatus.success)
                    {
                        row.Cells["Calories"].Value = calResponse.data.ToString("F1");
                        row.Cells["Proteins"].Value = proteins.ToString("F1");
                        row.Cells["Fats"].Value = fats.ToString("F1");
                        row.Cells["Carbohydrates"].Value = carbo.ToString("F1");
                        row.Cells["Warning"].Value = "";

                        CalculateTotalNutrients();
                    }
                }
                else
                {
                    row.Cells["Calories"].Value = "0.0";
                    row.Cells["Proteins"].Value = "0.0";
                    row.Cells["Fats"].Value = "0.0";
                    row.Cells["Carbohydrates"].Value = "0.0";
                    row.Cells["Warning"].Value = "⚠️";
                    CalculateTotalNutrients(); 
                }
            }
        }

        private void CalculateTotalNutrients()
        {
            double totalCalories = 0;
            double totalProteins = 0;
            double totalFats = 0;
            double totalCarbs = 0;

            foreach (DataGridViewRow row in DGV_Ingredients.Rows)
            {
                if (row.IsNewRow) continue;

                double.TryParse(row.Cells["Calories"].Value?.ToString(), out double cal);
                double.TryParse(row.Cells["Proteins"].Value?.ToString(), out double prot);
                double.TryParse(row.Cells["Fats"].Value?.ToString(), out double fat);
                double.TryParse(row.Cells["Carbohydrates"].Value?.ToString(), out double carb);

                totalCalories += cal;
                totalProteins += prot;
                totalFats += fat;
                totalCarbs += carb;
            }

            int.TryParse(combox_ServingsCount.Text, out int servings);
            if (servings <= 0) servings = 1;

            label_CaloriesPerDish.Text = "Калорії на страву: " + totalCalories.ToString("F1");
            label_DishProteins.Text = "Білки: " + totalProteins.ToString("F1") + " г";
            label_DishFats.Text = "Жири: " + totalFats.ToString("F1") + " г";
            label_DishCarbohydrates.Text = "Вуглеводи: " + totalCarbs.ToString("F1") + " г";

            label_CaloriesPerServing.Text = "Калорії на порцію: " + (totalCalories / servings).ToString("F1");
            label_ServingProteins.Text = "Білки: " + (totalProteins / servings).ToString("F1") + " г";
            label_ServingFats.Text = "Жири: " + (totalFats / servings).ToString("F1") + " г";
            label_ServingCarbohydrates.Text = "Вуглеводи: " + (totalCarbs / servings).ToString("F1") + " г";
        }
        #endregion

        #region BTN handlers
        private void BTN_AddStep_Click(object sender, EventArgs e)
        {
            StepControl step = new StepControl();
            step.DeleteClicked += Step_DeleteClicked;
            FLP_RecipeSteps.Controls.Add(step);
            updateStepNumbers();
        }

        private void Step_DeleteClicked(object sender, EventArgs e)
        {
            StepControl del = sender as StepControl;
            if(del != null)
            {
                FLP_RecipeSteps.Controls.Remove(del);
                updateStepNumbers();
            }
        }

        private void updateStepNumbers()
        {
            int currentStepNumber = 1;
            foreach(StepControl stepControl in FLP_RecipeSteps.Controls.OfType<StepControl>())
            {
                stepControl.StepNumberText = "Крок №" + currentStepNumber;
                currentStepNumber++;
            }
        }

        private void BTN_SaveRecipe_Click(object sender, EventArgs e)
        {
            string name = txtbox_RecipeName.Text.Trim();
            string description = txtbox_Description.Text.Trim();
            string link = txtbox_Link.Text.Trim();
            bool isPublic = CB_IsPublic.Checked;

            int servings;
            int.TryParse(combox_ServingsCount.Text, out servings);
            if (servings <= 0) servings = 1;

            List<Ingredient> ingredients = GatherIngredientsFromGrid();
            List<Step> steps = GatherStepsFromControls();
            List<Guid> selectedCategoryIds = GatherSelectedCategoryIds();
            List<Guid> selectedCookBookIds = GatherSelectedCookBookIds();
            TimeSpan activeTime = ParseTimeFromComboBox(combox_ActivePreparationTime.Text);
            TimeSpan fullTime = ParseTimeFromComboBox(combox_FullPreparationTime.Text);
            
            if (_currentRecipe == null)
            {

                CreateRecipe_dto createDto = new CreateRecipe_dto
                {
                    Name = name,
                    Description = description,
                    SourceLink = link,
                    Servings = servings,
                    IsPublic = isPublic,
                    Ingredients = ingredients,
                    PreparationSteps = steps,
                    ReferencedCategories = selectedCategoryIds,
                    cookBooksReferencedIn = selectedCookBookIds,

                    ActivePreparationTime = activeTime,
                    FullPreparationTime = fullTime,

                    MainPhotoPath = _currentPhotoPath
                };

                Response<Recipe> saveRes = DataService.Instance.AddRecipe(createDto);
                if (saveRes.status != Response<Recipe>.responseStatus.error && saveRes.data != null)
                {
                    if (saveRes.status == Response<Recipe>.responseStatus.warning)
                    {
                        MessageBox.Show(saveRes.message, "Збережено з обмеженнями", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Новий рецепт успішно створено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    _currentRecipe = saveRes.data; 
                    LoadRecipeData(_currentRecipe); 
                    _isModified = false; 
                    ContentPanel.Visible = false;
                    BTN_DeleteRecipe.Visible = true;
                }
                else
                {
                    MessageBox.Show("Помилка збереження: " + saveRes.message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                UpdateRecipe_dto updateDto = new UpdateRecipe_dto
                {
                    ID = _currentRecipe.ID,
                    Name = name,
                    Description = description,
                    SourceLink = link,
                    Servings = servings,
                    IsPublic = isPublic,
                    Ingredients = ingredients,
                    PreparationSteps = steps,
                    ReferencedCategories = selectedCategoryIds,
                    cookBooksReferencedIn = selectedCookBookIds,
                    ActivePreparationTime = activeTime,
                    FullPreparationTime = fullTime,

                    MainPhotoPath = _currentPhotoPath
                };

                Response<Recipe> updateRes = DataService.Instance.UpdateRecipe(updateDto);
                if (updateRes.status != Response<Recipe>.responseStatus.error && updateRes.data != null)
                {
                    if (updateRes.status == Response<Recipe>.responseStatus.warning)
                    {
                        MessageBox.Show(updateRes.message, "Оновлено з обмеженнями", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Рецепт успішно оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    ContentPanel.Visible = false;
                    _isModified = false; 
                }
                else
                {
                    MessageBox.Show("Помилка оновлення: " + updateRes.message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BTN_CancelSaving_Click(object sender, EventArgs e)
        {
            if (_currentRecipe != null)
            {
                DialogResult result =
                MessageBox.Show("Скасувати всі внесені зміни й повернутися до початкової версії рецепта?", "Скасування",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    LoadRecipeData(_currentRecipe);
                    ContentPanel.Visible = false;
                }
            }
            else
            {
                DialogResult result = 
                MessageBox.Show("Очистити форму створення рецепта?", "Очищення", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    ClearForm();
                    ContentPanel.Visible = false;
                }
            }
        }

        private void BTN_Create_Click(object sender, EventArgs e)
        {
            if(AppSession.Instance.CurrentUser == null)
            {
                MessageBox.Show("Додавати нові рецепти можуть лише авторизовані користувачі. Будь ласка, увійдіть в акаунт.",
                "Авторизація", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetupForCreate();
            ContentPanel.Visible = true;
            Label_InspiredBy.Visible = false;
            Label_Author.Text = $"Автор: {AppSession.Instance.CurrentUser.Name}";
            
        }

        private void BTN_OpenCategories_Click(object sender, EventArgs e)
        {
            CheckedCB_FoodCategoryPanel.updatePanelSizeOnClick();
            bool isClosed = CheckedCB_FoodCategoryPanel.isClosed;

            if(isClosed)
            {
                BTN_OpenCategories.Image = Properties.Resources.OpenFilter_Icon_Down;
            }
            else
            {
                BTN_OpenCategories.Image = Properties.Resources.OpenFilter_Icon_Up;
            }
        }

        private void BTN_OpenCookBooks_Click(object sender, EventArgs e)
        {
            CheckedCB_CookBooksPanel.updatePanelSizeOnClick();
            bool isClosed = CheckedCB_CookBooksPanel.isClosed;

            if(isClosed)
            {
                BTN_OpenCookBooks.Image = Properties.Resources.OpenFilter_Icon_Down;
            }
            else
            {
                BTN_OpenCookBooks.Image = Properties.Resources.OpenFilter_Icon_Up;
            }
        }

        private void BTN_SetMainPic_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Виберіть головне фото рецепта";
                ofd.Filter = "Зображення (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Завантажуємо зображення без блокування файлу (щоб його можна було потім переміщати/видаляти)
                        byte[] imageBytes = File.ReadAllBytes(ofd.FileName);
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            MainPicBox.Image = Image.FromStream(ms);
                        }
                
                        _currentPhotoPath = ofd.FileName; // Зберігаємо новий шлях
                        SetModified(); // Вмикаємо прапорець незбережених змін
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Не вдалося завантажити зображення: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BTN_DeleteMainPic_Click(object sender, EventArgs e)
        {
            // Повертаємо стандартну картинку-заглушку
            MainPicBox.Image = Resources.StockPhoto;
            _currentPhotoPath = string.Empty; // Очищаємо шлях
            SetModified();
        }

        private void BTN_DeleteRecipe_Click(object sender, EventArgs e)
        {
            // Якщо рецепта в базі немає, кнопка взагалі не повинна спрацьовувати (хоча ми її сховали)
            if (_currentRecipe == null) return;

            // Запитуємо підтвердження у користувача
            DialogResult confirm = MessageBox.Show(
                $"Ви впевнені, що хочете назавжди видалити рецепт '{_currentRecipe.Name}'?\n" +
                "Цю дію неможливо скасувати, і він автоматично зникне з усіх кулінарних книг.", 
                "Підтвердження видалення", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            // Викликаємо оновлений каскадний метод видалення
            Response<bool> deleteRes = DataService.Instance.RemoveObject(
                ILocalRepository.dataType.recipe, 
                _currentRecipe.ID, 
                DataService.DataScope.Local);

            if (deleteRes.status != Response<bool>.responseStatus.error)
            {
                if (deleteRes.status == Response<bool>.responseStatus.warning)
                {
                    MessageBox.Show(deleteRes.message, "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Рецепт успішно видалено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Скидаємо стан форми
                ClearForm();
                _currentRecipe = null;
                BTN_DeleteRecipe.Visible = false; // Ховаємо кнопку після успішного видалення
                ContentPanel.Visible = false; // Закриваємо панель редагування
            }
            else
            {
                MessageBox.Show("Помилка при видаленні рецепта: " + deleteRes.message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Load data

        public void OnTabActivated()
        {
            LoadDataForAutocomplete(); // Оновлюємо самі списки (якщо створили нову книгу)
    
            // Якщо зараз відкритий рецепт - повертаємо його галочки на місце
            if (_currentRecipe != null)
            {
                RestoreChecksForCurrentRecipe();
            }
        }

        private void LoadDataForAutocomplete()
        {
            Response<List<Product>> productsRes = DataService.Instance.GetProducts(DataService.DataScope.Local);
            Response<List<ProductAlias>> aliasesRes = DataService.Instance.GetProductAliases(DataService.DataScope.Local);

            Response<List<Category>> categoriesRes = DataService.Instance.GetCategories(DataService.DataScope.Local);
            Response<List<CookBook>> cookBooksRes = DataService.Instance.GetCookBooks(DataService.DataScope.Local);

            if (productsRes.status != Response<List<Product>>.responseStatus.success)
            {
                MessageBox.Show($"Помилка сервісу продуктів: {productsRes.message}", "Помилка API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (aliasesRes.status != Response<List<ProductAlias>>.responseStatus.success)
            {
                MessageBox.Show($"Помилка сервісу псевдонімів: {aliasesRes.message}", "Помилка API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            if (productsRes.status == Response<List<Product>>.responseStatus.success && 
                aliasesRes.status == Response<List<ProductAlias>>.responseStatus.success)
            {
                productAliases = aliasesRes.data;
                aliasToProductMap.Clear(); // Очищаємо перед новим завантаженням
                autoComplete.Clear();
        
                foreach (ProductAlias alias in productAliases)
                {
                    Product product = productsRes.data.FirstOrDefault(p => p.ID == alias.ReferencedProduct);
                    if (product != null)
                    {
                        aliasToProductMap[alias.AliasName] = product;
                        autoComplete.Add(alias.AliasName);
                    }
                }
            }

            if (categoriesRes.status == Response<List<Category>>.responseStatus.success)
            {
                CheckedLB_FoodCategories.Items.Clear();
                CheckedLB_FoodCategories.DisplayMember = "Name"; 
                foreach (Category cat in categoriesRes.data)
                {
                    CheckedLB_FoodCategories.Items.Add(cat);
                }
            }

            if (cookBooksRes.status == Response<List<CookBook>>.responseStatus.success)
            {
                CheckedLB_CookBooks.Items.Clear();
                CheckedLB_CookBooks.DisplayMember = "Name";
                foreach (CookBook book in cookBooksRes.data)
                {
                    CheckedLB_CookBooks.Items.Add(book);
                }
            }
        }

        // Розставляє галочки для книг та категорій
        private void RestoreChecksForCurrentRecipe()
        {
            if (_currentRecipe == null) return;

            // Відновлюємо галочки для категорій
            for (int i = 0; i < CheckedLB_FoodCategories.Items.Count; i++)
            {
                Category currentCat = (Category)CheckedLB_FoodCategories.Items[i];
                bool isChecked = _currentRecipe.ReferencedCategories != null && _currentRecipe.ReferencedCategories.Contains(currentCat.ID);
                CheckedLB_FoodCategories.SetItemChecked(i, isChecked);
            }

            // Відновлюємо галочки для книг
            Response<List<CookBook>> cookBooksRes = DataService.Instance.GetCookBooks(DataService.DataScope.Local);
            if (cookBooksRes.status == Response<List<CookBook>>.responseStatus.success && cookBooksRes.data != null)
            {
                for (int i = 0; i < CheckedLB_CookBooks.Items.Count; i++)
                {
                    CookBook currentBook = (CookBook)CheckedLB_CookBooks.Items[i];
                    CookBook dbBook = cookBooksRes.data.FirstOrDefault(b => b.ID == currentBook.ID);
            
                    bool isChecked = dbBook != null && dbBook.ReferencedRecipes != null && dbBook.ReferencedRecipes.Contains(_currentRecipe.ID);
                    CheckedLB_CookBooks.SetItemChecked(i, isChecked);
                }
            }
        }

        private void LoadRecipeData(Recipe recipe)
        {
            _isLoadingData = true;

            txtbox_RecipeName.Text = recipe.Name;
            txtbox_Description.Text = recipe.Description;
            txtbox_Link.Text = recipe.SourceLink;
            
            combox_ActivePreparationTime.Text = FormatTimeForComboBox(recipe.ActivePreparationTime);
            combox_FullPreparationTime.Text = FormatTimeForComboBox(recipe.FullPreparationTime);    

            combox_ServingsCount.Text = recipe.Servings.ToString();
            CB_IsPublic.Checked = recipe.IsPublic;

            label_CreationDate.Text = "Дата створення: " + recipe.CreationDate.ToString("g");
            label_LastEdited.Text = "Останнє редагування: " + recipe.LastEdited.ToString("g");
        
            if (!string.IsNullOrEmpty(recipe.MainPhotoPath) && File.Exists(recipe.MainPhotoPath))
            {
                using (FileStream fs = new FileStream(recipe.MainPhotoPath, FileMode.Open, FileAccess.Read))
                {
                    MainPicBox.Image = Image.FromStream(fs);
                    _currentPhotoPath = recipe.MainPhotoPath;
                }
            }
            else
            {
                MainPicBox.Image = Resources.StockPhoto;
                _currentPhotoPath = string.Empty;
            }

            for (int i = 0; i < CheckedLB_FoodCategories.Items.Count; i++)
                CheckedLB_FoodCategories.SetItemChecked(i, false);

            for (int i = 0; i < CheckedLB_CookBooks.Items.Count; i++)
                CheckedLB_CookBooks.SetItemChecked(i, false);

            for (int i = 0; i < CheckedLB_FoodCategories.Items.Count; i++)
            {
                Category currentCat = (Category)CheckedLB_FoodCategories.Items[i];
                if (recipe.ReferencedCategories != null && recipe.ReferencedCategories.Contains(currentCat.ID))
                {
                    CheckedLB_FoodCategories.SetItemChecked(i, true);
                }
            }

            Response<List<CookBook>> cookBooksRes = DataService.Instance.GetCookBooks(DataService.DataScope.Local);
    
            if (cookBooksRes.status == Response<List<CookBook>>.responseStatus.success && cookBooksRes.data != null)
            {
                for (int i = 0; i < CheckedLB_CookBooks.Items.Count; i++)
                {
                    CookBook currentBook = (CookBook)CheckedLB_CookBooks.Items[i];
                    CookBook dbBook = cookBooksRes.data.FirstOrDefault(b => b.ID == currentBook.ID);
                    if (dbBook != null && dbBook.ReferencedRecipes != null && dbBook.ReferencedRecipes.Contains(recipe.ID))
                    {
                        CheckedLB_CookBooks.SetItemChecked(i, true);
                    }
                }
            }
        
            DGV_Ingredients.Rows.Clear();
            FLP_RecipeSteps.Controls.Clear();
        
            foreach (Ingredient ing in recipe.Ingredients)
            {
                int rowIndex = DGV_Ingredients.Rows.Add();
                DGV_Ingredients.Rows[rowIndex].Cells["IngredientColumn"].Value = ing.RawName;
                DGV_Ingredients.Rows[rowIndex].Cells["Weight"].Value = ing.Weight;
        
                UpdateRowCalculations(rowIndex);
            }
            foreach (Step step in recipe.PreparationSteps)
            {
                StepControl stepCtrl = new StepControl();
                stepCtrl.DeleteClicked += Step_DeleteClicked;
                stepCtrl.StepDescription = step.Description;
                stepCtrl.SetStepPhoto(step.PhotoPath);

                FLP_RecipeSteps.Controls.Add(stepCtrl);
            }
            updateStepNumbers();

            Label_Author.Text = $"Автор: {AppSession.Instance.CurrentUser.Name}";
            _isLoadingData = false;
            _isModified = false;
            BTN_DeleteRecipe.Visible = true;
            if (recipe.InspiredBy != Guid.Empty)
            {    
                var originalRecipeRes = DataService.Instance.GetRecipe(recipe.InspiredBy, DataService.DataScope.Global);
    
                if (originalRecipeRes.status == Response<Recipe>.responseStatus.success && originalRecipeRes.data != null)
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
        }

        public bool SetupForEdit(Recipe recipe)
        {
            if (_isModified)
            {
                DialogResult result = 
                MessageBox.Show("У вас є незбережені зміни в поточному рецепті. Зберегти їх перед відкриттям іншого?", "Увага",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    BTN_SaveRecipe_Click(null, null); 
                }
                else if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }

            _currentRecipe = recipe; 
            LoadRecipeData(recipe); 
            return true; 
        }
        #endregion

        #region Gather methods
        private List<Ingredient> GatherIngredientsFromGrid()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            foreach (DataGridViewRow row in DGV_Ingredients.Rows)
            {
                if (row.IsNewRow) continue;
                string aliasName = row.Cells["IngredientColumn"].Value?.ToString()?.Trim();
        
                if (string.IsNullOrEmpty(aliasName)) continue;
                double.TryParse(row.Cells["Weight"].Value?.ToString(), out double weight);

                Guid referencedProductId = Guid.Empty;         
                if (aliasToProductMap.TryGetValue(aliasName, out Product product))
                {
                    referencedProductId = product.ID;
                }

                ingredients.Add(new Ingredient
                {
                    ID = Guid.NewGuid(),
                    RawName = aliasName,
                    ReferencedProduct = referencedProductId,
                    Weight = weight
                });
            }
    
            return ingredients;
        }

        private List<Step> GatherStepsFromControls()
        {
            List<Step> steps = new List<Step>();
            int order = 1;

            foreach (StepControl stepCtrl in FLP_RecipeSteps.Controls.OfType<StepControl>())
            {
                steps.Add(new Step
                {
                    ID = Guid.NewGuid(),
                    Order = order++,
                    Description = stepCtrl.StepDescription,
                    PhotoPath = stepCtrl.PhotoPath
                    
                });
            }
            return steps;
        }

        private List<Guid> GatherSelectedCategoryIds()
        {
            List<Guid> selectedCategoryIds = new List<Guid>();
            foreach (object checkedItem in CheckedLB_FoodCategories.CheckedItems)
            {
                Category cat = (Category)checkedItem;
                selectedCategoryIds.Add(cat.ID);
            }
            return selectedCategoryIds;
        }

        private List<Guid> GatherSelectedCookBookIds()
        {
            List<Guid> selectedCookBookIds = new List<Guid>();
            foreach (object checkedItem in CheckedLB_CookBooks.CheckedItems)
            {
                CookBook book = (CookBook)checkedItem;
                selectedCookBookIds.Add(book.ID);
            }
            return selectedCookBookIds;
        }

        private TimeSpan ParseTimeFromComboBox(string comboBoxText)
        {
            if (string.IsNullOrEmpty(comboBoxText) || comboBoxText == "Не вибрано")
            {
                return TimeSpan.Zero;
            }

            switch (comboBoxText.Trim())
            {
                case "5 хв": return TimeSpan.FromMinutes(5);
                case "10 хв": return TimeSpan.FromMinutes(10);
                case "15 хв": return TimeSpan.FromMinutes(15);
                case "30 хв": return TimeSpan.FromMinutes(30);
                case "45 хв": return TimeSpan.FromMinutes(45);
                case "1 год": return TimeSpan.FromHours(1);
                case "1.5 год": return TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(30));
                case "2 год": return TimeSpan.FromHours(2);
                case "3+ год": return TimeSpan.FromHours(3);
                default: return TimeSpan.Zero;
            }
        }

        private string FormatTimeForComboBox(TimeSpan time)
        {
            if (time == TimeSpan.Zero) return "Не вибрано";
    
            if (time.TotalHours >= 1)
            {
                if (time.TotalMinutes == 60) return "1 год";
                if (time.TotalMinutes == 90) return "1.5 год";
                if (time.TotalMinutes == 120) return "2 год";
                return "3+ год";
            }
    
            return time.TotalMinutes.ToString() + " хв";
        }
        #endregion

        #region Other helpers
        private void SetModified()
        {
            _isModified = true;
        }

        public void SetupForCreate()
        {
            _currentRecipe = null;
            _isModified = false;
            BTN_DeleteRecipe.Visible = false;
            ClearForm();
        }

        private void ClearForm()
        {
            txtbox_RecipeName.Clear();
            txtbox_Description.Clear();
            txtbox_Link.Clear();
            MainPicBox.Image = Resources.StockPhoto;
            _currentPhotoPath = string.Empty;

            // Очищаємо рядки таблиці
            DGV_Ingredients.Rows.Clear();
            
            // 🔥 ФІКС ВИСОТИ: Повертаємо таблиці та панелі їх початковий мінімальний розмір
            int headerHeight = DGV_Ingredients.ColumnHeadersHeight;
            DGV_Ingredients.Height = headerHeight;
            // Обнуляємо висоту панелі, вона сама налаштується під хедер та панель калорійності
            IngredientsPanel.Height = headerHeight + CaloriesValPanel.Height + 20;

            CheckedLB_FoodCategories.ClearSelected();
            CheckedLB_CookBooks.ClearSelected();
            
            combox_ActivePreparationTime.SelectedIndex = -1;
            combox_ActivePreparationTime.Text = "";
            combox_FullPreparationTime.SelectedIndex = -1;
            combox_FullPreparationTime.Text = "";
            
            combox_ServingsCount.SelectedIndex = -1;
            combox_ServingsCount.Text = "";
            
            CB_IsPublic.Checked = false;

            FLP_RecipeSteps.Controls.Clear();
            _isModified = false;

            label_CreationDate.Text = "Дата створення: ";
            label_LastEdited.Text = "Останнє редагування: ";
        }

        private void AnyControl_ValueChanged(object sender, EventArgs e)
        {
            if (_isLoadingData) return;
            CalculateTotalNutrients();
            SetModified();
        }

        public bool CheckForUnsavedChanges()
        {
            if (!_isModified) return true;

            DialogResult result = 
            MessageBox.Show("У вас є незбережені зміни. Зберегти їх?", "Вихід", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                BTN_SaveRecipe_Click(null, null);
                return true; 
            }
            else if (result == DialogResult.No)
            {
                return true;
            }
            return false;
        }
        #endregion


    }
}