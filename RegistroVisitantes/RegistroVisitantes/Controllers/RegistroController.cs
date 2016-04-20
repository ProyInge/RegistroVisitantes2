using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroVisitantes.Models;
using PagedList;
using PagedList.Mvc;


namespace RegistroVisitantes.Controllers
{
    public class RegistroController : Controller
    {
        private BDContactos BDContac = new BDContactos();
        private BDReservas BDReserv = new BDReservas();


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
                db.PREREGISTRO.Add(form);
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
                db.PREREGISTRO.Add(form);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return Create();
        }

        public ActionResult ListVisitantes(int? Pagina)
        {
            var db = BDContac;
            var lista = db.CONTACTO.OrderBy(x => x.FIRST_NAME);

            int Size_Of_Page = 5;
            int No_Of_Page = (Pagina ?? 1);
            return View(lista.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        public ActionResult ListReservas(DateTime? fromDate, DateTime? toDate, int? Pagina)
        {
            var db = BDReserv;
            DateTime from = (fromDate ?? DateTime.Today);
            DateTime to = (toDate ?? DateTime.Today.AddDays(7));

            /*DateTime from = (fromDate ?? new DateTime(2012, 01, 01));
            DateTime to = (toDate ?? new DateTime(2013, 01, 01));*/

            ViewBag.fromDate = from;
            ViewBag.toDate = to;

            var lista = db.RESERVACION.Where(x => x.ENTRA != null && DateTime.Compare(x.ENTRA.Value, from) > 0 && DateTime.Compare(x.ENTRA.Value, to) < 0).OrderBy(s => s.ENTRA);

            int Size_Of_Page = 5;
            int No_Of_Page = (Pagina ?? 1);
            return View(lista.ToPagedList(No_Of_Page, Size_Of_Page));
        }

    }
}
