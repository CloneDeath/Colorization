using System.Drawing;

namespace Colorization
{
    public class ColorMask : DataMap<bool>
    {
        public void SaveAs(string filename) {
            var bmp = new Bitmap(Width, Height);
            for (var x = 0; x < Width; x++) {
                for (var y = 0; y < Height; y++) {
                    var mask = _map[x, y] ? 255 : 0;
                    bmp.SetPixel(x, y, Color.FromArgb(mask, mask, mask));
                }
            }
            bmp.Save(filename);
        }

        public ColorMask Copy() {
            var copy = new ColorMask(Width, Height);
            for (var x = 0; x < Width; x++) {
                for (var y = 0; y < Height; y++) {
                    copy[x, y] = _map[x, y];
                }
            }
            return copy;
        }

        public ColorMask(int width, int height) : base(width, height) {}
    }
}