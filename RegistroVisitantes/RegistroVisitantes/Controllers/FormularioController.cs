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
    public class FormularioController : Controller
    {
        private PreCntacto BDPreContac = new PreCntacto();
        // GET: Formulario
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Formulario/ESINTRO
        [HttpGet]
        public ActionResult CreateESINTRO()
        {
            var sexo = new SelectList(new[] { "Male", "Female" });
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
        public ActionResult CreateESINTRO([Bind()]Models.PREREGISTROCONTACTO form, string dietas, string carnes)
        {
            if (dietas.Equals("sr"))
            {
                form.NO_DIETARY_RESTRICTIONS = true;
            }
            if (dietas.Equals("veg"))
            {
                form.VEGETARIAN = true;
            }
            if (dietas.Equals("vg"))
            {
                form.VEGAN = true;
            }

            if (carnes.Equals("carne"))
            {
                form.BEEF = true;
            }
            if (carnes.Equals("pollo"))
            {
                form.CHICKEN = true;
            }
            if (carnes.Equals("cerdo"))
            {
                form.PORK = true;
            }
            if (carnes.Equals("pescado"))
           {
                form.FISH = true;
           }
            if (ModelState.IsValid)
            {
                var db = BDPreContac;
                db.PREREGISTROCONTACTO.Add(form);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return CreateESINTRO();
        }
        
        [HttpGet]
        public ActionResult CreateOET()
        {
            var sexo = new SelectList(new[] { "Female", "Male" });
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
        public ActionResult CreateOET([Bind()]Models.PREREGISTROCONTACTO form, string dietas)
        {
            if (dietas.Equals("sr"))
            {
                form.NO_DIETARY_RESTRICTIONS = true;
            }
            if (dietas.Equals("veg"))
            {
                form.VEGETARIAN = true;
            }
            if (dietas.Equals("vg"))
            {
                form.VEGAN = true;
            }

            //if (carnes.Equals("carne"))
            //{
                form.BEEF = true;
            //}
            //if (carnes.Equals("pollo"))
            //{
                form.CHICKEN = true;
           // }
            //if (carnes.Equals("cerdo"))
            //{
                form.PORK = true;
            //}
            //if (carnes.Equals("pescado"))
            //{
                form.FISH = true;
            //}
            form.PROPOSITO = "c";
            form.ROLCURSO = "a";



            if (ModelState.IsValid)
            {
                var db = BDPreContac;
                db.PREREGISTROCONTACTO.Add(form);
                
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                return RedirectToAction("Index");
            }
            return CreateOET();
        }
    }
}