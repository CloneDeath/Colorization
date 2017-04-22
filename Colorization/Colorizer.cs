using System;
using System.Drawing;

namespace Colorization
{
    public class Colorizer
    {
        private readonly ColorMask _colorMask;
        private readonly WeightMap _weightMap;
        private readonly MomentumMap _momentumU;
        private readonly MomentumMap _momentumV;

        public Colorizer(ColorMap initialFrame, ColorMask colorMask) {
            _colorMask = colorMask;
            CurrentFrame = initialFrame;
            _weightMap = new WeightMap(initialFrame);
            _momentumU = new MomentumMap(initialFrame.Width, initialFrame.Height);
            _momentumV = new MomentumMap(initialFrame.Width, initialFrame.Height);
        }

        public ColorMap CurrentFrame { get; set; }
        public double LearningRate { get; set; } = 0.05;
        public double Momentum { get; set; } = 0.9;

        public void RunIteration() {
            var nextFrame = CurrentFrame.Copy();

            for (var x = 0; x < nextFrame.Width; x++) {
                for (var y = 0; y < nextFrame.Height; y++) {
                    if (_colorMask[x, y]) continue;
                    
                    var weightedSumOfNeighborsU = GetWeightedSumOfNeighbors(x, y, c => c.U);
                    var uError = CurrentFrame[x, y].U - weightedSumOfNeighborsU;
                    _momentumU[x, y] = (_momentumU[x, y] * Momentum) + (uError * (1 - Momentum));
                    nextFrame[x, y].U -= (_momentumU[x, y] * LearningRate);

                    var weightedSumOfNeighborsV = GetWeightedSumOfNeighbors(x, y, c => c.V);
                    var vError = CurrentFrame[x, y].V - weightedSumOfNeighborsV;
                    _momentumV[x, y] = (_momentumV[x, y] * Momentum) + (vError * (1 - Momentum));
                    nextFrame[x, y].V -= (_momentumV[x, y] * LearningRate);
                }
            }

            CurrentFrame = nextFrame;
        }

        private double GetWeightedSumOfNeighbors(int x, int y, Func<ColorValue, double> component) {
            var center = new Point(x, y);
            var sum = 0.0;
            var totalWeight = 0.0;
            foreach (var point in CurrentFrame.GetNeighbors(x, y)) {
                var u = component(CurrentFrame[point]);
                var weight = _weightMap.GetWeight(center, point);
                totalWeight += weight;
                sum += (u * weight);
            }
            return sum / totalWeight;
        }
    }
}