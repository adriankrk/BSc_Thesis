using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Katalog.Repositories;

namespace Katalog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Katalog firm wraz z wizualizacją opinii na temat jakości pracy";

            // Zdefiniowanie obiektu repozytorium firm i pobranie listy nowych firm
            CompanyRepository _companyRepo = new CompanyRepository();
            var companies = _companyRepo.GetNewCompanies();

            return View(companies);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
