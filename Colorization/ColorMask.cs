using System.Drawing;

namespace Colorization
{
    public class ColorMask
    {
        public int Width { get; }
        public int Height { get; }

        private readonly bool[,] _mask;

        public bool this[int x, int y] {
            get { return _mask[x, y]; }
            set { _mask[x, y] = value; }
        }

        public bool this[Point p] {
            get { return _mask[p.X, p.Y]; }
            set { _mask[p.X, p.Y] = value; }
        }

        public ColorMask(int width, int height) {
            Width = width;
            Height = height;
            _mask = new bool[width, height];
        }

        public void SaveAs(string filename) {
            var bmp = new Bitmap(Width, Height);
            for (var x = 0; x < Width; x++) {
                for (var y = 0; y < Height; y++) {
                    var mask = _mask[x, y] ? 255 : 0;
                    bmp.SetPixel(x, y, Color.FromArgb(mask, mask, mask));
                }
            }
            bmp.Save(filename);
        }

        public ColorMask Copy() {
            var copy = new ColorMask(Width, Height);
            for (var x = 0; x < Width; x++) {
                for (var y = 0; y < Height; y++) {
                    copy[x, y] = _mask[x, y];
                }
            }
            return copy;
        }
    }
}