namespace SqliteHelper
{
    public partial class ManageRelationshipsDialog : Form
    {
        private DatabaseProject project;
        public bool RelationshipsChanged { get; private set; }

        public ManageRelationshipsDialog(DatabaseProject project)
        {
            this.project = project;
            InitializeComponent();
            LoadRelationships();
        }

        private void LoadRelationships()
        {
            listBoxRelationships.Items.Clear();

            if (project.Relationships.Count == 0)
            {
                listBoxRelationships.Items.Add("(No relationships defined)");
                buttonEdit.Enabled = false;
                buttonDelete.Enabled = false;
            }
            else
            {
                foreach (var rel in project.Relationships)
                {
                    string display = $"{rel.FromTable}.{rel.FromColumn} -> {rel.ToTable}.{rel.ToColumn} ({rel.RelationshipType})";
                    listBoxRelationships.Items.Add(display);
                }
                buttonEdit.Enabled = false;
                buttonDelete.Enabled = false;
            }
        }

        private void listBoxRelationships_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = listBoxRelationships.SelectedIndex >= 0 &&
                                listBoxRelationships.SelectedIndex < project.Relationships.Count;
            buttonEdit.Enabled = hasSelection;
            buttonDelete.Enabled = hasSelection;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (project.Tables.Count < 2)
            {
                MessageBox.Show("You need at least two tables to create a relationship.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var relationshipDialog = new RelationshipDialog(project))
            {
                if (relationshipDialog.ShowDialog() == DialogResult.OK)
                {
                    project.Relationships.Add(relationshipDialog.Relationship);
                    LoadRelationships();
                    RelationshipsChanged = true;
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            int index = listBoxRelationships.SelectedIndex;
            if (index < 0 || index >= project.Relationships.Count) return;

            var relationship = project.Relationships[index];
            using (var relationshipDialog = new RelationshipDialog(project, relationship))
            {
                if (relationshipDialog.ShowDialog() == DialogResult.OK)
                {
                    // The relationship object is already updated by reference
                    LoadRelationships();
                    RelationshipsChanged = true;
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int index = listBoxRelationships.SelectedIndex;
            if (index < 0 || index >= project.Relationships.Count) return;

            var relationship = project.Relationships[index];
            string message = $"Are you sure you want to delete the relationship:\n\n" +
                           $"{relationship.FromTable}.{relationship.FromColumn} -> {relationship.ToTable}.{relationship.ToColumn}?";

            var result = MessageBox.Show(message, "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                project.Relationships.RemoveAt(index);
                LoadRelationships();
                RelationshipsChanged = true;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
