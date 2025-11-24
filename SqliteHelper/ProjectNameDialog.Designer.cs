namespace SqliteHelper
{
    partial class ProjectNameDialog
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
            labelPrompt = new Label();
            textBoxProjectName = new TextBox();
            buttonOK = new Button();
            buttonCancel = new Button();
            SuspendLayout();
            //
            // labelPrompt
            //
            labelPrompt.AutoSize = true;
            labelPrompt.Location = new Point(12, 15);
            labelPrompt.Name = "labelPrompt";
            labelPrompt.Size = new Size(85, 15);
            labelPrompt.TabIndex = 0;
            labelPrompt.Text = "Project Name:";
            //
            // textBoxProjectName
            //
            textBoxProjectName.Location = new Point(12, 35);
            textBoxProjectName.Name = "textBoxProjectName";
            textBoxProjectName.Size = new Size(360, 23);
            textBoxProjectName.TabIndex = 1;
            //
            // buttonOK
            //
            buttonOK.Location = new Point(216, 70);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 2;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += buttonOK_Click;
            //
            // buttonCancel
            //
            buttonCancel.Location = new Point(297, 70);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            //
            // ProjectNameDialog
            //
            AcceptButton = buttonOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new Size(384, 105);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOK);
            Controls.Add(textBoxProjectName);
            Controls.Add(labelPrompt);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProjectNameDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "New Project";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelPrompt;
        private TextBox textBoxProjectName;
        private Button buttonOK;
        private Button buttonCancel;
    }
}
