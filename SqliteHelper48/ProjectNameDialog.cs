using System;
using System.Windows.Forms;
using ZidUtilities.CommonCode.Win;

namespace SqliteHelper48
{
    public partial class ProjectNameDialog : Form
    {
        public string ProjectName => textBoxProjectName.Text;

        public ProjectNameDialog()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxProjectName.Text))
            {
                MessageBox.Show("Please enter a project name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ProjectNameDialog_Load(object sender, EventArgs e)
        {
            string savedTheme = Properties.Settings.Default.SelectedTheme;
            themeManager1.Theme = Enum.TryParse<ZidThemes>(savedTheme, out var theme) ? theme : ZidThemes.None;
            themeManager1.ApplyTheme();
        }
    }
}
