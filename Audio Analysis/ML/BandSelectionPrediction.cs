using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    class BandSelectionPrediction
    {
        [ColumnName("Score")]
        public float BandIndex;
    }
}
