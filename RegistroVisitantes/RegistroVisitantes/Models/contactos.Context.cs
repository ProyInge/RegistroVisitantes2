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


public partial class BDContactos : DbContext
{
    public BDContactos()
        : base("name=BDContactos")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<CONTACTO> CONTACTO { get; set; }

    public virtual DbSet<PREREGISTRO> PREREGISTRO { get; set; }

    public virtual DbSet<V_EMPLEADOS> V_EMPLEADOS { get; set; }

    public virtual DbSet<USUARIO> USUARIO { get; set; }

}

}

