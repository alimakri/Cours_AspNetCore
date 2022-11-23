using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Controllers
{
        [Authorize]
    public class PrivateController : Controller
    {
        private readonly ILogger<PrivateController> Logger;
        public PrivateController(ILogger<PrivateController> log)
        {
            Logger = log;
        }
        public IActionResult Index()
        {
            Serilog.Log.Warning("PrivateController.Index");
            return View();
        }
    }
}
