// <copyright file="gpl-2.0.txt">
// ORIGINAL CODE BASE IS Copyright (C) 2006-2010 by Alphons van der Heijden.
// The code was donated on 2010-04-28 by Alphons van der Heijden to Brandon 'Dimentox Travanti' Husbands &
// Malcolm J. Kudra, who in turn License under the GPLv2 in agreement with Alphons van der Heijden's wishes.
//
// The community would like to thank Alphons for all of his hard work, blood sweat and tears. Without his work
// the community would be stuck with crappy editors.
//
// The source code in this file ("Source Code") is provided by The LSLEditor Group to you under the terms of the GNU
// General Public License, version 2.0 ("GPL"), unless you have obtained a separate licensing agreement ("Other
// License"), formally executed by you and The LSLEditor Group.
// Terms of the GPL can be found in the gplv2.txt document.
//
// GPLv2 Header
// ************
// LSLEditor, a External editor for the LSL Language.
// Copyright (C) 2010 The LSLEditor Group.
//
// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any
// later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
// warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more
// details.
//
// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free
// Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// ********************************************************************************************************************
// The above copyright notice and this permission notice shall be included in copies or substantial portions of the
// Software.
// ********************************************************************************************************************
// </copyright>
//
// <summary>
//
//
// </summary>

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using LSLEditor.Docking;

namespace LSLEditor.Plugins
{
    internal class Generic
    {
        private delegate void RunDelegate(string strFilePath, RichTextBox tb);

        public Generic(LSLEditorForm parent, string strPluginName)
        {
            if (!(parent.ActiveMdiForm is EditForm editForm))
            {
                return;
            }

            var strFilePath = editForm.FullPathName;
            var tb = editForm.TextBox as RichTextBox;

            var strDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var strPluginsDirectory = Path.Combine(strDirectory, "Plugins");
            var strProgram = Path.Combine(strPluginsDirectory, strPluginName + ".exe");

            Assembly assembly = null;

            try
            {
                assembly = Assembly.LoadFrom(strProgram);
            }
            catch
            {
                MessageBox.Show("Not a valid plugin, see http://www.lsleditor.org/help/Plugins.aspx", "Plugins", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (assembly == null)
            {
                return;
            }

            var args = new object[] { strFilePath, tb };

            foreach (var t in assembly.GetTypes())
            {
                if (t.BaseType.Name == "DockContent" && assembly.CreateInstance(t.FullName, false,
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance,
                        null, args, null, null) is DockContent form)
                {
                    parent.AddForm(form);
                    return;
                }
                var info = t.GetMethod("Run",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

                if (info == null)
                {
                    continue;
                }

                info.Invoke(null, args);
            }
        }
    }
}
