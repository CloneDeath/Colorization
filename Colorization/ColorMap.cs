using System.Collections.Generic;
using System.Drawing;

namespace Colorization
{
    public class ColorMap
    {
        public int Width { get; }
        public int Height { get; }
        private readonly ColorValue[,] _pixels;

        public ColorValue this[int x, int y] {
            get { return _pixels[x, y]; }
            set { _pixels[x, y] = value; }
        }

        public ColorValue this[Point p] {
            get { return this[p.X, p.Y]; }
            set { this[p.X, p.Y] = value; }
        }

        public ColorMap(int width, int height) {
            Width = width;
            Height = height;
            _pixels = new ColorValue[Width, Height];
        }
        public ColorMap(string fileName) {
            var bmp = new Bitmap(fileName);
            Width = bmp.Width;
            Height = bmp.Height;
            _pixels = new ColorValue[Width, Height];
            for (var x = 0; x < Width; x++) {
                for (var y = 0; y < Height; y++) {
                    _pixels[x, y] = new ColorValue(bmp.GetPixel(x, y));
                }
            }
        }

        public void SaveAs(string resultFile) {
            var bmp = new Bitmap(Width, Height);
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    bmp.SetPixel(x, y, _pixels[x, y].ToColor());
                }
            }
            bmp.Save(resultFile);
        }

        public ColorMap Copy() {
            var copy = new ColorMap(Width, Height);
            for (var x = 0; x < Width; x++) {
                for (var y = 0; y < Height; y++) {
                    copy[x, y] = _pixels[x, y].Copy();
                }
            }
            return copy;
        }

        public bool IsInBounds(int x, int y) {
            if (x < 0) return false;
            if (x >= Width) return false;
            if (y < 0) return false;
            if (y >= Height) return false;
            return true;
        }

        public List<Point> GetNeighbors(Point p) {
            return GetNeighbors(p.X, p.Y);
        } 

        public List<Point> GetNeighbors(int x, int y) {
            var results = new List<Point>();
            for (var dx = -1; dx <= 1; dx++) {
                for (var dy = -1; dy <= 1; dy++) {
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