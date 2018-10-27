using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LSLEditor.Tools
{
    public partial class IncludeExportSettings : UserControl, ICommit
    {
        public IncludeExportSettings()
        {
            InitializeComponent();
            checkBox1.Checked = Properties.Settings.Default.ShowIncludeMetaData;
        }

        public void Commit()
        {
            Properties.Settings.Default.ShowIncludeMetaData = checkBox1.Checked;
        }
    }
}
