using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Org.BouncyCastle.Crypto.Agreement.Srp;

namespace Tubes3_let_me_seedik
{
    public partial class Tubes : Form
    {
        private List<ControlInfo> controlInfos = new List<ControlInfo>();
        private Size initialFormSize;
        private bool isKMP = true;
        private readonly string targetFolderPath = System.IO.Path.GetFullPath("src/citra");

        public Tubes()
        {
            Console.WriteLine(targetFolderPath);
            InitializeComponent();
            labelFormat.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
            labelData.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
            // MakeButtonCircular(buttonCitra);
            // MakeButtonCircular(buttonSearch);
        }

        private void Tubes_Load(object sender, EventArgs e)
        {
            initialFormSize = this.ClientSize;
            foreach (Control control in this.Controls)
            {
                controlInfos.Add(new ControlInfo(control, control.Location, control.Size, control.Font));
            }
        }

        private void MakeButtonCircular(Button button)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, button.Width, button.Height);
            button.Region = new Region(path);
        }

        private void Tubes_Resize(object sender, EventArgs e)
        {
            if (initialFormSize.Width == 0 || initialFormSize.Height == 0) return;

            // Hitung rasio perubahan ukuran form
            float widthRatio = (float)this.ClientSize.Width / initialFormSize.Width;
            float heightRatio = (float)this.ClientSize.Height / initialFormSize.Height;
            float fontRatio = Math.Min(widthRatio, heightRatio);
            // Sesuaikan ukuran dan posisi kontrol berdasarkan rasio perubahan ukuran form
            foreach (ControlInfo controlInfo in controlInfos)
            {
                controlInfo.Control.Location = new Point((int)(controlInfo.OriginalLocation.X * widthRatio), (int)(controlInfo.OriginalLocation.Y * heightRatio));
                controlInfo.Control.Size = new Size((int)(controlInfo.OriginalSize.Width * widthRatio), (int)(controlInfo.OriginalSize.Height * heightRatio));
                // Sesuaikan ukuran font
                if (controlInfo.Control is Button || controlInfo.Control is Label)
                {
                    float newFontSize = controlInfo.OriginalFont.Size * fontRatio;
                    if (newFontSize > 0)
                    {
                        controlInfo.Control.Font = new Font(controlInfo.OriginalFont.FontFamily, newFontSize, controlInfo.OriginalFont.Style);
                    }
                }
            }
        }

        private void buttonCitra_Click(object sender, EventArgs e)
        {
            // Create an instance of OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter options and filter index
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.FilterIndex = 1;

            // Set the title of the dialog
            openFileDialog.Title = "Select an Image";

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Load the selected image into the PictureBox
                pictureBoxInput.Image = Image.FromFile(openFileDialog.FileName);
                // Optional: Set SizeMode to Zoom so the image scales properly
                pictureBoxInput.SizeMode = PictureBoxSizeMode.Zoom;
                try
                {
                    // Load the selected image into the PictureBox
                    pictureBoxInput.Image = Image.FromFile(openFileDialog.FileName);
                    // Optional: Set SizeMode to Zoom so the image scales properly
                    pictureBoxInput.SizeMode = PictureBoxSizeMode.Zoom;

                    // Get the file name and create the target file path
                    /*                    string fileName = Path.GetFileName(openFileDialog.FileName);*/
                    string targetFilePath = Path.Combine(targetFolderPath, "Data.bmp");

                    // Copy the selected file to the target folder
                    File.Copy(openFileDialog.FileName, targetFilePath, overwrite: true);

                    /* MessageBox.Show("Image copied successfully to " + targetFilePath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);*/
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while copying the image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            buttonSearch.Enabled = true;
        }

        private void buttonBM_Click(object sender, EventArgs e)
        {
            /*            buttonBM.BackColor = System.Drawing.Color.LightBlue;
                        buttonKMP.BackColor = System.Drawing.SystemColors.Control;*/
            buttonBM.Enabled = false;
            buttonKMP.Enabled = true;
            isKMP = false;
        }

        private void buttonKMP_Click(object sender, EventArgs e)
        {
            /*buttonKMP.BackColor = System.Drawing.Color.LightBlue;
            buttonBM.BackColor = System.Drawing.SystemColors.Control;*/
            buttonKMP.Enabled = false;
            buttonBM.Enabled = true;
            isKMP = true;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            buttonSearch.Enabled = false;
            // Thread.Sleep(3000);
            Stopwatch timer = Stopwatch.StartNew();
            BackEnd backEnd = new BackEnd(isKMP);
            timer.Stop();
            long timeSpan = timer.ElapsedMilliseconds;
            int waktu = (int) timeSpan;
            float kemiripan = backEnd.persentase;
            labelNilaiWaktu.Text = ": " + waktu.ToString() + " ms";
            labelNilaiKemiripan.Text = ": " + Math.Round(kemiripan, 2).ToString() + " %";
            pictureBoxbiodata.Image = null;
            // string nik = "1234567890123456";
            // string nama = "Rijal";
            // string tempatlahir = "Bandung";
            // string tanggallahir = "31 Januari 2001";
            // string gender = "Laki-laki";
            // string goldar = "O";
            // string alamat = "Cisitu";
            // string agama = "Islam";
            // string status = "Menikah";
            // string pekerjaan = "PNS";
            // string kwn = "Indonesia";
            string nik = backEnd.biodata[0];
            string nama = backEnd.biodata[1];
            string tempatlahir = backEnd.biodata[2];
            string tanggallahir = backEnd.biodata[3];
            string gender = backEnd.biodata[4];
            string goldar = backEnd.biodata[5];
            string alamat = backEnd.biodata[6];
            string agama = backEnd.biodata[7];
            string status = backEnd.biodata[8];
            string pekerjaan = backEnd.biodata[9];
            string kwn = backEnd.biodata[10];
            // labelFormat.Text = $"NIK : {nik}\nNama : {nama}\nTempat Lahir : {tempatlahir}\nTanggal Lahir : {tanggallahir}\nJenis Kelamin : {gender}\nGolongan Darah : {goldar}\nAlamat : {alamat}\nAgama : {agama}\nStatus Perkawinan : {status}\nPekerjaan : {pekerjaan}\nKewarganegaraan : {kwn}";
            labelFormat.Text = "NIK\nNama\nTempat Lahir\nTanggal Lahir\nJenis Kelamin\nGolongan Darah\nAlamat\nAgama\nStatus Perkawinan\nPekerjaan\nKewarganegaraan";
            labelFormat.Visible = true;
            labelData.Visible = true;
            labelData.Text = $": {nik}\n: {nama}\n: {tempatlahir}\n: {tanggallahir}\n: {gender}\n: {goldar}\n: {alamat}\n: {agama}\n: {status}\n: {pekerjaan}\n: {kwn}";
            pictureBoxbiodata.BorderStyle = BorderStyle.FixedSingle;
            // labelFormat.BorderStyle = BorderStyle.FixedSingle;
            // labelData.BorderStyle = BorderStyle.FixedSingle;
            /*labelFormat.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);*/
            /*String path = Path.GetFullPath("../../tes"); 
            pictureBoxOutput.Image = new Bitmap(Path.Combine(path, "100__M_Left_index_finger.bmp"));*/
            pictureBoxOutput.Image = new Bitmap(backEnd.pathGambar);
            // pictureBoxOutput.SizeMode = PictureBoxSizeMode.Zoom;
            buttonSearch.Enabled = true;
        }

        private void pictureBoxbiodata_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxInput_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxOutput_Click(object sender, EventArgs e)
        {

        }
    }

    class ControlInfo
    {
        public Control Control { get; private set; }
        public Point OriginalLocation { get; private set; }
        public Size OriginalSize { get; private set; }
        public Font OriginalFont { get; private set; }


        public ControlInfo(Control control, Point originalLocation, Size originalSize, Font originalFont)
        {
            this.Control = control;
            this.OriginalLocation = originalLocation;
            this.OriginalSize = originalSize;
            this.OriginalFont = originalFont;
        }
    }
}