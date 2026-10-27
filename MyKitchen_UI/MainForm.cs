using Guna.UI2.WinForms;
using MyKitchen_Core.Entities;
using MyKitchen_UI.Controls;
using MyKitchen_UI.Interfaces;

namespace MyKitchen_UI
{
    public partial class MainForm :Form
    {
        public static MainForm mainForm { get; private set; } = null!;
        public event EventHandler formResizing = null!;

        private struct tabState
        { 
            public UserControl page;
            public Image ActiveImg;
            public Image InactiveImg;
        }

        private Dictionary<Guna2ImageRadioButton, tabState> navigation;

        public MainForm()
        {

            mainForm = this;
            InitializeComponent();


            Guna2ResizeBox ResizeBox = new Guna2ResizeBox();
            mainForm.ControlAdded += (s, e) => ResizeBox.BringToFront();
            ResizeBox.Height = 15;
            ResizeBox.Width = 15;

            ResizeBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ResizeBox.Location = new Point(mainForm.ClientSize.Width - ResizeBox.Width, mainForm.ClientSize.Height - ResizeBox.Height);

            ResizeBox.TargetControl = mainForm;
            mainForm.Controls.Add(ResizeBox);
            initializeNav();
        }

        private void initializeNav()
        {
        navigation = new Dictionary<Guna.UI2.WinForms.Guna2ImageRadioButton, tabState>()
            {
                {
                    IRB_Profile, new tabState
                    {
                        page = new ProfileControl(),
                        ActiveImg = Properties.Resources.profile_orange,
                        InactiveImg = Properties.Resources.profile_white
                    }
                },
                {
                    IRB_LocalRecipes, new tabState
                    {
                        page = new MyRecipesControl(),
                        ActiveImg = Properties.Resources.localRecipes_orange,
                        InactiveImg = Properties.Resources.localRecipes_white
                    }
                },
                {
                    IRB_GlobalRecipes, new tabState
                    {
                        page = new GlobalControl(),
                        ActiveImg = Properties.Resources.web_orange,
                        InactiveImg = Properties.Resources.web_white
                    }
                },
                {
                    IRB_Edit, new tabState
                    {
                        page = new EditControl(),
                        ActiveImg = Properties.Resources.edit_orange,
                        InactiveImg = Properties.Resources.edit_white
                    }
                },
                {
                    IRB_CookBooks, new tabState
                    {
                        page = new CookBooksControl(),
                        ActiveImg = Properties.Resources.cookBooks_orange,
                        InactiveImg = Properties.Resources.cookBooks_white
                    }
                },
                {
                    IRB_Instruments, new tabState
                    {
                        page = new InstrumentsControl(), 
                        ActiveImg = Properties.Resources.instruments_orange,
                        InactiveImg = Properties.Resources.instruments_white
                    }
                }
            };

        foreach(var state in navigation)
        {
        state.Value.page.Dock = DockStyle.Fill;
        state.Value.page.Visible = false;
        state.Value.page.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        TLP_ContentHolder.Controls.Add(state.Value.page, 1, 0);
        }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            formResizing?.Invoke(this, EventArgs.Empty);
        }


        private void IRB_CheckedChanged(object sender, EventArgs e)
        {
            if(sender is Guna2ImageRadioButton bt && bt.Checked)
            {
                foreach(var state in navigation)
                {
                    Guna2ImageRadioButton button = state.Key;
                    tabState tabState = state.Value;
                    bool isSelected = button == bt;

                    if(isSelected)
                    { 
                        button.CheckedState.Image = tabState.ActiveImg;
                        if (tabState.page is IRefreshableTab refreshableTab)
                        {
                            refreshableTab.OnTabActivated();
                        }
                    } 
                    else
                    { 
                        button.Image = tabState.InactiveImg;
                    }
                    button.BackColor = isSelected ? Color.White : Color.FromArgb(240, 112, 22);
                    
                    tabState.page.Visible = isSelected;
                }
            }
        }

        public void OpenRecipeInEditMode(Recipe recipe)
        {
            if (recipe == null) return;
            var editTab = navigation.Values.FirstOrDefault(state => state.page is EditControl);

            if (editTab.page is EditControl editControl)
            {

                bool canProceed = editControl.SetupForEdit(recipe);

                if (canProceed)
                {
                    IRB_Edit.Checked = true;
                    editControl.P_ContentPanel.Visible = true;
                }
                else
                {
                    MessageBox.Show("Перехід скасовано. Поточні незбережені зміни захищено.", 
                        "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public void OpenRecipeInViewMode(Recipe recipe)
        {
            if (recipe == null) return;
            var viewTabState = navigation.FirstOrDefault(state => state.Value.page is MyRecipesControl && state.Key == IRB_LocalRecipes);

            if (viewTabState.Value.page is MyRecipesControl viewControl)
            {
                IRB_LocalRecipes.Checked = true;
                viewControl.ViewRecipeExternal(recipe);
            }
        }


    }
}
