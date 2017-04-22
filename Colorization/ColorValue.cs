using System;
using System.Drawing;

namespace Colorization
{
    public class ColorValue
    {
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }

        public double Y {
            get { return (0.257 * R) + (0.504 * G) + (0.098 * B) + (16 / 255.0); }
            set { ResetRgb(ClampValue(value), U, V); }
        }

        public double U {
            get { return -(0.148 * R) - (0.291 * G) + (0.439 * B) + (128 / 255.0); }
            set { ResetRgb(Y, ClampValue(value), V);}
        }

        public double V {
            get { return (0.439 * R) - (0.368 * G) - (0.071 * B) + (128 / 255.0); }
            set { ResetRgb(Y, U, ClampValue(value)); }
        }

        private double ClampValue(double value) {
            return Math.Max(0, Math.Min(1, value));
        }

        private void ResetRgb(double y, double u, double v)
        {
            R = 1.164 * (y - (16 / 255.0)) + 1.596 * (v - (128 / 255.0));
            G = 1.164 * (y - (16 / 255.0)) - 0.813 * (v - (128 / 255.0)) - 0.391 * (u - (128 / 255.0));
            B = 1.164 * (y - (16 / 255.0)) + 2.018 * (u - (128 / 255.0));
        }

        public ColorValue(Color color) {
            R = color.R / 255.0;
            G = color.G / 255.0;
            B = color.B / 255.0;
        }

        public ColorValue(double r, double g, double b) {
            R = r;
            G = g;
            B = b;
        }

        public Color ToColor() {
            return Color.FromArgb(GetColorChannel(R), GetColorChannel(G), GetColorChannel(B));
        }

        private static int GetColorChannel(double channel) {
            return Math.Min(255, Math.Max(0, (int)(channel * 255)));
        }

        public ColorValue Copy() {
            return new ColorValue(R, G, B);
        }

        public bool IsAboutEqualTo(ColorValue other) {
            if (Math.Abs(R - other.R) > (1 / 255.0)) return false;
            if (Math.Abs(G - other.G) > (1 / 255.0)) return false;
            if (Math.Abs(B - other.B) > (1 / 255.0)) return false;
            return true;
        }

        public bool UvIsAboutEqualTo(ColorValue other) {
            if (Math.Abs(U - other.U) > (1 / 255.0)) return false;
            if (Math.Abs(V - other.V) > (1 / 255.0)) return false;
            return true;
        }
    }
}