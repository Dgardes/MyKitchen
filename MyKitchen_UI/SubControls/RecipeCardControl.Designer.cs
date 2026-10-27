namespace MyKitchen_UI
{
    partial class RecipeCardControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
        if(disposing && (components != null))
        {
        components.Dispose();
        }
        base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecipeCardControl));
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        RecipePicBox = new Guna.UI2.WinForms.Guna2PictureBox();
        RecipeCardPanel = new Guna.UI2.WinForms.Guna2Panel();
        LB_Description = new Label();
        LB_Fats = new Label();
        LB_Carbohydrates = new Label();
        label4 = new Label();
        LB_Proteins = new Label();
        LB_Name = new Label();
        LB_Calories = new Label();
        ((System.ComponentModel.ISupportInitialize)RecipePicBox).BeginInit();
        RecipeCardPanel.SuspendLayout();
        SuspendLayout();
        // 
        // RecipePicBox
        // 
        RecipePicBox.BackColor = Color.Transparent;
        RecipePicBox.BackgroundImageLayout = ImageLayout.None;
        RecipePicBox.CustomizableEdges = customizableEdges1;
        RecipePicBox.Image = (Image)resources.GetObject("RecipePicBox.Image");
        RecipePicBox.ImageRotate = 0F;
        RecipePicBox.Location = new Point(15, 54);
        RecipePicBox.Name = "RecipePicBox";
        RecipePicBox.ShadowDecoration.CustomizableEdges = customizableEdges2;
        RecipePicBox.Size = new Size(350, 207);
        RecipePicBox.SizeMode = PictureBoxSizeMode.Zoom;
        RecipePicBox.TabIndex = 31;
        RecipePicBox.TabStop = false;
        // 
        // RecipeCardPanel
        // 
        RecipeCardPanel.BackColor = Color.FromArgb(255, 230, 210);
        RecipeCardPanel.BorderColor = Color.FromArgb(240, 112, 22);
        RecipeCardPanel.BorderRadius = 15;
        RecipeCardPanel.BorderThickness = 2;
        RecipeCardPanel.Controls.Add(LB_Description);
        RecipeCardPanel.Controls.Add(LB_Fats);
        RecipeCardPanel.Controls.Add(LB_Carbohydrates);
        RecipeCardPanel.Controls.Add(label4);
        RecipeCardPanel.Controls.Add(LB_Proteins);
        RecipeCardPanel.Controls.Add(LB_Name);
        RecipeCardPanel.Controls.Add(LB_Calories);
        RecipeCardPanel.Controls.Add(RecipePicBox);
        RecipeCardPanel.CustomizableEdges = customizableEdges3;
        RecipeCardPanel.Dock = DockStyle.Fill;
        RecipeCardPanel.FillColor = Color.FromArgb(255, 230, 210);
        RecipeCardPanel.Location = new Point(0, 0);
        RecipeCardPanel.Name = "RecipeCardPanel";
        RecipeCardPanel.ShadowDecoration.CustomizableEdges = customizableEdges4;
        RecipeCardPanel.Size = new Size(380, 426);
        RecipeCardPanel.TabIndex = 48;
        // 
        // LB_Description
        // 
        LB_Description.BackColor = Color.FromArgb(255, 230, 210);
        LB_Description.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
        LB_Description.ForeColor = Color.FromArgb(78, 52, 46);
        LB_Description.Location = new Point(15, 282);
        LB_Description.Name = "LB_Description";
        LB_Description.Size = new Size(350, 98);
        LB_Description.TabIndex = 56;
        LB_Description.Text = "LB_Description";
        // 
        // LB_Fats
        // 
        LB_Fats.AutoSize = true;
        LB_Fats.BackColor = Color.FromArgb(255, 230, 210);
        LB_Fats.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
        LB_Fats.ForeColor = Color.FromArgb(78, 52, 46);
        LB_Fats.Location = new Point(205, 392);
        LB_Fats.Name = "LB_Fats";
        LB_Fats.Size = new Size(39, 25);
        LB_Fats.TabIndex = 55;
        LB_Fats.Text = "Ж:";
        // 
        // LB_Carbohydrates
        // 
        LB_Carbohydrates.AutoSize = true;
        LB_Carbohydrates.BackColor = Color.FromArgb(255, 230, 210);
        LB_Carbohydrates.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
        LB_Carbohydrates.ForeColor = Color.FromArgb(78, 52, 46);
        LB_Carbohydrates.Location = new Point(290, 392);
        LB_Carbohydrates.Name = "LB_Carbohydrates";
        LB_Carbohydrates.Size = new Size(32, 25);
        LB_Carbohydrates.TabIndex = 54;
        LB_Carbohydrates.Text = "В:";
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.BackColor = Color.FromArgb(255, 230, 210);
        label4.Font = new Font("Microsoft Sans Serif", 9.749999F, FontStyle.Regular, GraphicsUnit.Point, 204);
        label4.ForeColor = Color.FromArgb(78, 52, 46);
        label4.Location = new Point(24, 264);
        label4.Name = "label4";
        label4.Size = new Size(120, 20);
        label4.TabIndex = 53;
        label4.Text = "Опис страви:";
        // 
        // LB_Proteins
        // 
        LB_Proteins.AutoSize = true;
        LB_Proteins.BackColor = Color.FromArgb(255, 230, 210);
        LB_Proteins.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
        LB_Proteins.ForeColor = Color.FromArgb(78, 52, 46);
        LB_Proteins.Location = new Point(123, 392);
        LB_Proteins.Name = "LB_Proteins";
        LB_Proteins.Size = new Size(32, 25);
        LB_Proteins.TabIndex = 52;
        LB_Proteins.Text = "Б:";
        // 
        // LB_Name
        // 
        LB_Name.BackColor = Color.FromArgb(255, 230, 210);
        LB_Name.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
        LB_Name.ForeColor = Color.FromArgb(78, 52, 46);
        LB_Name.Location = new Point(15, 7);
        LB_Name.Name = "LB_Name";
        LB_Name.Size = new Size(350, 55);
        LB_Name.TabIndex = 51;
        LB_Name.Text = "Назва рецепту";
        LB_Name.TextAlign = ContentAlignment.TopCenter;
        // 
        // LB_Calories
        // 
        LB_Calories.AutoSize = true;
        LB_Calories.BackColor = Color.FromArgb(255, 230, 210);
        LB_Calories.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
        LB_Calories.ForeColor = Color.FromArgb(78, 52, 46);
        LB_Calories.Location = new Point(10, 391);
        LB_Calories.Name = "LB_Calories";
        LB_Calories.Size = new Size(67, 25);
        LB_Calories.TabIndex = 50;
        LB_Calories.Text = "Ккал:";
        // 
        // RecipeCardControl
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(255, 230, 210);
        Controls.Add(RecipeCardPanel);
        Name = "RecipeCardControl";
        Size = new Size(380, 426);
        ((System.ComponentModel.ISupportInitialize)RecipePicBox).EndInit();
        RecipeCardPanel.ResumeLayout(false);
        RecipeCardPanel.PerformLayout();
        ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox RecipePicBox;
        private Guna.UI2.WinForms.Guna2Panel RecipeCardPanel;
        private Label LB_Calories;
        private Label LB_Proteins;
        private Label LB_Name;
        private Label LB_Fats;
        private Label LB_Carbohydrates;
        private Label label4;
        private Label LB_Description;
    }
}
