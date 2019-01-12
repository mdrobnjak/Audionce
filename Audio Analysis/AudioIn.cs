using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalysis
{
    public static class AudioIn
    {
        public static double[] sourceData;
        public const int RATE = 44100;
        public static Node dataList = new Node(new ComplexNumber(0, 0));
        public static Node endingNode;
        static WaveIn waveInStream;
        static BufferedWaveProvider bwp;
        static int BUFFERSIZE = (int)Math.Pow(2, 11); // must be a multiple of 2
        public static int distance2Node = 0;

        #region Settings
        public static int Mode
        {
            get;
            set;
        }
        #endregion

        public static void InitSoundCapture()
        {
            endingNode = dataList;
            
            waveInStream = new WaveIn
            {
                DeviceNumber = 0,
                
                NumberOfBuffers = 5,

                BufferMilliseconds = (int)((double)BUFFERSIZE / (double)RATE * 1000.0),

                WaveFormat = new WaveFormat(RATE, 1)
            };

            waveInStream.DataAvailable += new EventHandler<WaveInEventArgs>(waveInStream_DataAvailable);

            bwp = new BufferedWaveProvider(waveInStream.WaveFormat)
            {
                BufferLength = BUFFERSIZE * 2,
                DiscardOnBufferOverflow = true
            };
            
            waveInStream.StartRecording();
            
            Mode = 1;
        }

        private static void waveInStream_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (sourceData == null)
                sourceData = new double[e.BytesRecorded / 2];

            for (int i = 0; i < e.BytesRecorded; i += 2)
            {
                short sampleL = (short)((e.Buffer[i + 1] << 8) | e.Buffer[i + 0]);
                //  short sampleR = (short)((e.Buffer[i + 1+2] << 8) | e.Buffer[i + 2]);
                double sample32 = (sampleL) / 32722d;
                sourceData[i / 2] = sample32;// (double)(e.Buffer[i]) / 255;
            }

            bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);

            AppendData(sourceData);
        }

        private static void AppendData(double[] newData)
        {
            int N = 10000;
            //double[] data = new double[N];

            var prevNode = dataList;
            var shiftNode = dataList;


            for (int j = 0; j < newData.Length; j++)
            {
                endingNode.NextNode = new Node(new ComplexNumber(newData[j], 0));
                endingNode.NextNode.PrevNode = endingNode;
                endingNode = endingNode.NextNode;
                if (j == newData.Length - 1)
                    endingNode.isEndPoint = true;
                // data[thresholdCounter] = runningNode.Value;
                distance2Node++;
            }
            if (distance2Node > N)
            {
                for (int j = 0; j < newData.Length; j++)
                {
                    dataList = dataList.NextNode;
                }
                dataList.isStartPoint = true;
                dataList.PrevNode = null;
                distance2Node = distance2Node - newData.Length;
            }

        }

        public static void Dispose()
        {
            if (waveInStream != null)
            {
                waveInStream.StopRecording();
                waveInStream.Dispose();
                waveInStream = null;
            }
        }
    }
}
