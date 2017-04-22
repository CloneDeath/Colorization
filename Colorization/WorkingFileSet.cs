using System;
using System.IO;

namespace Colorization
{
    public class WorkingFileSet
    {
        private DateTime _createDate;

        public WorkingFileSet(string originalFile) {
            GrayFile = originalFile;
            _createDate = DateTime.Now;
        }

        public string GrayFile { get; }

        public string MarkedFile => GetFileWithSuffix("m");

        public string ResultFile => GetFileWithSuffix("res");

        public string MaskFile => GetFileWithSuffix("mask");

        private string GetFileWithSuffix(string suffix) {
            return Path.Combine(BaseFolder, Path.GetFileNameWithoutExtension(GrayFile) + "_" + suffix + Path.GetExtension(GrayFile));
        }

        private string BaseFolder => Path.GetDirectoryName(GrayFile);
        private string SubFolder => Path.Combine(BaseFolder, $"Render_{DateString}");

        private string DateString => $"{_createDate.Year}.{_createDate.Month:D2}.{_createDate.Day:D2}-{_createDate.Hour:D2}.{_createDate.Minute:D2}";
        public string AnimationFolder => Path.Combine(SubFolder, "Animation");
        public string GetAnimationFileForIteration(int iteration) {
            return Path.Combine(AnimationFolder, $"frame_{iteration:D5}.png");
        }

        public string FloodFolder => Path.Combine(SubFolder, "Flood");
        public string GetFloodFileForIteration(int iteration)
        {
            return Path.Combine(FloodFolder, $"frame_{iteration:D5}.png");
        }
    }
}