
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
    
public partial class PERSONA
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public PERSONA()
    {

        this.INFOVISITA = new HashSet<INFOVISITA>();

    }


    public int CEDULA { get; set; }

    public string NOMBRE { get; set; }

    public string APELLIDO { get; set; }

    public string EMAIL { get; set; }

    public string TELEFONO { get; set; }

    public string NACIONALIDAD { get; set; }

    public string PAIS { get; set; }

    public string CIUDAD { get; set; }

    public string DIRECCION { get; set; }

    public string POSICION { get; set; }

    public string INSTITUCION { get; set; }

    public string TITULO { get; set; }

    public string ROL { get; set; }

    public string GENERO { get; set; }

    public string COD_POSTAL { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<INFOVISITA> INFOVISITA { get; set; }

}

}
