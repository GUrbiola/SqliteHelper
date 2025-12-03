using System.Drawing;
using System.Windows.Forms;

namespace SqliteHelper48
{
    partial class RelationshipDialog
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
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelFromTable = new System.Windows.Forms.Label();
            this.comboFromTable = new System.Windows.Forms.ComboBox();
            this.labelFromColumn = new System.Windows.Forms.Label();
            this.comboFromColumn = new System.Windows.Forms.ComboBox();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelToTable = new System.Windows.Forms.Label();
            this.comboToTable = new System.Windows.Forms.ComboBox();
            this.labelToColumn = new System.Windows.Forms.Label();
            this.comboToColumn = new System.Windows.Forms.ComboBox();
            this.labelRelationType = new System.Windows.Forms.Label();
            this.comboRelationType = new System.Windows.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.themeManager1 = new ZidUtilities.CommonCode.Win.Controls.ThemeManager(this.components);
            this.SuspendLayout();
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelFrom.Location = new System.Drawing.Point(10, 13);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(39, 15);
            this.labelFrom.TabIndex = 0;
            this.labelFrom.Text = "From:";
            // 
            // labelFromTable
            // 
            this.labelFromTable.AutoSize = true;
            this.labelFromTable.Location = new System.Drawing.Point(10, 35);
            this.labelFromTable.Name = "labelFromTable";
            this.labelFromTable.Size = new System.Drawing.Size(37, 13);
            this.labelFromTable.TabIndex = 1;
            this.labelFromTable.Text = "Table:";
            // 
            // comboFromTable
            // 
            this.comboFromTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFromTable.FormattingEnabled = true;
            this.comboFromTable.Location = new System.Drawing.Point(69, 32);
            this.comboFromTable.Name = "comboFromTable";
            this.comboFromTable.Size = new System.Drawing.Size(215, 21);
            this.comboFromTable.TabIndex = 2;
            this.comboFromTable.SelectedIndexChanged += new System.EventHandler(this.comboFromTable_SelectedIndexChanged);
            // 
            // labelFromColumn
            // 
            this.labelFromColumn.AutoSize = true;
            this.labelFromColumn.Location = new System.Drawing.Point(10, 61);
            this.labelFromColumn.Name = "labelFromColumn";
            this.labelFromColumn.Size = new System.Drawing.Size(45, 13);
            this.labelFromColumn.TabIndex = 3;
            this.labelFromColumn.Text = "Column:";
            // 
            // comboFromColumn
            // 
            this.comboFromColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFromColumn.FormattingEnabled = true;
            this.comboFromColumn.Location = new System.Drawing.Point(69, 58);
            this.comboFromColumn.Name = "comboFromColumn";
            this.comboFromColumn.Size = new System.Drawing.Size(215, 21);
            this.comboFromColumn.TabIndex = 4;
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelTo.Location = new System.Drawing.Point(10, 95);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(23, 15);
            this.labelTo.TabIndex = 5;
            this.labelTo.Text = "To:";
            // 
            // labelToTable
            // 
            this.labelToTable.AutoSize = true;
            this.labelToTable.Location = new System.Drawing.Point(10, 117);
            this.labelToTable.Name = "labelToTable";
            this.labelToTable.Size = new System.Drawing.Size(37, 13);
            this.labelToTable.TabIndex = 6;
            this.labelToTable.Text = "Table:";
            // 
            // comboToTable
            // 
            this.comboToTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboToTable.FormattingEnabled = true;
            this.comboToTable.Location = new System.Drawing.Point(69, 114);
            this.comboToTable.Name = "comboToTable";
            this.comboToTable.Size = new System.Drawing.Size(215, 21);
            this.comboToTable.TabIndex = 7;
            this.comboToTable.SelectedIndexChanged += new System.EventHandler(this.comboToTable_SelectedIndexChanged);
            // 
            // labelToColumn
            // 
            this.labelToColumn.AutoSize = true;
            this.labelToColumn.Location = new System.Drawing.Point(10, 143);
            this.labelToColumn.Name = "labelToColumn";
            this.labelToColumn.Size = new System.Drawing.Size(45, 13);
            this.labelToColumn.TabIndex = 8;
            this.labelToColumn.Text = "Column:";
            // 
            // comboToColumn
            // 
            this.comboToColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboToColumn.FormattingEnabled = true;
            this.comboToColumn.Location = new System.Drawing.Point(69, 140);
            this.comboToColumn.Name = "comboToColumn";
            this.comboToColumn.Size = new System.Drawing.Size(215, 21);
            this.comboToColumn.TabIndex = 9;
            // 
            // labelRelationType
            // 
            this.labelRelationType.AutoSize = true;
            this.labelRelationType.Location = new System.Drawing.Point(10, 178);
            this.labelRelationType.Name = "labelRelationType";
            this.labelRelationType.Size = new System.Drawing.Size(34, 13);
            this.labelRelationType.TabIndex = 10;
            this.labelRelationType.Text = "Type:";
            // 
            // comboRelationType
            // 
            this.comboRelationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRelationType.FormattingEnabled = true;
            this.comboRelationType.Items.AddRange(new object[] {
            "One-to-One",
            "One-to-Many",
            "Many-to-Many"});
            this.comboRelationType.Location = new System.Drawing.Point(69, 175);
            this.comboRelationType.Name = "comboRelationType";
            this.comboRelationType.Size = new System.Drawing.Size(215, 21);
            this.comboRelationType.TabIndex = 11;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(149, 212);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(64, 20);
            this.buttonOK.TabIndex = 12;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(219, 212);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(64, 20);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // themeManager1
            // 
            this.themeManager1.ExcludedControlTypes.Add("ICSharpCode.TextEditor.TextEditorControl");
            this.themeManager1.ExcludedControlTypes.Add("ZidUtilities.CommonCode.ICSharpTextEditor.ExtendedEditor");
            this.themeManager1.ParentForm = this;
            // 
            // RelationshipDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(295, 243);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.comboRelationType);
            this.Controls.Add(this.labelRelationType);
            this.Controls.Add(this.comboToColumn);
            this.Controls.Add(this.labelToColumn);
            this.Controls.Add(this.comboToTable);
            this.Controls.Add(this.labelToTable);
            this.Controls.Add(this.labelTo);
            this.Controls.Add(this.comboFromColumn);
            this.Controls.Add(this.labelFromColumn);
            this.Controls.Add(this.comboFromTable);
            this.Controls.Add(this.labelFromTable);
            this.Controls.Add(this.labelFrom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RelationshipDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Relationship";
            this.Load += new System.EventHandler(this.RelationshipDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label labelFrom;
        private Label labelFromTable;
        private ComboBox comboFromTable;
        private Label labelFromColumn;
        private ComboBox comboFromColumn;
        private Label labelTo;
        private Label labelToTable;
        private ComboBox comboToTable;
        private Label labelToColumn;
        private ComboBox comboToColumn;
        private Label labelRelationType;
        private ComboBox comboRelationType;
        private Button buttonOK;
        private Button buttonCancel;
        private ZidUtilities.CommonCode.Win.Controls.ThemeManager themeManager1;
    }
}
