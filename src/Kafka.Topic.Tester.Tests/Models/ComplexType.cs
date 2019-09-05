using System;
using System.Collections.Generic;

namespace Kafka.Topic.Tester.Tests.Models
{
    public class ComplexType
    {
        public long Id { get; set; }

        public byte[] Bytes { get; set; }

        public List<NestedType> Collection { get; set; }

        public AnotherNestedType[] Array { get; set; }
    }

    public class NestedType
    {
        public Guid NestedIdentifier { get; set; }

        public AnotherNestedType NestedItem { get; set; }
    }

    public class AnotherNestedType
    {
        public int Property1 { get; set; }

        public string Property2 { get; set; }

        public decimal Property3 { get; set; }
    }
}
