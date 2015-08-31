using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using WeifenLuo.WinFormsUI.Docking;
using Mogre;
using GlueEditor.WorldGeometry;
using GlueEditor.Tools;
using GlueEditor.Core;
using GlueEngine.Core;
using GlueEngine.World;

namespace GlueEditor.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadWindowLayout();

            this.Disposed += new EventHandler(MainForm_Disposed);
            
            try
            {
                Editor.Initialise();
            }            
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void LoadWindowLayout()
        {
            this.SuspendLayout();
            ViewportForm persView = new ViewportForm(ViewportType.Perspective);
            ViewportForm topView = new ViewportForm(ViewportType.Top);
            ViewportForm sideView = new ViewportForm(ViewportType.Side);
            ViewportForm frontView = new ViewportForm(ViewportType.Front);

            persView.Show(this.dockPanel, DockState.Document);
            sideView.Show(persView.Pane, DockAlignment.Bottom, 0.5);
            topView.Show(persView.Pane, DockAlignment.Right, 0.5);
            frontView.Show(sideView.Pane, DockAlignment.Right, 0.5);

            this.ResumeLayout();
        }

        void MainForm_Disposed(object sender, EventArgs e)
        {
            Editor.ShutDown();
        }

        private void renderTimer_Tick(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                float deltaTime = (float)renderTimer.Interval / 1000.0f;

                Editor.Update(deltaTime);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }
       
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();
            form.ShowDialog();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }

        private string AxisToString(Vector3 axis)
        {
            if (axis == Vector3.UNIT_X)
                return "X";

            if (axis == Vector3.UNIT_Y)
                return "Y";

            if (axis == Vector3.UNIT_Z)
                return "Z";

            return "";
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbGo_CheckedChanged(object sender, EventArgs e)
        {
            Editor.Playing = tsbPlay.Checked;
        }

        private void propertryGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PropertyForm form = new PropertyForm();
            form.Show(this.dockPanel, DockState.DockRight);
        }

        private void toolBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolBoxForm form = new ToolBoxForm();
            form.Show(this.dockPanel, DockState.DockRight);
        }

        private void perspectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewportForm form = new ViewportForm(ViewportType.Perspective);
            form.Show(this.dockPanel, DockState.Document);
        }

        private void tsbSnapToGrid_Click(object sender, EventArgs e)
        {
            Editor.SnapToGrid = tsbSnapToGrid.Checked;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
                Editor.MultiSelect = true;
        }

        private void MainForm_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Modifiers == Keys.None)
                Editor.MultiSelect = false;
        }

        private void tsbGridPlus_Click(object sender, EventArgs e)
        {
            labelGridSize.Text = Editor.GridPlus().ToString("0.00");
        }

        private void tsbGridMinus_Click(object sender, EventArgs e)
        {
            labelGridSize.Text = Editor.GridMinus().ToString("0.00");
        }

        private void materialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MaterialForm form = new MaterialForm();
            form.Show(this.dockPanel, DockState.DockRight);
        }

        private void SaveAs()
        {
            if (this.saveFileDialog.ShowDialog(this) == DialogResult.OK)
                Editor.Save(this.saveFileDialog.FileName);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Editor.Save())
                SaveAs();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if(!Editor.Save())
                SaveAs();
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void Open()
        {
            if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
                Editor.Load(this.openFileDialog.FileName);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New();
        }

        private void New()
        {
            Editor.New();
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            New();
        }
    }
}
