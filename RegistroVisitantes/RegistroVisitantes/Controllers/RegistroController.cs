using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroVisitantes.Models;

namespace RegistroVisitantes.Controllers
{
    public class RegistroController : Controller
    {
        private EntitiesContactos BDContac = new EntitiesContactos();
        private EntitiesContactos BDReserv = new EntitiesContactos();

        public CONTACTO obtieneInvestigador(string correo)
        {
            CONTACTO cont = BDContac.CONTACTO.SingleOrDefault(c => c.E_MAIL == correo);
            return cont;
        }

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

        public string Prueba()
        {
            string resultado = "HOLA MUNDO";
            return resultado;
        }
    }
}
