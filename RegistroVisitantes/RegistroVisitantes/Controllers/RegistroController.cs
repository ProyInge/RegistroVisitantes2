﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroVisitantes.Models;
using PagedList;
using PagedList.Mvc;


namespace RegistroVisitantes.Controllers
{
    public class RegistroController : Controller
    {
        private BDContactos BDContac = new BDContactos();
        private BDReservas BDReserv = new BDReservas();


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
        // GET: /Registro/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind()]Models.PREREGISTRO form)
        {
            if (ModelState.IsValid)
            {
                var db = BDContac;
                db.PREREGISTRO.Add(form);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return Create();
        }

        [HttpGet]
        public ActionResult CreateOET()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateOET([Bind()]Models.PREREGISTRO form)
        {
            if (ModelState.IsValid)
            {
                var db = BDContac;
                db.PREREGISTRO.Add(form);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return Create();
        }

        public ActionResult ListVisitantes()
        {
            var db = BDContac;
            var lista = db.CONTACTO.Take(5).ToList();

            return View(lista);
        }

        public ActionResult ListReservas(DateTime? filterDate, int? Pagina)
        {
            var db = BDReserv;
            DateTime filter = (filterDate ?? DateTime.Today);
            ViewBag.fromDate = filter;

            var lista = db.FormReservacion.OrderBy(s => s.ENTRA);//.Where(x => x.ENTRA != null && DateTime.Compare(x.ENTRA.Value, filter) < 0).OrderByDescending(x => x.ENTRA);

            int Size_Of_Page = 5;
            int No_Of_Page = (Pagina ?? 1);
            return View(lista.ToPagedList(No_Of_Page, Size_Of_Page));
        }

    }
}
