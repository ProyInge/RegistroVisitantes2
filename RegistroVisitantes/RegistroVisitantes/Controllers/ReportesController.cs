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
using System.IO;
using OfficeOpenXml;
using System.Web.Hosting;
using System.Drawing;

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
            ViewBag.ESTACION = "00";
            ViewBag.TIPO = "00";
            ViewBag.NACIONALIDAD = "00";
            ViewBag.col1 = true;
            ViewBag.col2 = true;
            ViewBag.col3 = true;
            ViewBag.col4 = true;
            ViewBag.col5 = true;
            ViewBag.col6 = true;
            ViewBag.col7 = true;
            ViewBag.col8 = true;
            ViewBag.col9 = true;

            ViewBag.Columnas =

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
        public ActionResult Index(int? Pagina, [Bind(Include = "FECHADESDE,FECHAHASTA,ANFITRIONA,ESTACION,TIPO,NACIONALIDAD")] REPORTE reporte, bool col1, bool col2, bool? col3, bool? col4, bool? col5, bool? col6, bool? col7, bool? col8, bool? col9)
        {
            IQueryable<INFOVISITA> tabla;
            int totalReservLS = 0;
            int totalVisitLS = 0;
            int totalReservLC = 0;
            int totalVisitLC = 0;
            int totalReservPV = 0;
            int totalVisitPV = 0;
            int totalReservCRO = 0;
            int totalVisitCRO = 0;

            int totalReservOET = 0;
            int totalVisitOET = 0;
            int totalReservESINTRO = 0;
            int totalVisitESINTRO = 0;

            int totalReservInstitucion = 0;
            int totalVisitInstitucion = 0;
            int totalReservEstacion = 0;
            int totalVisitEstacion = 0;

            ViewBag.fromDate = reporte.FECHADESDE;
            ViewBag.toDate = reporte.FECHAHASTA;

            tabla = db.INFOVISITA.Where(x => x.RESERVACION.ENTRA >= reporte.FECHADESDE && x.RESERVACION.SALE <= reporte.FECHAHASTA).OrderBy(x => x.ID_RESERVACION);

            //se cuentan reservaciones y visitantes por estacion y por institucion
            ViewBag.ANFITRIONA = reporte.ANFITRIONA;
            switch (reporte.ANFITRIONA)
            {
                case "ESINTRO":
                    tabla = tabla.Where(x => x.RESERVACION.ANFITRIONA.Equals("01"));
                    totalVisitESINTRO = tabla.Count(x => x.RESERVACION.ANFITRIONA.Equals("01"));
                    totalReservESINTRO = tabla.Where(x => x.RESERVACION.ANFITRIONA.Equals("01")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                case "OET":
                    tabla = tabla.Where(x => x.RESERVACION.ANFITRIONA.Equals("02"));
                    totalVisitOET = tabla.Count(x => x.RESERVACION.ANFITRIONA.Equals("02"));
                    totalReservOET = tabla.Where(x => x.RESERVACION.ANFITRIONA.Equals("02")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                default:  // si se selecciona la opcion de todas las instituciones
                    totalVisitESINTRO = tabla.Count(x => x.RESERVACION.ANFITRIONA.Equals("01"));
                    totalReservESINTRO = tabla.Where(x => x.RESERVACION.ANFITRIONA.Equals("01")).Select(x => x.RESERVACION.ID).Distinct().Count();

                    totalVisitOET = tabla.Count(x => x.RESERVACION.ANFITRIONA.Equals("02"));
                    totalReservOET = tabla.Where(x => x.RESERVACION.ANFITRIONA.Equals("02")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
            }

            ViewBag.ESTACION = reporte.ESTACION;
            switch (reporte.ESTACION)
            {
                case "01":
                    tabla = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("La Selva"));
                    totalVisitLS = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("La Selva"));
                    totalReservLS = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("La Selva")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                case "02":
                    tabla = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Palo Verde"));
                    totalVisitPV = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Palo Verde"));
                    totalReservPV = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Palo Verde")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                case "03":
                    tabla = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Las Cruces"));
                    totalVisitLC = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Las Cruces"));
                    totalReservLC = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Las Cruces")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                case "04":
                    tabla = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Costa Rican Offices"));
                    totalVisitCRO = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Costa Rican Offices"));
                    totalReservCRO = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Costa Rican Offices")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                default:  //si se selecciona la opcion de todas las estaciones
                    totalVisitLS = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("La Selva"));
                    totalReservLS = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("La Selva")).Select(x => x.RESERVACION.ID).Distinct().Count();

                    totalVisitLC = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Las Cruces"));
                    totalReservLC = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Las Cruces")).Select(x => x.RESERVACION.ID).Distinct().Count();

                    totalVisitPV = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Palo Verde"));
                    totalReservPV = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Palo Verde")).Select(x => x.RESERVACION.ID).Distinct().Count();

                    totalVisitCRO = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Costa Rican Offices"));
                    totalReservCRO = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Costa Rican Offices")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
            }

            ViewBag.TIPO = reporte.TIPO;
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

            ViewBag.NACIONALIDAD = reporte.NACIONALIDAD;
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

            ViewBag.col1 = col1;
            ViewBag.col2 = col2;
            ViewBag.col3 = col3;
            ViewBag.col4 = col4;
            ViewBag.col5 = col5;
            ViewBag.col6 = col6;
            ViewBag.col7 = col7;
            ViewBag.col8 = col8;
            ViewBag.col9 = col9;

            //se suman cantidad de reservaciones de todas las instituciones
            totalReservInstitucion = totalReservESINTRO + totalReservOET;
            totalVisitInstitucion = totalVisitESINTRO + totalVisitOET;
            //se suman cantidad de reservaciones y visitantes de todas las estaciones
            totalReservEstacion = totalReservCRO + totalReservLC + totalReservLS + totalReservPV;
            totalVisitEstacion = totalVisitCRO + totalVisitLC + totalVisitLS + totalVisitPV;

            ViewBag.totalReservLS = totalReservLS;
            ViewBag.totalVisitLS = totalVisitLS;
            ViewBag.totalReservLC = totalReservLC;
            ViewBag.totalVisitLC = totalVisitLC;
            ViewBag.totalReservPV = totalReservPV;
            ViewBag.totalVisitPV = totalVisitPV;
            ViewBag.totalReservCRO = totalReservCRO;
            ViewBag.totalVisitCRO = totalVisitCRO;

            ViewBag.totalReservOET = totalReservOET;
            ViewBag.totalVisitOET = totalVisitOET;
            ViewBag.totalReservESINTRO = totalReservESINTRO;
            ViewBag.totalVisitESINTRO = totalVisitESINTRO;

            ViewBag.totalReservInstitucion = totalReservInstitucion;
            ViewBag.totalVisitInstitucion = totalVisitInstitucion;
            ViewBag.totalReservEstacion = totalReservEstacion;
            ViewBag.totalVisitEstacion = totalVisitEstacion;

            int Size_Of_Page = 5;
            int No_Of_Page = (Pagina ?? 1);
            return View(tabla.ToPagedList(No_Of_Page, Size_Of_Page));

        }

        public void saveExcel(IQueryable<INFOVISITA> t, bool? col1, bool? col2, bool? col3, bool? col4, bool? col5, bool? col6, bool? col7, bool? col8, bool? col9)
        {
            String fileName = Server.MapPath("~/Reporte.xlsx");
            var arch = new FileInfo(fileName);
            arch.Delete();
            using (ExcelPackage pck = new ExcelPackage(arch))
            {

                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Reporte");
                IEnumerable<INFOVISITA> tablaenum = t.AsEnumerable<INFOVISITA>();
                DataTable res = toDataTable(tablaenum, col1, col2, col3, col4, col5, col6, col7, col8, col9);
                //ver cual celda es la que trae las fechas y cambiar por los numeros
                ws.Column(5).Style.Numberformat.Format = "yyyy-mm-dd h:mm";
                ws.Column(4).Style.Numberformat.Format = "yyyy-mm-dd h:mm";


                ws.Cells["A1"].LoadFromDataTable(res, true);

                //ver la cantidad de parametros que vienen en true y cambiar la cantCeldas
                int cantCeldas = 9;
                int contador = 1;
                while (contador <= cantCeldas)
                {
                    ws.Cells[1, contador].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[1, contador].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    ws.Cells[1, contador].Style.Font.Bold = true;
                    contador++;
                }
                ws.Cells.AutoFitColumns();

                pck.Save();
            }
        }

        public ActionResult Download()
        {

            string file = Server.MapPath("~/Reporte.xlsx"); ;
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(file, contentType, Path.GetFileName(file));
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
