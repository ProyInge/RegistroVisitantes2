using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RegistroVisitantes.Models;
using PagedList;

namespace RegistroVisitantes.Controllers
{
    public class ReportesController : Controller
    {
        private BDRegistro db = new BDRegistro();


        public ActionResult Index(int? Pagina)
        {
            IQueryable<INFOVISITA> tabla;

            DateTime from = (DateTime.Today);
            DateTime to = (DateTime.Today.AddDays(7));

            /*DateTime from = (fromDate ?? new DateTime(2012, 01, 01));
            DateTime to = (toDate ?? new DateTime(2013, 01, 01));*/

            ViewBag.fromDate = from;
            ViewBag.toDate = to;

            //tabla = db.INFOVISITA.Where(x => String.Equals(x.ID_RESERVACION, reservacion.ID)).OrderBy(x => x.ID_RESERVACION);
            tabla = db.INFOVISITA.OrderBy(x => x.ID_RESERVACION);
            int Size_Of_Page = 5;
            int No_Of_Page = (Pagina ?? 1);
            return View(tabla.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        // POST: /Reportes
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "FECHADESDE,FECHAHASTA,ANFITRIONA,ESTACION,TIPO,NACIONALIDAD")] REPORTE reporte)
        {
            if (ModelState.IsValid)
            {
                db.REPORTE.Add(reporte);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reporte);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
