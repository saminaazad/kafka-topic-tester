using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kafka.Topic.Tester.Helpers
{
    public static class FIleHelper
    {
        public static string[] GetAllFiles(string root, string extension = null, bool recursive = false)
        {
            var directories = new List<string> { root };
            if (recursive)
            {
                directories.AddRange(Directory.GetDirectories(root, "*", SearchOption.AllDirectories));
            }

            var filenames = new List<string>();
            foreach (var path in directories)
            {
                var files = Directory.GetFiles(path, $"*.{extension}");
                var nameSpace = string.Join('.', path.Split('\\')).Remove(0, root.Length);
                filenames.AddRange(files.Select(file => $"{nameSpace}.{Path.GetFileName(file)}".Trim('.')));
            }

            return filenames.ToArray();
        }
    }
}
