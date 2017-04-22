using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Colorization
{
    public class Colorizer
    {
        private readonly WeightMap _weightMap;
        private ColorMask _needsUpdate;

        public Colorizer(ColorMap initialFrame) {
            CurrentFrame = initialFrame;
            _weightMap = new WeightMap(initialFrame);

            _needsUpdate = new ColorMask(initialFrame.Width, initialFrame.Height);
            foreach (var pixel in _needsUpdate.Points) {
                _needsUpdate[pixel] = true;
            }
        }

        public ColorMap CurrentFrame { get; set; }
        public double LearningRate { get; set; } = 0.05;
        public double Momentum { get; set; } = 0.9;

        public void RunIteration() {
            var nextFrame = CurrentFrame.Copy();
            var nextNeedsUpdate = new ColorMask(_needsUpdate.Width, _needsUpdate.Height);

            for (var y = 0; y < nextFrame.Height; y++) {
                for (var x = 0; x < nextFrame.Width; x++) {
                    if (_needsUpdate[x, y] == false) continue;

                    var neighbors = CurrentFrame.GetNeighbors(x, y);
                    var originalColor = CurrentFrame[x, y].Copy();

                    var possibleColors = GetColorChoices(neighbors, originalColor.Y);
                    var bestColor = originalColor.Copy();
                    var bestError = GetTotalSumOfErrors(x, y);

                    var needsUpdate = false;

                    foreach (var possibleColor in possibleColors) {
                        CurrentFrame[x, y] = possibleColor;
                        var error = GetTotalSumOfErrors(x, y);
                        if (error >= bestError) continue;

                        needsUpdate = true;
                        bestError = error;
                        bestColor = possibleColor.Copy();
                    }

                    if (needsUpdate) {
                        foreach (var neighbor in neighbors)
                        {
                            nextNeedsUpdate[neighbor] = true;
                        }

                        CurrentFrame[x, y] = originalColor;
                        nextFrame[x, y] = bestColor;
                    }
                }
            }

            _needsUpdate = nextNeedsUpdate;
            CurrentFrame = nextFrame;
        }

        private IEnumerable<ColorValue> GetColorChoices(List<Point> neighbors, double intensity) {
            var choices = neighbors.Select(n => CurrentFrame[n].Copy());
            foreach (var colorValue in choices) {
                colorValue.Y = intensity;
            }
            return choices;
        }

        private double GetTotalSumOfErrors(int x, int y) {
            var weightedSumOfNeighborsU = GetWeightedSumOfNeighbors(x, y, c => c.U);
            var weightedSumOfNeighborsV = GetWeightedSumOfNeighbors(x, y, c => c.V);
            return weightedSumOfNeighborsV + weightedSumOfNeighborsU;
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