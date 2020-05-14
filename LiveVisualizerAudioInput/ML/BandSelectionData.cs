using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveVisualizerAudioInput
{
    class BandSelectionData
    {
        [LoadColumn(0)]
        public float BandIndex;

        [LoadColumn(1, 5400)]
        [VectorType(5400)]
        public float[] AudioData;     
    }

    public class BandSelectionPrediction
    {
        [ColumnName("Score")]
        public float BandIndex;
    }
}
