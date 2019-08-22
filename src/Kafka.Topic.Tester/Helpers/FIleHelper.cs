using System.IO;
using System.Linq;

namespace Kafka.Topic.Tester.Helpers
{
    public static class FIleHelper
    {
        public static string[] GetAllFiles(string directory, string extension = null)
        {
            var files = Directory.GetFiles(directory, $"*.{extension}");
            return files.Select(file => Path.GetFileNameWithoutExtension(file)).ToArray();
        }        
    }
}
