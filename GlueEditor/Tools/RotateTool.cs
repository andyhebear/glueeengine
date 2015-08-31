using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GlueEditor.ViewportControllers;
using GlueEngine.World;
using GlueEditor.Core;
using Mogre;

namespace GlueEditor.Tools
{
    public partial class RotateTool : UserControl, ITool
    {
        public RotateTool()
        {
            InitializeComponent();
        }

        public new void MouseDown(IViewportController viewportController, MouseEventArgs e)
        {
        }

        public new void MouseUp(IViewportController viewportController, MouseEventArgs e)
        {
        }

        public new void MouseMove(IViewportController viewportController, int dx, int dy, MouseEventArgs e)
        {
        }

        private void RotateAllNodes(Vector3 axis, float amount)
        {
            foreach (EditNode editNode in Editor.SelectedNodes)
                editNode.Rotate(axis, new Radian(new Degree(amount)));
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            float amount = (float)numericUpDown1.Value;

            RotateAllNodes(Vector3.UNIT_Y, amount);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            float amount = (float)numericUpDown1.Value;

            RotateAllNodes(Vector3.UNIT_Y, -amount);
        }
    }
}
