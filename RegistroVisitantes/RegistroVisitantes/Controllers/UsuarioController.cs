﻿using RegistroVisitantes.Models;
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
            var query = from x in db.USUARIO
                               where x.USUAR == user.USUAR && x.CONTRASENA == user.CONTRASENA
                               select x;
            var sql = query.ToString();
            USUARIO usr = db.USUARIO.Where(u => u.USUAR == user.USUAR && u.CONTRASENA == user.CONTRASENA).FirstOrDefault();
            if (usr != null)
            {
                Session["Id"] = usr.ID.ToString();
                Session["Username"] = usr.USUAR.ToString();
                Session["Nombre"] = usr.NOMBRE.ToString();
                Session["Apellido"] = usr.APELLIDO.ToString();
                Session["Rol"] = usr.ROL.ToString();
                //LS=La Selva, PV=Palo Verde, LC=Las Cruces, CRO=Costa Rican Offices, NAO=North American Offices
                Session["Estacion"] = usr.ESTACION.NOMBRE;
                Session["IdEstacion"] = usr.ESTACION.ID;
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