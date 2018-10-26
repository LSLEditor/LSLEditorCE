using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LSLEditor.Tools
{
    public partial class ProjectIncludes : UserControl, ICommit
    {
        public ProjectIncludes()
        {
            this.InitializeComponent();

            this.InitIncludeDirsList();
        }

        private void InitIncludeDirsList()
        {
            this.listBoxIncludeDirs.Items.Clear();
            if (Properties.Settings.Default.IncludeDirectories == null)
            {
                Properties.Settings.Default.IncludeDirectories = new System.Collections.Specialized.StringCollection();
            }
            else
            {
                var intLen = Properties.Settings.Default.IncludeDirectories.Count;
                for (var intI = 0; intI < intLen; intI++)
                {
                    var item = Properties.Settings.Default.IncludeDirectories[intI].Trim();
                    if (item.Length > 0)
                    {
                        this.listBoxIncludeDirs.Items.Add(item);
                    }
                }
            }
        }

        private bool AddToIncludeDirs(string path)
        {
            // Check if it can find the directory
            if (Directory.Exists(path))
            {
                // Put directory seperator after path
                path = path.LastOrDefault() == '\\' || path.LastOrDefault() == '/' ? path : path + '\\';

                // Check if it's already in the settings
                if (!Properties.Settings.Default.IncludeDirectories.Contains(path))
                {
                    // Add to listbox
                    this.listBoxIncludeDirs.Items.Add(path);
                    return true;
                }
            }
            else
            {
                MessageBox.Show("The given directory was not found. \n\"" + path + "\"", "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private bool RemoveFromIncludeDirs()
        {
            if (this.listBoxIncludeDirs.SelectedItem != null)
            {
                this.listBoxIncludeDirs.Items.Remove(this.listBoxIncludeDirs.SelectedItem);
                return true;
            }
            return false;
        }

        public void Commit()
        {
            // Add to settings
            Properties.Settings.Default.IncludeDirectories = new System.Collections.Specialized.StringCollection();
            var intLen = this.listBoxIncludeDirs.Items.Count;
            for (var intI = 0; intI < intLen; intI++)
            {
                var item = this.listBoxIncludeDirs.Items[intI];
                if (item != null)
                {
                    Properties.Settings.Default.IncludeDirectories.Add(item.ToString());
                }
            }
        }

        private void buttonAddIncludeDir_Click(object sender, EventArgs e)
        {
            if (this.textBoxAddIncludeDir.Text != "")
            {
                this.AddToIncludeDirs(this.textBoxAddIncludeDir.Text);
            }
        }

        private void textBoxAddIncludeDir_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.textBoxAddIncludeDir.Text != "")
            {
                this.AddToIncludeDirs(this.textBoxAddIncludeDir.Text);
            }
        }

        private void buttonBrowseDirs_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialogSelectIncludeDir.RootFolder = Environment.SpecialFolder.MyComputer;
            if (this.folderBrowserDialogSelectIncludeDir.ShowDialog(this) == DialogResult.OK)
            {
                this.AddToIncludeDirs(this.folderBrowserDialogSelectIncludeDir.SelectedPath);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            this.RemoveFromIncludeDirs();
        }

        private void listBoxIncludeDirs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                this.RemoveFromIncludeDirs();
            }
        }
    }
}
