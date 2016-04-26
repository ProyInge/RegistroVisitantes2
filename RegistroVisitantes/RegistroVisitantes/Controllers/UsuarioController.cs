using RegistroVisitantes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistroVisitantes.Controllers
{
    public class UsuarioController : Controller
    {
        public ActionResult Ingresar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ingresar(USUARIO usuarioNuevo)
        {
            if (ModelState.IsValid)
            {
                using (BDContactos db = new BDContactos())
                {
                    db.USUARIO.Add(usuarioNuevo);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = usuarioNuevo.NOMBRE + " " + usuarioNuevo.APELLIDO + " se ingresó exitosamente.";
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(USUARIO user)
        {
            using (BDContactos db = new BDContactos())
            {
                var usr = db.USUARIO.Where(u => u.USERNAME == user.USERNAME && u.PASSWORD == user.PASSWORD).FirstOrDefault();
                if (Session["Id"] != null)
                {
                    Session["Id"] = usr.ID.ToString();
                    Session["Username"] = usr.USERNAME.ToString();
                    return RedirectToAction("Logueado");
                }
                else
                {
                    ModelState.AddModelError("", "El nombre de usuario y la contraseña no coinciden");
                }

            }
            return View();
        }

        public ActionResult Logueado()
        {
            if (Session["Id"] != null)
            {
                return RedirectToAction("Home", "Registro");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}