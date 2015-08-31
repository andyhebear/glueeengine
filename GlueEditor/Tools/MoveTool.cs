using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GlueEditor.ViewportControllers;
using Mogre;
using GlueEngine.World;
using GlueEditor.WorldGeometry;
using GlueEditor.Core;

namespace GlueEditor.Tools
{
    public partial class MoveTool : UserControl, ITool
    {
        private Vector3 mouseDownPosition;

        public MoveTool()
        {
            InitializeComponent();

        }

        public new void MouseDown(IViewportController viewportController, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.mouseDownPosition = viewportController.MouseToGrid(e.X, e.Y);
        }

        public new void MouseUp(IViewportController viewportController, MouseEventArgs e)
        {
        }

        public new void MouseMove(IViewportController viewportController, int dx, int dy, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Vector3 mouseUpPosition = viewportController.MouseToGrid(e.X, e.Y);
                Vector3 movement = mouseUpPosition - mouseDownPosition;

                foreach (EditNode editNode in Editor.SelectedNodes)
                    editNode.Position += movement;

                this.mouseDownPosition = mouseUpPosition;
            }
        }
    }
}
