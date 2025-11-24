namespace SqliteHelper
{
    public partial class TableEditorDialog : Form
    {
        public TableDefinition TableDefinition { get; private set; }

        public TableEditorDialog(TableDefinition? existingTable = null)
        {
            InitializeComponent();

            // Setup DataGridView columns FIRST
            SetupDataGridView();

            if (existingTable != null)
            {
                // Create a copy to avoid modifying the original
                TableDefinition = new TableDefinition
                {
                    Name = existingTable.Name,
                    Position = existingTable.Position,
                    Columns = new List<ColumnDefinition>()
                };

                // Copy each column
                foreach (var col in existingTable.Columns)
                {
                    TableDefinition.Columns.Add(new ColumnDefinition
                    {
                        Name = col.Name,
                        DataType = col.DataType,
                        IsPrimaryKey = col.IsPrimaryKey,
                        IsNotNull = col.IsNotNull,
                        IsAutoIncrement = col.IsAutoIncrement,
                        DefaultValue = col.DefaultValue
                    });
                }

                textBoxTableName.Text = existingTable.Name;

                // Now add rows to the grid
                foreach (var column in TableDefinition.Columns)
                {
                    AddColumnToGrid(column);
                }

                this.Text = "Edit Table";
            }
            else
            {
                TableDefinition = new TableDefinition();
                this.Text = "New Table";
            }
        }

        private void SetupDataGridView()
        {
            dataGridViewColumns.AutoGenerateColumns = false;

            dataGridViewColumns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ColumnName",
                HeaderText = "Column Name",
                DataPropertyName = "Name",
                Width = 150
            });

            var dataTypeColumn = new DataGridViewComboBoxColumn
            {
                Name = "DataType",
                HeaderText = "Data Type",
                DataPropertyName = "DataType",
                Width = 100
            };
            dataTypeColumn.Items.AddRange("INTEGER", "TEXT", "REAL", "BLOB", "NUMERIC");
            dataGridViewColumns.Columns.Add(dataTypeColumn);

            dataGridViewColumns.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsPrimaryKey",
                HeaderText = "Primary Key",
                DataPropertyName = "IsPrimaryKey",
                Width = 80
            });

            dataGridViewColumns.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsNotNull",
                HeaderText = "Not Null",
                DataPropertyName = "IsNotNull",
                Width = 70
            });

            dataGridViewColumns.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsAutoIncrement",
                HeaderText = "Auto Inc",
                DataPropertyName = "IsAutoIncrement",
                Width = 70
            });

            dataGridViewColumns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DefaultValue",
                HeaderText = "Default Value",
                DataPropertyName = "DefaultValue",
                Width = 120
            });
        }

        private void AddColumnToGrid(ColumnDefinition column)
        {
            int rowIndex = dataGridViewColumns.Rows.Add();
            var row = dataGridViewColumns.Rows[rowIndex];
            row.Cells["ColumnName"].Value = column.Name;
            row.Cells["DataType"].Value = column.DataType;
            row.Cells["IsPrimaryKey"].Value = column.IsPrimaryKey;
            row.Cells["IsNotNull"].Value = column.IsNotNull;
            row.Cells["IsAutoIncrement"].Value = column.IsAutoIncrement;
            row.Cells["DefaultValue"].Value = column.DefaultValue;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxTableName.Text))
            {
                MessageBox.Show("Please enter a table name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dataGridViewColumns.Rows.Count == 0 ||
                (dataGridViewColumns.Rows.Count == 1 && dataGridViewColumns.Rows[0].IsNewRow))
            {
                MessageBox.Show("Please add at least one column.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TableDefinition.Name = textBoxTableName.Text;
            TableDefinition.Columns.Clear();

            foreach (DataGridViewRow row in dataGridViewColumns.Rows)
            {
                if (row.IsNewRow) continue;

                var columnName = row.Cells["ColumnName"].Value?.ToString();
                if (string.IsNullOrWhiteSpace(columnName))
                    continue;

                var column = new ColumnDefinition
                {
                    Name = columnName,
                    DataType = row.Cells["DataType"].Value?.ToString() ?? "TEXT",
                    IsPrimaryKey = Convert.ToBoolean(row.Cells["IsPrimaryKey"].Value ?? false),
                    IsNotNull = Convert.ToBoolean(row.Cells["IsNotNull"].Value ?? false),
                    IsAutoIncrement = Convert.ToBoolean(row.Cells["IsAutoIncrement"].Value ?? false),
                    DefaultValue = row.Cells["DefaultValue"].Value?.ToString()
                };

                TableDefinition.Columns.Add(column);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            if (dataGridViewColumns.SelectedRows.Count > 0)
            {
                int rowIndex = dataGridViewColumns.SelectedRows[0].Index;
                if (rowIndex > 0 && !dataGridViewColumns.Rows[rowIndex].IsNewRow)
                {
                    SwapRows(rowIndex, rowIndex - 1);
                    dataGridViewColumns.Rows[rowIndex - 1].Selected = true;
                }
            }
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            if (dataGridViewColumns.SelectedRows.Count > 0)
            {
                int rowIndex = dataGridViewColumns.SelectedRows[0].Index;
                if (rowIndex < dataGridViewColumns.Rows.Count - 2 && !dataGridViewColumns.Rows[rowIndex].IsNewRow)
                {
                    SwapRows(rowIndex, rowIndex + 1);
                    dataGridViewColumns.Rows[rowIndex + 1].Selected = true;
                }
            }
        }

        private void SwapRows(int index1, int index2)
        {
            var row1 = dataGridViewColumns.Rows[index1];
            var row2 = dataGridViewColumns.Rows[index2];

            // Store values from row1
            var values1 = new object?[row1.Cells.Count];
            for (int i = 0; i < row1.Cells.Count; i++)
            {
                values1[i] = row1.Cells[i].Value;
            }

            // Store values from row2
            var values2 = new object?[row2.Cells.Count];
            for (int i = 0; i < row2.Cells.Count; i++)
            {
                values2[i] = row2.Cells[i].Value;
            }

            // Swap values
            for (int i = 0; i < row1.Cells.Count; i++)
            {
                row1.Cells[i].Value = values2[i];
                row2.Cells[i].Value = values1[i];
            }
        }
    }
}
