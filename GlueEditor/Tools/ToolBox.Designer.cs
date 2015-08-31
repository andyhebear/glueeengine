namespace GlueEditor.Tools
{
    partial class ToolBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolBox));
            this.rdoMove = new System.Windows.Forms.RadioButton();
            this.rdoRotate = new System.Windows.Forms.RadioButton();
            this.rdoSelect = new System.Windows.Forms.RadioButton();
            this.pnlToolOptions = new System.Windows.Forms.Panel();
            this.rdoSpawn = new System.Windows.Forms.RadioButton();
            this.rdoBlock = new System.Windows.Forms.RadioButton();
            this.rdoPaint = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // rdoMove
            // 
            this.rdoMove.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoMove.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoMove.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.rdoMove.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.rdoMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoMove.Image = ((System.Drawing.Image)(resources.GetObject("rdoMove.Image")));
            this.rdoMove.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rdoMove.Location = new System.Drawing.Point(50, 0);
            this.rdoMove.Margin = new System.Windows.Forms.Padding(0);
            this.rdoMove.Name = "rdoMove";
            this.rdoMove.Size = new System.Drawing.Size(50, 50);
            this.rdoMove.TabIndex = 23;
            this.rdoMove.Text = "Move";
            this.rdoMove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoMove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rdoMove.UseVisualStyleBackColor = true;
            this.rdoMove.CheckedChanged += new System.EventHandler(this.tool_CheckedChanged);
            // 
            // rdoRotate
            // 
            this.rdoRotate.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoRotate.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoRotate.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.rdoRotate.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.rdoRotate.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoRotate.Image = ((System.Drawing.Image)(resources.GetObject("rdoRotate.Image")));
            this.rdoRotate.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rdoRotate.Location = new System.Drawing.Point(100, 0);
            this.rdoRotate.Margin = new System.Windows.Forms.Padding(0);
            this.rdoRotate.Name = "rdoRotate";
            this.rdoRotate.Size = new System.Drawing.Size(50, 50);
            this.rdoRotate.TabIndex = 24;
            this.rdoRotate.Text = "Rotate";
            this.rdoRotate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoRotate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rdoRotate.UseVisualStyleBackColor = true;
            this.rdoRotate.CheckedChanged += new System.EventHandler(this.tool_CheckedChanged);
            // 
            // rdoSelect
            // 
            this.rdoSelect.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoSelect.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoSelect.Checked = true;
            this.rdoSelect.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.rdoSelect.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.rdoSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoSelect.Image = ((System.Drawing.Image)(resources.GetObject("rdoSelect.Image")));
            this.rdoSelect.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rdoSelect.Location = new System.Drawing.Point(0, 0);
            this.rdoSelect.Margin = new System.Windows.Forms.Padding(0);
            this.rdoSelect.Name = "rdoSelect";
            this.rdoSelect.Size = new System.Drawing.Size(50, 50);
            this.rdoSelect.TabIndex = 22;
            this.rdoSelect.TabStop = true;
            this.rdoSelect.Text = "Select";
            this.rdoSelect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rdoSelect.UseVisualStyleBackColor = true;
            this.rdoSelect.CheckedChanged += new System.EventHandler(this.tool_CheckedChanged);
            // 
            // pnlToolOptions
            // 
            this.pnlToolOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlToolOptions.Location = new System.Drawing.Point(0, 103);
            this.pnlToolOptions.Name = "pnlToolOptions";
            this.pnlToolOptions.Size = new System.Drawing.Size(150, 435);
            this.pnlToolOptions.TabIndex = 25;
            // 
            // rdoSpawn
            // 
            this.rdoSpawn.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoSpawn.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoSpawn.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.rdoSpawn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.rdoSpawn.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoSpawn.Image = ((System.Drawing.Image)(resources.GetObject("rdoSpawn.Image")));
            this.rdoSpawn.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rdoSpawn.Location = new System.Drawing.Point(0, 50);
            this.rdoSpawn.Margin = new System.Windows.Forms.Padding(0);
            this.rdoSpawn.Name = "rdoSpawn";
            this.rdoSpawn.Size = new System.Drawing.Size(50, 50);
            this.rdoSpawn.TabIndex = 26;
            this.rdoSpawn.Text = "Spawn";
            this.rdoSpawn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoSpawn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rdoSpawn.UseVisualStyleBackColor = true;
            this.rdoSpawn.CheckedChanged += new System.EventHandler(this.tool_CheckedChanged);
            // 
            // rdoBlock
            // 
            this.rdoBlock.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoBlock.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoBlock.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.rdoBlock.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.rdoBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoBlock.Image = ((System.Drawing.Image)(resources.GetObject("rdoBlock.Image")));
            this.rdoBlock.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rdoBlock.Location = new System.Drawing.Point(50, 50);
            this.rdoBlock.Margin = new System.Windows.Forms.Padding(0);
            this.rdoBlock.Name = "rdoBlock";
            this.rdoBlock.Size = new System.Drawing.Size(50, 50);
            this.rdoBlock.TabIndex = 27;
            this.rdoBlock.Text = "Block";
            this.rdoBlock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoBlock.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rdoBlock.UseVisualStyleBackColor = true;
            this.rdoBlock.CheckedChanged += new System.EventHandler(this.tool_CheckedChanged);
            // 
            // rdoPaint
            // 
            this.rdoPaint.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoPaint.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoPaint.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.rdoPaint.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.rdoPaint.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoPaint.Image = ((System.Drawing.Image)(resources.GetObject("rdoPaint.Image")));
            this.rdoPaint.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rdoPaint.Location = new System.Drawing.Point(100, 50);
            this.rdoPaint.Margin = new System.Windows.Forms.Padding(0);
            this.rdoPaint.Name = "rdoPaint";
            this.rdoPaint.Size = new System.Drawing.Size(50, 50);
            this.rdoPaint.TabIndex = 28;
            this.rdoPaint.Text = "Paint";
            this.rdoPaint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoPaint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rdoPaint.UseVisualStyleBackColor = true;
            this.rdoPaint.CheckedChanged += new System.EventHandler(this.tool_CheckedChanged);
            // 
            // ToolBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rdoPaint);
            this.Controls.Add(this.rdoBlock);
            this.Controls.Add(this.rdoSpawn);
            this.Controls.Add(this.pnlToolOptions);
            this.Controls.Add(this.rdoSelect);
            this.Controls.Add(this.rdoRotate);
            this.Controls.Add(this.rdoMove);
            this.Name = "ToolBox";
            this.Size = new System.Drawing.Size(150, 538);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoMove;
        private System.Windows.Forms.RadioButton rdoRotate;
        private System.Windows.Forms.RadioButton rdoSelect;
        private System.Windows.Forms.Panel pnlToolOptions;
        private System.Windows.Forms.RadioButton rdoSpawn;
        private System.Windows.Forms.RadioButton rdoBlock;
        private System.Windows.Forms.RadioButton rdoPaint;

    }
}
