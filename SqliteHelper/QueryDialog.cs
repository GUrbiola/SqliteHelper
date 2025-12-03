using System.Data;
using System.Text;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using Microsoft.Data.Sqlite;

namespace SqliteHelper
{
    public partial class QueryDialog : Form
    {
        private TextEditorControl sqlTextEditor = null!;
        private DataGridView resultsGridView = null!;
        private ToolStrip toolbar = null!;
        private SplitContainer splitContainer = null!;
        private ContextMenuStrip exportContextMenu = null!;
        private readonly DatabaseManager databaseManager;

        public QueryDialog(DatabaseManager databaseManager)
        {
            this.databaseManager = databaseManager;
            InitializeComponent();
            SetupQueryDialog();
        }

        private void SetupQueryDialog()
        {
            Text = "SQL Query Editor";
            Size = new Size(900, 600);
            StartPosition = FormStartPosition.CenterParent;

            // Main split container
            splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 250
            };

            // Top panel - SQL Editor
            var topPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            // Toolbar with icons
            toolbar = new ToolStrip
            {
                Dock = DockStyle.Top,
                ImageScalingSize = new Size(16, 16)
            };

            var btnRunQuery = new ToolStripButton
            {
                Text = "Run Query",
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = SystemIcons.Application.ToBitmap(),
                ToolTipText = "Execute the selected query or all text (F5)"
            };
            btnRunQuery.Click += BtnRunQuery_Click;

            var btnCommentLine = new ToolStripButton
            {
                Text = "Comment",
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = SystemIcons.Information.ToBitmap(),
                ToolTipText = "Comment current line (Ctrl+K)"
            };
            btnCommentLine.Click += BtnCommentLine_Click;

            var btnUncommentLine = new ToolStripButton
            {
                Text = "Uncomment",
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = SystemIcons.Question.ToBitmap(),
                ToolTipText = "Uncomment current line (Ctrl+U)"
            };
            btnUncommentLine.Click += BtnUncommentLine_Click;

            toolbar.Items.Add(btnRunQuery);
            toolbar.Items.Add(new ToolStripSeparator());
            toolbar.Items.Add(btnCommentLine);
            toolbar.Items.Add(btnUncommentLine);

            // SQL TextEditor with syntax highlighting
            sqlTextEditor = new TextEditorControl
            {
                Dock = DockStyle.Fill,
                ShowLineNumbers = true,
                ShowVRuler = false,
                ShowInvalidLines = false,
                AllowCaretBeyondEOL = false
            };

            // Set SQL syntax highlighting
            sqlTextEditor.SetHighlighting("SQL");
            sqlTextEditor.Font = new Font("Consolas", 10);

            // Set default text
            sqlTextEditor.Text = "-- Enter your SQL query here\n-- Press F5 or click Run Query to execute\nSELECT * FROM sqlite_master WHERE type='table';";

            // Add keyboard shortcuts
            sqlTextEditor.KeyDown += SqlTextEditor_KeyDown;

            topPanel.Controls.Add(sqlTextEditor);
            topPanel.Controls.Add(toolbar);

            splitContainer.Panel1.Controls.Add(topPanel);

            // Bottom panel - Results Grid
            resultsGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Export context menu
            exportContextMenu = new ContextMenuStrip();

            var exportPlainTextItem = new ToolStripMenuItem("Export to Plain Text (Tab-separated)");
            exportPlainTextItem.Click += ExportPlainText_Click;

            var exportCsvItem = new ToolStripMenuItem("Export to CSV");
            exportCsvItem.Click += ExportCsv_Click;

            var exportXlsxItem = new ToolStripMenuItem("Export to XLSX");
            exportXlsxItem.Click += ExportXlsx_Click;

            exportContextMenu.Items.Add(exportPlainTextItem);
            exportContextMenu.Items.Add(exportCsvItem);
            exportContextMenu.Items.Add(exportXlsxItem);

            resultsGridView.ContextMenuStrip = exportContextMenu;

            splitContainer.Panel2.Controls.Add(resultsGridView);

            Controls.Add(splitContainer);
        }

        private void SqlTextEditor_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                BtnRunQuery_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.K)
            {
                BtnCommentLine_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.U)
            {
                BtnUncommentLine_Click(sender, e);
                e.Handled = true;
            }
        }

        private void BtnRunQuery_Click(object? sender, EventArgs e)
        {
            string sql = sqlTextEditor.ActiveTextAreaControl.SelectionManager.SelectedText;
            if (string.IsNullOrWhiteSpace(sql))
            {
                sql = sqlTextEditor.Text;
            }

            if (string.IsNullOrWhiteSpace(sql))
            {
                MessageBox.Show("Please enter a SQL query.", "No Query", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Remove comments and trim
                sql = sql.Trim();

                // Always try to execute and get results
                var dataTable = ExecuteQuery(sql);
                if (dataTable != null)
                {
                    resultsGridView.DataSource = dataTable;
                    if (dataTable.Rows.Count > 0)
                    {
                        MessageBox.Show($"Query executed successfully. {dataTable.Rows.Count} row(s) returned.",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Query executed successfully. No rows returned.",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // Non-SELECT query (INSERT, UPDATE, DELETE, CREATE, etc.)
                    if (databaseManager.ExecuteNonQuery(sql, out string errorMessage))
                    {
                        resultsGridView.DataSource = null;
                        MessageBox.Show("Query executed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Error executing query:\n\n{errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing query:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable? ExecuteQuery(string sql)
        {
            try
            {
                var connection = databaseManager.GetConnection();
                if (connection == null || connection.State != ConnectionState.Open)
                    return null;

                using var command = connection.CreateCommand();
                command.CommandText = sql;

                var dataTable = new DataTable();
                using var reader = command.ExecuteReader();

                // Create columns
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dataTable.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                }

                // Add rows
                while (reader.Read())
                {
                    var row = dataTable.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[i] = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);
                    }
                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }
            catch
            {
                return null;
            }
        }

        private void BtnCommentLine_Click(object? sender, EventArgs e)
        {
            var document = sqlTextEditor.Document;
            var textArea = sqlTextEditor.ActiveTextAreaControl.TextArea;
            int lineNumber = textArea.Caret.Line;

            if (lineNumber < 0 || lineNumber >= document.TotalNumberOfLines)
                return;

            var lineSegment = document.GetLineSegment(lineNumber);
            string lineText = document.GetText(lineSegment.Offset, lineSegment.Length);

            if (!lineText.TrimStart().StartsWith("--"))
            {
                // Find first non-whitespace character
                int insertPosition = lineSegment.Offset;
                for (int i = 0; i < lineText.Length; i++)
                {
                    if (!char.IsWhiteSpace(lineText[i]))
                    {
                        insertPosition += i;
                        break;
                    }
                }

                document.Insert(insertPosition, "-- ");
            }
        }

        private void BtnUncommentLine_Click(object? sender, EventArgs e)
        {
            var document = sqlTextEditor.Document;
            var textArea = sqlTextEditor.ActiveTextAreaControl.TextArea;
            int lineNumber = textArea.Caret.Line;

            if (lineNumber < 0 || lineNumber >= document.TotalNumberOfLines)
                return;

            var lineSegment = document.GetLineSegment(lineNumber);
            string lineText = document.GetText(lineSegment.Offset, lineSegment.Length);

            var trimmed = lineText.TrimStart();
            if (trimmed.StartsWith("--"))
            {
                int commentIndex = lineText.IndexOf("--");
                if (commentIndex >= 0)
                {
                    int removeCount = 2;
                    // Check if there's a space after --
                    if (commentIndex + 2 < lineText.Length && lineText[commentIndex + 2] == ' ')
                    {
                        removeCount = 3;
                    }

                    document.Remove(lineSegment.Offset + commentIndex, removeCount);
                }
            }
        }

        private void ExportPlainText_Click(object? sender, EventArgs e)
        {
            if (resultsGridView.DataSource == null)
            {
                MessageBox.Show("No results to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var saveDialog = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                DefaultExt = "txt",
                Title = "Export to Plain Text"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sb = new StringBuilder();

                    // Headers
                    for (int i = 0; i < resultsGridView.Columns.Count; i++)
                    {
                        sb.Append(resultsGridView.Columns[i].HeaderText);
                        if (i < resultsGridView.Columns.Count - 1)
                            sb.Append('\t');
                    }
                    sb.AppendLine();

                    // Data
                    foreach (DataGridViewRow row in resultsGridView.Rows)
                    {
                        for (int i = 0; i < resultsGridView.Columns.Count; i++)
                        {
                            var value = row.Cells[i].Value;
                            sb.Append(value?.ToString() ?? "");
                            if (i < resultsGridView.Columns.Count - 1)
                                sb.Append('\t');
                        }
                        sb.AppendLine();
                    }

                    File.WriteAllText(saveDialog.FileName, sb.ToString());
                    MessageBox.Show("Export completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting data:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportCsv_Click(object? sender, EventArgs e)
        {
            if (resultsGridView.DataSource == null)
            {
                MessageBox.Show("No results to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var saveDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                DefaultExt = "csv",
                Title = "Export to CSV"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sb = new StringBuilder();

                    // Headers
                    for (int i = 0; i < resultsGridView.Columns.Count; i++)
                    {
                        sb.Append(EscapeCsvValue(resultsGridView.Columns[i].HeaderText));
                        if (i < resultsGridView.Columns.Count - 1)
                            sb.Append(',');
                    }
                    sb.AppendLine();

                    // Data
                    foreach (DataGridViewRow row in resultsGridView.Rows)
                    {
                        for (int i = 0; i < resultsGridView.Columns.Count; i++)
                        {
                            var value = row.Cells[i].Value;
                            sb.Append(EscapeCsvValue(value?.ToString() ?? ""));
                            if (i < resultsGridView.Columns.Count - 1)
                                sb.Append(',');
                        }
                        sb.AppendLine();
                    }

                    File.WriteAllText(saveDialog.FileName, sb.ToString());
                    MessageBox.Show("Export completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting data:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string EscapeCsvValue(string value)
        {
            if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }

        private void ExportXlsx_Click(object? sender, EventArgs e)
        {
            //if (resultsGridView.DataSource == null)
            //{
            //    MessageBox.Show("No results to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            //using var saveDialog = new SaveFileDialog
            //{
            //    Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
            //    DefaultExt = "xlsx",
            //    Title = "Export to XLSX"
            //};

            //if (saveDialog.ShowDialog() == DialogResult.OK)
            //{
            //    try
            //    {
            //        using var workbook = new XLWorkbook();
            //        var worksheet = workbook.Worksheets.Add("Query Results");

            //        // Headers
            //        for (int i = 0; i < resultsGridView.Columns.Count; i++)
            //        {
            //            worksheet.Cell(1, i + 1).Value = resultsGridView.Columns[i].HeaderText;
            //            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            //        }

            //        // Data
            //        int rowIndex = 2;
            //        foreach (DataGridViewRow row in resultsGridView.Rows)
            //        {
            //            for (int i = 0; i < resultsGridView.Columns.Count; i++)
            //            {
            //                var value = row.Cells[i].Value;
            //                if (value != null && value != DBNull.Value)
            //                {
            //                    worksheet.Cell(rowIndex, i + 1).Value = XLCellValue.FromObject(value);
            //                }
            //            }
            //            rowIndex++;
            //        }

            //        // Auto-fit columns
            //        worksheet.Columns().AdjustToContents();

            //        // Save
            //        workbook.SaveAs(saveDialog.FileName);

            //        MessageBox.Show("Export completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"Error exporting data:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Name = "QueryDialog";
            Text = "SQL Query Editor";
            ResumeLayout(false);
        }
    }
}
