using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Delaunay_triangulation
{
    public class Edge
    {
        public Point p1, p2;

        public Edge(Point _p1, Point _p2)
        {
            p1 = _p1;
            p2 = _p2;
        }

        public Point getStartPoint()
        {
            return p1;
        }
        
        public Point getEndPoint()
        {
            return p2;
        }

        public Edge reverse()
        {
            return new Edge(p2, p1);
        }
        
        public override bool Equals(Object obj)
        {
            Edge otherEdge = obj as Edge;
            if (otherEdge == null)
                return false;
            else
                return this.p1 == otherEdge.p1 && this.p2 == otherEdge.p2;
        }       
    }
}
