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
        private BDRegistro db = new BDRegistro();
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
            if(Request.IsAuthenticated) { 
                FormsAuthentication.SignOut();
                Session["Id"] = null;
                Session["Username"] = null;
                Session["Nombre"] = null;
                Session["Apellido"] = null;
                Session["Rol"] = null;
                Session["Estacion"] = null;
                Session["IdEstacion"] = null;
                Session["Siglas"] = null;
                ViewBag.ReturnUrl = returnUrl;
                return Redirect(Request.RawUrl);
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }



        [HttpPost]
        public ActionResult Login(USUARIO user)
        {
            USUARIO usr = db.USUARIO.Where(u => u.USUAR == user.USUAR && u.CONTRASENA == user.CONTRASENA).FirstOrDefault();
            if (usr != null)
            {
                Session["Id"] = usr.ID.ToString();
                Session["Username"] = usr.USUAR.ToString();
                Session["Nombre"] = usr.NOMBRE.ToString();
                Session["Apellido"] = usr.APELLIDO.ToString();
                Session["Rol"] = usr.ROL.ToString();
                //SII014192819200.7082987519=LS, SII014192819200.2788020223=PV, SII017112627200.5981444351=LC, SII014548761600.1313183743=NAO, SII014548761600.7018216447=CRO
                //LS=La Selva, PV=Palo Verde, LC=Las Cruces, CRO=Costa Rican Offices, NAO=North American Offices
                Session["Estacion"] = usr.ESTACION.NOMBRE;
                Session["IdEstacion"] = usr.ESTACION.ID;
                Session["Siglas"] = usr.ESTACION.SIGLAS;
                FormsAuthentication.SetAuthCookie(usr.USUAR.ToString(), true);
                resetRequest();
                return RedirectToAction("Logueado");
            }
            else
            {
                ModelState.AddModelError("", "El nombre de usuario y la contraseña no coinciden");
            }

            
            return View();
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
                if(ViewBag.ReturnUrl == null)
                { 
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(ViewBag.ReturnUrl);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}