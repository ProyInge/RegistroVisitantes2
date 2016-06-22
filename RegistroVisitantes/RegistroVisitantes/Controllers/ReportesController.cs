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

            DateTime from = new DateTime(2005, 3, 20); 
            DateTime to = (from.AddDays(7));

            /*DateTime from = (fromDate ?? new DateTime(2012, 01, 01));
            DateTime to = (toDate ?? new DateTime(2013, 01, 01));*/

            ViewBag.fromDate = from;
            ViewBag.toDate = to;

            ViewBag.ANFITRIONA = "TODAS";

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
        public ActionResult Index(int? Pagina, [Bind(Include = "FECHADESDE,FECHAHASTA,ANFITRIONA,ESTACION,TIPO,NACIONALIDAD")] REPORTE reporte)
        {
            IQueryable<INFOVISITA> tabla;

            ViewBag.fromDate = reporte.FECHADESDE;
            ViewBag.toDate = reporte.FECHAHASTA;

            //tabla = db.INFOVISITA.Where(x => String.Equals(x.ID_RESERVACION, reservacion.ID)).OrderBy(x => x.ID_RESERVACION);



            tabla = db.INFOVISITA.Where(x => x.RESERVACION.ENTRA >= reporte.FECHADESDE && x.RESERVACION.SALE <= reporte.FECHAHASTA).OrderBy(x => x.ID_RESERVACION);

            ViewBag.ANFITRIONA = reporte.ANFITRIONA;
            switch(reporte.ANFITRIONA)
            {
                case "ESINTRO":
                    tabla = tabla.Where(x => x.RESERVACION.ANFITRIONA.Equals("01"));
                    break;
                case "OET":
                    tabla = tabla.Where(x => x.RESERVACION.ANFITRIONA.Equals("02"));
                    break;
            }

            switch(reporte.ESTACION)
            {
                case "01":
                    tabla = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("La Selva"));
                    break;
                case "02":
                    tabla = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Palo Verde"));
                    break;
                case "03":
                    tabla = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Las Cruces"));
                    break;
                case "04":
                    tabla = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Costa Rican Offices"));
                    break;
            }

            switch (reporte.TIPO)
            {
                case "01":
                    tabla = tabla.Where(x => x.PERSONA.ROL.Equals("Staff"));
                    break;
                case "02":
                    tabla = tabla.Where(x => x.PERSONA.ROL.Equals("Investigador"));
                    break;
                case "03":
                    tabla = tabla.Where(x => x.PERSONA.ROL.Equals("Académico"));
                    break;
                case "04":
                    tabla = tabla.Where(x => x.PERSONA.ROL.Equals("Otro"));
                    break;
            }

            switch (reporte.NACIONALIDAD)
            {
                case "01":
                    tabla = tabla.Where(x => x.PERSONA.NACIONALIDAD.Equals("CR"));
                    break;
                case "02":
                    tabla = tabla.Where(x => x.PERSONA.NACIONALIDAD.Equals("US"));
                    break;
                case "03":
                    tabla = tabla.Where(x => x.PERSONA.NACIONALIDAD.Equals("FR"));
                    break;
            }

            int Size_Of_Page = 5;
            int No_Of_Page = (Pagina ?? 1);
            return View(tabla.ToPagedList(No_Of_Page, Size_Of_Page));
    
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
