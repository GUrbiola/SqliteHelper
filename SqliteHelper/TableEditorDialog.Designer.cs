namespace SqliteHelper
{
    partial class TableEditorDialog
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
            labelTableName = new Label();
            textBoxTableName = new TextBox();
            labelColumns = new Label();
            dataGridViewColumns = new DataGridView();
            buttonMoveUp = new Button();
            buttonMoveDown = new Button();
            buttonOK = new Button();
            buttonCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewColumns).BeginInit();
            SuspendLayout();
            //
            // labelTableName
            //
            labelTableName.AutoSize = true;
            labelTableName.Location = new Point(12, 15);
            labelTableName.Name = "labelTableName";
            labelTableName.Size = new Size(73, 15);
            labelTableName.TabIndex = 0;
            labelTableName.Text = "Table Name:";
            //
            // textBoxTableName
            //
            textBoxTableName.Location = new Point(91, 12);
            textBoxTableName.Name = "textBoxTableName";
            textBoxTableName.Size = new Size(581, 23);
            textBoxTableName.TabIndex = 1;
            //
            // labelColumns
            //
            labelColumns.AutoSize = true;
            labelColumns.Location = new Point(12, 50);
            labelColumns.Name = "labelColumns";
            labelColumns.Size = new Size(58, 15);
            labelColumns.TabIndex = 2;
            labelColumns.Text = "Columns:";
            //
            // dataGridViewColumns
            //
            dataGridViewColumns.AllowUserToOrderColumns = true;
            dataGridViewColumns.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewColumns.Location = new Point(12, 70);
            dataGridViewColumns.Name = "dataGridViewColumns";
            dataGridViewColumns.Size = new Size(660, 300);
            dataGridViewColumns.TabIndex = 3;
            //
            // buttonMoveUp
            //
            buttonMoveUp.Location = new Point(678, 70);
            buttonMoveUp.Name = "buttonMoveUp";
            buttonMoveUp.Size = new Size(75, 30);
            buttonMoveUp.TabIndex = 4;
            buttonMoveUp.Text = "Move Up";
            buttonMoveUp.UseVisualStyleBackColor = true;
            buttonMoveUp.Click += buttonMoveUp_Click;
            //
            // buttonMoveDown
            //
            buttonMoveDown.Location = new Point(678, 106);
            buttonMoveDown.Name = "buttonMoveDown";
            buttonMoveDown.Size = new Size(75, 30);
            buttonMoveDown.TabIndex = 5;
            buttonMoveDown.Text = "Move Down";
            buttonMoveDown.UseVisualStyleBackColor = true;
            buttonMoveDown.Click += buttonMoveDown_Click;
            //
            // buttonOK
            //
            buttonOK.Location = new Point(597, 385);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 6;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += buttonOK_Click;
            //
            // buttonCancel
            //
            buttonCancel.Location = new Point(678, 385);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 7;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            //
            // TableEditorDialog
            //
            AcceptButton = buttonOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new Size(765, 420);
            Controls.Add(buttonMoveDown);
            Controls.Add(buttonMoveUp);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOK);
            Controls.Add(dataGridViewColumns);
            Controls.Add(labelColumns);
            Controls.Add(textBoxTableName);
            Controls.Add(labelTableName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TableEditorDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Table Editor";
            ((System.ComponentModel.ISupportInitialize)dataGridViewColumns).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTableName;
        private TextBox textBoxTableName;
        private Label labelColumns;
        private DataGridView dataGridViewColumns;
        private Button buttonMoveUp;
        private Button buttonMoveDown;
        private Button buttonOK;
        private Button buttonCancel;
    }
}
