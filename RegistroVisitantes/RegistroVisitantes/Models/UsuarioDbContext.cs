﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RegistroVisitantes.Models
{
    public class UsuarioDbContext : DbContext
    {
        public DbSet<USUARIO> usuario { get; set; }
    }
}