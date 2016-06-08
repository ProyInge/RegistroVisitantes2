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

        /*
        * Desc: Lenguaje por defecto del formulario
        * Requiere: solicitud de vista del formulario
        * campo del formulario
        * Devuelve: N/A
        */
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

        /*
        * Desc: cambia el idioma de la información que se presenta en el formulario
        * Requiere: idioma al que se quiere cambiar, id de la reservación asociada al formulario de registro
        * Devuelve: La vista del formulario con el idioma actualizado
        */
        public ActionResult ChangeCulture(string ddlCulture, string idRes)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);

            Session["CurrentCulture"] = ddlCulture;
            return RedirectToAction("Index", new { idRes = idRes });
        }


        /*
         * Desc: Recibe la solicitud de la pagina del formulario, dependiendo de la organizacio a la que esta asociada
         * la reservacion se muestra el formulario correspondiente.
         * Requiere: El id de la reservacion a consultar
         * Devuelve: la vista del formulaio correspondiente al registro
         */
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
            if(reservacion.ANFITRIONA.Equals("01")) // si es de OET
            {
                return RedirectToAction("CreateOET", new { idRes = idRes });
            }
            else //si es de ESINTRO
            {
                return RedirectToAction("CreateESINTRO", new { idRes = idRes });
            }
        }



        /*
         * Desc: valida el campo de correo electronico
         * Requiere: el correo electronico ingresado por el usuario
         * Devuelve: verdadero si cumple el estado de correo válido, falso si no.
         */
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


        /*
         * Desc: Permite llenar automaticamente los campos del formulario de ESINTRO, para una persona que 
         * haya llenado el formulario en una visita anterior
         * Requiere: La identificación o correo electronico de la persona
         * Devuelve: el formulario con los campos completados
         */
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

        /*
         * Desc: Permite llenar automaticamente los campos del formulario de OET, para una persona que 
         * haya llenado el formulario en una visita anterior
         * Requiere: La identificación o correo electronico de la persona
         * Devuelve: el formulario con los campos completados
         */
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

         /*
         * Desc: Muestra un formulario de ESINTRO en blanco para el registro de un visitante
         * Requiere: La identificación de la reservacion asociada al registro de la persona
         * Devuelve: vista del formulario en blanco
         */
        [HttpGet]
        public ActionResult CreateESINTRO(String idRes)
        {
            if (idRes == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reservacion = BDRegistro.V_RESERVACION.Find(idRes);
            if (reservacion == null || !reservacion.ANFITRIONA.Equals("02")) //si no es un id de reserva valido
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound); // 404
            }
            //para llenar los dropdownlist de la interfaz con la información adecuada
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

        /*
        * Desc: Recibe los datos de un formulario de ESINTRO completado por un visitante
        * Requiere: La identificación de la reservacion asociada al registro de la persona, los datos de cada
        * campo del formulario
        * Devuelve: mensaje con resultado de envío
        */
        [HttpPost]
        public ActionResult CreateESINTRO(String idRes, [Bind()]Models.INFOVISITA form, FormCollection collection, string dietas, string genero, bool checkPollo = false, bool checkCarne = false, bool checkCerdo = false, bool checkPescado = false)
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

            form.CARNE = checkCarne; //checkboxes
            form.POLLO = checkPollo;
            form.CERDO = checkCerdo;
            form.PESCADO = checkPescado;
          

            form.ID_RESERVACION= idRes;
            if (genero == ViewResources.Resources.oet_fem) // si es femenino
            {
                form.PERSONA.GENERO = '1'.ToString();
            }
            else
            {
                form.PERSONA.GENERO = '0'.ToString(); //es masculino
            }
            if (dietas == ViewResources.Resources.oet_sinrestr)//si selecciona sin restricciones de dieta
            {
                form.DIETA = "No Restriction";
            }
            else
            {
                if (dietas.Equals(ViewResources.Resources.oet_vege))//vegetariano
                {
                    form.DIETA = "Vegetarian";
                }
                else
                {
                    form.DIETA = "Vegan"; //si seleciona dieta vegana
                }

            }
            form.ESTADO = true;

            if (ModelState.IsValid)
            {
                var db = BDRegistro;
                string nompais = (string)collection["PERSONA.PAISI.NOMBRE"].ToUpper(); ;
                V_PAISES pais = BDRegistro.V_PAISES.Where(x => String.Equals(x.NOMBRE, nompais)).FirstOrDefault();
                form.PERSONA.PAIS = (pais == null) ? null : pais.ISO;
                form.PERSONA.PAISI = pais;

                string gentpais = (string)collection["PERSONA.NACIONALIDADI.GENTILICIO"].ToUpper();
                V_PAISES nacion = BDRegistro.V_PAISES.Where(x => String.Equals(x.GENTILICIO, gentpais)).FirstOrDefault();
                form.PERSONA.NACIONALIDAD = (nacion == null) ? null : nacion.ISO;
                form.PERSONA.NACIONALIDADI = nacion;
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
            }
            return RedirectToAction("Index", "Reservas");
        }

        /*
        * Desc: Muestra un formulario de OET en blanco para el registro de un visitante
        * Requiere: La identificación de la reservacion asociada al registro de la persona
        * Devuelve: vista del formulario en blanco
        */
        [HttpGet]
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
            //llena los dropdownlist de la interfaz
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

        /*
        * Desc: Recibe los datos de un formulario de OET completado por un visitante
        * Requiere: La identificación de la reservacion asociada al registro de la persona, los datos de cada
        * campo del formulario
        * Devuelve: mensaje con resultado de envío
        */
        [HttpPost]
        public ActionResult CreateOET(String idRes, [Bind()]Models.INFOVISITA form, FormCollection collection, string dietas, string genero, bool checkPollo = false, bool checkCarne = false, bool checkCerdo = false, bool checkPescado = false)
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
           
            form.CARNE = checkCarne; //checkboxes
            form.POLLO = checkPollo;
            form.CERDO = checkCerdo;
            form.PESCADO = checkPescado;
            

            if (form.PERSONA.GENERO == ViewResources.Resources.oet_fem)//si es femenino
            {
                form.PERSONA.GENERO = '1'.ToString();

            }
            else
            {
                form.PERSONA.GENERO = '0'.ToString(); //masculino
            }

            form.ID_RESERVACION = idRes;
            form.CEDULA = form.PERSONA.CEDULA;
            form.ESTADO = true;            

            if (ModelState.IsValid)
            {
                var db = BDRegistro;
                string nominst = (string)collection["PERSONA.INSTITUCIONI.FULL_NAME"];
                V_INSTITUCION inst = BDRegistro.V_INSTITUCION.Where(x => String.Equals(x.FULL_NAME, nominst)).FirstOrDefault();
                form.PERSONA.INSTITUCION = (inst == null) ? (int?)null : inst.CAT_INSTITUCION;
                form.PERSONA.INSTITUCIONI = inst;

                string nompais = (string)collection["PERSONA.PAISI.NOMBRE"].ToUpper(); ;
                V_PAISES pais = BDRegistro.V_PAISES.Where(x => String.Equals(x.NOMBRE, nompais)).FirstOrDefault();
                form.PERSONA.PAIS = (pais == null) ? null : pais.ISO;
                form.PERSONA.PAISI = pais;

                string gentpais = (string)collection["PERSONA.NACIONALIDADI.GENTILICIO"].ToUpper();
                V_PAISES nacion = BDRegistro.V_PAISES.Where(x => String.Equals(x.GENTILICIO, gentpais)).FirstOrDefault();
                form.PERSONA.NACIONALIDAD = (nacion == null) ? null : nacion.ISO;
                form.PERSONA.NACIONALIDADI = nacion;
                db.INFOVISITA.Add(form);
                
                try
                {
                    db.SaveChanges(); //se guarda la información
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

        public ActionResult Instituciones(string term)
        {
            var result = (from a in BDRegistro.V_INSTITUCION
                          join b in BDRegistro.V_PAISES on a.COUNTRY equals b.ISO
                          where a.FULL_NAME.ToLower().Contains(term.ToLower()) || b.NOMBRE.ToLower().Contains(term.ToLower())
                          select new { a.FULL_NAME, b.NOMBRE, a.CAT_INSTITUCION }).Distinct();
            // Get Tags from database
            return this.Json(result,
                            JsonRequestBehavior.AllowGet);
        }

        public ActionResult Paises(string term)
        {
            var result = (from r in BDRegistro.V_PAISES
                          where r.NOMBRE.ToLower().Contains(term.ToLower())
                          select new { r.NOMBRE, r.CAT_PAISES }).Distinct();
            // Get Tags from database
            return this.Json(result,
                            JsonRequestBehavior.AllowGet);
        }

        public ActionResult Nacionalidades(string term)
        {
            var result = (from r in BDRegistro.V_PAISES
                          where r.NOMBRE.ToLower().Contains(term.ToLower())
                          select new { r.GENTILICIO, r.CAT_PAISES }).Distinct();
            // Get Tags from database
            return this.Json(result,
                            JsonRequestBehavior.AllowGet);
        }
    }
}