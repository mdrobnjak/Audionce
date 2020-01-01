using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    public static class AsciiFilter
    {
        private static int rows;
        private static int columns;

        public static void Init(Bitmap sourceBitmap, int pixelBlockSize)
        {
            rows = sourceBitmap.Height / pixelBlockSize;
            columns = sourceBitmap.Width / pixelBlockSize;
        }

        public static Bitmap TextToImage(this string text, Font font,
                                                   float factor)
        {
            Bitmap textBitmap = new Bitmap(1, 1);


            Graphics graphics = Graphics.FromImage(textBitmap);


            int width = (int)Math.Ceiling(
                        graphics.MeasureString(text, font).Width *
                        factor);


            int height = (int)Math.Ceiling(
                         graphics.MeasureString(text, font).Height *
                         factor);


            graphics.Dispose();


            textBitmap = new Bitmap(width, height,
                                    PixelFormat.Format32bppArgb);


            graphics = Graphics.FromImage(textBitmap);
            graphics.Clear(Color.Black);


            //graphics.CompositingQuality = CompositingQuality.HighQuality;
            //graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //graphics.SmoothingMode = SmoothingMode.HighQuality;
            //graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;


            graphics.ScaleTransform(factor, factor);
            graphics.DrawString(text, font, System.Drawing.Brushes.White, new PointF(0, 0));


            graphics.Flush();
            graphics.Dispose();


            return textBitmap;
        }

        public static string ASCIIFilter(this Bitmap sourceBitmap, int pixelBlockSize,
                                                              int colorCount = 0)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                    sourceBitmap.Width, sourceBitmap.Height),
                                                      ImageLockMode.ReadOnly,
                                                PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];


            System.Runtime.InteropServices.Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);


            StringBuilder asciiArt = new StringBuilder();


            int avgBlue = 0;
            int avgGreen = 0;
            int avgRed = 0;
            int offset = 0;


            //int rows = sourceBitmap.Height / pixelBlockSize;
            //int columns = sourceBitmap.Width / pixelBlockSize;


            if (colorCount > 0)
            {
                colorCharacters = GenerateRandomString(colorCount);
            }


            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    avgBlue = 0;
                    avgGreen = 0;
                    avgRed = 0;


                    for (int pY = 0; pY < pixelBlockSize; pY++)
                    {
                        for (int pX = 0; pX < pixelBlockSize; pX++)
                        {
                            offset = y * pixelBlockSize * sourceData.Stride +
                                     x * pixelBlockSize * 4;


                            offset += pY * sourceData.Stride;
                            offset += pX * 4;


                            avgBlue += pixelBuffer[offset];
                            avgGreen += pixelBuffer[offset + 1];
                            avgRed += pixelBuffer[offset + 2];
                        }
                    }


                    avgBlue = avgBlue / (pixelBlockSize * pixelBlockSize);
                    avgGreen = avgGreen / (pixelBlockSize * pixelBlockSize);
                    avgRed = avgRed / (pixelBlockSize * pixelBlockSize);


                    asciiArt.Append(GetColorCharacter(avgBlue, avgGreen, avgRed));
                }


                asciiArt.Append("\r\n");
            }


            return asciiArt.ToString();
        }

        //private static string colorCharacters = " !\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
        //private static string colorCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string colorCharacters = "_$@!#%&*?";


        private static string GetColorCharacter(int blue, int green, int red)
        {
            string colorChar = String.Empty;
            int intensity = (blue + green + red) / 3 *
                            (colorCharacters.Length - 1) / 255;


            colorChar = colorCharacters.Substring(intensity, 1).ToUpper();
            colorChar += colorChar.ToLower();


            return colorChar;
        }

        public static string RandomStringSort(this string stringValue)
        {
            char[] charArray = stringValue.ToCharArray();


            Random randomIndex = new Random((byte)charArray[0]);
            int iterator = charArray.Length;


            while (iterator > 1)
            {
                iterator -= 1;


                int nextIndex = randomIndex.Next(iterator + 1);


                char nextValue = charArray[nextIndex];
                charArray[nextIndex] = charArray[iterator];
                charArray[iterator] = nextValue;
            }


            return new string(charArray);
        }

        private static string GenerateRandomString(int maxSize)
        {
            StringBuilder stringBuilder = new StringBuilder(maxSize);
            Random randomChar = new Random();


            char charValue;


            for (int k = 0; k < maxSize; k++)
            {
                charValue = (char)(Math.Floor(255 * randomChar.NextDouble() * 4));


                if (stringBuilder.ToString().IndexOf(charValue) != -1)
                {
                    charValue = (char)(Math.Floor((byte)charValue *
                                            randomChar.NextDouble()));
                }


                if (Char.IsControl(charValue) == false &&
                    Char.IsPunctuation(charValue) == false &&
                    stringBuilder.ToString().IndexOf(charValue) == -1)
                {
                    stringBuilder.Append(charValue);

                    randomChar = new Random((int)((byte)charValue *
                                     (k + 1) + DateTime.Now.Ticks));
                }
                else
                {
                    randomChar = new Random((int)((byte)charValue *
                                     (k + 1) + DateTime.UtcNow.Ticks));
                    k -= 1;
                }
            }


            return stringBuilder.ToString().RandomStringSort();
        }
    }
}
