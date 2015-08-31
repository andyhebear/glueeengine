using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using GlueEditor.Core;

namespace GlueEditor.Tools
{
    public partial class ToolBox : UserControl
    {
        public ToolBox()
        {
            InitializeComponent();
        }

        private void tool_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            Editor.Tool = radioButton.Tag as ITool;

            UserControl control = radioButton.Tag as UserControl;
            control.Dock = DockStyle.Fill;

            this.pnlToolOptions.Controls.Clear();
            this.pnlToolOptions.Controls.Add(control);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.rdoSelect.Tag = new SelectTool();
            this.rdoMove.Tag = new MoveTool();
            this.rdoRotate.Tag = new RotateTool();
            this.rdoSpawn.Tag = new SpawnTool();
            this.rdoBlock.Tag = new BlockTool();
            this.rdoPaint.Tag = new PaintTool();

            Editor.Tool = this.rdoSelect.Tag as ITool;
            
            base.OnLoad(e);
        }
    }
}
