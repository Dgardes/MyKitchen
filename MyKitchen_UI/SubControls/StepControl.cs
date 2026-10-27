using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MyKitchen_UI.Properties;

namespace MyKitchen_UI
{
    public partial class StepControl : UserControl
    {
        public string PhotoPath { get; set; } = string.Empty;
        
        public string StepNumberText
        {
            get { return label_StepNumber.Text; }
            set { label_StepNumber.Text = value; }
        }
        
        public string StepDescription
        {
            get { return txtbox_StepDescription.Text; }
            set { txtbox_StepDescription.Text = value; }
        }

        public event EventHandler DeleteClicked;

        public StepControl()
        {
            InitializeComponent();
            SetDefaultImage(); 
        }

        public void SetStepPhoto(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                SetDefaultImage();
                return;
            }

            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    Image oldImage = StepPicBox.Image;
                    StepPicBox.Image = Image.FromStream(ms);
                    oldImage?.Dispose(); 
                }
                PhotoPath = path; 
            }
            catch (Exception ex)
            {
                SetDefaultImage();
            }
        }

        private void SetDefaultImage()
        {
            Image oldImage = StepPicBox.Image;
            StepPicBox.Image = Resources.StockPhoto; 
            oldImage?.Dispose();
            PhotoPath = string.Empty; 
        }

        private void BTN_DeleteStep_Click(object sender, EventArgs e)
        {
            DeleteClicked?.Invoke(this, EventArgs.Empty);
        }

        private void BTN_SetStepPic_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    SetStepPhoto(ofd.FileName); 
                }
            }
        }

        private void BTN_DeleteStepPic_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PhotoPath))
            {
                SetDefaultImage();
            }
        }
    }
}