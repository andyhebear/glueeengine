namespace GlueEditor.Forms
{
    partial class ViewportForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.viewportPanel = new GlueEditor.ViewportControllers.ViewportPanel();
            this.SuspendLayout();
            // 
            // viewportPanel
            // 
            this.viewportPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.viewportPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewportPanel.Location = new System.Drawing.Point(0, 0);
            this.viewportPanel.Name = "viewportPanel";
            this.viewportPanel.Size = new System.Drawing.Size(499, 400);
            this.viewportPanel.TabIndex = 0;
            this.viewportPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.viewportPanel_MouseMove);
            this.viewportPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.viewportPanel_MouseDown);
            this.viewportPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.viewportPanel_MouseUp);
            // 
            // ViewportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 400);
            this.Controls.Add(this.viewportPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ViewportForm";
            this.Text = "ViewportForm";
            this.ResumeLayout(false);

        }

        #endregion

        protected GlueEditor.ViewportControllers.ViewportPanel viewportPanel;
    }
}