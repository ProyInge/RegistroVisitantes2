﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------


namespace RegistroVisitantes.Models
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class BDRegistro : DbContext
{
    public BDRegistro()
        : base("name=BDRegistro")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<PERSONA> PERSONA { get; set; }

    public virtual DbSet<USUARIO> USUARIO { get; set; }

    public virtual DbSet<V_ESTACION> V_ESTACION { get; set; }

    public virtual DbSet<V_RESERVACION> V_RESERVACION { get; set; }

    public virtual DbSet<V_CONTACTO> V_CONTACTO { get; set; }

    public virtual DbSet<V_GRUPOANON> V_GRUPOANON { get; set; }

    public virtual DbSet<V_GRUPORSRV> V_GRUPORSRV { get; set; }

    public virtual DbSet<V_INSTITUCION> V_INSTITUCION { get; set; }

    public virtual DbSet<V_PAISES> V_PAISES { get; set; }

    public virtual DbSet<REPORTE> REPORTE { get; set; }

    public virtual DbSet<INFOVISITA> INFOVISITA { get; set; }

}

}

