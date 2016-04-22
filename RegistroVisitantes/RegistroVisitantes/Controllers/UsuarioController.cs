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
        public ActionResult Ingresar(Usuario usuarioNuevo)
        {
            if (ModelState.IsValid)
            {
                using (UsuarioDbContext db = new UsuarioDbContext())
                {
                    db.usuario.Add(usuarioNuevo);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = usuarioNuevo.Nombre + " " + usuarioNuevo.Apellido + " se ingresó exitosamente.";
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(Usuario user)
        {
            using (UsuarioDbContext db = new UsuarioDbContext())
            { 
                var usr = db.usuario.Where(u => u.Username == user.Username && u.Password == user.Password).FirstOrDefault();
                if (Session["Id"] != null)
                {
                    Session["Id"] = usr.Id.ToString();
                    Session["Username"] = usr.Username.ToString();
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
            if(Session["Id"] != null)
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