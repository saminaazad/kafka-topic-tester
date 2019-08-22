using Avro.Specific;
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

        public static void GenerateAvroTypes(string pathToAvrogen, string pathToSchema)
        {   
            string command = $"{pathToAvrogen}\\avrogen"; 
            string[] files = Directory.GetFiles(pathToSchema, "*.asvc");

            foreach (var schema in files)
            {
                AddAvroSchema(schema);

                string args = $"-s {schema} .";
                Process process = new Process();
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = args;
                process.Start();
                process.WaitForExit();
            }            
        }

        private static void AddAvroSchema(string file)
        {
            var jsonSchema = File.ReadAllText(file, Encoding.UTF8);
            var schema = JsonConvert.DeserializeObject<AvroSchema>(jsonSchema);
            var topicname = Path.GetFileNameWithoutExtension(file);

            Topics[topicname] = schema;
        } 

        public static string PopulateAvroJson(string topicname)
        {
            var type = Topics[topicname];

            var obj = Activator.CreateInstance(Type.GetType(type.Type));

            SetProperties(obj);

            var jObj = JObject.Parse(JsonConvert.SerializeObject(obj));
            RemoveSchemaProp(jObj);

            return jObj.ToString();
        }

        private static void RemoveSchemaProp(JObject obj)
        {
            obj.Remove("Schema");

            foreach(var prop in obj)
            {
                
            }
        }

        private static void SetProperties(Object obj, PropertyInfo prop = null)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.Name != "Schema" && !property.PropertyType.IsPrimitiveType())
                {
                    var value = Activator.CreateInstance(property.PropertyType);
                    property.SetValue(obj, value);
                    SetProperties(value);
                }
            }
        }

        static bool IsNullable(this Type type) => Nullable.GetUnderlyingType(type) != null;

        private static bool IsPrimitiveType(this Type type)
        {
            if (type == typeof(String) || type.IsInterface || type.IsNullable())
                return true;

            return type.IsPrimitive;
        }
    }
}
