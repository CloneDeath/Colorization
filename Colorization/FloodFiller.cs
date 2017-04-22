﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Colorization
{
    public class FloodFiller
    {
        private readonly ColorMask _colorMask;
        public ColorMap CurrentMap { get; set; }
        private readonly WeightMap _weightMap;

        public FloodFiller(ColorMap initialMap, ColorMask colorMask) {
            _colorMask = colorMask.Copy();
            CurrentMap = initialMap.Copy();
            _weightMap = new WeightMap(initialMap);
        }

        public void Run(WorkingFileSet workingSet) {
            Directory.CreateDirectory(workingSet.FloodFolder);

            var iteration = 0;
            for (var y = 0; y < CurrentMap.Height; y++)
            {
                for (var x = 0; x < CurrentMap.Width; x++) {
                    if (_colorMask[x, y]) continue;

                    Console.WriteLine($"Running Flood Iteration {iteration}...");
                    ExecuteDfs(new Point(x, y));
                    
                    iteration++;
                }

                CurrentMap.SaveAs(workingSet.GetFloodFileForIteration(y));
            }
        }

        private void ExecuteDfs(Point startingPoint) {
            var alreadyVisited = new List<Point>();
            var visitStack = new Dictionary<Point, double>();
            visitStack[startingPoint] = 1;

            while (visitStack.Any()) {
                var current = visitStack.OrderByDescending(kvp => kvp.Value).First().Key;
                visitStack.Remove(current);

                if (alreadyVisited.Contains(current)) continue;
                alreadyVisited.Add(current);

                if (_colorMask[current]) {
                    foreach (var node in alreadyVisited)
                    {
                        CurrentMap[node].U = CurrentMap[current].U;
                        CurrentMap[node].V = CurrentMap[current].V;
                        _colorMask[node] = true;
                    }
                    return;
                }

                foreach (var neighbor in CurrentMap.GetNeighbors(current)) {
                    var weight = _weightMap.GetWeight(current, neighbor);
                    if (!visitStack.ContainsKey(neighbor)) {
                        visitStack[neighbor] = weight;
                    }
                    else {
                        if (visitStack[neighbor] < weight) {
                            visitStack[neighbor] = weight;
                        }
                    }
                }
            }
        }
    }
}