﻿using System;
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

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Session["CurrentCulture"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["CurrentCulture"].ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["CurrentCulture"].ToString());
            }
            else
            {
                CultureInfo ci = new CultureInfo("en");
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_RESERVACION reservacion = BDRegistro.V_RESERVACION.Find(idRes);
            if (reservacion == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound); // 404
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
                PERSONA persona = BDRegistro.PERSONA.Where(p => p.EMAIL == ajaxInput).FirstOrDefault();
                return PartialView(persona);
            }
            else
            {   //Es una cedula
                PERSONA persona = BDRegistro.PERSONA.Find(ajaxInput);
                return PartialView(persona);
            }
            
        }

        public PartialViewResult AutocompletarOET(String ajaxInput)
        {
            if (IsValidEmail(ajaxInput)) //Es un email
            {
                PERSONA persona = BDRegistro.PERSONA.Where(p => p.EMAIL == ajaxInput).FirstOrDefault();
                return PartialView(persona);
            }
            else
            {   //Es una cedula
                PERSONA persona = BDRegistro.PERSONA.Find(ajaxInput);
                return PartialView(persona);
            }
        }
        // GET: /Formulario/ESINTRO
        [Authorize]
        [HttpGet]
        public ActionResult CreateESINTRO(String idRes)
        {
            if (idRes == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reservacion = BDRegistro.V_RESERVACION.Find(idRes);
            if (reservacion == null || !reservacion.ANFITRIONA.Equals("02"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound); // 404
            }
            var listSexo = new SelectList(new[] { ViewResources.Resources.oet_masc, ViewResources.Resources.oet_fem});
            var proposito = new SelectList(new[] { ViewResources.Resources.oet_prop1, ViewResources.Resources.oet_prop2, ViewResources.Resources.oet_prop3, ViewResources.Resources.oet_prop4, ViewResources.Resources.oet_prop5, ViewResources.Resources.oet_prop6, ViewResources.Resources.oet_prop7, ViewResources.Resources.oet_prop8, ViewResources.Resources.oet_prop9, ViewResources.Resources.oet_prop10, ViewResources.Resources.oet_prop11 });
            var position = new SelectList(new[] { ViewResources.Resources.oet_pos1, ViewResources.Resources.oet_pos2, ViewResources.Resources.oet_pos3, ViewResources.Resources.oet_pos4, ViewResources.Resources.oet_pos5, ViewResources.Resources.oet_pos6, ViewResources.Resources.oet_pos7, ViewResources.Resources.oet_pos8, ViewResources.Resources.oet_pos9, ViewResources.Resources.oet_pos10, ViewResources.Resources.oet_pos11, ViewResources.Resources.oet_pos12 });
            var role = new SelectList(new[] { ViewResources.Resources.oet_rol1, ViewResources.Resources.oet_rol2, ViewResources.Resources.oet_rol3, ViewResources.Resources.oet_rol4, ViewResources.Resources.oet_rol5 });
            ViewBag.sexoList = listSexo;
            ViewBag.propositoList = proposito;
            ViewBag.positionList = position;
            ViewBag.roleList = role;
            ViewBag.idRes = idRes;
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult CreateESINTRO(String idRes, [Bind()]Models.INFOVISITA form, string dietas, string genero, bool checkPollo = false, bool checkCarne = false, bool checkCerdo = false, bool checkPescado = false)
        {
            if (idRes == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.idRes = idRes;
            var reservacion = BDRegistro.V_RESERVACION.Find(idRes);
            if (reservacion == null || !reservacion.ANFITRIONA.Equals("02"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound); // 404
            }

            form.CARNE = checkCarne;
            form.POLLO = checkPollo;
            form.CERDO = checkCerdo;
            form.PESCADO = checkPescado;
          

            form.ID_RESERVACION= idRes;
            if (genero == ViewResources.Resources.oet_fem)
            {
                form.PERSONA.GENERO = '1'.ToString();

            }
            else
            {
                form.PERSONA.GENERO = '0'.ToString();
            }
            if (dietas == ViewResources.Resources.oet_sinrestr)
            {
                form.DIETA = "No Restriction";
            }
            else
            {
                if (dietas.Equals(ViewResources.Resources.oet_vege))
                {
                    form.DIETA = "Vegetarian";
                }
                else
                {
                    form.DIETA = "Vegan";
                }

            }
            form.ESTADO = true;

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
            var reservacion = BDRegistro.V_RESERVACION.Find(idRes);
            if (idRes != null && (reservacion == null || !reservacion.ANFITRIONA.Equals("01")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound); // 404
            }
           
            var listSexo = new SelectList(new[] { ViewResources.Resources.oet_masc, ViewResources.Resources.oet_fem});
            var proposito = new SelectList(new[] { ViewResources.Resources.oet_prop1, ViewResources.Resources.oet_prop2, ViewResources.Resources.oet_prop3, ViewResources.Resources.oet_prop4, ViewResources.Resources.oet_prop5, ViewResources.Resources.oet_prop6, ViewResources.Resources.oet_prop7, ViewResources.Resources.oet_prop8, ViewResources.Resources.oet_prop9, ViewResources.Resources.oet_prop10, ViewResources.Resources.oet_prop11 });
            var position = new SelectList(new[] { ViewResources.Resources.oet_pos1, ViewResources.Resources.oet_pos2, ViewResources.Resources.oet_pos3, ViewResources.Resources.oet_pos4, ViewResources.Resources.oet_pos5, ViewResources.Resources.oet_pos6, ViewResources.Resources.oet_pos7, ViewResources.Resources.oet_pos8, ViewResources.Resources.oet_pos9, ViewResources.Resources.oet_pos10, ViewResources.Resources.oet_pos11, ViewResources.Resources.oet_pos12 });
            var role = new SelectList(new[] { ViewResources.Resources.oet_rol1, ViewResources.Resources.oet_rol2, ViewResources.Resources.oet_rol3, ViewResources.Resources.oet_rol4, ViewResources.Resources.oet_rol5 });
            ViewBag.sexoList = listSexo;
            ViewBag.propositoList = proposito;
            ViewBag.positionList = position;
            ViewBag.roleList = role;
            ViewBag.idRes = idRes;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateOET(String idRes, [Bind()]Models.INFOVISITA form, string dietas, string genero, bool checkPollo = false, bool checkCarne = false, bool checkCerdo = false, bool checkPescado = false)
         {
            if (idRes == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.idRes = idRes;
            var reservacion = BDRegistro.V_RESERVACION.Find(idRes);
            if (reservacion == null || !reservacion.ANFITRIONA.Equals("01"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound); // 404
            }
           
            form.CARNE = checkCarne;
            form.POLLO = checkPollo;
          form.CERDO = checkCerdo;
           form.PESCADO = checkPescado;
            

            if (form.PERSONA.GENERO == ViewResources.Resources.oet_fem)
            {
                form.PERSONA.GENERO = '1'.ToString();

            }
            else
            {
                form.PERSONA.GENERO = '0'.ToString();
            }

            form.ID_RESERVACION = idRes;
            form.CEDULA = form.PERSONA.CEDULA;
            form.ESTADO = true;            

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