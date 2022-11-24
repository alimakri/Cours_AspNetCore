using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private IWebHostEnvironment Env;

        public DocumentController(IWebHostEnvironment env, IConfiguration config)
        {
            Env = env;
        }

        [Route("{controller=Document}/{doc}/{typemime}", Name = "loadDoc")]
        public IActionResult Index(string doc, string typemime)
        {
            string filePath = $@"{Env.WebRootPath.Replace("wwwroot", "documents")}\{doc}";
            try
            {
                var stream = new FileStream(filePath, FileMode.Open);
                return new FileStreamResult(stream, $"application/{typemime}");
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, $"DocumentController.Index {filePath}");
                return RedirectToAction("Error");
            }
        }
    }
}
