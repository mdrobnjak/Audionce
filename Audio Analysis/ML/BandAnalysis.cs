using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    class BandAnalysis
    {    
        static string trainingRow;

        public static void CreateTrainingRowFromAudioData(List<List<double>> fftDataHistory)
        {
            double max;

            trainingRow = "";

            for(int i = 0; i < fftDataHistory.Count; i++) //For each band
            {
                max = 0;
                for (int j = 0; j < fftDataHistory[i].Count; j++) //For each audio data over time
                {
                    if (fftDataHistory[i][j] > max) max = fftDataHistory[i][j];
                }

                for (int j = 0; j < fftDataHistory[i].Count; j++)
                {
                    trainingRow += (fftDataHistory[i][j] / max).ToString() + ",";
                }
            }

            trainingRow = trainingRow.Remove(trainingRow.Length - 1, 1);
            trainingRow += Environment.NewLine;

            //txtPrediction.Text = ML.PredictRealTime(ML.mlContext, Array.ConvertAll(csvRow.Split(','), float.Parse)).ToString();
        }

        public static void CompleteAndSaveTraining()
        {
            trainingRow = trainingRow.Insert(0, Range.Active.LowCutAbsolute.ToString() + ",");
            File.AppendAllText(ML._trainDataPath, trainingRow);
        }
    }
}
