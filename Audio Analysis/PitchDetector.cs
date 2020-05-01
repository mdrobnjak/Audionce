using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    public class PitchDetector
    {
        int sampleRate = 88200;
        int minFreq = 30;
        int maxFreq = 2000;
        int maxOffset, minOffset;

        float[] variableSizeBuffer = new float[10584];

        float[] prevBuffer = new float[10584];

        public float GetPitch(float[] audioFrame)
        {
            maxOffset = sampleRate / minFreq;
            minOffset = sampleRate / maxFreq;

            float maxCorr = 0;
            int maxLag = 0;

            for (int lag = maxOffset; lag >= minOffset; lag--)
            {
                float corr = 0; // this is calculated as the sum of squares

                for (int i = 0; i < audioFrame.Length; i++)
                {
                    int oldIndex = i - lag;
                    float sample = ((oldIndex < 0) ? prevBuffer[audioFrame.Length + oldIndex] : audioFrame[oldIndex]);
                    corr += (sample * audioFrame[i]);
                }

                if (corr > maxCorr)
                {
                    maxCorr = corr;
                    maxLag = lag;
                }
            }

            prevBuffer = audioFrame;

            return sampleRate / maxLag;
        }

        public void HandleAudioData(float[] audioFrame)
        {
            if (variableSizeBuffer[0] == 0)
            {
                Array.Copy(audioFrame, 0, variableSizeBuffer, 0, audioFrame.Length);
            }
            else if (variableSizeBuffer[2646] == 0)
            {
                Array.Copy(audioFrame, 0, variableSizeBuffer, 2646, audioFrame.Length);
            }
            else if (variableSizeBuffer[5292] == 0)
            {
                Array.Copy(audioFrame, 0, variableSizeBuffer, 5292, audioFrame.Length);
            }
            else if (variableSizeBuffer[7938] == 0)
            {
                Array.Copy(audioFrame, 0, variableSizeBuffer, 7938, audioFrame.Length);
                Console.WriteLine(GetPitch(variableSizeBuffer));
                variableSizeBuffer = new float[10584];
            }
        }
    }
}
