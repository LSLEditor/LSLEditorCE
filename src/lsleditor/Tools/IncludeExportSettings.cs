using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LSLEditor.Tools
{
    public partial class IncludeExportSettings : UserControl, ICommit
    {
        public IncludeExportSettings()
        {
            this.InitializeComponent();
            this.checkBox1.Checked = Properties.Settings.Default.ShowIncludeMetaData;
        }

        public void Commit()
        {
            Properties.Settings.Default.ShowIncludeMetaData = this.checkBox1.Checked;
        }
    }
}
