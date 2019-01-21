using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    class BandSelectionData
    {
        [LoadColumn(0)]
        public float BandIndex;

        [LoadColumn(1, 100)]
        [VectorType(100)]
        public float[] AlgorithmDatas;     
    }

    public class BandSelectionPrediction
    {
        [ColumnName("Score")]
        public float BandIndex;
    }
}
