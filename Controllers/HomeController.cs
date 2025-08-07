using Microsoft.AspNetCore.Mvc;

namespace Azure_Semantic_Kernel_Workshop.Controllers
{
    [Controller]
    [Route("/")]
    [RequireGraphToken]
    public class HomeController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var filePath = Path.Combine(_environment.WebRootPath, "Views", "Home", "index.html");
            return PhysicalFile(filePath, "text/html");
        }
    }
}
