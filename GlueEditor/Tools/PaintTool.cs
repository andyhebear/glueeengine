using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GlueEditor.ViewportControllers;
using Mogre;
using GlueEngine.Core;
using GlueEditor.Core;
using GlueEditor.WorldGeometry;
using GlueEngine.World;
using System.Reflection;

namespace GlueEditor.Tools
{
    public partial class PaintTool : UserControl, ITool
    {
        public PaintTool()
        {
            InitializeComponent();
        }

        public new void MouseDown(IViewportController viewportController, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Block hitBlock = Editor.SelectBlock(viewportController, e.X, e.Y);

                if (hitBlock != null)
                    hitBlock.MaterialName = this.treeMaterial.SelectedNode.Text;
            }
        }

        public new void MouseUp(IViewportController viewportController, MouseEventArgs e)
        {
        }

        public new void MouseMove(IViewportController viewportController, int dx, int dy, MouseEventArgs e)
        {
        }

        private void PaintTool_Load(object sender, EventArgs e)
        {
            this.imageList.Images.Add(new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GlueEditor.Media.folder_palette.png")));
            this.imageList.Images.Add(new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GlueEditor.Media.palette.png")));

            foreach(string group in Engine.Graphics.GetMaterialGroups())
            {
                TreeNode groupNode = new TreeNode(group);
                groupNode.ImageIndex = 0;
                groupNode.SelectedImageIndex = 0;

                foreach (string materialName in Engine.Graphics.GetMaterialList(group))
                {
                    TreeNode materialNode = new TreeNode(materialName);
                    materialNode.ImageIndex = 1;
                    materialNode.SelectedImageIndex = 1;
                    groupNode.Nodes.Add(materialNode);
                }

                this.treeMaterial.Nodes.Add(groupNode);
            }
        }
    }
}
