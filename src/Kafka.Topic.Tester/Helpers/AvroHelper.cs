using System.Diagnostics;
using System.IO;

namespace Kafka.Topic.Tester.Helpers
{
    public static class AvroHelper
    {
        public static void GenerateAvroTypes(string pathToAvrogen, string pathToSchema)
        {   
            string command = $"{pathToAvrogen}\\avrogen"; 
            string[] files = Directory.GetFiles(pathToSchema, "*.asvc");

            foreach (var schema in files)
            {
                string args = $"-s {pathToSchema}\\log-message-event.asvc .";
                Process process = new Process();
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = args;
                process.Start();
                process.WaitForExit();
            }            
        }
    }
}
