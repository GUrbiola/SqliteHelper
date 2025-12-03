using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ZidUtilities.CommonCode.Win;

namespace SqliteHelper48
{
    public partial class MainForm : Form
    {
        private string? currentProjectPath;
        private RecentProjects recentProjects;
        private RecentDatabases recentDatabases;
        private DatabaseManager databaseManager;
        private string? currentTableName;

        public MainForm()
        {
            InitializeComponent();
            recentProjects = RecentProjects.Load();
            recentDatabases = RecentDatabases.Load();
            databaseManager = new DatabaseManager();

            InitializeTreeViewIcons();
            UpdateRecentProjectsMenu();
            UpdateRecentDatabasesMenu();
            UpdateStatusBar();

            // Add MouseUp event for TreeView context menus
            databaseTreeView.MouseUp += databaseTreeView_MouseUp;
        }

        private void createProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var nameDialog = new ProjectNameDialog())
            {
                if (nameDialog.ShowDialog() == DialogResult.OK)
                {
                    string projectName = nameDialog.ProjectName;

                    using (SaveFileDialog saveDialog = new SaveFileDialog())
                    {
                        saveDialog.Filter = "SQLite Helper Project (*.shlp)|*.shlp|All Files (*.*)|*.*";
                        saveDialog.DefaultExt = "shlp";
                        saveDialog.FileName = projectName;
                        saveDialog.Title = "Save Database Project";

                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            currentProjectPath = saveDialog.FileName;
                            CreateNewProject(currentProjectPath, projectName);
                            OpenDiagramDesigner();
                        }
                    }
                }
            }
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "SQLite Helper Project (*.shlp)|*.shlp|All Files (*.*)|*.*";
                openDialog.DefaultExt = "shlp";
                openDialog.Title = "Open Database Project";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    currentProjectPath = openDialog.FileName;
                    LoadProject(currentProjectPath);
                    OpenDiagramDesigner();
                }
            }
        }

        private void CreateNewProject(string path, string name)
        {
            var project = new DatabaseProject
            {
                Name = name,
                CreatedDate = DateTime.Now,
                Tables = new List<TableDefinition>()
            };

            project.SaveToFile(path);
            recentProjects.AddProject(path, name);
            UpdateRecentProjectsMenu();
            closeProjectToolStripMenuItem.Enabled = true;
            createSqliteFileToolStripMenuItem.Enabled = true;
            UpdateStatusBar();
        }

        private void LoadProject(string path)
        {
            try
            {
                var project = DatabaseProject.LoadFromFile(path);
                recentProjects.AddProject(path, project.Name);
                UpdateRecentProjectsMenu();
                closeProjectToolStripMenuItem.Enabled = true;
                createSqliteFileToolStripMenuItem.Enabled = true;
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading project: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenDiagramDesigner()
        {
            if (string.IsNullOrEmpty(currentProjectPath))
                return;

            var designerForm = new DiagramDesignerForm(currentProjectPath);
            designerForm.Show();
        }

        private void UpdateRecentProjectsMenu()
        {
            recentProjectsToolStripMenuItem.DropDownItems.Clear();

            if (recentProjects.Projects.Count == 0)
            {
                var emptyItem = new ToolStripMenuItem("(No recent projects)");
                emptyItem.Enabled = false;
                recentProjectsToolStripMenuItem.DropDownItems.Add(emptyItem);
            }
            else
            {
                foreach (var project in recentProjects.Projects)
                {
                    var menuItem = new ToolStripMenuItem
                    {
                        Text = $"{project.Name} ({Path.GetFileName(project.Path)})",
                        Tag = project.Path,
                        ToolTipText = project.Path
                    };
                    menuItem.Click += RecentProjectMenuItem_Click;
                    recentProjectsToolStripMenuItem.DropDownItems.Add(menuItem);
                }

                recentProjectsToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());

                var clearItem = new ToolStripMenuItem("Clear Recent Projects");
                clearItem.Click += ClearRecentProjects_Click;
                recentProjectsToolStripMenuItem.DropDownItems.Add(clearItem);
            }
        }

        private void RecentProjectMenuItem_Click(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is string path)
            {
                if (File.Exists(path))
                {
                    currentProjectPath = path;
                    LoadProject(path);
                    OpenDiagramDesigner();
                }
                else
                {
                    MessageBox.Show($"Project file not found: {path}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    recentProjects.RemoveProject(path);
                    UpdateRecentProjectsMenu();
                }
            }
        }

        private void ClearRecentProjects_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to clear the recent projects list?",
                "Clear Recent Projects", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                recentProjects.Projects.Clear();
                recentProjects.Save();
                UpdateRecentProjectsMenu();
            }
        }

        private void projectToolStripMenuItem_DropDownOpening(object? sender, EventArgs e)
        {
            // Refresh the recent projects list when the menu is opened
            // This ensures we remove any projects that no longer exist
            bool changed = false;
            var projectsToRemove = recentProjects.Projects.Where(p => !File.Exists(p.Path)).ToList();

            foreach (var project in projectsToRemove)
            {
                recentProjects.Projects.Remove(project);
                changed = true;
            }

            if (changed)
            {
                recentProjects.Save();
                UpdateRecentProjectsMenu();
            }
        }

        private void createSqliteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentProjectPath))
            {
                MessageBox.Show("No project is currently loaded.", "No Project", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var project = DatabaseProject.LoadFromFile(currentProjectPath);

                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "SQLite Database (*.db)|*.db|SQLite Database (*.sqlite)|*.sqlite|All Files (*.*)|*.*";
                    saveDialog.DefaultExt = "db";
                    saveDialog.FileName = project.Name;
                    saveDialog.Title = "Save SQLite Database File";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        bool success = SqliteDatabaseExporter.CreateDatabaseWithSchema(project, saveDialog.FileName, out string errorMessage);

                        if (success)
                        {
                            MessageBox.Show($"SQLite database created successfully!\n\nLocation: {saveDialog.FileName}",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"Failed to create database:\n\n{errorMessage}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading project: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void closeProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentProjectPath = null;
            closeProjectToolStripMenuItem.Enabled = false;
            createSqliteFileToolStripMenuItem.Enabled = false;
            UpdateStatusBar();
        }

        private void InitializeTreeViewIcons()
        {
            // Create simple icons using text-based representation
            // In a real application, you would load actual icon files
            treeViewImageList.Images.Add("database", Properties.Resources.Db32);
            treeViewImageList.Images.Add("tables", Properties.Resources.TableObj32);
            treeViewImageList.Images.Add("table", Properties.Resources.Table32);
            treeViewImageList.Images.Add("views", Properties.Resources.TableObj32);
            treeViewImageList.Images.Add("view", Properties.Resources.View32);
            treeViewImageList.Images.Add("indexes", Properties.Resources.Index32);
            treeViewImageList.Images.Add("index", Properties.Resources.Id32);
        }

        private void openDataBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "SQLite Database (*.db)|*.db|SQLite Database (*.sqlite)|*.sqlite|All Files (*.*)|*.*";
                openDialog.Title = "Open SQLite Database";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    OpenDatabase(openDialog.FileName);
                }
            }
        }

        private void OpenDatabase(string path)
        {
            if (databaseManager.OpenDatabase(path, out string errorMessage))
            {
                recentDatabases.AddDatabase(path);
                UpdateRecentDatabasesMenu();
                LoadDatabaseStructure();
                EnableDatabaseControls(true);
                UpdateStatusBar();
            }
            else
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDatabaseStructure()
        {
            databaseTreeView.Nodes.Clear();
            objectInfoTextBox.Clear();
            dataGridView.DataSource = null;

            try
            {
                var rootNode = new TreeNode(Path.GetFileName(databaseManager.DatabasePath ?? "Database"))
                {
                    ImageKey = "database",
                    SelectedImageKey = "database"
                };

                // Tables
                var tablesNode = new TreeNode("Tables")
                {
                    ImageKey = "tables",
                    SelectedImageKey = "tables",
                    Tag = "tables_folder"
                };

                foreach (var tableName in databaseManager.GetTableNames())
                {
                    var tableNode = new TreeNode(tableName)
                    {
                        ImageKey = "table",
                        SelectedImageKey = "table",
                        Tag = $"table:{tableName}"
                    };
                    tablesNode.Nodes.Add(tableNode);
                }

                rootNode.Nodes.Add(tablesNode);

                // Views
                var viewsNode = new TreeNode("Views")
                {
                    ImageKey = "views",
                    SelectedImageKey = "views",
                    Tag = "views_folder"
                };

                foreach (var viewName in databaseManager.GetViewNames())
                {
                    var viewNode = new TreeNode(viewName)
                    {
                        ImageKey = "view",
                        SelectedImageKey = "view",
                        Tag = $"view:{viewName}"
                    };
                    viewsNode.Nodes.Add(viewNode);
                }

                rootNode.Nodes.Add(viewsNode);

                // Indexes
                var indexesNode = new TreeNode("Indexes")
                {
                    ImageKey = "indexes",
                    SelectedImageKey = "indexes",
                    Tag = "indexes_folder"
                };

                foreach (var indexName in databaseManager.GetIndexNames())
                {
                    var indexNode = new TreeNode(indexName)
                    {
                        ImageKey = "index",
                        SelectedImageKey = "index",
                        Tag = $"index:{indexName}"
                    };
                    indexesNode.Nodes.Add(indexNode);
                }

                rootNode.Nodes.Add(indexesNode);

                databaseTreeView.Nodes.Add(rootNode);
                rootNode.Expand();
                tablesNode.Expand();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading database structure: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void databaseTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null || e.Node.Tag == null)
                return;

            var tag = e.Node.Tag.ToString();
            if (string.IsNullOrEmpty(tag))
                return;

            var parts = tag.Split(':');
            if (parts.Length != 2)
            {
                objectInfoTextBox.Clear();
                dataGridView.DataSource = null;
                currentTableName = null;
                return;
            }

            var objectType = parts[0];
            var objectName = parts[1];

            switch (objectType)
            {
                case "table":
                    objectInfoTextBox.Text = databaseManager.GetTableSchema(objectName);
                    LoadTableData(objectName);
                    currentTableName = objectName;
                    break;

                case "view":
                    objectInfoTextBox.Text = databaseManager.GetViewSchema(objectName);
                    LoadViewData(objectName);
                    currentTableName = objectName;
                    break;

                case "index":
                    objectInfoTextBox.Text = databaseManager.GetIndexSchema(objectName);
                    dataGridView.DataSource = null;
                    currentTableName = null;
                    break;

                default:
                    objectInfoTextBox.Clear();
                    dataGridView.DataSource = null;
                    currentTableName = null;
                    break;
            }
        }

        private void LoadTableData(string tableName)
        {
            try
            {
                var dataTable = databaseManager.GetTableData(tableName);
                dataGridView.DataSource = dataTable;
                dataGridView.ContextMenuStrip = gridContextMenu;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading table data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void LoadViewData(string tableName)
        {
            try
            {
                var dataTable = databaseManager.GetTableData(tableName);
                dataGridView.DataSource = dataTable;
                dataGridView.ContextMenuStrip = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading table data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateRecentDatabasesMenu()
        {
            recentDatabasesToolStripMenuItem.DropDownItems.Clear();

            if (recentDatabases.Databases.Count == 0)
            {
                var emptyItem = new ToolStripMenuItem("(No recent databases)");
                emptyItem.Enabled = false;
                recentDatabasesToolStripMenuItem.DropDownItems.Add(emptyItem);
            }
            else
            {
                foreach (var database in recentDatabases.Databases)
                {
                    var menuItem = new ToolStripMenuItem
                    {
                        Text = $"{database.Name} ({Path.GetFileName(database.Path)})",
                        Tag = database.Path,
                        ToolTipText = database.Path
                    };
                    menuItem.Click += RecentDatabaseMenuItem_Click;
                    recentDatabasesToolStripMenuItem.DropDownItems.Add(menuItem);
                }

                recentDatabasesToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());

                var clearItem = new ToolStripMenuItem("Clear Recent Databases");
                clearItem.Click += ClearRecentDatabases_Click;
                recentDatabasesToolStripMenuItem.DropDownItems.Add(clearItem);
            }
        }

        private void RecentDatabaseMenuItem_Click(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is string path)
            {
                if (File.Exists(path))
                {
                    OpenDatabase(path);
                }
                else
                {
                    MessageBox.Show($"Database file not found: {path}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    recentDatabases.RemoveDatabase(path);
                    UpdateRecentDatabasesMenu();
                }
            }
        }

        private void ClearRecentDatabases_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to clear the recent databases list?",
                "Clear Recent Databases", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                recentDatabases.Databases.Clear();
                recentDatabases.Save();
                UpdateRecentDatabasesMenu();
            }
        }

        private void closeDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseDatabase();
        }

        private void CloseDatabase()
        {
            databaseManager.CloseDatabase();
            databaseTreeView.Nodes.Clear();
            objectInfoTextBox.Clear();
            dataGridView.DataSource = null;
            currentTableName = null;
            EnableDatabaseControls(false);
            UpdateStatusBar();
        }

        private void saveCopyOfDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (databaseManager.DatabasePath == null)
            {
                MessageBox.Show("No database is currently open.", "No Database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "SQLite Database (*.db)|*.db|SQLite Database (*.sqlite)|*.sqlite|All Files (*.*)|*.*";
                saveDialog.DefaultExt = "db";
                saveDialog.FileName = Path.GetFileNameWithoutExtension(databaseManager.DatabasePath) + "_copy";
                saveDialog.Title = "Save Copy of Database";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.Copy(databaseManager.DatabasePath, saveDialog.FileName, true);
                        MessageBox.Show($"Database copied successfully!\n\nLocation: {saveDialog.FileName}",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error copying database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EnableDatabaseControls(bool enabled)
        {
            databaseTreeView.Enabled = enabled;
            objectInfoTextBox.Enabled = enabled;
            dataGridView.Enabled = enabled;
            closeDatabaseToolStripMenuItem.Enabled = enabled;
            saveCopyOfDatabaseToolStripMenuItem.Enabled = enabled;
            runQueryToolStripMenuItem.Enabled = enabled;
            reloadDatabaseToolStripMenuItem.Enabled = enabled;
        }

        private void UpdateStatusBar()
        {
            if (!string.IsNullOrEmpty(currentProjectPath))
            {
                var projectName = Path.GetFileNameWithoutExtension(currentProjectPath);
                projectStatusLabel.Text = $"Project: {projectName}";
            }
            else
            {
                projectStatusLabel.Text = "No project";
            }

            if (databaseManager.DatabasePath != null)
            {
                var dbName = Path.GetFileName(databaseManager.DatabasePath);
                databaseStatusLabel.Text = $"Database: {dbName} ({databaseManager.DatabasePath})";
            }
            else
            {
                databaseStatusLabel.Text = "No database";
            }
        }

        private void addRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentTableName))
            {
                MessageBox.Show("Please select a table first.", "No Table Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var columnInfo = databaseManager.GetTableColumnInfo(currentTableName);
            if (columnInfo.Count == 0)
            {
                MessageBox.Show("Could not retrieve table information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using var dialog = new RowEditorDialog(columnInfo);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Build INSERT statement
                    var columns = new List<string>();
                    var values = new List<string>();

                    foreach (var kvp in dialog.Values)
                    {
                        columns.Add(kvp.Key);

                        if (kvp.Value == DBNull.Value || kvp.Value == null)
                        {
                            values.Add("NULL");
                        }
                        else if (kvp.Value is string)
                        {
                            var valueStr = kvp.Value.ToString()?.Replace("'", "''");
                            values.Add($"'{valueStr}'");
                        }
                        else if (kvp.Value is DateTime dt)
                        {
                            values.Add($"'{dt:yyyy-MM-dd HH:mm:ss}'");
                        }
                        else if (kvp.Value is byte[] bytes)
                        {
                            var hex = BitConverter.ToString(bytes).Replace("-", "");
                            values.Add($"X'{hex}'");
                        }
                        else if (kvp.Value is bool b)
                        {
                            values.Add(b ? "1" : "0");
                        }
                        else
                        {
                            values.Add(kvp.Value.ToString() ?? "NULL");
                        }
                    }

                    var insertQuery = $"INSERT INTO {currentTableName} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)})";

                    if (databaseManager.ExecuteNonQuery(insertQuery, out string errorMessage))
                    {
                        LoadTableData(currentTableName);
                        MessageBox.Show("Row added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Error adding row:\n\n{errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding row:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void editRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to edit.", "No Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(currentTableName))
            {
                MessageBox.Show("No table selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dataGridView.DataSource is not DataTable dataTable)
            {
                MessageBox.Show("No data table loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView.SelectedRows[0];
            if (selectedRow.Index < 0 || selectedRow.Index >= dataTable.Rows.Count)
            {
                MessageBox.Show("Invalid row selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var dataRow = dataTable.Rows[selectedRow.Index];

            var columnInfo = databaseManager.GetTableColumnInfo(currentTableName);
            if (columnInfo.Count == 0)
            {
                MessageBox.Show("Could not retrieve table information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using var dialog = new RowEditorDialog(columnInfo, dataRow);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Build UPDATE statement
                    var setClauses = new List<string>();
                    var whereClauses = new List<string>();

                    // Build WHERE clause from original values
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        var oldValue = dataRow[column];
                        if (oldValue == DBNull.Value)
                        {
                            whereClauses.Add($"{column.ColumnName} IS NULL");
                        }
                        else if (oldValue is string || oldValue is DateTime)
                        {
                            var valueStr = oldValue.ToString()?.Replace("'", "''");
                            whereClauses.Add($"{column.ColumnName} = '{valueStr}'");
                        }
                        else
                        {
                            whereClauses.Add($"{column.ColumnName} = {oldValue}");
                        }
                    }

                    // Build SET clause from new values
                    foreach (var kvp in dialog.Values)
                    {
                        if (kvp.Value == DBNull.Value || kvp.Value == null)
                        {
                            setClauses.Add($"{kvp.Key} = NULL");
                        }
                        else if (kvp.Value is string)
                        {
                            var valueStr = kvp.Value.ToString()?.Replace("'", "''");
                            setClauses.Add($"{kvp.Key} = '{valueStr}'");
                        }
                        else if (kvp.Value is DateTime dt)
                        {
                            setClauses.Add($"{kvp.Key} = '{dt:yyyy-MM-dd HH:mm:ss}'");
                        }
                        else if (kvp.Value is byte[] bytes)
                        {
                            var hex = BitConverter.ToString(bytes).Replace("-", "");
                            setClauses.Add($"{kvp.Key} = X'{hex}'");
                        }
                        else if (kvp.Value is bool b)
                        {
                            setClauses.Add($"{kvp.Key} = {(b ? "1" : "0")}");
                        }
                        else
                        {
                            setClauses.Add($"{kvp.Key} = {kvp.Value}");
                        }
                    }

                    var updateQuery = $"UPDATE {currentTableName} SET {string.Join(", ", setClauses)} WHERE {string.Join(" AND ", whereClauses)}";

                    if (databaseManager.ExecuteNonQuery(updateQuery, out string errorMessage))
                    {
                        LoadTableData(currentTableName);
                        MessageBox.Show("Row updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Error updating row:\n\n{errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating row:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.", "No Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(currentTableName))
            {
                MessageBox.Show("No table selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete the selected row(s)?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Get the DataTable from the DataGridView
                    if (dataGridView.DataSource is DataTable dataTable)
                    {
                        foreach (DataGridViewRow row in dataGridView.SelectedRows)
                        {
                            if (row.Index < 0 || row.Index >= dataTable.Rows.Count)
                                continue;

                            var dataRow = dataTable.Rows[row.Index];

                            // Build DELETE statement based on all columns
                            var whereClauses = new List<string>();
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                var value = dataRow[column];
                                if (value == DBNull.Value)
                                {
                                    whereClauses.Add($"{column.ColumnName} IS NULL");
                                }
                                else
                                {
                                    var valueStr = value.ToString()?.Replace("'", "''");
                                    whereClauses.Add($"{column.ColumnName} = '{valueStr}'");
                                }
                            }

                            var deleteQuery = $"DELETE FROM {currentTableName} WHERE {string.Join(" AND ", whereClauses)}";

                            if (!databaseManager.ExecuteNonQuery(deleteQuery, out string errorMessage))
                            {
                                MessageBox.Show($"Error deleting row: {errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        // Refresh the data
                        LoadTableData(currentTableName);
                        MessageBox.Show("Row(s) deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting row: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void runQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var queryDialog = new QueryDialog(databaseManager);
            queryDialog.ShowDialog();
        }

        private void reloadDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (databaseManager.DatabasePath == null)
            {
                MessageBox.Show("No database is currently open.", "No Database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Store the current database path
            string currentPath = databaseManager.DatabasePath;

            // Reload the database structure
            LoadDatabaseStructure();
            MessageBox.Show("Database reloaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void databaseTreeView_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Get the node at the mouse position
                TreeNode? node = databaseTreeView.GetNodeAt(e.X, e.Y);
                if (node != null)
                {
                    databaseTreeView.SelectedNode = node;

                    if (node.Tag == null)
                        return;

                    string tag = node.Tag.ToString() ?? "";

                    // Show appropriate context menu based on node type
                    if (tag == "tables_folder")
                    {
                        tablesContextMenu.Show(databaseTreeView, e.Location);
                    }
                    else if (tag.StartsWith("table:"))
                    {
                        tableContextMenu.Show(databaseTreeView, e.Location);
                    }
                    else if (tag.StartsWith("view:"))
                    {
                        viewContextMenu.Show(databaseTreeView, e.Location);
                    }
                }
            }
        }

        private void createTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dialog = new TableEditorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (databaseManager.ExecuteNonQuery(dialog.TableSql, out string errorMessage))
                    {
                        LoadDatabaseStructure();
                        MessageBox.Show($"Table created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Error creating table:\n\n{errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating table:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void deleteTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (databaseTreeView.SelectedNode == null)
                return;

            string tag = databaseTreeView.SelectedNode.Tag?.ToString() ?? "";
            if (!tag.StartsWith("table:"))
                return;

            string tableName = tag.Substring(6); // Remove "table:" prefix

            var result = MessageBox.Show($"Are you sure you want to delete the table '{tableName}'?\n\nThis action cannot be undone.",
                "Confirm Delete Table", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string dropQuery = $"DROP TABLE IF EXISTS {tableName}";
                    if (databaseManager.ExecuteNonQuery(dropQuery, out string errorMessage))
                    {
                        if (currentTableName == tableName)
                        {
                            currentTableName = null;
                            dataGridView.DataSource = null;
                            objectInfoTextBox.Clear();
                        }
                        LoadDatabaseStructure();
                        MessageBox.Show($"Table '{tableName}' deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Error deleting table:\n\n{errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting table:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void deleteViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (databaseTreeView.SelectedNode == null)
                return;

            string tag = databaseTreeView.SelectedNode.Tag?.ToString() ?? "";
            if (!tag.StartsWith("view:"))
                return;

            string viewName = tag.Substring(5); // Remove "view:" prefix

            var result = MessageBox.Show($"Are you sure you want to delete the view '{viewName}'?\n\nThis action cannot be undone.",
                "Confirm Delete View", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string dropQuery = $"DROP VIEW IF EXISTS {viewName}";
                    if (databaseManager.ExecuteNonQuery(dropQuery, out string errorMessage))
                    {
                        if (currentTableName == viewName)
                        {
                            currentTableName = null;
                            dataGridView.DataSource = null;
                            objectInfoTextBox.Clear();
                        }
                        LoadDatabaseStructure();
                        MessageBox.Show($"View '{viewName}' deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Error deleting view:\n\n{errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting view:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            databaseManager?.Dispose();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load saved theme preference
            string savedTheme = Properties.Settings.Default.SelectedTheme;
            ZidThemes selectedTheme = ZidThemes.None;
            if (Enum.TryParse<ZidThemes>(savedTheme, out selectedTheme))
                themeManager1.Theme = selectedTheme;
            else
                themeManager1.Theme = ZidThemes.None;
            themeManager1.ApplyTheme();
            
            Dictionary<string, ZidThemes> themes = new Dictionary<string, ZidThemes>();
            foreach (var theme in Enum.GetValues(typeof(ZidThemes)))
                themes.Add(theme.ToString(), (ZidThemes)theme);

            themes.OrderBy(t => t.Key);
            foreach (var theme in themes.OrderBy(t => t.Key))
            {
                ToolStripMenuItem menuOption= new System.Windows.Forms.ToolStripMenuItem();
                menuOption.Name = $"ZidTheme_{theme.Key}";
                menuOption.Text = theme.Key;
                menuOption.Tag = theme.Value;
                if (theme.Key == savedTheme)
                    menuOption.Checked = true;

                menuOption.Click += delegate (object? sender, EventArgs e)
                {
                    ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
                    if (sender != null)
                    {
                        ZidThemes theme = (ZidThemes)menuItem.Tag;
                        if (themeManager1 != null)
                        {
                            themeManager1.Theme = theme;
                            Properties.Settings.Default.SelectedTheme = theme.ToString();
                            Properties.Settings.Default.Save();
                            themeManager1.ApplyTheme();
                        }

                        // Uncheck all other menu items
                        foreach (ToolStripMenuItem item in themeToolStripMenuItem.DropDownItems)
                            item.Checked = false;

                        menuItem.Checked = true;
                    }
                };

                themeToolStripMenuItem.DropDownItems.Add(menuOption);

            }



        }

    }
}
