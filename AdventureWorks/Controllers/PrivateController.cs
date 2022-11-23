using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Controllers
{
    public class PrivateController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
