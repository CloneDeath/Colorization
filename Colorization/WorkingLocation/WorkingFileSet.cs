using System;
using System.IO;

namespace Colorization.WorkingLocation
{
    public class WorkingFileSet
    {
        public string ProjectName { get; }

        private DateTime _createDate;

        public WorkingFileSet(string projectName, string inputDirectory, string outputDirectory) {
            ProjectName = projectName;
            _createDate = DateTime.Now;

            OutputDirectory = outputDirectory;

            foreach (var file in Directory.GetFiles(inputDirectory)) {
                var baseFileName = Path.GetFileNameWithoutExtension(file).ToLower();
                if (baseFileName == "gray") {
                    GrayFile = file;
                }
                if (baseFileName == "marked") {
                    MarkedFile = file;
                }
            }
        }

        public string GrayFile { get; }
        public string MarkedFile { get; }

        public string ResultFile => GetOutputFile($"result_{DateString}");
        public string MaskFile => GetOutputFile($"mask_{DateString}");

        private string GetOutputFile(string suffix) {
            return Path.Combine(OutputDirectory, suffix + ".png");
        }

        private string OutputDirectory { get; }

        private string SubFolder => Path.Combine(OutputDirectory, $"Render_{DateString}");

        private string DateString => $"{_createDate.Year}.{_createDate.Month:D2}.{_createDate.Day:D2}-{_createDate.Hour:D2}.{_createDate.Minute:D2}";

        public string SmoothFolder => Path.Combine(SubFolder, "Smooth");
        public string GetSmoothFileForIteration(int iteration) {
            return Path.Combine(SmoothFolder, $"frame_{iteration:D5}.png");
        }

        public string FloodFolder => Path.Combine(SubFolder, "Flood");
        public string GetFloodFileForIteration(int iteration)
        {
            return Path.Combine(FloodFolder, $"frame_{iteration:D5}.png");
        }
    }
}