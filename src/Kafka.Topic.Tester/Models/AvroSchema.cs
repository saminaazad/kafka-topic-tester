namespace Kafka.Topic.Tester.Models
{
    public class AvroSchema
    {
        public string Topic { get; set; }

        public string Namespace { get; set; }

        public string Name { get; set; }

        public string Type => $"{Namespace}.{Name}";

        public object[] Fields { get; set; }
    }
}
