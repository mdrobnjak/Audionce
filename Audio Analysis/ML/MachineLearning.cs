using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    public static class ML
    {
        public static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "BandSelectionData-train.csv");
        static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "BandSelectionData-test.csv");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");
        static TextLoader _textLoader;

        public static MLContext mlContext;

        public static void Predict()
        {
            mlContext = new MLContext(seed: 0);

            _textLoader = mlContext.Data.CreateTextReader(new TextLoader.Arguments()
            {
                Separator = ",",
                HasHeader = true,
                Column = new[]
                {
                    new TextLoader.Column("BandIndex", DataKind.R4, 0),

                    new TextLoader.Column("AlgorithmDatas", DataKind.R4, new []{new TextLoader.Range(1, 100) })
                }
            });

            var model = Train(mlContext, _trainDataPath);

            Evaluate(mlContext, model);            
        }

        public static ITransformer Train(MLContext mlContext, string dataPath)
        {
            IDataView dataView = _textLoader.Read(dataPath);
            var pipeline = mlContext.Transforms.CopyColumns("BandIndex", "Label")
                .Append(mlContext.Transforms.Concatenate("Features", "AlgorithmDatas"))
                .Append(mlContext.Regression.Trainers.StochasticDualCoordinateAscent());

            var model = pipeline.Fit(dataView);

            SaveModelAsFile(mlContext, model);
            return model;

        }

        private static void SaveModelAsFile(MLContext mlContext, ITransformer model)
        {
            using (var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                mlContext.Model.Save(model, fileStream);

            Console.WriteLine("The model is saved to {0}", _modelPath);
        }

        private static void Evaluate(MLContext mlContext, ITransformer model)
        {
            IDataView dataView = _textLoader.Read(_testDataPath);
            var predictions = model.Transform(dataView);
            var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");
            Console.WriteLine();
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*       Model quality metrics evaluation         ");
            Console.WriteLine($"*------------------------------------------------");

            Console.WriteLine($"*       R2 Score:      {metrics.RSquared:0.##}");

            Console.WriteLine($"*       RMS loss:      {metrics.Rms:#.##}");
        }

        private static void PredictFromFile(MLContext mlContext)
        {
            ITransformer loadedModel;
            using (var stream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                loadedModel = mlContext.Model.Load(stream);
            }

            var predictionFunction = loadedModel.CreatePredictionEngine<BandSelectionData, BandSelectionPrediction>(mlContext);

            float bandIndex;
            float[] algDatas = new float[100];

            //Get sample from test csv:
            using (var reader = new StreamReader(_testDataPath))
            {
                string line;
                string[] values = new string[101];
                int count = 0;
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    values = line.Split(',');

                    if (count == 0)
                    {
                        count++;
                        continue;
                    }

                    bandIndex = float.Parse(values[0]);
                    for (int i = 1; i <= 100; i++)
                    {
                        algDatas[i - 1] = float.Parse(values[i]);
                    }

                    var BandSelectionSample = new BandSelectionData()
                    {
                        BandIndex = bandIndex,
                        AlgorithmDatas = algDatas
                    };

                    var prediction = predictionFunction.Predict(BandSelectionSample);

                    Console.WriteLine($"**********************************************************************");
                    Console.WriteLine("Predicted index: " + prediction.BandIndex + ", actual index: " + bandIndex + "");
                    Console.WriteLine($"**********************************************************************");

                    count++;
                }
            }
        }

        public static float PredictRealTime(MLContext mlContext, float[] algDatas)
        {
            ITransformer loadedModel;
            using (var stream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                loadedModel = mlContext.Model.Load(stream);
            }

            var predictionFunction = loadedModel.CreatePredictionEngine<BandSelectionData, BandSelectionPrediction>(mlContext);
            
            var BandSelectionSample = new BandSelectionData()
            {
                BandIndex = 0,
                AlgorithmDatas = algDatas
            };

            var prediction = predictionFunction.Predict(BandSelectionSample);

            return prediction.BandIndex;
        }
    }
}
