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
    
    public partial class ESTACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ESTACION()
        {
            this.RESERVABLE1 = new HashSet<RESERVABLE>();
        }
    
        public string ID { get; set; }
        public string NOMBRE { get; set; }
        public string SIGLAS { get; set; }
        public string RESERVABLE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RESERVABLE> RESERVABLE1 { get; set; }
    }
}
