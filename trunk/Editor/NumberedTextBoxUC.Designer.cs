namespace NumberedTextBox
{
    partial class NumberedTextBoxUC
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.numbered1 = new LSLEditor.Editor.Numbered();
			this.syntaxRichTextBox1 = new LSLEditor.SyntaxRichTextBox();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.splitContainer1.Panel1.Controls.Add(this.numbered1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.syntaxRichTextBox1);
			this.splitContainer1.Size = new System.Drawing.Size(403, 267);
			this.splitContainer1.SplitterDistance = 41;
			this.splitContainer1.SplitterWidth = 1;
			this.splitContainer1.TabIndex = 2;
			this.splitContainer1.Text = "splitContainer1";
			// 
			// numbered1
			// 
			this.numbered1.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.numbered1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.numbered1.Location = new System.Drawing.Point(0, 0);
			this.numbered1.Name = "numbered1";
			this.numbered1.Size = new System.Drawing.Size(41, 267);
			this.numbered1.TabIndex = 0;
			// 
			// syntaxRichTextBox1
			// 
			this.syntaxRichTextBox1.AcceptsTab = true;
			this.syntaxRichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.syntaxRichTextBox1.DetectUrls = false;
			this.syntaxRichTextBox1.Dirty = true;
			this.syntaxRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.syntaxRichTextBox1.Location = new System.Drawing.Point(0, 0);
			this.syntaxRichTextBox1.Name = "syntaxRichTextBox1";
			this.syntaxRichTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
			this.syntaxRichTextBox1.Size = new System.Drawing.Size(361, 267);
			this.syntaxRichTextBox1.TabIndex = 0;
			this.syntaxRichTextBox1.Text = "";
			this.syntaxRichTextBox1.ToolTipping = false;
			this.syntaxRichTextBox1.WordWrap = false;
			// 
			// NumberedTextBoxUC
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Name = "NumberedTextBoxUC";
			this.Size = new System.Drawing.Size(403, 267);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private LSLEditor.Editor.Numbered numbered1;
		private LSLEditor.SyntaxRichTextBox syntaxRichTextBox1;
    }
}
