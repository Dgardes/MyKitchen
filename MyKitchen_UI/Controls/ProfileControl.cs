using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MyKitchen_Core.Entities;
using MyKitchen_Core.Services;
using MyKitchen_Core.Tools;
using MyKitchen_Core.DTO.SearchDTOs;
using MyKitchen_UI.Interfaces;

namespace MyKitchen_UI
{
    public partial class ProfileControl : UserControl, IRefreshableTab
    {
        public ProfileControl()
        {
            InitializeComponent();

            ChangeProfilePanel.Visible = false;
            LoginPanel.Visible = true;
            RegisterPanel.Visible = false;

            UpdateUIState();
        }

        public void OnTabActivated()
        {
            UpdateUIState();
        }

        private void UpdateUIState()
        {
            Response<List<Recipe>> localRecipesRes = DataService.Instance.GetRecipes(DataService.DataScope.Local);
            List<Recipe> allLocalRecipes = localRecipesRes.data ?? new List<Recipe>();
            
            if (localRecipesRes.status != Response<List<Recipe>>.responseStatus.error)
            {
                // Загальна статистика бази (залишаємо без змін)
                LB_LocalRecipesCount.Text = "Кількість рецептів у локальному сховищі: " + allLocalRecipes.Count.ToString();
            }

            Response<List<CookBook>> localBooksRes = DataService.Instance.GetCookBooks(DataService.DataScope.Local);
            List<CookBook> allLocalBooks = localBooksRes.data ?? new List<CookBook>();
            
            if (localBooksRes.status != Response<List<CookBook>>.responseStatus.error)
            {
                // Загальна статистика бази (залишаємо без змін)
                LB_LocalCookBooksCount.Text = "Кількість книг рецептів у локальному сховищі: " + allLocalBooks.Count.ToString();
            }

            UserProfile currentUser = AppSession.Instance.CurrentUser;

            if (currentUser != null)
            {
                LB_UserName.Text = "Ім'я користувача: " + currentUser.Name;
                BTN_Logout.Visible = true;
                BTN_ChangeProfile.Text = "Змінити профіль";

                int ownRecipesCount = allLocalRecipes.Count(r => r.AuthorID == currentUser.ID && r.InspiredBy == Guid.Empty);
                LB_OwnRecipes.Text = "Кількість власних рецептів користувача: " + ownRecipesCount.ToString();

                int downloadedRecipesCount = allLocalRecipes.Count(r => r.AuthorID == currentUser.ID && r.InspiredBy != Guid.Empty);
                LB_DownloadedRecipes.Text = "Кількість завантажених рецептів: " + downloadedRecipesCount.ToString();

                int publishedRecipesCount = allLocalRecipes.Count(r => r.AuthorID == currentUser.ID && r.IsPublic);
                Lb_PublishedUserRecipes.Text = "Кількість опублікованих рецептів користувача: " + publishedRecipesCount.ToString();

                int ownBooksCount = allLocalBooks.Count(b => b.AuthorID == currentUser.ID);
                LB_OwnCookBooks.Text = "Кількість власних книг рецептів користувача: " + ownBooksCount.ToString();

                int publishedBooksCount = allLocalBooks.Count(b => b.AuthorID == currentUser.ID && b.IsPublic);
                LB_PublishedUserCookBooks.Text = "Кількість опублікованих книг рецептів користувача: " + publishedBooksCount.ToString();
            }   
            else
            {
                LB_UserName.Text = "Ім'я користувача: Гість";
                LB_OwnRecipes.Text = "Кількість власних рецептів користувача: 0";
                LB_OwnCookBooks.Text = "Кількість власних книг рецептів користувача: 0";
                Lb_PublishedUserRecipes.Text = "Кількість опублікованих рецептів користувача: 0";
                LB_PublishedUserCookBooks.Text = "Кількість опублікованих книг рецептів користувача: 0";
                LB_DownloadedRecipes.Text = "Кількість завантажених рецептів: 0";
                
                BTN_Logout.Visible = false;
                BTN_ChangeProfile.Text = "Увійти в аккаунт";
            }
        }

        private void BTN_ChangeProfile_Click(object sender, EventArgs e)
        {
            ChangeProfilePanel.Visible = true;
            ChangeProfilePanel.BringToFront();
            LoginPanel.Visible = true;
            RegisterPanel.Visible = false;
        }

        private void BTN_Login_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible = true;
            RegisterPanel.Visible = false;
        }

        private void BTN_Register_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible = false;
            RegisterPanel.Visible = true;
        }

        private void BTN_ConfirmLogin_Click(object sender, EventArgs e)
        {
            string email = TXT_Email_Login.Text.Trim();
            string password = TXT_Password_Login.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля для входу!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Введіть коректну адресу електронної пошти!", "Помилка валідації", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Response<bool> loginRes = AppSession.Instance.Login(email, password);

            if (loginRes.status == Response<bool>.responseStatus.success && loginRes.data)
            {
                MessageBox.Show("Вхід виконано успішно!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                TXT_Email_Login.Clear();
                TXT_Password_Login.Clear();
                
                ChangeProfilePanel.Visible = false;
                UpdateUIState();
            }
            else
            {
                MessageBox.Show("Помилка авторизації: " + loginRes.message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BTN_ConfirmRegister_Click(object sender, EventArgs e)
        {
            string email = TXT_Email_Register.Text.Trim();
            string password = TXT_Password_Register.Text.Trim();
            string username = TXT_Username_Register.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля для реєстрації!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Введіть коректну адресу електронної пошти!", "Помилка валідації", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Response<bool> regRes = AppSession.Instance.Register(username, email, password);

            if (regRes.status == Response<bool>.responseStatus.success && regRes.data)
            {
                MessageBox.Show("Реєстрація пройшла успішно! Тепер ви можете авторизуватися.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                TXT_Email_Register.Clear();
                TXT_Password_Register.Clear();
                TXT_Username_Register.Clear();

                LoginPanel.Visible = true;
                RegisterPanel.Visible = false;
            }
            else
            {
                MessageBox.Show("Помилка реєстрації: " + regRes.message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BTN_Logout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Ви впевнені, що хочете вийти з аккаунту?", "Вихід", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                AppSession.Instance.Logout();
                UpdateUIState();
            }
        }
    }
}