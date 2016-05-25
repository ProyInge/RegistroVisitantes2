using RegistroVisitantes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            var listSexo = new List<SelectListItem>();
            listSexo.Add(new SelectListItem { Text = "Hombre", Value = "M" });
            listSexo.Add(new SelectListItem { Selected = true, Text = "Mujer", Value = "F" });
            var listRol = new List<SelectListItem>();
            listRol.Add(new SelectListItem { Selected = true, Text = "Secretario", Value = "S" });
            listRol.Add(new SelectListItem { Text = "Superusuario", Value = "R" });
            listRol.Add(new SelectListItem { Text = "Administrador", Value = "A" });
            var listEstacion = new List<SelectListItem>();
            listEstacion.Add(new SelectListItem { Selected = true, Text = "La Selva (LS)", Value = "SII014192819200.7082987519" });
            listEstacion.Add(new SelectListItem { Text = "Palo Verde (PV)", Value = "SII014192819200.2788020223" });
            listEstacion.Add(new SelectListItem { Text = "Las Cruces (LC)", Value = "SII017112627200.5981444351" });
            listEstacion.Add(new SelectListItem { Text = "Costa Rican Offices (CRO)", Value = "SII014548761600.7018216447" });
            listEstacion.Add(new SelectListItem { Text = "North American Offices (NAO)", Value = "SII014548761600.13131837437" });
            ViewBag.listSexo = listSexo;
            ViewBag.listRol = listRol;
            ViewBag.listEstacion = listEstacion;
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

        [Authorize]
        public ActionResult Administracion(int? idUsr, string returnUrl)
        {
            if (idUsr == null)
            {
                return HttpNotFound();
            }
            if((decimal)Session["Id"] == idUsr) { 
                USUARIO usr = db.USUARIO.Find(idUsr);
                if (usr == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    var listSexo = new List<SelectListItem>();
                    switch (usr.SEXO)
                    {
                        case "M":
                            listSexo.Add(new SelectListItem { Selected = true, Text = "Hombre", Value = "M" });
                            listSexo.Add(new SelectListItem { Text = "Mujer", Value = "F" });
                            break;
                        case "F":
                            listSexo.Add(new SelectListItem { Selected = true, Text = "Mujer", Value = "F" });
                            listSexo.Add(new SelectListItem { Text = "Hombre", Value = "M" });
                            break;
                    }
                    var listRol = new List<SelectListItem>();
                    switch (usr.ROL)
                    {
                        case "R":
                            listRol.Add(new SelectListItem { Selected = true, Text = "Superusuario", Value = "R" });
                            listRol.Add(new SelectListItem { Text = "Secretario", Value = "S" });
                            listRol.Add(new SelectListItem { Text = "Administrador", Value = "A" });
                            break;
                        case "S":
                            listRol.Add(new SelectListItem { Selected = true, Text = "Secretario", Value = "S" });
                            listRol.Add(new SelectListItem { Text = "Superusuario", Value = "R" });
                            listRol.Add(new SelectListItem { Text = "Administrador", Value = "A" });
                            break;
                        case "A":
                            listRol.Add(new SelectListItem { Selected = true, Text = "Administrador", Value = "A" });
                            listRol.Add(new SelectListItem { Text = "Secretario", Value = "S" });
                            listRol.Add(new SelectListItem { Text = "Superusuario", Value = "R" });
                            break;
                    }
                    //SII014192819200.7082987519=LS, SII014192819200.2788020223=PV, SII017112627200.5981444351=LC, SII014548761600.1313183743=NAO, SII014548761600.7018216447=CRO
                    //LS=La Selva, PV=Palo Verde, LC=Las Cruces, CRO=Costa Rican Offices, NAO=North American Offices
                    var listEstacion = new List<SelectListItem>();
                    switch (usr.IDESTACION)
                    {
                        case "SII014192819200.7082987519":
                            listEstacion.Add(new SelectListItem { Selected = true, Text = "La Selva (LS)", Value = "SII014192819200.7082987519" });
                            listEstacion.Add(new SelectListItem { Text = "Palo Verde (PV)", Value = "SII014192819200.2788020223" });
                            listEstacion.Add(new SelectListItem { Text = "Las Cruces (LC)", Value = "SII017112627200.5981444351" });
                            listEstacion.Add(new SelectListItem { Text = "Costa Rican Offices (CRO)", Value = "SII014548761600.7018216447" });
                            listEstacion.Add(new SelectListItem { Text = "North American Offices (NAO)", Value = "SII014548761600.13131837437" });
                            break;
                        case "SII014192819200.2788020223":
                            listEstacion.Add(new SelectListItem { Text = "La Selva (LS)", Value = "SII014192819200.7082987519" });
                            listEstacion.Add(new SelectListItem { Selected = true, Text = "Palo Verde (PV)", Value = "SII014192819200.2788020223" });
                            listEstacion.Add(new SelectListItem { Text = "Las Cruces (LC)", Value = "SII017112627200.5981444351" });
                            listEstacion.Add(new SelectListItem { Text = "Costa Rican Offices (CRO)", Value = "SII014548761600.7018216447" });
                            listEstacion.Add(new SelectListItem { Text = "North American Offices (NAO)", Value = "SII014548761600.13131837437" });
                            break;
                        case "SII017112627200.5981444351":
                            listEstacion.Add(new SelectListItem { Text = "La Selva (LS)", Value = "SII014192819200.7082987519" });
                            listEstacion.Add(new SelectListItem { Text = "Palo Verde (PV)", Value = "SII014192819200.2788020223" });
                            listEstacion.Add(new SelectListItem { Selected = true, Text = "Las Cruces (LC)", Value = "SII017112627200.5981444351" });
                            listEstacion.Add(new SelectListItem { Text = "Costa Rican Offices (CRO)", Value = "SII014548761600.7018216447" });
                            listEstacion.Add(new SelectListItem { Text = "North American Offices (NAO)", Value = "SII014548761600.13131837437" });
                            break;
                        case "SII014548761600.7018216447":
                            listEstacion.Add(new SelectListItem { Text = "La Selva (LS)", Value = "SII014192819200.7082987519" });
                            listEstacion.Add(new SelectListItem { Text = "Palo Verde (PV)", Value = "SII014192819200.2788020223" });
                            listEstacion.Add(new SelectListItem { Text = "Las Cruces (LC)", Value = "SII017112627200.5981444351" });
                            listEstacion.Add(new SelectListItem { Selected = true, Text = "Costa Rican Offices (CRO)", Value = "SII014548761600.7018216447" });
                            listEstacion.Add(new SelectListItem { Text = "North American Offices (NAO)", Value = "SII014548761600.13131837437" });
                            break;
                        case "SII014548761600.13131837437":
                            listEstacion.Add(new SelectListItem { Text = "La Selva (LS)", Value = "SII014192819200.7082987519" });
                            listEstacion.Add(new SelectListItem { Text = "Palo Verde (PV)", Value = "SII014192819200.2788020223" });
                            listEstacion.Add(new SelectListItem { Text = "Las Cruces (LC)", Value = "SII017112627200.5981444351" });
                            listEstacion.Add(new SelectListItem { Text = "Costa Rican Offices (CRO)", Value = "SII014548761600.7018216447" });
                            listEstacion.Add(new SelectListItem { Selected = true, Text = "North American Offices (NAO)", Value = "SII014548761600.13131837437" });
                            break;
                    }
                    ViewBag.listEstacion = listEstacion;
                    ViewBag.listSexo = listSexo;
                    ViewBag.listRol = listRol;
                    return View(usr);
                }
            } 
            else
            {
                return Login(returnUrl);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Administracion(USUARIO editado, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                USUARIO usr = db.USUARIO.Find(editado.ID);
                usr.NOMBRE = editado.NOMBRE;
                usr.APELLIDO = editado.APELLIDO;
                usr.CONTRASENA = editado.CONTRASENA;
                usr.EMAIL = editado.EMAIL;
                usr.SEXO = editado.SEXO;
                usr.USUAR = editado.USUAR;
                usr.ROL = editado.ROL == null ? usr.ROL : editado.ROL;
                usr.IDESTACION = editado.IDESTACION == null ? usr.IDESTACION : editado.IDESTACION;
                usr.ESTACION = editado.ESTACION == null ? usr.ESTACION : editado.ESTACION;
                if(usr.ESTACION == null)
                {
                    usr.ESTACION = db.V_ESTACION.Find(usr.IDESTACION);
                }

                Session["Id"] = usr.ID;
                Session["Username"] = usr.USUAR.ToString();
                Session["Nombre"] = usr.NOMBRE.ToString();
                Session["Apellido"] = usr.APELLIDO.ToString();
                //S=Secre, A=Admin, R=Superusuario
                Session["Rol"] = usr.ROL.ToString();
                //SII014192819200.7082987519=LS, SII014192819200.2788020223=PV, SII017112627200.5981444351=LC, SII014548761600.1313183743=NAO, SII014548761600.7018216447=CRO
                //LS=La Selva, PV=Palo Verde, LC=Las Cruces, CRO=Costa Rican Offices, NAO=North American Offices
                Session["Estacion"] = usr.ESTACION.NOMBRE;
                Session["IdEstacion"] = usr.ESTACION.ID;
                Session["Siglas"] = usr.ESTACION.SIGLAS;
                Session["Genero"] = usr.SEXO == "M" ? "o" : "a";
                Session["retUrl"] = returnUrl;
          

                db.Entry(usr).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = usr.NOMBRE + " " + usr.APELLIDO + " se modificó exitosamente.";

            }
            return Logueado();
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
                Session["Genero"] = null;
                Session["retURL"] = returnUrl;
                
                return Redirect(Request.RawUrl);
            }
            else
            {
                Session["retURL"] = returnUrl;
                return View();
            }
        }



        [HttpPost]
        public ActionResult Login(USUARIO user)
        {
            USUARIO usr = db.USUARIO.Where(u => u.USUAR == user.USUAR && u.CONTRASENA == user.CONTRASENA).FirstOrDefault();
            if (usr != null)
            {
                Session["Id"] = usr.ID;
                Session["Username"] = usr.USUAR.ToString();
                Session["Nombre"] = usr.NOMBRE.ToString();
                Session["Apellido"] = usr.APELLIDO.ToString();
                //S=Secre, A=Admin, R=Superusuario
                Session["Rol"] = usr.ROL.ToString();
                //SII014192819200.7082987519=LS, SII014192819200.2788020223=PV, SII017112627200.5981444351=LC, SII014548761600.1313183743=NAO, SII014548761600.7018216447=CRO
                //LS=La Selva, PV=Palo Verde, LC=Las Cruces, CRO=Costa Rican Offices, NAO=North American Offices
                Session["Estacion"] = usr.ESTACION.NOMBRE;
                Session["IdEstacion"] = usr.ESTACION.ID;
                Session["Siglas"] = usr.ESTACION.SIGLAS;
                Session["Genero"] = usr.SEXO == "M" ? "o":"a";
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
                if(Session["retURL"] == null)
                { 
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect((string)Session["retURL"]);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}