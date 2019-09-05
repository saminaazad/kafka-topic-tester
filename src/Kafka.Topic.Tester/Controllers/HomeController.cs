using Kafka.Topic.Tester.Helpers;
using Kafka.Topic.Tester.Models;
using Kafka.Topic.Tester.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kafka.Topic.Tester.Controllers
{
    public class HomeController : Controller
    {
        private readonly Dictionary<string, AvroSchema> _schema;
        private readonly ApplicationSettings _settings;

        public HomeController(IOptions<ApplicationSettings> options)
        {
            _settings = options.Value;
            _schema = AvroHelper.LoadCurrentSchema(_settings.SchemaDirectory);
        }

        public IActionResult Index()
        {
            return View(new DashboardViewModel
            {
                SchemaNames = FIleHelper.GetAllFiles(_settings.SchemaDirectory, Constants.AvroSchemaFileExtension),
                TypeNames = FIleHelper.GetAllFiles(_settings.MessageTypeDirectory, Constants.AvroTypeFileExtension, true)
            });
        }

        public IActionResult Refresh()
        {
            AvroHelper.GenerateAvroTypes(_settings.AvrogenDirectory, _settings.SchemaDirectory, _settings.MessageTypeDirectory);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddSchema(string topic)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Produce(string topicname = null)
        {
            var topics = FIleHelper.GetAllFiles(_settings.SchemaDirectory, Constants.AvroSchemaFileExtension);
            if (string.IsNullOrEmpty(topicname))
            {
                return View(new ProducerViewModel
                {
                    TopicNames = topics
                });
            }
            
            return View(new ProducerViewModel
            {
                TopicNames = FIleHelper.GetAllFiles(_settings.SchemaDirectory, Constants.AvroSchemaFileExtension),
                CurrentTopic = topicname,
                AvroJson = _schema.ContainsKey(topicname) ? AvroHelper.PopulateAvroJson(_schema[topicname]) : string.Empty
            });
        }

        [HttpGet]
        public IActionResult Consume(string topic)
        {
            return View();
        }

        public IActionResult Populate(string topicname)
        {
            var text = AvroHelper.PopulateAvroJson(_schema[topicname]);
            return View("Produce", new ProducerViewModel
            {
                TopicNames = FIleHelper.GetAllFiles(_settings.SchemaDirectory, Constants.AvroSchemaFileExtension)
            });
        }
    }
}
