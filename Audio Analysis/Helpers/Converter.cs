using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AudioAnalyzer
{
    public class Converter
    {
        public float _xCenter = 0;
        public float _yCenter = 0;
        private float _maxScaledX = 0;
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

        public Converter(float xCenter, float yCenter, float maxScaledX, float maxScaledY)
        {
            _xCenter = xCenter;
            _yCenter = yCenter;
            _maxScaledX = maxScaledX;
            _maxScaledY = maxScaledY;
        }   
    }
}
