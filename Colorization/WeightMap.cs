using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Colorization
{
    public class WeightMap
    {
        private readonly ColorMap _initialFrame;

        public WeightMap(ColorMap initialFrame) {
            _initialFrame = initialFrame;
        }
        
        public virtual double GetWeight(Point r, Point s) {
            var yr = _initialFrame[r.X, r.Y].Y;
            var ys = _initialFrame[s.X, s.Y].Y;
            var mean = GetMeanIntensity(r);
            var variance = GetVariance(r);
            if (variance <= 0) return 1;
            return 1 + ((1 / variance) * (yr - mean) * (ys - mean));
        }

        public double GetVariance(Point r) {
            var intensities = GetIntensitiesAroundPoint(r);
            var mean = GetMeanIntensity(r);
            return intensities.Select(i => Math.Pow(i - mean, 2)).Sum() / (intensities.Count);
        }

        public double GetMeanIntensity(Point r) {
            var intensities = GetIntensitiesAroundPoint(r);
            return intensities.Average();
        }

        
        protected virtual List<double> GetIntensitiesAroundPoint(Point r) {
            var intensities = new List<double>();
            foreach (var neighbor in _initialFrame.GetNeighbors(r)) {
                intensities.Add(_initialFrame[neighbor].Y);
            }
            return intensities;
        }
    }
}