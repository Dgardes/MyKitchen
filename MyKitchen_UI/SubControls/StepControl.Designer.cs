namespace MyKitchen_UI
{
    partial class StepControl
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
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
        BTN_DeleteStep = new Guna.UI2.WinForms.Guna2Button();
        txtbox_StepDescription = new Guna.UI2.WinForms.Guna2TextBox();
        label_StepNumber = new Label();
        BTN_SetStepPic = new Guna.UI2.WinForms.Guna2Button();
        BTN_DeleteStepPic = new Guna.UI2.WinForms.Guna2Button();
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
        guna2Panel1.Controls.Add(BTN_DeleteStep);
        guna2Panel1.Controls.Add(txtbox_StepDescription);
        guna2Panel1.Controls.Add(label_StepNumber);
        guna2Panel1.Controls.Add(BTN_SetStepPic);
        guna2Panel1.Controls.Add(BTN_DeleteStepPic);
        guna2Panel1.Controls.Add(StepPicBox);
        guna2Panel1.CustomizableEdges = customizableEdges11;
        guna2Panel1.Dock = DockStyle.Fill;
        guna2Panel1.FillColor = Color.FromArgb(255, 230, 210);
        guna2Panel1.Location = new Point(0, 0);
        guna2Panel1.Name = "guna2Panel1";
        guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges12;
        guna2Panel1.Size = new Size(853, 355);
        guna2Panel1.TabIndex = 0;
        // 
        // BTN_DeleteStep
        // 
        BTN_DeleteStep.BackColor = Color.FromArgb(255, 230, 210);
        BTN_DeleteStep.BorderRadius = 5;
        BTN_DeleteStep.CustomizableEdges = customizableEdges1;
        BTN_DeleteStep.DisabledState.BorderColor = Color.DarkGray;
        BTN_DeleteStep.DisabledState.CustomBorderColor = Color.DarkGray;
        BTN_DeleteStep.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
        BTN_DeleteStep.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
        BTN_DeleteStep.FillColor = Color.FromArgb(240, 112, 22);
        BTN_DeleteStep.Font = new Font("Montserrat Medium", 12F, FontStyle.Bold);
        BTN_DeleteStep.ForeColor = Color.White;
        BTN_DeleteStep.Location = new Point(203, 13);
        BTN_DeleteStep.Name = "BTN_DeleteStep";
        BTN_DeleteStep.ShadowDecoration.CustomizableEdges = customizableEdges2;
        BTN_DeleteStep.Size = new Size(173, 35);
        BTN_DeleteStep.TabIndex = 51;
        BTN_DeleteStep.Text = "Видалити крок";
        BTN_DeleteStep.Click += BTN_DeleteStep_Click;
        // 
        // txtbox_StepDescription
        // 
        txtbox_StepDescription.BackColor = Color.FromArgb(255, 230, 210);
        txtbox_StepDescription.BorderColor = Color.FromArgb(240, 112, 22);
        txtbox_StepDescription.BorderRadius = 5;
        txtbox_StepDescription.CustomizableEdges = customizableEdges3;
        txtbox_StepDescription.DefaultText = "";
        txtbox_StepDescription.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
        txtbox_StepDescription.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
        txtbox_StepDescription.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
        txtbox_StepDescription.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
        txtbox_StepDescription.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
        txtbox_StepDescription.Font = new Font("Montserrat Medium", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
        txtbox_StepDescription.ForeColor = Color.FromArgb(78, 52, 46);
        txtbox_StepDescription.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
        txtbox_StepDescription.Location = new Point(15, 57);
        txtbox_StepDescription.Margin = new Padding(4);
        txtbox_StepDescription.Multiline = true;
        txtbox_StepDescription.Name = "txtbox_StepDescription";
        txtbox_StepDescription.PlaceholderText = "";
        txtbox_StepDescription.SelectedText = "";
        txtbox_StepDescription.ShadowDecoration.CustomizableEdges = customizableEdges4;
        txtbox_StepDescription.Size = new Size(361, 284);
        txtbox_StepDescription.TabIndex = 50;
        // 
        // label_StepNumber
        // 
        label_StepNumber.AutoSize = true;
        label_StepNumber.BackColor = Color.FromArgb(255, 230, 210);
        label_StepNumber.Font = new Font("Montserrat Medium", 12F, FontStyle.Bold);
        label_StepNumber.ForeColor = Color.FromArgb(78, 52, 46);
        label_StepNumber.Location = new Point(15, 21);
        label_StepNumber.Name = "label_StepNumber";
        label_StepNumber.Size = new Size(81, 22);
        label_StepNumber.TabIndex = 49;
        label_StepNumber.Text = "Крок №";
        // 
        // BTN_SetStepPic
        // 
        BTN_SetStepPic.BackColor = Color.FromArgb(255, 230, 210);
        BTN_SetStepPic.BorderRadius = 5;
        BTN_SetStepPic.CustomizableEdges = customizableEdges5;
        BTN_SetStepPic.DisabledState.BorderColor = Color.DarkGray;
        BTN_SetStepPic.DisabledState.CustomBorderColor = Color.DarkGray;
        BTN_SetStepPic.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
        BTN_SetStepPic.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
        BTN_SetStepPic.FillColor = Color.FromArgb(240, 112, 22);
        BTN_SetStepPic.Font = new Font("Montserrat Medium", 12F, FontStyle.Bold);
        BTN_SetStepPic.ForeColor = Color.White;
        BTN_SetStepPic.Location = new Point(469, 296);
        BTN_SetStepPic.Name = "BTN_SetStepPic";
        BTN_SetStepPic.ShadowDecoration.CustomizableEdges = customizableEdges6;
        BTN_SetStepPic.Size = new Size(370, 45);
        BTN_SetStepPic.TabIndex = 48;
        BTN_SetStepPic.Text = "Завантажити з пристрою";
        BTN_SetStepPic.Click += BTN_SetStepPic_Click;
        // 
        // BTN_DeleteStepPic
        // 
        BTN_DeleteStepPic.BackColor = Color.FromArgb(255, 230, 210);
        BTN_DeleteStepPic.BorderRadius = 5;
        BTN_DeleteStepPic.CustomizableEdges = customizableEdges7;
        BTN_DeleteStepPic.DisabledState.BorderColor = Color.DarkGray;
        BTN_DeleteStepPic.DisabledState.CustomBorderColor = Color.DarkGray;
        BTN_DeleteStepPic.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
        BTN_DeleteStepPic.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
        BTN_DeleteStepPic.FillColor = Color.FromArgb(240, 112, 22);
        BTN_DeleteStepPic.Font = new Font("Montserrat Medium", 12F, FontStyle.Bold);
        BTN_DeleteStepPic.ForeColor = Color.White;
        BTN_DeleteStepPic.Location = new Point(392, 296);
        BTN_DeleteStepPic.Name = "BTN_DeleteStepPic";
        BTN_DeleteStepPic.ShadowDecoration.CustomizableEdges = customizableEdges8;
        BTN_DeleteStepPic.Size = new Size(59, 45);
        BTN_DeleteStepPic.TabIndex = 47;
        BTN_DeleteStepPic.Text = "🗑️";
        BTN_DeleteStepPic.Click += BTN_DeleteStepPic_Click;
        // 
        // StepPicBox
        // 
        StepPicBox.BackColor = Color.FromArgb(255, 230, 210);
        StepPicBox.BackgroundImageLayout = ImageLayout.None;
        StepPicBox.BorderRadius = 15;
        StepPicBox.CustomizableEdges = customizableEdges9;
        StepPicBox.ImageRotate = 0F;
        StepPicBox.Location = new Point(392, 13);
        StepPicBox.Name = "StepPicBox";
        StepPicBox.ShadowDecoration.CustomizableEdges = customizableEdges10;
        StepPicBox.Size = new Size(447, 266);
        StepPicBox.SizeMode = PictureBoxSizeMode.Zoom;
        StepPicBox.TabIndex = 31;
        StepPicBox.TabStop = false;
        // 
        // StepControl
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.White;
        Controls.Add(guna2Panel1);
        Name = "StepControl";
        Size = new Size(853, 355);
        guna2Panel1.ResumeLayout(false);
        guna2Panel1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)StepPicBox).EndInit();
        ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2PictureBox StepPicBox;
        private Guna.UI2.WinForms.Guna2Button BTN_SetStepPic;
        private Guna.UI2.WinForms.Guna2Button BTN_DeleteStepPic;
        private Guna.UI2.WinForms.Guna2Button BTN_DeleteStep;
        private Guna.UI2.WinForms.Guna2TextBox txtbox_StepDescription;
        private Label label_StepNumber;
    }
}
