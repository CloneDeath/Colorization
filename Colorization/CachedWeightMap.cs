using System.Collections.Generic;
using System.Drawing;

namespace Colorization
{
    public class CachedWeightMap : WeightMap
    {
        public CachedWeightMap(ColorMap initialFrame)
                : base(initialFrame) {}

        private readonly Dictionary<Point, Dictionary<Point, double>> _weightCache = 
            new Dictionary<Point, Dictionary<Point, double>>();
        public override double GetWeight(Point r, Point s) {
            if (!_weightCache.ContainsKey(r))
            {
                _weightCache[r] = new Dictionary<Point, double>();
            }
            var rCache = _weightCache[r];
            if (rCache.ContainsKey(s)) return rCache[s];

            var weight = base.GetWeight(r, s);

            rCache[s] = weight;
            return weight;
        }

        private readonly Dictionary<Point, List<double>> _intensityCache = new Dictionary<Point, List<double>>();
        protected override List<double> GetIntensitiesAroundPoint(Point r) {
            if (_intensityCache.ContainsKey(r))
            {
                return _intensityCache[r];
            }
            var intensities = base.GetIntensitiesAroundPoint(r);
            _intensityCache[r] = intensities;
            return intensities;
        }
    }
}
