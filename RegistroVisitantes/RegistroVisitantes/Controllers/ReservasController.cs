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
    public class ReservasController : Controller
    {
        private BDRegistro db = new BDRegistro();

        // GET: Reservas
        [Authorize]
        public ActionResult Index(DateTime? fromDate, DateTime? toDate, int? Pagina)
        {
            DateTime from = (fromDate ?? DateTime.Today);
            DateTime to = (toDate ?? DateTime.Today.AddDays(7));

            /*DateTime from = (fromDate ?? new DateTime(2012, 01, 01));
            DateTime to = (toDate ?? new DateTime(2013, 01, 01));*/

            ViewBag.fromDate = from;
            ViewBag.toDate = to;
            //S=Secre, A=Admin, R=Superusuario
            IOrderedQueryable<V_RESERVACION> lista;
            if (Session["Rol"] != null){
                string estacion = (string)Session["IdEstacion"];
                if ((string)Session["Rol"] == "S")
                {
                    lista = db.V_RESERVACION.Where(x => x.ENTRA != null && DateTime.Compare(x.ENTRA.Value, from) > 0 && DateTime.Compare(x.ENTRA.Value, to) < 0 && estacion == x.ESTACION).OrderBy(s => s.ENTRA);
                }
                else if((string)Session["Rol"] == "A")
                {
                    lista = db.V_RESERVACION.Where(x => x.ENTRA != null && DateTime.Compare(x.ENTRA.Value, from) > 0 && DateTime.Compare(x.ENTRA.Value, to) < 0 && estacion == x.ESTACION).OrderBy(s => s.ENTRA);
                }
                else
                {
                    lista = db.V_RESERVACION.Where(x => x.ENTRA != null && DateTime.Compare(x.ENTRA.Value, from) > 0 && DateTime.Compare(x.ENTRA.Value, to) < 0).OrderBy(s => s.ENTRA);
                }
            }
            else
            {
                lista = db.V_RESERVACION.Where(x => x.ENTRA != null && DateTime.Compare(x.ENTRA.Value, from) > 0 && DateTime.Compare(x.ENTRA.Value, to) < 0).OrderBy(s => s.ENTRA);
            }
            
            int Size_Of_Page = 10;
            int No_Of_Page = (Pagina ?? 1);
            return View(lista.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        // GET: Reservas/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_RESERVACION rESERVACION = db.V_RESERVACION.Find(id);
            if (rESERVACION == null)
            {
                return HttpNotFound();
            }
            return View(rESERVACION);
        }

        // GET: Reservas/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reservas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ANFITRIONA,NUMERO,ESTADO,PRIORIDAD,GRUPO,ENTRA,ENTRAMTN,SALE,SALEMTN,RESPONSABLE,SOLICITANTE,NOTAS,SOLICITADAEL,FORMAPAGO,SALDO,CUENTACLIENTEKEY,CUENTACLIENTEOCLASS,ESTACION,MODIFICADOR,MODIFICADO,MONTO_PREPAGO,REFERENCIA_PREPAGO,PRIMERA_COMIDA,CREACION,ULTIMA_MODIFICACION,MODIFICA_RACK,MODIFICADO_RACK,FLAG,URL")] V_RESERVACION rESERVACION)
        {
            if (ModelState.IsValid)
            {
                db.V_RESERVACION.Add(rESERVACION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rESERVACION);
        }

        // GET: Reservas/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_RESERVACION rESERVACION = db.V_RESERVACION.Find(id);
            if (rESERVACION == null)
            {
                return HttpNotFound();
            }
            return View(rESERVACION);
        }

        // POST: Reservas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,ANFITRIONA,NUMERO,ESTADO,PRIORIDAD,GRUPO,ENTRA,ENTRAMTN,SALE,SALEMTN,RESPONSABLE,SOLICITANTE,NOTAS,SOLICITADAEL,FORMAPAGO,SALDO,CUENTACLIENTEKEY,CUENTACLIENTEOCLASS,ESTACION,MODIFICADOR,MODIFICADO,MONTO_PREPAGO,REFERENCIA_PREPAGO,PRIMERA_COMIDA,CREACION,ULTIMA_MODIFICACION,MODIFICA_RACK,MODIFICADO_RACK,FLAG,URL")] V_RESERVACION rESERVACION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rESERVACION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rESERVACION);
        }

        // GET: Reservas/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_RESERVACION rESERVACION = db.V_RESERVACION.Find(id);
            if (rESERVACION == null)
            {
                return HttpNotFound();
            }
            return View(rESERVACION);
        }

        // POST: Reservas/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            V_RESERVACION rESERVACION = db.V_RESERVACION.Find(id);
            db.V_RESERVACION.Remove(rESERVACION);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
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
