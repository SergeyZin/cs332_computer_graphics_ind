using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Delaunay_triangulation
{
    public partial class Form1 : Form
    {
        private List<Point> points = new List<Point>();
        private List<Edge> edges = new List<Edge>();
        private List<Edge> living_edges = new List<Edge>();
        private SolidBrush brush1 = new SolidBrush(Color.Red);
        private SolidBrush brush2 = new SolidBrush(Color.Blue);
        private Pen pen = new Pen(Color.Black, 1);
        private Graphics g;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.White);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            points.Add(e.Location);
            g.FillEllipse(brush1, e.X - 3, e.Y - 3, 7, 7);
            pictureBox1.Invalidate();
        }

        private void drawEdge(Edge e)
        {
            g.DrawLine(pen, e.p1, e.p2);
        }

        private void drawPoint(Point p)
        {
            g.FillEllipse(brush2, p.X - 3, p.Y - 3, 7, 7);
        }

        private void redrawTriangulation()
        {
            g.Clear(Color.White);

            foreach (Point p in points)
                drawPoint(p);

            foreach (Edge e in edges)
                drawEdge(e);
        }

        // Угол между рёбрами
        private double angleBetweenEdges(Edge e1, Edge e2)
        {
            int e1X = e1.p2.X - e1.p1.X;
            int e1Y = e1.p2.Y - e1.p1.Y;
            int e2X = e2.p2.X - e2.p1.X;
            int e2Y = e2.p2.Y - e2.p1.Y;

            double angle = Math.Acos((e1X * e2X + e1Y * e2Y) / (Math.Sqrt(e1X * e1X + e1Y * e1Y) * Math.Sqrt(e2X * e2X + e2Y * e2Y))) * (180 / Math.PI);

            return angle;
        }

        private int pointPos(Point p, Edge e)
        {
            double pos = (e.p2.Y - e.p1.Y) * p.X + (e.p1.X - e.p2.X) * p.Y + (e.p1.Y - e.p2.Y) * e.p1.X + (e.p2.X - e.p1.X) * e.p1.Y;
            if (pos < 0)
                return -1; // точка справа
            else if (pos > 0)
                return 1; // точка слева
            else
                return 0; // точка на ребре
        }

        // Поиск первой точки
        private Point findFirstPoint(List<Point> pts)
        {
            int xMin = pictureBox1.Width;
            int yMin = pictureBox1.Height; 
            Point firstPoint = pts[0];
            foreach (Point p in pts)
            {
                if (p.X < xMin)
                {
                    xMin = p.X;
                    firstPoint = p;
                }
            }
            return firstPoint;
        }

        // Поиск второй точки
        private Point findSecondPoint(List<Point> pts, Point firstPoint)
        {
            double minAng = 180;
            Point secondPoint = firstPoint;
            foreach (Point p in pts)
            {
                Edge e1 = new Edge(firstPoint, new Point(firstPoint.X, firstPoint.Y - 1));
                Edge e2 = new Edge(firstPoint, p);
                double angle = angleBetweenEdges(e1, e2);
                if (angle < minAng)
                {
                    minAng = angle;
                    secondPoint = p;
                }
            }
            return secondPoint;
        }

        // Триангуляция Делоне
        private void triangBtn_Click(object sender, EventArgs e)
        {
            redrawTriangulation();

            Point firstPoint = findFirstPoint(points);
            Point secondPoint = findSecondPoint(points, firstPoint);

            drawPoint(firstPoint);
            drawPoint(secondPoint);

            living_edges.Add(new Edge(firstPoint, secondPoint));

            while (living_edges.Count() > 0)
            {
                int i = living_edges.Count() - 1;
                Edge alive = living_edges[i];
                living_edges.RemoveAt(i);

                Point thirdPoint = Point.Empty;
                double radius = double.MaxValue;

                foreach (Point p in points)
                {
                    if (pointPos(p, alive) == -1)
                    {
                        Triangle triang = new Triangle(alive.p1, alive.p2, p);
                        if (triang.dist < radius)
                        {
                            radius = triang.dist;
                            thirdPoint = p;
                        }
                    }
                    drawPoint(p);
                }

                drawEdge(alive);

                if (thirdPoint != Point.Empty)
                {
                    Edge edge = new Edge(alive.p1, thirdPoint);
                    int ind = living_edges.FindIndex(new Predicate<Edge>(_edge => _edge.Equals(edge.reverse())));
                    if (ind >= 0)
                    {
                        drawEdge(living_edges[ind]);
                        living_edges.RemoveAt(ind);
                    }
                    else
                        living_edges.Add(edge);

                    edge = new Edge(thirdPoint, alive.p2);
                    ind = living_edges.FindIndex(new Predicate<Edge>(_edge => _edge.Equals(edge.reverse())));
                    if (ind >= 0)
                    {
                        drawEdge(living_edges[ind]);
                        living_edges.RemoveAt(ind);
                    }
                    else
                        living_edges.Add(edge);
                }
            }
            pictureBox1.Invalidate();
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            points.Clear();
            edges.Clear();
            pictureBox1.Invalidate();
        }
    }
}
