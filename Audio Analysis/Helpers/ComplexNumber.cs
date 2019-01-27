using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    public class ComplexNumber
    {
        public float R;
        public float I;

        public ComplexNumber(float real, float img)
        {
            R = real;
            I = img;
        }

        public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
        {
            return new ComplexNumber(a.R * b.R - a.I * b.I, a.R * b.I + a.I * b.R);
        }
        public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
        {
            return new ComplexNumber(a.R + b.R, a.I + b.I);
        }
        public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
        {
            return new ComplexNumber(a.R - b.R, a.I - b.I);
        }

        public static ComplexNumber FromPolar(float length, float angle)
        {
            return new ComplexNumber((float)(length * Math.Cos(angle)), (float)(length * Math.Sin(angle)));
        }
        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt(Math.Pow(R, 2) + Math.Pow(I, 2));
            }
        }
    }
}
