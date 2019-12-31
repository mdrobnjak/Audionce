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
    public partial class ImageForm : Form
    {
        Bitmap bmp;
        List<Color> pixelColors;

        public ImageForm()
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
        }

        delegate void ImageCallback();

        public void Animate()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                ImageCallback d = new ImageCallback(Animate);
                this.Invoke(d);
            }
            else
            {
                if (Enabled) ColorScroll();
            }
        }

        byte increment = 1;

        void ColorScroll()
        {
            int i = 0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    bmp.SetPixel(x, y, Color.FromArgb(255, 
                        (pixelColors[i].R + increment) % 255, 
                        (pixelColors[i].G + increment * 2) % 255, 
                        (pixelColors[i].B + increment * 5) % 255));

                    i++;
                }
            }
            
            pictureBox1.Image = bmp;

            pictureBox1.Refresh();

            increment++;
        }

        /// <summary>
        /// Process using Lockbits, Unsafe, and Parallel.
        /// </summary>
        /// <param name="processedBitmap"></param>
        private void ProcessUsingLockbitsAndUnsafeAndParallel(Bitmap processedBitmap)
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
                        int oldBlue = currentLine[x] + (int)(.1 *x);
                        int oldGreen = currentLine[x + 1] + (int)(.11 * x);
                        int oldRed = currentLine[x + 2] + (int)(.12 * x);

                        currentLine[x] = (byte)oldBlue;
                        currentLine[x + 1] = (byte)oldGreen;
                        currentLine[x + 2] = (byte)oldRed;
                    }
                });
                processedBitmap.UnlockBits(bitmapData);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ProcessUsingLockbitsAndUnsafeAndParallel(bmp);
            pictureBox1.Image = bmp;
            //pictureBox1.Refresh();
        }
    }
}
