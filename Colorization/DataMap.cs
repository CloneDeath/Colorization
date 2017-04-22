using System.Collections.Generic;
using System.Drawing;

namespace Colorization
{
    public class DataMap<T>
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        protected T[,] _map;
        
        public T this[int x, int y]
        {
            get { return _map[x, y]; }
            set { _map[x, y] = value; }
        }

        public T this[Point p]
        {
            get { return _map[p.X, p.Y]; }
            set { _map[p.X, p.Y] = value; }
        }

        protected DataMap() { } 
        public DataMap(int width, int height)
        {
            Width = width;
            Height = height;
            _map = new T[width, height];
        }

        public IEnumerable<Point> Points {
            get {
                for (var y = 0; y < Height; y++) {
                    for (var x = 0; x < Width; x++) {
                        yield return new Point(x, y);
                    }
                }
            }
        }


        public bool IsInBounds(int x, int y)
        {
            if (x < 0) return false;
            if (x >= Width) return false;
            if (y < 0) return false;
            if (y >= Height) return false;
            return true;
        }

        public List<Point> GetNeighbors(Point p)
        {
            return GetNeighbors(p.X, p.Y);
        }

        public List<Point> GetNeighbors(int x, int y)
        {
            var results = new List<Point>();
            for (var dx = -1; dx <= 1; dx++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    var nx = dx + x;
                    var ny = dy + y;
                    if (!IsInBounds(nx, ny)) continue;

                    results.Add(new Point(nx, ny));
                }
            }
            return results;
        }
    }
}
