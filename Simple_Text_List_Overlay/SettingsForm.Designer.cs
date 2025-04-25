
namespace Simple_Text_List_Overlay {
    partial class SettingsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.lblFilePathLabel = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.chkUseOutline = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelOutlineColor = new System.Windows.Forms.Panel();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btnOutlineColor = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnTextColor = new System.Windows.Forms.Button();
            this.panelTextColor = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.numSwitchTime = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.lblFontPreview = new System.Windows.Forms.Label();
            this.btnSelectFont = new System.Windows.Forms.Button();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.chkRandomOrder = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numSwitchTime)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFilePathLabel
            // 
            this.lblFilePathLabel.Location = new System.Drawing.Point(12, 9);
            this.lblFilePathLabel.Name = "lblFilePathLabel";
            this.lblFilePathLabel.Size = new System.Drawing.Size(139, 23);
            this.lblFilePathLabel.TabIndex = 0;
            this.lblFilePathLabel.Text = "Tips File Path :";
            this.lblFilePathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilePath.Location = new System.Drawing.Point(157, 11);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(255, 20);
            this.txtFilePath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(418, 9);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(47, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(390, 197);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(15, 197);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            this.openFileDialog1.Title = "Select Tips Text File";
            // 
            // chkUseOutline
            // 
            this.chkUseOutline.AutoSize = true;
            this.chkUseOutline.Location = new System.Drawing.Point(46, 48);
            this.chkUseOutline.Name = "chkUseOutline";
            this.chkUseOutline.Size = new System.Drawing.Size(105, 17);
            this.chkUseOutline.TabIndex = 5;
            this.chkUseOutline.Text = "Use Text Outline";
            this.chkUseOutline.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(154, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Outline Color :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelOutlineColor
            // 
            this.panelOutlineColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOutlineColor.Location = new System.Drawing.Point(260, 46);
            this.panelOutlineColor.Name = "panelOutlineColor";
            this.panelOutlineColor.Size = new System.Drawing.Size(24, 24);
            this.panelOutlineColor.TabIndex = 7;
            // 
            // btnOutlineColor
            // 
            this.btnOutlineColor.Location = new System.Drawing.Point(290, 47);
            this.btnOutlineColor.Name = "btnOutlineColor";
            this.btnOutlineColor.Size = new System.Drawing.Size(47, 23);
            this.btnOutlineColor.TabIndex = 8;
            this.btnOutlineColor.Text = "...";
            this.btnOutlineColor.UseVisualStyleBackColor = true;
            this.btnOutlineColor.Click += new System.EventHandler(this.btnOutlineColor_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(154, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Text Color :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTextColor
            // 
            this.btnTextColor.Location = new System.Drawing.Point(290, 77);
            this.btnTextColor.Name = "btnTextColor";
            this.btnTextColor.Size = new System.Drawing.Size(47, 23);
            this.btnTextColor.TabIndex = 10;
            this.btnTextColor.Text = "...";
            this.btnTextColor.UseVisualStyleBackColor = true;
            this.btnTextColor.Click += new System.EventHandler(this.btnTextColor_Click);
            // 
            // panelTextColor
            // 
            this.panelTextColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTextColor.Location = new System.Drawing.Point(260, 76);
            this.panelTextColor.Name = "panelTextColor";
            this.panelTextColor.Size = new System.Drawing.Size(24, 24);
            this.panelTextColor.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 23);
            this.label3.TabIndex = 11;
            this.label3.Text = "Tip Display Time (seconds) :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numSwitchTime
            // 
            this.numSwitchTime.Location = new System.Drawing.Point(161, 110);
            this.numSwitchTime.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numSwitchTime.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numSwitchTime.Name = "numSwitchTime";
            this.numSwitchTime.Size = new System.Drawing.Size(120, 20);
            this.numSwitchTime.TabIndex = 12;
            this.numSwitchTime.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 23);
            this.label4.TabIndex = 13;
            this.label4.Text = "Tip Display Time (seconds) :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFontPreview
            // 
            this.lblFontPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFontPreview.Location = new System.Drawing.Point(161, 139);
            this.lblFontPreview.Name = "lblFontPreview";
            this.lblFontPreview.Size = new System.Drawing.Size(123, 23);
            this.lblFontPreview.TabIndex = 14;
            this.lblFontPreview.Text = "Aa Bb Cc Sample Text";
            this.lblFontPreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSelectFont
            // 
            this.btnSelectFont.Location = new System.Drawing.Point(290, 139);
            this.btnSelectFont.Name = "btnSelectFont";
            this.btnSelectFont.Size = new System.Drawing.Size(100, 23);
            this.btnSelectFont.TabIndex = 15;
            this.btnSelectFont.Text = "Select Font...";
            this.btnSelectFont.UseVisualStyleBackColor = true;
            this.btnSelectFont.Click += new System.EventHandler(this.btnSelectFont_Click);
            // 
            // chkRandomOrder
            // 
            this.chkRandomOrder.AutoSize = true;
            this.chkRandomOrder.Location = new System.Drawing.Point(15, 174);
            this.chkRandomOrder.Name = "chkRandomOrder";
            this.chkRandomOrder.Size = new System.Drawing.Size(159, 17);
            this.chkRandomOrder.TabIndex = 16;
            this.chkRandomOrder.Text = "Show Tips in Random Order";
            this.chkRandomOrder.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 229);
            this.Controls.Add(this.chkRandomOrder);
            this.Controls.Add(this.btnSelectFont);
            this.Controls.Add(this.lblFontPreview);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numSwitchTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnTextColor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panelTextColor);
            this.Controls.Add(this.btnOutlineColor);
            this.Controls.Add(this.panelOutlineColor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkUseOutline);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.lblFilePathLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numSwitchTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFilePathLabel;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox chkUseOutline;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelOutlineColor;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button btnOutlineColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnTextColor;
        private System.Windows.Forms.Panel panelTextColor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numSwitchTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblFontPreview;
        private System.Windows.Forms.Button btnSelectFont;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.CheckBox chkRandomOrder;
    }
}