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
    public partial class SettingsForm : Form
    {
        static int iGen = 3;

        public static Range[] Ranges;

        public SettingsForm()
        {
            InitializeComponent();

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
                gvSettings.Rows[0].Cells[i].Value = Ranges[i].AutoSettings.Bandwidth;
                gvSettings.Rows[1].Cells[i].Value = Ranges[i].AutoSettings.ThresholdMultiplier;
            }

            gvSettings.Columns[Range.Count].Name = gvSettings.Columns[Range.Count].HeaderText = "General";

            //Single Rows
            gvSettings.Rows.Add(); //Spacer
            gvSettings.Rows.Add();
            gvSettings.Rows[3].HeaderCell.Value = "Seconds to Collect";
            gvSettings.Rows[3].Cells[iGen].Value = AutoSettings.SecondsToCollect;

        }

        private void gvAutoSettings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                object cellValue = gvSettings.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                switch (e.RowIndex)
                {
                    case 0:
                        Ranges[e.ColumnIndex].AutoSettings.Bandwidth = Convert.ToInt32(cellValue);
                        break;
                    case 1:
                        Ranges[e.ColumnIndex].AutoSettings.ThresholdMultiplier = Convert.ToDouble(cellValue);
                        break;
                    case 3:
                        if (e.ColumnIndex == iGen) AutoSettings.SecondsToCollect = Convert.ToDouble(cellValue);
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
