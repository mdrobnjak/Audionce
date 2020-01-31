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
    public partial class ImageForm3 : Form
    {
        Bitmap bmp;

        public ImageForm3()
        {
            InitializeComponent();

            bmp = (Bitmap)Bitmap.FromFile(pictureBox1.ImageLocation);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (jitterCount > 0)
            {
                pictureBox1.Image = bmp.RotateImage(Rand.NextInt() % jitterCount * 4, Rand.NextInt() % jitterCount * 4, Rand.NextInt() % jitterCount * 4);
                jitterCount--;
            }
        }

        int jitterCount = 0;

        public void Trigger1()
        {
            jitterCount = 15;
        }

        public void Trigger2()
        {

        }
    }
}
