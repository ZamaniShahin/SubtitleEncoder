using System.ComponentModel;

namespace SubtitleEncoder
{
    partial class SaveOptionDialog
    {
        private IContainer components = null;
        private RadioButton fixedFolderRadioButton;
        private RadioButton subfoldersRadioButton;
        private Button okButton;
        private Button cancelButton;

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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Text = "Save Options";

            // Define a RadioButton control for saving in the fixed folder
            this.fixedFolderRadioButton = new System.Windows.Forms.RadioButton();
            this.fixedFolderRadioButton.AutoSize = true;
            this.fixedFolderRadioButton.Location = new System.Drawing.Point(30, 30);
            this.fixedFolderRadioButton.Name = "fixedFolderRadioButton";
            this.fixedFolderRadioButton.Size = new System.Drawing.Size(155, 21);
            this.fixedFolderRadioButton.TabIndex = 0;
            this.fixedFolderRadioButton.TabStop = true;
            this.fixedFolderRadioButton.Text = "Save All Files In One Folder";
            this.fixedFolderRadioButton.UseVisualStyleBackColor = true;
            this.Controls.Add(this.fixedFolderRadioButton);

            // Define a RadioButton control for saving with subfolders
            this.subfoldersRadioButton = new System.Windows.Forms.RadioButton();
            this.subfoldersRadioButton.AutoSize = true;
            this.subfoldersRadioButton.Location = new System.Drawing.Point(30, 60);
            this.subfoldersRadioButton.Name = "subfoldersRadioButton";
            this.subfoldersRadioButton.Size = new System.Drawing.Size(145, 21);
            this.subfoldersRadioButton.TabIndex = 1;
            this.subfoldersRadioButton.TabStop = true;
            this.subfoldersRadioButton.Text = "Save With Subfolders";
            this.subfoldersRadioButton.UseVisualStyleBackColor = true;
            this.Controls.Add(this.subfoldersRadioButton);

            // Define OK button
            this.okButton = new System.Windows.Forms.Button();
            this.okButton.Location = new System.Drawing.Point(50, 120);
            this.okButton.Size = new System.Drawing.Size(75, 25);
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            this.Controls.Add(this.okButton);

            // Define Cancel button
            this.cancelButton = new System.Windows.Forms.Button();
            this.cancelButton.Location = new System.Drawing.Point(150, 120);
            this.cancelButton.Size = new System.Drawing.Size(75, 25);
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            this.Controls.Add(this.cancelButton);
        }

        #endregion
    }
}
