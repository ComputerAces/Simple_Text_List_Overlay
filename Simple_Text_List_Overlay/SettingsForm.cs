using System;
using System.Drawing; // Required for using Color
using System.Windows.Forms;

// Ensure this namespace matches your project name
namespace Simple_Text_List_Overlay {
    public partial class SettingsForm : Form {
        public SettingsForm()
        {
            InitializeComponent();

            // Optional: Set initial colors for panels if not done in designer
            // These will be overwritten by loaded settings anyway in Form_Load
            // panelOutlineColor.BackColor = Properties.Settings.Default.OutlineColor;
            // panelTextColor.BackColor = Properties.Settings.Default.TextColor;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // Load all settings from Properties.Settings.Default into the controls
            txtFilePath.Text = Properties.Settings.Default.TipsFilePath;
            chkUseOutline.Checked = Properties.Settings.Default.UseOutline;
            panelOutlineColor.BackColor = Properties.Settings.Default.OutlineColor;
            panelTextColor.BackColor = Properties.Settings.Default.TextColor;
            chkRandomOrder.Checked = Properties.Settings.Default.RandomOrder; // Load RandomOrder setting

            // Ensure the value is within the NumericUpDown's bounds before setting
            int savedTime = Properties.Settings.Default.SwitchTimeSeconds;
            if (savedTime >= numSwitchTime.Minimum && savedTime <= numSwitchTime.Maximum)
            {
                numSwitchTime.Value = savedTime;
            }
            else
            {
                // Set to default or nearest bound if saved value is invalid
                numSwitchTime.Value = Math.Max(numSwitchTime.Minimum, Math.Min(numSwitchTime.Maximum, 30)); // Default 30
            }

            // Load Font setting and update preview label
            try
            {
                // Use a fallback if the setting is null
                Font loadedFont = Properties.Settings.Default.TextFont ?? new Font("Arial Black", 16, FontStyle.Bold);
                lblFontPreview.Font = loadedFont;
                // Also update the font dialog's initial font
                fontDialog1.Font = loadedFont;
            }
            catch // Catch errors during font loading/applying
            {
                lblFontPreview.Font = new Font("Arial Black", 16, FontStyle.Bold); // Fallback font
                fontDialog1.Font = lblFontPreview.Font;
                MessageBox.Show("Could not load the saved font setting. Using default.", "Font Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            lblFontPreview.ForeColor = Properties.Settings.Default.TextColor; // Update preview color too
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // Set initial directory if a path already exists
            if (!string.IsNullOrEmpty(txtFilePath.Text))
            {
                try
                {
                    openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(txtFilePath.Text);
                }
                catch { /* Ignore invalid path errors for InitialDirectory */ }
            }

            // Show the dialog
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Update the text box with the selected file path
                txtFilePath.Text = openFileDialog1.FileName;
            }
        }

        // Event handler for the Outline Color Button
        private void btnOutlineColor_Click(object sender, EventArgs e)
        {
            // Set the dialog's current color to the panel's color
            colorDialog1.Color = panelOutlineColor.BackColor;

            // Show the color dialog
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                // Update the panel's background color with the selected color
                panelOutlineColor.BackColor = colorDialog1.Color;
            }
        }

        // Event handler for the Text Color Button
        // *** Assumes your button name is btnTextColor, change if different ***
        private void btnTextColor_Click(object sender, EventArgs e)
        {
            // Set the dialog's current color to the panel's color
            colorDialog1.Color = panelTextColor.BackColor;

            // Show the color dialog
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                // Update the panel's background color with the selected color
                panelTextColor.BackColor = colorDialog1.Color;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Optional validation could go here

            // Save all settings from controls back to Properties.Settings.Default
            Properties.Settings.Default.TipsFilePath = txtFilePath.Text;
            Properties.Settings.Default.UseOutline = chkUseOutline.Checked;
            Properties.Settings.Default.OutlineColor = panelOutlineColor.BackColor;
            Properties.Settings.Default.TextColor = panelTextColor.BackColor;
            Properties.Settings.Default.SwitchTimeSeconds = (int)numSwitchTime.Value;
            Properties.Settings.Default.TextFont = lblFontPreview.Font; // Save font from preview label (updated by font dialog)
            Properties.Settings.Default.RandomOrder = chkRandomOrder.Checked; // Save RandomOrder setting

            // Persist the changes
            Properties.Settings.Default.Save();

            // Indicate success and close the form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSelectFont_Click(object sender, EventArgs e)
        {
            // Initialize the FontDialog with the current font from the preview label
            fontDialog1.Font = lblFontPreview.Font;
            // Optional: Set color in dialog (though we use separate controls)
            // fontDialog1.Color = lblFontPreview.ForeColor;

            // Show the dialog
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                // Update the preview label's font
                lblFontPreview.Font = fontDialog1.Font;
                // Update the preview label's color (optional, if you want font dialog to control it)
                // lblFontPreview.ForeColor = fontDialog1.Color;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Indicate cancellation and close the form
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}