namespace Colorization
{
    public class MomentumMap
    {
        public int Width { get; }
        public int Height { get; }

        private readonly double[,] _momentum;

        public MomentumMap(int width, int height) {
            Width = width;
            Height = height;

            _momentum = new double[Width, Height];
        }

        public double this[int x, int y] {
            get { return _momentum[x, y]; }
            set { _momentum[x, y] = value; }
        }
    }
}
