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
    using System.Collections.Generic;
    
    public partial class USUARIO
    {
        public decimal ID { get; set; }
        public string NOMBRE { get; set; }
        public string APELLIDO { get; set; }
        public string EMAIL { get; set; }
        public string USUAR { get; set; }
        public string CONTRASENA { get; set; }
        public string ROL { get; set; }
        public string IDESTACION { get; set; }
    
        public virtual V_ESTACION ESTACION { get; set; }
    }
}
