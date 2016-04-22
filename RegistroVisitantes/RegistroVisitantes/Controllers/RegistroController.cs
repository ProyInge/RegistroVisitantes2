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
            var sexo = new SelectList(new[] { "Hombre", "Mujer" });
            var proposito = new SelectList(new[] { " Visiting Scientist(without project at the Station)", "Researcher (with project at the Station", "Educational Course", "University extension course", "Environmental education program", "Natural history visitor", "Special event or meeting", "Journalist (reporter, writer, filmer)", "OTS staff (on business not covered by other categories)", "Other" });
            var position = new SelectList(new[] { "Principal Investigator", "CO-IP", "Senior Staff", "Tutor", "Supervisor", "Coordinator", "Collaborator", "Student", "Technical", "Field Assistant", "Interns", "Volunteer" });
            var role = new SelectList(new[] { "Student", "Professor", "Coordinator", "Assistant" });
            ViewBag.sexoList = sexo;
            ViewBag.propositoList = proposito;
            ViewBag.positionList = position;
            ViewBag.roleList = role;
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind()]Models.PREREGISTRO form,string dietas, string carnes)
        {

            if (dietas.Equals("sr"))
            {
                form.CONTACTO.NO_DIETARY_RESTRICTIONS = true;
            }
            if (dietas.Equals("veg"))
            {
                form.CONTACTO.VEGETARIAN = true;
            }
            if (dietas.Equals("vg"))
            {
                form.CONTACTO.VEGAN = true;
            }

            if (carnes.Equals("carne"))
            {
                form.CONTACTO.BEEF = true;
            }
            if (carnes.Equals("pollo"))
            {
                form.CONTACTO.CHICKEN = true;
            }
            if (carnes.Equals("cerdo"))
            {
                form.CONTACTO.PORK = true;
            }
            if (carnes.Equals("pescado"))
            {
                form.CONTACTO.FISH = true;
            }
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
            var sexo = new SelectList(new[] { "Hombre", "Mujer" });
            var proposito = new SelectList(new[] { " Visiting Scientist(without project at the Station)" , "Researcher (with project at the Station", "Educational Course", "University extension course", "Environmental education program", "Natural history visitor", "Special event or meeting", "Journalist (reporter, writer, filmer)", "OTS staff (on business not covered by other categories)", "Other" });
            var position = new SelectList(new[] { "Principal Investigator", "CO-IP", "Senior Staff", "Tutor", "Supervisor", "Coordinator", "Collaborator", "Student", "Technical", "Field Assistant", "Interns", "Volunteer" });
            var role = new SelectList(new[] { "Student", "Professor", "Coordinator", "Assistant" });
            ViewBag.sexoList = sexo;
            ViewBag.propositoList = proposito;
            ViewBag.positionList = position;
            ViewBag.roleList = role;
            return View();
        }

        [HttpPost]
        public ActionResult CreateOET([Bind()]Models.PREREGISTRO form, string dietas, string carnes)
        {
            if(dietas.Equals("sr"))
            {
                form.CONTACTO.NO_DIETARY_RESTRICTIONS = true;
            }
            if (dietas.Equals("veg"))
            {
                form.CONTACTO.VEGETARIAN = true;
            }
            if (dietas.Equals("vg"))
            {
                form.CONTACTO.VEGAN = true;
            }

            if (carnes.Equals("carne"))
            {
                form.CONTACTO.BEEF = true;
            }
            if (carnes.Equals("pollo"))
            {
                form.CONTACTO.CHICKEN = true;
            }
            if (carnes.Equals("cerdo"))
            {
                form.CONTACTO.PORK = true;
            }
            if (carnes.Equals("pescado"))
            {
                form.CONTACTO.FISH = true;
            }

            if (ModelState.IsValid)
            {
                var db = BDContac;
                db.PREREGISTRO.Add(form);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return Create();
        }

        public ActionResult ListVisitantes(int? id, String nombre, int? Pagina)
        {
            var db = BDContac;
            IQueryable<CONTACTO> lista;

            if(id != null)
            {
                lista = db.CONTACTO.AsQueryable();
                lista = from p in db.PREREGISTRO
                      from c in lista
                        where p.IDCONTACTO == id.Value && p.IDCONTACTO == c.CONTACTO1
                      select c;
            }
            else
            {
                lista = db.CONTACTO.OrderBy(x => x.FIRST_NAME);
            }

            /*if(nombre != null)
            {
                lista = db.CONTACTO.Where(x => x.FIRST_NAME.Equals(nombre)).OrderBy(x => x.FIRST_NAME);
            }
            else
            {
                lista = db.CONTACTO.OrderBy(x => x.FIRST_NAME);
            }*/


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
