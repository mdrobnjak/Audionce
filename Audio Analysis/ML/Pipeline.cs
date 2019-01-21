using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    class Pipeline
    {
        public Pipeline()
        {
            var mlContext = new MLContext();

            var reader = mlContext.Data.CreateTextReader<BandSelectionData>(separatorChar: ',', hasHeader: true);
            //IDataView trainingDataView = reader.Read("BandSelectionData.csv");

            //var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label")
            //    .Append(mlContext.Transforms.Concatenate("Features", "HighestPeak1", "HighestSingleChange1"))
            //    .Append(mlContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent(labelColumn: "Label", featureColumn: "Features"))
            //    .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            
            var pipeline = mlContext.Transforms.Concatenate("NumFeatures", "HighestPeak1", "HighestSingleChange1", "")
    .Append(mlContext.Transforms.Categorical.OneHotEncoding(inputColumn: "productId", outputColumn: "CatFeatures"))
    .Append(mlContext.Transforms.Concatenate("Features", "NumFeatures", "CatFeatures"))
    .Append(mlContext.Transforms.CopyColumns("BandIndex", "Label"))
    .Append(mlContext.Regression.Trainers.FastTreeTweedie("Label", "Features"));

            //var model = pipeline.Fit(trainingDataView);

            //var prediction = model.CreatePredictionEngine<BandSelectionData, BandSelectionPrediction>(mlContext).Predict(
            //    new BandSelectionData()
            //    {
            //        HighestPeak1 = 0.05,
            //        HighestSingleChange21 = 0.7,
            //    });


        }
    }
}
