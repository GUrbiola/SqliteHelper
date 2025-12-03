using System.Drawing;
using System.Windows.Forms;

namespace SqliteHelper48
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
            this.components = new System.ComponentModel.Container();
            this.labelTableName = new System.Windows.Forms.Label();
            this.textBoxTableName = new System.Windows.Forms.TextBox();
            this.labelColumns = new System.Windows.Forms.Label();
            this.dataGridViewColumns = new System.Windows.Forms.DataGridView();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.themeManager1 = new ZidUtilities.CommonCode.Win.Controls.ThemeManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewColumns)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTableName
            // 
            this.labelTableName.AutoSize = true;
            this.labelTableName.Location = new System.Drawing.Point(10, 13);
            this.labelTableName.Name = "labelTableName";
            this.labelTableName.Size = new System.Drawing.Size(68, 13);
            this.labelTableName.TabIndex = 0;
            this.labelTableName.Text = "Table Name:";
            // 
            // textBoxTableName
            // 
            this.textBoxTableName.Location = new System.Drawing.Point(78, 10);
            this.textBoxTableName.Name = "textBoxTableName";
            this.textBoxTableName.Size = new System.Drawing.Size(499, 20);
            this.textBoxTableName.TabIndex = 1;
            // 
            // labelColumns
            // 
            this.labelColumns.AutoSize = true;
            this.labelColumns.Location = new System.Drawing.Point(10, 43);
            this.labelColumns.Name = "labelColumns";
            this.labelColumns.Size = new System.Drawing.Size(50, 13);
            this.labelColumns.TabIndex = 2;
            this.labelColumns.Text = "Columns:";
            // 
            // dataGridViewColumns
            // 
            this.dataGridViewColumns.AllowUserToOrderColumns = true;
            this.dataGridViewColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewColumns.Location = new System.Drawing.Point(10, 61);
            this.dataGridViewColumns.Name = "dataGridViewColumns";
            this.dataGridViewColumns.Size = new System.Drawing.Size(566, 260);
            this.dataGridViewColumns.TabIndex = 3;
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Location = new System.Drawing.Point(581, 61);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(64, 26);
            this.buttonMoveUp.TabIndex = 4;
            this.buttonMoveUp.Text = "Move Up";
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Location = new System.Drawing.Point(581, 92);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(64, 26);
            this.buttonMoveDown.TabIndex = 5;
            this.buttonMoveDown.Text = "Move Down";
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(512, 334);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(64, 20);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(581, 334);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(64, 20);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // themeManager1
            // 
            this.themeManager1.ParentForm = this;
            // 
            // TableEditorDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(656, 364);
            this.Controls.Add(this.buttonMoveDown);
            this.Controls.Add(this.buttonMoveUp);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.dataGridViewColumns);
            this.Controls.Add(this.labelColumns);
            this.Controls.Add(this.textBoxTableName);
            this.Controls.Add(this.labelTableName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TableEditorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Table Editor";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewColumns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private ZidUtilities.CommonCode.Win.Controls.ThemeManager themeManager1;
    }
}
