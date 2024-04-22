using System;
using System.Windows.Forms;

namespace SubtitleEncoder
{
    public partial class SaveOptionDialog : Form
    {
        public bool SaveInFixedFolder { get; private set; }

        public SaveOptionDialog()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            // Set the SaveInFixedFolder property based on the checked state of the radio buttons
            SaveInFixedFolder = fixedFolderRadioButton.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}