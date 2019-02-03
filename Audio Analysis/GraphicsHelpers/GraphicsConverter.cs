using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AudioAnalyzer
{
    public class GraphicsConverter
    {

        public float _containerHeight = 0;
        private float _maxScaledY = 0;

        public float MaxScaledY
        {
            get
            {
                return _maxScaledY;
            }
            set
            {
                _maxScaledY = value;
            }
        }

        public GraphicsConverter(float containerHeight, float maxScaledY)
        {
            _containerHeight = containerHeight;
            _maxScaledY = maxScaledY;
        }   
    }
}
