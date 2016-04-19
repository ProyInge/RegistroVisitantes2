using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RegistroVisitantes.Models
{

    public class Formulario
    {
        public virtual PREREGISTRO PREREGISTRO { get; set; }
        public virtual RESERVACION RESERVACION { get; set; }
    }

}