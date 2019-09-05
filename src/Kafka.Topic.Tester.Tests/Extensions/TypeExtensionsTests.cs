using Kafka.Topic.Tester.Extensions;
using Kafka.Topic.Tester.Tests.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kafka.Topic.Tester.Tests
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void CreateTypeInstance_should_return_instance_with_default_values_for_complex_type()
        {
            var expected = new ComplexType
            {
                Id = 0,
                Bytes = new byte[] { 0 },
                Collection = new List<NestedType>
                {
                    new NestedType
                    {
                        NestedIdentifier = Guid.Empty,
                        NestedItem = new AnotherNestedType
                        {
                            Property1 = 0,
                            Property2 = "string",
                            Property3 = 0
                        }
                    }
                },
                Array = new AnotherNestedType[]
                {
                    new AnotherNestedType
                    {
                        Property1 = 0,
                        Property2 = "string",
                        Property3 = 0
                    }
                }
            };

            var result = typeof(ComplexType).CreateTypeInstance();
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }
    }
}
