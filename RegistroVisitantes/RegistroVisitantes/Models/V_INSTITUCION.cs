
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
    
public partial class V_INSTITUCION
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public V_INSTITUCION()
    {

        this.PERSONAS = new HashSet<PERSONA>();

    }


    public int CAT_INSTITUCION { get; set; }

    public string DESCRIPCION { get; set; }

    public string FULL_NAME { get; set; }

    public string COUNTRY { get; set; }

    public Nullable<bool> X_SISTEMA { get; set; }

    public Nullable<System.DateTime> CREADO { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<PERSONA> PERSONAS { get; set; }

}

}
