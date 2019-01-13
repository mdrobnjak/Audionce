using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AudioAnalysis
{
    public partial class frmAudioAnalysis : Form
    {
        private void InitTabFFT()
        {
            string[] n = { "8192", "4096", "2048", "1024", "512", "256", "128", "64", "32", "16", "8", "4", "2", "1" };
            cboN_FFT.Items.AddRange(n);
            cboN_FFT.Text = FFT.N_FFT.ToString();

            txtSpectrumScale.Text = converterScale.ToString();
            txtTimer1Interval.Text = timer1.Interval.ToString();
            txtDropOffScale.Text = FFT.dropOffScale.ToString();
        }

        private void btnCommitFFTSettings_Click(object sender, EventArgs e)
        {
            int intVar;
            double dblVar;

            if (!Int32.TryParse(cboN_FFT.Text, out intVar)) return;
            FFT.N_FFTBuffer = intVar;
            Spectrum.SyncBandsAndFreqs();
            Spectrum.InitFullSpectrum();
            if (!Int32.TryParse(txtSpectrumScale.Text, out intVar)) return;
            InitConverter(intVar);
            if (!Int32.TryParse(txtTimer1Interval.Text, out intVar)) return;
            timer1.Interval = intVar;
            if (!Double.TryParse(txtDropOffScale.Text, out dblVar)) return;
            FFT.dropOffScale = dblVar;
            if (!Int32.TryParse(txtMode.Text, out intVar)) return;
            FFT.mode = intVar;

            UpdateControls();
        }
    }

    public class FFT
    {
        public static int N_FFT = 4096;
        public static int N_FFTBuffer = N_FFT;
        public static bool rawFFT = false;
        public static double dropOffScale = 10;
        public static int mode = 0;

        public static int[] chunk_freq = { 800, 1600, 3200, 6400, 12800, 30000 };
        public static int[] chunk_freq_jump = { 1, 2, 4, 6, 8, 10, 16 };
        static double lastDelay = 0;

        private static ComplexNumber[] RawFFT(ComplexNumber[] data)
        {
            int N = data.Length;
            ComplexNumber[] X = new ComplexNumber[N];
            ComplexNumber[] e, E, d, D;
            if (N == 1)
            {
                X[0] = data[0];
                return X;
            }

            int k = 0;
            e = new ComplexNumber[N / 2];
            d = new ComplexNumber[N / 2];
            for (k = 0; k < N / 2; k++)
            {
                e[k] = data[k * 2];
                d[k] = data[k * 2 + 1];
            }
            E = RawFFT(e);
            D = RawFFT(d);

            for (k = 0; k < N / 2; k++)
            {
                D[k] *= ComplexNumber.FromPolar(1, -2 * Math.PI * k / N);
            }

            for (k = 0; k < N / 2; k++)
            {
                X[k] = E[k] + D[k];
                X[k + N / 2] = E[k] - D[k];
            }
            return X;
        }

        public static double[] FFTWithProcessing(double[] lastData)
        {
            DateTime chkpoint1 = DateTime.Now;
            if (AudioIn.dataList==null)
                return null;
            int actualN = AudioIn.distance2Node + 1;

            if (actualN < N_FFT)
                return new double[0];

            bool transformed = false;
            if (lastData == null || lastData.Length == 0)
            {
                lastData = new double[N_FFT];
            }
            else
            {
                transformed = true;
            }
            ComplexNumber[] data = new ComplexNumber[N_FFT];
            var runningNode = AudioIn.endingNode;
            for (int i = 0; i < N_FFT; i++)
            {
                data[i] = runningNode.Value;
                if (runningNode.PrevNode == null)
                {
                }
                runningNode = runningNode.PrevNode;
            }
            var result = RawFFT(data);

            if (rawFFT)
            {
                var resultDouble = result.Select(x => x.Magnitude).ToArray();
                Array.Resize(ref resultDouble, Spectrum.numBands);
                return resultDouble;
            }

            double N2 = result.Length / 2;
            double[] finalresult = new double[lastData.Length];
            int k = 1, transformedDataIndex = 0;
            double value = 0;

            for (int i = 0; i < N2; i += k)
            {
                value = 0;
                var mappedFreq = i * AudioIn.RATE / 2 / N2;
                for (int l = 0; l < chunk_freq.Length; l++)
                {
                    if (mappedFreq < chunk_freq[l] || l == chunk_freq.Length - 1)
                    {
                        k = chunk_freq_jump[l];//chunk_freq[l] / chunk_freq[0];
                        break;
                    }
                }

                for (int j = i; j < i + k && j < N2; j++)
                {
                    value += result[j].Magnitude;
                }

                
                lastData[transformedDataIndex] -= lastDelay * dropOffScale;
                if (mode == 0)
                    finalresult[transformedDataIndex] = value;
                else
                    finalresult[transformedDataIndex] = value > lastData[transformedDataIndex] ? value : lastData[transformedDataIndex];
                transformedDataIndex++;
            }


            if (!transformed)
                Array.Resize(ref finalresult, transformedDataIndex);


            DateTime chkpoint1_end = DateTime.Now;
            lastDelay = chkpoint1_end.Subtract(chkpoint1).TotalMilliseconds;
            //osdPanel.AddSet("FFT delay(ms)", lastDelay.ToString());
            return finalresult;

        }
    }
}
