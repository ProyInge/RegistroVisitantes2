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

namespace RegistroVisitantes.Controllers
{
    public class VisitantesController : Controller
    {
        private BDReservas BDReservas = new BDReservas();
        private BDRegistro BDRegistro = new BDRegistro();

        // GET: Visitantes
        [Authorize]
        public ActionResult Index(String idRes, int? Pagina)
        {
            IQueryable<INFOVISITA> tabla;
            if (idRes != null && !idRes.Equals(""))
            {
                if (Session["Rol"] != null)
                {
                    string rol = (string)Session["IdEstacion"];
                    if ((string)Session["Rol"] == "S")
                    {
                        tabla = BDRegistro.INFOVISITA.Where(x => String.Equals(x.ID_RESERVACION, idRes) && String.Equals(x.RESERVACION.ESTACION, Session["IdReservacion"])).OrderBy(x => x.ID_RESERVACION);
                    }
                    else if ((string)Session["Rol"] == "A")
                    {
                        tabla = BDRegistro.INFOVISITA.Where(x => String.Equals(x.ID_RESERVACION, idRes) && String.Equals(x.RESERVACION.ESTACION, Session["IdReservacion"])).OrderBy(x => x.ID_RESERVACION);
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
                ViewBag.idRes = idRes;
            }
            else
            {
                if (Session["Rol"] != null)
                {
                    string estacion = (string)Session["IdEstacion"];
                    if ((string)Session["Rol"] == "S")
                    {
                        tabla = BDRegistro.INFOVISITA.Where(x => String.Equals(x.RESERVACION.ESTACION, estacion)).OrderBy(x => x.ID_RESERVACION);
                    }
                    else if ((string)Session["Rol"] == "A")
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

        // GET: Visitantes/Details/5
        [Authorize]
        public ActionResult Details(String idRes, String ced)
        {
            RESERVACION res = BDReservas.RESERVACION.Find(idRes);

            if (res.ANFITRIONA.Equals("01"))
            {
                return RedirectToAction("DetailsOET", new { idR = idRes, cedula = ced });
            }
            else
            {
                return RedirectToAction("DetailsESINTRO", new { idR = idRes, cedula = ced });
            }
        }

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
                if (iInfoVisita.PERSONA.GENERO.Equals("0"))
                {
                    ViewBag.sexoList = "Male";
                }
                else
                {
                    ViewBag.sexoList = "Female";
                }

                switch (iInfoVisita.DIETA)
                {
                    case "No Restriction":
                        ViewBag.dieta = "No Restriction";
                        break;
                    case "Vegetarian":
                        ViewBag.dieta = "Vegetarian";
                        break;
                    case "Vegan":
                        ViewBag.dieta = "Vegan";
                        break;
                }

                ViewBag.Carne = iInfoVisita.CARNE;
                ViewBag.Pollo = iInfoVisita.POLLO;
                ViewBag.Pescado = iInfoVisita.PESCADO;
                ViewBag.Cerdo = iInfoVisita.CERDO;
            }
                return View(iInfoVisita);
        }

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
                if (iInfoVisita.PERSONA.GENERO.Equals("0"))
                {
                    ViewBag.sexoList =  "Male";
                }
                else
                {
                    ViewBag.sexoList =  "Female" ;
                }
               
                switch (iInfoVisita.DIETA) {
                    case "No Restriction": 
                            ViewBag.dieta = "No Restriction";
                        break;
                    case "Vegetarian":
                        ViewBag.dieta = "Vegetarian";
                        break;
                    case "Vegan":
                        ViewBag.dieta = "Vegan";
                        break;
                }
               
            ViewBag.Carne = iInfoVisita.CARNE;
            ViewBag.Pollo = iInfoVisita.POLLO;
            ViewBag.Pescado = iInfoVisita.PESCADO;
            ViewBag.Cerdo = iInfoVisita.CERDO;

            }
            return View(iInfoVisita);
        }


        public ActionResult Edit(String idRes, String ced)
        {


            RESERVACION res = BDReservas.RESERVACION.Find(idRes);

            if (res.ANFITRIONA.Equals("01"))
            {
                return RedirectToAction("EditOET");
            }
            else
            {
                return RedirectToAction("EditESINTRO");
            }
        }

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
                if (iInfoVisita.PERSONA.GENERO.Equals("0"))
                {
                    listSexo.Add(new SelectListItem { Selected = true, Text = "Male", Value = "Male" });
                    listSexo.Add(new SelectListItem { Text = "Female", Value = "Female" });

                }
                else
                {
                    listSexo.Add(new SelectListItem { Selected = true, Text = "Female", Value = "Female" });
                    listSexo.Add(new SelectListItem { Text = "Male", Value = "Male" });
                }
                ViewBag.listSexo = listSexo;

                var listDieta = new List<SelectListItem>();
                switch (iInfoVisita.DIETA)
                {
                    case "No Restriction":
                        listDieta.Add(new SelectListItem { Selected = true, Text = "No Restriction", Value = "No Restriction" });
                        listDieta.Add(new SelectListItem { Text = "Vegetarian", Value = "Vegetarian" });
                        listDieta.Add(new SelectListItem { Text = "Vegan", Value = "Vegan" });
                        break;
                    case "Vegetarian":
                        listDieta.Add(new SelectListItem { Selected = true, Text = "Vegetarian", Value = "Vegetarian" });
                        listDieta.Add(new SelectListItem { Text = "No Restriction", Value = "No Restriction" });
                        listDieta.Add(new SelectListItem { Text = "Vegan", Value = "Vegan" });
                        break;
                    case "Vegan":
                        listDieta.Add(new SelectListItem { Selected = true, Text = "Vegan", Value = "Vegan" });
                        listDieta.Add(new SelectListItem { Text = "No Restriction", Value = "No Restriction" });
                        listDieta.Add(new SelectListItem { Text = "Vegetarian", Value = "Vegetarian" });
                        break;
                }

                var listaPosicion = new List<SelectListItem>();
                listaPosicion.Add(
                    (iInfoVisita.PERSONA.POSICION == ViewResources.Resources.oet_pos1) ? 
                    (new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_pos1, Value = ViewResources.Resources.oet_pos1 }):
                    (new SelectListItem { Text = ViewResources.Resources.oet_pos1, Value = ViewResources.Resources.oet_pos1 })
                );
                listaPosicion.Add(
                    (iInfoVisita.PERSONA.POSICION == ViewResources.Resources.oet_pos2) ?
                    (new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_pos2, Value = ViewResources.Resources.oet_pos2 }) :
                    (new SelectListItem { Text = ViewResources.Resources.oet_pos2, Value = ViewResources.Resources.oet_pos2 })
                );
                listaPosicion.Add(
                    (iInfoVisita.PERSONA.POSICION == ViewResources.Resources.oet_pos3) ?
                    (new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_pos3, Value = ViewResources.Resources.oet_pos3 }) :
                    (new SelectListItem { Text = ViewResources.Resources.oet_pos3, Value = ViewResources.Resources.oet_pos3 })
                );
                listaPosicion.Add(
                    (iInfoVisita.PERSONA.POSICION == ViewResources.Resources.oet_pos4) ?
                    (new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_pos4, Value = ViewResources.Resources.oet_pos4 }) :
                    (new SelectListItem { Text = ViewResources.Resources.oet_pos4, Value = ViewResources.Resources.oet_pos4 })
                );
                listaPosicion.Add(
                    (iInfoVisita.PERSONA.POSICION == ViewResources.Resources.oet_pos5) ?
                    (new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_pos5, Value = ViewResources.Resources.oet_pos5 }) :
                    (new SelectListItem { Text = ViewResources.Resources.oet_pos5, Value = ViewResources.Resources.oet_pos5 })
                );
                listaPosicion.Add(
                    (iInfoVisita.PERSONA.POSICION == ViewResources.Resources.oet_pos6) ?
                    (new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_pos6, Value = ViewResources.Resources.oet_pos6 }) :
                    (new SelectListItem { Text = ViewResources.Resources.oet_pos6, Value = ViewResources.Resources.oet_pos6 })
                );
                listaPosicion.Add(
                    (iInfoVisita.PERSONA.POSICION == ViewResources.Resources.oet_pos7) ?
                    (new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_pos7, Value = ViewResources.Resources.oet_pos7 }) :
                    (new SelectListItem { Text = ViewResources.Resources.oet_pos7, Value = ViewResources.Resources.oet_pos7 })
                );
                listaPosicion.Add(
                    (iInfoVisita.PERSONA.POSICION == ViewResources.Resources.oet_pos8) ?
                    (new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_pos8, Value = ViewResources.Resources.oet_pos8 }) :
                    (new SelectListItem { Text = ViewResources.Resources.oet_pos8, Value = ViewResources.Resources.oet_pos8 })
                );
                listaPosicion.Add(
                    (iInfoVisita.PERSONA.POSICION == ViewResources.Resources.oet_pos9) ?
                    (new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_pos9, Value = ViewResources.Resources.oet_pos9 }) :
                    (new SelectListItem { Text = ViewResources.Resources.oet_pos9, Value = ViewResources.Resources.oet_pos9 })
                );
                listaPosicion.Add(
                    (iInfoVisita.PERSONA.POSICION == ViewResources.Resources.oet_pos10) ?
                    (new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_pos10, Value = ViewResources.Resources.oet_pos10 }) :
                    (new SelectListItem { Text = ViewResources.Resources.oet_pos10, Value = ViewResources.Resources.oet_pos10 })
                );
                listaPosicion.Add(
                    (iInfoVisita.PERSONA.POSICION == ViewResources.Resources.oet_pos11) ?
                    (new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_pos11, Value = ViewResources.Resources.oet_pos11 }) :
                    (new SelectListItem { Text = ViewResources.Resources.oet_pos11, Value = ViewResources.Resources.oet_pos11 })
                );
                listaPosicion.Add(
                    (iInfoVisita.PERSONA.POSICION == ViewResources.Resources.oet_pos12) ?
                    (new SelectListItem { Selected = true, Text = ViewResources.Resources.oet_pos12, Value = ViewResources.Resources.oet_pos12 }) :
                    (new SelectListItem { Text = ViewResources.Resources.oet_pos12, Value = ViewResources.Resources.oet_pos12 })
                );

                ViewBag.listaPosicion = listaPosicion;
                ViewBag.listDieta = listDieta;
                ViewBag.Carne = iInfoVisita.CARNE;
                ViewBag.Pollo = iInfoVisita.POLLO;
                ViewBag.Pescado = iInfoVisita.PESCADO;
                ViewBag.Cerdo = iInfoVisita.CERDO;

            }
            return View(iInfoVisita);
        }

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
            }else
            {

                var listSexo = new List<SelectListItem>();
                if (iInfoVisita.PERSONA.GENERO.Equals("0"))
                {
                    listSexo.Add(new SelectListItem { Selected = true, Text = "Male", Value = "Male" });
                    listSexo.Add(new SelectListItem { Text = "Female", Value = "Female" });
                    
                }
                else
                {
                    listSexo.Add(new SelectListItem { Selected = true, Text = "Female", Value = "Female" });
                    listSexo.Add(new SelectListItem { Text = "Male", Value = "Male" });
                }
                ViewBag.listSexo = listSexo;

                var listDieta = new List<SelectListItem>();
                switch (iInfoVisita.DIETA)
                {
                    case "No Restriction":
                        listDieta.Add(new SelectListItem { Selected = true, Text = "No Restriction", Value = "No Restriction" });
                        listDieta.Add(new SelectListItem { Text = "Vegetarian", Value = "Vegetarian" });
                        listDieta.Add(new SelectListItem { Text = "Vegan", Value = "Vegan" });
                        break;
                    case "Vegetarian":
                        listDieta.Add(new SelectListItem { Selected = true, Text = "Vegetarian", Value = "Vegetarian" });
                        listDieta.Add(new SelectListItem { Text = "No Restriction", Value = "No Restriction" });
                        listDieta.Add(new SelectListItem { Text = "Vegan", Value = "Vegan" });
                        break;
                    case "Vegan":
                        listDieta.Add(new SelectListItem { Selected = true, Text = "Vegan", Value = "Vegan" });
                        listDieta.Add(new SelectListItem { Selected = true, Text = "No Restriction", Value = "No Restriction" });
                        listDieta.Add(new SelectListItem { Text = "Vegetarian", Value = "Vegetarian" });                      
                        break;
                }
                ViewBag.listDieta = listDieta;
                ViewBag.Carne = iInfoVisita.CARNE;
                ViewBag.Pollo = iInfoVisita.POLLO;
                ViewBag.Pescado = iInfoVisita.PESCADO;
                ViewBag.Cerdo = iInfoVisita.CERDO;

            }
           
            return View(iInfoVisita);
   

        }



        // POST: /visitantes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOET( INFOVISITA infov)
        {
            if (ModelState.IsValid)
            {
                BDRegistro.Entry(infov).State = EntityState.Modified;
                BDRegistro.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }


        // POST: /visitantes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditESINTRO(INFOVISITA infov)
        {

            if (ModelState.IsValid)
            {

                if (infov.PERSONA.GENERO == "Female")
                {
                    infov.PERSONA.GENERO = '1'.ToString();

                }
                else
                {
                    infov.PERSONA.GENERO = '0'.ToString();
                }

                infov.CARNE = true;
                infov.POLLO = true;
                infov.CERDO = true;
                infov.PESCADO = true;
             

                BDRegistro.Entry(infov).State = EntityState.Modified;
                BDRegistro.Entry(infov.PERSONA).State = EntityState.Modified;
                BDRegistro.SaveChanges();
                return RedirectToAction("Index");
             }
            return View();
        }








        // GET: Visitantes/Create
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

        // GET: Visitantes/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            /*if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PREREGISTROCONTACTO pREREGISTROCONTACTO = db.PREREGISTROCONTACTO.Find(id);
            if (pREREGISTROCONTACTO == null)
            {
                return HttpNotFound();
            }
            return View(pREREGISTROCONTACTO);*/
            return null;
        }

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

        //GET: Visitantes/Delete/?idRes=xxxx?idPer=yyyy

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

        // POST: Visitantes/Delete/5
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
            

            iInfoVisita.ESTADO = !iInfoVisita.ESTADO;
            //db.INFOVISITA.Remove(pREREGISTROCONTACTO);
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

    }
}
