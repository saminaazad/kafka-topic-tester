using Avro.Specific;
using Kafka.Topic.Tester.Extensions;
using Kafka.Topic.Tester.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Kafka.Topic.Tester.Helpers
{
    public static class AvroHelper
    {
        private static Dictionary<string, AvroSchema> Topics = new Dictionary<string, AvroSchema>();

        public static void GenerateAvroTypes(string pathToAvrogen, string pathToSchema, string destination)
        {   
            string command = $"{pathToAvrogen}\\avrogen"; 
            string[] files = Directory.GetFiles(pathToSchema, "*.asvc");

            foreach (var schema in files)
            {
                string args = $"-s {schema} .\\{destination}";
                Process process = new Process();
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = args;
                process.Start();
                process.WaitForExit();
            }            
        }

        public static Dictionary<string, AvroSchema> LoadCurrentSchema(string pathToSchema)
        {
            var schema = new Dictionary<string, AvroSchema>();

            string[] files = Directory.GetFiles(pathToSchema, "*.asvc");            
            foreach(var file in files)
            {
                schema.Add(Path.GetFileNameWithoutExtension(file), GetAvroSchema(file));
            }

            return schema;
        }

        private static AvroSchema GetAvroSchema(string file)
        {
            var jsonSchema = File.ReadAllText(file, Encoding.UTF8);
            return JsonConvert.DeserializeObject<AvroSchema>(jsonSchema);
        } 

        public static string PopulateAvroJson(AvroSchema schema)
        {
            var obj = Type.GetType(schema.Type).CreateTypeInstance("Schema");
            var jsonObject = JObject.Parse(JsonConvert.SerializeObject(obj));
            jsonObject.RemoveProperties("Schema");

            return jsonObject.ToString();
        }        
    }
}
