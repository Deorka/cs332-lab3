using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lineDrawing
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        Random rnd = new Random();
        Color color;
        bool firstPoint = false;
        bool bres = true;
        bool wu = false;
        int x0 = -1;
        int y0 = -1;
        int x1 = -1;
        int y1 = -1;
        public Form1()
        {
            InitializeComponent();
            int wdth = pictureBox1.Width;
            int hght = pictureBox1.Height;
            bmp = new Bitmap(wdth, hght);
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        public void Bresenham_line_algorithm(int x0, int y0, int x1, int y1)
        {
            if (x0 > x1)
            {
                (x0, x1) = (x1, x0);
                (y0, y1) = (y1, y0);
            }
            int dy = y1 - y0;
            int dx = x1 - x0;
            int xi = x0;
            int yi = y0;
            int di = 2 * dy - dx;
            int step = 1;
            double grad = Math.Abs((double)dy / dx);

            // for a line with gradient <= 1
            if (dx == 0 || grad <= 1)
            {
                // если убывает
                if ((double)dy / dx < 0)
                {
                    step = -1;
                    dy = -dy;
                }
                for (int x = x0; x < x1; x++)
                {
                    bmp.SetPixel(x, yi, color);
                    // если не нужно поднимать y
                    if (di < 0)
                    {
                        di += 2 * dy;
                    }
                    // если нужно поднимать y
                    else if (di >= 0)
                    {
                        yi += step;
                        di += 2 * (dy - dx);
                    }
                }
            }
            // for a line with gradient > 1
            else if (grad > 1)
            {
                // если убывает
                if ((double)dy / dx < 0)
                {
                    step = -1;
                    dy = -dy;
                    // так как проходим по y
                    xi = x1;
                    (y0, y1) = (y1, y0);
                }
                for (int y = y0; y < y1; y++)
                {
                    // если не нужно поднимать x
                    bmp.SetPixel(xi, y, color);
                    if (di < 0)
                    {
                        di += 2 * dx;
                    }
                    // если нужно поднимать x
                    else if (di >= 0)
                    {
                        xi += step;
                        di += 2 * (dx - dy);
                    }
                }
            }
        }
        public void WuLine(int x0, int y0, int x1, int y1)
        {
            var steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                (x0, y0) = (y0, x0);
                (x1, y1) = (y1, x1);
            }
            if (x0 > x1)
            {
                (x0, x1) = (x1, x0);
                (y0, y1) = (y1, y0);
            }

            DrawPoint(steep, x0, y0, 1);
            DrawPoint(steep, x1, y1, 1); // Последний аргумент — интенсивность в долях единицы
            float dx = x1 - x0;
            float dy = y1 - y0;
            float gradient = dy / dx;
            float y = y0 + gradient;
            for (var x = x0 + 1; x <= x1 - 1; x++)
            {
                DrawPoint(steep, x, (int)y, (int)((1 - (y - (int)y)) * 255));
                DrawPoint(steep, x, (int)y + 1, (int)((y - (int)y) * 255));
                y += gradient;
            }
        }

        public void DrawPoint(bool steep, int x, int y, int alpha)
        {
            if (steep)
            {
                (x, y) = (y, x);
            }
            Color color = Color.FromArgb(alpha, 0, 0, 0);
            bmp.SetPixel(x, y, color);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = bmp;
            color = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

            if (firstPoint == false)
            {
                x0 = e.X;
                y0 = e.Y;
                firstPoint = true;
            }
            else if (bres == true)
            {
                x1 = e.X;
                y1 = e.Y;
                Bresenham_line_algorithm(x0, y0, x1, y1);
                firstPoint = false;
            }
            else if (wu == true)
            {
                x1 = e.X;
                y1 = e.Y;
                WuLine(x0, y0, x1, y1);
                firstPoint = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            bres = true;
            wu = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            bres = false;
            wu = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int wdth = pictureBox1.Width;
            int hght = pictureBox1.Height;
            bmp = new Bitmap(wdth, hght);
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }
    }
}
