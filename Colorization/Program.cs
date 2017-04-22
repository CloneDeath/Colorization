using System;
using System.Diagnostics;
using System.IO;
using Colorization.WorkingLocation;

namespace Colorization
{
    public class Program
    {
        public static void Main(string[] args) {
            var directory = new WorkingDirectory(@"images\");

            foreach (var workingSet in directory.GetWorkingFileSets()) {
                Console.WriteLine($"Beginning Work on {workingSet.ProjectName}");
                GenerateOutputForWorkingSet(workingSet);
            }
            
            Console.WriteLine("Complete!");
            Console.ReadLine();
        }

        private static void GenerateOutputForWorkingSet(WorkingFileSet workingSet) {
            Console.WriteLine("Loading Grayscale Image...");
            var startingImage = new ColorMap(workingSet.GrayFile);
            Console.WriteLine("Loading Marked Image...");
            var markedImage = new ColorMap(workingSet.MarkedFile);

            var result = ColorizeImage(startingImage, markedImage, workingSet);

            Console.WriteLine("Saving Results...");
            if (File.Exists(workingSet.ResultFile)) File.Delete(workingSet.ResultFile);
            result.SaveAs(workingSet.ResultFile);
        }

        private static ColorMap ColorizeImage(ColorMap startingImage, ColorMap markedImage, WorkingFileSet workingSet) {
            Directory.CreateDirectory(workingSet.SmoothFolder);

            var colorMask = GetColorMask(startingImage, markedImage);
            colorMask.SaveAs(workingSet.MaskFile);

            var initialFrame = GetInitialFrame(startingImage, markedImage, colorMask, workingSet);

            var colorizer = new Colorizer(initialFrame);
            for (var i = 0; i < 20; i++) {
                Console.WriteLine($"Running Colorizer Iteration {i}...");
                var dt = Stopwatch.StartNew();
                colorizer.RunIteration();
                colorizer.CurrentFrame.SaveAs(workingSet.GetSmoothFileForIteration(i));
                Console.WriteLine($"Finished Iteration {i}. Duration: {dt.Elapsed}");
            }
            return colorizer.CurrentFrame;
        }

        private static ColorMask GetColorMask(ColorMap startingImage, ColorMap markedImage) {
            var mask = new ColorMask(markedImage.Width, markedImage.Height);
            for (var x = 0; x < mask.Width; x++) {
                for (var y = 0; y < mask.Height; y++) {
                    var starting = startingImage[x, y];
                    var marked = markedImage[x, y];

                    if (starting.IsAboutEqualTo(marked)) continue;
                    mask[x, y] = true;
                }
            }
            return mask;
        }

        private static ColorMap GetInitialFrame(ColorMap startingImage, ColorMap markedImage, ColorMask colorMask, WorkingFileSet workingSet) {
            for (var x = 0; x < startingImage.Width; x++) {
                for (var y = 0; y < startingImage.Height; y++) {
                    startingImage[x, y].U = markedImage[x, y].U;
                    startingImage[x, y].V = markedImage[x, y].V;
                }
            }

            var floodFiller = new FloodFiller(startingImage, colorMask);
            floodFiller.Run(workingSet);
            return floodFiller.CurrentMap;
        }
    }
}
