using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    /// <summary>
    /// ML training data should be in a more abbreviated form than a full 5 seconds of range audio. 
    /// A few key metrics should be determined for the 5 second sample and entered as training data.
    /// Ideally, the metrics would correlate to frequencies rather than band indices for maximum modularity.
    /// </summary>
    class BandAnalysis
    {    
        static string trainingRow;

        public static void CreateTrainingRowFromAudioData(List<List<float>> fftDataHistory)
        {
            float max;

            trainingRow = "";

            for(int i = 0; i < fftDataHistory.Count; i++) //For each band
            {
                max = 0;
                for (int j = 0; j < 300; j++) //For each audio data over time
                {
                    if (fftDataHistory[i][j] > max) max = fftDataHistory[i][j];
                }

                for (int j = 0; j < 300; j++)
                {
                    trainingRow += (fftDataHistory[i][j] / max).ToString() + ",";
                }
            }

            trainingRow = trainingRow.Remove(trainingRow.Length - 1, 1);
            trainingRow += Environment.NewLine;


            if (!ML.Initialized) return;
            System.Windows.Forms.MessageBox.Show(ML.PredictRealTime(ML.mlContext, Array.ConvertAll(trainingRow.Split(','), float.Parse)).ToString());
        }

        public static void CompleteAndSaveTraining()
        {
            trainingRow = trainingRow.Insert(0, Range.Active.LowCutAbsolute.ToString() + ",");
            File.AppendAllText(ML._trainDataPath, trainingRow);
            System.Windows.Forms.MessageBox.Show("Training Entry Saved.");
        }
    }
}
