using RegistroVisitantes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RegistroVisitantes.Controllers
{
    public class UsuarioController : Controller
    {
        private BDContactos db = new BDContactos();
        public ActionResult Ingresar()
        {
            return View();
        }
         
        [HttpPost]
        public ActionResult Ingresar(USUARIO usuarioNuevo, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                
                db.USUARIO.Add(usuarioNuevo);
                db.SaveChanges();
                
                ModelState.Clear();
                ViewBag.Message = usuarioNuevo.NOMBRE + " " + usuarioNuevo.APELLIDO + " se ingresó exitosamente.";
            }
            return Login(returnUrl);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            FormsAuthentication.SignOut();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        [HttpPost]
        public ActionResult Login(USUARIO user)
        {
            
            USUARIO usr = db.USUARIO.Where(u => u.USERNAME == user.USERNAME && u.PASSWORD == user.PASSWORD).FirstOrDefault();
            if (usr != null)
            {
                Session["Id"] = usr.ID.ToString();
                Session["Username"] = usr.USERNAME.ToString();
                FormsAuthentication.SetAuthCookie(usr.USERNAME.ToString(), true);
                resetRequest();
                return RedirectToAction("Logueado");
            }
            else
            {
                ModelState.AddModelError("", "El nombre de usuario y la contraseña no coinciden");
            }

            
            return View();
        }


        [HttpPost]
        public ActionResult Logout()
        {
            Session["Id"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Logueado");
        }

        private void resetRequest()
        {
            var authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var roles = authTicket.UserData.Split(',');
                    System.Web.HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), roles);
                }
            }
        }

        public ActionResult Logueado()
        {
            if (Session["Id"] != null)
            {
                return RedirectToAction("Index", "Formulario");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}