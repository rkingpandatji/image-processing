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

namespace image_processing
{
    public partial class Form1 : Form
    {
        Image imgOrignal,newImg,RrotImg;
        //Bitmap bmp1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pbImage.Left = (ClientSize.Width-pbImage.Width)/2;
            pbImage.Top = (ClientSize.Height-pbImage.Height)/2;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() {Filter = "JPG|*.jpg" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pbImage.Image = Image.FromFile(ofd.FileName);
                    imgOrignal = pbImage.Image;
                }
            }
        }

        Image zoom(Image img, Size size)
        {
            Bitmap bmp = new Bitmap(img, img.Width+(img.Width*size.Width/100),img.Height+(img.Height*size.Height/100));
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bmp;
            
        }
        private void trkbr_Zoom_Scroll(object sender, EventArgs e)
        {
            pbImage.Image = zoom(imgOrignal,new Size(trkbr_Zoom.Value,trkbr_Zoom.Value));

           newImg  = pbImage.Image;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pbImage.Image != null)
            {
                pbImage.Dispose();
            }
        }

        Image brigth(Image img, float val)
        {
            float value = val * 0.01f;
            float[][] colorMatrixElements = {

      new float[] {

            1,
            0,
            0,
            0,
            0
      },
      new float[] {
            0,
            1,
            0,
            0,
            0
      },
      new float[] {
            0,
            0,
            1,
            0,
            0
      },
      new float[] {
            0,
            0,
            0,
            1,
            0
      },
      new float[] {
            value,
            value,
            value,
            0,
            1
      }
};
            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            Image _img = img;

            Graphics _g = default(Graphics);

            Bitmap bm_dest = new Bitmap(Convert.ToInt32(_img.Width), Convert.ToInt32(_img.Height));

            _g = Graphics.FromImage(bm_dest);

            _g.DrawImage(_img, new Rectangle(0, 0, bm_dest.Width + 1, bm_dest.Height + 1), 0, 0, bm_dest.Width + 1, bm_dest.Height + 1, GraphicsUnit.Pixel, imageAttributes);

            return bm_dest;
        }
        private void trkbr_bright_Scroll(object sender, EventArgs e)
        {
            pbImage.Image = brigth(imgOrignal, trkbr_bright.Value);

            newImg = pbImage.Image;
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            double contrast = trackBar3.Value;
            Bitmap temp = (Bitmap)imgOrignal;
            Bitmap bmap = (Bitmap)temp.Clone();
            if (contrast < -100) contrast = -100;
            if (contrast > 100) contrast = 100;
            contrast = (100.0 + contrast) / 100.0;
            contrast *= contrast;
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    double pR = c.R / 255.0;
                    pR -= 0.5;
                    pR *= contrast;
                    pR += 0.5;
                    pR *= 255;
                    if (pR < 0) pR = 0;
                    if (pR > 255) pR = 255;

                    double pG = c.G / 255.0;
                    pG -= 0.5;
                    pG *= contrast;
                    pG += 0.5;
                    pG *= 255;
                    if (pG < 0) pG = 0;
                    if (pG > 255) pG = 255;

                    double pB = c.B / 255.0;
                    pB -= 0.5;
                    pB *= contrast;
                    pB += 0.5;
                    pB *= 255;
                    if (pB < 0) pB = 0;
                    if (pB > 255) pB = 255;

                    bmap.SetPixel(i, j,
        Color.FromArgb((byte)pR, (byte)pG, (byte)pB));
                }
            }
           newImg = (Bitmap)bmap.Clone();
            pbImage.Image = newImg;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnRRight_Click(object sender, EventArgs e)
        {
            newImg.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pbImage.Image = newImg;
            newImg = pbImage.Image;

        }

        private void btnRLeft_Click(object sender, EventArgs e)
        {
            newImg.RotateFlip(RotateFlipType.Rotate270FlipNone);
            pbImage.Image = newImg;
            newImg = pbImage.Image;
        }
      
    }
}
