using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Windows.Input;

namespace ex1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			colorDialog1.Color = left.Color;
			new_image();
		}

		private Pen right = new Pen(Color.White);
		private Pen left = new Pen(Color.Black);
		private Point start;
		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			start = e.Location;
		}

		//Рисование
		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			Bitmap im = (Bitmap)pictureBox1.Image;
			if (e.Button == MouseButtons.Left)
			{
				using (Graphics g = Graphics.FromImage(im))
					g.DrawLine(left, start, e.Location);
				start = e.Location;
			}
			else if (e.Button == MouseButtons.Right)
			{
				using (Graphics g = Graphics.FromImage(im))
					g.DrawLine(right, start, e.Location);
				start = e.Location;
			}
			pictureBox1.Image = im;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			new_image();
		}

		private void new_image()
		{
			Bitmap im = new Bitmap(pictureBox1.Width, pictureBox1.Height);
			using (Graphics g = Graphics.FromImage(im))
				g.Clear(Color.White);
			
			if (pictureBox1.Image != null)
				pictureBox1.Image.Dispose();
			pictureBox1.Image = im;
		}

		private Bitmap img;
		private void button4_Click(object sender, EventArgs e)
		{
			img = (Bitmap)pictureBox1.Image;
			List<Point> points = find_contour();
			foreach (Point p in points)
				img.SetPixel(p.X, p.Y, Color.Red);  //Смена цвета выделенной границы 
			pictureBox1.Image = img;
		}

		//Поиск правой верхней точки области
		private Point find_right_top(Color start)
		{
			for (int i = img.Width - 1; i >= 0; i--)
				for (int j = 0; j < img.Height; j++)
					if (start != img.GetPixel(i, j))
						return new Point(i, j);
			return new Point(img.Width / 2, img.Height / 2);
		}

		//Смена направления движения по границе связанной области
		private Point change_direction(Point cur, int dir)
		{
			switch (dir)
			{
				case 0: return new Point(cur.X + 1, cur.Y);
				case 1: return new Point(cur.X + 1, cur.Y - 1);
				case 2: return new Point(cur.X, cur.Y - 1);
				case 3: return new Point(cur.X - 1, cur.Y - 1);
				case 4: return new Point(cur.X - 1, cur.Y);
				case 5: return new Point(cur.X - 1, cur.Y + 1);
				case 6: return new Point(cur.X, cur.Y + 1);
				case 7: return new Point(cur.X + 1, cur.Y + 1);
				default: return cur;
			}
		}

		private int dir;
		private Point next_point(Point cur, Color col)
		{
			int start_dir = (dir + 2) % 8;
			Point next_p = change_direction(cur, start_dir);
			int new_dir = start_dir;
			//пока цвет следующего пикселя не цвета границы
			while (img.GetPixel(next_p.X, next_p.Y) != col)
			{
				if (new_dir < 0)
					new_dir += 8;
				next_p = change_direction(cur, new_dir);
				new_dir--;
			}
			dir = new_dir;
			return next_p;
		}

		//Поиск границы связанной области
		private List<Point> find_contour()
		{
			List<Point> points = new List<Point>();
			Point start_point = find_right_top(img.GetPixel(img.Width - 1, 0));
			points.Add(start_point);
			dir = 6;

			Color col = img.GetPixel(start_point.X, start_point.Y);
			Point next_p = next_point(start_point, col);
			points.Add(next_p);
			while (next_p != start_point)
			{
				next_p = next_point(next_p, col);
				points.Add(next_p);
			}
			return points;
		}

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
