using Microsoft.ML;
using Microsoft.ML.Core.Data;
using System;
using System.IO;
using Microsoft.ML.Data;

namespace AudioAnalyzer
{
    public class ProductModelHelper
    {
        /// <summary>
        /// Train and save model for predicting band selection.
        /// </summary>
        /// <param name="dataPath">Input training file path</param>
        /// <param name="outputModelPath">Trained model path</param>
        public static void TrainAndSaveModel(MLContext mlContext, string dataPath, string outputModelPath = "bandIndex_fastTreeTweedie.zip")
        {
            if (File.Exists(outputModelPath))
            {
                File.Delete(outputModelPath);
            }

            CreateBandSelectionModelUsingPipeline(mlContext, dataPath, outputModelPath);
        }


        /// <summary>
        /// Build model for predicting band selection using Learning Pipelines API
        /// </summary>
        /// <param name="dataPath">Input training file path</param>
        /// <returns></returns>
        private static void CreateBandSelectionModelUsingPipeline(MLContext mlContext, string dataPath, string outputModelPath)
        {
            Console.WriteLine("Training band selection forecasting");
            

            //var reader = mlContext.Data.CreateTextReader<BandSelectionData>(separatorChar: ',', hasHeader: true);

            IDataView trainingDataView = mlContext.Data.CreateTextReader(new TextLoader.Arguments()
            {
                Separator = ",",
                HasHeader = true,
                Column = new[]
                {
                    new TextLoader.Column("BandIndex", DataKind.R8, 0),
                    new TextLoader.Column("HighestPeak1", DataKind.R8, 1),
                    new TextLoader.Column("HighestSingleChange21", DataKind.R8, 21)
                }
            }).Read("BandSelectionData.csv");

            var trainingPipeline = mlContext.Transforms.CopyColumns("BandIndex", "Label")
                .Append(mlContext.Transforms.Concatenate("Features", "HighestPeak1", "HighestSingleChange21"))
                .Append(mlContext.Regression.Trainers.FastTree());

            // Cross-Validate with single dataset (since we don't have two datasets, one for training and for evaluate)
            // in order to evaluate and get the model's accuracy metrics
            Console.WriteLine("=============== Cross-validating to get model's accuracy metrics ===============");
            //var crossValidationResults = mlContext.Regression.CrossValidate(trainingDataView, trainingPipeline, numFolds: 2, labelColumn: "Label");
            //ConsoleHelper.PrintRegressionFoldsAverageMetrics(trainer.ToString(), crossValidationResults);

            // Train the model
            var model = trainingPipeline.Fit(trainingDataView);

            // Save the model for later comsumption from end-user apps
            using (var file = File.OpenWrite(outputModelPath))
                model.SaveTo(mlContext, file);
        }

        /// <summary>
        /// Predict samples using saved model
        /// </summary>
        /// <param name="outputModelPath">Model file path</param>
        /// <returns></returns>
        public static void TestPrediction(MLContext mlContext, string outputModelPath = "bandIndex_fastTreeTweedie.zip")
        {
            //ConsoleWriteHeader("Testing Product Unit Sales Forecast model");

            // Read the model that has been previously saved by the method SaveModel

            ITransformer trainedModel;
            using (var stream = File.OpenRead(outputModelPath))
            {
                trainedModel = mlContext.Model.Load(stream);
            }

            var predictionEngine = trainedModel.CreatePredictionEngine<BandSelectionData, BandSelectionPrediction>(mlContext);

            Console.WriteLine("** Testing Product 1 **");

            // Build sample data
            BandSelectionData dataSample = new BandSelectionData()
            {
                BandIndex = 5,
                HighestPeak1 = 0.05,
                HighestSingleChange21 = .5
            };

            // Predict the nextperiod/month forecast to the one provided
            BandSelectionPrediction prediction = predictionEngine.Predict(dataSample);
            Console.WriteLine($"HighestPeak1: {dataSample.HighestPeak1}, HighestSingleChange21: {dataSample.HighestSingleChange21 } - Real value (bandIndex): , Forecast Prediction (bandIndex): {prediction.BandIndex}");

            dataSample = new BandSelectionData()
            {
                BandIndex = 5,
                HighestPeak1 = 0.05,
                HighestSingleChange21 = .5
            };

            // Predicts the nextperiod/month forecast to the one provided
            prediction = predictionEngine.Predict(dataSample);
            Console.WriteLine($"HighestPeak1: {dataSample.HighestPeak1}, HighestSingleChange21: {dataSample.HighestSingleChange21 } - Real value (bandIndex): , Forecast Prediction (bandIndex): {prediction.BandIndex}");

            Console.WriteLine(" ");

            Console.WriteLine("** Testing Product 2 **");

            dataSample = new BandSelectionData()
            {
                BandIndex = 5,
                HighestPeak1 = 0.05,
                HighestSingleChange21 = .5
            };

            prediction = predictionEngine.Predict(dataSample);
            Console.WriteLine($"HighestPeak1: {dataSample.HighestPeak1}, HighestSingleChange21: {dataSample.HighestSingleChange21 } - Real value (bandIndex): , Forecast Prediction (bandIndex): {prediction.BandIndex}");

            dataSample = new BandSelectionData()
            {
                BandIndex = 5,
                HighestPeak1 = 0.05,
                HighestSingleChange21 = .5
            };

            prediction = predictionEngine.Predict(dataSample);
            Console.WriteLine($"HighestPeak1: {dataSample.HighestPeak1}, HighestSingleChange21: {dataSample.HighestSingleChange21 } - Real value (bandIndex): , Forecast Prediction (bandIndex): {prediction.BandIndex}");
        }
    }
}