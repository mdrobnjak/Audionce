using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveVisualizerAudioInput
{
    public partial class SettingsForm : Form
    {
        static int iGen = 3;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            InitGV();

            this.gvSettings.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvAutoSettings_CellValueChanged);
        }

        void InitGV()
        {
            int[] bandwidths = new int[Range.Count];
            double[] threshMults = new double[Range.Count];

            gvSettings.Columns.Add("Range", "Range " + 1);

            //Per Range Rows
            gvSettings.Rows.Add();
            gvSettings.Rows[0].HeaderCell.Value = "Bandwidth (Bands)";

            gvSettings.Rows.Add();
            gvSettings.Rows[1].HeaderCell.Value = "Threshold Multiplier";

            for (int i = 0; i < Range.Count; i++)
            {
                gvSettings.Columns.Add("Range", "Range " + (i + 2));
                gvSettings.Rows[0].Cells[i].Value = Range.Ranges[i].AutoSettings.Bandwidth;
                gvSettings.Rows[1].Cells[i].Value = Range.Ranges[i].AutoSettings.ThresholdMultiplier;
            }

            gvSettings.Columns[Range.Count].Name = gvSettings.Columns[Range.Count].HeaderText = "General";

            //Seconds to collect
            gvSettings.Rows.Add(); //Spacer
            gvSettings.Rows.Add();
            gvSettings.Rows[3].HeaderCell.Value = "Seconds to Collect";
            gvSettings.Rows[3].Cells[iGen].Value = AutoSettings.SecondsToCollect;

            //FFT Timer Interval
            gvSettings.Rows.Add(); //Spacer
            gvSettings.Rows.Add();
            gvSettings.Rows[5].HeaderCell.Value = "FFT Interval";
            gvSettings.Rows[5].Cells[iGen].Value = ((LiveVisualizerAudioInputMDIForm)this.MdiParent).GetTimerInterval();

        }

        private void gvAutoSettings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                object cellValue = gvSettings.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                switch (e.RowIndex)
                {
                    case 0:
                        Range.Ranges[e.ColumnIndex].AutoSettings.Bandwidth = Convert.ToInt32(cellValue);
                        break;
                    case 1:
                        Range.Ranges[e.ColumnIndex].AutoSettings.ThresholdMultiplier = Convert.ToSingle(cellValue);
                        break;
                    case 3:
                        if (e.ColumnIndex == iGen) AutoSettings.SecondsToCollect = Convert.ToSingle(cellValue);
                        break;
                    case 5:
                        if (e.ColumnIndex == iGen) ((LiveVisualizerAudioInputMDIForm)this.MdiParent).SetTimerInterval(Convert.ToInt32(cellValue));
                        break;
                    default:
                        break;

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
