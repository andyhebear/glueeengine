using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using GlueEditor.ViewportControllers;
using GlueEditor.Core;
using Mogre;
using GlueEngine.Core;

namespace GlueEditor.Forms
{
    public partial class ViewportForm : DockContent
    {
        private ViewportType viewportType = ViewportType.Perspective;
        private IViewportController viewportController;

        public ViewportForm(ViewportType viewportType)
        {
            InitializeComponent();

            this.viewportType = viewportType;
        }
        
        protected override void OnLoad(EventArgs e)
        {
            switch (viewportType)
            {
                case ViewportType.Perspective:
                    this.viewportController = Editor.CreatePerspectiveViewport(this.viewportPanel);
                    break;
                case ViewportType.Front:
                    this.viewportController = Editor.CreateOrthographicViewport(this.viewportPanel, Vector3.UNIT_Z);
                    break;
                case ViewportType.Side:
                    this.viewportController = Editor.CreateOrthographicViewport(this.viewportPanel, Vector3.UNIT_X);                    
                    break;
                case ViewportType.Top:
                    this.viewportController = Editor.CreateOrthographicViewport(this.viewportPanel, Vector3.UNIT_Y);
                    break;
                default:
                    throw new Exception("Viewport type " + viewportType.ToString() + " not implemented");
            }

            base.OnLoad(e);
        }

        private void viewportPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!Editor.Widget.Grab(this.viewportController, e.X, e.Y))
                {
                    //if (!Editor.SelectionBox.MouseDown(viewportController, e.X, e.Y))
                    //    Editor.Select(this.viewportController, e);                        
                }
            }
        }

        private void viewportPanel_MouseMove(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    if (Editor.Widget.IsGrabbed)
            //        Editor.Widget.MoveTo(this.viewportController, e.X, e.Y);
            //    else
            //        Editor.SelectionBox.MouseMove(viewportController, e.X, e.Y);
            //}
            //else
            //{
            //    if (Editor.Widget.MouseOverAxis(this.viewportController, e.X, e.Y) != Vector3.ZERO)
            //        this.viewportController.Cursor = Cursors.Hand;
            //    //else
            //    //    this.viewportController.Cursor = Cursors.Default;

            //    Editor.SelectionBox.MouseMove(viewportController, e.X, e.Y);
            //}
        }

        private void viewportPanel_MouseUp(object sender, MouseEventArgs e)
        {
            //if(e.Button == MouseButtons.Left)
            //    Editor.SelectionBox.MouseUp(viewportController, e.X, e.Y);
        }
    }

    public enum ViewportType
    {
        Perspective, Top, Side, Front
    }
}
