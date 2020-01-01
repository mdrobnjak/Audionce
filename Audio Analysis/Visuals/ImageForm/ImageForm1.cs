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
    public partial class ImageForm1 : Form
    {
        Bitmap bmp;
        List<Color> pixelColors;

        public ImageForm1()
        {
            InitializeComponent();

            bmp = (Bitmap)Bitmap.FromFile(pictureBox1.ImageLocation);
            pixelColors = new List<Color>();

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    pixelColors.Add(bmp.GetPixel(x, y));
                }
            }

            AsciiFilter.Init(bmp, 2);
        }

        /// <summary>
        /// Process using Lockbits, Unsafe, and Parallel.
        /// </summary>
        /// <param name="processedBitmap"></param>
        private void VerticalColorBandDesync(Bitmap processedBitmap, int blueIncrement, int greenIncrement, int redIncrement)
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
                        if (currentLine[x] != (byte)0 &&
                           currentLine[x + 1] != (byte)0 &&
                           currentLine[x + 2] != (byte)0)
                        {
                            int newBlue = currentLine[x] + (int)(blueIncrement * x);
                            int newGreen = currentLine[x + 1] + (int)(greenIncrement * x);
                            int newRed = currentLine[x + 2] + (int)(redIncrement * x);

                            currentLine[x] = (byte)newBlue;
                            currentLine[x + 1] = (byte)newGreen;
                            currentLine[x + 2] = (byte)newRed;
                        }
                    }
                });
                processedBitmap.UnlockBits(bitmapData);
            }
        }

        /// <summary>
        /// Process using Lockbits, Unsafe, and Parallel.
        /// </summary>
        /// <param name="processedBitmap"></param>
        private void Darken(Bitmap processedBitmap, int colorDecrement)
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
                        if (currentLine[x] > 20)
                        {
                            int newBlue = Math.Max(currentLine[x] - colorDecrement, 20);
                            int newGreen = Math.Max(currentLine[x + 1] - colorDecrement, 20);
                            int newRed = Math.Max(currentLine[x + 2] - colorDecrement, 20);

                            currentLine[x] = (byte)newBlue;
                            currentLine[x + 1] = (byte)newGreen;
                            currentLine[x + 2] = (byte)newRed;
                        }
                    }
                });
                processedBitmap.UnlockBits(bitmapData);
            }
        }

        float rate1 = 0;
        float rate2 = 0;
        float rate3 = 0;

        float angle1 = 0;
        float angle2 = 0;
        float angle3 = 0;

        Bitmap displayBuffer;
        int darkenAmount = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (reconnect)
            {
                angle1 += (float)Math.Ceiling(((angle2 % 360) - (angle1 % 360)) / 1);
                angle2 += rate2;
                angle3 += (float)Math.Ceiling(((angle2 % 360) - (angle3 % 360)) / 1);

                reconnect = false;
            }
            else
            {
                angle1 += rate1;
                angle2 += rate2;
                angle3 += rate3;
            }

            displayBuffer = bmp.RotateImage(angle3, angle1, angle2);//.ASCIIFilter(2, 0).TextToImage(new Font("Courier", 10), 1);
            Darken(displayBuffer, darkenAmount);
            pictureBox1.Image = displayBuffer;
            pictureBox2.Image = (Image)pictureBox1.Image.Clone();
            pictureBox2.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);

            pictureBox5.Image = (Image)pictureBox4.Image.Clone();
            pictureBox5.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);

            if (rate1 > 2) rate1 -= 2f;
            if (rate2 > 1) rate2 -= 1f;
            if (rate3 > 3) rate3 -= 3f;

            darkenAmount += 15;
        }

        public void Trigger1()
        {
            rate1 = 17;
            rate2 = 15;
            rate3 = 13;
            timer1_Tick(null, null);
        }

        bool reconnect = false;

        public void Trigger2()
        {
            darkenAmount = 0;
            reconnect = true;
            timer1_Tick(null, null);
        }
    }
}
