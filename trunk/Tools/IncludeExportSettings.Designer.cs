namespace LSLEditor.Tools
{
    partial class IncludeExportSettings
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
            this.groupBoxIncludeExportSettings = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBoxIncludeExportSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxIncludeExportSettings
            // 
            this.groupBoxIncludeExportSettings.Controls.Add(this.checkBox1);
            this.groupBoxIncludeExportSettings.Location = new System.Drawing.Point(3, 3);
            this.groupBoxIncludeExportSettings.Name = "groupBoxIncludeExportSettings";
            this.groupBoxIncludeExportSettings.Size = new System.Drawing.Size(380, 64);
            this.groupBoxIncludeExportSettings.TabIndex = 0;
            this.groupBoxIncludeExportSettings.TabStop = false;
            this.groupBoxIncludeExportSettings.Text = "Include export settings";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(16, 28);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(184, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Show include metadata on export";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // IncludeExportSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxIncludeExportSettings);
            this.Name = "IncludeExportSettings";
            this.Size = new System.Drawing.Size(386, 266);
            this.groupBoxIncludeExportSettings.ResumeLayout(false);
            this.groupBoxIncludeExportSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxIncludeExportSettings;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}
