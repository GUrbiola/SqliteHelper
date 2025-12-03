using System.Drawing;
using System.Windows.Forms;

namespace SqliteHelper48
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.scriptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateInitScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateUpdateScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.addTableButton = new System.Windows.Forms.ToolStripButton();
            this.addRelationshipButton = new System.Windows.Forms.ToolStripButton();
            this.manageRelationshipsButton = new System.Windows.Forms.ToolStripButton();
            this.addCommentButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.diagramPanel = new System.Windows.Forms.Panel();
            this.diagramContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.autoArrangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeManager1 = new ZidUtilities.CommonCode.Win.Controls.ThemeManager(this.components);
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.diagramContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scriptsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(878, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // scriptsToolStripMenuItem
            // 
            this.scriptsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateInitScriptToolStripMenuItem,
            this.generateUpdateScriptToolStripMenuItem});
            this.scriptsToolStripMenuItem.Name = "scriptsToolStripMenuItem";
            this.scriptsToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.scriptsToolStripMenuItem.Text = "&Scripts";
            // 
            // generateInitScriptToolStripMenuItem
            // 
            this.generateInitScriptToolStripMenuItem.Name = "generateInitScriptToolStripMenuItem";
            this.generateInitScriptToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.generateInitScriptToolStripMenuItem.Text = "Generate &Initialization Script...";
            // 
            // generateUpdateScriptToolStripMenuItem
            // 
            this.generateUpdateScriptToolStripMenuItem.Name = "generateUpdateScriptToolStripMenuItem";
            this.generateUpdateScriptToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.generateUpdateScriptToolStripMenuItem.Text = "Generate &Update Script...";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTableButton,
            this.addRelationshipButton,
            this.manageRelationshipsButton,
            this.addCommentButton,
            this.toolStripSeparator1,
            this.saveButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(878, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // addTableButton
            // 
            this.addTableButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addTableButton.Name = "addTableButton";
            this.addTableButton.Size = new System.Drawing.Size(63, 22);
            this.addTableButton.Text = "Add Table";
            this.addTableButton.Click += new System.EventHandler(this.addTableButton_Click);
            // 
            // addRelationshipButton
            // 
            this.addRelationshipButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addRelationshipButton.Name = "addRelationshipButton";
            this.addRelationshipButton.Size = new System.Drawing.Size(101, 22);
            this.addRelationshipButton.Text = "Add Relationship";
            this.addRelationshipButton.Click += new System.EventHandler(this.addRelationshipButton_Click);
            // 
            // manageRelationshipsButton
            // 
            this.manageRelationshipsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.manageRelationshipsButton.Name = "manageRelationshipsButton";
            this.manageRelationshipsButton.Size = new System.Drawing.Size(127, 22);
            this.manageRelationshipsButton.Text = "Manage Relationships";
            this.manageRelationshipsButton.Click += new System.EventHandler(this.manageRelationshipsButton_Click);
            // 
            // addCommentButton
            // 
            this.addCommentButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addCommentButton.Name = "addCommentButton";
            this.addCommentButton.Size = new System.Drawing.Size(90, 22);
            this.addCommentButton.Text = "Add Comment";
            this.addCommentButton.Click += new System.EventHandler(this.addCommentButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // saveButton
            // 
            this.saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(35, 22);
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 644);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip.Size = new System.Drawing.Size(878, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Ready";
            // 
            // diagramPanel
            // 
            this.diagramPanel.AutoScroll = true;
            this.diagramPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.diagramPanel.ContextMenuStrip = this.diagramContextMenu;
            this.diagramPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramPanel.Location = new System.Drawing.Point(0, 49);
            this.diagramPanel.Name = "diagramPanel";
            this.diagramPanel.Size = new System.Drawing.Size(878, 595);
            this.diagramPanel.TabIndex = 2;
            // 
            // diagramContextMenu
            // 
            this.diagramContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoArrangeToolStripMenuItem});
            this.diagramContextMenu.Name = "diagramContextMenu";
            this.diagramContextMenu.Size = new System.Drawing.Size(146, 26);
            // 
            // autoArrangeToolStripMenuItem
            // 
            this.autoArrangeToolStripMenuItem.Name = "autoArrangeToolStripMenuItem";
            this.autoArrangeToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.autoArrangeToolStripMenuItem.Text = "&Auto Arrange";
            // 
            // themeManager1
            // 
            this.themeManager1.ParentForm = this;
            // 
            // DiagramDesignerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 666);
            this.Controls.Add(this.diagramPanel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "DiagramDesignerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Diagram Designer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.DiagramDesignerForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.diagramContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private ContextMenuStrip diagramContextMenu;
        private ToolStripMenuItem autoArrangeToolStripMenuItem;
        private ZidUtilities.CommonCode.Win.Controls.ThemeManager themeManager1;
    }
}
