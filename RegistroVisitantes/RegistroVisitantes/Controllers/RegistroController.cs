using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroVisitantes.Models;

namespace RegistroVisitantes.Controllers
{
    public class RegistroController : Controller
    {
        private BDContactos BDContac = new BDContactos();
        private BDReservas BDReserv = new BDReservas();

  /*      public FORMULARIO obtieneInvestigador(string correo)
        {
            FORMULARIO form = BDContac.FORMULARIO.SingleOrDefault(f => f.E_MAIL == correo);
            return form;
        }

        public FORMULARIO obtieneFormulario(string idReserv)
        {
            FORMULARIO form = BDContac.FORMULARIO.SingleOrDefault(f => f.IDRESERVACION == idReserv);
            return form;
        }

        public FORMULARIO obtieneFormulario(int idPrereg)
        {
            FORMULARIO form = BDContac.FORMULARIO.SingleOrDefault(f => f.NUMPREREGISTRO == idPrereg);
            return form;
        }

        public void guardaFormulario(FORMULARIO f)
        {
            BDContac.FORMULARIO.Add(f);
            BDContac.SaveChanges();
        }
        */

        // GET: Registro
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Registro/Reservas
        [HttpGet]
        public ActionResult Reservas()
        {
            return View();
        }

        // GET: /Registro/Visitantes
        [HttpGet]
        public ActionResult Visitantes()
        {
            return View();
        }

        // GET: /Registro/Formulario
        [HttpGet]
        public ActionResult Formulario()
        {
            return View();
        }

        // GET: /Registro/FormularioOET
        [HttpGet]
        public ActionResult FormularioOET()
        {
            return View();
        }

        // GET: /Registro/Idioma
        [HttpGet]
        public ActionResult Idioma()
        {
            return View();
        }
        // GET: /Registro/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind()]Models.PREREGISTRO form)
        {
            if (ModelState.IsValid)
            {
                var db = BDContac;
                db.Form.Add(form);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return Create();
        }

        [HttpGet]
        public ActionResult CreateOET()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateOET([Bind()]Models.PREREGISTRO form)
        {
            if (ModelState.IsValid)
            {
                var db = BDContac;
                db.Form.Add(form);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return Create();
        }

        public ActionResult ListVisitantes()
        {
            var db = BDContac;
            var lista = db.FormContacto.Take(5).ToList();

            return View(lista);
        }

        public ActionResult ListReservas(DateTime? filterDate)
        {
            var db = BDReserv;
            var lista = db.FormReservarcion.ToList();
            DateTime filter;
            if (filterDate == null) {
                filter = DateTime.Today;
            }
            else
            {
                filter = filterDate.Value;
            }
            ViewBag.fromDate = filter;
            lista = lista.Where(x => x.ENTRA != null && DateTime.Compare(x.ENTRA.Value, filter) < 0).OrderByDescending(x => x.ENTRA).Take(5).ToList();

            return View(lista);
        }

    }
}
