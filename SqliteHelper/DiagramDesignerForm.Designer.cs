namespace SqliteHelper
{
    partial class DiagramDesignerForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            menuStrip = new MenuStrip();
            scriptsToolStripMenuItem = new ToolStripMenuItem();
            generateInitScriptToolStripMenuItem = new ToolStripMenuItem();
            generateUpdateScriptToolStripMenuItem = new ToolStripMenuItem();
            toolStrip = new ToolStrip();
            addTableButton = new ToolStripButton();
            addRelationshipButton = new ToolStripButton();
            manageRelationshipsButton = new ToolStripButton();
            addCommentButton = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            saveButton = new ToolStripButton();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            diagramPanel = new Panel();
            menuStrip.SuspendLayout();
            toolStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { scriptsToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1024, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            // 
            // scriptsToolStripMenuItem
            // 
            scriptsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { generateInitScriptToolStripMenuItem, generateUpdateScriptToolStripMenuItem });
            scriptsToolStripMenuItem.Name = "scriptsToolStripMenuItem";
            scriptsToolStripMenuItem.Size = new Size(54, 20);
            scriptsToolStripMenuItem.Text = "&Scripts";
            // 
            // generateInitScriptToolStripMenuItem
            // 
            generateInitScriptToolStripMenuItem.Name = "generateInitScriptToolStripMenuItem";
            generateInitScriptToolStripMenuItem.Size = new Size(230, 22);
            generateInitScriptToolStripMenuItem.Text = "Generate &Initialization Script...";
            generateInitScriptToolStripMenuItem.Click += generateInitScriptToolStripMenuItem_Click;
            // 
            // generateUpdateScriptToolStripMenuItem
            // 
            generateUpdateScriptToolStripMenuItem.Name = "generateUpdateScriptToolStripMenuItem";
            generateUpdateScriptToolStripMenuItem.Size = new Size(230, 22);
            generateUpdateScriptToolStripMenuItem.Text = "Generate &Update Script...";
            generateUpdateScriptToolStripMenuItem.Click += generateUpdateScriptToolStripMenuItem_Click;
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { addTableButton, addRelationshipButton, manageRelationshipsButton, addCommentButton, toolStripSeparator1, saveButton });
            toolStrip.Location = new Point(0, 24);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(1024, 25);
            toolStrip.TabIndex = 1;
            toolStrip.Text = "toolStrip1";
            // 
            // addTableButton
            // 
            addTableButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            addTableButton.Name = "addTableButton";
            addTableButton.Size = new Size(63, 22);
            addTableButton.Text = "Add Table";
            addTableButton.Click += addTableButton_Click;
            // 
            // addRelationshipButton
            // 
            addRelationshipButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            addRelationshipButton.Name = "addRelationshipButton";
            addRelationshipButton.Size = new Size(101, 22);
            addRelationshipButton.Text = "Add Relationship";
            addRelationshipButton.Click += addRelationshipButton_Click;
            // 
            // manageRelationshipsButton
            // 
            manageRelationshipsButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            manageRelationshipsButton.Name = "manageRelationshipsButton";
            manageRelationshipsButton.Size = new Size(127, 22);
            manageRelationshipsButton.Text = "Manage Relationships";
            manageRelationshipsButton.Click += manageRelationshipsButton_Click;
            // 
            // addCommentButton
            // 
            addCommentButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            addCommentButton.Name = "addCommentButton";
            addCommentButton.Size = new Size(90, 22);
            addCommentButton.Text = "Add Comment";
            addCommentButton.Click += addCommentButton_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // saveButton
            // 
            saveButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(35, 22);
            saveButton.Text = "Save";
            saveButton.Click += saveButton_Click;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new Point(0, 746);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1024, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(39, 17);
            toolStripStatusLabel.Text = "Ready";
            // 
            // diagramPanel
            // 
            diagramPanel.AutoScroll = true;
            diagramPanel.BackColor = Color.WhiteSmoke;
            diagramPanel.Dock = DockStyle.Fill;
            diagramPanel.Location = new Point(0, 49);
            diagramPanel.Name = "diagramPanel";
            diagramPanel.Size = new Size(1024, 697);
            diagramPanel.TabIndex = 2;
            // 
            // DiagramDesignerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 768);
            Controls.Add(diagramPanel);
            Controls.Add(statusStrip);
            Controls.Add(toolStrip);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "DiagramDesignerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Diagram Designer";
            WindowState = FormWindowState.Maximized;
            FormClosing += DiagramDesignerForm_FormClosing;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem scriptsToolStripMenuItem;
        private ToolStripMenuItem generateInitScriptToolStripMenuItem;
        private ToolStripMenuItem generateUpdateScriptToolStripMenuItem;
        private ToolStrip toolStrip;
        private ToolStripButton addTableButton;
        private ToolStripButton addRelationshipButton;
        private ToolStripButton manageRelationshipsButton;
        private ToolStripButton addCommentButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton saveButton;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private Panel diagramPanel;
    }
}
