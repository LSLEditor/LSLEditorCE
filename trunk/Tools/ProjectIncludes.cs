using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LSLEditor.Tools
{
    public partial class ProjectIncludes : UserControl, ICommit
    {
        public ProjectIncludes()
        {
            InitializeComponent();
            listBoxIncludeDirs.Items.Clear();
            listBoxIncludeDirs.Items.AddRange(Properties.Settings.Default.IncludeDirectories.ToArray());
        }

        private bool AddToIncludeDirs(string path)
        {
            // Check if it can find the directory
            if(Directory.Exists(path))
            {
                // Put directory seperator after path
                path = path.LastOrDefault() == '\\' || path.LastOrDefault() == '/' ? path : path + '\\';
                
                // Check if it's already in the settings
                if(!Properties.Settings.Default.IncludeDirectories.Contains(path))
                {
                    // Add to listbox
                    listBoxIncludeDirs.Items.Add(path);
                    return true;
                }
            } else
            {
				MessageBox.Show("The given directory was not found. \n\"" + path + "\"", "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private bool RemoveFromIncludeDirs()
        {
            if(listBoxIncludeDirs.SelectedItem != null)
            {
                listBoxIncludeDirs.Items.Remove(listBoxIncludeDirs.SelectedItem);
                return true;
            }
            return false;
        }

        public void Commit()
        {
            List<string> items = new List<string>();
            // Add to settings
            foreach(Object item in listBoxIncludeDirs.Items)
            {
                items.Add(item.ToString());
            }

            Properties.Settings.Default.IncludeDirectories = items;
        }

        private void buttonAddIncludeDir_Click(object sender, EventArgs e)
        {
            if(textBoxAddIncludeDir.Text != "")
            {
                AddToIncludeDirs(textBoxAddIncludeDir.Text);
            }
        }

        private void textBoxAddIncludeDir_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                if (textBoxAddIncludeDir.Text != "")
                {
                    AddToIncludeDirs(textBoxAddIncludeDir.Text);
                }
            }
        }

        private void buttonBrowseDirs_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialogSelectIncludeDir.RootFolder = Environment.SpecialFolder.MyComputer;
            if (this.folderBrowserDialogSelectIncludeDir.ShowDialog(this) == DialogResult.OK)
            {
                AddToIncludeDirs(this.folderBrowserDialogSelectIncludeDir.SelectedPath);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            RemoveFromIncludeDirs();
        }

        private void listBoxIncludeDirs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                RemoveFromIncludeDirs();
            }
        }
    }
}
