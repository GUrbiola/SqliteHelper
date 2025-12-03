
namespace SqliteHelper
{
    public partial class CreateTableDialog : Form
    {
        private TextBox tableNameTextBox = null!;
        private ICSharpCode.TextEditor.TextEditorControl sqlTextEditor = null!;
        private Button btnOK = null!;
        private Button btnCancel = null!;

        public string TableName { get; private set; } = string.Empty;
        public string CreateTableSQL { get; private set; } = string.Empty;

        public CreateTableDialog()
        {
            InitializeComponent();
            SetupDialog();
        }

        private void SetupDialog()
        {
            Text = "Create New Table";
            Size = new Size(700, 500);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(600, 400);

            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 2,
                Padding = new Padding(10)
            };

            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Table Name
            var lblTableName = new Label
            {
                Text = "Table Name:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            mainPanel.Controls.Add(lblTableName, 0, 0);

            tableNameTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(3)
            };
            tableNameTextBox.TextChanged += TableNameTextBox_TextChanged;
            mainPanel.Controls.Add(tableNameTextBox, 1, 0);

            // SQL Label
            var lblSQL = new Label
            {
                Text = "SQL:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            mainPanel.Controls.Add(lblSQL, 0, 1);
            mainPanel.SetColumnSpan(lblSQL, 2);

            // SQL TextEditor
            var editorPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(3)
            };

            sqlTextEditor = new ICSharpCode.TextEditor.TextEditorControl
            {
                Dock = DockStyle.Fill,
                ShowLineNumbers = true,
                ShowVRuler = false,
                ShowInvalidLines = false,
                AllowCaretBeyondEOL = false
            };

            sqlTextEditor.SetHighlighting("SQL");
            sqlTextEditor.Font = new Font("Consolas", 10);
            sqlTextEditor.Text = "-- Enter CREATE TABLE statement\n-- Example:\n-- CREATE TABLE table_name (\n--     id INTEGER PRIMARY KEY AUTOINCREMENT,\n--     name TEXT NOT NULL,\n--     age INTEGER\n-- );";

            editorPanel.Controls.Add(sqlTextEditor);
            mainPanel.Controls.Add(editorPanel, 0, 2);
            mainPanel.SetColumnSpan(editorPanel, 2);

            // Button panel
            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(3)
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Width = 75,
                Height = 30
            };

            btnOK = new Button
            {
                Text = "OK",
                Width = 75,
                Height = 30,
                Enabled = false
            };
            btnOK.Click += BtnOK_Click;

            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Controls.Add(btnOK);

            mainPanel.Controls.Add(buttonPanel, 0, 3);
            mainPanel.SetColumnSpan(buttonPanel, 2);

            Controls.Add(mainPanel);

            AcceptButton = btnOK;
            CancelButton = btnCancel;
        }

        private void TableNameTextBox_TextChanged(object? sender, EventArgs e)
        {
            // Enable OK button only if table name is provided
            btnOK.Enabled = !string.IsNullOrWhiteSpace(tableNameTextBox.Text);

            // Auto-update CREATE TABLE template if SQL is still default
            if (!string.IsNullOrWhiteSpace(tableNameTextBox.Text))
            {
                string currentSQL = sqlTextEditor.Text.Trim();
                if (currentSQL.StartsWith("-- Enter CREATE TABLE"))
                {
                    string tableName = tableNameTextBox.Text.Trim();
                    sqlTextEditor.Text = $@"CREATE TABLE {tableName} (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL
);";
                }
            }
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            TableName = tableNameTextBox.Text.Trim();
            CreateTableSQL = sqlTextEditor.Text.Trim();

            if (string.IsNullOrWhiteSpace(TableName))
            {
                MessageBox.Show("Please enter a table name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrWhiteSpace(CreateTableSQL))
            {
                MessageBox.Show("Please enter the CREATE TABLE SQL statement.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }

            if (!CreateTableSQL.ToUpper().Contains("CREATE TABLE"))
            {
                MessageBox.Show("The SQL must contain a CREATE TABLE statement.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 500);
            Name = "CreateTableDialog";
            Text = "Create New Table";
            ResumeLayout(false);
        }
    }
}
