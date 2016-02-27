using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace PscdPack
{
    public partial class MainForm : Form
    {
        PscdFormat pak;
        bool changed;

        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        void MainFormLoad(object sender, EventArgs e)
        {
            extraSaveModeComboBox.DataSource = Enum.GetValues(typeof(ExtraSaveMode));
            regionComboBox.DataSource = Enum.GetValues(typeof(RomRegion));
        }

        void clearInterface()
        {
            nameTextBox.Text = string.Empty;
            romSizeLabel.Text = string.Empty;
            extraPageMaskTextBox.Text = string.Empty;
            extraSizeMaskTextbox.Text = string.Empty;
            thumbPictureBox.Image = null;
            saveButton.Enabled = false;
            closeButton.Enabled = false;
            dumpRomButton.Enabled = false;
            replaceRomButton.Enabled = false;
            extractThumbButton.Enabled = false;
            replaceThumbButton.Enabled = false;
            removeThumbButton.Enabled = false;
        }

        void enableInterface()
        {
            saveButton.Enabled = true;
            closeButton.Enabled = true;
            replaceRomButton.Enabled = true;
        }

        bool askChanged()
        {
            if (changed)
            {
                switch (MessageBox.Show(this, "The pack has been modified. Do you want to save the changes?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        changed = !savePak(false);
                        return !changed;
                    case DialogResult.No:
                        changed = false;
                        return true;
                    case DialogResult.Cancel:
                        return false;
                }
            }

            return true;
        }

        void closePak()
        {
            if (!askChanged()) return;
            if (pak != null) pak.Dispose();
            clearInterface();
        }

        bool savePak(bool verbose = true)
        {
            try
            {
                // Get the external memory settings
                pak.ExtraSaveMode = (ExtraSaveMode)extraSaveModeComboBox.SelectedIndex;
                ushort pm;
                if (!ushort.TryParse(extraPageMaskTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out pm))
                {
                    MessageBox.Show(this, "Cannot parse page mask as a 16-bit hexadecimal number.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                ushort sm;
                if (!ushort.TryParse(extraPageMaskTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out sm))
                {
                    MessageBox.Show(this, "Cannot parse size mask as a 16-bit hexadecimal number.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                pak.ExtraSavePageMask = pm;
                pak.ExtraSaveSizeMask = sm;
                pak.Name = nameTextBox.Text;
                pak.Region = (RomRegion)regionComboBox.SelectedIndex;
                pak.Save();
                if (verbose)
                {
                    MessageBox.Show(this, "Pack saved.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error saving pack: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        void loadThumbnailFromPak()
        {
            using (var imageStream = pak.GetThumbnail())
            {
                if (imageStream.Length > 0)
                {
                    extractThumbButton.Enabled = true;
                    removeThumbButton.Enabled = true;
                    try
                    {
                        Image bmp = Image.FromStream(imageStream);
                        thumbPictureBox.Image = bmp;
                    }
                    catch
                    {
                        thumbPictureBox.Image = null;
                    }
                }
                else
                {
                    thumbPictureBox.Image = null;
                }
            }
        }

        void OpenButtonClick(object sender, EventArgs e)
        {
            try
            {
                closePak();
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    pak = new PscdFormat(saveFileDialog.FileName, PscdFormat.ClassicsKeyBytes);
                    changed = false;
                    if (pak.FileHasContent)
                    {
                        pak.Load();
                        dumpRomButton.Enabled = true;
                        replaceThumbButton.Enabled = true;
                        romSizeLabel.Text = (pak.ImageSize / 1024) + " KB";
                        loadThumbnailFromPak();
                    }
                    else
                    {
                        romSizeLabel.Text = "Not loaded";
                    }
                    nameTextBox.Text = pak.Name;
                    if ((int)pak.ExtraSaveMode < extraSaveModeComboBox.Items.Count) extraSaveModeComboBox.SelectedIndex = (int)pak.ExtraSaveMode;
                    extraPageMaskTextBox.Text = pak.ExtraSavePageMask.ToString("X4");
                    extraSizeMaskTextbox.Text = pak.ExtraSaveSizeMask.ToString("X4");
                    if ((int)pak.Region < regionComboBox.Items.Count) regionComboBox.SelectedIndex = (int)pak.Region;
                    enableInterface();
                }
            }
            catch (Exception ex)
            {
                if (pak != null) pak.Dispose();
                MessageBox.Show(this, "Error opening pack: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void SaveButtonClick(object sender, EventArgs e)
        {
            changed = !savePak();
        }

        void CloseButtonClick(object sender, EventArgs e)
        {
            closePak();
        }

        void DumpRomButtonClick(object sender, EventArgs e)
        {
            try
            {
                extractFileDialog.Title = "Save ROM Image";
                extractFileDialog.FileName = System.IO.Path.ChangeExtension(saveFileDialog.FileName, ".bin");
                if (extractFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    using (var file = extractFileDialog.OpenFile())
                    using (var rom = pak.GetImage())
                    {
                        rom.WriteTo(file);
                        file.Flush();
                    }
                    MessageBox.Show(this, "ROM extracted.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error extracting ROM: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void ExtractThumbButtonClick(object sender, EventArgs e)
        {
            try
            {
                extractFileDialog.Title = "Save Thumbnail";
                // I assume this is actually an image, and is a BMP
                extractFileDialog.FileName = Path.ChangeExtension(saveFileDialog.FileName, ".bmp");
                if (extractFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    using (var file = extractFileDialog.OpenFile())
                    using (var rom = pak.GetThumbnail())
                    {
                        rom.WriteTo(file);
                        file.Flush();
                    }
                    MessageBox.Show(this, "Thumbnail extracted.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error extracting thumbnail: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void RemoveThumbButtonClick(object sender, EventArgs e)
        {
            changed = true;
            thumbPictureBox.Image = null;
            pak.SetThumbnail(null);
            extractThumbButton.Enabled = false;
            removeThumbButton.Enabled = false;
        }

        void ExitButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel |= !askChanged();
        }

        void ReplaceRomButtonClick(object sender, EventArgs e)
        {
            try
            {
                replaceFileDialog.Title = "Replace ROM";
                if (replaceFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    changed = true;
                    byte[] rom = File.ReadAllBytes(replaceFileDialog.FileName);
                    pak.SetImage(rom);
                    romSizeLabel.Text = (pak.ImageSize / 1024) + " KB";
                    dumpRomButton.Enabled = true;

                    // Attempt to auto-load some info from ROM
                    try
                    {
                        using (var ms = new MemoryStream(rom))
                        {
                            var br = new BinaryReader(ms);

                            // Game name
                            ms.Seek(0x120, SeekOrigin.Begin);
                            nameTextBox.Text = new string(br.ReadChars(0x30)).TrimEnd(' ');

                            // SRAM configuration
                            ms.Seek(0x1b0, SeekOrigin.Begin);
                            if (br.ReadChar() == 'R' && br.ReadChar() == 'A')
                            {
                                byte x = br.ReadByte();
                                byte y = br.ReadByte();
                                uint ramStart = (uint)((br.ReadByte() << 24) | (br.ReadByte() << 16) | (br.ReadByte() << 8) | br.ReadByte());
                                uint ramEnd = (uint)((br.ReadByte() << 24) | (br.ReadByte() << 16) | (br.ReadByte() << 8) | br.ReadByte());
                                ExtraSaveMode mode = ExtraSaveMode.None;
                                ushort ramStartPacked = (ushort)((ramStart >> 0x0c) & 0x3fff);
                                if (y == 0x20)
                                {
                                    uint ramSize = ramEnd - ramStart;
                                    mode = ExtraSaveMode.Sram;
                                    extraPageMaskTextBox.Text = ramStartPacked.ToString("X4");
                                    extraSizeMaskTextbox.Text = (((ramSize + 0xff) >> 8) & 0xffff).ToString("X4");
                                }
                                else
                                {
                                    // Probably EEPROM
                                    mode = (ExtraSaveMode)((y >> 6) + 1); // I'm spitballing here
                                    extraPageMaskTextBox.Text = ramStartPacked.ToString("X4");
                                    extraSizeMaskTextbox.Text = (ramStart & 0xffff).ToString("X4");
                                }
                                extraSaveModeComboBox.SelectedIndex = (int)mode;
                            }
                            else
                            {
                                extraSaveModeComboBox.SelectedIndex = (int)ExtraSaveMode.None;
                            }

                            // Game region
                            ms.Seek(0x1c0, SeekOrigin.Begin);
                            string regions = new string(br.ReadChars(0x10)).TrimEnd(' ');
                            int rflags = 0;
                            if (regions.Contains("U")) rflags |= 1 << 0;
                            if (regions.Contains("J")) rflags |= 1 << 1;
                            if (regions.Contains("E")) rflags |= 1 << 2;
                            RomRegion renum = RomRegion.Worldwide;
                            if (rflags == (rflags & ~rflags)) // Only one flag present 
                            {
                                switch (rflags)
                                {
                                    case 1 << 0:
                                        renum = RomRegion.Usa;
                                        break;
                                    case 1 << 1:
                                        renum = RomRegion.Japan;
                                        break;
                                    case 1 << 2:
                                        renum = RomRegion.Europe;
                                        break;
                                }
                            }
                            regionComboBox.SelectedIndex = (int)renum;
                        }
                    }
                    catch { }

                    replaceThumbButton.Enabled = true;
                    MessageBox.Show(this, "ROM replaced.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error replacing ROM: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void ReplaceThumbButtonClick(object sender, EventArgs e)
        {
            try
            {
                replaceFileDialog.Title = "Replace Thumbnail";
                if (replaceFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    changed = true;
                    byte[] rom = File.ReadAllBytes(replaceFileDialog.FileName);
                    pak.SetThumbnail(rom);
                    loadThumbnailFromPak();
                    MessageBox.Show(this, "Thumbnail replaced.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error replacing thumbnail: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
