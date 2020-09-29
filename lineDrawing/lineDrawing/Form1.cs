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
            var bad = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            // модифицируем координаты точек, чтобы не было проблем с вычислениями
            // слишком большой угол наклона
            if (bad)
            {
                (x0, y0) = (y0, x0);
                (x1, y1) = (y1, x1);
            }
            // линия растёт не слева направо
            if (x0 > x1)
            {
                (x0, x1) = (x1, x0);
                (y0, y1) = (y1, y0);
            }

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            // вертикальное расстояние между текущим y и точным y для текущего x, с домножением на dx
            // чтобы каждый шаг ошибка изменялась на dy
            int error = dx / 2;

            // направление роста y
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                if (bad)
                {
                    (x, y) = (y, x);
                }
                // возвращаем координаты на свои места, чтобы нарисовать отрезок корректно
                bmp.SetPixel(x, y, color);
                if (bad)
                {
                    (x, y) = (y, x);
                }
                // когда ошибка пересекает рубеж, новый пиксел сдвигается выше
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
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

            DrawPoint(steep, x0, y0, 1); // Эта функция автоматом меняет координаты местами в зависимости от переменной steep
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
