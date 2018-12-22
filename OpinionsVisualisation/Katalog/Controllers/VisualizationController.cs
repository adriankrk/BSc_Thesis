using Katalog.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Katalog.Controllers
{
    public class VisualizationController : Controller
    {
        private OpinionRepository _opinionRepo;

        // Konstruktor kontrolera firm.
        public VisualizationController()
        {
            _opinionRepo = new OpinionRepository();
        }


        //
        // GET: /Visualization/

        public ActionResult Index(int id)
        {
            //return View();
            var opinions = _opinionRepo.GetOpinionsByCompanyId(id);
            var opinions_json = opinions.Select(opinion => new { label = opinion.Id, value = opinion.Classification, type = "var" }).ToArray();
            return Json(opinions_json, JsonRequestBehavior.AllowGet);
        }

    }
}
