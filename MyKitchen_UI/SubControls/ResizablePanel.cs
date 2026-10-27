using Guna.UI2.WinForms;
using MyKitchen_UI.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel; //бібліотека для візуалізації властивостей в дизайнері
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyKitchen_UI
{
    public partial class ResizablePanel : Guna2Panel
    {
        private IResizableContainer resContainer;
        public bool isClosed { get; private set; }

        //створємо нову категорію та ставимо видимість поля 
        [Category("Resizable panel property")]
        [Browsable(true)]
        public int heightWhenClosed { get; set; }
        
        [Category("Resizable panel property")]
        [Browsable(true)]
        public int bottomOffsetFromContainer { get; set; }

        [Category("Resizable panel property")]
        [Browsable(true)]
        public Control resizableContainer { get; set; }


        public ResizablePanel()
        {

        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if(this.DesignMode) { return; }

            if(resizableContainer != null)
            { 
                resContainer = resizableContainer as IResizableContainer;

                resContainer.controlResizing += main_controlResizing;
                resContainer.controlLoaded += onControlLoaded;

                updatePanelSize(true);
            }
        }

        public void updatePanelSize(bool close)
        {
            if(close)
            { 
                this.Height = heightWhenClosed; 
                isClosed = true;
            }
            else
            { 
                if (resContainer == null) return;
                this.Height = resContainer.containerSize.Height - bottomOffsetFromContainer;
                isClosed = false;
            }    
        }

        public void updatePanelSizeOnClick()
        { 
            isClosed = !isClosed;
            if(isClosed)
            { updatePanelSize(true); }
            else
            { updatePanelSize(false); }
        }

        private void main_controlResizing(object sender, EventArgs e)
        { 
            if(!isClosed)
            { updatePanelSize(false); }
        }

        private void onControlLoaded(object sender, EventArgs e)
        { 
            updatePanelSize(true);
        }
    }
}
