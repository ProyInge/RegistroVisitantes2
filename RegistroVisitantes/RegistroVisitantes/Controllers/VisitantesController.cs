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

namespace RegistroVisitantes.Controllers
{
    public class VisitantesController : Controller
    {
        private BDReservas db = new BDReservas();

        // GET: Visitantes
        public ActionResult Index(String id, int? Pagina)
        {
            IQueryable<INFOVISITA> pREREGISTRO;
            if (id != null && !id.Equals(""))
            {
                pREREGISTRO = db.INFOVISITA.Where(x => String.Equals(x.ID_RESERVACION, id)).OrderBy(x => x.ID_RESERVACION);

            }
            else
            {
                pREREGISTRO = db.INFOVISITA.OrderBy(x => x.ID_RESERVACION);
            }


            int Size_Of_Page = 10;
            int No_Of_Page = (Pagina ?? 1);
            return View(pREREGISTRO.ToPagedList(No_Of_Page, Size_Of_Page));
            //return null;
        }

        public ActionResult Reserva(string id, int? Pagina)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pREREGISTRO = db.INFOVISITA.Where(x => String.Equals(x.ID_RESERVACION, id)).OrderBy(x => x.ID_RESERVACION);
            int Size_Of_Page = 10;
            int No_Of_Page = (Pagina ?? 1);
            return View(pREREGISTRO.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        // GET: Visitantes/Details/5
        public ActionResult Details(int? id)
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

        // GET: Visitantes/Create
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
        
        // GET: Visitantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INFOVISITA pREREGISTROCONTACTO = db.INFOVISITA.Find(id);
            if (pREREGISTROCONTACTO == null)
            {
                return HttpNotFound();
            }
            return View(pREREGISTROCONTACTO);
        }

        // POST: Visitantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            INFOVISITA pREREGISTROCONTACTO = db.INFOVISITA.Find(id);
            db.INFOVISITA.Remove(pREREGISTROCONTACTO);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
