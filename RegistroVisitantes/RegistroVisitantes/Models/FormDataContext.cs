using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RegistroVisitantes.Models
{
    public class FormDataContext : DbContext
    {
        public DbSet<FormModel> Form { get; set; }
    }
}