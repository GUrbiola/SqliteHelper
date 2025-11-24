namespace SqliteHelper
{
    public partial class MainForm : Form
    {
        private string? currentProjectPath;
        private RecentProjects recentProjects;

        public MainForm()
        {
            InitializeComponent();
            recentProjects = RecentProjects.Load();
            UpdateRecentProjectsMenu();
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
            toolStripStatusLabel.Text = $"Project '{name}' created successfully.";
            manageProjectToolStripMenuItem.Enabled = true;
            EnableDatabaseMenuItems(true);
        }

        private void LoadProject(string path)
        {
            try
            {
                var project = DatabaseProject.LoadFromFile(path);
                recentProjects.AddProject(path, project.Name);
                UpdateRecentProjectsMenu();
                toolStripStatusLabel.Text = $"Project '{project.Name}' loaded successfully.";
                manageProjectToolStripMenuItem.Enabled = true;
                EnableDatabaseMenuItems(true);
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

        private void EnableDatabaseMenuItems(bool enabled)
        {
            createSqliteFileToolStripMenuItem.Enabled = enabled;
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
    }
}
