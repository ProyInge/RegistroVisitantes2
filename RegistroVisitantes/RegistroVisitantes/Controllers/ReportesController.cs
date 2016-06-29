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


        public ActionResult Index()
        {
            DateTime from = new DateTime(2005, 3, 20);
            DateTime to = (from.AddDays(365));
       
            ViewBag.fromDate = from;
            ViewBag.toDate = to;

            return View();
        }

        private IQueryable<INFOVISITA> getTabla(REPORTE reporte)
        {
            IQueryable<INFOVISITA> tabla;
            tabla = db.INFOVISITA.Where(x => x.RESERVACION.ENTRA >= reporte.FECHADESDE && x.RESERVACION.ENTRA <= reporte.FECHAHASTA).OrderBy(x => x.ID_RESERVACION);

            // se cuentan reservaciones y visitantes por estacion y por institucion
            switch (reporte.ANFITRIONA)
            {
                case "ESINTRO":
                    tabla = tabla.Where(x => x.RESERVACION.ANFITRIONA.Equals("01"));
                    break;
                case "OET":
                    tabla = tabla.Where(x => x.RESERVACION.ANFITRIONA.Equals("02"));
                    break;
            }
            switch (reporte.ESTACION)
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
            return tabla;
        }

        public ActionResult Detalles(int? Pagina, [Bind(Include = "FECHADESDE,FECHAHASTA,ANFITRIONA,ESTACION,TIPO,NACIONALIDAD")] REPORTE reporte, bool? col1, bool? col2, bool? col3, bool? col4, bool? col5, bool? col6, bool? col7, bool? col8, bool? col9)
        {
            
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

            ViewBag.FECHADESDE = reporte.FECHADESDE;
            ViewBag.FECHAHASTA = reporte.FECHAHASTA;
            ViewBag.ANFITRIONA = reporte.ANFITRIONA;
            ViewBag.ESTACION = reporte.ESTACION;
            ViewBag.TIPO = reporte.TIPO;
            ViewBag.NACIONALIDAD = reporte.NACIONALIDAD;

            ViewBag.col1 = col1;
            ViewBag.col2 = col2;
            ViewBag.col3 = col3;
            ViewBag.col4 = col4;
            ViewBag.col5 = col5;
            ViewBag.col6 = col6;
            ViewBag.col7 = col7;
            ViewBag.col8 = col8;
            ViewBag.col9 = col9;

            var tabla = getTabla(reporte);

            // se cuentan reservaciones y visitantes por estacion y por institucion
            switch (reporte.ANFITRIONA)
            {
                case "ESINTRO":
                    totalVisitESINTRO = tabla.Count(x => x.RESERVACION.ANFITRIONA.Equals("01"));
                    totalReservESINTRO = tabla.Where(x => x.RESERVACION.ANFITRIONA.Equals("01")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                case "OET":
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

            switch (reporte.ESTACION)
            {
                case "01":
                    totalVisitLS = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("La Selva"));
                    totalReservLS = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("La Selva")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                case "02":
                    totalVisitPV = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Palo Verde"));
                    totalReservPV = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Palo Verde")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                case "03":
                    totalVisitLC = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Las Cruces"));
                    totalReservLC = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Las Cruces")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                case "04":
                    totalVisitCRO = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Costa Rican Offices"));
                    totalReservCRO = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Costa Rican Offices")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                default:  // si se selecciona la opcion de todas las estaciones
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

        public ActionResult Download([Bind(Include = "FECHADESDE,FECHAHASTA,ANFITRIONA,ESTACION,TIPO,NACIONALIDAD")] REPORTE reporte, bool? col1, bool? col2, bool? col3, bool? col4, bool? col5, bool? col6, bool? col7, bool? col8, bool? col9)
        {
            var tabla = getTabla(reporte);
            saveExcel(tabla, col1, col2, col3, col4, col5, col6, col7, col8, col9, reporte);
            string file = Server.MapPath("~/Reporte.xlsx"); ;
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(file, contentType, Path.GetFileName(file));
        }

        public DataTable toDataTable(IEnumerable<INFOVISITA> tabla, bool? col1, bool? col2, bool? col3, bool? col4, bool? col5, bool? col6, bool? col7, bool? col8, bool? col9)
        {
            var dt = new DataTable();
            if (col1 != false) { dt.Columns.Add("Compañía", typeof(String)); }
            if (col2 != false) { dt.Columns.Add("Estación", typeof(String)); }
            if (col3 != false) { dt.Columns.Add("Reservación", typeof(String)); }
            if (col4 != false) { dt.Columns.Add("Fecha entrada", typeof(DateTime)); }
            if (col5 != false) { dt.Columns.Add("Fecha salida", typeof(DateTime)); }
            if (col6 != false) { dt.Columns.Add("Tipo visitante", typeof(String)); }
            if (col7 != false) { dt.Columns.Add("Nacionalidad", typeof(String)); }
            if (col8 != false) { dt.Columns.Add("Nombre completo", typeof(String)); }
            if (col9 != false) { dt.Columns.Add("Correo electrónico", typeof(String)); }
            foreach (INFOVISITA iv in tabla)
            {
                DataRow newRow = dt.NewRow();
                if (col1 != false) { newRow["Compañía"] = iv.RESERVACION.ANFITRIONA == "01" ? "OET" : "ESINTRO"; }
                if (col2 != false) { newRow["Estación"] = iv.RESERVACION.ESTACIONI.NOMBRE; }
                if (col3 != false) { newRow["Reservación"] = iv.RESERVACION.NUMERO; }
                if (col4 != false) { newRow["Fecha entrada"] = iv.RESERVACION.ENTRA; }
                if (col5 != false) { newRow["Fecha salida"] = iv.RESERVACION.SALE; }
                if (col6 != false) { newRow["Tipo visitante"] = iv.PERSONA.ROL; }
                if (col7 != false) { newRow["Nacionalidad"] = iv.PERSONA.NACIONALIDADI != null ? iv.PERSONA.NACIONALIDADI.GENTILICIO : ""; }
                if (col8 != false) { newRow["Nombre completo"] = iv.PERSONA.NOMBRE + " " + iv.PERSONA.APELLIDO; }
                if (col9 != false) { newRow["Correo electrónico"] = iv.PERSONA.EMAIL; }
                dt.Rows.Add(newRow);
            }
            return dt;
        }


        public DataTable toDataTableSumInsti(IEnumerable<INFOVISITA> tabla, REPORTE reporte)
        {
            var dt = new DataTable();
            dt.Columns.Add("Compañía", typeof(String)); 
            dt.Columns.Add("Número de reservaciones", typeof(int)); 
            dt.Columns.Add("Número de visitantes", typeof(int));

            DataRow oetRow = dt.NewRow();
            oetRow["Compañía"] = "OET";
            DataRow esintroRow = dt.NewRow();
            esintroRow["Compañía"] = "ESINTRO";
            DataRow totalesRow = dt.NewRow();
            totalesRow["Compañía"] = "Total";
            int totalReservOET = 0;
            int totalVisitOET = 0;
            int totalReservESINTRO = 0;
            int totalVisitESINTRO = 0;
            int totalReservInstitucion = 0;
            int totalVisitInstitucion = 0;
            switch (reporte.ANFITRIONA)
            {
                case "ESINTRO":
                    totalVisitESINTRO = tabla.Count(x => x.RESERVACION.ANFITRIONA.Equals("01"));
                    totalReservESINTRO = tabla.Where(x => x.RESERVACION.ANFITRIONA.Equals("01")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                case "OET":
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

            totalReservInstitucion = totalReservESINTRO + totalReservOET;
            totalVisitInstitucion = totalVisitESINTRO + totalVisitOET;

            oetRow["Número de reservaciones"] = totalReservOET;
            oetRow["Número de visitantes"] = totalVisitOET;

            esintroRow["Número de reservaciones"] = totalReservESINTRO;
            esintroRow["Número de visitantes"] = totalVisitESINTRO;

            totalesRow["Número de reservaciones"] = totalReservInstitucion;
            totalesRow["Número de visitantes"] = totalVisitInstitucion;

            dt.Rows.Add(oetRow);
            dt.Rows.Add(esintroRow);
            dt.Rows.Add(totalesRow);


            return dt;
        }

        public DataTable toDataTableSumEstacion(IEnumerable<INFOVISITA> tabla, REPORTE reporte)
        {
            var dt = new DataTable();
            dt.Columns.Add("Estación", typeof(String));
            dt.Columns.Add("Número de reservaciones", typeof(int));
            dt.Columns.Add("Número de visitantes", typeof(int));

            DataRow LSRow = dt.NewRow();
            LSRow["Estación"] = "La Selva";
            DataRow PVRow = dt.NewRow();
            PVRow["Estación"] = "Palo Verde";
            DataRow LCRow = dt.NewRow();
            LCRow["Estación"] = "Las Cruces";
            DataRow CRORow = dt.NewRow();
            CRORow["Estación"] = "Central";
            DataRow totalesRow = dt.NewRow();
            totalesRow["Estación"] = "Total";
            int totalReservLS = 0;
            int totalVisitLS = 0;
            int totalReservLC = 0;
            int totalVisitLC = 0;
            int totalReservPV = 0;
            int totalVisitPV = 0;
            int totalReservCRO = 0;
            int totalVisitCRO = 0;
            int totalReservEstacion = 0;
            int totalVisitEstacion = 0;

            switch (reporte.ESTACION)
            {
                case "01":
                    totalVisitLS = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("La Selva"));
                    totalReservLS = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("La Selva")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                case "02":
                    totalVisitPV = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Palo Verde"));
                    totalReservPV = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Palo Verde")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                case "03":
                    totalVisitLC = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Las Cruces"));
                    totalReservLC = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Las Cruces")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                case "04":
                    totalVisitCRO = tabla.Count(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Costa Rican Offices"));
                    totalReservCRO = tabla.Where(x => x.RESERVACION.ESTACIONI.NOMBRE.Equals("Costa Rican Offices")).Select(x => x.RESERVACION.ID).Distinct().Count();
                    break;
                default:  // si se selecciona la opcion de todas las estaciones
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

            totalReservEstacion = totalReservCRO + totalReservLC + totalReservLS + totalReservPV;
            totalVisitEstacion = totalVisitCRO + totalVisitLC + totalVisitLS + totalVisitPV;

            LSRow["Número de reservaciones"] = totalReservLS;
            LSRow["Número de visitantes"] = totalVisitLS;

            PVRow["Número de reservaciones"] = totalReservPV;
            PVRow["Número de visitantes"] = totalVisitPV;

            LCRow["Número de reservaciones"] = totalReservLC;
            LCRow["Número de visitantes"] = totalVisitLC;

            CRORow["Número de reservaciones"] = totalReservCRO;
            CRORow["Número de visitantes"] = totalVisitCRO;

            totalesRow["Número de reservaciones"] = totalReservEstacion;
            totalesRow["Número de visitantes"] = totalVisitEstacion;

            dt.Rows.Add(LSRow);
            dt.Rows.Add(PVRow);
            dt.Rows.Add(LCRow);
            dt.Rows.Add(CRORow);
            dt.Rows.Add(totalesRow);


            return dt;
        }

        public void saveExcel(IQueryable<INFOVISITA> t, bool? col1, bool? col2, bool? col3, bool? col4, bool? col5, bool? col6, bool? col7, bool? col8, bool? col9, REPORTE reporte)
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

                //Agrega los totales
                DataTable totalInsti = toDataTableSumInsti(tablaenum, reporte);
                DataTable totalEstacion = toDataTableSumEstacion(tablaenum, reporte);
                
                ws.Cells[tablaenum.Count()+3, 1].LoadFromDataTable(totalInsti, true);
                ws.Cells[tablaenum.Count()+3, 6].LoadFromDataTable(totalEstacion, true);

                ws.Cells.AutoFitColumns();

                pck.Save();
            }
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
