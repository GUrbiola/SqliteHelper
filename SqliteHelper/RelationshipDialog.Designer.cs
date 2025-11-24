namespace SqliteHelper
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
            labelFrom = new Label();
            labelFromTable = new Label();
            comboFromTable = new ComboBox();
            labelFromColumn = new Label();
            comboFromColumn = new ComboBox();
            labelTo = new Label();
            labelToTable = new Label();
            comboToTable = new ComboBox();
            labelToColumn = new Label();
            comboToColumn = new ComboBox();
            labelRelationType = new Label();
            comboRelationType = new ComboBox();
            buttonOK = new Button();
            buttonCancel = new Button();
            SuspendLayout();
            //
            // labelFrom
            //
            labelFrom.AutoSize = true;
            labelFrom.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelFrom.Location = new Point(12, 15);
            labelFrom.Name = "labelFrom";
            labelFrom.Size = new Size(38, 15);
            labelFrom.TabIndex = 0;
            labelFrom.Text = "From:";
            //
            // labelFromTable
            //
            labelFromTable.AutoSize = true;
            labelFromTable.Location = new Point(12, 40);
            labelFromTable.Name = "labelFromTable";
            labelFromTable.Size = new Size(37, 15);
            labelFromTable.TabIndex = 1;
            labelFromTable.Text = "Table:";
            //
            // comboFromTable
            //
            comboFromTable.DropDownStyle = ComboBoxStyle.DropDownList;
            comboFromTable.FormattingEnabled = true;
            comboFromTable.Location = new Point(80, 37);
            comboFromTable.Name = "comboFromTable";
            comboFromTable.Size = new Size(250, 23);
            comboFromTable.TabIndex = 2;
            comboFromTable.SelectedIndexChanged += comboFromTable_SelectedIndexChanged;
            //
            // labelFromColumn
            //
            labelFromColumn.AutoSize = true;
            labelFromColumn.Location = new Point(12, 70);
            labelFromColumn.Name = "labelFromColumn";
            labelFromColumn.Size = new Size(53, 15);
            labelFromColumn.TabIndex = 3;
            labelFromColumn.Text = "Column:";
            //
            // comboFromColumn
            //
            comboFromColumn.DropDownStyle = ComboBoxStyle.DropDownList;
            comboFromColumn.FormattingEnabled = true;
            comboFromColumn.Location = new Point(80, 67);
            comboFromColumn.Name = "comboFromColumn";
            comboFromColumn.Size = new Size(250, 23);
            comboFromColumn.TabIndex = 4;
            //
            // labelTo
            //
            labelTo.AutoSize = true;
            labelTo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelTo.Location = new Point(12, 110);
            labelTo.Name = "labelTo";
            labelTo.Size = new Size(23, 15);
            labelTo.TabIndex = 5;
            labelTo.Text = "To:";
            //
            // labelToTable
            //
            labelToTable.AutoSize = true;
            labelToTable.Location = new Point(12, 135);
            labelToTable.Name = "labelToTable";
            labelToTable.Size = new Size(37, 15);
            labelToTable.TabIndex = 6;
            labelToTable.Text = "Table:";
            //
            // comboToTable
            //
            comboToTable.DropDownStyle = ComboBoxStyle.DropDownList;
            comboToTable.FormattingEnabled = true;
            comboToTable.Location = new Point(80, 132);
            comboToTable.Name = "comboToTable";
            comboToTable.Size = new Size(250, 23);
            comboToTable.TabIndex = 7;
            comboToTable.SelectedIndexChanged += comboToTable_SelectedIndexChanged;
            //
            // labelToColumn
            //
            labelToColumn.AutoSize = true;
            labelToColumn.Location = new Point(12, 165);
            labelToColumn.Name = "labelToColumn";
            labelToColumn.Size = new Size(53, 15);
            labelToColumn.TabIndex = 8;
            labelToColumn.Text = "Column:";
            //
            // comboToColumn
            //
            comboToColumn.DropDownStyle = ComboBoxStyle.DropDownList;
            comboToColumn.FormattingEnabled = true;
            comboToColumn.Location = new Point(80, 162);
            comboToColumn.Name = "comboToColumn";
            comboToColumn.Size = new Size(250, 23);
            comboToColumn.TabIndex = 9;
            //
            // labelRelationType
            //
            labelRelationType.AutoSize = true;
            labelRelationType.Location = new Point(12, 205);
            labelRelationType.Name = "labelRelationType";
            labelRelationType.Size = new Size(34, 15);
            labelRelationType.TabIndex = 10;
            labelRelationType.Text = "Type:";
            //
            // comboRelationType
            //
            comboRelationType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRelationType.FormattingEnabled = true;
            comboRelationType.Items.AddRange(new object[] { "One-to-One", "One-to-Many", "Many-to-Many" });
            comboRelationType.Location = new Point(80, 202);
            comboRelationType.Name = "comboRelationType";
            comboRelationType.Size = new Size(250, 23);
            comboRelationType.TabIndex = 11;
            //
            // buttonOK
            //
            buttonOK.Location = new Point(174, 245);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 12;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += buttonOK_Click;
            //
            // buttonCancel
            //
            buttonCancel.Location = new Point(255, 245);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 13;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            //
            // RelationshipDialog
            //
            AcceptButton = buttonOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new Size(344, 280);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOK);
            Controls.Add(comboRelationType);
            Controls.Add(labelRelationType);
            Controls.Add(comboToColumn);
            Controls.Add(labelToColumn);
            Controls.Add(comboToTable);
            Controls.Add(labelToTable);
            Controls.Add(labelTo);
            Controls.Add(comboFromColumn);
            Controls.Add(labelFromColumn);
            Controls.Add(comboFromTable);
            Controls.Add(labelFromTable);
            Controls.Add(labelFrom);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RelationshipDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Relationship";
            ResumeLayout(false);
            PerformLayout();
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
    }
}
