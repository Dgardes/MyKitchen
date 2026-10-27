namespace MyKitchen_UI
{
    partial class StepViewControl
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
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
        label_Description = new Label();
        label_StepNumber = new Label();
        StepPicBox = new Guna.UI2.WinForms.Guna2PictureBox();
        guna2Panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)StepPicBox).BeginInit();
        SuspendLayout();
        // 
        // guna2Panel1
        // 
        guna2Panel1.BorderColor = Color.FromArgb(240, 112, 22);
        guna2Panel1.BorderRadius = 15;
        guna2Panel1.BorderThickness = 2;
        guna2Panel1.Controls.Add(label_Description);
        guna2Panel1.Controls.Add(label_StepNumber);
        guna2Panel1.Controls.Add(StepPicBox);
        guna2Panel1.CustomizableEdges = customizableEdges3;
        guna2Panel1.Dock = DockStyle.Fill;
        guna2Panel1.FillColor = Color.FromArgb(255, 230, 210);
        guna2Panel1.Location = new Point(0, 0);
        guna2Panel1.Name = "guna2Panel1";
        guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges4;
        guna2Panel1.Size = new Size(861, 315);
        guna2Panel1.TabIndex = 1;
        // 
        // label_Description
        // 
        label_Description.BackColor = Color.FromArgb(255, 230, 210);
        label_Description.Font = new Font("Montserrat Medium", 12F, FontStyle.Bold);
        label_Description.ForeColor = Color.FromArgb(78, 52, 46);
        label_Description.Location = new Point(13, 54);
        label_Description.Name = "label_Description";
        label_Description.Size = new Size(365, 233);
        label_Description.TabIndex = 50;
        label_Description.Text = "Description";
        // 
        // label_StepNumber
        // 
        label_StepNumber.AutoSize = true;
        label_StepNumber.BackColor = Color.FromArgb(255, 230, 210);
        label_StepNumber.Font = new Font("Montserrat Medium", 12F, FontStyle.Bold);
        label_StepNumber.ForeColor = Color.FromArgb(78, 52, 46);
        label_StepNumber.Location = new Point(13, 21);
        label_StepNumber.Name = "label_StepNumber";
        label_StepNumber.Size = new Size(81, 22);
        label_StepNumber.TabIndex = 49;
        label_StepNumber.Text = "Крок №";
        // 
        // StepPicBox
        // 
        StepPicBox.BackColor = Color.FromArgb(255, 230, 210);
        StepPicBox.BackgroundImageLayout = ImageLayout.None;
        StepPicBox.BorderRadius = 15;
        StepPicBox.CustomizableEdges = customizableEdges1;
        StepPicBox.ImageRotate = 0F;
        StepPicBox.Location = new Point(397, 21);
        StepPicBox.Name = "StepPicBox";
        StepPicBox.ShadowDecoration.CustomizableEdges = customizableEdges2;
        StepPicBox.Size = new Size(447, 266);
        StepPicBox.SizeMode = PictureBoxSizeMode.Zoom;
        StepPicBox.TabIndex = 31;
        StepPicBox.TabStop = false;
        // 
        // StepViewControl
        // 
        AutoScaleMode = AutoScaleMode.None;
        Controls.Add(guna2Panel1);
        Name = "StepViewControl";
        Size = new Size(861, 315);
        guna2Panel1.ResumeLayout(false);
        guna2Panel1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)StepPicBox).EndInit();
        ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Label label_Description;
        private Label label_StepNumber;
        private Guna.UI2.WinForms.Guna2PictureBox StepPicBox;
    }
}
