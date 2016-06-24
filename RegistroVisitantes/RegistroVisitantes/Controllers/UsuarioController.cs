using PagedList;
using RegistroVisitantes.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
        //Instancia de la base de datos para efectuar consultas y guardar cambios
        private BDRegistro db = new BDRegistro();

        /*
        * Desc: Muestra la lista con los datos más relevantes de los usuarios del sistema. 
        * Requiere: n/a
        * Devuelve: La vista de la lista de usuarios.
        */

        [Authorize]

        public ActionResult Index()
        {
            
            return View(db.USUARIO.ToList());
        }

        /*
        * Desc: Entra a la página de confirmación de eliminación del usuario
        * Requiere: el ID del usuario a eliminar y un booleano indicando si hubo algún error
        * Devuelve: La vista de la confirmación de eliminación del usuario.
        */
        [Authorize]
        [HttpGet]
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Hubo un error al eliminar el usuario. Por favor, intente de nuevo.";
            }
            USUARIO usr = db.USUARIO.Find(id);
            if (usr == null)
            {
                return HttpNotFound();
            }
            return View(usr);
        }

        /*
        * Desc: Ejecuta la eliminación del usuario y redirecciona al Index en caso de éxito o sino redirecciona al GET de Delete
        *       indicando que hubo un error.
        * Requiere: el ID del usuario a eliminar
        * Devuelve: La vista del Index en caso de éxito o de Delete en caso de error.
        */
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                USUARIO usr = db.USUARIO.Find(id);
                db.USUARIO.Remove(usr);
                db.SaveChanges();
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index","Usuario");
        }

        /*
        * Desc: Formulario que crea un nuevo usuario en el sistema.
        * Requiere: n/a
        * Devuelve: La vista del formulario para crear el usuario.
        */
        [Authorize]
        public ActionResult Registrar()
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



        /*
        * Desc: Crea un nuevo usuario en el sistema.
        * Requiere: un objeto USUARIO con el usuario a crear.
        * Devuelve: la vista del Index.
        */
        [Authorize]
        [HttpPost]
        public ActionResult Registrar(USUARIO usuarioNuevo)
        {
            if (ModelState.IsValid)
            {
                
                db.USUARIO.Add(usuarioNuevo);
                db.SaveChanges();
                
                ModelState.Clear();
                ViewBag.Message = usuarioNuevo.NOMBRE + " " + usuarioNuevo.APELLIDO + " se ingresó exitosamente.";
            }
            return RedirectToAction("Index","Usuario");
        }

        /*
        * Desc: Muestra la información del usuario y permite editarla
        * Requiere: Un id de usuario válido @idUsr
        * Devuelve: La vista del formulario con la información del usuario
        */
        [Authorize]
        public ActionResult Administrar(int? idUsr, int? mensaje)
        {
            if (idUsr == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if( (string)Session["Rol"]=="R" || (decimal)Session["Id"] == idUsr ) { 
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
                    if (mensaje == 1)
                    {
                        ViewBag.Mensaje = "Y";
                    }
                    if (mensaje == 0)
                    {
                        ViewBag.Mensaje = "N";
                    }
                    return View(usr);
                }
            } 
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        /*
        * Desc: Modifica al usuario determinado @usr con la información modificada del usuario obtenida @editado
        * Requiere: Un usuario válido en el sistema
        * Devuelve: Home en caso de estar logueado, la pantalla de ingreso en otro
        */
        [Authorize]
        [HttpPost]
        public ActionResult Administrar(USUARIO editado)
        {
            int mensaje = -1;
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
                if((decimal)Session["id"] == usr.ID) { 
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
                }

                try
                { 
                    db.Entry(usr).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = usr.NOMBRE + " " + usr.APELLIDO + " se modificó exitosamente.";
                    mensaje = 1;
                }
                catch
                {
                    mensaje = 0;
                }

            }
            return RedirectToAction("Administrar", new { @idUsr=editado.ID, @mensaje=mensaje });
        }

        /*
        * Desc: Crea el formulario de ingreso al sistema en caso de no estar logueado, en otro caso realiza el cierre de sesión
        * Requiere: -
        * Devuelve: La vista del formulario con la información modificada
        */
        [AllowAnonymous]
        public ActionResult Ingresar(string ReturnUrl)
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
                return Redirect(Request.RawUrl);
            }
            else
            {
                return View();
            }
        }

        /*
        * Desc: Loguea al usuario recibido @user en el sistema utilizando resetRequest 
        * Requiere: Un usuario válido
        * Devuelve: Home en caso de un login exitoso y la página de ingreso en otro caso
        */
        [HttpPost]
        public ActionResult Ingresar(USUARIO user, string ReturnUrl)
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
                Session["Genero"] = usr.SEXO == "M" ? "o" : "a";
                FormsAuthentication.SetAuthCookie(usr.USUAR.ToString(), true);
                resetRequest();
                return RedirectToAction("Logueado", new { ReturnUrl = ReturnUrl });
            }
            else
            {
                ModelState.AddModelError("", "El nombre de usuario y la contraseña no coinciden");
                return View();
            }

        }

        /*
        * Desc: Reinicia el cookie en el navegador para manejar la seguridad con estos elementos
        * Requiere: -
        * Devuelve: -
        */
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

        /*
        * Desc: Revisa si el usuario esta logueado y redirige según sea necesario
        * Requiere: -
        * Devuelve: La vista de login si el usuario no está logueado, la vista Home en cualquier otro caso
        */
        [Authorize]
        public ActionResult Logueado(string ReturnUrl)
        {
            if (Session["Id"] != null)
            {
                if(ReturnUrl!=null)
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
                
            }
            else
            {
                return RedirectToAction("Ingresar");
            }
        }
    }
}