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
    public class VisitantesController : Controller
    {
        private BDContactos db = new BDContactos();

        // GET: Visitantes
        // .Include(p => p.CONTACTO) esto incluye por fk la entidad CONTACTO asociada
        public ActionResult Index(String id, int? Pagina)
        {
            IQueryable<PREREGISTRO> pREREGISTRO;
            if (id != null && !id.Equals(""))
            {
                pREREGISTRO = db.PREREGISTRO.Where(x => String.Equals(x.IDRESERVACION, id)).Include(p => p.CONTACTO).OrderBy(x => x.IDRESERVACION);

            }
            else {
                pREREGISTRO = db.PREREGISTRO.Include(p => p.CONTACTO).OrderBy(x => x.IDRESERVACION);
            }


            int Size_Of_Page = 10;
            int No_Of_Page = (Pagina ?? 1);
            return View(pREREGISTRO.ToPagedList(No_Of_Page, Size_Of_Page));
        }
        public ActionResult Reserva(string id, int? Pagina)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pREREGISTRO = db.PREREGISTRO.Include(p => p.CONTACTO).Where(x => String.Equals(x.IDRESERVACION, id)).OrderBy(x => x.IDRESERVACION);
            int Size_Of_Page = 10;
            int No_Of_Page = (Pagina ?? 1);
            return View(pREREGISTRO.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        // GET: Visitantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PREREGISTRO pREREGISTRO = db.PREREGISTRO.Find(id);
            if (pREREGISTRO == null)
            {
                return HttpNotFound();
            }
            return View(pREREGISTRO);
        }

        // GET: Visitantes/Create
        public ActionResult Create()
        {
            ViewBag.IDCONTACTO = new SelectList(db.CONTACTO, "CONTACTO1", "FIRST_NAME");
            return View();
        }

        // POST: Visitantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NUMPREREGISTRO,IDCONTACTO,IDRESERVACION,PROPOSITO,IDGRUPO,COMOENTERO,FECHA,NOMCURSO,NUMCURSO,ROLCURSO,NOMPROYECTO,INVERSIONES,FUENTE,RESOLUCION,PERMISO,EXPIRACION")] PREREGISTRO pREREGISTRO)
        {
            if (ModelState.IsValid)
            {
                db.PREREGISTRO.Add(pREREGISTRO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDCONTACTO = new SelectList(db.CONTACTO, "CONTACTO1", "FIRST_NAME", pREREGISTRO.IDCONTACTO);
            return View(pREREGISTRO);
        }

        // GET: Visitantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PREREGISTRO pREREGISTRO = db.PREREGISTRO.Find(id);
            if (pREREGISTRO == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDCONTACTO = new SelectList(db.CONTACTO, "CONTACTO1", "FIRST_NAME", pREREGISTRO.IDCONTACTO);
            return View(pREREGISTRO);
        }

        // POST: Visitantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NUMPREREGISTRO,IDCONTACTO,IDRESERVACION,PROPOSITO,IDGRUPO,COMOENTERO,FECHA,NOMCURSO,NUMCURSO,ROLCURSO,NOMPROYECTO,INVERSIONES,FUENTE,RESOLUCION,PERMISO,EXPIRACION")] PREREGISTRO pREREGISTRO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pREREGISTRO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDCONTACTO = new SelectList(db.CONTACTO, "CONTACTO1", "FIRST_NAME", pREREGISTRO.IDCONTACTO);
            return View(pREREGISTRO);
        }

        // GET: Visitantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PREREGISTRO pREREGISTRO = db.PREREGISTRO.Find(id);
            if (pREREGISTRO == null)
            {
                return HttpNotFound();
            }
            return View(pREREGISTRO);
        }

        // POST: Visitantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PREREGISTRO pREREGISTRO = db.PREREGISTRO.Find(id);
            db.PREREGISTRO.Remove(pREREGISTRO);
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
