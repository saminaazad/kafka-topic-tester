using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka.Topic.Tester.Models
{
    public class ProducerViewModel
    {
        public string[] TopicNames { get; set; }

        public string CurrentTopic { get; set; }

        public string AvroJson { get; set; }
    }
}
