namespace SqliteHelper
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip = new MenuStrip();
            projectToolStripMenuItem = new ToolStripMenuItem();
            createProjectToolStripMenuItem = new ToolStripMenuItem();
            openProjectToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            recentProjectsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            createSqliteFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            closeProjectToolStripMenuItem = new ToolStripMenuItem();
            dataBaseToolStripMenuItem = new ToolStripMenuItem();
            openDataBaseToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            recentDatabasesToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            runQueryToolStripMenuItem = new ToolStripMenuItem();
            reloadDatabaseToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            closeDatabaseToolStripMenuItem = new ToolStripMenuItem();
            saveCopyOfDatabaseToolStripMenuItem = new ToolStripMenuItem();
            tablesContextMenu = new ContextMenuStrip(components);
            createTableToolStripMenuItem = new ToolStripMenuItem();
            tableContextMenu = new ContextMenuStrip(components);
            deleteTableToolStripMenuItem = new ToolStripMenuItem();
            viewContextMenu = new ContextMenuStrip(components);
            deleteViewToolStripMenuItem = new ToolStripMenuItem();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            projectStatusLabel = new ToolStripStatusLabel();
            databaseStatusLabel = new ToolStripStatusLabel();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            databaseTreeView = new TreeView();
            treeViewImageList = new ImageList(components);
            objectInfoPanel = new Panel();
            objectInfoTextBox = new RichTextBox();
            objectInfoLabel = new Label();
            dataGridView = new DataGridView();
            gridContextMenu = new ContextMenuStrip(components);
            addRowToolStripMenuItem = new ToolStripMenuItem();
            editRowToolStripMenuItem = new ToolStripMenuItem();
            deleteRowToolStripMenuItem = new ToolStripMenuItem();
            menuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            tablesContextMenu.SuspendLayout();
            tableContextMenu.SuspendLayout();
            viewContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            objectInfoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            gridContextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { projectToolStripMenuItem, dataBaseToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(800, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            //
            // projectToolStripMenuItem
            //
            projectToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { createProjectToolStripMenuItem, openProjectToolStripMenuItem, toolStripSeparator1, recentProjectsToolStripMenuItem, toolStripSeparator2, createSqliteFileToolStripMenuItem, toolStripSeparator3, closeProjectToolStripMenuItem });
            projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            projectToolStripMenuItem.Size = new Size(56, 20);
            projectToolStripMenuItem.Text = "&Project";
            projectToolStripMenuItem.DropDownOpening += projectToolStripMenuItem_DropDownOpening;
            // 
            // createProjectToolStripMenuItem
            // 
            createProjectToolStripMenuItem.Name = "createProjectToolStripMenuItem";
            createProjectToolStripMenuItem.Size = new Size(180, 22);
            createProjectToolStripMenuItem.Text = "&Create Project...";
            createProjectToolStripMenuItem.Click += createProjectToolStripMenuItem_Click;
            // 
            // openProjectToolStripMenuItem
            // 
            openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            openProjectToolStripMenuItem.Size = new Size(180, 22);
            openProjectToolStripMenuItem.Text = "&Open Project...";
            openProjectToolStripMenuItem.Click += openProjectToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // recentProjectsToolStripMenuItem
            // 
            recentProjectsToolStripMenuItem.Name = "recentProjectsToolStripMenuItem";
            recentProjectsToolStripMenuItem.Size = new Size(180, 22);
            recentProjectsToolStripMenuItem.Text = "&Recent Projects";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(177, 6);
            //
            // createSqliteFileToolStripMenuItem
            //
            createSqliteFileToolStripMenuItem.Enabled = false;
            createSqliteFileToolStripMenuItem.Name = "createSqliteFileToolStripMenuItem";
            createSqliteFileToolStripMenuItem.Size = new Size(180, 22);
            createSqliteFileToolStripMenuItem.Text = "Create Sqlite File";
            createSqliteFileToolStripMenuItem.Click += createSqliteFileToolStripMenuItem_Click;
            //
            // toolStripSeparator3
            //
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(177, 6);
            //
            // closeProjectToolStripMenuItem
            //
            closeProjectToolStripMenuItem.Enabled = false;
            closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
            closeProjectToolStripMenuItem.Size = new Size(180, 22);
            closeProjectToolStripMenuItem.Text = "C&lose Project";
            closeProjectToolStripMenuItem.Click += closeProjectToolStripMenuItem_Click;
            //
            // dataBaseToolStripMenuItem
            //
            dataBaseToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openDataBaseToolStripMenuItem, toolStripSeparator4, recentDatabasesToolStripMenuItem, toolStripSeparator5, runQueryToolStripMenuItem, reloadDatabaseToolStripMenuItem, toolStripSeparator6, closeDatabaseToolStripMenuItem, saveCopyOfDatabaseToolStripMenuItem });
            dataBaseToolStripMenuItem.Name = "dataBaseToolStripMenuItem";
            dataBaseToolStripMenuItem.Size = new Size(70, 20);
            dataBaseToolStripMenuItem.Text = "&Data Base";
            //
            // openDataBaseToolStripMenuItem
            //
            openDataBaseToolStripMenuItem.Name = "openDataBaseToolStripMenuItem";
            openDataBaseToolStripMenuItem.Size = new Size(230, 22);
            openDataBaseToolStripMenuItem.Text = "&Open Data Base...";
            openDataBaseToolStripMenuItem.Click += openDataBaseToolStripMenuItem_Click;
            //
            // toolStripSeparator4
            //
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(227, 6);
            //
            // recentDatabasesToolStripMenuItem
            //
            recentDatabasesToolStripMenuItem.Name = "recentDatabasesToolStripMenuItem";
            recentDatabasesToolStripMenuItem.Size = new Size(230, 22);
            recentDatabasesToolStripMenuItem.Text = "&Recent Databases";
            //
            // toolStripSeparator5
            //
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(227, 6);
            //
            // runQueryToolStripMenuItem
            //
            runQueryToolStripMenuItem.Enabled = false;
            runQueryToolStripMenuItem.Name = "runQueryToolStripMenuItem";
            runQueryToolStripMenuItem.Size = new Size(230, 22);
            runQueryToolStripMenuItem.Text = "&Run Query...";
            runQueryToolStripMenuItem.Click += runQueryToolStripMenuItem_Click;
            //
            // reloadDatabaseToolStripMenuItem
            //
            reloadDatabaseToolStripMenuItem.Enabled = false;
            reloadDatabaseToolStripMenuItem.Name = "reloadDatabaseToolStripMenuItem";
            reloadDatabaseToolStripMenuItem.Size = new Size(230, 22);
            reloadDatabaseToolStripMenuItem.Text = "Re&load Database";
            reloadDatabaseToolStripMenuItem.Click += reloadDatabaseToolStripMenuItem_Click;
            //
            // toolStripSeparator6
            //
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(227, 6);
            //
            // closeDatabaseToolStripMenuItem
            //
            closeDatabaseToolStripMenuItem.Enabled = false;
            closeDatabaseToolStripMenuItem.Name = "closeDatabaseToolStripMenuItem";
            closeDatabaseToolStripMenuItem.Size = new Size(230, 22);
            closeDatabaseToolStripMenuItem.Text = "&Close Database";
            closeDatabaseToolStripMenuItem.Click += closeDatabaseToolStripMenuItem_Click;
            //
            // saveCopyOfDatabaseToolStripMenuItem
            //
            saveCopyOfDatabaseToolStripMenuItem.Enabled = false;
            saveCopyOfDatabaseToolStripMenuItem.Name = "saveCopyOfDatabaseToolStripMenuItem";
            saveCopyOfDatabaseToolStripMenuItem.Size = new Size(230, 22);
            saveCopyOfDatabaseToolStripMenuItem.Text = "&Save Copy of Current Database...";
            saveCopyOfDatabaseToolStripMenuItem.Click += saveCopyOfDatabaseToolStripMenuItem_Click;
            //
            // tablesContextMenu
            //
            tablesContextMenu.Items.AddRange(new ToolStripItem[] { createTableToolStripMenuItem });
            tablesContextMenu.Name = "tablesContextMenu";
            tablesContextMenu.Size = new Size(170, 26);
            //
            // createTableToolStripMenuItem
            //
            createTableToolStripMenuItem.Name = "createTableToolStripMenuItem";
            createTableToolStripMenuItem.Size = new Size(169, 22);
            createTableToolStripMenuItem.Text = "Create New Table...";
            createTableToolStripMenuItem.Click += createTableToolStripMenuItem_Click;
            //
            // tableContextMenu
            //
            tableContextMenu.Items.AddRange(new ToolStripItem[] { deleteTableToolStripMenuItem });
            tableContextMenu.Name = "tableContextMenu";
            tableContextMenu.Size = new Size(145, 26);
            //
            // deleteTableToolStripMenuItem
            //
            deleteTableToolStripMenuItem.Name = "deleteTableToolStripMenuItem";
            deleteTableToolStripMenuItem.Size = new Size(144, 22);
            deleteTableToolStripMenuItem.Text = "Delete Table";
            deleteTableToolStripMenuItem.Click += deleteTableToolStripMenuItem_Click;
            //
            // viewContextMenu
            //
            viewContextMenu.Items.AddRange(new ToolStripItem[] { deleteViewToolStripMenuItem });
            viewContextMenu.Name = "viewContextMenu";
            viewContextMenu.Size = new Size(140, 26);
            //
            // deleteViewToolStripMenuItem
            //
            deleteViewToolStripMenuItem.Name = "deleteViewToolStripMenuItem";
            deleteViewToolStripMenuItem.Size = new Size(139, 22);
            deleteViewToolStripMenuItem.Text = "Delete View";
            deleteViewToolStripMenuItem.Click += deleteViewToolStripMenuItem_Click;
            //
            // statusStrip
            //
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel, projectStatusLabel, databaseStatusLabel });
            statusStrip.Location = new Point(0, 428);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(800, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip1";
            //
            // toolStripStatusLabel
            //
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(39, 17);
            toolStripStatusLabel.Text = "Ready";
            //
            // projectStatusLabel
            //
            projectStatusLabel.BorderSides = ToolStripStatusLabelBorderSides.Left;
            projectStatusLabel.Name = "projectStatusLabel";
            projectStatusLabel.Size = new Size(84, 17);
            projectStatusLabel.Text = "No project";
            //
            // databaseStatusLabel
            //
            databaseStatusLabel.BorderSides = ToolStripStatusLabelBorderSides.Left;
            databaseStatusLabel.Name = "databaseStatusLabel";
            databaseStatusLabel.Size = new Size(89, 17);
            databaseStatusLabel.Text = "No database";
            //
            // splitContainer1
            //
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Name = "splitContainer1";
            //
            // splitContainer1.Panel1
            //
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            //
            // splitContainer1.Panel2
            //
            splitContainer1.Panel2.Controls.Add(dataGridView);
            splitContainer1.Size = new Size(800, 404);
            splitContainer1.SplitterDistance = 300;
            splitContainer1.TabIndex = 2;
            //
            // splitContainer2
            //
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            //
            // splitContainer2.Panel1
            //
            splitContainer2.Panel1.Controls.Add(databaseTreeView);
            //
            // splitContainer2.Panel2
            //
            splitContainer2.Panel2.Controls.Add(objectInfoPanel);
            splitContainer2.Size = new Size(300, 404);
            splitContainer2.SplitterDistance = 250;
            splitContainer2.TabIndex = 0;
            //
            // databaseTreeView
            //
            databaseTreeView.Dock = DockStyle.Fill;
            databaseTreeView.Enabled = false;
            databaseTreeView.ImageIndex = 0;
            databaseTreeView.ImageList = treeViewImageList;
            databaseTreeView.Location = new Point(0, 0);
            databaseTreeView.Name = "databaseTreeView";
            databaseTreeView.SelectedImageIndex = 0;
            databaseTreeView.Size = new Size(300, 250);
            databaseTreeView.TabIndex = 0;
            databaseTreeView.AfterSelect += databaseTreeView_AfterSelect;
            //
            // treeViewImageList
            //
            treeViewImageList.ColorDepth = ColorDepth.Depth32Bit;
            treeViewImageList.ImageSize = new Size(16, 16);
            treeViewImageList.TransparentColor = Color.Transparent;
            //
            // objectInfoPanel
            //
            objectInfoPanel.Controls.Add(objectInfoTextBox);
            objectInfoPanel.Controls.Add(objectInfoLabel);
            objectInfoPanel.Dock = DockStyle.Fill;
            objectInfoPanel.Location = new Point(0, 0);
            objectInfoPanel.Name = "objectInfoPanel";
            objectInfoPanel.Size = new Size(300, 150);
            objectInfoPanel.TabIndex = 0;
            //
            // objectInfoTextBox
            //
            objectInfoTextBox.BackColor = SystemColors.Window;
            objectInfoTextBox.Dock = DockStyle.Fill;
            objectInfoTextBox.Enabled = false;
            objectInfoTextBox.Location = new Point(0, 20);
            objectInfoTextBox.Name = "objectInfoTextBox";
            objectInfoTextBox.ReadOnly = true;
            objectInfoTextBox.Size = new Size(300, 130);
            objectInfoTextBox.TabIndex = 1;
            objectInfoTextBox.Text = "";
            //
            // objectInfoLabel
            //
            objectInfoLabel.BackColor = SystemColors.Control;
            objectInfoLabel.Dock = DockStyle.Top;
            objectInfoLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            objectInfoLabel.Location = new Point(0, 0);
            objectInfoLabel.Name = "objectInfoLabel";
            objectInfoLabel.Padding = new Padding(5, 3, 0, 0);
            objectInfoLabel.Size = new Size(300, 20);
            objectInfoLabel.TabIndex = 0;
            objectInfoLabel.Text = "Object Information";
            //
            // dataGridView
            //
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.ContextMenuStrip = gridContextMenu;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.Enabled = false;
            dataGridView.Location = new Point(0, 0);
            dataGridView.Name = "dataGridView";
            dataGridView.Size = new Size(496, 404);
            dataGridView.TabIndex = 0;
            //
            // gridContextMenu
            //
            gridContextMenu.Items.AddRange(new ToolStripItem[] { addRowToolStripMenuItem, editRowToolStripMenuItem, deleteRowToolStripMenuItem });
            gridContextMenu.Name = "gridContextMenu";
            gridContextMenu.Size = new Size(140, 70);
            //
            // addRowToolStripMenuItem
            //
            addRowToolStripMenuItem.Name = "addRowToolStripMenuItem";
            addRowToolStripMenuItem.Size = new Size(139, 22);
            addRowToolStripMenuItem.Text = "Add Row";
            addRowToolStripMenuItem.Click += addRowToolStripMenuItem_Click;
            //
            // editRowToolStripMenuItem
            //
            editRowToolStripMenuItem.Name = "editRowToolStripMenuItem";
            editRowToolStripMenuItem.Size = new Size(139, 22);
            editRowToolStripMenuItem.Text = "Edit Row";
            editRowToolStripMenuItem.Click += editRowToolStripMenuItem_Click;
            //
            // deleteRowToolStripMenuItem
            //
            deleteRowToolStripMenuItem.Name = "deleteRowToolStripMenuItem";
            deleteRowToolStripMenuItem.Size = new Size(139, 22);
            deleteRowToolStripMenuItem.Text = "Delete Row";
            deleteRowToolStripMenuItem.Click += deleteRowToolStripMenuItem_Click;
            //
            // MainForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer1);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SQLite Helper - Database Project Manager";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            tablesContextMenu.ResumeLayout(false);
            tableContextMenu.ResumeLayout(false);
            viewContextMenu.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            objectInfoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            gridContextMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
        private ContextMenuStrip tablesContextMenu;
        private ToolStripMenuItem createTableToolStripMenuItem;
        private ContextMenuStrip tableContextMenu;
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
    }
}
