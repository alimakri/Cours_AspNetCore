using AdventureWorks.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Controllers
{
    [Authorize]
    public class LogController : Controller
    {
        private Repository Repo;
        public LogController(IConfiguration config)
        {
            Repo = new Repository(config);
        }
        public IActionResult Index()
        {
            return View(Repo.Get_log());
        }
        public IActionResult Purge()
        {
            Repo.Purge_Log();
            return RedirectToAction("Index");
        }
    }
}
