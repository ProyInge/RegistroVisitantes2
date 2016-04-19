using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroVisitantes.Models;

namespace RegistroVisitantes.Controllers
{
    public class FormController : Controller
    {
        // GET: Form
        public ActionResult Index()
        {
            var db = new FormDataContext();
            var forms = db.Form.ToArray();

            return View(forms);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var sexo = new SelectList(new[] { "Hombre", "Mujer" });
            ViewBag.sexoList = sexo;
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind()]Models.FormModel form)
        {
            if(ModelState.IsValid)
            {
                var db = new FormDataContext();
                db.Form.Add(form);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return Create();
        }
    }
}