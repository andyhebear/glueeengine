namespace GlueEditor.Tools
{
    partial class SpawnTool
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstEntityTypes = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.NewEntityTypeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.staticEntityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dynamicEntityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstEntityTypes
            // 
            this.lstEntityTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstEntityTypes.FormattingEnabled = true;
            this.lstEntityTypes.Location = new System.Drawing.Point(0, 24);
            this.lstEntityTypes.Name = "lstEntityTypes";
            this.lstEntityTypes.Size = new System.Drawing.Size(150, 251);
            this.lstEntityTypes.TabIndex = 0;
            this.lstEntityTypes.SelectedIndexChanged += new System.EventHandler(this.lstEntity_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewEntityTypeMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(150, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // NewEntityTypeMenuItem
            // 
            this.NewEntityTypeMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.staticEntityToolStripMenuItem,
            this.dynamicEntityToolStripMenuItem});
            this.NewEntityTypeMenuItem.Name = "NewEntityTypeMenuItem";
            this.NewEntityTypeMenuItem.Size = new System.Drawing.Size(40, 20);
            this.NewEntityTypeMenuItem.Text = "New";
            // 
            // staticEntityToolStripMenuItem
            // 
            this.staticEntityToolStripMenuItem.Name = "staticEntityToolStripMenuItem";
            this.staticEntityToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.staticEntityToolStripMenuItem.Text = "Static Entity";
            this.staticEntityToolStripMenuItem.Click += new System.EventHandler(this.staticEntityToolStripMenuItem_Click);
            // 
            // dynamicEntityToolStripMenuItem
            // 
            this.dynamicEntityToolStripMenuItem.Name = "dynamicEntityToolStripMenuItem";
            this.dynamicEntityToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.dynamicEntityToolStripMenuItem.Text = "Dynamic Entity";
            this.dynamicEntityToolStripMenuItem.Click += new System.EventHandler(this.dynamicEntityToolStripMenuItem_Click);
            // 
            // SpawnTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstEntityTypes);
            this.Controls.Add(this.menuStrip1);
            this.Name = "SpawnTool";
            this.Size = new System.Drawing.Size(150, 275);
            this.Load += new System.EventHandler(this.SpawnTool_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstEntityTypes;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem NewEntityTypeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem staticEntityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dynamicEntityToolStripMenuItem;
    }
}
