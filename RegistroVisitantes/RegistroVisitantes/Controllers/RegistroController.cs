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
        private BDReservas BDReserv = new BDReservas();


        // GET: Registro
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Registro/Formulario
        [Authorize]
        [HttpGet]
        public ActionResult Formulario()
        {
            return View();
        }

        // GET: /Registro/FormularioOET
        [HttpGet]
        [Authorize]
        public ActionResult FormularioOET()
        {
            return View();
        }

        // GET: /Registro/Idioma
        [HttpGet]
        [Authorize]
        public ActionResult Idioma()
        {
            return View();
        }

        // GET: /Registro/Create
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            /*var sexo = new SelectList(new[] { "Male", "Female" });
            var proposito = new SelectList(new[] { " Visiting Scientist(without project at the Station)", "Researcher (with project at the Station", "Educational Course", "University extension course", "Environmental education program", "Natural history visitor", "Special event or meeting", "Journalist (reporter, writer, filmer)", "OTS staff (on business not covered by other categories)", "Other" });
            var position = new SelectList(new[] { "Principal Investigator", "CO-IP", "Senior Staff", "Tutor", "Supervisor", "Coordinator", "Collaborator", "Student", "Technical", "Field Assistant", "Interns", "Volunteer" });
            var role = new SelectList(new[] { "Student", "Professor", "Coordinator", "Assistant" });
            ViewBag.sexoList = sexo;
            ViewBag.propositoList = proposito;
            ViewBag.positionList = position;
            ViewBag.roleList = role;*/
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(string dietas, string carnes)
        {

            /*if (dietas.Equals("sr"))
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
            }*/
            return Create();
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateOET()
        {
           /* var sexo = new SelectList(new[] { "Female", "Male" });
            var proposito = new SelectList(new[] { " Visiting Scientist(without project at the Station)" , "Researcher (with project at the Station", "Educational Course", "University extension course", "Environmental education program", "Natural history visitor", "Special event or meeting", "Journalist (reporter, writer, filmer)", "OTS staff (on business not covered by other categories)", "Other" });
            var position = new SelectList(new[] { "Principal Investigator", "CO-IP", "Senior Staff", "Tutor", "Supervisor", "Coordinator", "Collaborator", "Student", "Technical", "Field Assistant", "Interns", "Volunteer" });
            var role = new SelectList(new[] { "Student", "Professor", "Coordinator", "Assistant" });
            ViewBag.sexoList = sexo;
            ViewBag.propositoList = proposito;
            ViewBag.positionList = position;
            ViewBag.roleList = role;*/
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateOET( string dietas, string carnes)
        {
            /*if(dietas.Equals("sr"))
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
            }*/
            return Create();
        }

        
    }
}
