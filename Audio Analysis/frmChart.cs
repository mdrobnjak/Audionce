using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AudioAnalyzer
{
    public partial class frmChart : Form
    {
        new bool Enabled = true;
        StripLine stripline = new StripLine();

        public frmChart()
        {
            InitializeComponent();

            InitStripLine();
        }

        void InitStripLine()
        {
            stripline.Interval = 0;
            stripline.IntervalOffset = 35;
            stripline.StripWidth = 5;
            stripline.BackColor = Color.Red;
            chartAmplitude.ChartAreas[0].AxisY.StripLines.Add(stripline);
        }

        delegate void DrawCallback();

        public void Draw()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (chartAmplitude.InvokeRequired)
            {
                DrawCallback d = new DrawCallback(Draw);
                this.Invoke(d);
            }
            else
            {
                if (Enabled) DrawChart();
            }
        }

        private void DrawChart()
        {
            chartAmplitude.Series[0].Points.AddY(Range.Active.Audio);

            chartAmplitude.ChartAreas[0].AxisY.Maximum = GetMaxYFromLast(200);
            trkbrThreshold.Maximum = (int)chartAmplitude.ChartAreas[0].AxisY.Maximum + 1;

            //cvt.MaxScaledY = 800 / chart1.ChartAreas[0].AxisY.Maximum; //AutoScaling Spectrum Y

            chartAmplitude.ChartAreas[0].AxisX.Minimum = chartAmplitude.ChartAreas[0].AxisX.Maximum - 250;
        }

        private double GetMaxYFromLast(int numPoints)
        {
            return chartAmplitude.Series[0].Points.FindMaxByValue("Y1",

                    (chartAmplitude.Series[0].Points.Count() > numPoints ? (chartAmplitude.Series[0].Points.Count() - numPoints) : chartAmplitude.Series[0].Points.Count() - 1)

                    ).YValues[0];
        }

        public void UpdateControls()
        {
            if (Range.Active.Threshold > trkbrThreshold.Maximum)
            {
                trkbrThreshold.Maximum = (int)(Range.Active.Threshold * 1.33);
            }

            trkbrThreshold.Value = (int)Range.Active.Threshold;
        }

        private void trkbrThreshold_ValueChanged(object sender, EventArgs e)
        {
            stripline.IntervalOffset = Range.Active.Threshold = trkbrThreshold.Value;

            txtThreshold.Text = trkbrThreshold.Value.ToString();
        }

        public void AutoThreshold()
        {
            trkbrThreshold.Value = (int)(Range.Active.AutoSettings.ThresholdMultiplier * chartAmplitude.ChartAreas[0].AxisY.Maximum);
        }
    }
}
