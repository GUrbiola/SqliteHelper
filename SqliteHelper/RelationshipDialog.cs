namespace SqliteHelper
{
    public partial class RelationshipDialog : Form
    {
        public Relationship Relationship { get; private set; }
        private DatabaseProject project;

        public RelationshipDialog(DatabaseProject project, Relationship? existingRelationship = null)
        {
            this.project = project;
            InitializeComponent();

            // Populate table combos
            foreach (var table in project.Tables)
            {
                comboFromTable.Items.Add(table.Name);
                comboToTable.Items.Add(table.Name);
            }

            if (existingRelationship != null)
            {
                Relationship = existingRelationship;

                // Set from table first (this will populate from columns)
                comboFromTable.SelectedItem = existingRelationship.FromTable;
                // Then set the from column
                comboFromColumn.SelectedItem = existingRelationship.FromColumn;

                // Set to table (this will populate to columns)
                comboToTable.SelectedItem = existingRelationship.ToTable;
                // Then set the to column
                comboToColumn.SelectedItem = existingRelationship.ToColumn;

                comboRelationType.SelectedItem = existingRelationship.RelationshipType;
                this.Text = "Edit Relationship";
            }
            else
            {
                Relationship = new Relationship();
                comboRelationType.SelectedIndex = 0;
                this.Text = "New Relationship";
            }
        }

        private void comboFromTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboFromColumn.Items.Clear();
            if (comboFromTable.SelectedItem is string tableName)
            {
                var table = project.Tables.FirstOrDefault(t => t.Name == tableName);
                if (table != null)
                {
                    foreach (var column in table.Columns)
                    {
                        comboFromColumn.Items.Add(column.Name);
                    }
                }
            }
        }

        private void comboToTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboToColumn.Items.Clear();
            if (comboToTable.SelectedItem is string tableName)
            {
                var table = project.Tables.FirstOrDefault(t => t.Name == tableName);
                if (table != null)
                {
                    foreach (var column in table.Columns)
                    {
                        comboToColumn.Items.Add(column.Name);
                    }
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (comboFromTable.SelectedItem == null || comboFromColumn.SelectedItem == null ||
                comboToTable.SelectedItem == null || comboToColumn.SelectedItem == null)
            {
                MessageBox.Show("Please select all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Relationship.FromTable = comboFromTable.SelectedItem.ToString() ?? string.Empty;
            Relationship.FromColumn = comboFromColumn.SelectedItem.ToString() ?? string.Empty;
            Relationship.ToTable = comboToTable.SelectedItem.ToString() ?? string.Empty;
            Relationship.ToColumn = comboToColumn.SelectedItem.ToString() ?? string.Empty;
            Relationship.RelationshipType = comboRelationType.SelectedItem?.ToString() ?? "One-to-Many";

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
