using AdventureWorks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AdventureWorks.Controllers
{
    [Authorize]
    public class PrivateController : Controller
    {
        private readonly ILogger<PrivateController> Logger;
        private IWebHostEnvironment Env;

        public PrivateController(ILogger<PrivateController> log, IWebHostEnvironment env)
        {
            Logger = log;
            Env = env;
        }
        public IActionResult Index()
        {
            var data = new ExcelData();
            var path = $@"{Env.WebRootPath.Replace("wwwroot", "data")}\DataPrivate.xlsx";

            var ds = data.Get_DataPrivate(path);

            // Blocs
            List<BlocData> blocs = new List<BlocData>();
            var libelleBlocs = ds.Tables[0].AsEnumerable().Select(x => x.Field<string>("Bloc")).Distinct().ToList();
            foreach (var libelle in libelleBlocs)
            {
                blocs.Add(new BlocData { Libelle = libelle });
            }
            foreach (var bloc in blocs)
            {
                bloc.Rubrics = ds.Tables[0].AsEnumerable()
                    .Where(x => x.Field<string>("bloc") == bloc.Libelle)
                    .Select(x => x.Field<string>("Rubrique"))
                    .Distinct()
                    .Select(x => new RubricData { Libelle = x })
                    .ToList();

            }
            foreach (var bloc in blocs)
            {
                foreach (var rubric in bloc.Rubrics)
                {
                    rubric.Items = ds.Tables[0].AsEnumerable()
                    .Where(x => x.Field<string>("bloc") == bloc.Libelle)
                    .Where(x => x.Field<string>("Rubrique") == rubric.Libelle)
                    .Select(x => new BlocItem
                    {
                        Libelle = x.Field<string>("Contenu"),
                        Copie = x.Field<string>("Copie"),
                        Lien = x.Field<string>("Lien")
                    })
                    .ToList();
                }
            }
            return View(blocs);
        }
    }
    class BlocData
    {
        public string? Libelle { get; set; }
        public List<RubricData> Rubrics { get; set; }
    }
    class RubricData
    {
        public int Ordre { get; set; }
        public string? Libelle { get; set; }
        public List<BlocItem> Items { get; set; }
    }
    class BlocItem
    {
        public int? Ordre { get; set; }
        public string? Libelle { get; set; }
        public string? Copie { get; set; }
        public string? Lien { get; set; }
    }
}
