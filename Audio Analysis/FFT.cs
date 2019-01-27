using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AudioAnalyzer
{
    public class FFT
    {
        public static double[] transformedData;

        public static int N_FFT = 64;
        public static int N_FFTBuffer = N_FFT;
        public static bool rawFFT = false;
        public static double dropOffScale = 10;
        public static bool DropOff = false;

        public static int[] chunk_freq = { 800, 1600, 3200, 6400, 12800, 30000 };
        public static int[] chunk_freq_jump = { 1, 2, 4, 6, 8, 10, 16 };

        static ComplexNumber[][] E = new ComplexNumber[12][];
        static ComplexNumber[][] D = new ComplexNumber[12][];

        public static void InitJaggedArrays()
        {
            for(int i = 0; i <= Math.Log(N_FFTBuffer / 2, 2); i++)
            {
                int size = (int)Math.Pow(2, Math.Log(N_FFTBuffer / 2, 2) - i);
                E[i] = new ComplexNumber[size];
                D[i] = new ComplexNumber[size];
            }
        }

        private static void Eff(ref ComplexNumber[] data, int i)
        {
            int N = data.Length;
            if (N == 1)
            {
                return;
            }

            int k = 0;
            for (k = 0; k < N / 2; k++)
            {
                E[i][k] = data[k * 2];
                D[i][k] = data[k * 2 + 1];
            }
            Eff(ref E[i],i+1);
            Eff(ref D[i],i+1);

            for (k = 0; k < N / 2; k++)
            {
                D[i][k] *= ComplexNumber.FromPolar(1, -2 * Math.PI * k / N);
                data[k] = E[i][k] + D[i][k];
                data[k + N / 2] = E[i][k] - D[i][k];
            }
        }
        
        private static void RawFFTByRef(ref ComplexNumber[] data)
        {
            int N = data.Length;
            ComplexNumber[] e, d;
            if (N == 1)
            {
                return;
            }

            int k = 0;
            e = new ComplexNumber[N / 2];
            d = new ComplexNumber[N / 2];
            for (k = 0; k < N / 2; k++)
            {
                e[k] = data[k * 2];
                d[k] = data[k * 2 + 1];
            }
            RawFFTByRef(ref e);
            RawFFTByRef(ref d);

            for (k = 0; k < N / 2; k++)
            {
                d[k] *= ComplexNumber.FromPolar(1, -2 * Math.PI * k / N);
                data[k] = e[k] + d[k];
                data[k + N / 2] = e[k] - d[k];
            }
        }

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

        static DateTime chkpoint1;

        public static double[] FFTWithProcessing(double[] lastData)
        {
            chkpoint1 = DateTime.Now;
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
                    break;
                }
                runningNode = runningNode.PrevNode;
            }
            //var result = RawFFT(data);
            Eff(ref data, 0);


            if (rawFFT)
            {
                var resultDouble = data.Select(x => x.Magnitude).ToArray();
                Array.Resize(ref resultDouble, Spectrum.TotalBands);
                return resultDouble;
            }
            

            double N2 = data.Length / 2;
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
                    value += data[j].Magnitude;
                }

                
                lastData[transformedDataIndex] -= DateTime.Now.Subtract(chkpoint1).TotalMilliseconds * dropOffScale;
                if (!DropOff)
                    finalresult[transformedDataIndex] = value;
                else
                    finalresult[transformedDataIndex] = value > lastData[transformedDataIndex] ? value : lastData[transformedDataIndex];
                transformedDataIndex++;
            }


            if (!transformed)
                Array.Resize(ref finalresult, transformedDataIndex);
            
            //Console.WriteLine(DateTime.Now.Subtract(chkpoint1).TotalMilliseconds);
            return finalresult;
        }
    }
}
