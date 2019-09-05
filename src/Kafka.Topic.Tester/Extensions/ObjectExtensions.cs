using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kafka.Topic.Tester.Extensions
{
    public static class ObjectExtensions
    {
        private static Dictionary<string, object> Default = new Dictionary<string, object>()
        {
            {"boolean", false },
            {"byte[]", new byte[0] },
            {"int32", 0 },
            {"int64", 0 },
            {"single", (float)0.0 },
            {"double", (double)0.0 },
            {"string", "string" }
        };

        public static void SetPropertiesWithDefaultValue(this Object source, PropertyInfo prop = null)
        {
            
        }

        public static void RemoveProperties(this JObject jsonObject, string property)
        {
            jsonObject.Remove(property);

            foreach (var prop in jsonObject)
            {
                if (prop.Value.Type == JTokenType.Object)
                {
                    RemoveProperties((JObject)prop.Value, property);
                }
            }
        }
    }
}
