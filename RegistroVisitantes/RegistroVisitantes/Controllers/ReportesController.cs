using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistroVisitantes.Controllers
{
    public class ReportesController : Controller
    {
        // GET: Reportes
        public ActionResult Index()
        {
            DateTime from = DateTime.Today;
            DateTime to =   DateTime.Today.AddDays(7);

            ViewBag.fromDate = from;
            ViewBag.toDate = to;

            return View();
        }
    }
}