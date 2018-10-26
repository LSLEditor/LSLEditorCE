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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace LSLEditor.Solution
{
    public partial class SolutionExplorer : ToolWindow
    {
        public enum TypeSL : int
        {
            // Inventory containers
            Inventory = 0,
            Animations = 1,
            Body_Parts = 2,
            Calling_Cards = 3,
            Clothes = 4,
            Gestures = 5,
            Landmarks = 6,
            Lost_And_Found = 7,
            Notecards = 8,
            Objects = 9,
            Photo_Album = 10,
            Scripts = 11,
            Sounds = 12,
            Textures = 13,
            Trash = 14,
            Folder = 15,

            // Special containers
            Solution = 16,
            Project = 17,
            Prim = 18, // not used, use Object instead
            Unknown = 19,

            // Normal items!!

            // Animations
            Animation = 20,

            Gesture = 21,
            Landmark = 22,
            Notecard = 23,
            Object = 24, // This is a normal container item!!!

            // Photo Album
            Snapshot = 25,

            Script = 26,
            LSLIScript = 42,
            Sound = 27,
            Texture = 28,

            // Body parts
            Eyes = 29,
            Hair = 30,
            Shape = 31,
            Skin = 32,

            // Clothes
            Gloves = 33,
            Jacket = 34,
            Pants = 35,
            Shirt = 36,
            Shoes = 37,
            Skirt = 38,
            Socks = 39,
            Underpants = 40,
            Undershirt = 41
        }

        private readonly TypeSL[] BodyPartsTypes =
        {
            TypeSL.Eyes,
            TypeSL.Hair,
            TypeSL.Shape,
            TypeSL.Skin
        };

        private readonly TypeSL[] ClothesTypes =
        {
            TypeSL.Gloves,
            TypeSL.Jacket,
            TypeSL.Pants,
            TypeSL.Shirt,
            TypeSL.Shoes,
            TypeSL.Skirt,
            TypeSL.Socks,
            TypeSL.Underpants,
            TypeSL.Undershirt
        };

        private readonly TypeSL[] FilesTypes =
        {
            TypeSL.Animation,
            TypeSL.Gesture,
            TypeSL.Landmark ,
            TypeSL.Notecard,
            TypeSL.Snapshot,
            TypeSL.Object,
            TypeSL.Script,
            TypeSL.LSLIScript,
            TypeSL.Sound,
            TypeSL.Texture
        };

        public struct RealTag
        {
            public TypeSL ItemType;
            public string Name;
            public string Description;
            public Guid Guid;
            public string Path;

            public RealTag(TypeSL ItemType, string Name, Guid Guid)
            {
                this.ItemType = ItemType;
                this.Name = Name;
                this.Description = string.Empty;
                this.Guid = Guid;
                this.Path = null;
            }

            public RealTag(TypeSL ItemType, string Name, Guid Guid, string Path)
            {
                this.ItemType = ItemType;
                this.Name = Name;
                this.Description = string.Empty;
                this.Guid = Guid;
                this.Path = Path;
            }
        }

        public string DirectorySolution;
        public string NameSolution;
        public string Repository;

        private TreeNode ActiveSolution;
        private TreeNode ActiveProject;
        private TreeNode ActiveObject;

        private TreeNode CutObject;
        private TreeNode CopyObject;

        public LSLEditorForm parent;

        private readonly SolidBrush brushHighLightBackground;
        private readonly SolidBrush brushNormalForeColor;
        private readonly SolidBrush brushCutForeColor;
        private readonly SolidBrush brushBackColor;
        private readonly Pen penWhite;
        private readonly Pen penBackColor;

        private readonly ImageList imageList1;

        private readonly OpenFileDialog openFileDialog1;

        private readonly Timer timer;

        private bool m_dirty;

        private ListViewItem m_HoveredItem;
        private readonly ToolTip toolTip1;

        public bool IsDirty
        {
            get
            {
                return this.m_dirty;
            }
        }

        public SolutionExplorer()
        {
            this.InitializeComponent();

            this.Repository = null;

            this.toolTip1 = new ToolTip();

            this.m_dirty = false;

            this.components = new System.ComponentModel.Container();

            this.imageList1 = new ImageList
            {
                TransparentColor = Color.Transparent
            };
            for (var intI = 0; intI <= 42; intI++)//41
            {
                var typeSL = (TypeSL)intI;
                this.imageList1.Images.Add(intI.ToString(), new Bitmap(typeof(LSLEditorForm), "ImagesSolutionExplorer." + typeSL.ToString().Replace("_", " ") + ".gif"));
            }

            // take care of this as an property
            //this.parent = parent;

            this.brushHighLightBackground = new SolidBrush(Color.FromArgb(0xff, 0x6d, 0x6e, 0x6f));
            this.brushNormalForeColor = new SolidBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            this.brushCutForeColor = new SolidBrush(Color.FromArgb(0xff, Color.DarkGray));
            this.brushBackColor = new SolidBrush(Color.FromArgb(0xff, 0x3e, 0x3e, 0x3e));
            this.penWhite = new Pen(Color.White);
            this.penBackColor = new Pen(this.brushBackColor);

            this.treeView1.ImageList = this.imageList1;
            this.treeView1.ShowRootLines = false; // just 1 root
            this.treeView1.ForeColor = Color.White;
            this.treeView1.BackColor = Color.FromArgb(0xff, 0x3e, 0x3e, 0x3e);
            this.treeView1.LabelEdit = false;
            this.treeView1.BeforeCollapse += this.treeView1_BeforeCollapse;
            this.treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.treeView1.DrawNode += this.treeView1_DrawNode;
            this.treeView1.AfterLabelEdit += this.treeView1_AfterLabelEdit;
            this.treeView1.MouseDown += this.treeView1_MouseDown;

            this.treeView1.NodeMouseDoubleClick += this.treeView1_NodeMouseDoubleClick;

            this.treeView1.MouseUp += this.treeView1_MouseUp;

            this.treeView1.TreeViewNodeSorter = new NodeSorter();

            this.listView1.Dock = DockStyle.Fill;
            this.listView1.View = View.Details;
            this.listView1.HeaderStyle = ColumnHeaderStyle.None;
            //this.listView1.Scrollable = false;

            this.listView1.GridLines = true;
            this.listView1.Columns.Add("Name", 60);
            this.listView1.Columns.Add("Value", 250);

            // for disposing
            this.timer = new Timer(this.components)
            {
                Interval = 250
            };
            this.timer.Tick += this.timer_Tick;

            this.openFileDialog1 = new OpenFileDialog();
        }

        public string GetCurrentSolutionPath()
        {
            if (this.ActiveSolution == null)
            {
                return null;
            }

            if (this.DirectorySolution == null)
            {
                return null;
            }

            if (this.NameSolution == null)
            {
                return null;
            }

            return Path.Combine(this.DirectorySolution, this.NameSolution + ".sol");
        }

        public string GetProjectName(Guid guid)
        {
            var tn = this.FindGuid(this.treeView1.TopNode, guid);
            if (tn != null)
            {
                while (tn != this.treeView1.TopNode)
                {
                    tn = tn.Parent;
                    if (this.GetTypeSL(tn) == TypeSL.Project)
                    {
                        return tn.Text;
                    }
                }
            }
            return "";
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.timer.Stop();
            this.RenameAction(this.treeView1.SelectedNode);
        }

        private int NumberOfProjects()
        {
            if (this.ActiveSolution == null)
            {
                return 0;
            }

            var intProjects = 0;
            foreach (TreeNode tn in this.ActiveSolution.Nodes)
            {
                if (this.GetTypeSL(tn) == TypeSL.Project)
                {
                    intProjects++;
                }
            }
            return intProjects;
        }

        private bool HasInventory()
        {
            foreach (TreeNode tn in this.ActiveSolution.Nodes)
            {
                if (this.GetTypeSL(tn) == TypeSL.Inventory)
                {
                    return true;
                }
            }
            return false;
        }

        private void EmptyInventory(TreeNode parentNode)
        {
            this.m_dirty = true;
            var treeNode = new TreeNode("Inventory", (int)TypeSL.Inventory, (int)TypeSL.Inventory);
            var strInventoryPath = Path.Combine(this.DirectorySolution, "Inventory");
            treeNode.Tag = new RealTag(TypeSL.Inventory, "Inventory", Guid.NewGuid(), strInventoryPath);
            parentNode.Nodes.Insert(0, treeNode);
            for (var intI = 1; intI < 15; intI++)
            {
                var typeSL = (TypeSL)intI;
                var strName = typeSL.ToString().Replace('_', ' ');
                var tn = new TreeNode(strName, intI, intI)
                {
                    Tag = new RealTag(typeSL, strName, Guid.NewGuid())
                };
                treeNode.Nodes.Add(tn);
            }
        }

        private EditForm GetEditForm(Guid guid)
        {
            var lslEditorForm = this.parent;
            foreach (var form in lslEditorForm.Children)
            {
                var editForm = form as EditForm;
                if (editForm?.IsDisposed != false)
                {
                    continue;
                }

                if (editForm.guid == guid)
                {
                    return editForm;
                }
            }
            return null;
        }

        public void CloseSolution()
        {
            if (this.ActiveSolution == null)
            {
                return;
            }

            this.parent.StopSimulator();

            if (this.parent.CloseAllOpenWindows())
            {
                this.SaveSolutionFile();

                this.ActiveProject = null;
                this.ActiveSolution = null;
                this.ActiveObject = null;

                this.m_dirty = false;

                this.treeView1.Nodes.Clear();

                this.listView1.Items.Clear();

                if (!Properties.Settings.Default.ShowSolutionExplorer)
                {
                    this.parent.ShowSolutionExplorer(false);
                }

                this.parent.SolutionItemsFileMenu(false);
            }
        }

        public bool CreateNew(string strNameProject, string strDirectory, string strNameSolution, bool blnCreateSolutionDirectory)
        {
            this.DirectorySolution = Path.Combine(strDirectory, strNameSolution);

            var strSolutionFileName = Path.Combine(this.DirectorySolution, strNameSolution + ".sol");

            if (File.Exists(strSolutionFileName))
            {
                MessageBox.Show("Solution file already exists", "Create new Solution", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            this.AddSolution(strNameSolution);

            var strProjectPath = blnCreateSolutionDirectory ? Path.Combine(this.DirectorySolution, strNameProject) : this.DirectorySolution;

            // made active, if only project
            this.AddItem(TypeSL.Project, strNameProject, Guid.NewGuid(), strProjectPath);

            return true;
        }

        public void AddExistingProject()
        {
            this.openFileDialog1.Multiselect = false;
            this.openFileDialog1.FileName = "";
            this.openFileDialog1.Filter = "LSLEditor Project file (*.prj)|*.prj|All Files (*.*)|*.*";
            if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                if (File.Exists(this.openFileDialog1.FileName))
                {
                    this.LoadItem(this.openFileDialog1.FileName, false);
                }
            }
        }

        #region UserInterface stuff

        // disable root solution collapse (there is no + sign)
        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = (e.Node == this.ActiveSolution);
        }

        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.IsEditing)
            {
                return;
            }

            if (e.Bounds.Height == 0)
            {
                return;
            }

            var font = e.Node == this.ActiveProject || e.Node == this.ActiveObject
                ? new Font(this.treeView1.Font, FontStyle.Bold)
                : new Font(this.treeView1.Font, FontStyle.Regular);

            // Draw background
            var sizeF = e.Graphics.MeasureString(e.Node.Text, font);
            var rect = new Rectangle(e.Bounds.Location, new Size((int)sizeF.Width + 5, e.Bounds.Height - 1));

            if ((e.State & TreeNodeStates.Selected) != 0)
            {
                e.Graphics.FillRectangle(this.brushHighLightBackground, rect);
                e.Graphics.DrawRectangle(this.penWhite, rect);
            }
            else
            {
                e.Graphics.FillRectangle(this.brushBackColor, rect);
                e.Graphics.DrawRectangle(this.penBackColor, rect);
            }

            // Draw text
            var point = new Point(e.Bounds.Location.X, e.Bounds.Location.Y + 2);
            if (e.Node.ForeColor != Color.Empty)
            {
                e.Graphics.DrawString(e.Node.Text, font, this.brushCutForeColor, point);
            }
            else
            {
                e.Graphics.DrawString(e.Node.Text, font, this.brushNormalForeColor, point);
            }

            // Clean up
            font.Dispose();
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            this.treeView1.LabelEdit = false;

            if (e.Label == null || e.Label.Trim()?.Length == 0)
            {
                e.CancelEdit = true;
                return;
            }

            // check for illegal chars!!
            var strLabel = e.Label;

            foreach (var chrF in e.Label)
            {
                foreach (var chrInvalid in Path.GetInvalidFileNameChars())
                {
                    if (chrF == chrInvalid)
                    {
                        MessageBox.Show("Char '" + chrInvalid + "' is not allowed", "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.CancelEdit = true;
                        return;
                    }
                }
            }

            var strBackup = e.Node.Text;
            var strSource = this.GetFullPath(e.Node);
            e.Node.Text = e.Label;
            var strDestination = this.GetFullPath(e.Node);

            if (e.Node == this.ActiveSolution)
            {
                this.NameSolution = e.Label;
                this.UpdateVisualSolutionName();
                var strOldSolutionFile = Path.GetFullPath(Path.Combine(strSource, strBackup + ".sol"));
                var strNewSolutionFile = Path.GetFullPath(Path.Combine(strDestination, this.NameSolution + ".sol"));

                if (File.Exists(strNewSolutionFile) && string.Compare(strOldSolutionFile, strNewSolutionFile, true) != 0)
                {
                    if (MessageBox.Show("Destination solution exists! overwrite?", "Oops...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        e.Node.Text = strBackup;
                        e.CancelEdit = true;
                        return;
                    }
                    try
                    {
                        File.Delete(strNewSolutionFile);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Error: " + exception.Message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Node.Text = strBackup;
                        e.CancelEdit = true;
                        return;
                    }
                }

                if (File.Exists(strOldSolutionFile))
                {
                    File.Move(strOldSolutionFile, strNewSolutionFile);
                }

                this.ShowProperties(this.ActiveSolution);

                e.CancelEdit = true;
                return;
            }

            // source and destination are the same, cancel it all (file or directory)
            if (string.Compare(strSource, strDestination, true) == 0)
            {
                e.Node.Text = strBackup;
                e.CancelEdit = true;
                return;
            }

            // rename file
            if (File.Exists(strSource))
            {
                if (File.Exists(strDestination))
                {
                    if (MessageBox.Show("Destination file exists! overwrite?", "Oops...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        e.Node.Text = strBackup;
                        e.CancelEdit = true;
                        return;
                    }
                    try
                    {
                        File.Delete(strDestination);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Error: " + exception.Message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Node.Text = strBackup;
                        e.CancelEdit = true;
                        return;
                    }
                }
                try
                {
                    File.Move(strSource, strDestination);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error: " + exception.Message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Node.Text = strBackup;
                    e.CancelEdit = true;
                    return;
                }

                var rt = (RealTag)e.Node.Tag;
                var oldName = rt.Name;
                rt.Name = e.Node.Text;
                e.Node.Tag = rt; // save name
                var editForm = this.GetEditForm(rt.Guid);
                if (editForm != null)
                {
                    editForm.FullPathName = strDestination; // GetFullPath(e.Node);
                    editForm.SaveCurrentFile();
                }

                if (rt.ItemType == TypeSL.LSLIScript)
                {
                    var form = this.GetEditForm(rt.Guid);
                    if (form != null)
                    {
                        form.SaveCurrentFile();
                        form.Dirty = true;
                        form.Show();

                        // Get the expanded version of the form
                        var x = Helpers.LSLIPathHelper.GetExpandedTabName(Helpers.LSLIPathHelper.CreateExpandedScriptName(oldName));
                        var xform = (EditForm)this.parent.GetForm(x);

                        if (xform?.Visible == true)
                        {
                            var src = xform.SourceCode;
                            xform.Dirty = false;
                            xform.Close();
                            form.SourceCode = src;
                            form.Dirty = true;
                            this.parent.ExpandForm(form);
                        }
                    }
                }

                return; // rename file complete
            }

            // rename directory
            if (Directory.Exists(strSource))
            {
                if (Directory.Exists(strDestination))
                {
                    if (MessageBox.Show("Destination directory exists! overwrite?", "Oops...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        e.Node.Text = strBackup;
                        e.CancelEdit = true;
                        return;
                    }
                    try
                    {
                        Directory.Delete(strDestination, true);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Error: " + exception.Message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Node.Text = strBackup;
                        e.CancelEdit = true;
                        return;
                    }
                }

                try
                {
                    Directory.Move(strSource, strDestination);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error: " + exception.Message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Node.Text = strBackup;
                    e.CancelEdit = true;
                    return;
                }

                var rt = (RealTag)e.Node.Tag;
                rt.Name = e.Node.Text;
                e.Node.Tag = rt; // save name

                // update all path of opened windows
                foreach (var form in this.parent.Children)
                {
                    var editForm = form as EditForm;
                    if (editForm?.IsDisposed != false)
                    {
                        continue;
                    }

                    var tn = this.FindGuid(e.Node, editForm.guid);
                    if (tn != null)
                    {
                        editForm.FullPathName = this.GetFullPath(tn);
                        editForm.SaveCurrentFile();
                    }
                }
                return;
            }

            MessageBox.Show("Can not rename, source does not exist", "Oops...");
            e.Node.Text = strBackup;
            e.CancelEdit = true;
        }
        #endregion

        private bool IsItem(TreeNode treeNode)
        {
            var typeSL = this.GetTypeSL(treeNode);
            return (int)typeSL >= 20;
        }

        private bool IsItemAndNotObject(TreeNode treeNode)
        {
            var typeSL = this.GetTypeSL(treeNode);
            var intTypeSL = (int)typeSL;
            return intTypeSL >= 20 && typeSL != TypeSL.Object;
        }

        private bool IsInventoryContainer(TreeNode treeNode)
        {
            var TypeSL = this.GetTypeSL(treeNode);
            return (int)TypeSL <= 15;
        }

        private TypeSL GetTypeSL(TreeNode treeNode)
        {
            if (treeNode == null)
            {
                return TypeSL.Unknown;
            }

            if (treeNode.Tag == null)
            {
                return TypeSL.Unknown;
            }

            var realTag = (RealTag)treeNode.Tag;
            return realTag.ItemType;
        }

        private string GetPath(TreeNode treeNode)
        {
            if (treeNode == null)
            {
                return null;
            }

            if (treeNode.Tag == null)
            {
                return null;
            }

            var realTag = (RealTag)treeNode.Tag;
            var strPath = realTag.Path;
            if (strPath == null)
            {
                return null;
            }

            if (strPath.EndsWith(treeNode.Text))
            {
                return strPath;
            }

            // Project has been renamed
            strPath = Path.Combine(Path.GetDirectoryName(strPath), treeNode.Text);
            realTag.Path = strPath;
            treeNode.Tag = realTag;
            return strPath;
        }

        private string GetXml(int intDepth, TreeNode treeNode)
        {
            var sb = new StringBuilder();
            var realTag = (RealTag)treeNode.Tag;
            var strTagName = realTag.ItemType.ToString();
            var strValue = treeNode.Text;
            if (treeNode == this.ActiveSolution)
            {
                strValue = this.NameSolution;
            }
            for (var intI = 0; intI < intDepth; intI++)
            {
                sb.Append('\t');
            }
            var strActive = "";
            var strDescription = "";
            var strEnd = "";
            if (treeNode == this.ActiveObject)
            {
                strActive = @" active=""true""";
            }
            if (realTag.Description != string.Empty)
            {
                strDescription = @" description=""" + realTag.Description + @"""";
            }
            if (treeNode.Nodes.Count == 0)
            {
                strEnd = " /";
            }
            sb.AppendFormat("<{0} name=\"{1}\" guid=\"{2}\"{3}{4}{5}>\r\n", strTagName, strValue, realTag.Guid, strActive, strDescription, strEnd);
            if (treeNode.Nodes.Count > 0)
            {
                foreach (TreeNode childNode in treeNode.Nodes)
                {
                    sb.Append(this.GetXml(intDepth + 1, childNode));
                }
                for (var intI = 0; intI < intDepth; intI++)
                {
                    sb.Append('\t');
                }
                sb.AppendFormat("</{0}>\r\n", strTagName);
            }
            return sb.ToString();
        }

        private string GetFullPath(TreeNode treeNode)
        {
            if (treeNode == this.ActiveSolution)
            {
                return this.DirectorySolution;
            }

            var parentNode = treeNode;
            var strPath = "";
            while (this.GetPath(parentNode) == null)
            {
                strPath = Path.Combine(parentNode.Text, strPath);
                parentNode = parentNode.Parent;
            }
            strPath = Path.Combine(this.GetPath(parentNode), strPath);
            return Path.GetFullPath(strPath);
        }

        private void RecursiveLoad(TreeNode treeNode, XmlNode xmlParentNode)
        {
            foreach (XmlNode xmlNode in xmlParentNode.SelectNodes("./*"))
            {
                var typeSL = (TypeSL)Enum.Parse(typeof(TypeSL), xmlNode.Name);

                if (typeSL == TypeSL.Prim) // Old!!!
                {
                    typeSL = TypeSL.Object;
                }

                var strName = xmlNode.Attributes["name"].Value;
                var guid = new Guid(xmlNode.Attributes["guid"].Value);

                var attributePath = xmlNode.Attributes["path"];
                var attributeDescription = xmlNode.Attributes["description"];
                var intImageIndex = (int)typeSL;
                var tn = new TreeNode(strName, intImageIndex, intImageIndex);

                var realTag = new RealTag(typeSL, strName, guid);

                if (attributePath != null)
                {
                    realTag.Path = attributePath.Value;
                }

                if (attributeDescription != null)
                {
                    realTag.Description = attributeDescription.Value;
                }

                tn.Tag = realTag;

                var attributeActive = xmlNode.Attributes["active"];
                if (attributeActive != null)
                {
                    if (attributeActive.Value == "true")
                    {
                        this.ActiveObject = tn;
                    }
                }
                treeNode.Nodes.Add(tn);
                this.RecursiveLoad(tn, xmlNode);
            }
        }

        private void LoadItem(string strPath, bool blnActive)
        {
            var xmlItemFile = new XmlDocument();
            try
            {
                xmlItemFile.Load(strPath);
            }
            catch
            {
                MessageBox.Show("Not a valid item file", "Oops...");
                return;
            }
            foreach (XmlNode xmlItem in xmlItemFile.SelectNodes("./*"))
            {
                var typeSL = (TypeSL)Enum.Parse(typeof(TypeSL), xmlItem.Name);
                var currentItem = this.AddItem(typeSL,
                    xmlItem.Attributes["name"].Value,
                    new Guid(xmlItem.Attributes["guid"].Value),
                    Path.GetDirectoryName(strPath));
                if (currentItem == null)
                {
                    continue;
                }

                if (blnActive)
                {
                    this.ActiveProject = currentItem;
                }

                this.RecursiveLoad(currentItem, xmlItem);
            }
        }

        public bool OpenSolution(string strPath)
        {
            this.DirectorySolution = Path.GetDirectoryName(strPath);
            this.NameSolution = Path.GetFileNameWithoutExtension(strPath);

            var xml = new XmlDocument();
            try
            {
                xml.Load(strPath);
            }
            catch
            {
                MessageBox.Show("Not a valid Solution file", "Oops...");
                this.parent.UpdateRecentProjectList(strPath, false);
                return false;
            }

            this.treeView1.Nodes.Clear();

            foreach (XmlNode xmlSolution in xml.SelectNodes("//Solution"))
            {
                if (xmlSolution.Attributes["repository"] != null)
                {
                    this.Repository = xmlSolution.Attributes["repository"].Value;
                }

                this.AddSolution(xmlSolution.Attributes["name"].Value);
                foreach (XmlNode xmlItem in xmlSolution.SelectNodes("./*"))
                {
                    var strItemPath = xmlItem.Attributes["path"].Value;
                    if (!Path.IsPathRooted(strItemPath))
                    {
                        strItemPath = Path.Combine(this.DirectorySolution, strItemPath);
                    }

                    if (!File.Exists(strItemPath))
                    {
                        continue; // TODO verbose error
                    }

                    var blnActive = false;
                    if (xmlItem.Attributes["active"] != null)
                    {
                        blnActive = (xmlItem.Attributes["active"].Value == "true");
                    }

                    this.LoadItem(strItemPath, blnActive);
                }
            }
            this.parent.ShowSolutionExplorer(true);
            this.parent.SolutionItemsFileMenu(true);
            this.parent.UpdateRecentProjectList(strPath, true);
            this.m_dirty = false;

            return true;
        }

        public bool SaveSolutionFile()
        {
            if (this.ActiveSolution == null)
            {
                return false;
            }

            if (this.DirectorySolution == null)
            {
                return false;
            }

            if (this.NameSolution == null)
            {
                return false;
            }

            if (!Directory.Exists(this.DirectorySolution))
            {
                Directory.CreateDirectory(this.DirectorySolution);
            }

            var strSolutionFileName = Path.Combine(this.DirectorySolution, this.NameSolution + ".sol");
            var sw = new StreamWriter(strSolutionFileName);
            var strRepositoryAttribute = this.Repository == null ? "" : string.Format(" repository=\"{0}\"", this.Repository);
            sw.WriteLine("<Solution name=\"{0}\"{1}>", this.NameSolution, strRepositoryAttribute);
            foreach (TreeNode nodeItem in this.ActiveSolution.Nodes)
            {
                var realTag = (RealTag)nodeItem.Tag;
                var strItemPath = this.SaveItemFile(nodeItem);
                if (strItemPath != null)
                {
                    // Make path relative, if it is the same as solution directory
                    if (strItemPath.IndexOf(this.DirectorySolution, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        strItemPath = strItemPath.Substring(this.DirectorySolution.Length + 1);
                    }

                    var strActive = "";
                    if (this.ActiveProject == nodeItem)
                    {
                        strActive = @" active=""true""";
                    }

                    sw.WriteLine("\t<Project name=\"{0}\" path=\"{1}\"{2} />", nodeItem.Text, strItemPath, strActive);
                }
            }
            sw.WriteLine("</Solution>");
            sw.Close();
            this.m_dirty = false;

            return true;
        }

        private string SaveItemFile(TreeNode nodeItem)
        {
            var strName = nodeItem.Text;
            var realTag = (RealTag)nodeItem.Tag;
            var strPath = realTag.Path;

            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }

            var strExtension = ".prj";
            if (realTag.Name == "Inventory")
            {
                strExtension = ".inv";
            }

            var strFilePath = Path.Combine(strPath, strName + strExtension);
            var sw = new StreamWriter(strFilePath);
            sw.Write(this.GetXml(0, nodeItem));
            sw.Close();
            return strFilePath;
        }

        private void AddSolution(string strNameSolution)
        {
            this.NameSolution = strNameSolution;
            this.ActiveSolution = new TreeNode("Solution '" + strNameSolution + "'", (int)TypeSL.Solution, (int)TypeSL.Solution)
            {
                Tag = new RealTag(TypeSL.Solution, strNameSolution, Guid.NewGuid())
            };
            this.treeView1.Nodes.Insert(0, this.ActiveSolution);
            this.ActiveSolution.Expand();
            this.parent.SolutionItemsFileMenu(true);
        }

        private void UpdateVisualSolutionName()
        {
            var intProjects = this.NumberOfProjects();
            this.ActiveSolution.Text = "Solution '" + this.NameSolution + "' (" + intProjects + " project" + ((intProjects != 1) ? "s" : "") + ")";
        }

        private bool ItemExist(Guid guid)
        {
            foreach (TreeNode itemNode in this.ActiveSolution.Nodes)
            {
                var realTag = (RealTag)itemNode.Tag;
                if (realTag.Guid == guid)
                {
                    return true;
                }
            }
            return false;
        }

        private TreeNode AddItem(TypeSL typeSL, string strName, Guid guid, string strPath)
        {
            if (this.ItemExist(guid))
            {
                return null;
            }

            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }

            var treeNode = new TreeNode(strName, (int)typeSL, (int)typeSL)
            {
                Tag = new RealTag(typeSL, strName, guid, strPath)
            };

            if (this.ActiveProject == null && typeSL == TypeSL.Project)
            {
                this.ActiveProject = treeNode;
            }

            this.ActiveSolution.Nodes.Add(treeNode);
            this.ActiveSolution.Expand();
            this.UpdateVisualSolutionName();

            this.m_dirty = true;

            return treeNode;
        }

        private TreeNode AddNewContainer(TreeNode parent, TypeSL typeSL, string strNewName, Guid guid)
        {
            if (parent == null)
            {
                return null;
            }

            var strDirectoryContainer = Path.Combine(this.GetFullPath(parent), strNewName);
            if (!Directory.Exists(strDirectoryContainer))
            {
                Directory.CreateDirectory(strDirectoryContainer);
            }

            var treeNode = new TreeNode(strNewName, (int)typeSL, (int)typeSL)
            {
                Tag = new RealTag(typeSL, strNewName, guid)
            };

            if (this.ActiveObject == null && typeSL == TypeSL.Object)
            {
                this.ActiveObject = treeNode;
            }

            parent.Nodes.Add(treeNode);
            parent.Expand();

            this.m_dirty = true;

            return treeNode;
        }

        private void AddNewFile(TreeNode parent, TypeSL typeFile)
        {
            if (parent == null)
            {
                return;
            }

            var strName = typeFile + "{0}";
            var strExtension = "";
            switch (typeFile)
            {
                case TypeSL.Script:
                    strExtension = ".lsl";
                    break;
                case TypeSL.LSLIScript:
                    strExtension = ".lsli";
                    break;
                case TypeSL.Notecard:
                    strExtension = ".txt";
                    break;
                case TypeSL.Texture:
                    strExtension = ".tga";
                    break;
                case TypeSL.Animation:
                    strExtension = ".ani";
                    break;
                case TypeSL.Gesture:
                    strExtension = ".ges";
                    break;
                case TypeSL.Landmark:
                    strExtension = ".lmrk";
                    break;
                case TypeSL.Snapshot:
                    strExtension = ".pic";
                    break;
                case TypeSL.Sound:
                    strExtension = ".snd";
                    break;
                default:
                    break;
            }
            var strNewName = this.GetNewFileName(parent, strName + strExtension);

            var strFilePath = Path.Combine(this.GetFullPath(parent), strNewName);

            if (!File.Exists(strFilePath))
            {
                var sw = new StreamWriter(strFilePath);

                switch (typeFile)
                {
                    case TypeSL.Script:
                        sw.Write(AutoFormatter.ApplyFormatting(0, Helpers.GetTemplate.Source()));
                        break;
                    case TypeSL.LSLIScript:
                        sw.Write(AutoFormatter.ApplyFormatting(0, Helpers.GetTemplate.Source()));
                        break;
                    case TypeSL.Notecard:
                        sw.Write("notecard");
                        break;
                    default:
                        // empty file
                        break;
                }
                sw.Close();
            }

            var newFile = new TreeNode(strNewName, (int)typeFile, (int)typeFile)
            {
                Tag = new RealTag(typeFile, strNewName, Guid.NewGuid())
            };
            parent.Nodes.Add(newFile);
            parent.Expand();

            this.m_dirty = true;
        }

        private void AddExistingFile(TreeNode parent, string strName, Guid guid)
        {
            if (parent == null)
            {
                return;
            }

            var strFilePath = Path.Combine(this.GetFullPath(parent), strName);

            var typeFile = TypeSL.Unknown;

            switch (Path.GetExtension(strName))
            {
                case ".lsl":
                    typeFile = TypeSL.Script;
                    break;
                case ".txt":
                    typeFile = TypeSL.Notecard;
                    break;
                case ".ogg":
                case ".wav":
                case ".mp3":
                    typeFile = TypeSL.Sound;
                    break;
                case ".ani":
                    typeFile = TypeSL.Animation;
                    break;
                case ".gif":
                case ".jpg":
                case ".jpeg":
                case ".tiff":
                case ".bmp":
                case ".png":
                    typeFile = TypeSL.Snapshot;
                    break;
                default:
                    break;
            }
            var newFile = new TreeNode(strName, (int)typeFile, (int)typeFile)
            {
                Tag = new RealTag(typeFile, strName, guid)
            };
            parent.Nodes.Add(newFile);
            parent.Expand();

            this.m_dirty = true;
        }

        private void AddExistingFile(TreeNode tn)
        {
            if (this.GetTypeSL(tn) != TypeSL.Object)
            {
                return;
            }

            this.openFileDialog1.FileName = "";
            this.openFileDialog1.Filter = "All Files (*.*)|*.*";
            this.openFileDialog1.Multiselect = true;
            if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var strSourcePath in this.openFileDialog1.FileNames)
                {
                    var strName = Path.GetFileName(strSourcePath);
                    var strDestinationPath = Path.Combine(this.GetFullPath(tn), strName);

                    var blnAdd = true;
                    foreach (TreeNode child in tn.Nodes)
                    {
                        if (child.Text == strName)
                        {
                            blnAdd = false;

                            if (string.Equals(strSourcePath, strDestinationPath, StringComparison.OrdinalIgnoreCase))
                            {
                                break;
                            }

                            if (MessageBox.Show("Overwrite existing file?", "File already exists", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                child.Remove();
                                blnAdd = true;
                                break;
                            }
                        }
                    }
                    if (!blnAdd)
                    {
                        continue;
                    }

                    if (!string.Equals(strSourcePath, strDestinationPath, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            File.Copy(strSourcePath, strDestinationPath, true);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    this.AddExistingFile(tn, strName, Guid.NewGuid());
                }
            }
        }

        public void AddNewObject()
        {
            if (this.ActiveProject == null)
            {
                return;
            }

            this.AddNewContainer(this.ActiveProject, TypeSL.Object, this.GetNewDirectoryName(this.ActiveProject, "Object{0}"), Guid.NewGuid());
        }

        #region ContextMenus

        private ToolStripMenuItem cmsItemsAdd(ContextMenuStrip cms, string strName, string strValue, string strKeys)
        {
            var tsmi = new ToolStripMenuItem(strValue)
            {
                ShortcutKeyDisplayString = strKeys,
                Name = strName
            };
            cms.Items.Add(tsmi);
            return tsmi;
        }

        private ToolStripMenuItem cmsItemsAdd(ContextMenuStrip cms, string strName, string strValue)
        {
            var tsmi = new ToolStripMenuItem(strValue)
            {
                Name = strName
            };
            cms.Items.Add(tsmi);
            return tsmi;
        }

        private ContextMenuStrip ContextMenuSolution()
        {
            var cms = new ContextMenuStrip();
            cms.MouseClick += this.cms_MouseClick;
            this.cmsItemsAdd(cms, "CloseSolution", "Close Solution");
            cms.Items.Add(new ToolStripSeparator());

            if (Properties.Settings.Default.VersionControlSVN)
            {
                if (this.Repository == null)
                {
                    this.cmsItemsAdd(cms, "SvnImport", "Import Solution SVN");
                    cms.Items.Add(new ToolStripSeparator());
                }
                else // already workingcopy
                {
                    this.cmsItemsAdd(cms, "SvnUpdate", "SVN update");
                    this.cmsItemsAdd(cms, "SvnCommit", "SVN commit");
                    cms.Items.Add(new ToolStripSeparator());
                }
            }

            this.cmsItemsAdd(cms, "AddNewProject", "Add New Project");
            this.cmsItemsAdd(cms, "AddExistingProject", "Add Existing Project...");
            if (!this.HasInventory())
            {
                cms.Items.Add(new ToolStripSeparator());
                this.cmsItemsAdd(cms, "AddInventory", "Add Empty Inventory");
            }
            cms.Items.Add(new ToolStripSeparator());
            if (this.GetTypeSL(this.CutObject) == TypeSL.Project || this.GetTypeSL(this.CopyObject) == TypeSL.Project)
            {
                this.cmsItemsAdd(cms, "Paste", "Paste project", "Ctrl+V");
            }

            this.cmsItemsAdd(cms, "Rename", "Rename", "F2");
            cms.Items.Add(new ToolStripSeparator());
            this.cmsItemsAdd(cms, "Properties", "Properties");
            return cms;
        }

        private ContextMenuStrip ContextMenuInventory(TypeSL typeSL)
        {
            var cms = new ContextMenuStrip();
            cms.MouseClick += this.cms_MouseClick;

            if (typeSL != TypeSL.Inventory)
            {
                var tsmiClothes = this.cmsItemsAdd(cms, "Add", "Add Clothes");
                tsmiClothes.DropDownItemClicked += this.tsmi_DropDownItemClicked;
                foreach (var file in this.ClothesTypes)
                {
                    tsmiClothes.DropDownItems.Add(file.ToString(), this.imageList1.Images[(int)file]);
                }

                var tsmiBodyParts = this.cmsItemsAdd(cms, "Add", "Add Body Parts");
                tsmiBodyParts.DropDownItemClicked += this.tsmi_DropDownItemClicked;
                foreach (var file in this.BodyPartsTypes)
                {
                    tsmiBodyParts.DropDownItems.Add(file.ToString(), this.imageList1.Images[(int)file]);
                }

                var tsmiFiles = this.cmsItemsAdd(cms, "Add", "Add Items");
                tsmiFiles.DropDownItemClicked += this.tsmi_DropDownItemClicked;
                foreach (var file in this.FilesTypes)
                {
                    tsmiFiles.DropDownItems.Add(file.ToString(), this.imageList1.Images[(int)file]);
                }

                cms.Items.Add(new ToolStripSeparator());
            }
            this.cmsItemsAdd(cms, "AddNewFolder", "Add New Folder");

            if (typeSL == TypeSL.Folder)
            {
                cms.Items.Add(new ToolStripSeparator());
                this.cmsItemsAdd(cms, "Cut", "Cut", "Ctrl+X");
                this.cmsItemsAdd(cms, "Copy", "Copy", "Ctrl+C");
                this.cmsItemsAdd(cms, "Delete", "Delete", "Del");
                this.cmsItemsAdd(cms, "Rename", "Rename", "F2");
            }
            if (this.CutObject != null || this.CopyObject != null)
            {
                cms.Items.Add(new ToolStripSeparator());
                this.cmsItemsAdd(cms, "Paste", "Paste", "Ctrl+V");
            }
            cms.Items.Add(new ToolStripSeparator());
            this.cmsItemsAdd(cms, "Properties", "Properties");

            return cms;
        }

        private ContextMenuStrip ContextMenuProject()
        {
            var cms = new ContextMenuStrip();
            cms.MouseClick += this.cms_MouseClick;
            this.cmsItemsAdd(cms, "AddNewObject", "Add New Object");
            this.cmsItemsAdd(cms, "AddExistingObject", "Add Existing Object...");
            cms.Items.Add(new ToolStripSeparator());
            if (this.NumberOfProjects() > 1 || this.ActiveProject == null)
            {
                this.cmsItemsAdd(cms, "SetAsActiveProject", "Set as Active Project");
            }

            this.cmsItemsAdd(cms, "DebugStart", "Debug Start");
            cms.Items.Add(new ToolStripSeparator());
            this.cmsItemsAdd(cms, "Cut", "Cut", "Ctrl+X");
            this.cmsItemsAdd(cms, "Copy", "Copy", "Ctrl+C");
            this.cmsItemsAdd(cms, "Remove", "Remove");
            if (this.GetTypeSL(this.CutObject) == TypeSL.Object || this.GetTypeSL(this.CopyObject) == TypeSL.Object)
            {
                this.cmsItemsAdd(cms, "Paste", "Paste Object", "Ctrl+V");
            }

            this.cmsItemsAdd(cms, "Rename", "Rename", "F2");
            cms.Items.Add(new ToolStripSeparator());
            this.cmsItemsAdd(cms, "Properties", "Properties");
            return cms;
        }

        public void CreateNewFileDrowDownMenu(ToolStripMenuItem tsmi)
        {
            tsmi.DropDownItemClicked += this.tsmi_DropDownItemClicked;
            foreach (var file in this.FilesTypes)
            {
                tsmi.DropDownItems.Add(file.ToString(), this.imageList1.Images[(int)file]);
            }
        }

        public void CreateNewFileDrowDownMenu(ContextMenuStrip cms)
        {
            var tsmi = this.cmsItemsAdd(cms, "Add", "Add New Item");
            tsmi.DropDownItemClicked += this.tsmi_DropDownItemClicked;
            foreach (var file in this.FilesTypes)
            {
                tsmi.DropDownItems.Add(file.ToString(), this.imageList1.Images[(int)file]);
            }
        }

        private ContextMenuStrip ContextMenuBox()
        {
            var cms = new ContextMenuStrip();
            cms.MouseClick += this.cms_MouseClick;

            this.CreateNewFileDrowDownMenu(cms);
            //ToolStripMenuItem tsmi = cmsItemsAdd(cms, "Add", "Add New File");
            //tsmi.DropDownItemClicked += new ToolStripItemClickedEventHandler(tsmi_DropDownItemClicked);
            //foreach (TypeSL file in FilesTypes)
            //	tsmi.DropDownItems.Add(file.ToString(), this.imageList1.Images[(int)file]);

            this.cmsItemsAdd(cms, "AddExistingFile", "Add Existing File(s)...");
            cms.Items.Add(new ToolStripSeparator());
            this.cmsItemsAdd(cms, "SetAsActiveBox", "Set As Active Object");
            cms.Items.Add(new ToolStripSeparator());
            this.cmsItemsAdd(cms, "ExcludeFromProject", "Exclude From Project");
            cms.Items.Add(new ToolStripSeparator());
            this.cmsItemsAdd(cms, "Cut", "Cut", "Ctrl+X");
            this.cmsItemsAdd(cms, "Copy", "Copy", "Ctrl+C");
            this.cmsItemsAdd(cms, "Delete", "Delete", "Del");
            if (this.IsItem(this.CutObject) || this.IsItem(this.CopyObject))
            {
                this.cmsItemsAdd(cms, "Paste", "Paste", "Ctrl+V");
            }

            this.cmsItemsAdd(cms, "Rename", "Rename", "F2");
            cms.Items.Add(new ToolStripSeparator());
            this.cmsItemsAdd(cms, "Properties", "Properties");
            return cms;
        }

        private ContextMenuStrip ContextMenuFile()
        {
            var cms = new ContextMenuStrip();
            cms.MouseClick += this.cms_MouseClick;
            this.cmsItemsAdd(cms, "Open", "Open");
            cms.Items.Add(new ToolStripSeparator());
            this.cmsItemsAdd(cms, "Cut", "Cut", "Ctrl+X");
            this.cmsItemsAdd(cms, "Copy", "Copy", "Ctrl+C");
            this.cmsItemsAdd(cms, "Delete", "Delete", "Del");
            this.cmsItemsAdd(cms, "Rename", "Rename", "F2");
            cms.Items.Add(new ToolStripSeparator());
            var tsmi = this.cmsItemsAdd(cms, "ChangeFileType", "Change filetype");
            this.CreateNewFileDrowDownMenu(tsmi);
            cms.Items.Add(new ToolStripSeparator());
            this.cmsItemsAdd(cms, "Properties", "Properties");
            return cms;
        }

        #endregion

        private void tsmi_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Add New File
            TreeNode tn;
            var tsddi = sender as ToolStripDropDownItem;
            var cms = tsddi.Owner as ContextMenuStrip;

            tn = cms == null ? this.ActiveObject : cms.Tag as TreeNode;

            if (tn != null)
            {
                var strName = tsddi.Name;
                if (strName == "ChangeFileType")
                {
                    var realTag = (RealTag)tn.Tag;
                    var stypeSl = (TypeSL)Enum.Parse(typeof(TypeSL), e.ClickedItem.Text);
                    realTag.ItemType = stypeSl;
                    tn.Tag = realTag;
                    tn.ImageIndex = (int)stypeSl;
                    tn.SelectedImageIndex = (int)stypeSl;
                    this.treeView1.Invalidate(tn.Bounds);
                }
                else
                {
                    var typeSL = (TypeSL)Enum.Parse(typeof(TypeSL), e.ClickedItem.Text);
                    if (typeSL != TypeSL.Object)
                    {
                        this.AddNewFile(tn, typeSL);
                    }
                    else
                    {
                        this.AddNewContainer(tn, TypeSL.Object, this.GetNewDirectoryName(tn, "Object{0}"), Guid.NewGuid());
                    }
                }
            }
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            var treeNode = this.treeView1.GetNodeAt(new Point(e.X, e.Y));
            if (treeNode == null)
            {
                return;
            }

            if (e.X < treeNode.Bounds.X) // only when clicked on the label
            {
                return;
            }

            if (this.timer.Tag != null)
            {
                if (this.timer.Tag == treeNode)
                {
                    this.timer.Tag = null;
                    this.timer.Start();
                    return;
                }
            }
            this.timer.Tag = treeNode;
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            var treeNode = this.treeView1.GetNodeAt(new Point(e.X, e.Y));
            if (treeNode == null)
            {
                return;
            }

            this.treeView1.SelectedNode = treeNode;

            var realTag = (RealTag)treeNode.Tag;

            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip cms = null;
                switch (realTag.ItemType)
                {
                    case TypeSL.Solution:
                        cms = this.ContextMenuSolution();
                        break;
                    case TypeSL.Project:
                        cms = this.ContextMenuProject();
                        break;
                    case TypeSL.Object:
                        cms = this.ContextMenuBox();
                        break;
                    default:
                        if (this.IsInventoryContainer(treeNode))
                        {
                            cms = this.ContextMenuInventory(realTag.ItemType);
                        }
                        else if (this.IsItem(treeNode))
                        {
                            cms = this.ContextMenuFile();
                        }
                        else
                        {
                            cms = new ContextMenuStrip();
                        }

                        break;
                }
                cms.Tag = treeNode;
                this.treeView1.ContextMenuStrip = cms;
            }
        }

        private string GetNewName(TreeNode treeNode, bool blnDirectory, string strFormat)
        {
            var strDirectory = this.GetFullPath(treeNode);
            if (File.Exists(strDirectory))
            {
                strDirectory = Path.GetDirectoryName(strDirectory);
            }

            if (!Directory.Exists(strDirectory))
            {
                Directory.CreateDirectory(strDirectory);
            }

            var realTag = (RealTag)treeNode.Tag;
            if (blnDirectory)
            {
                var strName = strFormat.Replace("({0}) ", "").Replace("{0}", "").Replace("Copy of ", "");
                if (!Directory.Exists(Path.Combine(strDirectory, strName)))
                {
                    return strName;
                }

                strName = strFormat.Replace("({0}) ", "").Replace("{0}", "");
                if (!Directory.Exists(Path.Combine(strDirectory, strName)))
                {
                    return strName;
                }

                for (var intI = 1; intI < 255; intI++)
                {
                    strName = string.Format(strFormat, intI);
                    if (!Directory.Exists(Path.Combine(strDirectory, strName)))
                    {
                        return strName;
                    }
                }
            }
            else
            {
                var strName = strFormat.Replace("({0}) ", "").Replace("{0}", "").Replace("Copy of ", "");
                if (!File.Exists(Path.Combine(strDirectory, strName)))
                {
                    return strName;
                }

                strName = strFormat.Replace("({0}) ", "").Replace("{0}", "");
                if (!File.Exists(Path.Combine(strDirectory, strName)))
                {
                    return strName;
                }

                for (var intI = 2; intI < 255; intI++)
                {
                    strName = string.Format(strFormat, intI);
                    if (!File.Exists(Path.Combine(strDirectory, strName)))
                    {
                        return strName;
                    }
                }
            }
            MessageBox.Show("There are 255 items with the same name", "Oops...");
            return null;
        }

        private string GetNewFileName(TreeNode treeNode, string strFormat)
        {
            return this.GetNewName(treeNode, false, strFormat);
        }

        private string GetNewDirectoryName(TreeNode treeNode, string strFormat)
        {
            return this.GetNewName(treeNode, true, strFormat);
        }

        private void DeleteAction(TreeNode tn)
        {
            if (MessageBox.Show("Delete " + tn.Text, "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.DeleteChildren(tn);
            }
        }

        private void DeleteChildren(TreeNode tn)
        {
            // remove backwards, foreach does not work
            for (var intI = tn.Nodes.Count - 1; intI >= 0; intI--)
            {
                this.DeleteChildren(tn.Nodes[intI]);
            }

            var strPath = this.GetFullPath(tn);
            if (File.Exists(strPath))
            {
                File.Delete(strPath);
                var rt = (RealTag)tn.Tag;
                var editForm = this.GetEditForm(rt.Guid);
                if (editForm != null)
                {
                    this.parent.ActivateMdiForm(editForm);
                    this.parent.CloseActiveWindow();
                }
            }
            else if (Directory.Exists(strPath))
            {
                if (Directory.GetFiles(strPath).Length == 0)
                {
                    Directory.Delete(strPath, true);
                }
            }
            else
            {
                MessageBox.Show("Not found: " + tn.Text, "Oops...");
            }
            tn.Remove();

            this.m_dirty = true;
        }

        private void CopyChildren(TreeNode Source, TreeNode Destination)
        {
            var realTag = (RealTag)Source.Tag;
            realTag.Guid = Guid.NewGuid(); // copy objects have new guid
            if (realTag.Path != null)
            {
                realTag.Path = Path.Combine(Path.GetDirectoryName(this.GetFullPath(Source)), Destination.Text);
            }

            Destination.Tag = realTag;
            Destination.ImageIndex = Source.ImageIndex;
            Destination.SelectedImageIndex = Source.SelectedImageIndex;

            var strSourcePath = this.GetFullPath(Source);
            var strDestionationPath = this.GetFullPath(Destination);
            if (File.Exists(strSourcePath))
            {
                File.Copy(strSourcePath, strDestionationPath);
            }
            else if (Directory.Exists(strSourcePath))
            {
                Directory.CreateDirectory(strDestionationPath);
            }
            else
            {
                MessageBox.Show("Error: " + strSourcePath, "Oops...");
            }

            foreach (TreeNode child in Source.Nodes)
            {
                var clone = new TreeNode(child.Text);
                Destination.Nodes.Add(clone);
                this.CopyChildren(child, clone);
            }
            this.m_dirty = true;
        }

        private void PasteAction(TreeNode tn)
        {
            // paste on same object, get parent!!
            // paste on item, get parent!!
            if (tn == this.CopyObject || tn == this.CutObject || this.IsItemAndNotObject(tn))
            {
                tn = tn.Parent;
            }

            // sanitycheck, search all parents, may not be CopyObject or CutObject
            var parentNode = tn;
            while (parentNode != null)
            {
                if (parentNode == this.CopyObject || parentNode == this.CutObject)
                {
                    return;
                }

                parentNode = parentNode.Parent;
            }

            switch (this.GetTypeSL(tn))
            {
                case TypeSL.Solution:
                    if (this.GetTypeSL(this.CutObject) != TypeSL.Project
                     && this.GetTypeSL(this.CopyObject) != TypeSL.Project)
                    {
                        return;
                    }

                    break;
                case TypeSL.Project:
                    if (this.GetTypeSL(this.CutObject) != TypeSL.Object
                     && this.GetTypeSL(this.CopyObject) != TypeSL.Object)
                    {
                        return;
                    }

                    break;
                case TypeSL.Object:
                    if (!this.IsItem(this.CutObject) && !this.IsItem(this.CopyObject))
                    {
                        return;
                    }

                    break;
                default:
                    if (this.IsInventoryContainer(tn))
                    {
                        if (!this.IsItem(this.CutObject) && !this.IsItem(this.CopyObject)
                          && this.GetTypeSL(this.CutObject) != TypeSL.Folder
                          && this.GetTypeSL(this.CopyObject) != TypeSL.Folder)
                        {
                            return;
                        }
                    }
                    else if (!this.IsItem(tn)) // is special container
                    {
                        // must be a file
                        if (!this.IsItem(this.CutObject) && !this.IsItem(this.CopyObject)
                          && this.GetTypeSL(this.CutObject) != TypeSL.Folder
                          && this.GetTypeSL(this.CopyObject) != TypeSL.Folder)
                        {
                            return;
                        }
                    }
                    break;
            }

            if (this.CutObject != null) // Paste the Cut object
            {
                var strName = this.CutObject.Text;

                foreach (TreeNode treeNode in tn.Nodes)
                {
                    if (treeNode.Text == strName)
                    {
                        MessageBox.Show("Destination already exists", "Oops...");
                        return;
                    }
                }

                var Destination = new TreeNode(strName);
                tn.Nodes.Add(Destination);

                this.CopyChildren(this.CutObject, Destination);
                this.DeleteChildren(this.CutObject);

                this.CutObject.Remove();
                this.CutObject = null;
            }
            if (this.CopyObject != null) // Paste the Copy object
            {
                var strName = this.IsItem(this.CopyObject) && this.GetTypeSL(this.CopyObject) != TypeSL.Object
                    ? this.GetNewFileName(tn, "Copy ({0}) of " + this.CopyObject.Text)
                    : this.GetNewDirectoryName(tn, "Copy ({0}) of " + this.CopyObject.Text);
                var Destination = new TreeNode(strName);
                tn.Nodes.Add(Destination);

                this.CopyChildren(this.CopyObject, Destination);

                this.CopyObject = null;

                if (tn == this.ActiveSolution)
                {
                    this.UpdateVisualSolutionName();
                }
            }
        }

        private void RemoveAction(TreeNode tn)
        {
            if (this.GetTypeSL(tn) == TypeSL.Project)
            {
                this.SaveItemFile(tn); // saving pending updates!!
                if (this.ActiveProject == tn)
                {
                    this.ActiveProject = null;
                }
                // Does not!!!! delete, only remove form project
                tn.Remove();
                this.UpdateVisualSolutionName();
                this.m_dirty = true;
            }
        }

        private void RenameAction(TreeNode tn)
        {
            var realTag = (RealTag)tn.Tag;
            if (this.IsInventoryContainer(tn))
            {
                if (realTag.ItemType != TypeSL.Folder)
                {
                    return;
                }
            }

            if (realTag.ItemType == TypeSL.Solution)
            {
                tn.Text = this.NameSolution;
            }

            this.treeView1.LabelEdit = true;
            tn.BeginEdit();
        }

        private void CutAction(TreeNode tn)
        {
            if (this.CutObject != null)
            {
                this.CutObject.ForeColor = Color.Empty;
            }

            this.CutObject = tn;
            tn.ForeColor = Color.DarkGray;
        }

        private void CopyAction(TreeNode tn)
        {
            if (this.CutObject != null)
            {
                this.CutObject.ForeColor = Color.Empty;
                this.CutObject = null;
            }
            this.CopyObject = tn;
        }

        public void AddNewProjectAction()
        {
            var strNewProjectName = this.GetNewDirectoryName(this.ActiveSolution, "Project{0}");
            var strProjectPath = Path.Combine(this.DirectorySolution, strNewProjectName);
            this.AddItem(TypeSL.Project, strNewProjectName, Guid.NewGuid(), strProjectPath);
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.timer.Stop();
            this.timer.Tag = null;

            var guid = ((RealTag)e.Node.Tag).Guid;
            var path = this.GetFullPath(e.Node);

            // already opened
            var editForm = this.GetEditForm(guid);
            if (editForm != null)
            {
                if (!(editForm.Tag is TabPage tabPage))
                {
                    if (editForm.Visible)
                    {
                        editForm.Focus();
                    }
                    else
                    {
                        // Check if there's a related expanded lsl or lsli opened. If so, focus it. Else open the lsli.
                        var expandedForm = (EditForm)this.parent.GetForm(Helpers.LSLIPathHelper.GetExpandedTabName(Helpers.LSLIPathHelper.CreateExpandedScriptName(Path.GetFileName(path))));
                        var collapsedForm = (EditForm)this.parent.GetForm(Helpers.LSLIPathHelper.CreateCollapsedScriptName(Path.GetFileName(path)));
                        if (expandedForm?.Visible == true)
                        {
                            expandedForm.Focus();
                        }
                        else if (collapsedForm?.Visible == true)
                        {
                            collapsedForm.Focus();
                        }
                        else
                        {
                            // Open a new one
                            if (this.GetTypeSL(e.Node) == TypeSL.Script || this.GetTypeSL(e.Node) == TypeSL.LSLIScript)
                            {
                                this.parent.OpenFile(this.GetFullPath(e.Node), guid, true);
                            }
                        }
                    }
                }
                else
                {
                    if (tabPage.Parent is TabControl tabControl)
                    {
                        tabControl.SelectedTab = tabPage;
                    }
                }
                return;
            }

            // Check if it's an lsli that has an open expanded form
            if (this.GetTypeSL(e.Node) == TypeSL.Script || this.GetTypeSL(e.Node) == TypeSL.LSLIScript)
            {
                if (Helpers.LSLIPathHelper.IsLSLI(path))
                {
                    // Check if there's a related expanded lsl opened. If so, focus it. Else open the lsli.
                    var expandedForm = (EditForm)this.parent.GetForm(Helpers.LSLIPathHelper.GetExpandedTabName(Helpers.LSLIPathHelper.CreateExpandedScriptName(Path.GetFileName(path))));
                    var collapsedForm = (EditForm)this.parent.GetForm(Helpers.LSLIPathHelper.CreateCollapsedScriptName(Path.GetFileName(path)));
                    if (expandedForm?.Visible == true)
                    {
                        expandedForm.Focus();
                    }
                    else if (collapsedForm?.Visible == true)
                    {
                        collapsedForm.Focus();
                    }
                    else
                    {
                        // Open a new one
                        if (this.GetTypeSL(e.Node) == TypeSL.Script || this.GetTypeSL(e.Node) == TypeSL.LSLIScript)
                        {
                            this.parent.OpenFile(this.GetFullPath(e.Node), guid, true);
                        }
                    }
                }
                else
                {
                    // Open a new one
                    if (this.GetTypeSL(e.Node) == TypeSL.Script || this.GetTypeSL(e.Node) == TypeSL.LSLIScript)
                    {
                        this.parent.OpenFile(this.GetFullPath(e.Node), guid, true);
                    }
                }
            }

            if (this.GetTypeSL(e.Node) == TypeSL.Notecard)
            {
                this.parent.OpenFile(this.GetFullPath(e.Node), guid, false);
            }
        }

        private void PropertiesActions(TreeNode tn)
        {
            var realTag = (SolutionExplorer.RealTag)tn.Tag;
            var oldGuid = realTag.Guid;
            var prop = new GuidProperty(realTag);

            if (prop.ShowDialog(this) == DialogResult.OK)
            {
                var editForm = this.GetEditForm(oldGuid);
                if (editForm != null)
                {
                    editForm.guid = prop.guid;
                }

                realTag.Guid = prop.guid;
                tn.Tag = realTag;
                this.ShowProperties(tn);
            }
        }

        private void ShowProperties(TreeNode tn)
        {
            var realTag = (RealTag)tn.Tag;

            this.listView1.Items.Clear();

            var lvi = new ListViewItem("Name");
            lvi.SubItems.Add(tn.Text);
            this.listView1.Items.Add(lvi);

            if (realTag.Description != string.Empty)
            {
                lvi = new ListViewItem("Description");
                lvi.SubItems.Add(realTag.Description);
                this.listView1.Items.Add(lvi);
            }

            lvi = new ListViewItem("Guid");
            lvi.SubItems.Add(realTag.Guid.ToString());
            this.listView1.Items.Add(lvi);

            lvi = new ListViewItem("Path");
            lvi.SubItems.Add(this.GetFullPath(tn));
            this.listView1.Items.Add(lvi);

            lvi = new ListViewItem("Modified");
            var fi = new FileInfo(this.GetFullPath(tn));
            lvi.SubItems.Add(fi.LastWriteTime.ToString("s"));
            this.listView1.Items.Add(lvi);
        }

        private void SetAsActiveProject(TreeNode tn)
        {
            this.ActiveObject = null;
            this.ActiveProject = tn;
            foreach (TreeNode child in this.ActiveProject.Nodes)
            {
                if (this.GetTypeSL(child) == TypeSL.Object)
                {
                    this.ActiveObject = child;
                    break;
                }
            }
            this.treeView1.Invalidate();
        }

        private void SetAsActiveBox(TreeNode tn)
        {
            this.ActiveProject = null;
            this.ActiveObject = tn;
            if (this.GetTypeSL(this.ActiveObject.Parent) == TypeSL.Project)
            {
                this.ActiveProject = this.ActiveObject.Parent;
            }

            this.treeView1.Invalidate();
        }

        private void DebugStart()
        {
            MessageBox.Show("TODO: Debug start", "Todo");
        }

        private void AddExistingObject(TreeNode tn)
        {
            MessageBox.Show("TODO: Add Existing Object", "Todo");
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.ShowProperties(e.Node);
        }

        private TreeNode FindGuid(TreeNode treeNode, Guid guid)
        {
            if (treeNode == null)
            {
                return null;
            }

            foreach (TreeNode tn in treeNode.Nodes)
            {
                var rt = (RealTag)tn.Tag;
                if (rt.Guid == guid)
                {
                    return tn;
                }

                var found = this.FindGuid(tn, guid);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }

        private delegate bool IHateThisToo(Guid guid, string name);
        private delegate string IHateThis(Guid guid);

        public string GetObjectName(Guid guid)
        {
            if (this.treeView1.InvokeRequired)
            {
                return this.treeView1.Invoke(new IHateThis(this.GetObjectName), new object[] { guid }).ToString();
            }

            var treeNode = this.FindGuid(this.treeView1.TopNode, guid);
            if (treeNode == null)
            {
                return string.Empty;
            }

            var parent = treeNode.Parent;
            var realTag = (RealTag)parent.Tag;
            if (realTag.ItemType != TypeSL.Object)
            {
                return string.Empty;
            }

            return realTag.Name;
        }

        public bool SetObjectName(Guid guid, string name)
        {
            if (this.treeView1.InvokeRequired)
            {
                return (bool)this.treeView1.Invoke(new IHateThisToo(this.SetObjectName), new object[] { guid, name });
            }

            var treeNode = this.FindGuid(this.treeView1.TopNode, guid);
            if (treeNode == null)
            {
                return false;
            }

            var parent = treeNode.Parent;
            var realTag = (RealTag)parent.Tag;
            if (realTag.ItemType != TypeSL.Object)
            {
                return false;
            }

            var strBackup = parent.Text;
            var strSource = this.GetFullPath(parent);
            parent.Text = name;
            var strDestination = this.GetFullPath(parent);

            try
            {
                Directory.Move(strSource, strDestination);
                realTag.Name = name;
                parent.Tag = realTag;
                parent.Text = name;
                return true;
            }
            catch
            {
                parent.Text = strBackup;
                return false;
            }
        }

        public string GetObjectDescription(Guid guid)
        {
            if (this.treeView1.InvokeRequired)
            {
                return this.treeView1.Invoke(new IHateThis(this.GetObjectDescription), new object[] { guid }).ToString();
            }

            var treeNode = this.FindGuid(this.treeView1.TopNode, guid);
            if (treeNode == null)
            {
                return string.Empty;
            }

            var parent = treeNode.Parent;
            var realTag = (RealTag)parent.Tag;
            if (realTag.ItemType != TypeSL.Object)
            {
                return string.Empty;
            }

            return realTag.Description;
        }

        public bool SetObjectDescription(Guid guid, string description)
        {
            if (this.treeView1.InvokeRequired)
            {
                return (bool)this.treeView1.Invoke(new IHateThisToo(this.SetObjectDescription), new object[] { guid, description });
            }

            var treeNode = this.FindGuid(this.treeView1.TopNode, guid);
            if (treeNode == null)
            {
                return false;
            }

            var parent = treeNode.Parent;
            var realTag = (RealTag)parent.Tag;
            if (realTag.ItemType != TypeSL.Object)
            {
                return false;
            }

            realTag.Description = description;
            parent.Tag = realTag;
            return true;
        }

        public string GetScriptName(Guid guid)
        {
            if (this.treeView1.InvokeRequired)
            {
                return this.treeView1.Invoke(new IHateThis(this.GetScriptName), new object[] { guid }).ToString();
            }

            var treeNode = this.FindGuid(this.treeView1.TopNode, guid);
            if (treeNode == null)
            {
                return string.Empty;
            }

            return treeNode.Text;
        }

        public string GetKey(Guid guid)
        {
            if (this.treeView1.InvokeRequired)
            {
                return this.treeView1.Invoke(new IHateThis(this.GetKey), new object[] { guid }).ToString();
            }

            var treeNode = this.FindGuid(this.treeView1.TopNode, guid);
            if (treeNode == null)
            {
                return string.Empty;
            }

            var parent = treeNode.Parent;
            var rt = (RealTag)parent.Tag;
            return rt.Guid.ToString();
        }

        private TypeSL GetTypeSLFromInteger(int type)
        {
            switch (type)
            {
                case -1:                    //INVENTORY_ALL -1  all inventory items
                    return TypeSL.Unknown;
                case 20:                    //INVENTORY_ANIMATION  20  animations  0x14  
                    return TypeSL.Animation;
                case 13:                    //INVENTORY_BODYPART  13  body parts  0x0D  
                    return TypeSL.Hair | TypeSL.Skin | TypeSL.Eyes | TypeSL.Shape;
                case 5:                     //INVENTORY_CLOTHING  5  clothing  0x05  
                    return TypeSL.Jacket | TypeSL.Gloves | TypeSL.Pants | TypeSL.Shirt | TypeSL.Shoes | TypeSL.Skirt | TypeSL.Socks | TypeSL.Underpants | TypeSL.Undershirt;
                case 21:                    //INVENTORY_GESTURE  21  gestures  0x15  
                    return TypeSL.Gesture;
                case 3:                     //INVENTORY_LANDMARK  3  landmarks  0x03  
                    return TypeSL.Landmark;
                case 7:                     //INVENTORY_NOTECARD  7  notecards  0x07
                    return TypeSL.Notecard;
                case 6:                     //INVENTORY_OBJECT  6  objects  0x06  
                    return TypeSL.Object;
                case 10:                    //INVENTORY_SCRIPT  10  scripts  0x0A  
                    return TypeSL.Script;
                case 1:                     //INVENTORY_SOUND  1  sounds  0x01  
                    return TypeSL.Sound;
                case 0:                     //INVENTORY_TEXTURE  0  textures  0x00  
                    return TypeSL.Texture;
                default:
                    return TypeSL.Unknown;
            }
        }

        private delegate void DelegateRemoveInventory(Guid guid, SecondLife.String name);

        public void RemoveInventory(Guid guid, SecondLife.String name)
        {
            if (this.treeView1.InvokeRequired)
            {
                this.treeView1.Invoke(new DelegateRemoveInventory(this.RemoveInventory), new object[] { guid, name });
                return;
            }

            var treeNode = this.FindGuid(this.treeView1.TopNode, guid);
            if (treeNode == null)
            {
                return;
            }

            var parent = treeNode.Parent;
            for (var i = 0; i < parent.Nodes.Count; ++i)
            {
                if (parent.Nodes[i].Text == name)
                {
                    parent.Nodes.RemoveAt(i);
                    return;
                }
            }
        }

        private delegate string DelegateGetInventoryName(Guid guid, int type, int number);

        public string GetInventoryName(Guid guid, int type, int number)
        {
            if (this.treeView1.InvokeRequired)
            {
                return this.treeView1.Invoke(new DelegateGetInventoryName(this.GetInventoryName), new object[] { guid, type, number }).ToString();
            }

            var typeSL = this.GetTypeSLFromInteger(type);

            var treeNode = this.FindGuid(this.treeView1.TopNode, guid);
            if (treeNode == null)
            {
                return string.Empty;
            }

            var parent = treeNode.Parent;
            var intI = 0;
            foreach (TreeNode tn in parent.Nodes)
            {
                if (typeSL == TypeSL.Unknown || (typeSL & this.GetTypeSL(tn)) == this.GetTypeSL(tn))
                {
                    if (intI == number)
                    {
                        return tn.Text;
                    }

                    intI++;
                }
            }
            return string.Empty;
        }

        private delegate string DelegateGetInventoryKey(Guid guid, string name);

        public string GetInventoryKey(Guid guid, string name)
        {
            if (this.treeView1.InvokeRequired)
            {
                return this.treeView1.Invoke(new DelegateGetInventoryKey(this.GetInventoryKey), new object[] { guid, name }).ToString();
            }

            var treeNode = this.FindGuid(this.treeView1.TopNode, guid);
            if (treeNode == null)
            {
                return string.Empty;
            }

            var parent = treeNode.Parent;
            foreach (TreeNode tn in parent.Nodes)
            {
                if (tn.Text == name)
                {
                    var rt = (RealTag)tn.Tag;
                    return rt.Guid.ToString();
                }
            }
            return string.Empty;
        }

        private delegate int DelegateGetInventoryType(Guid guid, string name);

        public int GetInventoryType(Guid guid, string name)
        {
            if (this.treeView1.InvokeRequired)
            {
                return (int)this.treeView1.Invoke(new DelegateGetInventoryType(this.GetInventoryType), new object[] { guid, name });
            }

            var treeNode = this.FindGuid(this.treeView1.TopNode, guid);
            if (treeNode == null)
            {
                return SecondLife.INVENTORY_NONE;
            }

            var parent = treeNode.Parent;
            foreach (TreeNode tn in parent.Nodes)
            {
                if (tn.Text == name)
                {
                    var rt = (RealTag)tn.Tag;
                    switch (rt.ItemType)
                    {
                        case TypeSL.Texture:
                            return SecondLife.INVENTORY_TEXTURE;
                        case TypeSL.Sound:
                            return SecondLife.INVENTORY_SOUND;
                        case TypeSL.Landmark:
                            return SecondLife.INVENTORY_LANDMARK;
                        case TypeSL.Gloves:
                        case TypeSL.Jacket:
                        case TypeSL.Pants:
                        case TypeSL.Shirt:
                        case TypeSL.Shoes:
                        case TypeSL.Skirt:
                        case TypeSL.Socks:
                        case TypeSL.Underpants:
                        case TypeSL.Undershirt:
                            return SecondLife.INVENTORY_CLOTHING;
                        case TypeSL.Object:
                            return SecondLife.INVENTORY_OBJECT;
                        case TypeSL.Script:
                            return SecondLife.INVENTORY_SCRIPT;
                        case TypeSL.Animation:
                            return SecondLife.INVENTORY_ANIMATION;
                        case TypeSL.Eyes:
                        case TypeSL.Hair:
                        case TypeSL.Shape:
                        case TypeSL.Skin:
                            return SecondLife.INVENTORY_BODYPART;
                        case TypeSL.Gesture:
                            return SecondLife.INVENTORY_GESTURE;
                        case TypeSL.Notecard:
                            return SecondLife.INVENTORY_NOTECARD;
                        default:
                            return SecondLife.INVENTORY_NONE;
                    }
                }
            }
            return SecondLife.INVENTORY_NONE;
        }

        private delegate int DelegateGetInventoryNumber(Guid guid, int type);

        public int GetInventoryNumber(Guid guid, int type)
        {
            if (this.treeView1.InvokeRequired)
            {
                return (int)this.treeView1.Invoke(new DelegateGetInventoryNumber(this.GetInventoryNumber), new object[] { guid, type });
            }

            var typeSL = this.GetTypeSLFromInteger(type);

            var treeNode = this.FindGuid(this.treeView1.TopNode, guid);
            if (treeNode == null)
            {
                return -1;
            }

            var parent = treeNode.Parent;
            var intI = 0;
            foreach (TreeNode tn in parent.Nodes)
            {
                if (typeSL == TypeSL.Unknown || (typeSL & this.GetTypeSL(tn)) == this.GetTypeSL(tn))
                {
                    intI++;
                }
            }
            return intI;
        }

        private delegate string DelegateGetPath(Guid guid, string name);

        public string GetPath(Guid guid, string name)
        {
            if (this.treeView1.InvokeRequired)
            {
                return this.treeView1.Invoke(new DelegateGetPath(this.GetPath), new object[] { guid, name }).ToString();
            }

            var treeNode = this.FindGuid(this.treeView1.TopNode, guid);
            if (treeNode == null)
            {
                return string.Empty;
            }

            var parent = treeNode.Parent;
            foreach (TreeNode tn in parent.Nodes)
            {
                if (tn.Text == name || ((RealTag)tn.Tag).Guid.ToString() == name)
                {
                    return this.GetFullPath(tn);
                }
            }
            return string.Empty;
        }

        private delegate Guid GetParentGuidDelegate(Guid guid);

        public Guid GetParentGuid(Guid guid)
        {
            if (this.treeView1.InvokeRequired)
            {
                return (Guid)this.treeView1.Invoke(new GetParentGuidDelegate(this.GetParentGuid), new object[] { guid });
            }
            var treeNode = this.FindGuid(this.treeView1.TopNode, guid);
            if (treeNode == null)
            {
                return Guid.Empty;
            }

            var parent = treeNode.Parent;
            // Only return parent when it is an object
            if (((RealTag)parent.Tag).ItemType != TypeSL.Object)
            {
                return guid;
            }
            else // Return object itself, has no parent object
            {
                return ((RealTag)parent.Tag).Guid;
            }
        }

        private delegate Guid GetGuidFromObjectNrDelegate(Guid ObjectGuid, int intLinkNum);

        public Guid GetGuidFromObjectNr(Guid ObjectGuid, int intLinkNum)
        {
            if (this.treeView1.InvokeRequired)
            {
                return (Guid)this.treeView1.Invoke(new GetGuidFromObjectNrDelegate(this.GetGuidFromObjectNr), new object[] { ObjectGuid, intLinkNum });
            }
            var treeNode = this.FindGuid(this.treeView1.TopNode, ObjectGuid);
            if (treeNode == null)
            {
                return Guid.Empty;
            }

            var intNr = 0;
            foreach (TreeNode tn in treeNode.Nodes)
            {
                var realTag = (RealTag)tn.Tag;
                if (realTag.ItemType == TypeSL.Object)
                {
                    intNr++;
                    if (intNr == intLinkNum)
                    {
                        return realTag.Guid;
                    }
                }
            }
            return Guid.Empty; // no objects found
        }

        private void GetScriptsFromNode(TreeNode treeNode, List<Guid> list, bool recursive)
        {
            foreach (TreeNode tn in treeNode.Nodes)
            {
                var realTag = (RealTag)tn.Tag;
                if (realTag.ItemType == TypeSL.Script)
                {
                    list.Add(realTag.Guid);
                }
                else
                    if (realTag.ItemType == TypeSL.Object && recursive)
                {
                    this.GetScriptsFromNode(tn, list, recursive);
                }
            }
        }

        private delegate List<Guid> GetScriptsDelegate(Guid guid, bool recursive);

        public List<Guid> GetScripts(Guid guid, bool recursive)
        {
            if (this.treeView1.InvokeRequired)
            {
                return this.treeView1.Invoke(new GetScriptsDelegate(this.GetScripts), new object[] { guid, recursive }) as List<Guid>;
            }

            var list = new List<Guid>();
            var treeNode = this.FindGuid(this.treeView1.TopNode, guid);
            if (treeNode == null)
            {
                return list; // empty
            }

            this.GetScriptsFromNode(treeNode, list, recursive);
            return list;
        }

        private void listView1_MouseMove(object sender, MouseEventArgs e)
        {
            var lvi = this.listView1.GetItemAt(e.X, e.Y);
            if (lvi == null)
            {
                this.toolTip1.SetToolTip(this.listView1, "");
            }
            else if (lvi != this.m_HoveredItem)
            {
                this.m_HoveredItem = lvi;
                this.toolTip1.SetToolTip(this.listView1, lvi.SubItems[1].Text);
            }
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.treeView1.SelectedNode == null)
            {
                return;
            }

            if (e.KeyCode == Keys.ControlKey)
            {
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.F2:
                    this.RenameAction(this.treeView1.SelectedNode);
                    e.Handled = true;
                    break;
                case Keys.Delete:
                    this.DeleteAction(this.treeView1.SelectedNode);
                    e.Handled = true;
                    break;
                case Keys.C:
                    if (e.Control)
                    {
                        this.CopyAction(this.treeView1.SelectedNode);
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                    }
                    break;
                case Keys.V:
                    if (e.Control)
                    {
                        this.PasteAction(this.treeView1.SelectedNode);
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                    }
                    break;
                case Keys.X:
                    if (e.Control)
                    {
                        this.CutAction(this.treeView1.SelectedNode);
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                    }
                    break;
                default:
                    break;
            }
        }

        private void SvnImport()
        {
            var svnArguments = new SvnAguments
            {
                Text = "SVN import",
                Comment = "Initial import"
            };
            if (svnArguments.ShowDialog(this) == DialogResult.OK)
            {
                var strRepository = svnArguments.Repository;
                var strMessage = svnArguments.Comment;

                strMessage = strMessage.Replace('"', '\'');

                var strNameOfDirectory = Path.GetFileName(this.DirectorySolution);

                if (!strRepository.EndsWith(strNameOfDirectory))
                {
                    if (!strRepository.EndsWith("/"))
                    {
                        strRepository += "/";
                    }

                    strRepository += strNameOfDirectory;
                }

                this.Repository = strRepository;
                this.SaveSolutionFile();

                // import "c:\temp\a\b \d\dir" "file:///d:/temp/svn/...." -m "Initital "
                var strArguments = string.Format("import \"{0}\" \"{1}\" -m \"{2}\"", this.DirectorySolution, this.Repository, strMessage);
                var svn = new Svn();
                if (!svn.Execute(strArguments, false, true))
                {
                    return;
                }

                try
                {
                    Directory.Move(this.DirectorySolution, this.DirectorySolution + ".bak");
                    svn.Execute("checkout \"" + this.Repository + "\" \"" + this.DirectorySolution + "\"", false, true);
                }
                catch // (Exception exception)
                {
                    MessageBox.Show("Can't rename Directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SvnUpdate()
        {
            var svn = new Svn();
            svn.Execute("update \"" + this.DirectorySolution + "\"", true, true);
        }

        private void SvnCommit()
        {
            var svnArguments = new SvnAguments
            {
                Text = "SVN Commit",
                ReadOnly = true,
                Repository = this.Repository,
                Comment = ""
            };
            if (svnArguments.ShowDialog(this) == DialogResult.OK)
            {
                var svn = new Svn();
                svn.Execute("commit \"" + this.DirectorySolution + "\" -m \"" + svnArguments.Comment + "\"", true, true);
            }
        }

        private void cms_MouseClick(object sender, MouseEventArgs e)
        {
            var cms = sender as ContextMenuStrip;
            var tn = cms.Tag as TreeNode;

            var tsi = cms.GetItemAt(e.X, e.Y) as ToolStripMenuItem;
            if (tsi == null)
            {
                return; // TODO, why is this??
            }

            if (tsi.DropDownItems.Count == 0)
            {
                cms.Visible = false;
            }

            this.treeView1.Invalidate(tn.Bounds);

            switch (tsi.Name)
            {
                case "SvnImport":
                    this.SvnImport();
                    break;
                case "SvnUpdate":
                    this.SvnUpdate();
                    break;
                case "SvnCommit":
                    this.SvnCommit();
                    break;
                case "CloseSolution":
                    this.CloseSolution();
                    break;
                case "AddExistingFile":
                    this.AddExistingFile(tn);
                    break;
                case "AddExistingProject":
                    this.AddExistingProject();
                    break;
                case "AddNewProject":
                    this.AddNewProjectAction();
                    break;
                case "AddNewObject":
                    this.AddNewContainer(tn, TypeSL.Object, this.GetNewDirectoryName(tn, "Object{0}"), Guid.NewGuid());
                    break;
                case "AddNewFolder":
                    this.AddNewContainer(tn, TypeSL.Folder, this.GetNewDirectoryName(tn, "Folder{0}"), Guid.NewGuid());
                    break;
                case "AddInventory":
                    this.EmptyInventory(tn);
                    break;
                case "SetAsActiveProject":
                    this.SetAsActiveProject(tn);
                    break;
                case "SetAsActiveBox":
                    this.SetAsActiveBox(tn);
                    break;
                case "Cut":
                    this.CutAction(tn);
                    break;
                case "Copy":
                    this.CopyAction(tn);
                    break;
                case "Delete":
                    this.DeleteAction(tn);
                    break;
                case "Paste":
                    this.PasteAction(tn);
                    break;
                case "Remove":
                    this.RemoveAction(tn);
                    break;
                case "Rename":
                    this.RenameAction(tn);
                    break;
                case "Add":
                    // via dropdownitems
                    return;
                case "ChangeFileType":
                    // via dropdownitems
                    return;
                case "Properties":
                    this.PropertiesActions(tn);
                    break;
                case "DebugStart":
                    this.DebugStart();
                    break;
                case "AddExistingObject":
                    this.AddExistingObject(tn);
                    break;
                default:
                    MessageBox.Show("cms_MouseClick: " + tsi.Text, "TODO!!");
                    break;
            }
        }
    }

    public class NodeSorter : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            var tx = x as TreeNode;
            var ty = y as TreeNode;
            return string.Compare(tx.Text, ty.Text);
        }
    }
}
