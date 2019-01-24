using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioAnalyzer
{
    public partial class frmAudioAnalyzerMDI : Form
    {
        private int childFormNumber = 0;
        MdiLayout DefaultLayout = MdiLayout.TileHorizontal;

        frmSpectrum frmSpectrum;
        frmChart frmChart;
        frmArduino frmArduino;
        frmAutoSettings frmAutoSettings;

        Range[] Ranges;

        public frmAudioAnalyzerMDI()
        {
            InitializeComponent();

            using (System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess())
                p.PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            
            Range.Init(ref this.Ranges);
            Range.Init(ref FileIO.Ranges);
            Range.Init(ref frmAutoSettings.Ranges);
            
            AudioIn.InitSoundCapture();
            frmSpectrum.SyncBandsAndFreqs();
            lblPreset.Text = FileIO.InitPathAndGetPreset();
            frmArduino.InitArduinoPort();

            LoadChildForms();

            InitControls();
        }

        private void frmAudioAnalyzerMDI_Load(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        void InitControls()
        {
            for (int i = 0; i < Range.Count; i++)
            {
                cboRange.Items.Add("Range " + i);
            }
            cboRange.SelectedIndex = 0;
        }

        void LoadChildForms()
        {
            frmSpectrum = new frmSpectrum();
            frmSpectrum.MdiParent = this;
            childFormNumber++;
            frmSpectrum.Show();

            frmChart = new frmChart();
            frmChart.MdiParent = this;
            childFormNumber++;
            frmChart.Show();
        }

        public void timerFFT_Tick(object sender, EventArgs e)
        {


            if (FFT.N_FFT != FFT.N_FFTBuffer)
            {
                FFT.N_FFT = FFT.N_FFTBuffer;
                FFT.transformedData = null;
            }


            AudioIn.waveInStream.DataAvailable -= new EventHandler<WaveInEventArgs>(AudioIn.waveInStream_DataAvailable);
            FFT.transformedData = FFT.FFTWithProcessing(FFT.transformedData);

            for (int r = 0; r < Range.Count; r++)
            {
                Filter(r);

                ApplySubtraction(r);

                Ranges[r].AutoSettings.ApplyAutoSettings();

                if (Gate(r))
                {
                    frmArduino.Trigger(r);
                    //SetProgressBars(r);
                }
                else
                {
                    //FadeProgressBars(r);
                }

            }

            Task.Run(() => frmSpectrum.Draw());
            Task.Run(() => frmChart.Draw());

            AudioIn.waveInStream.DataAvailable += new EventHandler<WaveInEventArgs>(AudioIn.waveInStream_DataAvailable);

            //if (AutoSettings.Ranging)
            //{
            //    AutoSettings.CollectFFTData(FFT.transformedData);
            //}
            //else if (AutoSettings.ReadyToProcess)
            //{
            //    AutoSettings.ReadyToProcess = false;

            //    //Range1
            //    PrintBandAnalysis(Ranges[0].AutoSettings.DoBandAnalysis());
            //    Ranges[0].AutoSettings.KickSelector();

            //    //Range2
            //    Ranges[1].AutoSettings.SnareSelector();

            //    //Range3
            //    Ranges[2].AutoSettings.HatSelector();

            //    //SelectedRange
            //    trkbrThreshold.Maximum = (int)(Range.Active.Threshold * 1.33);
            //    UpdateControls();

            //    AutoSettings.Reset();
            //}
        }

        #region Visual Studio Generated Code

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            openFileDialog.InitialDirectory = FileIO.Path;
            
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;

                FileIO.ReadConfig(FileName);

                frmSpectrum.UpdateControls();
                frmChart.UpdateControls();

                lblPreset.Text = FileName.Split('\\').Last().Split('.')[0];
            }

        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;

                FileIO.WriteConfig(FileName);
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        #endregion

        private void frmAudioAnalyzerMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            AudioIn.Dispose();
        }

        private void cboRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            Range.SetActive(cboRange.SelectedIndex);

            frmSpectrum.UpdateControls();
            frmChart.UpdateControls();
        }

        private void nFFTToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach(ToolStripMenuItem dropDownItem in nFFTToolStripMenuItem.DropDownItems)
            {
                if(dropDownItem.Checked)
                {
                    dropDownItem.Checked = false;
                }
            }
            ((ToolStripMenuItem)(e.ClickedItem)).Checked = true;

            int intVar;

            if (!Int32.TryParse(((ToolStripMenuItem)(e.ClickedItem)).Text, out intVar)) return;
            FFT.N_FFTBuffer = intVar;
            frmSpectrum.SyncBandsAndFreqs();
            frmSpectrum.UpdateControls();
        }

        private void arduinoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmArduino = new frmArduino();
            frmArduino.MdiParent = this;
            childFormNumber++;
            frmArduino.Show();
        }

        private void autoSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAutoSettings = new frmAutoSettings();
            frmAutoSettings.MdiParent = this;
            childFormNumber++;
            frmAutoSettings.Show();
        }
    }
}
