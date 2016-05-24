using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroVisitantes.Models;
using PagedList;
using PagedList.Mvc;
using System.Net;
using ViewResources;
using System.Threading;
using System.Globalization;

namespace RegistroVisitantes.Controllers
{
    public class FormularioController : Controller
    {
        private BDRegistro BDRegistro = new BDRegistro ();
        private BDReservas BDReservas = new BDReservas();

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Session["CurrentCulture"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["CurrentCulture"].ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["CurrentCulture"].ToString());
            }
        }

        public ActionResult ChangeCulture(string ddlCulture, string idRes)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);

            Session["CurrentCulture"] = ddlCulture;
            return RedirectToAction("Index", new { idRes = idRes });
        }

        // GET: Formulario
        [Authorize]
        public ActionResult Index(String idRes)
        {
            if (idRes == null)
            {
                return RedirectToAction("Error", "Home"); //new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RESERVACION reservacion = BDReservas.RESERVACION.Find(idRes);
            if (reservacion == null)
            {
                return RedirectToAction("Error", "Home");  //new HttpStatusCodeResult(HttpStatusCode.NotFound); // 404
            }
            if(reservacion.ANFITRIONA.Equals("01"))
            {
                return RedirectToAction("CreateOET", new { idRes = idRes });
            }
            else
            {
                return RedirectToAction("CreateESINTRO", new { idRes = idRes });
            }
        }

        // GET: /Formulario/ESINTRO
        [Authorize]
        [HttpGet]
        public ActionResult CreateESINTRO(String idRes)
        {
            if (idRes == null)
            {
                return RedirectToAction("Error", "Home");  //new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reservacion = BDReservas.RESERVACION.Find(idRes);
            if (reservacion == null || !reservacion.ANFITRIONA.Equals("02"))
            {
                return RedirectToAction("Error", "Home");  //new HttpStatusCodeResult(HttpStatusCode.NotFound); // 404
            }
            //var sexo = new SelectList(new[] { "Male", "Female" });
            var proposito = new SelectList(new[] { "Visiting Scientist(without project at the Station)", "Researcher (with project at the Station", "Educational Course", "University extension course", "Environmental education program", "Natural history visitor", "Special event or meeting", "Journalist (reporter, writer, filmer)", "OTS staff (on business not covered by other categories)", "Other" });
            var position = new SelectList(new[] { "N/A", "Principal Investigator", "CO-IP", "Senior Staff", "Tutor", "Supervisor", "Coordinator", "Collaborator", "Student", "Technical", "Field Assistant", "Interns", "Volunteer" });
            var role = new SelectList(new[] { "N/A","Student", "Professor", "Coordinator", "Assistant" });
            //ViewBag.sexoList = sexo;
            ViewBag.propositoList = proposito;
            ViewBag.positionList = position;
            ViewBag.roleList = role;
            return View();
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public PartialViewResult AutocompletarESINTRO(String ajaxInput)
        {
            if (IsValidEmail(ajaxInput)) //Es un email
            {
                PERSONA persona = BDReservas.PERSONA.Where(p => p.EMAIL == ajaxInput).FirstOrDefault();
                return PartialView(persona);
            }
            else
            {   //Es una cedula
                PERSONA persona = BDReservas.PERSONA.Find(ajaxInput);
                return PartialView(persona);
            }
            
        }

        public PartialViewResult AutocompletarOET(String ajaxInput)
        {
            if (IsValidEmail(ajaxInput)) //Es un email
            {
                PERSONA persona = BDReservas.PERSONA.Where(p => p.EMAIL == ajaxInput).FirstOrDefault();
                return PartialView(persona);
            }
            else
            {   //Es una cedula
                PERSONA persona = BDReservas.PERSONA.Find(ajaxInput);
                return PartialView(persona);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateESINTRO(String idRes, [Bind()]Models.INFOVISITA form, string dietas, string genero, bool checkPollo = false, bool checkCarne = false, bool checkCerdo = false, bool checkPescado = false)
        {
            if (idRes == null)
            {
                return RedirectToAction("Error", "Home");  //new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reservacion = BDReservas.RESERVACION.Find(idRes);
            if (reservacion == null || !reservacion.ANFITRIONA.Equals("02"))
            {
                return RedirectToAction("Error", "Home");  //new HttpStatusCodeResult(HttpStatusCode.NotFound); // 404
            }

            form.CARNE = checkCarne;
            form.POLLO = checkPollo;
            form.CERDO = checkCerdo;
            form.PESCADO = checkPescado;
          

            if (genero == "female")
            {
                form.PERSONA.GENERO = '1'.ToString();

            }
            else
            {
                form.PERSONA.GENERO = '0'.ToString();
            }


            form.ID_RESERVACION= idRes;
            form.DIETA = dietas;

            if (ModelState.IsValid)
            {
                var db = BDRegistro;
                db.INFOVISITA.Add(form);
                

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
                //return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Reservas");
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateOET(String idRes)
        {
            /*if (idRes == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
            var reservacion = BDReservas.RESERVACION.Find(idRes);
            if (idRes != null && (reservacion == null || !reservacion.ANFITRIONA.Equals("01")))
            {
                return RedirectToAction("Error", "Home");  //new HttpStatusCodeResult(HttpStatusCode.NotFound); // 404
            }
            var sexo = new SelectList(new[] { "Female", "Male" });
            var proposito = new SelectList(new[] { ViewResources.Resources.oet_prop1, ViewResources.Resources.oet_prop2, ViewResources.Resources.oet_prop3, ViewResources.Resources.oet_prop4, ViewResources.Resources.oet_prop5, ViewResources.Resources.oet_prop6, ViewResources.Resources.oet_prop7, ViewResources.Resources.oet_prop8, ViewResources.Resources.oet_prop9, ViewResources.Resources.oet_prop10, ViewResources.Resources.oet_prop11 });
            var position = new SelectList(new[] { ViewResources.Resources.oet_pos1, ViewResources.Resources.oet_pos2, ViewResources.Resources.oet_pos3, ViewResources.Resources.oet_pos4, ViewResources.Resources.oet_pos5, ViewResources.Resources.oet_pos6, ViewResources.Resources.oet_pos7, ViewResources.Resources.oet_pos8, ViewResources.Resources.oet_pos9, ViewResources.Resources.oet_pos10, ViewResources.Resources.oet_pos11, ViewResources.Resources.oet_pos12 });
            var role = new SelectList(new[] { ViewResources.Resources.oet_rol1, ViewResources.Resources.oet_rol2, ViewResources.Resources.oet_rol3, ViewResources.Resources.oet_rol4, ViewResources.Resources.oet_rol5 });
            ViewBag.sexoList = sexo;
            ViewBag.propositoList = proposito;
            ViewBag.positionList = position;
            ViewBag.roleList = role;
            ViewBag.idRes = idRes;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateOET(String idRes, [Bind()]Models.INFOVISITA form, string genero, bool checkPollo = false, bool checkCarne = false, bool checkCerdo = false, bool checkPescado = false)
        {
            if (idRes == null)
            {
                return RedirectToAction("Error","Home");  //new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reservacion = BDReservas.RESERVACION.Find(idRes);
            if (reservacion == null || !reservacion.ANFITRIONA.Equals("01"))
            {
                return RedirectToAction("Error", "Home");  //new HttpStatusCodeResult(HttpStatusCode.NotFound); // 404
            }
            if (checkCarne) {
                form.CARNE = true;
            }
            if (checkPollo)
            {
                form.POLLO = true;
            }
            if (checkCerdo)
            {
                form.CERDO = true;
            }
            if (checkPescado)
            {
                form.PESCADO = true;
            }

            if (genero == "female")
            {
                form.PERSONA.GENERO = '1'.ToString();

            }
            else {
                form.PERSONA.GENERO = '0'.ToString();
            }


            form.ID_RESERVACION = idRes;
            form.CEDULA = form.PERSONA.CEDULA;


            if (ModelState.IsValid)
            {
                var db = BDRegistro;
                db.INFOVISITA.Add(form);
                
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
               // return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Reservas");
        }
    }
}