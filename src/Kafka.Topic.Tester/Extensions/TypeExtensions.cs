using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka.Topic.Tester.Extensions
{
    public static class TypeExtensions
    {
        private static Dictionary<string, object> Default = new Dictionary<string, object>()
        {
            { "boolean", false },
            { "byte", (byte)0 },
            { "int32", 0 },
            { "int64", (long)0 },
            { "single", (float)0.0 },
            { "double", 0.0 },
            { "string", "string" },
            { "guid", Guid.Empty },
            { "datetime", DateTime.Now }
        };

        public static bool IsNullableType(this Type type) => Nullable.GetUnderlyingType(type) != null;
        private static bool IsEnumerableType(this Type type) => (type != typeof(string) && type.GetInterface(nameof(IEnumerable)) != null);
        private static bool IsSimpleType(this Type type) => Default.ContainsKey(type.Name.ToLower());
        private static bool IsComplexType(this Type type) => !(type.IsSimpleType() || type.IsNullableType() || type.IsEnumerableType() || type.IsInterface || type.IsAbstract);

        public static object CreateTypeInstance(this Type type, params string[] excludeProperties)
        {
            var instance = Activator.CreateInstance(type);
            foreach (var property in type.GetProperties())
            {
                if(excludeProperties.Contains(property.Name, StringComparer.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if (property.PropertyType.IsEnumerableType())
                {
                    var collection = CreateEnumerableInstance(property.PropertyType, excludeProperties);
                    property.SetValue(instance, collection);
                }
                else if (property.PropertyType.IsComplexType())
                {
                    var value = Activator.CreateInstance(property.PropertyType);
                    property.SetValue(instance, CreateTypeInstance(property.PropertyType, excludeProperties));
                }
                else
                {
                    property.SetValue(instance, GetDefaultValue(property.PropertyType));
                }
            }

            return instance;
        }

        private static object GetDefaultValue(Type type)
        {
            var typename = type.Name.ToLowerInvariant();
            return Default.ContainsKey(typename) ? Default[typename] : null;
        }

        private static IEnumerable CreateEnumerableInstance(Type type, params string[] excludeProperties)
        {
            if (type.IsArray)
            {
                var array = Array.CreateInstance(type.GetElementType(), 1);
                array.SetValue(CreateTypeInstance(type.GetElementType()), 0);
                return array;
            }

            if (type.isDi)

            var genericType = typeof(List<>).MakeGenericType(type.GenericTypeArguments);
            var collection = (IList)Activator.CreateInstance(genericType);
            collection.Add(CreateTypeInstance(type.GenericTypeArguments[0], excludeProperties));

            return collection;
        }
    }
}
