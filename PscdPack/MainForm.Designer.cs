namespace PscdPack
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox extraSaveModeComboBox;
		private System.Windows.Forms.TextBox extraPageMaskTextBox;
		private System.Windows.Forms.Label romSizeLabel;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox regionComboBox;
		private System.Windows.Forms.TextBox extraSizeMaskTextbox;
		private System.Windows.Forms.PictureBox thumbPictureBox;
		private System.Windows.Forms.Button openButton;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Button dumpRomButton;
		private System.Windows.Forms.Button replaceRomButton;
		private System.Windows.Forms.Button extractThumbButton;
		private System.Windows.Forms.Button replaceThumbButton;
		private System.Windows.Forms.Button removeThumbButton;
		private System.Windows.Forms.Button exitButton;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.SaveFileDialog extractFileDialog;
		private System.Windows.Forms.OpenFileDialog replaceFileDialog;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.thumbPictureBox = new System.Windows.Forms.PictureBox();
            this.regionComboBox = new System.Windows.Forms.ComboBox();
            this.extraSizeMaskTextbox = new System.Windows.Forms.TextBox();
            this.extraSaveModeComboBox = new System.Windows.Forms.ComboBox();
            this.extraPageMaskTextBox = new System.Windows.Forms.TextBox();
            this.romSizeLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.openButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.dumpRomButton = new System.Windows.Forms.Button();
            this.replaceRomButton = new System.Windows.Forms.Button();
            this.extractThumbButton = new System.Windows.Forms.Button();
            this.replaceThumbButton = new System.Windows.Forms.Button();
            this.removeThumbButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.extractFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.replaceFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.thumbPictureBox);
            this.groupBox1.Controls.Add(this.regionComboBox);
            this.groupBox1.Controls.Add(this.extraSizeMaskTextbox);
            this.groupBox1.Controls.Add(this.extraSaveModeComboBox);
            this.groupBox1.Controls.Add(this.extraPageMaskTextBox);
            this.groupBox1.Controls.Add(this.romSizeLabel);
            this.groupBox1.Controls.Add(this.nameTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 191);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ROM Info";
            // 
            // thumbPictureBox
            // 
            this.thumbPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.thumbPictureBox.Location = new System.Drawing.Point(50, 125);
            this.thumbPictureBox.Name = "thumbPictureBox";
            this.thumbPictureBox.Size = new System.Drawing.Size(160, 60);
            this.thumbPictureBox.TabIndex = 10;
            this.thumbPictureBox.TabStop = false;
            // 
            // regionComboBox
            // 
            this.regionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.regionComboBox.FormattingEnabled = true;
            this.regionComboBox.Location = new System.Drawing.Point(70, 98);
            this.regionComboBox.Name = "regionComboBox";
            this.regionComboBox.Size = new System.Drawing.Size(184, 21);
            this.regionComboBox.TabIndex = 9;
            // 
            // extraSizeMaskTextbox
            // 
            this.extraSizeMaskTextbox.Location = new System.Drawing.Point(202, 71);
            this.extraSizeMaskTextbox.Name = "extraSizeMaskTextbox";
            this.extraSizeMaskTextbox.Size = new System.Drawing.Size(52, 20);
            this.extraSizeMaskTextbox.TabIndex = 8;
            // 
            // extraSaveModeComboBox
            // 
            this.extraSaveModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.extraSaveModeComboBox.FormattingEnabled = true;
            this.extraSaveModeComboBox.Location = new System.Drawing.Point(6, 71);
            this.extraSaveModeComboBox.Name = "extraSaveModeComboBox";
            this.extraSaveModeComboBox.Size = new System.Drawing.Size(132, 21);
            this.extraSaveModeComboBox.TabIndex = 7;
            // 
            // extraPageMaskTextBox
            // 
            this.extraPageMaskTextBox.Location = new System.Drawing.Point(144, 71);
            this.extraPageMaskTextBox.Name = "extraPageMaskTextBox";
            this.extraPageMaskTextBox.Size = new System.Drawing.Size(52, 20);
            this.extraPageMaskTextBox.TabIndex = 6;
            // 
            // romSizeLabel
            // 
            this.romSizeLabel.Location = new System.Drawing.Point(70, 42);
            this.romSizeLabel.Name = "romSizeLabel";
            this.romSizeLabel.Size = new System.Drawing.Size(184, 13);
            this.romSizeLabel.TabIndex = 5;
            this.romSizeLabel.Text = "Not loaded";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(70, 19);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(184, 20);
            this.nameTextBox.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Region:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "On-cartridge Memory Config:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "ROM Size:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(12, 209);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(82, 23);
            this.openButton.TabIndex = 1;
            this.openButton.Text = "New/Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.OpenButtonClick);
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(100, 209);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(84, 23);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // closeButton
            // 
            this.closeButton.Enabled = false;
            this.closeButton.Location = new System.Drawing.Point(190, 209);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(82, 23);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButtonClick);
            // 
            // dumpRomButton
            // 
            this.dumpRomButton.Enabled = false;
            this.dumpRomButton.Location = new System.Drawing.Point(12, 238);
            this.dumpRomButton.Name = "dumpRomButton";
            this.dumpRomButton.Size = new System.Drawing.Size(127, 23);
            this.dumpRomButton.TabIndex = 4;
            this.dumpRomButton.Text = "Extract ROM";
            this.dumpRomButton.UseVisualStyleBackColor = true;
            this.dumpRomButton.Click += new System.EventHandler(this.DumpRomButtonClick);
            // 
            // replaceRomButton
            // 
            this.replaceRomButton.Enabled = false;
            this.replaceRomButton.Location = new System.Drawing.Point(145, 238);
            this.replaceRomButton.Name = "replaceRomButton";
            this.replaceRomButton.Size = new System.Drawing.Size(127, 23);
            this.replaceRomButton.TabIndex = 5;
            this.replaceRomButton.Text = "Replace ROM";
            this.replaceRomButton.UseVisualStyleBackColor = true;
            this.replaceRomButton.Click += new System.EventHandler(this.ReplaceRomButtonClick);
            // 
            // extractThumbButton
            // 
            this.extractThumbButton.Enabled = false;
            this.extractThumbButton.Location = new System.Drawing.Point(12, 267);
            this.extractThumbButton.Name = "extractThumbButton";
            this.extractThumbButton.Size = new System.Drawing.Size(127, 23);
            this.extractThumbButton.TabIndex = 6;
            this.extractThumbButton.Text = "Extract Thumb";
            this.extractThumbButton.UseVisualStyleBackColor = true;
            this.extractThumbButton.Click += new System.EventHandler(this.ExtractThumbButtonClick);
            // 
            // replaceThumbButton
            // 
            this.replaceThumbButton.Enabled = false;
            this.replaceThumbButton.Location = new System.Drawing.Point(145, 267);
            this.replaceThumbButton.Name = "replaceThumbButton";
            this.replaceThumbButton.Size = new System.Drawing.Size(127, 23);
            this.replaceThumbButton.TabIndex = 7;
            this.replaceThumbButton.Text = "Replace Thumb";
            this.replaceThumbButton.UseVisualStyleBackColor = true;
            this.replaceThumbButton.Click += new System.EventHandler(this.ReplaceThumbButtonClick);
            // 
            // removeThumbButton
            // 
            this.removeThumbButton.Enabled = false;
            this.removeThumbButton.Location = new System.Drawing.Point(12, 296);
            this.removeThumbButton.Name = "removeThumbButton";
            this.removeThumbButton.Size = new System.Drawing.Size(127, 23);
            this.removeThumbButton.TabIndex = 8;
            this.removeThumbButton.Text = "Remove Thumb";
            this.removeThumbButton.UseVisualStyleBackColor = true;
            this.removeThumbButton.Click += new System.EventHandler(this.RemoveThumbButtonClick);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(145, 296);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(127, 23);
            this.exitButton.TabIndex = 9;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButtonClick);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "pak";
            this.saveFileDialog.Filter = "Sega Genesis Classics ROM Pack|*.pak";
            this.saveFileDialog.OverwritePrompt = false;
            this.saveFileDialog.Title = "Open or Create ROM Pack";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 331);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.removeThumbButton);
            this.Controls.Add(this.replaceThumbButton);
            this.Controls.Add(this.extractThumbButton);
            this.Controls.Add(this.replaceRomButton);
            this.Controls.Add(this.dumpRomButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "SEGA Genesis Classics ROM Packer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbPictureBox)).EndInit();
            this.ResumeLayout(false);

		}
	}
}
