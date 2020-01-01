using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioAnalyzer
{
    public partial class ImageForm2 : Form
    {
        Bitmap bmp;

        public ImageForm2()
        {
            InitializeComponent();

            bmp = (Bitmap)Bitmap.FromFile(pictureBox1.ImageLocation);
        }

        /// <summary>
        /// Process using Lockbits, Unsafe, and Parallel.
        /// </summary>
        /// <param name="processedBitmap"></param>
        private void RedGreenScroll(Bitmap processedBitmap, int colorIncrement, int colorDecrement)
        {
            unsafe
            {
                BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite, processedBitmap.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        int newGreen = (currentLine[x + 1] + colorIncrement) % 256;
                        int newRed = (currentLine[x + 2] + colorIncrement) % 256;

                        if (currentLine[x] > 20)
                        {
                            newGreen = Math.Max(newGreen - colorDecrement, 20);
                            newRed = Math.Max(newRed - colorDecrement, 20);

                            int newBlue = Math.Max(currentLine[x] - colorDecrement, 20);
                            currentLine[x] = (byte)newBlue;

                        }
                        currentLine[x + 1] = (byte)newGreen;
                        currentLine[x + 2] = (byte)newRed;
                    }
                });
                processedBitmap.UnlockBits(bitmapData);
            }
        }

        /// <summary>
        /// Process using Lockbits, Unsafe, and Parallel.
        /// </summary>
        /// <param name="processedBitmap"></param>
        private void BlueGreenScroll(Bitmap processedBitmap, int colorIncrement)
        {
            unsafe
            {
                BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite, processedBitmap.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        int newBlue = (currentLine[x] + colorIncrement) % 256;
                        int newGreen = (currentLine[x+1] + colorIncrement) % 256;
                        currentLine[x] = (byte)newBlue;
                        currentLine[x+1] = (byte)newGreen;
                    }
                });
                processedBitmap.UnlockBits(bitmapData);
            }
        }

        Bitmap displayBuffer;
        int colorScrollAmount = 0;
        int colorScrollIncrement = 1;
        int darkenAmount = 10;

        private void timer1_Tick(object sender, EventArgs e)
        {
            displayBuffer = (Bitmap)bmp.Clone();
            colorScrollAmount += colorScrollIncrement;
            blueScrollAmount += blueScrollIncrement;

            RedGreenScroll(displayBuffer, colorScrollAmount, darkenAmount);
            BlueGreenScroll(displayBuffer, blueScrollAmount);
            pictureBox1.Image = displayBuffer;
        }

        public void Trigger1()
        {
            blueScrollIncrement = 0;
            colorScrollIncrement = 2;
            colorScrollAmount += 20;
            timer1_Tick(null, null);
        }
        
        int blueScrollAmount = 0;
        int blueScrollIncrement = -1;

        public void Trigger2()
        {
            colorScrollIncrement = 0;
            blueScrollIncrement = -2;
            blueScrollAmount += -20;
            timer1_Tick(null, null);
        }
    }
}
