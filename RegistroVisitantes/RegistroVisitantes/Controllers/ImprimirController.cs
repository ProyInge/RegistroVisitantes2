using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;

namespace RegistroVisitantes.Controllers
{
    public class ImprimirController : Controller
    {

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Session["CurrentCulture"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["CurrentCulture"].ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["CurrentCulture"].ToString());
            }
            else
            {
                CultureInfo ci = new CultureInfo("en");
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }
        }

        // GET: Imprimir
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult ImprimirOET()
        {
            return View();
        }

        [Authorize]
        public ActionResult ImprimirESINTRO()
        {
            return View();
        }

        public ActionResult ChangeCultureESINTRO(string ddlCulture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);

            Session["CurrentCulture"] = ddlCulture;
            return RedirectToAction("ImprimirESINTRO");
        }

        public ActionResult ChangeCultureOET(string ddlCulture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);

            Session["CurrentCulture"] = ddlCulture;
            return RedirectToAction("ImprimirOET");
        }


    }
}