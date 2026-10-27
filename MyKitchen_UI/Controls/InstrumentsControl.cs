using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MyKitchen_Core.Entities;
using MyKitchen_Core.Services;
using MyKitchen_Core.Tools;
using MyKitchen_Core.DTO.CreateAndUpdateDTOs;
using MyKitchen_UI.Interfaces;

namespace MyKitchen_UI.Controls
{
    public partial class InstrumentsControl : UserControl, IRefreshableTab
    {
        private List<Product> _allProducts = new List<Product>();
        private List<ProductAlias> _allAliases = new List<ProductAlias>();
        
        private Product? _currentProduct = null;
        private bool _isLoadingData = false;

        public InstrumentsControl()
        {
            InitializeComponent();
            SetupInitialState();
            LoadData();
        }

        public void OnTabActivated()
        {
            LoadData();
            ClearAliasFields();
        }

        #region Initialization & UI State

        private void SetupInitialState()
        {
            BTN_SaveProduct.Visible = false;
            BTN_DeleteProduct.Visible = false;
            BTN_AddProductAlias.Visible = false;
            BTN_DeleteProductAlias.Visible = false;

            label_IsCustom.Text = "";

            ToggleEditFields(false);

            // Реєстрація базових івентів пошуку та вибору
            TXT_ProductNameSearch.TextChanged += SearchProduct_TextChanged;

            // Слідкуємо за змінами полів БЖВ
            TXT_ProductName.TextChanged += OnProductModified;
            TXT_Proteins.TextChanged += OnProductModified;
            TXT_Fats.TextChanged += OnProductModified;
            TXT_Carbohydrates.TextChanged += OnProductModified;
            TXT_ProductAliasName.TextChanged += OnAliasNameChanged;

            // Захист від введення тексту в цифрові поля КБЖУ
            TXT_Proteins.KeyPress += NumericOnly_KeyPress;
            TXT_Fats.KeyPress += NumericOnly_KeyPress;
            TXT_Carbohydrates.KeyPress += NumericOnly_KeyPress;

            BTN_DeleteProduct.Click += BTN_DeleteProduct_Click;
            BTN_DeleteProductAlias.Click += BTN_DeleteProductAlias_Click;
        }

        private void ToggleEditFields(bool canEdit)
        {
            TXT_ProductName.ReadOnly = !canEdit;
            TXT_Proteins.ReadOnly = !canEdit;
            TXT_Fats.ReadOnly = !canEdit;
            TXT_Carbohydrates.ReadOnly = !canEdit;
        }

        private void NumericOnly_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // Дозволяємо лише одну крапку розділювача
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void ClearAliasFields()
        {
            TXT_ProductAliasName.Text = "";
            BTN_AddProductAlias.Visible = false;
            BTN_DeleteProductAlias.Visible = false;
        }

        #endregion

        #region Data Loading

        private void LoadData()
        {
            _isLoadingData = true;

            Response<List<Product>> productsRes = DataService.Instance.GetProducts(DataService.DataScope.Local);
            if (productsRes.status != Response<List<Product>>.responseStatus.error && productsRes.data != null)
            {
                _allProducts = productsRes.data.OrderBy(p => p.Name).ToList();
            }

            Response<List<ProductAlias>> aliasesRes = DataService.Instance.GetProductAliases(DataService.DataScope.Local);
            if (aliasesRes.status != Response<List<ProductAlias>>.responseStatus.error && aliasesRes.data != null)
            {
                _allAliases = aliasesRes.data;
            }

            UpdateProductsList(_allProducts);
            _isLoadingData = false;
        }

        private void UpdateProductsList(List<Product> productsToDisplay)
        {
            ListBox_Products.Items.Clear();
            ListBox_Products.DisplayMember = "Name";

            foreach (var product in productsToDisplay)
            {
                ListBox_Products.Items.Add(product);
            }
        }

        private void UpdateAliasesList(Guid productId)
        {
            ListBox_ProductAliases.Items.Clear();
            ListBox_ProductAliases.DisplayMember = "AliasName";

            var productAliases = _allAliases.Where(a => a.ReferencedProduct == productId).ToList();
            foreach (var alias in productAliases)
            {
                ListBox_ProductAliases.Items.Add(alias);
            }
        }

        #endregion

        #region Interactions & Event Handlers

        private void SearchProduct_TextChanged(object? sender, EventArgs e)
        {
            string query = TXT_ProductNameSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(query))
            {
                UpdateProductsList(_allProducts);
            }
            else
            {
                var filtered = _allProducts.Where(p => p.Name != null && p.Name.ToLower().Contains(query)).ToList();
                UpdateProductsList(filtered);
            }
        }

        private void ListBox_Products_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (ListBox_Products.SelectedItem is Product selectedProduct)
            {
                _isLoadingData = true;
                _currentProduct = selectedProduct;

                TXT_ProductName.Text = selectedProduct.Name;
                TXT_Proteins.Text = selectedProduct.Proteins.ToString("F1");
                TXT_Fats.Text = selectedProduct.Fats.ToString("F1");
                TXT_Carbohydrates.Text = selectedProduct.Carbohydrates.ToString("F1");

                var calRes = CaloriesCalculator.calculateCalories(selectedProduct.Proteins, selectedProduct.Carbohydrates, selectedProduct.Fats);
                if (calRes.status == Response<double>.responseStatus.success)
                {
                    label_Calories.Text = $"Калорійність: {calRes.data:F1} ккал";
                }

                if (selectedProduct.IsCustom)
                {
                    label_IsCustom.Text = "Користувацький продукт";
                    BTN_DeleteProduct.Visible = true;
                }
                else
                {
                    label_IsCustom.Text = "⚠ Системний продукт (не редагується)";
                    BTN_DeleteProduct.Visible = false;
                }

                UpdateAliasesList(selectedProduct.ID);
                ToggleEditFields(selectedProduct.IsCustom); 
                
                BTN_SaveProduct.Visible = false;
                ClearAliasFields();

                _isLoadingData = false;
            }
        }

        private void ListBox_ProductAliases_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (ListBox_ProductAliases.SelectedItem is ProductAlias selectedAlias)
            {
                TXT_ProductAliasName.Text = selectedAlias.AliasName;
                BTN_DeleteProductAlias.Visible = true;
                BTN_AddProductAlias.Visible = false;
            }
        }

        private void OnProductModified(object? sender, EventArgs e)
        {
            if (_isLoadingData || _currentProduct == null) return;
            
            // Показуємо кнопку збереження лише якщо це кастомний продукт
            if (_currentProduct.IsCustom)
            {
                BTN_SaveProduct.Visible = true;
            }

            double.TryParse(TXT_Proteins.Text, out double p);
            double.TryParse(TXT_Fats.Text, out double f);
            double.TryParse(TXT_Carbohydrates.Text, out double c);
            
            var calRes = CaloriesCalculator.calculateCalories(p, c, f);
            if (calRes.status == Response<double>.responseStatus.success)
            {
                label_Calories.Text = $"Калорійність: {calRes.data:F1} ккал";
            }
        }

        private void OnAliasNameChanged(object? sender, EventArgs e)
        {
            if (_currentProduct == null) return;

            string aliasText = TXT_ProductAliasName.Text.Trim();
            
            // Якщо такий аліас вже є в списку обраного продукту — ховаємо кнопку додавання
            bool aliasExists = _allAliases.Any(a => a.ReferencedProduct == _currentProduct.ID && 
                                                    a.AliasName.Equals(aliasText, StringComparison.OrdinalIgnoreCase));

            BTN_AddProductAlias.Visible = !string.IsNullOrEmpty(aliasText) && !aliasExists;
        }

        #endregion

        #region CRUD Керування продуктами

        private void BTN_CreateProduct_Click(object? sender, EventArgs e)
        {
            _isLoadingData = true;
            _currentProduct = null;

            TXT_ProductName.Text = "Новий продукт";
            TXT_Proteins.Text = "0.0";
            TXT_Fats.Text = "0.0";
            TXT_Carbohydrates.Text = "0.0";
            label_Calories.Text = "Калорійність: 0.0 ккал";
            label_IsCustom.Text = "Створення нового користувацького продукту";

            ToggleEditFields(true);
            ListBox_ProductAliases.Items.Clear();
            ClearAliasFields();

            BTN_SaveProduct.Visible = true;
            BTN_DeleteProduct.Visible = false;
            _isLoadingData = false;
        }

        private void BTN_SaveProduct_Click(object? sender, EventArgs e)
        {
            string name = TXT_ProductName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Назва продукту не може бути порожньою!", "Помилка валідації", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double.TryParse(TXT_Proteins.Text, out double p);
            double.TryParse(TXT_Fats.Text, out double f);
            double.TryParse(TXT_Carbohydrates.Text, out double c);

            // Розрахунок калорій
            double calories = 0;
            var calRes = CaloriesCalculator.calculateCalories(p, c, f);
            if (calRes.status == Response<double>.responseStatus.success) calories = calRes.data;

            if (_currentProduct == null) // СТВОРЕННЯ (Використовуємо CreateProduct_dto)
            {
                CreateProduct_dto newProductDto = new CreateProduct_dto
                {
                    Name = name,
                    Proteins = p,
                    Fats = f,
                    Carbohydrates = c,
                    Calories = calories
                };

                var res = DataService.Instance.AddProduct(newProductDto);
                if (res.status != Response<Product>.responseStatus.error && res.data != null)
                {
                    MessageBox.Show("Продукт успішно додано до вашої бази!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    SelectProductInListBox(res.data.ID);
                }
                else
                {
                    MessageBox.Show("Помилка збереження: " + res.message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // ОНОВЛЕННЯ (Використовуємо UpdateProduct_dto)
            {
                if (!_currentProduct.IsCustom) return; // Захист

                UpdateProduct_dto updateProductDto = new UpdateProduct_dto
                {
                    ID = _currentProduct.ID,
                    Name = name,
                    Proteins = p,
                    Fats = f,
                    Carbohydrates = c,
                    Calories = calories
                };

                var res = DataService.Instance.UpdateProduct(updateProductDto);
                if (res.status != Response<Product>.responseStatus.error)
                {
                    MessageBox.Show("Дані продукту успішно оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    SelectProductInListBox(_currentProduct.ID);
                }
                else
                {
                    MessageBox.Show("Помилка оновлення: " + res.message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BTN_DeleteProduct_Click(object? sender, EventArgs e)
        {
            // 1. Перевірка наявності продукту
            if (_currentProduct == null)
            {
                MessageBox.Show("Помилка: Продукт не обрано.", "Видалення", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2. Перевірка на "системність"
            if (!_currentProduct.IsCustom)
            {
                MessageBox.Show("Системні продукти видаляти не можна!", "Видалення", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Ви впевнені, що хочете назавжди видалити продукт '{_currentProduct.Name}'?\n" +
                "⚠️ Увага: Усі його псевдоніми та синоніми також будуть автоматично видалені для збереження цілісності бази даних!",
                "Каскадне видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // КРОК 3: КАСКАДНЕ ВИДАЛЕННЯ АЛІАСІВ ЦЬОГО ПРОДУКТУ
                // Знаходимо всі аліаси, які прив'язані до ID поточного продукту
                var aliasesToRemove = _allAliases.Where(a => a.ReferencedProduct == _currentProduct.ID).ToList();
                bool aliasDeletionFailed = false;
                string aliasErrorMessage = "";

                foreach (var alias in aliasesToRemove)
                {
                    var aliasRes = DataService.Instance.RemoveObject(
                        MyKitchen_Core.Interfaces.ILocalRepository.dataType.productAlias,
                        alias.ID,
                        DataService.DataScope.Local);

                    if (aliasRes.status == Response<bool>.responseStatus.error)
                    {
                        aliasDeletionFailed = true;
                        aliasErrorMessage = aliasRes.message;
                        break;
                    }
                }

                // Якщо аліаси не видалилися, зупиняємо процес, щоб не плодити помилки цілісності
                if (aliasDeletionFailed)
                {
                    MessageBox.Show($"Не вдалося очистити пов'язані псевдоніми: {aliasErrorMessage}\nОперацію скасовано.", 
                                    "Помилка цілісності", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 4. Спроба видалення самого продукту (коли аліаси вже успішно стерті)
                var res = DataService.Instance.RemoveObject(
                    MyKitchen_Core.Interfaces.ILocalRepository.dataType.product, 
                    _currentProduct.ID, 
                    DataService.DataScope.Local);

                // 5. Аналіз результату
                if (res.status == Response<bool>.responseStatus.success)
                {
                    MessageBox.Show("Продукт та всі його псевдоніми успішно видалені з системи!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(); // Перезавантажуємо списки з диска
                    ClearProductFields(); // Очищаємо форми
                }
                else
                {
                    MessageBox.Show($"Помилка видалення продукту: {res.message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearProductFields()
        {
            TXT_ProductName.Clear();
            TXT_Proteins.Clear();
            TXT_Fats.Clear();
            TXT_Carbohydrates.Clear();
            label_Calories.Text = "Калорійність: 0.0 ккал";
            label_IsCustom.Text = "";
            ListBox_ProductAliases.Items.Clear();
            ClearAliasFields();
            BTN_SaveProduct.Visible = false;
            BTN_DeleteProduct.Visible = false;
        }

        private void SelectProductInListBox(Guid id)
        {
            for (int i = 0; i < ListBox_Products.Items.Count; i++)
            {
                if (ListBox_Products.Items[i] is Product p && p.ID == id)
                {
                    ListBox_Products.SelectedIndex = i;
                    break;
                }
            }
        }

        #endregion

        #region CRUD Керування псевдонімами

        private void BTN_AddProductAlias_Click(object? sender, EventArgs e)
        {
            if (_currentProduct == null) return;

            string aliasName = TXT_ProductAliasName.Text.Trim();
            if (string.IsNullOrEmpty(aliasName)) return;


            CreateProductAlias_dto newAliasDto = new CreateProductAlias_dto
            {
                AliasName = aliasName,
                ReferencedProduct = _currentProduct.ID
            };

            var res = DataService.Instance.AddProductAlias(newAliasDto);
            if (res.status != Response<ProductAlias>.responseStatus.error)
            {
                LoadData(); // Оновлюємо внутрішні кеші
                UpdateAliasesList(_currentProduct.ID); // Перемальовуємо UI аліасів
                TXT_ProductAliasName.Clear();
                BTN_AddProductAlias.Visible = false;
            }
            else
            {
                MessageBox.Show("Помилка при збереженні псевдоніму: " + res.message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BTN_DeleteProductAlias_Click(object? sender, EventArgs e)
        {
            if (ListBox_ProductAliases.SelectedItem is ProductAlias selectedAlias)
            {
                var res = DataService.Instance.RemoveObject(
                    MyKitchen_Core.Interfaces.ILocalRepository.dataType.productAlias, 
                    selectedAlias.ID, 
                    DataService.DataScope.Local);

                if (res.status == Response<bool>.responseStatus.success)
                {
                    MessageBox.Show("Псевдонім успішно видалено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    if (_currentProduct != null) UpdateAliasesList(_currentProduct.ID);
                    TXT_ProductAliasName.Clear();
                    BTN_DeleteProductAlias.Visible = false;
                }
                else
                {
                    MessageBox.Show($"Помилка при видаленні псевдоніму: {res.message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, оберіть псевдонім для видалення.", "Вибір", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        private void BTN_CreateReport_Click(object? sender, EventArgs e)
        {
            // 1. Зчитуємо повні пакети даних через бізнес-сервіс додатка
            var localRecipesRes = DataService.Instance.GetRecipes(DataService.DataScope.Local);
            var localBooksRes = DataService.Instance.GetCookBooks(DataService.DataScope.Local);
            var globalRecipesRes = DataService.Instance.GetRecipes(DataService.DataScope.Global);
            var globalBooksRes = DataService.Instance.GetCookBooks(DataService.DataScope.Global);

            var localRecipes = localRecipesRes.data ?? new List<Recipe>();
            var localBooks = localBooksRes.data ?? new List<CookBook>();
            var globalRecipes = globalRecipesRes.data ?? new List<Recipe>();
            var globalBooks = globalBooksRes.data ?? new List<CookBook>();

            List<UserProfile> emptyUsersList = new List<UserProfile>();

            // 2. Ініціалізуємо стандартний системний діалог вибору шляху на диску
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Виберіть місце для збереження повного системного звіту";
                sfd.Filter = "Текстові файли (*.txt)|*.txt|Усі файли (*.*)|*.*";
                sfd.FileName = $"Системний_Звіт_{DateTime.Now:yyyy-MM-dd}"; // Дефолтне ім'я файлу
                sfd.DefaultExt = "txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // 3. Викликаємо твій клас з API
                    var reportResult = MyKitchen_Core.Services.ReportBuilder.GenerateCombinedReport(
                        localRecipes, 
                        localBooks, 
                        globalRecipes, 
                        globalBooks, 
                        emptyUsersList, 
                        sfd.FileName
                    );

                    // 4. Аналізуємо статус відповіді сервера (Response)
                    if (reportResult.status == Response<bool>.responseStatus.success)
                    {
                        MessageBox.Show($"Повний звіт системи успішно згенеровано та збережено за шляхом:\n{sfd.FileName}", 
                            "Генерація звітів", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося скомпілювати звіт: " + reportResult.message, 
                            "Помилка експорту", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}