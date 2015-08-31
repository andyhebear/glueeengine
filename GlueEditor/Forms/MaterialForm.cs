using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GlueEditor.Core;
using WeifenLuo.WinFormsUI.Docking;
using GlueEngine.Core;

namespace GlueEditor.Forms
{
    public partial class MaterialForm : DockContent
    {
        public MaterialForm()
        {
            InitializeComponent();
        }

        private void MaterialForm_Load(object sender, EventArgs e)
        {
            cmbMaterial.Items.AddRange(Engine.Graphics.GetMaterialList("Stuff"));
        }

        private void cmbMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(cmbMaterial.Text != "")
            //    Editor.Material = cmbMaterial.Text;
        }
    }
}
