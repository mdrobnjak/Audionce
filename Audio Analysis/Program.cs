using Microsoft.ML;
using System;
using System.IO;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;
using System.Windows.Forms;

namespace AudioAnalyzer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (frmAudioAnalyzer myapps = new frmAudioAnalyzer())
            {
                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(myapps);
            }
        }

        static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "BandSelectionData-train.csv");
        static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "BandSelectionData-test.csv");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");
        static TextLoader _textLoader;

        static void Main2(string[] args)
        {
            MLContext mlContext = new MLContext(seed: 0);

            _textLoader = mlContext.Data.CreateTextReader(new TextLoader.Arguments()
            {
                Separator = ",",
                HasHeader = true,
                Column = new[]
                {
                    new TextLoader.Column("BandIndex", DataKind.R8, 0),

                    new TextLoader.Column("AlgorithmDatas", DataKind.R8, new []{new TextLoader.Range(1, 40) })

                    //new TextLoader.Column("HighestPeak1", DataKind.R8, 1),
                    //new TextLoader.Column("HighestPeak2", DataKind.R8, 2),
                    //new TextLoader.Column("HighestPeak3", DataKind.R8, 3),
                    //new TextLoader.Column("HighestPeak4", DataKind.R8, 4),
                    //new TextLoader.Column("HighestPeak5", DataKind.R8, 5),
                    //new TextLoader.Column("HighestPeak6", DataKind.R8, 6),
                    //new TextLoader.Column("HighestPeak7", DataKind.R8, 7),
                    //new TextLoader.Column("HighestPeak8", DataKind.R8, 8),
                    //new TextLoader.Column("HighestPeak9", DataKind.R8, 9),
                    //new TextLoader.Column("HighestPeak10", DataKind.R8, 10),
                    //new TextLoader.Column("HighestPeak11", DataKind.R8, 11),
                    //new TextLoader.Column("HighestPeak12", DataKind.R8, 12),
                    //new TextLoader.Column("HighestPeak13", DataKind.R8, 13),
                    //new TextLoader.Column("HighestPeak14", DataKind.R8, 14),
                    //new TextLoader.Column("HighestPeak15", DataKind.R8, 15),
                    //new TextLoader.Column("HighestPeak16", DataKind.R8, 16),
                    //new TextLoader.Column("HighestPeak17", DataKind.R8, 17),
                    //new TextLoader.Column("HighestPeak18", DataKind.R8, 18),
                    //new TextLoader.Column("HighestPeak19", DataKind.R8, 19),
                    //new TextLoader.Column("HighestPeak20", DataKind.R8, 20),

                    //new TextLoader.Column("HighestSingleChange21", DataKind.R8, 21),
                    //new TextLoader.Column("HighestSingleChange22", DataKind.R8, 22),
                    //new TextLoader.Column("HighestSingleChange23", DataKind.R8, 23),
                    //new TextLoader.Column("HighestSingleChange24", DataKind.R8, 24),
                    //new TextLoader.Column("HighestSingleChange25", DataKind.R8, 25),
                    //new TextLoader.Column("HighestSingleChange26", DataKind.R8, 26),
                    //new TextLoader.Column("HighestSingleChange27", DataKind.R8, 27),
                    //new TextLoader.Column("HighestSingleChange28", DataKind.R8, 28),
                    //new TextLoader.Column("HighestSingleChange29", DataKind.R8, 29),
                    //new TextLoader.Column("HighestSingleChange30", DataKind.R8, 30),
                    //new TextLoader.Column("HighestSingleChange31", DataKind.R8, 31),
                    //new TextLoader.Column("HighestSingleChange32", DataKind.R8, 32),
                    //new TextLoader.Column("HighestSingleChange33", DataKind.R8, 33),
                    //new TextLoader.Column("HighestSingleChange34", DataKind.R8, 34),
                    //new TextLoader.Column("HighestSingleChange35", DataKind.R8, 35),
                    //new TextLoader.Column("HighestSingleChange36", DataKind.R8, 36),
                    //new TextLoader.Column("HighestSingleChange37", DataKind.R8, 37),
                    //new TextLoader.Column("HighestSingleChange38", DataKind.R8, 38),
                    //new TextLoader.Column("HighestSingleChange39", DataKind.R8, 39),
                    //new TextLoader.Column("HighestSingleChange40", DataKind.R8, 40)
                }
            });

            var model = Train(mlContext, _trainDataPath);

            Evaluate(mlContext, model);

            TestSinglePrediction(mlContext);

            Console.Read();
        }

        public static ITransformer Train(MLContext mlContext, string dataPath)
        {
            IDataView dataView = _textLoader.Read(dataPath);
            var pipeline = mlContext.Transforms.CopyColumns("BandIndex", "Label")
                .Append(mlContext.Transforms.Concatenate("Features", "AlgorithmDatas"))
                .Append(mlContext.Regression.Trainers.FastTree());

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

        private static void TestSinglePrediction(MLContext mlContext)
        {
            ITransformer loadedModel;
            using (var stream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                loadedModel = mlContext.Model.Load(stream);
            }

            var predictionFunction = loadedModel.CreatePredictionEngine<BandSelectionData, BandSelectionPrediction>(mlContext);

            var BandSelectionSample = new BandSelectionData()
            {
                BandIndex = 5,
                AlgorithmDatas = new double[40]// To predict. Actual/Observed = 15.5
            };

            var prediction = predictionFunction.Predict(BandSelectionSample);

            Console.WriteLine($"**********************************************************************");
            Console.WriteLine($"Predicted fare: {prediction.BandIndex:0.####}, actual fare: XXX");
            Console.WriteLine($"**********************************************************************");
        }
    }
}
