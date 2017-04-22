using System.Collections.Generic;
using System.Drawing;

namespace Colorization
{
    public class ColorMap : DataMap<ColorValue>
    {
        public ColorMap(int width, int height) : base(width, height) { }
        public ColorMap(string fileName) {
            var bmp = new Bitmap(fileName);
            Width = bmp.Width;
            Height = bmp.Height;
            _map = new ColorValue[Width, Height];
            for (var x = 0; x < Width; x++) {
                for (var y = 0; y < Height; y++) {
                    _map[x, y] = new ColorValue(bmp.GetPixel(x, y));
                }
            }
        }

        public void SaveAs(string resultFile) {
            var bmp = new Bitmap(Width, Height);
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    bmp.SetPixel(x, y, _map[x, y].ToColor());
                }
            }
            bmp.Save(resultFile);
        }

        public ColorMap Copy() {
            var copy = new ColorMap(Width, Height);
            for (var x = 0; x < Width; x++) {
                for (var y = 0; y < Height; y++) {
                    copy[x, y] = _map[x, y].Copy();
                }
            }
            return copy;
        }

    }
}