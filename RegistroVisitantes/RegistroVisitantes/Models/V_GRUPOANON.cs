
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
    
public partial class V_GRUPOANON
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public V_GRUPOANON()
    {

        this.RESERVACIONESNUMS = new HashSet<V_RESERVACION>();

    }


    public string ID { get; set; }

    public Nullable<short> INFANTES { get; set; }

    public Nullable<short> NINOS { get; set; }

    public Nullable<short> MUJERES { get; set; }

    public Nullable<short> HOMBRES { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<V_RESERVACION> RESERVACIONESNUMS { get; set; }

}

}