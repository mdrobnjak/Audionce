using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    class Scale
    {
        public double x, y, z;

        public Scale(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

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

        public static float NextFloat()
        {
            return (float)r.NextDouble();
        }

        public static float NextFloatNeg()
        {
            return r.Next() % 2 == 0 ? (float)r.NextDouble() : -(float)r.NextDouble();
        }
    }

    public static class PictureAnalysis
    {
        public static List<Color> TenMostUsedColors { get; private set; }
        public static List<int> TenMostUsedColorIncidences { get; private set; }

        public static Color MostUsedColor { get; private set; }
        public static int MostUsedColorIncidence { get; private set; }

        private static int pixelColor;

        private static Dictionary<int, int> dctColorIncidence;

        public static void GetMostUsedColor(Bitmap theBitMap)
        {
            TenMostUsedColors = new List<Color>();
            TenMostUsedColorIncidences = new List<int>();

            MostUsedColor = Color.Empty;
            MostUsedColorIncidence = 0;

            // does using Dictionary<int,int> here
            // really pay-off compared to using
            // Dictionary<Color, int> ?

            // would using a SortedDictionary be much slower, or ?

            dctColorIncidence = new Dictionary<int, int>();

            // this is what you want to speed up with unmanaged code
            for (int row = 0; row < theBitMap.Size.Width; row++)
            {
                for (int col = 0; col < theBitMap.Size.Height; col++)
                {
                    Color candidate = theBitMap.GetPixel(row, col);
                    if (!candidate.IsNamedColor) continue;

                    pixelColor = theBitMap.GetPixel(row, col).ToArgb();

                    if (dctColorIncidence.Keys.Contains(pixelColor))
                    {
                        dctColorIncidence[pixelColor]++;
                    }
                    else
                    {
                        dctColorIncidence.Add(pixelColor, 1);
                    }
                }
            }

            // note that there are those who argue that a
            // .NET Generic Dictionary is never guaranteed
            // to be sorted by methods like this
            var dctSortedByValueHighToLow = dctColorIncidence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            // this should be replaced with some elegant Linq ?
            foreach (KeyValuePair<int, int> kvp in dctSortedByValueHighToLow.Take(10))
            {
                TenMostUsedColors.Add(Color.FromArgb(kvp.Key));
                TenMostUsedColorIncidences.Add(kvp.Value);
            }

            MostUsedColor = Color.FromArgb(dctSortedByValueHighToLow.First().Key);
            MostUsedColorIncidence = dctSortedByValueHighToLow.First().Value;
        }

        //public static float pixelHue;
        //private static Dictionary<float, int> dctHueIncidence;

        //public static void GetMostUsedHue(Bitmap theBitMap)
        //{
        //    TenMostUsedColors = new List<Color>();
        //    TenMostUsedColorIncidences = new List<int>();

        //    MostUsedColor = Color.Empty;
        //    MostUsedColorIncidence = 0;

        //    // does using Dictionary<int,int> here
        //    // really pay-off compared to using
        //    // Dictionary<Color, int> ?

        //    // would using a SortedDictionary be much slower, or ?

        //    dctHueIncidence = new Dictionary<float, int>();

        //    // this is what you want to speed up with unmanaged code
        //    for (int row = 0; row < theBitMap.Size.Width; row++)
        //    {
        //        for (int col = 0; col < theBitMap.Size.Height; col++)
        //        {
        //            pixelHue = theBitMap.GetPixel(row, col).GetHue();

        //            if (dctHueIncidence.Keys.Contains(pixelHue))
        //            {
        //                dctHueIncidence[pixelHue]++;
        //            }
        //            else
        //            {
        //                dctHueIncidence.Add(pixelHue, 1);
        //            }
        //        }
        //    }

        //    // note that there are those who argue that a
        //    // .NET Generic Dictionary is never guaranteed
        //    // to be sorted by methods like this
        //    var dctSortedByValueHighToLow = dctHueIncidence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        //    // this should be replaced with some elegant Linq ?
        //    foreach (KeyValuePair<float, int> kvp in dctSortedByValueHighToLow.Take(10))
        //    {
        //        TenMostUsedColors.Add(Color.From(kvp.Key));
        //        TenMostUsedColorIncidences.Add(kvp.Value);
        //    }

        //    MostUsedColor = Color.FromArgb(dctSortedByValueHighToLow.First().Key);
        //    MostUsedColorIncidence = dctSortedByValueHighToLow.First().Value;
        //}

    }
}
