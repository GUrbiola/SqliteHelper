using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ZidUtilities.CommonCode.Win;

namespace SqliteHelper48
{
    public partial class DiagramDesignerForm : Form
    {
        private string projectPath;
        private DatabaseProject? project;
        private TableControl? selectedTable;
        private CommentControl? selectedComment;
        private Point dragOffset;
        private bool isDragging;

        public DiagramDesignerForm(string projectPath)
        {
            InitializeComponent();
            this.projectPath = projectPath;

            // Enable double buffering for smooth drawing
            diagramPanel.Paint += DiagramPanel_Paint;

            LoadProject();
        }

        private void LoadProject()
        {
            try
            {
                project = DatabaseProject.LoadFromFile(projectPath);
                this.Text = $"Diagram Designer - {project.Name}";

                // Load existing tables onto the canvas
                foreach (var table in project.Tables)
                {
                    AddTableToCanvas(table);
                }

                // Load existing comments
                foreach (var comment in project.Comments)
                {
                    AddCommentToCanvas(comment);
                }

                toolStripStatusLabel.Text = "Drag tables to move. Double-click to edit.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading project: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveProject()
        {
            if (project != null)
            {
                // Update table positions from controls
                foreach (Control control in diagramPanel.Controls)
                {
                    if (control is TableControl tableControl)
                    {
                        var tableDef = project.Tables.FirstOrDefault(t => t.Name == tableControl.TableDefinition.Name);
                        if (tableDef != null)
                        {
                            tableDef.Position = tableControl.Location;
                        }
                    }
                    else if (control is CommentControl commentControl)
                    {
                        commentControl.UpdateComment();
                    }
                }

                project.SaveToFile(projectPath);
                toolStripStatusLabel.Text = "Project saved successfully.";
                diagramPanel.Invalidate(); // Refresh to redraw relationships
            }
        }

        private void addTableButton_Click(object sender, EventArgs e)
        {
            using (var tableDialog = new TableEditorDialog())
            {
                if (tableDialog.ShowDialog() == DialogResult.OK)
                {
                    var newTable = tableDialog.TableDefinition;
                    newTable.Position = new Point(50, 50);

                    project?.Tables.Add(newTable);
                    AddTableToCanvas(newTable);
                    SaveProject();
                }
            }
        }

        private void AddTableToCanvas(TableDefinition table)
        {
            var tableControl = new TableControl(table);
            tableControl.Location = table.Position;
            tableControl.MouseDown += TableControl_MouseDown;
            tableControl.MouseMove += TableControl_MouseMove;
            tableControl.MouseUp += TableControl_MouseUp;
            tableControl.DoubleClick += TableControl_DoubleClick;

            diagramPanel.Controls.Add(tableControl);
        }

        private void TableControl_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && sender is TableControl table)
            {
                selectedTable = table;
                isDragging = true;
                dragOffset = new Point(e.X, e.Y);
                table.BringToFront();
            }
        }

        private void TableControl_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isDragging && selectedTable != null && sender is TableControl table)
            {
                Point newLocation = new Point(
                    table.Left + e.X - dragOffset.X,
                    table.Top + e.Y - dragOffset.Y
                );

                table.Location = newLocation;
                diagramPanel.Invalidate(); // Redraw relationships as table moves
            }
        }

        private void TableControl_MouseUp(object? sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                SaveProject();
            }
        }

        private void TableControl_DoubleClick(object? sender, EventArgs e)
        {
            if (sender is TableControl table && project != null)
            {
                using (var tableDialog = new TableEditorDialog(table.TableDefinition))
                {
                    if (tableDialog.ShowDialog() == DialogResult.OK)
                    {
                        var updatedTable = tableDialog.TableDefinition;

                        // Update the table in the project's table list
                        var projectTable = project.Tables.FirstOrDefault(t => t.Name == table.TableDefinition.Name);
                        if (projectTable != null)
                        {
                            // Update all properties
                            projectTable.Name = updatedTable.Name;
                            projectTable.Columns = updatedTable.Columns;
                            projectTable.Position = table.Location; // Keep current position
                        }

                        // Update the visual control
                        table.UpdateTable(updatedTable);

                        SaveProject();
                        toolStripStatusLabel.Text = $"Table '{updatedTable.Name}' updated successfully.";
                    }
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveProject();
            ZidUtilities.CommonCode.Win.Forms.MessageBoxDialog.Show(
                "Project saved successfully.",
                "Save Project",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                themeManager1.Theme
                );
        }

        private void DiagramDesignerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveProject();
        }

        private void generateInitScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (project == null || project.Tables.Count == 0)
            {
                MessageBox.Show("There are no tables to generate a script for.", "No Tables", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string script = SqlGenerator.GenerateInitializationScript(project);

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "SQL Script (*.sql)|*.sql|All Files (*.*)|*.*";
                saveDialog.DefaultExt = "sql";
                saveDialog.FileName = $"{project.Name}_init.sql";
                saveDialog.Title = "Save Initialization Script";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveDialog.FileName, script);
                        toolStripStatusLabel.Text = "Initialization script generated successfully.";
                        MessageBox.Show($"Initialization script saved to:\n{saveDialog.FileName}", "Script Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving script: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void generateUpdateScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (project == null || project.Tables.Count == 0)
            {
                MessageBox.Show("There are no tables to generate a script for.", "No Tables", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string script = SqlGenerator.GenerateUpdateScript(project);

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "SQL Script (*.sql)|*.sql|All Files (*.*)|*.*";
                saveDialog.DefaultExt = "sql";
                saveDialog.FileName = $"{project.Name}_update.sql";
                saveDialog.Title = "Save Update Script";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveDialog.FileName, script);
                        toolStripStatusLabel.Text = "Update script generated successfully.";
                        MessageBox.Show($"Update script saved to:\n{saveDialog.FileName}", "Script Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving script: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void addRelationshipButton_Click(object sender, EventArgs e)
        {
            if (project == null || project.Tables.Count < 2)
            {
                MessageBox.Show("You need at least two tables to create a relationship.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var relationshipDialog = new RelationshipDialog(project))
            {
                if (relationshipDialog.ShowDialog() == DialogResult.OK)
                {
                    project.Relationships.Add(relationshipDialog.Relationship);
                    SaveProject();
                    toolStripStatusLabel.Text = "Relationship added successfully.";
                }
            }
        }

        private void manageRelationshipsButton_Click(object sender, EventArgs e)
        {
            if (project == null) return;

            using (var manageDialog = new ManageRelationshipsDialog(project))
            {
                manageDialog.ShowDialog();

                if (manageDialog.RelationshipsChanged)
                {
                    SaveProject();
                    toolStripStatusLabel.Text = "Relationships updated.";
                }
            }
        }

        private void addCommentButton_Click(object sender, EventArgs e)
        {
            var comment = new DiagramComment
            {
                Text = "New Comment",
                Position = new Point(0, 0),
                Size = new Size(200, 100)
            };

            project?.Comments.Add(comment);
            AddCommentToCanvas(comment);
            SaveProject();
        }

        private void AddCommentToCanvas(DiagramComment comment)
        {
            var commentControl = new CommentControl(comment);
            commentControl.MouseDown += CommentControl_MouseDown;
            commentControl.MouseMove += CommentControl_MouseMove;
            commentControl.MouseUp += CommentControl_MouseUp;
            commentControl.DeleteRequested += CommentControl_DeleteRequested;

            diagramPanel.Controls.Add(commentControl);
        }

        private void CommentControl_DeleteRequested(object? sender, EventArgs e)
        {
            if (sender is CommentControl commentControl && project != null)
            {
                var result = MessageBox.Show("Are you sure you want to delete this comment?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Remove from project
                    project.Comments.Remove(commentControl.Comment);

                    // Remove from canvas
                    diagramPanel.Controls.Remove(commentControl);

                    SaveProject();
                    toolStripStatusLabel.Text = "Comment deleted.";
                }
            }
        }

        private void CommentControl_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && sender is CommentControl comment)
            {
                // Only start dragging if not clicking on the resize grip area
                if (e.X < comment.Width - 16 || e.Y < comment.Height - 16)
                {
                    selectedComment = comment;
                    selectedTable = null;
                    isDragging = true;
                    dragOffset = new Point(e.X, e.Y);
                    comment.BringToFront();
                }
            }
        }

        private void CommentControl_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isDragging && selectedComment != null && sender is CommentControl comment)
            {
                // Only move if not in resize area
                if (dragOffset.X < comment.Width - 16 || dragOffset.Y < comment.Height - 16)
                {
                    Point newLocation = new Point(
                        comment.Left + e.X - dragOffset.X,
                        comment.Top + e.Y - dragOffset.Y
                    );

                    comment.Location = newLocation;
                }
            }
        }

        private void CommentControl_MouseUp(object? sender, MouseEventArgs e)
        {
            if (isDragging && selectedComment != null)
            {
                isDragging = false;
                selectedComment.UpdateComment();
                SaveProject();
                selectedComment = null;
            }
        }

        private void DiagramPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (project == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Draw relationships
            foreach (var relationship in project.Relationships)
            {
                DrawRelationship(g, relationship);
            }
        }

        private void DrawRelationship(Graphics g, Relationship relationship)
        {
            // Find the table controls
            TableControl? fromTable = null;
            TableControl? toTable = null;

            foreach (Control control in diagramPanel.Controls)
            {
                if (control is TableControl tc)
                {
                    if (tc.TableDefinition.Name == relationship.FromTable)
                        fromTable = tc;
                    if (tc.TableDefinition.Name == relationship.ToTable)
                        toTable = tc;
                }
            }

            if (fromTable == null || toTable == null) return;

            // Calculate connection points (center right/left of tables)
            Point fromPoint = new Point(fromTable.Right, fromTable.Top + fromTable.Height / 2);
            Point toPoint = new Point(toTable.Left, toTable.Top + toTable.Height / 2);

            // If tables overlap horizontally, connect from top/bottom instead
            if (fromTable.Right > toTable.Left && fromTable.Left < toTable.Right)
            {
                if (fromTable.Top < toTable.Top)
                {
                    fromPoint = new Point(fromTable.Left + fromTable.Width / 2, fromTable.Bottom);
                    toPoint = new Point(toTable.Left + toTable.Width / 2, toTable.Top);
                }
                else
                {
                    fromPoint = new Point(fromTable.Left + fromTable.Width / 2, fromTable.Top);
                    toPoint = new Point(toTable.Left + toTable.Width / 2, toTable.Bottom);
                }
            }
            else if (fromPoint.X > toPoint.X)
            {
                // Swap if from is to the right of to
                fromPoint = new Point(fromTable.Left, fromTable.Top + fromTable.Height / 2);
                toPoint = new Point(toTable.Right, toTable.Top + toTable.Height / 2);
            }

            // Draw the line
            using (Pen pen = new Pen(Color.DarkBlue, 2))
            {
                pen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 5);
                g.DrawLine(pen, fromPoint, toPoint);
            }

            // Draw relationship type label
            Point midPoint = new Point((fromPoint.X + toPoint.X) / 2, (fromPoint.Y + toPoint.Y) / 2);
            using (Font font = new Font("Arial", 8))
            using (SolidBrush brush = new SolidBrush(Color.DarkBlue))
            {
                string label = relationship.RelationshipType;
                SizeF labelSize = g.MeasureString(label, font);
                g.FillRectangle(Brushes.White, midPoint.X - labelSize.Width / 2 - 2, midPoint.Y - labelSize.Height / 2 - 2, labelSize.Width + 4, labelSize.Height + 4);
                g.DrawString(label, font, brush, midPoint.X - labelSize.Width / 2, midPoint.Y - labelSize.Height / 2);
            }
        }

        private void autoArrangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (project == null) return;

            // Separate tables and comments
            var tables = new List<TableControl>();
            var comments = new List<CommentControl>();

            foreach (Control control in diagramPanel.Controls)
            {
                if (control is TableControl tc)
                    tables.Add(tc);
                else if (control is CommentControl cc)
                    comments.Add(cc);
            }

            if (tables.Count == 0 && comments.Count == 0)
            {
                MessageBox.Show("No objects to arrange.", "Auto Arrange", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Layout configuration
            const int marginX = 50;
            const int marginY = 50;
            const int spacingX = 100;
            const int spacingY = 80;
            const int commentAreaX = 50; // Space for comments on the right

            // Calculate grid dimensions for tables
            int tableColumns = (int)Math.Ceiling(Math.Sqrt(tables.Count));
            if (tableColumns == 0) tableColumns = 1;

            // Arrange tables in a grid
            int currentX = marginX;
            int currentY = marginY;
            int columnIndex = 0;
            int maxHeightInRow = 0;

            foreach (var table in tables)
            {
                table.Location = new Point(currentX, currentY);

                maxHeightInRow = Math.Max(maxHeightInRow, table.Height);

                columnIndex++;
                if (columnIndex >= tableColumns)
                {
                    // Move to next row
                    columnIndex = 0;
                    currentX = marginX;
                    currentY += maxHeightInRow + spacingY;
                    maxHeightInRow = 0;
                }
                else
                {
                    // Move to next column
                    currentX += table.Width + spacingX;
                }

                // Update the table's position in the project
                table.TableDefinition.Position = table.Location;
            }

            // Calculate the right edge of the table area
            int tableAreaMaxX = marginX;
            foreach (var table in tables)
            {
                tableAreaMaxX = Math.Max(tableAreaMaxX, table.Right);
            }

            // Arrange comments vertically on the right side or below tables
            int commentX = tableAreaMaxX + commentAreaX + spacingX;
            int commentY = marginY;
            const int commentSpacing = 30;

            foreach (var comment in comments)
            {
                comment.Location = new Point(commentX, commentY);
                commentY += comment.Height + commentSpacing;

                // Update the comment's position in the project
                comment.UpdateComment();
            }

            // Save the project with new positions
            SaveProject();

            // Refresh the diagram to redraw relationships
            diagramPanel.Invalidate();

            toolStripStatusLabel.Text = $"Auto-arranged {tables.Count} table(s) and {comments.Count} comment(s).";

            MessageBox.Show($"Successfully arranged {tables.Count} table(s) and {comments.Count} comment(s).",
                          "Auto Arrange Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DiagramDesignerForm_Load(object sender, EventArgs e)
        {
            string savedTheme = Properties.Settings.Default.SelectedTheme;
            themeManager1.Theme = Enum.TryParse<ZidThemes>(savedTheme, out var theme) ? theme : ZidThemes.None;
            themeManager1.ApplyTheme();

        }
    }
}
