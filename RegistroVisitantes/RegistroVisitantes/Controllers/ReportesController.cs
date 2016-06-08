using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RegistroVisitantes.Models;

namespace RegistroVisitantes.Controllers
{
    public class ReportesController : Controller
    {
        private BDRegistro db = new BDRegistro();

        // GET: /Reportes/
        public ActionResult Index()
        {
            return View(db.REPORTE.ToList());
        }

        // GET: /Reportes/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REPORTE reporte = db.REPORTE.Find(id);
            if (reporte == null)
            {
                return HttpNotFound();
            }
            return View(reporte);
        }

        // GET: /Reportes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Reportes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="FECHADESDE,FECHAHASTA,ANFITRIONA,ESTACION,TIPO,NACIONALIDAD")] REPORTE reporte)
        {
            if (ModelState.IsValid)
            {
                db.REPORTE.Add(reporte);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reporte);
        }

        // GET: /Reportes/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REPORTE reporte = db.REPORTE.Find(id);
            if (reporte == null)
            {
                return HttpNotFound();
            }
            return View(reporte);
        }

        // POST: /Reportes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,FECHADESDE,FECHAHASTA,ANFITRIONA,ESTACION,TIPO,NACIONALIDAD")] REPORTE reporte)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reporte).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reporte);
        }

        // GET: /Reportes/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REPORTE reporte = db.REPORTE.Find(id);
            if (reporte == null)
            {
                return HttpNotFound();
            }
            return View(reporte);
        }

        // POST: /Reportes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            REPORTE reporte = db.REPORTE.Find(id);
            db.REPORTE.Remove(reporte);
            db.SaveChanges();
            return RedirectToAction("Index");
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
