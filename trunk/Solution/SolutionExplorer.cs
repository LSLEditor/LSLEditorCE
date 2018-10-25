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
		private TypeSL[] BodyPartsTypes = 
		{
			TypeSL.Eyes, 
			TypeSL.Hair, 
			TypeSL.Shape, 
			TypeSL.Skin 
		};
		private TypeSL[] ClothesTypes = 
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
		private TypeSL[] FilesTypes = 
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

			public RealTag(TypeSL ItemType,string Name, Guid Guid, string Path)
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

		private SolidBrush brushHighLightBackground;
		private SolidBrush brushNormalForeColor;
		private SolidBrush brushCutForeColor;
		private SolidBrush brushBackColor;
		private Pen penWhite;
		private Pen penBackColor;

		private ImageList imageList1;

		private OpenFileDialog openFileDialog1;

		private Timer timer;

		private bool m_dirty;

		private ListViewItem m_HoveredItem;
		private ToolTip toolTip1;


		public bool IsDirty
		{
			get
			{
				return m_dirty;
			}
		}

		public SolutionExplorer()
		{
			InitializeComponent();

			this.Repository = null;

			this.toolTip1 = new ToolTip();

			this.m_dirty = false;

			this.components = new System.ComponentModel.Container();

			imageList1 = new ImageList();
			imageList1.TransparentColor = Color.Transparent;
			for (int intI = 0; intI <= 42; intI++)//41
			{
				TypeSL typeSL = (TypeSL)intI;
				imageList1.Images.Add(intI.ToString(), new Bitmap(typeof(LSLEditorForm), "ImagesSolutionExplorer." + typeSL.ToString().Replace("_", " ") + ".gif"));
			}

			// take care of this as an property
			//this.parent = parent;

			this.brushHighLightBackground = new SolidBrush(Color.FromArgb(0xff, 0x6d, 0x6e, 0x6f));
			this.brushNormalForeColor = new SolidBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
			this.brushCutForeColor = new SolidBrush(Color.FromArgb(0xff, Color.DarkGray));
			this.brushBackColor = new SolidBrush(Color.FromArgb(0xff, 0x3e, 0x3e, 0x3e));
			this.penWhite = new Pen(Color.White);
			this.penBackColor = new Pen(this.brushBackColor);


			this.treeView1.ImageList = imageList1;
			this.treeView1.ShowRootLines = false; // just 1 root
			this.treeView1.ForeColor = Color.White;
			this.treeView1.BackColor = Color.FromArgb(0xff, 0x3e, 0x3e, 0x3e);
			this.treeView1.LabelEdit = false;
			this.treeView1.BeforeCollapse += new TreeViewCancelEventHandler(treeView1_BeforeCollapse);
			this.treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
			this.treeView1.DrawNode += new DrawTreeNodeEventHandler(treeView1_DrawNode);
			this.treeView1.AfterLabelEdit += new NodeLabelEditEventHandler(treeView1_AfterLabelEdit);
			this.treeView1.MouseDown += new MouseEventHandler(treeView1_MouseDown);

			this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);

			this.treeView1.MouseUp += new MouseEventHandler(treeView1_MouseUp);

			this.treeView1.TreeViewNodeSorter = new NodeSorter();
			
			this.listView1.Dock = DockStyle.Fill;
			this.listView1.View = View.Details;
			this.listView1.HeaderStyle = ColumnHeaderStyle.None;
			//this.listView1.Scrollable = false;
			
			this.listView1.GridLines = true;
			this.listView1.Columns.Add("Name",60);
			this.listView1.Columns.Add("Value",250);

			// for disposing
			this.timer = new Timer(this.components);
			this.timer.Interval = 250;
			this.timer.Tick += new EventHandler(timer_Tick);

			this.openFileDialog1 = new OpenFileDialog();
		}

		public string GetCurrentSolutionPath()
		{
			if (ActiveSolution == null)
				return null;
			if (DirectorySolution == null)
				return null;
			if (NameSolution == null)
				return null;

			return Path.Combine(DirectorySolution, NameSolution + ".sol");
		}


		public string GetProjectName(Guid guid)
		{
			TreeNode tn = FindGuid(this.treeView1.TopNode, guid);
			if (tn != null)
			{
				while (tn != this.treeView1.TopNode)
				{
					tn = tn.Parent;
					if (GetTypeSL(tn) == TypeSL.Project)
						return tn.Text;
				}
			}
			return "";
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			timer.Stop();
			RenameAction(this.treeView1.SelectedNode);
		}

		private int NumberOfProjects()
		{
			if (ActiveSolution == null)
				return 0;
			int intProjects = 0;
			foreach (TreeNode tn in ActiveSolution.Nodes)
			{
				if (GetTypeSL(tn) == TypeSL.Project)
					intProjects++;
			}
			return intProjects;
		}

		private bool HasInventory()
		{
			foreach (TreeNode tn in ActiveSolution.Nodes)
			{
				if (GetTypeSL(tn) == TypeSL.Inventory)
					return true;
			}
			return false;
		}

		private void EmptyInventory(TreeNode parentNode)
		{
			this.m_dirty = true;
			TreeNode treeNode = new TreeNode("Inventory", (int)TypeSL.Inventory, (int)TypeSL.Inventory);
			string strInventoryPath = Path.Combine(DirectorySolution, "Inventory");
			treeNode.Tag = new RealTag(TypeSL.Inventory, "Inventory", Guid.NewGuid(), strInventoryPath);
			parentNode.Nodes.Insert(0,treeNode);
			for (int intI = 1; intI < 15; intI++)
			{
				TypeSL typeSL = (TypeSL)intI;
				string strName = typeSL.ToString().Replace('_', ' ');
				TreeNode tn = new TreeNode(strName, intI, intI);
				tn.Tag = new RealTag(typeSL, strName, Guid.NewGuid());
				treeNode.Nodes.Add(tn);
			}
		}

		private EditForm GetEditForm(Guid guid)
		{
			LSLEditorForm lslEditorForm = this.parent;
			foreach (Form form in lslEditorForm.Children)
			{
				EditForm editForm = form as EditForm;
				if (editForm == null || editForm.IsDisposed)
					continue;
				if (editForm.guid == guid)
					return editForm;
			}
			return null;
		}

		public void CloseSolution()
		{
			if (this.ActiveSolution == null)
				return;

			this.parent.StopSimulator();

			if (this.parent.CloseAllOpenWindows())
			{
				SaveSolutionFile();

				this.ActiveProject = null;
				this.ActiveSolution = null;
				this.ActiveObject = null;

				this.m_dirty = false;

				this.treeView1.Nodes.Clear();

				this.listView1.Items.Clear();

				if(!Properties.Settings.Default.ShowSolutionExplorer)
					this.parent.ShowSolutionExplorer(false);
				this.parent.SolutionItemsFileMenu(false);
			}
		}

		public bool CreateNew(string strNameProject,string strDirectory,string strNameSolution,bool blnCreateSolutionDirectory)
		{
			DirectorySolution = Path.Combine(strDirectory,strNameSolution);

			string strSolutionFileName = Path.Combine(DirectorySolution, strNameSolution + ".sol");

			if(File.Exists(strSolutionFileName))
			{
				MessageBox.Show("Solution file already exists", "Create new Solution", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			AddSolution(strNameSolution);

			string strProjectPath;
			if (blnCreateSolutionDirectory)
				strProjectPath = Path.Combine(DirectorySolution, strNameProject);
			else
				strProjectPath = DirectorySolution;

			// made active, if only project
			AddItem(TypeSL.Project,strNameProject, Guid.NewGuid() , strProjectPath);

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
					LoadItem(this.openFileDialog1.FileName,false);
			}
		}

		#region UserInterface stuff

		// disable root solution collapse (there is no + sign)
		void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
		{
			e.Cancel = (e.Node == this.ActiveSolution);
		}

		void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			if (e.Node.IsEditing)
				return;

			if (e.Bounds.Height == 0)
				return;

			Font font;
			if (e.Node == ActiveProject || e.Node == ActiveObject)
				font = new Font(this.treeView1.Font, FontStyle.Bold);
			else
				font = new Font(this.treeView1.Font, FontStyle.Regular);

			// Draw background
			SizeF sizeF = e.Graphics.MeasureString(e.Node.Text, font);
			Rectangle rect = new Rectangle(e.Bounds.Location, new Size((int)sizeF.Width + 5, e.Bounds.Height - 1));

			if ((e.State & TreeNodeStates.Selected) != 0)
			{
				e.Graphics.FillRectangle(brushHighLightBackground, rect);
				e.Graphics.DrawRectangle(penWhite, rect);
			}
			else
			{
				e.Graphics.FillRectangle(brushBackColor, rect);
				e.Graphics.DrawRectangle(penBackColor, rect);
			}

			// Draw text
			Point point = new Point(e.Bounds.Location.X, e.Bounds.Location.Y + 2);
			if (e.Node.ForeColor != Color.Empty)
				e.Graphics.DrawString(e.Node.Text, font, brushCutForeColor, point);
			else
				e.Graphics.DrawString(e.Node.Text, font, brushNormalForeColor, point);

			// Clean up
			font.Dispose();
		}

		void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			this.treeView1.LabelEdit = false;

			if (e.Label == null || e.Label.Trim() == "")
			{
				e.CancelEdit = true;
				return;
			}

			// check for illegal chars!!
			string strLabel = e.Label;

			foreach (char chrF in e.Label.ToCharArray())
			{
				foreach (char chrInvalid in Path.GetInvalidFileNameChars())
				{
					if (chrF == chrInvalid)
					{
						MessageBox.Show("Char '" + chrInvalid + "' is not allowed", "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
						e.CancelEdit = true;
						return;
					}
				}
			}

			string strBackup = e.Node.Text;
			string strSource = GetFullPath(e.Node);
			e.Node.Text = e.Label;
			string strDestination = GetFullPath(e.Node);

			if (e.Node == ActiveSolution)
			{
				NameSolution = e.Label;
				UpdateVisualSolutionName();
				string strOldSolutionFile = Path.GetFullPath(Path.Combine(strSource, strBackup + ".sol"));
				string strNewSolutionFile = Path.GetFullPath(Path.Combine(strDestination, NameSolution + ".sol"));

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

				if(File.Exists(strOldSolutionFile))
					File.Move(strOldSolutionFile, strNewSolutionFile);
				ShowProperties(ActiveSolution);

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
					if (MessageBox.Show("Destination file exists! overwrite?", "Oops...", MessageBoxButtons.YesNo , MessageBoxIcon.Warning) != DialogResult.Yes)
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

				RealTag rt = (RealTag)e.Node.Tag;
                string oldName = rt.Name;
				rt.Name = e.Node.Text;
				e.Node.Tag = rt; // save name
				EditForm editForm = GetEditForm(rt.Guid);
				if (editForm != null)
				{
					editForm.FullPathName = strDestination; // GetFullPath(e.Node);
					editForm.SaveCurrentFile();
				}

                if (rt.ItemType == TypeSL.LSLIScript)
                {
                    EditForm form = GetEditForm(rt.Guid);
                    if(form != null)
                    {
                        form.SaveCurrentFile();
                        form.Dirty = true;
                        form.Show();

                        // Get the expanded version of the form
                        string x = Helpers.LSLIPathHelper.GetExpandedTabName(Helpers.LSLIPathHelper.CreateExpandedScriptName(oldName));
                        EditForm xform = (EditForm)parent.GetForm(x);

                        if (xform != null && xform.Visible)
                        {
                            string src = xform.SourceCode;
                            xform.Dirty = false;
                            xform.Close();
                            form.SourceCode = src;
                            form.Dirty = true;
                            parent.ExpandForm(form);
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

				RealTag rt = (RealTag)e.Node.Tag;
				rt.Name = e.Node.Text;
				e.Node.Tag = rt; // save name

				// update all path of opened windows
				foreach (Form form in this.parent.Children)
				{
					EditForm editForm = form as EditForm;
					if (editForm == null || editForm.IsDisposed)
						continue;
					TreeNode tn = FindGuid(e.Node, editForm.guid);
					if (tn != null)
					{
						editForm.FullPathName = GetFullPath(tn);
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
			TypeSL typeSL = GetTypeSL(treeNode);
			return ((int)typeSL >= 20);
		}

		private bool IsItemAndNotObject(TreeNode treeNode)
		{
			TypeSL typeSL = GetTypeSL(treeNode);
			int intTypeSL = (int)typeSL;
			return (intTypeSL >= 20 && typeSL != TypeSL.Object);
		}


		private bool IsInventoryContainer(TreeNode treeNode)
		{
			TypeSL TypeSL = GetTypeSL(treeNode);
			return ((int)TypeSL <= 15);
		}

		private TypeSL GetTypeSL(TreeNode treeNode)
		{
			if (treeNode == null)
				return TypeSL.Unknown;
			if (treeNode.Tag == null)
				return TypeSL.Unknown;
			RealTag realTag = (RealTag)treeNode.Tag;
			return realTag.ItemType;
		}

		private string GetPath(TreeNode treeNode)
		{
			if (treeNode == null)
				return null;
			if (treeNode.Tag == null)
				return null;
			RealTag realTag = (RealTag)treeNode.Tag;
			string strPath = realTag.Path;
			if (strPath == null)
				return null;

			if(strPath.EndsWith(treeNode.Text))
				return strPath;

			// Project has been renamed
			strPath = Path.Combine(Path.GetDirectoryName(strPath), treeNode.Text);
			realTag.Path = strPath;
			treeNode.Tag = realTag;
			return strPath;
		}

		private string GetXml(int intDepth, TreeNode treeNode)
		{
			StringBuilder sb = new StringBuilder();
			RealTag realTag = (RealTag)treeNode.Tag;
			string strTagName = realTag.ItemType.ToString();
			string strValue = treeNode.Text;
			if (treeNode == ActiveSolution) {
				strValue = NameSolution;
			}
			for (int intI = 0; intI < intDepth; intI++) {
				sb.Append('\t');
			}
			string strActive = "";
			string strDescription = "";
			string strEnd = "";
			if (treeNode == ActiveObject) {
				strActive = @" active=""true""";
			}
			if (realTag.Description != string.Empty) {
				strDescription = @" description=""" + realTag.Description + @"""";
			}
			if (treeNode.Nodes.Count == 0) {
				strEnd = " /";
			}
			sb.AppendFormat("<{0} name=\"{1}\" guid=\"{2}\"{3}{4}{5}>\r\n", strTagName, strValue, realTag.Guid, strActive, strDescription, strEnd);
			if (treeNode.Nodes.Count > 0) {
				foreach (TreeNode childNode in treeNode.Nodes) {
					sb.Append(GetXml(intDepth + 1, childNode));
				}
				for (int intI = 0; intI < intDepth; intI++) {
					sb.Append('\t');
				}
				sb.AppendFormat("</{0}>\r\n", strTagName);
			}
			return sb.ToString();
		}

		private string GetFullPath(TreeNode treeNode)
		{
			if (treeNode == ActiveSolution)
				return DirectorySolution;

			TreeNode parentNode = treeNode;
			string strPath = "";
			while (GetPath(parentNode) == null)
			{
				strPath = Path.Combine(parentNode.Text, strPath);
				parentNode = parentNode.Parent;
			}
			strPath = Path.Combine(GetPath(parentNode), strPath);
			return Path.GetFullPath(strPath);
		}

		private void RecursiveLoad(TreeNode treeNode, XmlNode xmlParentNode)
		{
			foreach (XmlNode xmlNode in xmlParentNode.SelectNodes("./*"))
			{
				TypeSL typeSL = (TypeSL)Enum.Parse(typeof(TypeSL), xmlNode.Name);

				if (typeSL == TypeSL.Prim) // Old!!!
					typeSL = TypeSL.Object;

				string strName = xmlNode.Attributes["name"].Value;
				Guid guid = new Guid(xmlNode.Attributes["guid"].Value);

				XmlAttribute attributePath = xmlNode.Attributes["path"];
				XmlAttribute attributeDescription = xmlNode.Attributes["description"];
				int intImageIndex = (int)typeSL;
				TreeNode tn = new TreeNode(strName, intImageIndex, intImageIndex);

				RealTag realTag = new RealTag(typeSL, strName, guid);

				if (attributePath != null)
					realTag.Path = attributePath.Value;
				if (attributeDescription != null)
					realTag.Description = attributeDescription.Value;

				tn.Tag = realTag;

				XmlAttribute attributeActive = xmlNode.Attributes["active"];
				if (attributeActive != null)
				{
					if (attributeActive.Value == "true")
						ActiveObject = tn;
				}
				treeNode.Nodes.Add(tn);
				RecursiveLoad(tn, xmlNode);
			}
		}


		private void LoadItem(string strPath, bool blnActive)
		{
			XmlDocument xmlItemFile = new XmlDocument();
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
				TypeSL typeSL = (TypeSL)Enum.Parse(typeof(TypeSL), xmlItem.Name);
				TreeNode currentItem = AddItem(typeSL,
					xmlItem.Attributes["name"].Value,
					new Guid(xmlItem.Attributes["guid"].Value),
					Path.GetDirectoryName(strPath));
				if (currentItem == null)
					continue;
				if (blnActive)
					ActiveProject = currentItem;
				RecursiveLoad(currentItem, xmlItem);
			}
		}

		public bool OpenSolution(string strPath)
		{
			DirectorySolution = Path.GetDirectoryName(strPath);
			NameSolution = Path.GetFileNameWithoutExtension(strPath);

			XmlDocument xml = new XmlDocument();
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
					this.Repository = xmlSolution.Attributes["repository"].Value;

				AddSolution(xmlSolution.Attributes["name"].Value);
				foreach (XmlNode xmlItem in xmlSolution.SelectNodes("./*"))
				{
					string strItemPath = xmlItem.Attributes["path"].Value;
					if (!Path.IsPathRooted(strItemPath))
						strItemPath = Path.Combine(DirectorySolution, strItemPath);
					if (!File.Exists(strItemPath))
						continue; // TODO verbose error
					bool blnActive = false;
					if (xmlItem.Attributes["active"] != null)
						blnActive = (xmlItem.Attributes["active"].Value == "true");
					LoadItem(strItemPath,blnActive);
				}
			}
			this.parent.ShowSolutionExplorer(true);
			this.parent.SolutionItemsFileMenu(true);
			this.parent.UpdateRecentProjectList(strPath,true);
			this.m_dirty = false;

			return true;
		}

		public bool SaveSolutionFile()
		{
			if (ActiveSolution == null)
				return false;
			if (DirectorySolution == null)
				return false;
			if (NameSolution == null)
				return false;

			if (!Directory.Exists(DirectorySolution))
				Directory.CreateDirectory(DirectorySolution);

			string strSolutionFileName = Path.Combine(DirectorySolution, NameSolution + ".sol");
			StreamWriter sw = new StreamWriter(strSolutionFileName);
			string strRepositoryAttribute;
			if (this.Repository == null)
				strRepositoryAttribute = "";
			else
				strRepositoryAttribute = string.Format(" repository=\"{0}\"",this.Repository);
			sw.WriteLine("<Solution name=\"{0}\"{1}>",NameSolution,strRepositoryAttribute);
			foreach (TreeNode nodeItem in ActiveSolution.Nodes)
			{
				RealTag realTag = (RealTag)nodeItem.Tag;
				string strItemPath = SaveItemFile(nodeItem);
				if (strItemPath != null)
				{
					// Make path relative, if it is the same as solution directory
					if (strItemPath.ToLower().IndexOf(DirectorySolution.ToLower()) == 0)
						strItemPath = strItemPath.Substring(DirectorySolution.Length+1);
					string strActive = "";
					if (ActiveProject == nodeItem)
						strActive = @" active=""true""";
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
			string strName = nodeItem.Text;
			RealTag realTag = (RealTag)nodeItem.Tag;
			string strPath = realTag.Path;

			if (!Directory.Exists(strPath))
				Directory.CreateDirectory(strPath);

			string strExtension = ".prj";
			if (realTag.Name == "Inventory")
				strExtension = ".inv";
			string strFilePath = Path.Combine(strPath, strName + strExtension);
			StreamWriter sw = new StreamWriter(strFilePath);
			sw.Write(GetXml(0, nodeItem));
			sw.Close();
			return strFilePath;
		}

		private void AddSolution(string strNameSolution)
		{
			this.NameSolution = strNameSolution;
			ActiveSolution = new TreeNode("Solution '" + strNameSolution + "'", (int)TypeSL.Solution, (int)TypeSL.Solution);
			ActiveSolution.Tag = new RealTag(TypeSL.Solution, strNameSolution, Guid.NewGuid());
			this.treeView1.Nodes.Insert(0,ActiveSolution);
			ActiveSolution.Expand();
			this.parent.SolutionItemsFileMenu(true);
		}

		private void UpdateVisualSolutionName()
		{
			int intProjects = NumberOfProjects();
			ActiveSolution.Text = "Solution '" + this.NameSolution + "' (" + intProjects + " project" + ((intProjects != 1) ? "s" : "") + ")";
		}

		private bool ItemExist(Guid guid)
		{
			foreach (TreeNode itemNode in ActiveSolution.Nodes)
			{
				RealTag realTag = (RealTag)itemNode.Tag;
				if (realTag.Guid == guid)
					return true;
			}
			return false;
		}

		private TreeNode AddItem(TypeSL typeSL, string strName, Guid guid, string strPath)
		{
			if (ItemExist(guid))
				return null;

			if (!Directory.Exists(strPath))
				Directory.CreateDirectory(strPath);

			TreeNode treeNode = new TreeNode(strName, (int)typeSL, (int)typeSL);
			treeNode.Tag = new RealTag(typeSL, strName, guid, strPath);

			if (ActiveProject == null && typeSL == TypeSL.Project)
				ActiveProject = treeNode;

			ActiveSolution.Nodes.Add(treeNode);
			ActiveSolution.Expand();
			UpdateVisualSolutionName();

			this.m_dirty = true;

			return treeNode;
		}

		private TreeNode AddNewContainer(TreeNode parent, TypeSL typeSL, string strNewName, Guid guid)
		{
			if (parent == null)
				return null;

			string strDirectoryContainer = Path.Combine(GetFullPath(parent), strNewName);
			if (!Directory.Exists(strDirectoryContainer))
				Directory.CreateDirectory(strDirectoryContainer);

			TreeNode treeNode = new TreeNode(strNewName, (int)typeSL, (int)typeSL);
			treeNode.Tag = new RealTag(typeSL, strNewName, guid);

			if (ActiveObject == null && typeSL == TypeSL.Object)
				ActiveObject = treeNode;

			parent.Nodes.Add(treeNode);
			parent.Expand();

			this.m_dirty = true;

			return treeNode;
		}


		private void AddNewFile(TreeNode parent, TypeSL typeFile)
		{
			if (parent == null)
				return;

			string strName = typeFile+"{0}";
			string strExtension = "";
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
			string strNewName = GetNewFileName(parent, strName + strExtension);

			string strFilePath = Path.Combine(GetFullPath(parent), strNewName);

			if (!File.Exists(strFilePath))
			{
				StreamWriter sw = new StreamWriter(strFilePath);

				switch (typeFile)
				{
                    case TypeSL.Script:
						sw.Write(AutoFormatter.ApplyFormatting(0,Helpers.GetTemplate.Source()));
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

			TreeNode newFile = new TreeNode(strNewName, (int)typeFile, (int)typeFile);
            newFile.Tag = new RealTag(typeFile, strNewName, Guid.NewGuid());
			parent.Nodes.Add(newFile);
			parent.Expand();

            this.m_dirty = true;
		}

		private void AddExistingFile(TreeNode parent, string strName, Guid guid)
		{
			if (parent == null)
				return;

			string strFilePath = Path.Combine(GetFullPath(parent), strName);

			TypeSL typeFile = TypeSL.Unknown;

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
			TreeNode newFile = new TreeNode(strName, (int)typeFile, (int)typeFile);
			newFile.Tag = new RealTag(typeFile, strName, guid);
			parent.Nodes.Add(newFile);
			parent.Expand();

			this.m_dirty = true;
		}

		private void AddExistingFile(TreeNode tn)
		{
			if (GetTypeSL(tn) != TypeSL.Object)
				return;
			this.openFileDialog1.FileName = "";
			this.openFileDialog1.Filter = "All Files (*.*)|*.*";
			this.openFileDialog1.Multiselect = true;
			if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
			{
				foreach (string strSourcePath in this.openFileDialog1.FileNames)
				{
					string strName = Path.GetFileName(strSourcePath);
					string strDestinationPath = Path.Combine(GetFullPath(tn), strName);

					bool blnAdd = true;
					foreach (TreeNode child in tn.Nodes)
					{
						if (child.Text == strName)
						{
							blnAdd = false;

							if (strSourcePath.ToLower() == strDestinationPath.ToLower())
								break;

							if (MessageBox.Show("Overwrite existing file?", "File already exists", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
							{
								child.Remove();
								blnAdd = true;
								break;
							}
						}
					}
					if (!blnAdd)
						continue;
					if (strSourcePath.ToLower() != strDestinationPath.ToLower())
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
					AddExistingFile(tn, strName, Guid.NewGuid());
				}
			}
		}

		public void AddNewObject()
		{
			if (ActiveProject == null)
				return;
			AddNewContainer(ActiveProject, TypeSL.Object, GetNewDirectoryName(ActiveProject, "Object{0}"), Guid.NewGuid());
		}

		#region ContextMenus

		private ToolStripMenuItem cmsItemsAdd(ContextMenuStrip cms, string strName, string strValue,string strKeys)
		{
			ToolStripMenuItem tsmi = new ToolStripMenuItem(strValue);
			tsmi.ShortcutKeyDisplayString = strKeys;
			tsmi.Name = strName;
			cms.Items.Add(tsmi);
			return tsmi;
		}

		private ToolStripMenuItem cmsItemsAdd(ContextMenuStrip cms, string strName, string strValue)
		{
			ToolStripMenuItem tsmi = new ToolStripMenuItem(strValue);
			tsmi.Name = strName;
			cms.Items.Add(tsmi);
			return tsmi;
		}

		private ContextMenuStrip ContextMenuSolution()
		{
			ContextMenuStrip cms = new ContextMenuStrip();
			cms.MouseClick += new MouseEventHandler(cms_MouseClick);
			cmsItemsAdd(cms, "CloseSolution", "Close Solution");
			cms.Items.Add(new ToolStripSeparator());

			if (Properties.Settings.Default.VersionControlSVN)
			{
				if (this.Repository == null)
				{
					cmsItemsAdd(cms, "SvnImport", "Import Solution SVN");
					cms.Items.Add(new ToolStripSeparator());
				}
				else // already workingcopy
				{
					cmsItemsAdd(cms, "SvnUpdate", "SVN update");
					cmsItemsAdd(cms, "SvnCommit", "SVN commit");
					cms.Items.Add(new ToolStripSeparator());
				}
			}

			cmsItemsAdd(cms, "AddNewProject", "Add New Project");
			cmsItemsAdd(cms, "AddExistingProject", "Add Existing Project...");
			if (!HasInventory())
			{
				cms.Items.Add(new ToolStripSeparator());
				cmsItemsAdd(cms, "AddInventory", "Add Empty Inventory");
			}
			cms.Items.Add(new ToolStripSeparator());
			if (GetTypeSL(CutObject) == TypeSL.Project || GetTypeSL(CopyObject) == TypeSL.Project)
				cmsItemsAdd(cms, "Paste", "Paste project" , "Ctrl+V");
			cmsItemsAdd(cms, "Rename", "Rename" , "F2");
			cms.Items.Add(new ToolStripSeparator());
			cmsItemsAdd(cms, "Properties", "Properties");
			return cms;
		}

		private ContextMenuStrip ContextMenuInventory(TypeSL typeSL)
		{
			ContextMenuStrip cms = new ContextMenuStrip();
			cms.MouseClick += new MouseEventHandler(cms_MouseClick);

			if (typeSL != TypeSL.Inventory)
			{
				ToolStripMenuItem tsmiClothes = cmsItemsAdd(cms, "Add", "Add Clothes");
				tsmiClothes.DropDownItemClicked += new ToolStripItemClickedEventHandler(tsmi_DropDownItemClicked);
				foreach (TypeSL file in ClothesTypes)
					tsmiClothes.DropDownItems.Add(file.ToString(), this.imageList1.Images[(int)file]);

				ToolStripMenuItem tsmiBodyParts = cmsItemsAdd(cms, "Add", "Add Body Parts");
				tsmiBodyParts.DropDownItemClicked += new ToolStripItemClickedEventHandler(tsmi_DropDownItemClicked);
				foreach (TypeSL file in BodyPartsTypes)
					tsmiBodyParts.DropDownItems.Add(file.ToString(), this.imageList1.Images[(int)file]);

				ToolStripMenuItem tsmiFiles = cmsItemsAdd(cms, "Add", "Add Items");
				tsmiFiles.DropDownItemClicked += new ToolStripItemClickedEventHandler(tsmi_DropDownItemClicked);
				foreach (TypeSL file in FilesTypes)
					tsmiFiles.DropDownItems.Add(file.ToString(), this.imageList1.Images[(int)file]);
				cms.Items.Add(new ToolStripSeparator());
			}
			cmsItemsAdd(cms, "AddNewFolder", "Add New Folder");

			if (typeSL == TypeSL.Folder)
			{
				cms.Items.Add(new ToolStripSeparator());
				cmsItemsAdd(cms, "Cut", "Cut" , "Ctrl+X");
				cmsItemsAdd(cms, "Copy", "Copy","Ctrl+C");
				cmsItemsAdd(cms, "Delete", "Delete", "Del");
				cmsItemsAdd(cms, "Rename", "Rename", "F2");
			}
			if (CutObject != null || CopyObject != null)
			{
				cms.Items.Add(new ToolStripSeparator());
				cmsItemsAdd(cms, "Paste", "Paste", "Ctrl+V");
			}
			cms.Items.Add(new ToolStripSeparator());
			cmsItemsAdd(cms, "Properties", "Properties");

			return cms;
		}

		private ContextMenuStrip ContextMenuProject()
		{
			ContextMenuStrip cms = new ContextMenuStrip();
			cms.MouseClick += new MouseEventHandler(cms_MouseClick);
			cmsItemsAdd(cms, "AddNewObject", "Add New Object");
			cmsItemsAdd(cms, "AddExistingObject", "Add Existing Object...");
			cms.Items.Add(new ToolStripSeparator());
			if (NumberOfProjects() > 1 || ActiveProject == null)
				cmsItemsAdd(cms, "SetAsActiveProject", "Set as Active Project");
			cmsItemsAdd(cms, "DebugStart","Debug Start");
			cms.Items.Add(new ToolStripSeparator());
			cmsItemsAdd(cms, "Cut","Cut","Ctrl+X");
			cmsItemsAdd(cms, "Copy", "Copy", "Ctrl+C");
			cmsItemsAdd(cms, "Remove","Remove");
			if (GetTypeSL(CutObject) == TypeSL.Object || GetTypeSL(CopyObject) == TypeSL.Object)
				cmsItemsAdd(cms, "Paste", "Paste Object","Ctrl+V");
			cmsItemsAdd(cms, "Rename","Rename","F2");
			cms.Items.Add(new ToolStripSeparator());
			cmsItemsAdd(cms, "Properties","Properties");
			return cms;
		}

		public void CreateNewFileDrowDownMenu(ToolStripMenuItem tsmi)
		{
			tsmi.DropDownItemClicked += new ToolStripItemClickedEventHandler(tsmi_DropDownItemClicked);
			foreach (TypeSL file in FilesTypes)
				tsmi.DropDownItems.Add(file.ToString(), this.imageList1.Images[(int)file]);
		}

		public void CreateNewFileDrowDownMenu(ContextMenuStrip cms)
		{
			ToolStripMenuItem tsmi = cmsItemsAdd(cms, "Add", "Add New Item");
			tsmi.DropDownItemClicked += new ToolStripItemClickedEventHandler(tsmi_DropDownItemClicked);
			foreach (TypeSL file in FilesTypes)
				tsmi.DropDownItems.Add(file.ToString(), this.imageList1.Images[(int)file]);
		}

		private ContextMenuStrip ContextMenuBox()
		{
			ContextMenuStrip cms = new ContextMenuStrip();
			cms.MouseClick += new MouseEventHandler(cms_MouseClick);

			CreateNewFileDrowDownMenu(cms);
			//ToolStripMenuItem tsmi = cmsItemsAdd(cms, "Add", "Add New File");
			//tsmi.DropDownItemClicked += new ToolStripItemClickedEventHandler(tsmi_DropDownItemClicked);
			//foreach (TypeSL file in FilesTypes)
			//	tsmi.DropDownItems.Add(file.ToString(), this.imageList1.Images[(int)file]);

			cmsItemsAdd(cms, "AddExistingFile", "Add Existing File(s)...");
			cms.Items.Add(new ToolStripSeparator());
			cmsItemsAdd(cms, "SetAsActiveBox", "Set As Active Object");
			cms.Items.Add(new ToolStripSeparator());
			cmsItemsAdd(cms, "ExcludeFromProject", "Exclude From Project");
			cms.Items.Add(new ToolStripSeparator());
			cmsItemsAdd(cms, "Cut", "Cut","Ctrl+X");
			cmsItemsAdd(cms, "Copy", "Copy", "Ctrl+C");
			cmsItemsAdd(cms, "Delete", "Delete", "Del");
			if (IsItem(CutObject) || IsItem(CopyObject))
				cmsItemsAdd(cms, "Paste", "Paste", "Ctrl+V");
			cmsItemsAdd(cms, "Rename", "Rename","F2");
			cms.Items.Add(new ToolStripSeparator());
			cmsItemsAdd(cms, "Properties", "Properties");
			return cms;
		}

		private ContextMenuStrip ContextMenuFile()
		{
			ContextMenuStrip cms = new ContextMenuStrip();
			cms.MouseClick += new MouseEventHandler(cms_MouseClick);
			cmsItemsAdd(cms, "Open", "Open");
			cms.Items.Add(new ToolStripSeparator());
			cmsItemsAdd(cms, "Cut", "Cut" , "Ctrl+X");
			cmsItemsAdd(cms, "Copy", "Copy", "Ctrl+C");
			cmsItemsAdd(cms, "Delete", "Delete", "Del");
			cmsItemsAdd(cms, "Rename", "Rename", "F2");
			cms.Items.Add(new ToolStripSeparator());
			ToolStripMenuItem tsmi = cmsItemsAdd(cms, "ChangeFileType", "Change filetype");
			CreateNewFileDrowDownMenu(tsmi);
			cms.Items.Add(new ToolStripSeparator());
			cmsItemsAdd(cms, "Properties", "Properties");
			return cms;
		}

		#endregion

		void tsmi_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			// Add New File
			TreeNode tn;
			ToolStripDropDownItem tsddi = sender as ToolStripDropDownItem;
			ContextMenuStrip cms = tsddi.Owner as ContextMenuStrip;
			if (cms != null)
				tn = cms.Tag as TreeNode;
			else
				tn = ActiveObject;

			if (tn != null)
			{
				string strName = tsddi.Name;
				if (strName == "ChangeFileType")
				{
					RealTag realTag = (RealTag)tn.Tag;
					TypeSL stypeSl = (TypeSL)Enum.Parse(typeof(TypeSL), e.ClickedItem.Text);
					realTag.ItemType = stypeSl;
					tn.Tag = realTag;
					tn.ImageIndex = (int)stypeSl;
					tn.SelectedImageIndex = (int)stypeSl;
					this.treeView1.Invalidate(tn.Bounds);
					return;
				}
				else
				{
					TypeSL typeSL = (TypeSL)Enum.Parse(typeof(TypeSL), e.ClickedItem.Text);
					if (typeSL != TypeSL.Object)
						AddNewFile(tn, typeSL);
					else
						AddNewContainer(tn, TypeSL.Object, GetNewDirectoryName(tn, "Object{0}"), Guid.NewGuid());
				}
			}
		}

		void treeView1_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return; 

			TreeNode treeNode = this.treeView1.GetNodeAt(new Point(e.X, e.Y));
			if (treeNode == null)
				return;

			if (e.X < treeNode.Bounds.X) // only when clicked on the label
				return;

			if (timer.Tag != null)
			{
				if (timer.Tag == treeNode)
				{
					timer.Tag = null;
					timer.Start();
					return;
				}
			}
			timer.Tag = treeNode;
		}

		void treeView1_MouseDown(object sender, MouseEventArgs e)
		{
			TreeNode treeNode = this.treeView1.GetNodeAt(new Point(e.X, e.Y));
			if (treeNode == null)
				return;

			this.treeView1.SelectedNode = treeNode;

			RealTag realTag = (RealTag)treeNode.Tag;

			if (e.Button == MouseButtons.Right)
			{
				ContextMenuStrip cms = null;
				switch (realTag.ItemType)
				{
					case TypeSL.Solution:
						cms = ContextMenuSolution();
						break;
					case TypeSL.Project:
						cms = ContextMenuProject();
						break;
					case TypeSL.Object:
						cms = ContextMenuBox();
						break;
					default:
						if (IsInventoryContainer(treeNode))
							cms = ContextMenuInventory(realTag.ItemType);
						else if (IsItem(treeNode))
							cms = ContextMenuFile();
						else
							cms = new ContextMenuStrip();
						break;
				}
				cms.Tag = treeNode;
				this.treeView1.ContextMenuStrip = cms;
			}
		}

		private string GetNewName(TreeNode treeNode, bool blnDirectory, string strFormat)
		{
			string strDirectory = GetFullPath(treeNode);
			if (File.Exists(strDirectory))
				strDirectory = Path.GetDirectoryName(strDirectory);

			if (!Directory.Exists(strDirectory))
				Directory.CreateDirectory(strDirectory);

			RealTag realTag = (RealTag)treeNode.Tag;
			if (blnDirectory)
			{
				string strName = strFormat.Replace("({0}) ", "").Replace("{0}", "").Replace("Copy of ","");
				if (!Directory.Exists(Path.Combine(strDirectory, strName)))
					return strName;
				strName = strFormat.Replace("({0}) ", "").Replace("{0}", "");
				if (!Directory.Exists(Path.Combine(strDirectory, strName)))
					return strName;

				for (int intI = 1; intI < 255; intI++)
				{
					strName = string.Format(strFormat, intI);
					if (!Directory.Exists(Path.Combine(strDirectory, strName)))
						return strName;
				}
			}
			else
			{
				string strName = strFormat.Replace("({0}) ", "").Replace("{0}", "").Replace("Copy of ","");
				if (!File.Exists(Path.Combine(strDirectory, strName)))
					return strName;
				strName = strFormat.Replace("({0}) ", "").Replace("{0}", "");
				if (!File.Exists(Path.Combine(strDirectory, strName)))
					return strName;

				for (int intI = 2; intI < 255; intI++)
				{
					strName = string.Format(strFormat, intI);
					if (!File.Exists(Path.Combine(strDirectory, strName)))
						return strName;
				}
			}
			MessageBox.Show("There are 255 items with the same name", "Oops...");
			return null;
		}

		private string GetNewFileName(TreeNode treeNode, string strFormat)
		{
			return GetNewName(treeNode, false, strFormat);
		}

		private string GetNewDirectoryName(TreeNode treeNode, string strFormat)
		{
			return GetNewName(treeNode, true, strFormat);
		}

		private void DeleteAction(TreeNode tn)
		{
			if (MessageBox.Show("Delete " + tn.Text, "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
				DeleteChildren(tn);
		}

		private void DeleteChildren(TreeNode tn)
		{
			// remove backwards, foreach does not work
			for(int intI=tn.Nodes.Count-1;intI>=0;intI--)
				DeleteChildren(tn.Nodes[intI]);

			string strPath = GetFullPath(tn);
			if (File.Exists(strPath))
			{
				File.Delete(strPath);
				RealTag rt = (RealTag)tn.Tag;
				EditForm editForm = GetEditForm(rt.Guid);
				if (editForm != null)
				{
					this.parent.ActivateMdiForm(editForm);
					this.parent.CloseActiveWindow();
				}
			}
			else if (Directory.Exists(strPath))
			{
				if (Directory.GetFiles(strPath).Length == 0)
					Directory.Delete(strPath, true);
			}
			else
			{
				MessageBox.Show("Not found: " + tn.Text, "Oops...");
			}
			tn.Remove();

			this.m_dirty = true;
		}

		private void CopyChildren(TreeNode Source,TreeNode Destination)
		{
			RealTag realTag = (RealTag)Source.Tag;
			realTag.Guid = Guid.NewGuid(); // copy objects have new guid
			if (realTag.Path!=null)
				realTag.Path = Path.Combine(Path.GetDirectoryName(GetFullPath(Source)), Destination.Text);

			Destination.Tag = realTag;
			Destination.ImageIndex = Source.ImageIndex;
			Destination.SelectedImageIndex = Source.SelectedImageIndex;

			string strSourcePath = GetFullPath(Source);
			string strDestionationPath = GetFullPath(Destination);
			if (File.Exists(strSourcePath))
				File.Copy(strSourcePath, strDestionationPath);
			else if (Directory.Exists(strSourcePath))
				Directory.CreateDirectory(strDestionationPath);
			else
				MessageBox.Show("Error: " + strSourcePath, "Oops...");

			foreach (TreeNode child in Source.Nodes)
			{
				TreeNode clone = new TreeNode(child.Text);
				Destination.Nodes.Add(clone);
				CopyChildren(child, clone);
			}
			this.m_dirty = true;
		}

		private void PasteAction(TreeNode tn)
		{
			// paste on same object, get parent!!
			// paste on item, get parent!!
			if (tn == CopyObject || tn==CutObject || IsItemAndNotObject(tn))
				tn = tn.Parent;

			// sanitycheck, search all parents, may not be CopyObject or CutObject
			TreeNode parentNode = tn;
			while (parentNode != null)
			{
				if (parentNode == CopyObject || parentNode == CutObject)
					return;
				parentNode = parentNode.Parent;
			}

			switch(GetTypeSL(tn))
			{
				case TypeSL.Solution:
					if (GetTypeSL(CutObject) != TypeSL.Project &&
						GetTypeSL(CopyObject) != TypeSL.Project)
						return;
					break;
				case TypeSL.Project:
					if (GetTypeSL(CutObject) != TypeSL.Object &&
						GetTypeSL(CopyObject) != TypeSL.Object)
						return;
					break;
				case TypeSL.Object:
					if (!IsItem(CutObject) && !IsItem(CopyObject))
						return;
					break;
				default:
					if(IsInventoryContainer(tn))
					{
						if (!IsItem(CutObject) && !IsItem(CopyObject) &&
							GetTypeSL(CutObject) != TypeSL.Folder &&
							GetTypeSL(CopyObject) != TypeSL.Folder)
							return;
					}
					else if (!IsItem(tn)) // is special container
					{
						// must be a file
						if (!IsItem(CutObject) && !IsItem(CopyObject) &&
							GetTypeSL(CutObject) != TypeSL.Folder &&
							GetTypeSL(CopyObject) != TypeSL.Folder)
							return;
					}
					break;
			}

			if (CutObject != null) // Paste the Cut object
			{
				string strName = CutObject.Text;

				foreach(TreeNode treeNode in tn.Nodes)
				{
					if (treeNode.Text == strName)
					{
						MessageBox.Show("Destination already exists","Oops...");
						return;
					}
				}

				TreeNode Destination = new TreeNode(strName);
				tn.Nodes.Add(Destination);

				CopyChildren(CutObject, Destination);
				DeleteChildren(CutObject);

				CutObject.Remove();
				CutObject = null;
			}
			if (CopyObject != null) // Paste the Copy object
			{
				string strName;
				if (IsItem(CopyObject) && GetTypeSL(CopyObject) != TypeSL.Object)
					strName = GetNewFileName(tn, "Copy ({0}) of " + CopyObject.Text);
				else
					strName = GetNewDirectoryName(tn, "Copy ({0}) of " + CopyObject.Text);

				TreeNode Destination = new TreeNode(strName);
				tn.Nodes.Add(Destination);

				CopyChildren(CopyObject, Destination);

				CopyObject = null;

				if (tn == ActiveSolution)
					UpdateVisualSolutionName();
			}
		}

		private void RemoveAction(TreeNode tn)
		{
			if (GetTypeSL(tn) == TypeSL.Project)
			{
				SaveItemFile(tn); // saving pending updates!!
				if (ActiveProject == tn)
					ActiveProject = null;
				// Does not!!!! delete, only remove form project
				tn.Remove();
				UpdateVisualSolutionName();
				this.m_dirty = true;
			}
		}

		private void RenameAction(TreeNode tn)
		{
			RealTag realTag = (RealTag)tn.Tag;
			if (IsInventoryContainer(tn))
			{
				if(realTag.ItemType != TypeSL.Folder)
					return;
			}

			if (realTag.ItemType == TypeSL.Solution)
				tn.Text = this.NameSolution;
			this.treeView1.LabelEdit = true;
			tn.BeginEdit();
		}

        private void CutAction(TreeNode tn)
		{
			if (CutObject != null)
				CutObject.ForeColor = Color.Empty;
			CutObject = tn;
			tn.ForeColor = Color.DarkGray;
		}

		private void CopyAction(TreeNode tn)
		{
			if (CutObject != null)
			{
				CutObject.ForeColor = Color.Empty;
				CutObject = null;
			}
			CopyObject = tn;
		}

		public void AddNewProjectAction()
		{
			string strNewProjectName = GetNewDirectoryName(ActiveSolution, "Project{0}");
			string strProjectPath = Path.Combine(DirectorySolution, strNewProjectName);
			AddItem(TypeSL.Project, strNewProjectName, Guid.NewGuid(), strProjectPath);
		}

		private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			timer.Stop();
			timer.Tag = null;

			Guid guid = ((RealTag)e.Node.Tag).Guid;
            string path = GetFullPath(e.Node);

            // already opened
            EditForm editForm = GetEditForm(guid);
			if (editForm != null)
			{
				TabPage tabPage = editForm.Tag as TabPage;
				if (tabPage == null)
				{
                    if(editForm.Visible)
                    {
                        editForm.Focus();
                    } else
                    {
                        // Check if there's a related expanded lsl or lsli opened. If so, focus it. Else open the lsli.
                        EditForm expandedForm = (EditForm)parent.GetForm(Helpers.LSLIPathHelper.GetExpandedTabName(Helpers.LSLIPathHelper.CreateExpandedScriptName(Path.GetFileName(path))));
                        EditForm collapsedForm = (EditForm)parent.GetForm(Helpers.LSLIPathHelper.CreateCollapsedScriptName(Path.GetFileName(path)));
                        if (expandedForm != null && expandedForm.Visible)
                        {
                            expandedForm.Focus();
                        }
                        else if (collapsedForm != null && collapsedForm.Visible)
                        {
                            collapsedForm.Focus();
                        }
                        else
                        {
                            // Open a new one
                            if (GetTypeSL(e.Node) == TypeSL.Script || GetTypeSL(e.Node) == TypeSL.LSLIScript)
                            {
                                this.parent.OpenFile(GetFullPath(e.Node), guid, true);
                            }
                        }
                    }
				}
				else
				{
					TabControl tabControl = tabPage.Parent as TabControl;
					if(tabControl != null)
						tabControl.SelectedTab = tabPage;
				}
				return;
			}

            // Check if it's an lsli that has an open expanded form
            if (GetTypeSL(e.Node) == TypeSL.Script || GetTypeSL(e.Node) == TypeSL.LSLIScript)
            {
                if (Helpers.LSLIPathHelper.IsLSLI(path)) {
                    // Check if there's a related expanded lsl opened. If so, focus it. Else open the lsli.
                    EditForm expandedForm = (EditForm)parent.GetForm(Helpers.LSLIPathHelper.GetExpandedTabName(Helpers.LSLIPathHelper.CreateExpandedScriptName(Path.GetFileName(path))));
                    EditForm collapsedForm = (EditForm)parent.GetForm(Helpers.LSLIPathHelper.CreateCollapsedScriptName(Path.GetFileName(path)));
                    if (expandedForm != null && expandedForm.Visible)
                    {
                        expandedForm.Focus();
                    } else if(collapsedForm != null && collapsedForm.Visible)
                    {
                        collapsedForm.Focus();
                    } else
                    {
                        // Open a new one
                        if (GetTypeSL(e.Node) == TypeSL.Script || GetTypeSL(e.Node) == TypeSL.LSLIScript)
                        {
                            this.parent.OpenFile(GetFullPath(e.Node), guid, true);
                        }
                    }
                } else
                {
                    // Open a new one
                    if (GetTypeSL(e.Node) == TypeSL.Script || GetTypeSL(e.Node) == TypeSL.LSLIScript)
                    {
                        this.parent.OpenFile(GetFullPath(e.Node), guid, true);
                    }
                }
            }

			if (GetTypeSL(e.Node) == TypeSL.Notecard)
				this.parent.OpenFile(GetFullPath(e.Node), guid, false);
		}
		private void PropertiesActions(TreeNode tn)
		{
			SolutionExplorer.RealTag realTag = (SolutionExplorer.RealTag)tn.Tag;
			Guid oldGuid = realTag.Guid;
			GuidProperty prop = new GuidProperty(realTag);
			
			if (prop.ShowDialog(this) == DialogResult.OK)
			{
				EditForm editForm = GetEditForm(oldGuid);
				if (editForm != null)
					editForm.guid = prop.guid;
				realTag.Guid = prop.guid;
				tn.Tag = realTag;
				ShowProperties(tn);
			}
		}
		private void ShowProperties(TreeNode tn)
		{
			RealTag realTag = (RealTag)tn.Tag;

			this.listView1.Items.Clear();

			ListViewItem lvi = new ListViewItem("Name");
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
			lvi.SubItems.Add(GetFullPath(tn));
			this.listView1.Items.Add(lvi);

			lvi = new ListViewItem("Modified");
			FileInfo fi = new FileInfo(GetFullPath(tn));
			lvi.SubItems.Add(fi.LastWriteTime.ToString("s"));
			this.listView1.Items.Add(lvi);

		}

		private void SetAsActiveProject(TreeNode tn)
		{
			ActiveObject = null;
			ActiveProject = tn;
			foreach (TreeNode child in ActiveProject.Nodes)
			{
				if (GetTypeSL(child) == TypeSL.Object)
				{
					ActiveObject = child;
					break;
				}
			}
			this.treeView1.Invalidate();
		}

		private void SetAsActiveBox(TreeNode tn)
		{
			ActiveProject = null;
			ActiveObject = tn;
			if (GetTypeSL(ActiveObject.Parent) == TypeSL.Project)
				ActiveProject = ActiveObject.Parent;
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
			ShowProperties(e.Node);
		}

		private TreeNode FindGuid(TreeNode treeNode, Guid guid)
		{
			if (treeNode == null)
				return null;
			foreach (TreeNode tn in treeNode.Nodes)
			{
				RealTag rt = (RealTag)tn.Tag;
				if (rt.Guid == guid)
					return tn;
				TreeNode found = FindGuid(tn, guid);
				if (found != null)
					return found;
			}
			return null;
		}

		private delegate bool IHateThisToo(Guid guid, string name);
		private delegate string IHateThis(Guid guid);
		public string GetObjectName(Guid guid)
		{
			if (this.treeView1.InvokeRequired)
				return this.treeView1.Invoke(new IHateThis(GetObjectName), new object[] { guid }).ToString();
			TreeNode treeNode = FindGuid(this.treeView1.TopNode, guid);
			if (treeNode == null)
				return string.Empty;
			TreeNode parent = treeNode.Parent;
			RealTag realTag = (RealTag)parent.Tag;
			if (realTag.ItemType != TypeSL.Object)
				return string.Empty;
			return realTag.Name;
		}

		public bool SetObjectName(Guid guid, string name)
		{
			if (this.treeView1.InvokeRequired)
				return (bool)this.treeView1.Invoke(new IHateThisToo(SetObjectName), new object[] { guid, name });
			TreeNode treeNode = FindGuid(this.treeView1.TopNode, guid);
			if (treeNode == null)
				return false;
			TreeNode parent = treeNode.Parent;
			RealTag realTag = (RealTag)parent.Tag;
			if (realTag.ItemType != TypeSL.Object)
				return false;

			string strBackup = parent.Text;
			string strSource = GetFullPath(parent);
			parent.Text = name;
			string strDestination = GetFullPath(parent);

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
				return this.treeView1.Invoke(new IHateThis(GetObjectDescription), new object[] { guid }).ToString();
			TreeNode treeNode = FindGuid(this.treeView1.TopNode, guid);
			if (treeNode == null)
				return string.Empty;
			TreeNode parent = treeNode.Parent;
			RealTag realTag = (RealTag)parent.Tag;
			if (realTag.ItemType != TypeSL.Object)
				return string.Empty;
			return realTag.Description;
		}

		public bool SetObjectDescription(Guid guid, string description)
		{
			if (this.treeView1.InvokeRequired)
				return (bool)this.treeView1.Invoke(new IHateThisToo(SetObjectDescription), new object[] { guid, description });
			TreeNode treeNode = FindGuid(this.treeView1.TopNode, guid);
			if (treeNode == null)
				return false;
			TreeNode parent = treeNode.Parent;
			RealTag realTag = (RealTag)parent.Tag;
			if (realTag.ItemType != TypeSL.Object)
				return false;
			realTag.Description = description;
			parent.Tag = realTag;
			return true;
		}


		public string GetScriptName(Guid guid)
		{
			if (this.treeView1.InvokeRequired)
				return this.treeView1.Invoke(new IHateThis(GetScriptName), new object[] { guid }).ToString();
			TreeNode treeNode = FindGuid(this.treeView1.TopNode, guid);
			if (treeNode == null)
				return string.Empty;
			return treeNode.Text;
		}

		public string GetKey(Guid guid)
		{
			if (this.treeView1.InvokeRequired)
				return this.treeView1.Invoke(new IHateThis(GetKey), new object[] { guid }).ToString();

			TreeNode treeNode = FindGuid(this.treeView1.TopNode, guid);
			if (treeNode == null)
				return string.Empty;
			TreeNode parent = treeNode.Parent;
			RealTag rt = (RealTag)parent.Tag;
			return rt.Guid.ToString();
		}

		private TypeSL GetTypeSLFromInteger(int type)
		{
			TypeSL typeSL = TypeSL.Unknown;
			switch (type)
			{
				case -1:					//INVENTORY_ALL -1  all inventory items
					return TypeSL.Unknown;
				case 20:					//INVENTORY_ANIMATION  20  animations  0x14  
					return TypeSL.Animation;
				case 13:					//INVENTORY_BODYPART  13  body parts  0x0D  
					return TypeSL.Hair | TypeSL.Skin | TypeSL.Eyes | TypeSL.Shape;
				case 5:						//INVENTORY_CLOTHING  5  clothing  0x05  
					return TypeSL.Jacket | TypeSL.Gloves | TypeSL.Pants | TypeSL.Shirt | TypeSL.Shoes | TypeSL.Skirt | TypeSL.Socks | TypeSL.Underpants | TypeSL.Undershirt;
				case 21:					//INVENTORY_GESTURE  21  gestures  0x15  
					return TypeSL.Gesture;
				case 3:						//INVENTORY_LANDMARK  3  landmarks  0x03  
					return TypeSL.Landmark;
				case 7:						//INVENTORY_NOTECARD  7  notecards  0x07
					return TypeSL.Notecard;
				case 6:						//INVENTORY_OBJECT  6  objects  0x06  
					return TypeSL.Object;
				case 10:					//INVENTORY_SCRIPT  10  scripts  0x0A  
					return TypeSL.Script;
				case 1:						//INVENTORY_SOUND  1  sounds  0x01  
					return TypeSL.Sound;
				case 0:						//INVENTORY_TEXTURE  0  textures  0x00  
					return TypeSL.Texture;
				default:
					break;
			}
			return typeSL;
		}

		private delegate void DelegateRemoveInventory(Guid guid, SecondLife.String name);
		public void RemoveInventory(Guid guid, SecondLife.String name)
		{
			if (this.treeView1.InvokeRequired)
			{
				this.treeView1.Invoke(new DelegateRemoveInventory(RemoveInventory), new object[] { guid, name });
				return;
			}

			TreeNode treeNode = FindGuid(this.treeView1.TopNode, guid);
			if (treeNode == null)
				return;
			TreeNode parent = treeNode.Parent;
			for (int i = 0; i < parent.Nodes.Count; ++i)
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
				return this.treeView1.Invoke(new DelegateGetInventoryName(GetInventoryName), new object[] { guid, type, number }).ToString();
			}
			
			TypeSL typeSL = GetTypeSLFromInteger(type);

			TreeNode treeNode = FindGuid(this.treeView1.TopNode, guid);
			if (treeNode == null)
				return string.Empty;
			TreeNode parent = treeNode.Parent;
			int intI=0;
			foreach (TreeNode tn in parent.Nodes)
			{
				if (typeSL == TypeSL.Unknown || (typeSL & GetTypeSL(tn) ) == GetTypeSL(tn))
				{
					if (intI == number)
						return tn.Text;
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
				return this.treeView1.Invoke(new DelegateGetInventoryKey(GetInventoryKey), new object[] { guid, name }).ToString();
			}

			TreeNode treeNode = FindGuid(this.treeView1.TopNode, guid);
			if (treeNode == null)
				return string.Empty;
			TreeNode parent = treeNode.Parent;
			foreach (TreeNode tn in parent.Nodes)
			{
				if (tn.Text == name)
				{
					RealTag rt = (RealTag)tn.Tag;
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
				return (int)this.treeView1.Invoke(new DelegateGetInventoryType(GetInventoryType), new object[] { guid, name });
			}

			TreeNode treeNode = FindGuid(this.treeView1.TopNode, guid);
			if (treeNode == null)
				return SecondLife.INVENTORY_NONE;
			TreeNode parent = treeNode.Parent;
			foreach (TreeNode tn in parent.Nodes)
			{
				if (tn.Text == name)
				{
					RealTag rt = (RealTag)tn.Tag;
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
				return (int)this.treeView1.Invoke(new DelegateGetInventoryNumber(GetInventoryNumber), new object[] { guid, type });
			}

			TypeSL typeSL = GetTypeSLFromInteger(type);

			TreeNode treeNode = FindGuid(this.treeView1.TopNode, guid);
			if (treeNode == null)
				return -1;
			TreeNode parent = treeNode.Parent;
			int intI = 0;
			foreach (TreeNode tn in parent.Nodes)
			{
				if (typeSL == TypeSL.Unknown || (typeSL & GetTypeSL(tn)) == GetTypeSL(tn))
					intI++;
			}
			return intI;
		}

		private delegate string DelegateGetPath(Guid guid, string name);
		public string GetPath(Guid guid, string name)
		{
			if (this.treeView1.InvokeRequired)
			{
				return this.treeView1.Invoke(new DelegateGetPath(GetPath), new object[] { guid, name }).ToString();
			}

			TreeNode treeNode = FindGuid(this.treeView1.TopNode, guid);
			if (treeNode == null)
				return string.Empty;
			TreeNode parent = treeNode.Parent;
			foreach (TreeNode tn in parent.Nodes)
			{
				if (tn.Text == name || ((RealTag)tn.Tag).Guid.ToString() == name)
					return GetFullPath(tn);
			}
			return string.Empty;
		}

		private delegate Guid GetParentGuidDelegate(Guid guid);
		public Guid GetParentGuid(Guid guid)
		{
			if (this.treeView1.InvokeRequired)
			{
				return (Guid)this.treeView1.Invoke(new GetParentGuidDelegate(GetParentGuid), new object[] { guid });
			}
			TreeNode treeNode = FindGuid(this.treeView1.TopNode, guid);
			if (treeNode == null)
				return Guid.Empty;
			TreeNode parent = treeNode.Parent;
			// Only return parent when it is an object
			if (((RealTag)parent.Tag).ItemType != TypeSL.Object)
				return guid;
			else // Return object itself, has no parent object
				return ((RealTag)parent.Tag).Guid;
		}

		private delegate Guid GetGuidFromObjectNrDelegate(Guid ObjectGuid, int intLinkNum);
		public Guid GetGuidFromObjectNr(Guid ObjectGuid, int intLinkNum)
		{
			if (this.treeView1.InvokeRequired)
			{
				return (Guid)this.treeView1.Invoke(new GetGuidFromObjectNrDelegate(GetGuidFromObjectNr), new object[] { ObjectGuid, intLinkNum });
			}
			TreeNode treeNode = FindGuid(this.treeView1.TopNode, ObjectGuid);
			if (treeNode == null)
				return Guid.Empty;
			int intNr = 0;
			foreach (TreeNode tn in treeNode.Nodes)
			{
				RealTag realTag = (RealTag)tn.Tag;
				if (realTag.ItemType == TypeSL.Object)
				{
					intNr++;
					if (intNr == intLinkNum)
						return realTag.Guid;
				}
			}
			return Guid.Empty; // no objects found
		}

		private void GetScriptsFromNode(TreeNode treeNode, List<Guid> list, bool recursive)
		{
			foreach (TreeNode tn in treeNode.Nodes)
			{
				RealTag realTag = (RealTag)tn.Tag;
				if (realTag.ItemType == TypeSL.Script)
					list.Add(realTag.Guid);
				else
					if (realTag.ItemType == TypeSL.Object && recursive)
						GetScriptsFromNode(tn, list, recursive);
			}
		}

		private delegate List<Guid> GetScriptsDelegate(Guid guid, bool recursive);
		public List<Guid> GetScripts(Guid guid, bool recursive)
		{
			if (this.treeView1.InvokeRequired)
				return this.treeView1.Invoke(new GetScriptsDelegate(GetScripts), new object[] { guid, recursive }) as List<Guid>;

			List<Guid> list = new List<Guid>();
			TreeNode treeNode = FindGuid(this.treeView1.TopNode, guid);
			if (treeNode == null)
				return list; // empty
			GetScriptsFromNode(treeNode, list, recursive);
			return list;
		}

		private void listView1_MouseMove(object sender, MouseEventArgs e)
		{
			ListViewItem lvi = this.listView1.GetItemAt(e.X,e.Y);
			if (lvi == null)
				this.toolTip1.SetToolTip(this.listView1, "");
			else if (lvi != m_HoveredItem)
			{
				m_HoveredItem = lvi;
				this.toolTip1.SetToolTip(this.listView1, lvi.SubItems[1].Text);
			}
		}

		private void treeView1_KeyDown(object sender, KeyEventArgs e)
		{
			if (this.treeView1.SelectedNode == null)
				return;
			if (e.KeyCode == Keys.ControlKey)
				return;
			switch (e.KeyCode)
			{
				case Keys.F2:
					RenameAction(this.treeView1.SelectedNode);
					e.Handled = true;
					break;
				case Keys.Delete:
					DeleteAction(this.treeView1.SelectedNode);
					e.Handled = true;
					break;
				case Keys.C:
					if (e.Control)
					{
						CopyAction(this.treeView1.SelectedNode);
						e.SuppressKeyPress = true;
						e.Handled = true;
					}
					break;
				case Keys.V:
					if (e.Control)
					{
						PasteAction(this.treeView1.SelectedNode);
						e.SuppressKeyPress = true;
						e.Handled = true;
					}
					break;
				case Keys.X:
					if (e.Control)
					{
						CutAction(this.treeView1.SelectedNode);
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
			SvnAguments svnArguments = new SvnAguments();
			svnArguments.Text = "SVN import";
			svnArguments.Comment = "Initial import";
			if (svnArguments.ShowDialog(this) == DialogResult.OK)
			{
				string strRepository = svnArguments.Repository;
				string strMessage = svnArguments.Comment;

				strMessage = strMessage.Replace('"', '\'');

				string strNameOfDirectory = Path.GetFileName(this.DirectorySolution);

				if (!strRepository.EndsWith(strNameOfDirectory))
				{
					if (!strRepository.EndsWith("/"))
						strRepository += "/";
					strRepository += strNameOfDirectory;
				}

				this.Repository = strRepository;
				this.SaveSolutionFile();

				// import "c:\temp\a\b \d\dir" "file:///d:/temp/svn/...." -m "Initital "
				string strArguments = string.Format("import \"{0}\" \"{1}\" -m \"{2}\"", this.DirectorySolution, this.Repository, strMessage);
				Svn svn = new Svn();
				if (!svn.Execute(strArguments, false, true))
					return;
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
			Svn svn = new Svn();
			svn.Execute("update \"" + this.DirectorySolution + "\"",true, true);
		}

		private void SvnCommit()
		{
			SvnAguments svnArguments = new SvnAguments();
			svnArguments.Text = "SVN Commit";
			svnArguments.ReadOnly = true;
			svnArguments.Repository = this.Repository;
			svnArguments.Comment = "";
			if (svnArguments.ShowDialog(this) == DialogResult.OK)
			{
				Svn svn = new Svn();
				svn.Execute("commit \"" + this.DirectorySolution + "\" -m \"" + svnArguments.Comment+ "\"", true, true);
			}
		}

		private void cms_MouseClick(object sender, MouseEventArgs e)
		{
			ContextMenuStrip cms = sender as ContextMenuStrip;
			TreeNode tn = cms.Tag as TreeNode;
			ToolStripMenuItem tsi = cms.GetItemAt(e.X, e.Y) as ToolStripMenuItem;

			if (tsi == null)
				return; // TODO, why is this??

			if (tsi.DropDownItems.Count == 0)
				cms.Visible = false;
			this.treeView1.Invalidate(tn.Bounds);

			switch (tsi.Name)
			{
				case "SvnImport":
					SvnImport();
					break;
				case "SvnUpdate":
					SvnUpdate();
					break;
				case "SvnCommit":
					SvnCommit();
					break;
				case "CloseSolution":
					CloseSolution();
					break;
				case "AddExistingFile":
					AddExistingFile(tn);
					break;
				case "AddExistingProject":
					AddExistingProject();
					break;
				case "AddNewProject":
					AddNewProjectAction();
					break;
				case "AddNewObject":
					AddNewContainer(tn, TypeSL.Object, GetNewDirectoryName(tn, "Object{0}"), Guid.NewGuid());
					break;
				case "AddNewFolder":
					AddNewContainer(tn, TypeSL.Folder, GetNewDirectoryName(tn, "Folder{0}"), Guid.NewGuid());
					break;
				case "AddInventory":
					EmptyInventory(tn);
					break;
				case "SetAsActiveProject":
					SetAsActiveProject(tn);
					break;
				case "SetAsActiveBox":
					SetAsActiveBox(tn);
					break;
				case "Cut":
					CutAction(tn);
					break;
				case "Copy":
					CopyAction(tn);
					break;
				case "Delete":
					DeleteAction(tn);
					break;
				case "Paste":
					PasteAction(tn);
					break;
				case "Remove":
					RemoveAction(tn);
					break;
				case "Rename":
					RenameAction(tn);
					break;
				case "Add":
					// via dropdownitems
					return;
				case "ChangeFileType":
					// via dropdownitems
					return;
				case "Properties":
					PropertiesActions(tn);
					break;
				case "DebugStart":
					DebugStart();
					break;
				case "AddExistingObject":
					AddExistingObject(tn);
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
			TreeNode tx = x as TreeNode;
			TreeNode ty = y as TreeNode;
			return string.Compare(tx.Text, ty.Text);
		}
	}


}
