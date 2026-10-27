using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MyKitchen_Core.Entities;
using MyKitchen_UI.Properties;

namespace MyKitchen_UI
{
    public partial class RecipeCardControl :UserControl
    {
        public Recipe TargetRecipe { get; private set; }
        public event EventHandler OpenClicked;

        public RecipeCardControl(Recipe recipe, double calories, double proteins, double fats, double carbs)
        {
            InitializeComponent();
            TargetRecipe = recipe;

            LB_Name.Text = recipe.Name;
            string recipeName = recipe.Name ?? "Без назви";
            LB_Name.Text = recipeName.Length > 30 ? recipeName.Substring(0, 27) + "..." : recipeName;

            string recipeDesc = recipe.Description ?? "Опис відсутній.";
            LB_Description.Text = recipeDesc.Length > 60 ? recipeDesc.Substring(0, 57) + "..." : recipeDesc;

            LB_Calories.Text = "Ккал: " + calories.ToString("F0");
            LB_Proteins.Text = "Б: " + proteins.ToString("F1") + "г";
            LB_Fats.Text = "Ж: " + fats.ToString("F1") + "г";
            LB_Carbohydrates.Text = "В: " + carbs.ToString("F1") + "г";

            LoadRecipeImage();
            RegisterClickEvents();
        }

        private void LoadRecipeImage()
        {
            if(!string.IsNullOrEmpty(TargetRecipe.MainPhotoPath) && File.Exists(TargetRecipe.MainPhotoPath))
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(TargetRecipe.MainPhotoPath);
                    using(MemoryStream ms = new MemoryStream(bytes))
                    {
                        RecipePicBox.Image = Image.FromStream(ms);
                    }
                }
                catch
                {
                    RecipePicBox.Image = Resources.StockPhoto;
                }
            }
            else
            {
                RecipePicBox.Image = Resources.StockPhoto;
            }
        }

        private void RegisterClickEvents()
        {
            RecipeCardPanel.Click += Card_Click;
            LB_Name.Click += Card_Click;
            LB_Description.Click += Card_Click;
            label4.Click += Card_Click;
            RecipePicBox.Click += Card_Click;
            LB_Calories.Click += Card_Click;
            LB_Proteins.Click += Card_Click;
            LB_Fats.Click += Card_Click;
            LB_Carbohydrates.Click += Card_Click;
        }

        private void Card_Click(object sender, EventArgs e)
        {
            OpenClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}