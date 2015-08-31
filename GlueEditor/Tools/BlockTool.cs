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
using GlueEngine.Core;

namespace GlueEditor.Tools
{
    public partial class BlockTool : UserControl, ITool
    {
        public BlockTool()
        {
            InitializeComponent();
        }

        public new void MouseDown(IViewportController viewportController, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Editor.SelectionBox.MouseDown(viewportController, e);
        }

        public new void MouseUp(IViewportController viewportController, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Editor.SelectionBox.MouseUp(viewportController, e);
        }

        public new void MouseMove(IViewportController viewportController, int dx, int dy, MouseEventArgs e)
        {
            Editor.SelectionBox.MouseMove(viewportController, e);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (Editor.SelectionBox != null)
            {
                AxisAlignedBox box = Editor.SelectionBox.Box;

                if (box != null)
                {
                    Block block = new Block(box);
                    block.Create();
                    Editor.Blocks.Add(block);
                }
            }
        }
    }
}
