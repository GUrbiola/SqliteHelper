using System.Drawing;
using System.Windows.Forms;

namespace SqliteHelper48
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.recentProjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.createSqliteFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.closeProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataBaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDataBaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.recentDatabasesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.runQueryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.closeDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCopyOfDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createClaudeDocumentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tablesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cloneTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.projectStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.databaseStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.databaseTreeView = new System.Windows.Forms.TreeView();
            this.treeViewImageList = new System.Windows.Forms.ImageList(this.components);
            this.objectInfoPanel = new System.Windows.Forms.Panel();
            this.objectInfoTextBox = new System.Windows.Forms.RichTextBox();
            this.objectInfoLabel = new System.Windows.Forms.Label();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.gridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeManager1 = new ZidUtilities.CommonCode.Win.Controls.ThemeManager(this.components);
            this.menuStrip.SuspendLayout();
            this.tablesContextMenu.SuspendLayout();
            this.tableContextMenu.SuspendLayout();
            this.viewContextMenu.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.objectInfoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.gridContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectToolStripMenuItem,
            this.dataBaseToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1019, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createProjectToolStripMenuItem,
            this.openProjectToolStripMenuItem,
            this.toolStripSeparator1,
            this.recentProjectsToolStripMenuItem,
            this.toolStripSeparator2,
            this.createSqliteFileToolStripMenuItem,
            this.toolStripSeparator3,
            this.closeProjectToolStripMenuItem});
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.projectToolStripMenuItem.Text = "&Project";
            // 
            // createProjectToolStripMenuItem
            // 
            this.createProjectToolStripMenuItem.Name = "createProjectToolStripMenuItem";
            this.createProjectToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.createProjectToolStripMenuItem.Text = "&Create Project...";
            this.createProjectToolStripMenuItem.Click += new System.EventHandler(this.createProjectToolStripMenuItem_Click);
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.openProjectToolStripMenuItem.Text = "&Open Project...";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(158, 6);
            // 
            // recentProjectsToolStripMenuItem
            // 
            this.recentProjectsToolStripMenuItem.Name = "recentProjectsToolStripMenuItem";
            this.recentProjectsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.recentProjectsToolStripMenuItem.Text = "&Recent Projects";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(158, 6);
            // 
            // createSqliteFileToolStripMenuItem
            // 
            this.createSqliteFileToolStripMenuItem.Enabled = false;
            this.createSqliteFileToolStripMenuItem.Name = "createSqliteFileToolStripMenuItem";
            this.createSqliteFileToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.createSqliteFileToolStripMenuItem.Text = "Create Sqlite File";
            this.createSqliteFileToolStripMenuItem.Click += new System.EventHandler(this.createSqliteFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(158, 6);
            // 
            // closeProjectToolStripMenuItem
            // 
            this.closeProjectToolStripMenuItem.Enabled = false;
            this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
            this.closeProjectToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.closeProjectToolStripMenuItem.Text = "C&lose Project";
            this.closeProjectToolStripMenuItem.Click += new System.EventHandler(this.closeProjectToolStripMenuItem_Click);
            // 
            // dataBaseToolStripMenuItem
            // 
            this.dataBaseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDataBaseToolStripMenuItem,
            this.toolStripSeparator4,
            this.recentDatabasesToolStripMenuItem,
            this.toolStripSeparator5,
            this.runQueryToolStripMenuItem,
            this.reloadDatabaseToolStripMenuItem,
            this.toolStripSeparator6,
            this.closeDatabaseToolStripMenuItem,
            this.saveCopyOfDatabaseToolStripMenuItem,
            this.createClaudeDocumentationToolStripMenuItem});
            this.dataBaseToolStripMenuItem.Name = "dataBaseToolStripMenuItem";
            this.dataBaseToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.dataBaseToolStripMenuItem.Text = "&Data Base";
            // 
            // openDataBaseToolStripMenuItem
            // 
            this.openDataBaseToolStripMenuItem.Name = "openDataBaseToolStripMenuItem";
            this.openDataBaseToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.openDataBaseToolStripMenuItem.Text = "&Open Data Base...";
            this.openDataBaseToolStripMenuItem.Click += new System.EventHandler(this.openDataBaseToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(243, 6);
            // 
            // recentDatabasesToolStripMenuItem
            // 
            this.recentDatabasesToolStripMenuItem.Name = "recentDatabasesToolStripMenuItem";
            this.recentDatabasesToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.recentDatabasesToolStripMenuItem.Text = "&Recent Databases";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(243, 6);
            // 
            // runQueryToolStripMenuItem
            // 
            this.runQueryToolStripMenuItem.Enabled = false;
            this.runQueryToolStripMenuItem.Name = "runQueryToolStripMenuItem";
            this.runQueryToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.runQueryToolStripMenuItem.Text = "&Run Query...";
            this.runQueryToolStripMenuItem.Click += new System.EventHandler(this.runQueryToolStripMenuItem_Click);
            // 
            // reloadDatabaseToolStripMenuItem
            // 
            this.reloadDatabaseToolStripMenuItem.Enabled = false;
            this.reloadDatabaseToolStripMenuItem.Name = "reloadDatabaseToolStripMenuItem";
            this.reloadDatabaseToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.reloadDatabaseToolStripMenuItem.Text = "Re&load Database";
            this.reloadDatabaseToolStripMenuItem.Click += new System.EventHandler(this.reloadDatabaseToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(243, 6);
            // 
            // closeDatabaseToolStripMenuItem
            // 
            this.closeDatabaseToolStripMenuItem.Enabled = false;
            this.closeDatabaseToolStripMenuItem.Name = "closeDatabaseToolStripMenuItem";
            this.closeDatabaseToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.closeDatabaseToolStripMenuItem.Text = "&Close Database";
            this.closeDatabaseToolStripMenuItem.Click += new System.EventHandler(this.closeDatabaseToolStripMenuItem_Click);
            // 
            // saveCopyOfDatabaseToolStripMenuItem
            // 
            this.saveCopyOfDatabaseToolStripMenuItem.Enabled = false;
            this.saveCopyOfDatabaseToolStripMenuItem.Name = "saveCopyOfDatabaseToolStripMenuItem";
            this.saveCopyOfDatabaseToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.saveCopyOfDatabaseToolStripMenuItem.Text = "&Save Copy of Current Database...";
            this.saveCopyOfDatabaseToolStripMenuItem.Click += new System.EventHandler(this.saveCopyOfDatabaseToolStripMenuItem_Click);
            // 
            // createClaudeDocumentationToolStripMenuItem
            //
            this.createClaudeDocumentationToolStripMenuItem.Enabled = false;
            this.createClaudeDocumentationToolStripMenuItem.Name = "createClaudeDocumentationToolStripMenuItem";
            this.createClaudeDocumentationToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.createClaudeDocumentationToolStripMenuItem.Text = "Create Claude Documentation";
            this.createClaudeDocumentationToolStripMenuItem.Click += new System.EventHandler(this.createClaudeDocumentationToolStripMenuItem_Click);
            //
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.themeToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // themeToolStripMenuItem
            // 
            this.themeToolStripMenuItem.Name = "themeToolStripMenuItem";
            this.themeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.themeToolStripMenuItem.Text = "&Theme";
            // 
            // tablesContextMenu
            // 
            this.tablesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createTableToolStripMenuItem});
            this.tablesContextMenu.Name = "tablesContextMenu";
            this.tablesContextMenu.Size = new System.Drawing.Size(175, 26);
            // 
            // createTableToolStripMenuItem
            // 
            this.createTableToolStripMenuItem.Name = "createTableToolStripMenuItem";
            this.createTableToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.createTableToolStripMenuItem.Text = "Create New Table...";
            this.createTableToolStripMenuItem.Click += new System.EventHandler(this.createTableToolStripMenuItem_Click);
            // 
            // tableContextMenu
            // 
            this.tableContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editTableToolStripMenuItem,
            this.copyTableToolStripMenuItem,
            this.cloneTableToolStripMenuItem,
            this.deleteTableToolStripMenuItem});
            this.tableContextMenu.Name = "tableContextMenu";
            this.tableContextMenu.Size = new System.Drawing.Size(145, 92);
            // 
            // editTableToolStripMenuItem
            // 
            this.editTableToolStripMenuItem.Name = "editTableToolStripMenuItem";
            this.editTableToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.editTableToolStripMenuItem.Text = "Edit Table...";
            this.editTableToolStripMenuItem.Click += new System.EventHandler(this.editTableToolStripMenuItem_Click);
            // 
            // copyTableToolStripMenuItem
            // 
            this.copyTableToolStripMenuItem.Name = "copyTableToolStripMenuItem";
            this.copyTableToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.copyTableToolStripMenuItem.Text = "Copy Table...";
            this.copyTableToolStripMenuItem.Click += new System.EventHandler(this.copyTableToolStripMenuItem_Click);
            // 
            // cloneTableToolStripMenuItem
            // 
            this.cloneTableToolStripMenuItem.Name = "cloneTableToolStripMenuItem";
            this.cloneTableToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.cloneTableToolStripMenuItem.Text = "Clone Table...";
            this.cloneTableToolStripMenuItem.Click += new System.EventHandler(this.cloneTableToolStripMenuItem_Click);
            // 
            // deleteTableToolStripMenuItem
            // 
            this.deleteTableToolStripMenuItem.Name = "deleteTableToolStripMenuItem";
            this.deleteTableToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.deleteTableToolStripMenuItem.Text = "Delete Table";
            this.deleteTableToolStripMenuItem.Click += new System.EventHandler(this.deleteTableToolStripMenuItem_Click);
            // 
            // viewContextMenu
            // 
            this.viewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteViewToolStripMenuItem});
            this.viewContextMenu.Name = "viewContextMenu";
            this.viewContextMenu.Size = new System.Drawing.Size(136, 26);
            // 
            // deleteViewToolStripMenuItem
            // 
            this.deleteViewToolStripMenuItem.Name = "deleteViewToolStripMenuItem";
            this.deleteViewToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.deleteViewToolStripMenuItem.Text = "Delete View";
            this.deleteViewToolStripMenuItem.Click += new System.EventHandler(this.deleteViewToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.projectStatusLabel,
            this.databaseStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 636);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip.Size = new System.Drawing.Size(1019, 24);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 19);
            this.toolStripStatusLabel.Text = "Ready";
            // 
            // projectStatusLabel
            // 
            this.projectStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.projectStatusLabel.Name = "projectStatusLabel";
            this.projectStatusLabel.Size = new System.Drawing.Size(67, 19);
            this.projectStatusLabel.Text = "No project";
            // 
            // databaseStatusLabel
            // 
            this.databaseStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.databaseStatusLabel.Name = "databaseStatusLabel";
            this.databaseStatusLabel.Size = new System.Drawing.Size(77, 19);
            this.databaseStatusLabel.Text = "No database";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView);
            this.splitContainer1.Size = new System.Drawing.Size(1019, 612);
            this.splitContainer1.SplitterDistance = 381;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.databaseTreeView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.objectInfoPanel);
            this.splitContainer2.Size = new System.Drawing.Size(381, 612);
            this.splitContainer2.SplitterDistance = 377;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 0;
            // 
            // databaseTreeView
            // 
            this.databaseTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.databaseTreeView.Enabled = false;
            this.databaseTreeView.ImageIndex = 0;
            this.databaseTreeView.ImageList = this.treeViewImageList;
            this.databaseTreeView.Location = new System.Drawing.Point(0, 0);
            this.databaseTreeView.Name = "databaseTreeView";
            this.databaseTreeView.SelectedImageIndex = 0;
            this.databaseTreeView.Size = new System.Drawing.Size(381, 377);
            this.databaseTreeView.TabIndex = 0;
            this.databaseTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.databaseTreeView_AfterSelect);
            // 
            // treeViewImageList
            // 
            this.treeViewImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.treeViewImageList.ImageSize = new System.Drawing.Size(32, 32);
            this.treeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // objectInfoPanel
            // 
            this.objectInfoPanel.Controls.Add(this.objectInfoTextBox);
            this.objectInfoPanel.Controls.Add(this.objectInfoLabel);
            this.objectInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.objectInfoPanel.Name = "objectInfoPanel";
            this.objectInfoPanel.Size = new System.Drawing.Size(381, 232);
            this.objectInfoPanel.TabIndex = 0;
            // 
            // objectInfoTextBox
            // 
            this.objectInfoTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.objectInfoTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectInfoTextBox.Enabled = false;
            this.objectInfoTextBox.Location = new System.Drawing.Point(0, 17);
            this.objectInfoTextBox.Name = "objectInfoTextBox";
            this.objectInfoTextBox.ReadOnly = true;
            this.objectInfoTextBox.Size = new System.Drawing.Size(381, 215);
            this.objectInfoTextBox.TabIndex = 1;
            this.objectInfoTextBox.Text = "";
            // 
            // objectInfoLabel
            // 
            this.objectInfoLabel.BackColor = System.Drawing.SystemColors.Control;
            this.objectInfoLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.objectInfoLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.objectInfoLabel.Location = new System.Drawing.Point(0, 0);
            this.objectInfoLabel.Name = "objectInfoLabel";
            this.objectInfoLabel.Padding = new System.Windows.Forms.Padding(4, 3, 0, 0);
            this.objectInfoLabel.Size = new System.Drawing.Size(381, 17);
            this.objectInfoLabel.TabIndex = 0;
            this.objectInfoLabel.Text = "Object Information";
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.ContextMenuStrip = this.gridContextMenu;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Enabled = false;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(635, 612);
            this.dataGridView.TabIndex = 0;
            // 
            // gridContextMenu
            // 
            this.gridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRowToolStripMenuItem,
            this.editRowToolStripMenuItem,
            this.deleteRowToolStripMenuItem});
            this.gridContextMenu.Name = "gridContextMenu";
            this.gridContextMenu.Size = new System.Drawing.Size(134, 70);
            // 
            // addRowToolStripMenuItem
            // 
            this.addRowToolStripMenuItem.Name = "addRowToolStripMenuItem";
            this.addRowToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.addRowToolStripMenuItem.Text = "Add Row";
            this.addRowToolStripMenuItem.Click += new System.EventHandler(this.addRowToolStripMenuItem_Click);
            // 
            // editRowToolStripMenuItem
            // 
            this.editRowToolStripMenuItem.Name = "editRowToolStripMenuItem";
            this.editRowToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.editRowToolStripMenuItem.Text = "Edit Row";
            this.editRowToolStripMenuItem.Click += new System.EventHandler(this.editRowToolStripMenuItem_Click);
            // 
            // deleteRowToolStripMenuItem
            // 
            this.deleteRowToolStripMenuItem.Name = "deleteRowToolStripMenuItem";
            this.deleteRowToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.deleteRowToolStripMenuItem.Text = "Delete Row";
            this.deleteRowToolStripMenuItem.Click += new System.EventHandler(this.deleteRowToolStripMenuItem_Click);
            // 
            // themeManager1
            // 
            this.themeManager1.ExcludedControlTypes.Add("ICSharpCode.TextEditor.TextEditorControl");
            this.themeManager1.ExcludedControlTypes.Add("ZidUtilities.CommonCode.ICSharpTextEditor.ExtendedEditor");
            this.themeManager1.ExcludedControlTypes.Add("ICSharpCode.TextEditor.TextEditorControl");
            this.themeManager1.ExcludedControlTypes.Add("ZidUtilities.CommonCode.ICSharpTextEditor.ExtendedEditor");
            this.themeManager1.ExcludedControlTypes.Add("ICSharpCode.TextEditor.TextEditorControl");
            this.themeManager1.ExcludedControlTypes.Add("ZidUtilities.CommonCode.ICSharpTextEditor.ExtendedEditor");
            this.themeManager1.ExcludedControlTypes.Add("ICSharpCode.TextEditor.TextEditorControl");
            this.themeManager1.ExcludedControlTypes.Add("ZidUtilities.CommonCode.ICSharpTextEditor.ExtendedEditor");
            this.themeManager1.ExcludedControlTypes.Add("ICSharpCode.TextEditor.TextEditorControl");
            this.themeManager1.ExcludedControlTypes.Add("ZidUtilities.CommonCode.ICSharpTextEditor.ExtendedEditor");
            this.themeManager1.ParentForm = this;
            this.themeManager1.Theme = ZidUtilities.CommonCode.Win.ZidThemes.CodeProject;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 660);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SQLite Helper - Database Project Manager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tablesContextMenu.ResumeLayout(false);
            this.tableContextMenu.ResumeLayout(false);
            this.viewContextMenu.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.objectInfoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.gridContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem projectToolStripMenuItem;
        private ToolStripMenuItem createProjectToolStripMenuItem;
        private ToolStripMenuItem openProjectToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem recentProjectsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem createSqliteFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem closeProjectToolStripMenuItem;
        private ToolStripMenuItem dataBaseToolStripMenuItem;
        private ToolStripMenuItem openDataBaseToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem recentDatabasesToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem runQueryToolStripMenuItem;
        private ToolStripMenuItem reloadDatabaseToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem closeDatabaseToolStripMenuItem;
        private ToolStripMenuItem saveCopyOfDatabaseToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem themeToolStripMenuItem;
        private ContextMenuStrip tablesContextMenu;
        private ToolStripMenuItem createTableToolStripMenuItem;
        private ContextMenuStrip tableContextMenu;
        private ToolStripMenuItem editTableToolStripMenuItem;
        private ToolStripMenuItem copyTableToolStripMenuItem;
        private ToolStripMenuItem cloneTableToolStripMenuItem;
        private ToolStripMenuItem deleteTableToolStripMenuItem;
        private ContextMenuStrip viewContextMenu;
        private ToolStripMenuItem deleteViewToolStripMenuItem;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private ToolStripStatusLabel projectStatusLabel;
        private ToolStripStatusLabel databaseStatusLabel;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private TreeView databaseTreeView;
        private ImageList treeViewImageList;
        private Panel objectInfoPanel;
        private Label objectInfoLabel;
        private RichTextBox objectInfoTextBox;
        private DataGridView dataGridView;
        private ContextMenuStrip gridContextMenu;
        private ToolStripMenuItem addRowToolStripMenuItem;
        private ToolStripMenuItem editRowToolStripMenuItem;
        private ToolStripMenuItem deleteRowToolStripMenuItem;
        private ZidUtilities.CommonCode.Win.Controls.ThemeManager themeManager1;
        private ToolStripMenuItem createClaudeDocumentationToolStripMenuItem;
    }
}
