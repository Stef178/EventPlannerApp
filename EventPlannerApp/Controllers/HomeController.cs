using System.Diagnostics;
using EventPlannerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using EventPlannerApp.Data;

namespace EventPlannerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EventPlannerContext _context; // Je DbContext

        public HomeController(ILogger<HomeController> logger, EventPlannerContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Haal alle toekomstige evenementen op
            var events = _context.Events.Where(e => e.Date >= DateTime.Now).ToList();
            return View(events);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
