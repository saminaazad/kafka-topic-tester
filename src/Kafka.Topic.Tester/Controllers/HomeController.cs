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
            var schemaFiles = Directory.GetFiles(_settings.SchemaDirectory, "*.asvc");
            var typeFiles = Directory.GetFiles(_settings.MessageTypeDirectory, "*.cs");

            ShellHelper.Exexute();

            return View(new DashboardViewModel
            {
                SchemaNames = schemaFiles.Select(file => Path.GetFileNameWithoutExtension(file)).ToArray(),
                TypeNames = typeFiles.Select(file => Path.GetFileNameWithoutExtension(file)).ToArray()
            });
        }
    }
}
