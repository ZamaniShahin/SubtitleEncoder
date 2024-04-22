using System;
using System.Drawing;
using System.Windows.Forms;

namespace SubtitleEncoder
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button selectFolderButton;
        private System.Windows.Forms.Panel dragDropPanel;
        private System.Windows.Forms.Label dragDropLabel;
        private System.Windows.Forms.PictureBox logoPictureBox;

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
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Subtitle Encoder";
            this.BackColor = Color.FromArgb(240, 240, 240);


            // Adding a panel for drag and drop
            this.dragDropPanel = new System.Windows.Forms.Panel();
            this.dragDropPanel.Location = new System.Drawing.Point(150, 100);
            this.dragDropPanel.Size = new System.Drawing.Size(500, 200);
            this.dragDropPanel.BackColor = Color.White;
            this.dragDropPanel.BorderStyle = BorderStyle.FixedSingle; // Add border style
            this.dragDropPanel.Padding = new Padding(10);
            this.dragDropPanel.AllowDrop = true;
            this.dragDropPanel.DragEnter += new DragEventHandler(dragDropPanel_DragEnter);
            this.dragDropPanel.DragOver += new DragEventHandler(dragDropPanel_DragOver);
            this.dragDropPanel.DragDrop += new DragEventHandler(dragDropPanel_DragDrop);
            this.Controls.Add(this.dragDropPanel);

            // Adding a label for drag and drop
            this.dragDropLabel = new System.Windows.Forms.Label();
            this.dragDropLabel.Dock = DockStyle.Fill;
            this.dragDropLabel.Text = "Drag and drop folder here";
            this.dragDropLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.dragDropLabel.Font = new Font("Segoe UI", 14, FontStyle.Regular);
            this.dragDropLabel.ForeColor = Color.FromArgb(64, 64, 64);
            this.dragDropPanel.Controls.Add(this.dragDropLabel);

            // Adding a button
            this.selectFolderButton = new System.Windows.Forms.Button();
            this.selectFolderButton.Location = new System.Drawing.Point(300, 350);
            this.selectFolderButton.Size = new System.Drawing.Size(200, 40);
            this.selectFolderButton.Text = "Select Folder";
            this.selectFolderButton.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            this.selectFolderButton.BackColor = Color.FromArgb(0, 123, 255);
            this.selectFolderButton.ForeColor = Color.White;
            this.selectFolderButton.FlatStyle = FlatStyle.Flat;
            this.selectFolderButton.FlatAppearance.BorderSize = 0;
            this.selectFolderButton.Click += new System.EventHandler(this.selectFolderButton_Click);
            this.Controls.Add(this.selectFolderButton);
        }

        #endregion

        private async Task<string> PromptSaveOption()
{
    // Show a message box with the options
    DialogResult result = MessageBox.Show("How do you want to save the files?\n\n1. Save all files in one folder (Fixed folder)\n2. Save files with exact subfolders (Fixed folder with subfolders)",
        "Choose Saving Option", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

    // Determine the selected option
    if (result == DialogResult.Yes)
    {
        return "one-folder";
    }
    else
    {
        return "with-subfolders";
    }
}

        private async void selectFolderButton_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                // Show the dialog to select the input folder
                DialogResult result = folderDialog.ShowDialog();

                // If the user selects a folder
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    string inputFolder = folderDialog.SelectedPath;

                    // Prompt the user to choose the save option
                    using (var saveOptionDialog = new SaveOptionDialog())
                    {
                        DialogResult saveOptionResult = saveOptionDialog.ShowDialog();

                        if (saveOptionResult == DialogResult.OK)
                        {
                            bool saveInFixedFolder = saveOptionDialog.SaveInFixedFolder;

                            // Get the parent directory of the input folder
                            // string outputFolder = saveInFixedFolder ? Path.Combine(GetParentDirectory(inputFolder), "FixedSubtitles") : GetParentDirectory(inputFolder);
                            string outputFolder = Path.Combine(GetParentDirectory(inputFolder), "FixedSubtitles");

                            try
                            {
                                await EncodeLogic.EncodeIntoUtf8(inputFolder, outputFolder, saveInFixedFolder);
                                MessageBox.Show("The Subtitles are Written in this path:\n " + outputFolder);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error Happened Dude.");
                                // Optionally, you can show an error message to the user
                                MessageBox.Show("An error occurred during encoding: " + ex.Message);
                            }
                        }
                    }
                }
            }
        }



        private string GetParentDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            // Get the directory information for the specified path
            string parentDirectory = Path.GetDirectoryName(path);

            // If the parent directory is null or empty, return the original path
            if (string.IsNullOrEmpty(parentDirectory))
            {
                return path;
            }

            // Return the parent directory
            return parentDirectory;
        }


        private void dragDropPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                // Change panel's background color to indicate hover
                dragDropPanel.BackColor = Color.FromArgb(200, 200, 200);
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void dragDropPanel_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void dragDropPanel_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                string folderPath = files[0];
                MessageBox.Show("You dropped folder: " + folderPath);
            }
            // Restore panel's background color after drop
            dragDropPanel.BackColor = Color.White;
        }
    }
}
