using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MyKitchen_UI.Properties;

namespace MyKitchen_UI
{
    public partial class StepViewControl : UserControl
    {
        public string? PhotoPath { get; private set; }

        public StepViewControl(int stepNumber, string? description, string? photoPath)
        {
            InitializeComponent();
            
            PhotoPath = photoPath;
            label_StepNumber.Text = "Крок № " + stepNumber.ToString();
            label_Description.Text = !string.IsNullOrEmpty(description) ? description : "";
            LoadStepImage();
        }

        private void LoadStepImage()
        {
            if (!string.IsNullOrEmpty(PhotoPath) && File.Exists(PhotoPath))
            {
                try
                {
                    byte[] imageBytes = File.ReadAllBytes(PhotoPath);
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        StepPicBox.Image = Image.FromStream(ms);
                    }
                }
                catch
                {
                    StepPicBox.Image = Resources.StockPhoto;
                }
            }
            else
            {
                StepPicBox.Image = Resources.StockPhoto;
            }
        }
    }
}