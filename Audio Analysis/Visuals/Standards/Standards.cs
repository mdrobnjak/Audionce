using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    class Position
    {
        public double x, y, z;

        public Position(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    class Angle
    {
        public double x, y, z;

        public Angle(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public static class Rand
    {
        private static Random r = new Random();

        public static double NextDouble()
        {
            return r.NextDouble();
        }

        public static int NextInt()
        {
            return r.Next();
        }

        public static double NextDoubleNeg()
        {
            return r.Next() % 2 == 0 ? r.NextDouble() : -r.NextDouble();
        }
    }
}
