using System;
using System.Collections.Generic;
using System.IO;

namespace Colorization.WorkingLocation
{
    public class WorkingDirectory
    {
        private readonly string _directory;

        public WorkingDirectory(string directory) {
            _directory = directory;
            if (!Directory.Exists(directory)) {
                Console.WriteLine($"Coult not find base image directory! Make sure the folder '{directory}' exists.");
                return;
            }
        }

        private string InputDirectory => Path.Combine(_directory, "input");
        private string OutputDirectory => Path.Combine(_directory, "output");

        public WorkingFileSet[] GetWorkingFileSets() {
            var ret = new List<WorkingFileSet>();
            foreach (var directory in Directory.GetDirectories(InputDirectory)) {
                var projectName = Path.GetFileName(directory);
                var output = Path.Combine(OutputDirectory, projectName);
                ret.Add(new WorkingFileSet(projectName, directory, output));
            }
            return ret.ToArray();
        }
    }
}
