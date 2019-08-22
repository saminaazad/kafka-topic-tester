using Kafka.Topic.Tester.Helpers;
using Kafka.Topic.Tester.Models;
using Kafka.Topic.Tester.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;

namespace Kafka.Topic.Tester.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationSettings _settings;

        public HomeController(IOptions<ApplicationSettings> options)
        {
            _settings = options.Value;
        }

        public IActionResult Index()
        {
            return View(new DashboardViewModel
            {
                SchemaNames = FIleHelper.GetAllFiles(_settings.SchemaDirectory, Constants.AvroSchemaFileExtension),
                TypeNames = FIleHelper.GetAllFiles(_settings.MessageTypeDirectory, Constants.AvroTypeFileExtension)
            });
        }

        public IActionResult Refresh()
        {
            AvroHelper.GenerateAvroTypes(_settings.AvrogenDirectory, _settings.SchemaDirectory);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddSchema(string topic)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Produce()
        {
            ViewData["sample"] = "";
            return View(new ProducerViewModel
            {
                TopicNames = FIleHelper.GetAllFiles(_settings.SchemaDirectory, Constants.AvroSchemaFileExtension)
            });
        }

        [HttpGet]
        public IActionResult Consume(string topic)
        {
            return View();
        }

        public IActionResult Populate(string topicname)
        {
            var text = AvroHelper.PopulateAvroJson(topicname);
            ViewData["sample"] = text;
            return View("Produce", new ProducerViewModel
            {
                TopicNames = FIleHelper.GetAllFiles(_settings.SchemaDirectory, Constants.AvroSchemaFileExtension)
            });
        }
    }
}
