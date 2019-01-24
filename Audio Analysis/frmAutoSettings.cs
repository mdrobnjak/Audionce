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
    public partial class frmAutoSettings : Form
    {
        public static Range[] Ranges;

        public frmAutoSettings()
        {
            InitializeComponent();

            InitGV();

            this.gvAutoSettings.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvAutoSettings_CellValueChanged);
        }

        void InitGV()
        {
            int[] bandwidths = new int[Range.Count];
            double[] threshMults = new double[Range.Count];

            gvAutoSettings.Columns.Add("Range", "Range " + 1);

            //Per Range Rows
            gvAutoSettings.Rows.Add();
            gvAutoSettings.Rows[0].HeaderCell.Value = "Bandwidth (Bands)";

            gvAutoSettings.Rows.Add();
            gvAutoSettings.Rows[1].HeaderCell.Value = "Threshold Multiplier";
            
            for (int i = 0; i < Range.Count; i++)
            {
                gvAutoSettings.Columns.Add("Range", "Range " + (i + 1));
                gvAutoSettings.Rows[0].Cells[i].Value = Ranges[i].AutoSettings.Bandwidth;
                gvAutoSettings.Rows[1].Cells[i].Value = Ranges[i].AutoSettings.ThresholdMultiplier;
            }

            gvAutoSettings.Columns.RemoveAt(Range.Count);

            //Single Rows
            gvAutoSettings.Rows.Add(); //Spacer
            gvAutoSettings.Rows.Add();
            gvAutoSettings.Rows[3].HeaderCell.Value = "Seconds to Collect";
            gvAutoSettings.Rows[3].Cells[0].Value = AutoSettings.SecondsToCollect;

        }

        private void gvAutoSettings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                object cellValue = gvAutoSettings.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                switch (e.RowIndex)
                {
                    case 0:
                        Ranges[e.ColumnIndex].AutoSettings.Bandwidth = Convert.ToInt32(cellValue);
                        break;
                    case 1:
                        Ranges[e.ColumnIndex].AutoSettings.ThresholdMultiplier = Convert.ToDouble(cellValue);
                        break;
                    case 3:
                        AutoSettings.SecondsToCollect = Convert.ToDouble(cellValue);
                        break;
                    default:
                        break;

                }
            }
            catch
            {
            }
        }
    }
}
