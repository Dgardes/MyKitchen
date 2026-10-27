namespace MyKitchen_UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
        if(disposing && (components != null))
        {
        components.Dispose();
        }
        base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        components = new System.ComponentModel.Container();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        BorderlessForm = new Guna.UI2.WinForms.Guna2BorderlessForm(components);
        BorderPanel = new Guna.UI2.WinForms.Guna2Panel();
        Lb_AppName = new Label();
        PicBox_Icon = new Guna.UI2.WinForms.Guna2PictureBox();
        ControlBox_Minimize = new Guna.UI2.WinForms.Guna2ControlBox();
        ControlBox_Maximize = new Guna.UI2.WinForms.Guna2ControlBox();
        ControlBox_Close = new Guna.UI2.WinForms.Guna2ControlBox();
        TLP_ContentHolder = new TableLayoutPanel();
        TLP_Navigation = new TableLayoutPanel();
        IRB_Instruments = new Guna.UI2.WinForms.Guna2ImageRadioButton();
        IRB_Profile = new Guna.UI2.WinForms.Guna2ImageRadioButton();
        IRB_GlobalRecipes = new Guna.UI2.WinForms.Guna2ImageRadioButton();
        IRB_LocalRecipes = new Guna.UI2.WinForms.Guna2ImageRadioButton();
        IRB_CookBooks = new Guna.UI2.WinForms.Guna2ImageRadioButton();
        IRB_Edit = new Guna.UI2.WinForms.Guna2ImageRadioButton();
        BorderDragControl = new Guna.UI2.WinForms.Guna2DragControl(components);
        BorderPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)PicBox_Icon).BeginInit();
        TLP_ContentHolder.SuspendLayout();
        TLP_Navigation.SuspendLayout();
        SuspendLayout();
        // 
        // BorderlessForm
        // 
        BorderlessForm.ContainerControl = this;
        BorderlessForm.DockIndicatorTransparencyValue = 0.6D;
        BorderlessForm.TransparentWhileDrag = true;
        // 
        // BorderPanel
        // 
        BorderPanel.Controls.Add(Lb_AppName);
        BorderPanel.Controls.Add(PicBox_Icon);
        BorderPanel.Controls.Add(ControlBox_Minimize);
        BorderPanel.Controls.Add(ControlBox_Maximize);
        BorderPanel.Controls.Add(ControlBox_Close);
        BorderPanel.CustomizableEdges = customizableEdges15;
        BorderPanel.Dock = DockStyle.Top;
        BorderPanel.FillColor = Color.FromArgb(240, 112, 22);
        BorderPanel.Location = new Point(0, 0);
        BorderPanel.Name = "BorderPanel";
        BorderPanel.ShadowDecoration.CustomizableEdges = customizableEdges16;
        BorderPanel.Size = new Size(1250, 30);
        BorderPanel.TabIndex = 0;
        // 
        // Lb_AppName
        // 
        Lb_AppName.AutoSize = true;
        Lb_AppName.BackColor = Color.FromArgb(240, 112, 22);
        Lb_AppName.Dock = DockStyle.Left;
        Lb_AppName.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
        Lb_AppName.ForeColor = Color.White;
        Lb_AppName.Location = new Point(34, 0);
        Lb_AppName.MinimumSize = new Size(0, 30);
        Lb_AppName.Name = "Lb_AppName";
        Lb_AppName.Size = new Size(101, 30);
        Lb_AppName.TabIndex = 4;
        Lb_AppName.Text = "MyKitchen";
        // 
        // PicBox_Icon
        // 
        PicBox_Icon.BackColor = Color.Transparent;
        PicBox_Icon.BackgroundImageLayout = ImageLayout.Zoom;
        PicBox_Icon.CustomizableEdges = customizableEdges7;
        PicBox_Icon.Dock = DockStyle.Left;
        PicBox_Icon.Image = Properties.Resources.My_Kitchen_im;
        PicBox_Icon.ImageRotate = 0F;
        PicBox_Icon.Location = new Point(0, 0);
        PicBox_Icon.Name = "PicBox_Icon";
        PicBox_Icon.ShadowDecoration.CustomizableEdges = customizableEdges8;
        PicBox_Icon.Size = new Size(34, 30);
        PicBox_Icon.TabIndex = 3;
        PicBox_Icon.TabStop = false;
        PicBox_Icon.UseTransparentBackground = true;
        // 
        // ControlBox_Minimize
        // 
        ControlBox_Minimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
        ControlBox_Minimize.CustomizableEdges = customizableEdges9;
        ControlBox_Minimize.Dock = DockStyle.Right;
        ControlBox_Minimize.FillColor = Color.FromArgb(240, 112, 22);
        ControlBox_Minimize.IconColor = Color.White;
        ControlBox_Minimize.Location = new Point(1115, 0);
        ControlBox_Minimize.Name = "ControlBox_Minimize";
        ControlBox_Minimize.ShadowDecoration.CustomizableEdges = customizableEdges10;
        ControlBox_Minimize.Size = new Size(45, 30);
        ControlBox_Minimize.TabIndex = 2;
        // 
        // ControlBox_Maximize
        // 
        ControlBox_Maximize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MaximizeBox;
        ControlBox_Maximize.CustomizableEdges = customizableEdges11;
        ControlBox_Maximize.Dock = DockStyle.Right;
        ControlBox_Maximize.FillColor = Color.FromArgb(240, 112, 22);
        ControlBox_Maximize.IconColor = Color.White;
        ControlBox_Maximize.Location = new Point(1160, 0);
        ControlBox_Maximize.Name = "ControlBox_Maximize";
        ControlBox_Maximize.ShadowDecoration.CustomizableEdges = customizableEdges12;
        ControlBox_Maximize.Size = new Size(45, 30);
        ControlBox_Maximize.TabIndex = 1;
        // 
        // ControlBox_Close
        // 
        ControlBox_Close.CustomizableEdges = customizableEdges13;
        ControlBox_Close.Dock = DockStyle.Right;
        ControlBox_Close.FillColor = Color.FromArgb(240, 112, 22);
        ControlBox_Close.IconColor = Color.White;
        ControlBox_Close.Location = new Point(1205, 0);
        ControlBox_Close.Name = "ControlBox_Close";
        ControlBox_Close.ShadowDecoration.CustomizableEdges = customizableEdges14;
        ControlBox_Close.Size = new Size(45, 30);
        ControlBox_Close.TabIndex = 0;
        // 
        // TLP_ContentHolder
        // 
        TLP_ContentHolder.BackColor = Color.White;
        TLP_ContentHolder.ColumnCount = 3;
        TLP_ContentHolder.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        TLP_ContentHolder.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 1250F));
        TLP_ContentHolder.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        TLP_ContentHolder.Controls.Add(TLP_Navigation, 1, 1);
        TLP_ContentHolder.Dock = DockStyle.Fill;
        TLP_ContentHolder.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        TLP_ContentHolder.Location = new Point(0, 30);
        TLP_ContentHolder.Margin = new Padding(0);
        TLP_ContentHolder.Name = "TLP_ContentHolder";
        TLP_ContentHolder.Padding = new Padding(2);
        TLP_ContentHolder.RowCount = 2;
        TLP_ContentHolder.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        TLP_ContentHolder.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
        TLP_ContentHolder.Size = new Size(1250, 520);
        TLP_ContentHolder.TabIndex = 1;
        // 
        // TLP_Navigation
        // 
        TLP_Navigation.AutoSize = true;
        TLP_Navigation.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        TLP_Navigation.BackColor = Color.FromArgb(240, 112, 22);
        TLP_Navigation.ColumnCount = 6;
        TLP_Navigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
        TLP_Navigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
        TLP_Navigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
        TLP_Navigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
        TLP_Navigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
        TLP_Navigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
        TLP_Navigation.Controls.Add(IRB_Instruments, 5, 0);
        TLP_Navigation.Controls.Add(IRB_Profile, 0, 0);
        TLP_Navigation.Controls.Add(IRB_GlobalRecipes, 1, 0);
        TLP_Navigation.Controls.Add(IRB_LocalRecipes, 2, 0);
        TLP_Navigation.Controls.Add(IRB_CookBooks, 3, 0);
        TLP_Navigation.Controls.Add(IRB_Edit, 4, 0);
        TLP_Navigation.Dock = DockStyle.Fill;
        TLP_Navigation.Location = new Point(0, 418);
        TLP_Navigation.Margin = new Padding(0);
        TLP_Navigation.MaximumSize = new Size(1250, 75);
        TLP_Navigation.MinimumSize = new Size(700, 75);
        TLP_Navigation.Name = "TLP_Navigation";
        TLP_Navigation.RowCount = 1;
        TLP_Navigation.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        TLP_Navigation.Size = new Size(1250, 75);
        TLP_Navigation.TabIndex = 3;
        // 
        // IRB_Instruments
        // 
        IRB_Instruments.CheckedState.Image = (Image)resources.GetObject("resource.Image");
        IRB_Instruments.CheckedState.ImageSize = new Size(40, 40);
        IRB_Instruments.Dock = DockStyle.Fill;
        IRB_Instruments.HoverState.ImageSize = new Size(60, 60);
        IRB_Instruments.Image = Properties.Resources.instruments_white;
        IRB_Instruments.ImageOffset = new Point(0, 0);
        IRB_Instruments.ImageRotate = 0F;
        IRB_Instruments.ImageSize = new Size(40, 40);
        IRB_Instruments.Location = new Point(1043, 3);
        IRB_Instruments.Name = "IRB_Instruments";
        IRB_Instruments.ShadowDecoration.CustomizableEdges = customizableEdges1;
        IRB_Instruments.Size = new Size(204, 69);
        IRB_Instruments.TabIndex = 10;
        IRB_Instruments.CheckedChanged += IRB_CheckedChanged;
        // 
        // IRB_Profile
        // 
        IRB_Profile.CheckedState.Image = (Image)resources.GetObject("resource.Image1");
        IRB_Profile.CheckedState.ImageSize = new Size(40, 40);
        IRB_Profile.Dock = DockStyle.Fill;
        IRB_Profile.HoverState.ImageSize = new Size(60, 60);
        IRB_Profile.Image = Properties.Resources.profile_white;
        IRB_Profile.ImageOffset = new Point(0, 0);
        IRB_Profile.ImageRotate = 0F;
        IRB_Profile.ImageSize = new Size(40, 40);
        IRB_Profile.Location = new Point(3, 3);
        IRB_Profile.Name = "IRB_Profile";
        IRB_Profile.ShadowDecoration.CustomizableEdges = customizableEdges2;
        IRB_Profile.Size = new Size(202, 69);
        IRB_Profile.TabIndex = 5;
        IRB_Profile.CheckedChanged += IRB_CheckedChanged;
        // 
        // IRB_GlobalRecipes
        // 
        IRB_GlobalRecipes.CheckedState.Image = (Image)resources.GetObject("resource.Image2");
        IRB_GlobalRecipes.CheckedState.ImageSize = new Size(40, 40);
        IRB_GlobalRecipes.Dock = DockStyle.Fill;
        IRB_GlobalRecipes.HoverState.ImageSize = new Size(60, 60);
        IRB_GlobalRecipes.Image = Properties.Resources.web_white;
        IRB_GlobalRecipes.ImageOffset = new Point(0, 0);
        IRB_GlobalRecipes.ImageRotate = 0F;
        IRB_GlobalRecipes.ImageSize = new Size(40, 40);
        IRB_GlobalRecipes.Location = new Point(211, 3);
        IRB_GlobalRecipes.Name = "IRB_GlobalRecipes";
        IRB_GlobalRecipes.ShadowDecoration.CustomizableEdges = customizableEdges3;
        IRB_GlobalRecipes.Size = new Size(202, 69);
        IRB_GlobalRecipes.TabIndex = 6;
        IRB_GlobalRecipes.CheckedChanged += IRB_CheckedChanged;
        // 
        // IRB_LocalRecipes
        // 
        IRB_LocalRecipes.CheckedState.Image = (Image)resources.GetObject("resource.Image3");
        IRB_LocalRecipes.CheckedState.ImageSize = new Size(40, 40);
        IRB_LocalRecipes.Dock = DockStyle.Fill;
        IRB_LocalRecipes.HoverState.ImageSize = new Size(60, 60);
        IRB_LocalRecipes.Image = Properties.Resources.localRecipes_white;
        IRB_LocalRecipes.ImageOffset = new Point(0, 0);
        IRB_LocalRecipes.ImageRotate = 0F;
        IRB_LocalRecipes.ImageSize = new Size(40, 40);
        IRB_LocalRecipes.Location = new Point(419, 3);
        IRB_LocalRecipes.Name = "IRB_LocalRecipes";
        IRB_LocalRecipes.ShadowDecoration.CustomizableEdges = customizableEdges4;
        IRB_LocalRecipes.Size = new Size(202, 69);
        IRB_LocalRecipes.TabIndex = 7;
        IRB_LocalRecipes.CheckedChanged += IRB_CheckedChanged;
        // 
        // IRB_CookBooks
        // 
        IRB_CookBooks.CheckedState.Image = (Image)resources.GetObject("resource.Image4");
        IRB_CookBooks.CheckedState.ImageSize = new Size(40, 40);
        IRB_CookBooks.Dock = DockStyle.Fill;
        IRB_CookBooks.HoverState.ImageSize = new Size(60, 60);
        IRB_CookBooks.Image = Properties.Resources.cookBooks_white;
        IRB_CookBooks.ImageOffset = new Point(0, 0);
        IRB_CookBooks.ImageRotate = 0F;
        IRB_CookBooks.ImageSize = new Size(40, 40);
        IRB_CookBooks.Location = new Point(627, 3);
        IRB_CookBooks.Name = "IRB_CookBooks";
        IRB_CookBooks.ShadowDecoration.CustomizableEdges = customizableEdges5;
        IRB_CookBooks.Size = new Size(202, 69);
        IRB_CookBooks.TabIndex = 8;
        IRB_CookBooks.CheckedChanged += IRB_CheckedChanged;
        // 
        // IRB_Edit
        // 
        IRB_Edit.CheckedState.Image = (Image)resources.GetObject("resource.Image5");
        IRB_Edit.CheckedState.ImageSize = new Size(40, 40);
        IRB_Edit.Dock = DockStyle.Fill;
        IRB_Edit.HoverState.ImageSize = new Size(60, 60);
        IRB_Edit.Image = Properties.Resources.edit_white;
        IRB_Edit.ImageOffset = new Point(0, 0);
        IRB_Edit.ImageRotate = 0F;
        IRB_Edit.ImageSize = new Size(40, 40);
        IRB_Edit.Location = new Point(835, 3);
        IRB_Edit.Name = "IRB_Edit";
        IRB_Edit.ShadowDecoration.CustomizableEdges = customizableEdges6;
        IRB_Edit.Size = new Size(202, 69);
        IRB_Edit.TabIndex = 9;
        IRB_Edit.CheckedChanged += IRB_CheckedChanged;
        // 
        // BorderDragControl
        // 
        BorderDragControl.DockIndicatorTransparencyValue = 0.6D;
        BorderDragControl.TargetControl = BorderPanel;
        BorderDragControl.UseTransparentDrag = true;
        // 
        // MainForm
        // 
        AutoScaleMode = AutoScaleMode.None;
        ClientSize = new Size(1250, 550);
        Controls.Add(TLP_ContentHolder);
        Controls.Add(BorderPanel);
        DoubleBuffered = true;
        FormBorderStyle = FormBorderStyle.None;
        MinimumSize = new Size(1250, 550);
        Name = "MainForm";
        Text = "MainForm";
        SizeChanged += MainForm_SizeChanged;
        BorderPanel.ResumeLayout(false);
        BorderPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)PicBox_Icon).EndInit();
        TLP_ContentHolder.ResumeLayout(false);
        TLP_ContentHolder.PerformLayout();
        TLP_Navigation.ResumeLayout(false);
        ResumeLayout(false);
        }

        #endregion
        private Guna.UI2.WinForms.Guna2BorderlessForm BorderlessForm;
        private Guna.UI2.WinForms.Guna2Panel BorderPanel;
        private Label Lb_AppName;
        private Guna.UI2.WinForms.Guna2PictureBox PicBox_Icon;
        private Guna.UI2.WinForms.Guna2ControlBox ControlBox_Minimize;
        private Guna.UI2.WinForms.Guna2ControlBox ControlBox_Maximize;
        private Guna.UI2.WinForms.Guna2ControlBox ControlBox_Close;
        private TableLayoutPanel TLP_ContentHolder;
        private Guna.UI2.WinForms.Guna2DragControl BorderDragControl;
        private TableLayoutPanel TLP_Navigation;
        private Guna.UI2.WinForms.Guna2ImageRadioButton IRB_Profile;
        private Guna.UI2.WinForms.Guna2ImageRadioButton IRB_GlobalRecipes;
        private Guna.UI2.WinForms.Guna2ImageRadioButton IRB_LocalRecipes;
        private Guna.UI2.WinForms.Guna2ImageRadioButton IRB_CookBooks;
        private Guna.UI2.WinForms.Guna2ImageRadioButton IRB_Edit;
        private Guna.UI2.WinForms.Guna2ImageRadioButton IRB_Instruments;
    }
}