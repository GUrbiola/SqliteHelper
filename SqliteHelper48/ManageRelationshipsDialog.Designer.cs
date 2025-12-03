using System.Drawing;
using System.Windows.Forms;

namespace SqliteHelper48
{
    partial class ManageRelationshipsDialog
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
            this.labelRelationships = new System.Windows.Forms.Label();
            this.listBoxRelationships = new System.Windows.Forms.ListBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.themeManager1 = new ZidUtilities.CommonCode.Win.Controls.ThemeManager(this.components);
            this.SuspendLayout();
            // 
            // labelRelationships
            // 
            this.labelRelationships.AutoSize = true;
            this.labelRelationships.Location = new System.Drawing.Point(10, 13);
            this.labelRelationships.Name = "labelRelationships";
            this.labelRelationships.Size = new System.Drawing.Size(73, 13);
            this.labelRelationships.TabIndex = 0;
            this.labelRelationships.Text = "Relationships:";
            // 
            // listBoxRelationships
            // 
            this.listBoxRelationships.FormattingEnabled = true;
            this.listBoxRelationships.Location = new System.Drawing.Point(10, 30);
            this.listBoxRelationships.Name = "listBoxRelationships";
            this.listBoxRelationships.Size = new System.Drawing.Size(481, 277);
            this.listBoxRelationships.TabIndex = 1;
            this.listBoxRelationships.SelectedIndexChanged += new System.EventHandler(this.listBoxRelationships_SelectedIndexChanged);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(495, 30);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(77, 26);
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Enabled = false;
            this.buttonEdit.Location = new System.Drawing.Point(495, 62);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(77, 26);
            this.buttonEdit.TabIndex = 3;
            this.buttonEdit.Text = "Edit";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Enabled = false;
            this.buttonDelete.Location = new System.Drawing.Point(495, 93);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(77, 26);
            this.buttonDelete.TabIndex = 4;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(495, 281);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(77, 26);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // themeManager1
            // 
            this.themeManager1.ExcludedControlTypes.Add("ICSharpCode.TextEditor.TextEditorControl");
            this.themeManager1.ExcludedControlTypes.Add("ZidUtilities.CommonCode.ICSharpTextEditor.ExtendedEditor");
            this.themeManager1.ParentForm = this;
            // 
            // ManageRelationshipsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(583, 321);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.listBoxRelationships);
            this.Controls.Add(this.labelRelationships);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageRelationshipsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Relationships";
            this.Load += new System.EventHandler(this.ManageRelationshipsDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label labelRelationships;
        private ListBox listBoxRelationships;
        private Button buttonAdd;
        private Button buttonEdit;
        private Button buttonDelete;
        private Button buttonClose;
        private ZidUtilities.CommonCode.Win.Controls.ThemeManager themeManager1;
    }
}
