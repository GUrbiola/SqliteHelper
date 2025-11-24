namespace SqliteHelper
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
            labelRelationships = new Label();
            listBoxRelationships = new ListBox();
            buttonAdd = new Button();
            buttonEdit = new Button();
            buttonDelete = new Button();
            buttonClose = new Button();
            SuspendLayout();
            //
            // labelRelationships
            //
            labelRelationships.AutoSize = true;
            labelRelationships.Location = new Point(12, 15);
            labelRelationships.Name = "labelRelationships";
            labelRelationships.Size = new Size(83, 15);
            labelRelationships.TabIndex = 0;
            labelRelationships.Text = "Relationships:";
            //
            // listBoxRelationships
            //
            listBoxRelationships.FormattingEnabled = true;
            listBoxRelationships.ItemHeight = 15;
            listBoxRelationships.Location = new Point(12, 35);
            listBoxRelationships.Name = "listBoxRelationships";
            listBoxRelationships.Size = new Size(560, 319);
            listBoxRelationships.TabIndex = 1;
            listBoxRelationships.SelectedIndexChanged += listBoxRelationships_SelectedIndexChanged;
            //
            // buttonAdd
            //
            buttonAdd.Location = new Point(578, 35);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(90, 30);
            buttonAdd.TabIndex = 2;
            buttonAdd.Text = "Add";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            //
            // buttonEdit
            //
            buttonEdit.Enabled = false;
            buttonEdit.Location = new Point(578, 71);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new Size(90, 30);
            buttonEdit.TabIndex = 3;
            buttonEdit.Text = "Edit";
            buttonEdit.UseVisualStyleBackColor = true;
            buttonEdit.Click += buttonEdit_Click;
            //
            // buttonDelete
            //
            buttonDelete.Enabled = false;
            buttonDelete.Location = new Point(578, 107);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(90, 30);
            buttonDelete.TabIndex = 4;
            buttonDelete.Text = "Delete";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            //
            // buttonClose
            //
            buttonClose.Location = new Point(578, 324);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(90, 30);
            buttonClose.TabIndex = 5;
            buttonClose.Text = "Close";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            //
            // ManageRelationshipsDialog
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(680, 370);
            Controls.Add(buttonClose);
            Controls.Add(buttonDelete);
            Controls.Add(buttonEdit);
            Controls.Add(buttonAdd);
            Controls.Add(listBoxRelationships);
            Controls.Add(labelRelationships);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ManageRelationshipsDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Manage Relationships";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelRelationships;
        private ListBox listBoxRelationships;
        private Button buttonAdd;
        private Button buttonEdit;
        private Button buttonDelete;
        private Button buttonClose;
    }
}
