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
using System.Data.Entity.Validation;
using System.Diagnostics;
using ViewResources;
using System.Threading;
using System.Globalization;

namespace RegistroVisitantes.Controllers
{
    public class VisitantesController : Controller
    {
        private BDRegistro BDRegistro = new BDRegistro();
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
                CultureInfo ci = new CultureInfo("es");
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }
        }
        
        /*
        * Desc: cambia el idioma de la información que se presenta en el formulario
        * Requiere: idioma al que se quiere cambiar, id de la reservación asociada al formulario de registro, cedula de la persona
        * Devuelve: La vista del formulario con la información modificada
        */
        public ActionResult ChangeCulture(string ddlCulture, string idRes, string cedula)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);

            Session["CurrentCulture"] = ddlCulture;
            return RedirectToAction("Details", new { idRes = idRes, ced=cedula });
        }

        /*
         * Desc: Devuelve la pagina con la tabla de visitantes de una reservacion, si esta no se especifica se
         * muestran todas las personas con una reservacion.
         * Requiere: El id de la reservacion a consultar, numero de pagina de la tabla de visitantes.
         * Devuelve: la vista con la información de visitantes   
         */
        [Authorize]
        public ActionResult Index(String idRes, String numRes, int? Pagina)
        {
            IQueryable<INFOVISITA> tabla;
            if (numRes != null && !numRes.Equals(""))
            {
                var resQuery = BDRegistro.V_RESERVACION.Where(x => String.Equals(x.NUMERO, numRes));
                V_RESERVACION reservacion = resQuery.First();
                if (Session["Rol"] != null)
                {
                    string rol = (string)Session["IdEstacion"];
                    if ((string)Session["Rol"] != "R")
                    {       
                        tabla = BDRegistro.INFOVISITA.Where(x => String.Equals(x.ID_RESERVACION, reservacion.ID) && String.Equals(x.RESERVACION.ESTACION, rol)).OrderBy(x => x.ID_RESERVACION);
                    }
                    else
                    {
                        tabla = BDRegistro.INFOVISITA.Where(x => String.Equals(x.ID_RESERVACION, reservacion.ID)).OrderBy(x => x.ID_RESERVACION);
                    }
                }
                else
                {
                    tabla = BDRegistro.INFOVISITA.Where(x => String.Equals(x.ID_RESERVACION, reservacion.ID)).OrderBy(x => x.ID_RESERVACION);
                }
                //V_RESERVACION num = BDRegistro.V_RESERVACION.Find(idRes);
                ViewBag.num = reservacion.NUMERO;
                ViewBag.idRes = idRes;
            }
            else if (idRes != null && !idRes.Equals(""))
            {
                if (Session["Rol"] != null)
                {
                    string rol = (string)Session["IdEstacion"];
                    if ((string)Session["Rol"] != "R")
                    {
                        tabla = BDRegistro.INFOVISITA.Where(x => String.Equals(x.ID_RESERVACION, idRes) && String.Equals(x.RESERVACION.ESTACION, rol)).OrderBy(x => x.ID_RESERVACION);
                    }
                    else
                    {
                        tabla = BDRegistro.INFOVISITA.Where(x => String.Equals(x.ID_RESERVACION, idRes)).OrderBy(x => x.ID_RESERVACION);
                    }
                }
                else
                {
                    tabla = BDRegistro.INFOVISITA.Where(x => String.Equals(x.ID_RESERVACION, idRes)).OrderBy(x => x.ID_RESERVACION);
                }
                V_RESERVACION num = BDRegistro.V_RESERVACION.Find(idRes);
                ViewBag.num = num.NUMERO;
                ViewBag.idRes = idRes;                
            }          
            else
            {
                if (Session["Rol"] != null)
                {
                    string estacion = (string)Session["IdEstacion"];
                    if ((string)Session["Rol"] != "R")
                    {
                        tabla = BDRegistro.INFOVISITA.Where(x => String.Equals(x.RESERVACION.ESTACION, estacion)).OrderBy(x => x.ID_RESERVACION);
                    }
                    else
                    {
                        tabla = BDRegistro.INFOVISITA.OrderBy(x => x.ID_RESERVACION);
                    }
                }
                else
                {
                    tabla = BDRegistro.INFOVISITA.OrderBy(x => x.ID_RESERVACION);
                }
            }
            int Size_Of_Page = 10;
            int No_Of_Page = (Pagina ?? 1);
            return View(tabla.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        /*
         * Desc: Revisa a cuál organizacion pertenece la reservacion de la persona identificada or cedula.
         * Requiere: el id de la reservacion a la que pertenece la persona y su cedula
         * Devuelve: vista de la pagina con la información de registro de la persona determinada por cedula.
         */ 
        [Authorize]
        public ActionResult Details(String idRes, String ced)
        {
            V_RESERVACION res = BDRegistro.V_RESERVACION.Find(idRes);
            
            if (res.ANFITRIONA.Equals("01") || res.ANFITRIONA == null)
            {
                return RedirectToAction("DetailsOET", new { idR = idRes, cedula = ced });
            }
            else
            {
                return RedirectToAction("DetailsESINTRO", new { idR = idRes, cedula = ced });
            }
        }


        /*
         * Desc: Muestra la pagina del formulario con la informacion ingresada por un visitante de la OET
         * Requiere: id de la reservacion y la cedula de la persona
         * Devulve: vista de la pagina con la informacion de la persona registrada
         */
        [Authorize]
        public ActionResult DetailsOET(String idR, String cedula)
        {

            if (idR == null || cedula == null)
            {   
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            INFOVISITA iInfoVisita = BDRegistro.INFOVISITA.Find(idR, cedula);
            if (iInfoVisita == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (iInfoVisita.PERSONA.GENERO.Equals("M"))
                {
                    ViewBag.sexoList = ViewResources.Resources.oet_masc;
                }
                else
                {
                    ViewBag.sexoList = ViewResources.Resources.oet_fem;
                }

                switch (iInfoVisita.DIETA)
                {
                    case "No Restriction":
                    case "Sin restricción":
                    case "Pas de restrictions":
                        ViewBag.dieta = ViewResources.Resources.oet_sinrestr;
                        break;
                    case "Vegetarian":
                    case "Vegetariano":
                    case "Végétarien":
                        ViewBag.dieta = ViewResources.Resources.oet_vege;
                        break;

                    case "Vegan":
                    case "Vegano":
                        ViewBag.dieta = ViewResources.Resources.oet_vegano;
                        break;
                }

                ViewBag.Carne = iInfoVisita.CARNE;
                ViewBag.Pollo = iInfoVisita.POLLO;
                ViewBag.Pescado = iInfoVisita.PESCADO;
                ViewBag.Cerdo = iInfoVisita.CERDO;
                ViewBag.idRes = idR;
                ViewBag.ced = cedula;
                if (iInfoVisita.PERSONA.PAISI != null)
                { 
                    iInfoVisita.PERSONA.PAISI.NOMBRE = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(iInfoVisita.PERSONA.PAISI.NOMBRE));
                }
                if (iInfoVisita.PERSONA.NACIONALIDADI != null)
                {
                    iInfoVisita.PERSONA.NACIONALIDADI.GENTILICIO = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(iInfoVisita.PERSONA.NACIONALIDADI.GENTILICIO));
                }
                return View(iInfoVisita);
            }
            
        }

        /*
        * Desc: Muestra la pagina del formulario con la informacion ingresada por un visitante de ESINTRO
        * Requiere: id de la reservacion y la cedula de la persona
        * Devuelve: vista de la pagina con la informacion de la persona registrada
        */
        [Authorize]
        public ActionResult DetailsESINTRO(String idR, String cedula)
        {

            if (idR == null || cedula == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INFOVISITA iInfoVisita = BDRegistro.INFOVISITA.Find(idR, cedula);
            if (iInfoVisita == null)
            {
                return HttpNotFound();
            }
            else
            {
               
                if (iInfoVisita.PERSONA.GENERO.Equals("M"))
                {
                    ViewBag.sexo = ViewResources.Resources.oet_masc;
                }
                else
                {
                    ViewBag.sexo = ViewResources.Resources.oet_fem;
                }
               
                switch (iInfoVisita.DIETA) {
                    case "No Restriction":
                        ViewBag.dieta = ViewResources.Resources.oet_sinrestr;
                            break; 
                    case "Vegetarian":
                        ViewBag.dieta = ViewResources.Resources.oet_vege;
                        break;
                    case "Vegan":
                        ViewBag.dieta = ViewResources.Resources.oet_vegano;
                        break;
                }
                ViewBag.Carne = iInfoVisita.CARNE;
                ViewBag.Pollo = iInfoVisita.POLLO;
                ViewBag.Pescado = iInfoVisita.PESCADO;
                ViewBag.Cerdo = iInfoVisita.CERDO;
                ViewBag.idRes = idR;
                ViewBag.ced = cedula;
                if (iInfoVisita.PERSONA.PAISI != null)
                {
                    iInfoVisita.PERSONA.PAISI.NOMBRE = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(iInfoVisita.PERSONA.PAISI.NOMBRE));
                }
                if (iInfoVisita.PERSONA.NACIONALIDADI != null)
                {
                    iInfoVisita.PERSONA.NACIONALIDADI.GENTILICIO = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(iInfoVisita.PERSONA.NACIONALIDADI.GENTILICIO));
                }

            }
            return View(iInfoVisita);
        }

        /*
         * Desc: Revisa la organizacion a la que esta asociada la reserva y redirecciona a la vista segun sea el caso
         * Requiere: el id de la reservación asociada y la cedula de la persona
         * Devuelve: un enlace a la vista correspondiente según la organización de a reservación
         * 
         */ 
        public ActionResult Edit(String idRes, String ced)
        { 
            V_RESERVACION res = BDRegistro.V_RESERVACION.Find(idRes);

            if (res.ANFITRIONA.Equals("01"))
            {
                return RedirectToAction("EditOET");
            }
            else
            {
                return RedirectToAction("EditESINTRO");
            }
        }

        /**
         * Desc: Muestra el formulario con la información ingresada previamente por el visitante, para dar 
         * la posibilidad de editarla o llenar los campos faltantes.
         * Requiere: id de la reservación y cédula del visitnate que llenó el formulario
         * Devuelve:La vista del formulario con la información de un visitante de la OET
         */ 
        public ActionResult EditOET(String idRes, String ced)
        {
            if (idRes == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INFOVISITA iInfoVisita = BDRegistro.INFOVISITA.Find(idRes, ced);
            if (iInfoVisita == null)
            {
                return HttpNotFound();
            }
            else
            {
                var listSexo = new List<SelectListItem>();
                if (iInfoVisita.PERSONA.GENERO.Equals("M"))
                {
                    listSexo.Add(new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_masc, Value = ViewResources.Resources.oet_masc });
                    listSexo.Add(new SelectListItem { Text = ViewResources.Resources.oet_fem, Value = ViewResources.Resources.oet_fem });
                }
                else
                {
                    listSexo.Add(new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_fem, Value = ViewResources.Resources.oet_fem });
                    listSexo.Add(new SelectListItem { Text = ViewResources.Resources.oet_masc, Value = ViewResources.Resources.oet_masc });
                }

                var listDieta = new List<SelectListItem>();
                switch (iInfoVisita.DIETA)
                {
                    case "Vegetarian":
                    case "Vegetariano":
                    case "Végétarien":
                        listDieta.Add(new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_vege, Value = ViewResources.Resources.oet_vege });
                        listDieta.Add(new SelectListItem { Text = ViewResources.Resources.oet_sinrestr, Value = ViewResources.Resources.oet_sinrestr });
                        listDieta.Add(new SelectListItem { Text = ViewResources.Resources.oet_vegano, Value = ViewResources.Resources.oet_vegano });
                        break;

                    case "Vegan":
                    case "Vegano":
                        listDieta.Add(new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_vegano, Value = "Vegan" });
                        listDieta.Add(new SelectListItem { Text = ViewResources.Resources.oet_sinrestr, Value = ViewResources.Resources.oet_sinrestr });
                        listDieta.Add(new SelectListItem { Text = ViewResources.Resources.oet_vege, Value = ViewResources.Resources.oet_vege });
                        break;

                    default:
                        listDieta.Add(new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_sinrestr, Value = ViewResources.Resources.oet_sinrestr });
                        listDieta.Add(new SelectListItem { Text = ViewResources.Resources.oet_vege, Value = ViewResources.Resources.oet_vege });
                        listDieta.Add(new SelectListItem { Text = ViewResources.Resources.oet_vegano, Value = ViewResources.Resources.oet_vegano });
                        break;
                }

                var listaPosicion = new List<SelectListItem>();
                String[] posiciones =  { ViewResources.Resources.oet_pos1, ViewResources.Resources.oet_pos2,
                    ViewResources.Resources.oet_pos3, ViewResources.Resources.oet_pos4,
                    ViewResources.Resources.oet_pos5, ViewResources.Resources.oet_pos6,
                    ViewResources.Resources.oet_pos7, ViewResources.Resources.oet_pos8,
                    ViewResources.Resources.oet_pos9, ViewResources.Resources.oet_pos10,
                    ViewResources.Resources.oet_pos11, ViewResources.Resources.oet_pos12 };
                

                foreach (String pos in posiciones)
                {
                    listaPosicion.Add(
                       (iInfoVisita.PERSONA.POSICION == pos) ?
                           (new SelectListItem { Selected = true, Text = pos, Value = pos }) :
                           (new SelectListItem { Text = pos, Value = pos })
                    );
                }

                var listaPropositos = new List<SelectListItem>();
                String[] propositos = { ViewResources.Resources.oet_prop1, ViewResources.Resources.oet_prop2,
                    ViewResources.Resources.oet_prop3, ViewResources.Resources.oet_prop4,
                    ViewResources.Resources.oet_prop5, ViewResources.Resources.oet_prop6,
                    ViewResources.Resources.oet_prop7, ViewResources.Resources.oet_prop8,
                    ViewResources.Resources.oet_prop9, ViewResources.Resources.oet_prop10,
                    ViewResources.Resources.oet_prop11 };

                foreach (String prop in propositos)
                {
                    listaPropositos.Add( (iInfoVisita.PROPOSITO == prop) ?
                        (new SelectListItem { Selected = true, Text = prop, Value = prop}) :
                        (new SelectListItem { Text = prop, Value = prop})
                    );
                }


                var listaRoles = new List<SelectListItem>();
                String [] roles = { ViewResources.Resources.oet_rol1, ViewResources.Resources.oet_rol2, ViewResources.Resources.oet_rol3, ViewResources.Resources.oet_rol4, ViewResources.Resources.oet_rol5 };
                foreach (String rol in roles)
                {
                    listaRoles.Add((iInfoVisita.ROL_CURSO == rol) ?
                        (new SelectListItem { Selected = true, Text = rol, Value = rol }) :
                        (new SelectListItem { Text = rol, Value = rol })
                    );
                }

                ViewBag.listRoles = listaRoles;
                ViewBag.listProposito = listaPropositos;
                ViewBag.listSexo = listSexo;
                ViewBag.listaPosicion = listaPosicion;
                ViewBag.listDieta = listDieta;
                ViewBag.Carne = iInfoVisita.CARNE;
                ViewBag.Pollo = iInfoVisita.POLLO;
                ViewBag.Pescado = iInfoVisita.PESCADO;
                ViewBag.Cerdo = iInfoVisita.CERDO;
                ViewBag.idRes = idRes;
                ViewBag.ced = ced;
                if (iInfoVisita.PERSONA.PAISI != null)
                {
                    iInfoVisita.PERSONA.PAISI.NOMBRE = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(iInfoVisita.PERSONA.PAISI.NOMBRE));
                }
                if (iInfoVisita.PERSONA.NACIONALIDADI != null)
                {
                    iInfoVisita.PERSONA.NACIONALIDADI.GENTILICIO = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(iInfoVisita.PERSONA.NACIONALIDADI.GENTILICIO));
                }
                return View(iInfoVisita);
            }
            
        }

        /**
         * Desc: Muestra el formulario con la información ingresada previamente por el visitante, para dar 
         * la posibilidad de editarla o llenar los campos faltantes.
         * Requiere: id de la reservación y cédula del visitnate que llenó el formulario
         * Devuelve:La vista del formulario con la información de un visitante de ESINTRO
         */
        public ActionResult EditESINTRO(String idRes, String ced)
        {
            if (idRes == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INFOVISITA iInfoVisita = BDRegistro.INFOVISITA.Find(idRes, ced);
            
            if (iInfoVisita == null)
            {
                return HttpNotFound();
            }
            else
            {
                var listSexo = new List<SelectListItem>();
                if (iInfoVisita.PERSONA.GENERO.Equals("M"))
                {
                    listSexo.Add(new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_masc, Value = ViewResources.Resources.oet_masc });
                    listSexo.Add(new SelectListItem { Text = ViewResources.Resources.oet_fem, Value = ViewResources.Resources.oet_fem });
                    
                }
                else
                {
                    listSexo.Add(new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_fem, Value = ViewResources.Resources.oet_fem });
                    listSexo.Add(new SelectListItem { Text = ViewResources.Resources.oet_masc, Value = ViewResources.Resources.oet_masc });
                }
                ViewBag.listSexo = listSexo;

                var listDieta = new List<SelectListItem>();
                switch (iInfoVisita.DIETA)
                {                   
                    case "Vegetarian":
                        listDieta.Add(new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_vege, Value = ViewResources.Resources.oet_vege });
                        listDieta.Add(new SelectListItem { Text = ViewResources.Resources.oet_sinrestr, Value = ViewResources.Resources.oet_sinrestr });
                        listDieta.Add(new SelectListItem { Text = ViewResources.Resources.oet_vegano, Value = ViewResources.Resources.oet_vegano });
                        break;

                    case "Vegan":
                        listDieta.Add(new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_vegano, Value = "Vegan" });
                        listDieta.Add(new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_sinrestr, Value = ViewResources.Resources.oet_sinrestr });
                        listDieta.Add(new SelectListItem { Text = ViewResources.Resources.oet_vege, Value = ViewResources.Resources.oet_vege });                      
                        break;

                    default:
                        listDieta.Add(new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_sinrestr, Value = ViewResources.Resources.oet_sinrestr });
                        listDieta.Add(new SelectListItem { Text = ViewResources.Resources.oet_vege, Value = ViewResources.Resources.oet_vege });
                        listDieta.Add(new SelectListItem { Text = ViewResources.Resources.oet_vegano, Value = ViewResources.Resources.oet_vegano });
                        break;
                }
                ViewBag.listDieta = listDieta;
                ViewData["DIETA"] = listDieta;
                ViewBag.Carne = iInfoVisita.CARNE;
                ViewBag.Pollo = iInfoVisita.POLLO;
                ViewBag.Pescado = iInfoVisita.PESCADO;
                ViewBag.Cerdo = iInfoVisita.CERDO;
                if (iInfoVisita.PERSONA.PAISI != null)
                {
                    iInfoVisita.PERSONA.PAISI.NOMBRE = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(iInfoVisita.PERSONA.PAISI.NOMBRE));
                }
                if (iInfoVisita.PERSONA.NACIONALIDADI != null)
                {
                    iInfoVisita.PERSONA.NACIONALIDADI.GENTILICIO = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(iInfoVisita.PERSONA.NACIONALIDADI.GENTILICIO));
                }
            }
            return View(iInfoVisita);
        }



        /**
        * Desc: Envía la información del formulario para guardar los cambios realizados.
        * Requiere: una entidad infovisita con la información a guardar
        * Devuelve: La vista del formulario de OET con la información modificada
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOET(INFOVISITA infov, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                /*if (infov.PERSONA.GENERO == ViewResources.Resources.oet_fem)
                {
                    infov.PERSONA.GENERO = '1'.ToString();

                }
                else
                {
                    infov.PERSONA.GENERO = '0'.ToString();
                }*/
                string genero = collection["PERSONA.GENERO"];
                if (genero == ViewResources.Resources.oet_fem) // si es femenino
                {
                    infov.PERSONA.GENERO = 'F'.ToString();
                }
                else
                {
                    infov.PERSONA.GENERO = 'M'.ToString(); //es masculino
                }

                string nominst = (string)collection["PERSONA.INSTITUCIONI.FULL_NAME"];
                V_INSTITUCION inst = BDRegistro.V_INSTITUCION.Where(x => String.Equals(x.FULL_NAME, nominst)).FirstOrDefault();
                infov.PERSONA.INSTITUCION = (inst == null) ? (int?)null : inst.CAT_INSTITUCION;
                infov.PERSONA.INSTITUCIONI = inst;

                string nompais = (string)collection["PERSONA.PAISI.NOMBRE"].ToUpper(); ;
                V_PAISES pais = BDRegistro.V_PAISES.Where(x => String.Equals(x.NOMBRE, nompais)).FirstOrDefault();
                infov.PERSONA.PAIS = (pais == null) ? null : pais.ISO;
                infov.PERSONA.PAISI = pais;

                string gentpais = (string)collection["PERSONA.NACIONALIDADI.GENTILICIO"].ToUpper();
                V_PAISES nacion = BDRegistro.V_PAISES.Where(x => String.Equals(x.GENTILICIO, gentpais)).FirstOrDefault();
                infov.PERSONA.NACIONALIDAD = (nacion == null) ? null : nacion.ISO;
                infov.PERSONA.NACIONALIDADI = nacion;

                infov.CARNE = true;
                infov.POLLO = true;
                infov.CERDO = true;
                infov.PESCADO = true;
                
                BDRegistro.Entry(infov).State = EntityState.Modified;
                BDRegistro.Entry(infov.PERSONA).State = EntityState.Modified;
                BDRegistro.SaveChanges();
                return RedirectToAction("DetailsOET", new { idR = infov.ID_RESERVACION, cedula = infov.CEDULA});
            }
            return View();

        }

        /**
        * Desc: Envía la información del formulario para guardar los cambios realizados.
        * Requiere: una entidad infovisita con la información a guardar
        * Devuelve: La vista del formulario de ESINTRO con la información modificada
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditESINTRO(INFOVISITA infov, FormCollection collection)
        {

            if (ModelState.IsValid)
            {

                /* if (infov.PERSONA.GENERO == ViewResources.Resources.oet_fem)
                 {
                     infov.PERSONA.GENERO = '1'.ToString();

                 }
                 else
                 {
                     infov.PERSONA.GENERO = '0'.ToString();
                 }*/

                string genero = collection["PERSONA.GENERO"];
                if (genero == ViewResources.Resources.oet_fem) // si es femenino
                {
                    infov.PERSONA.GENERO = 'F'.ToString();
                }
                else
                {
                    infov.PERSONA.GENERO = 'M'.ToString(); //es masculino
                }

                if (infov.DIETA.Equals(ViewResources.Resources.oet_sinrestr))
                {
                    infov.DIETA = "No Restriction";
                }
                else {
                    if (infov.DIETA.Equals(ViewResources.Resources.oet_vege))
                    {
                        infov.DIETA = "Vegetarian";
                    }
                    else
                    {
                        infov.DIETA = "Vegan";
                    }
                }

                string nompais = (string)collection["PERSONA.PAISI.NOMBRE"].ToUpper(); ;
                V_PAISES pais = BDRegistro.V_PAISES.Where(x => String.Equals(x.NOMBRE, nompais)).FirstOrDefault();
                infov.PERSONA.PAIS = (pais == null) ? null : pais.ISO;
                infov.PERSONA.PAISI = pais;

                string gentpais = (string)collection["PERSONA.NACIONALIDADI.GENTILICIO"].ToUpper();
                V_PAISES nacion = BDRegistro.V_PAISES.Where(x => String.Equals(x.GENTILICIO, gentpais)).FirstOrDefault();
                infov.PERSONA.NACIONALIDAD = (nacion == null) ? null : nacion.ISO;
                infov.PERSONA.NACIONALIDADI = nacion;

                BDRegistro.Entry(infov).State = EntityState.Modified;
                BDRegistro.Entry(infov.PERSONA).State = EntityState.Modified;
                BDRegistro.SaveChanges();
                return RedirectToAction("DetailsESINTRO", new { idR = infov.ID_RESERVACION, cedula = infov.CEDULA });
            }
            return View();
        }


        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Visitantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONTACTO,TYPE,FIRST_NAME,LAST_NAME,BIRTH_DAY,BIRTH_PLACE,INSTITUTION,CATEGORY,GENDER,PHONE,PHONE_AREA_CODE,FAX,FAX_AREA_CODE,EMERGENCY_CONTACT1,EMERGENCY_NUMBER1,EMERGENCY_E_MAIL1,EMERGENCY_CONTACT2,EMERGENCY_NUMBER2,EMERGENCY_E_MAIL2,E_MAIL,IDENTIFICATION_TYPE,IDENTIFICATION,EXPIRATION_DATE,COUNTRY_BIRTH,NACIONALITY,ETHNIC_GROUP,ADDRESS_L1,AREA_OF_INTEREST,ACADEMIC_DEGREE,SECRET_QUESTION,ANSWER,PASSWORD,GRUPO,EMERGENCY_NUMBER1_CODE,EMERGENCY_NUMBER2_CODE,HOME_PHONE,HOME_PHONE_AREA,ADDRESS_COUNTRY,ADDRESS_L2,CITY,STATE,ZIP_CODE,CAT_TIPO_ESTUDIANTE,STATION,NO_DIETARY_RESTRICTIONS,VEGETARIAN,VEGAN,BEEF,CHICKEN,PORK,FISH,OTHER_DIETARY,COUNTRY_EMITS,CREADO,MODIFICADO,NICKNAME,DEPARTAMENT,POSITION,SKYPE,WEBSITE,CKOTHER_DIETARY_RESTRIC,TXOTHER_DIETARY_RESTRIC,PRIN,RELATIONSHIP_CONTACT1,RELATIONSHIP_CONTACT2,INSURANCE_COMPANY,INSURANCE_NUMBER,INSURANCE_EXPIRATION,ESPECIALIDAD,DATO_ESTACION,CONTACTO_RESERVAS,SHARE_INFORMATION,PHONE_COUNTRY_CODE,FAX_COUNTRY_CODE,EMERGENCY_COUNTRY_NUM_CODE1,EMERGENCY_COUNTRY_NUM_CODE2,HORA_CREACION,E_MAIL2,NUMPREREGISTRO,IDRESERVACION,PROPOSITO,IDGRUPO,COMOENTERO,FECHA,NOMCURSO,NUMCURSO,ROLCURSO,NOMPROYECTO,INVERSIONES,FUENTE,RESOLUCION,PERMISO,EXPIRACION,ALERGIAS")] PREREGISTROCONTACTO pREREGISTROCONTACTO)
        {
            if (ModelState.IsValid)
            {
                db.PREREGISTROCONTACTO.Add(pREREGISTROCONTACTO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pREREGISTROCONTACTO);
        }*/

    
        // POST: Visitantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONTACTO,TYPE,FIRST_NAME,LAST_NAME,BIRTH_DAY,BIRTH_PLACE,INSTITUTION,CATEGORY,GENDER,PHONE,PHONE_AREA_CODE,FAX,FAX_AREA_CODE,EMERGENCY_CONTACT1,EMERGENCY_NUMBER1,EMERGENCY_E_MAIL1,EMERGENCY_CONTACT2,EMERGENCY_NUMBER2,EMERGENCY_E_MAIL2,E_MAIL,IDENTIFICATION_TYPE,IDENTIFICATION,EXPIRATION_DATE,COUNTRY_BIRTH,NACIONALITY,ETHNIC_GROUP,ADDRESS_L1,AREA_OF_INTEREST,ACADEMIC_DEGREE,SECRET_QUESTION,ANSWER,PASSWORD,GRUPO,EMERGENCY_NUMBER1_CODE,EMERGENCY_NUMBER2_CODE,HOME_PHONE,HOME_PHONE_AREA,ADDRESS_COUNTRY,ADDRESS_L2,CITY,STATE,ZIP_CODE,CAT_TIPO_ESTUDIANTE,STATION,NO_DIETARY_RESTRICTIONS,VEGETARIAN,VEGAN,BEEF,CHICKEN,PORK,FISH,OTHER_DIETARY,COUNTRY_EMITS,CREADO,MODIFICADO,NICKNAME,DEPARTAMENT,POSITION,SKYPE,WEBSITE,CKOTHER_DIETARY_RESTRIC,TXOTHER_DIETARY_RESTRIC,PRIN,RELATIONSHIP_CONTACT1,RELATIONSHIP_CONTACT2,INSURANCE_COMPANY,INSURANCE_NUMBER,INSURANCE_EXPIRATION,ESPECIALIDAD,DATO_ESTACION,CONTACTO_RESERVAS,SHARE_INFORMATION,PHONE_COUNTRY_CODE,FAX_COUNTRY_CODE,EMERGENCY_COUNTRY_NUM_CODE1,EMERGENCY_COUNTRY_NUM_CODE2,HORA_CREACION,E_MAIL2,NUMPREREGISTRO,IDRESERVACION,PROPOSITO,IDGRUPO,COMOENTERO,FECHA,NOMCURSO,NUMCURSO,ROLCURSO,NOMPROYECTO,INVERSIONES,FUENTE,RESOLUCION,PERMISO,EXPIRACION,ALERGIAS")] PREREGISTROCONTACTO pREREGISTROCONTACTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pREREGISTROCONTACTO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pREREGISTROCONTACTO);
        }*/

        /*
        * Desc: Muestra un mensaje de confirmación para cambiar el estado de una persona en un registro.
        * Requiere: id de la reservación asociada y la cedula o identificación de la persona
        * Devuelve: La vista del mensaje de confirmación
        */
        [Authorize]
        public ActionResult Delete(string idRes, string idPer)
        {
            if (idRes == null || idPer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INFOVISITA iInfoVisita = BDRegistro.INFOVISITA.Find(idRes, idPer);
           
            if (iInfoVisita == null)
            {
                return HttpNotFound();
            }
            return View(iInfoVisita);
        }

        /*
        * Desc: Se confirma el cambio del estado de la persona en la reservación. Pasa de activo a inactivo o viceversa.
        * Requiere: id de la reservación y identificación de la persona.
        * Devuelve: La vista de la pagina principal de visitantes
        */
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string idRes, string idPer)
        {
            if (idRes == null || idPer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INFOVISITA iInfoVisita = BDRegistro.INFOVISITA.Find(idRes, idPer);
            if (iInfoVisita == null)
            {
                return HttpNotFound();
            }
            if (iInfoVisita.ESTADO != "F")
            {
                iInfoVisita.ESTADO = iInfoVisita.ESTADO == "A" ? "I" : "A";
            }
            BDRegistro.SaveChanges();

            return RedirectToAction("Index");
        }


        [Authorize]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BDRegistro.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize]
        public ActionResult Refrescar()
        {
            return Redirect(Request.UrlReferrer.ToString());
        }


        public ActionResult Instituciones(string term)
        {
            var result = (  from a in BDRegistro.V_INSTITUCION
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
