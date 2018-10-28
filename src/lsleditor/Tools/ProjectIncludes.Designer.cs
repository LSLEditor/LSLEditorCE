namespace LSLEditor.Tools
{
    partial class ProjectIncludes
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
            this.textBoxAddIncludeDir = new System.Windows.Forms.TextBox();
            this.groupBoxIncludeDirs = new System.Windows.Forms.GroupBox();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAddIncludeDir = new System.Windows.Forms.Button();
            this.listBoxIncludeDirs = new System.Windows.Forms.ListBox();
            this.buttonBrowseDirs = new System.Windows.Forms.Button();
            this.labelIncludeDirs = new System.Windows.Forms.Label();
            this.folderBrowserDialogSelectIncludeDir = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBoxIncludeDirs.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxAddIncludeDir
            // 
            this.textBoxAddIncludeDir.Location = new System.Drawing.Point(16, 40);
            this.textBoxAddIncludeDir.Name = "textBoxAddIncludeDir";
            this.textBoxAddIncludeDir.Size = new System.Drawing.Size(270, 20);
            this.textBoxAddIncludeDir.TabIndex = 0;
            this.textBoxAddIncludeDir.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxAddIncludeDir_KeyPress);
            // 
            // groupBoxIncludeDirs
            // 
            this.groupBoxIncludeDirs.Controls.Add(this.buttonRemove);
            this.groupBoxIncludeDirs.Controls.Add(this.buttonAddIncludeDir);
            this.groupBoxIncludeDirs.Controls.Add(this.listBoxIncludeDirs);
            this.groupBoxIncludeDirs.Controls.Add(this.buttonBrowseDirs);
            this.groupBoxIncludeDirs.Controls.Add(this.labelIncludeDirs);
            this.groupBoxIncludeDirs.Location = new System.Drawing.Point(3, 3);
            this.groupBoxIncludeDirs.Name = "groupBoxIncludeDirs";
            this.groupBoxIncludeDirs.Size = new System.Drawing.Size(386, 266);
            this.groupBoxIncludeDirs.TabIndex = 1;
            this.groupBoxIncludeDirs.TabStop = false;
            this.groupBoxIncludeDirs.Text = "Include directories";
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(290, 229);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 4;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonAddIncludeDir
            // 
            this.buttonAddIncludeDir.Location = new System.Drawing.Point(331, 36);
            this.buttonAddIncludeDir.Name = "buttonAddIncludeDir";
            this.buttonAddIncludeDir.Size = new System.Drawing.Size(34, 23);
            this.buttonAddIncludeDir.TabIndex = 3;
            this.buttonAddIncludeDir.Text = "Add";
            this.buttonAddIncludeDir.UseVisualStyleBackColor = true;
            this.buttonAddIncludeDir.Click += new System.EventHandler(this.buttonAddIncludeDir_Click);
            // 
            // listBoxIncludeDirs
            // 
            this.listBoxIncludeDirs.FormattingEnabled = true;
            this.listBoxIncludeDirs.HorizontalScrollbar = true;
            this.listBoxIncludeDirs.Location = new System.Drawing.Point(13, 76);
            this.listBoxIncludeDirs.Name = "listBoxIncludeDirs";
            this.listBoxIncludeDirs.Size = new System.Drawing.Size(352, 147);
            this.listBoxIncludeDirs.TabIndex = 2;
            this.listBoxIncludeDirs.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBoxIncludeDirs_KeyUp);
            // 
            // buttonBrowseDirs
            // 
            this.buttonBrowseDirs.Location = new System.Drawing.Point(292, 36);
            this.buttonBrowseDirs.Name = "buttonBrowseDirs";
            this.buttonBrowseDirs.Size = new System.Drawing.Size(32, 23);
            this.buttonBrowseDirs.TabIndex = 1;
            this.buttonBrowseDirs.Text = "...";
            this.buttonBrowseDirs.UseVisualStyleBackColor = true;
            this.buttonBrowseDirs.Click += new System.EventHandler(this.buttonBrowseDirs_Click);
            // 
            // labelIncludeDirs
            // 
            this.labelIncludeDirs.AutoSize = true;
            this.labelIncludeDirs.Location = new System.Drawing.Point(14, 20);
            this.labelIncludeDirs.Name = "labelIncludeDirs";
            this.labelIncludeDirs.Size = new System.Drawing.Size(109, 13);
            this.labelIncludeDirs.TabIndex = 0;
            this.labelIncludeDirs.Text = "Add include directory:";
            // 
            // ProjectIncludes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxAddIncludeDir);
            this.Controls.Add(this.groupBoxIncludeDirs);
            this.Name = "ProjectIncludes";
            this.Size = new System.Drawing.Size(392, 272);
            this.groupBoxIncludeDirs.ResumeLayout(false);
            this.groupBoxIncludeDirs.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxAddIncludeDir;
        private System.Windows.Forms.GroupBox groupBoxIncludeDirs;
        private System.Windows.Forms.Label labelIncludeDirs;
        private System.Windows.Forms.Button buttonBrowseDirs;
        private System.Windows.Forms.ListBox listBoxIncludeDirs;
        private System.Windows.Forms.Button buttonAddIncludeDir;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogSelectIncludeDir;
    }
}
