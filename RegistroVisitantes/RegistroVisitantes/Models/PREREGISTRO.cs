
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
    
public partial class PREREGISTRO
{

    public int NUMPREREGISTRO { get; set; }

    public Nullable<int> IDCONTACTO { get; set; }

    public string IDRESERVACION { get; set; }

    public string PROPOSITO { get; set; }

    public string IDGRUPO { get; set; }

    public string COMOENTERO { get; set; }

    public Nullable<System.DateTime> FECHA { get; set; }

    public string NOMCURSO { get; set; }

    public string NUMCURSO { get; set; }

    public string ROLCURSO { get; set; }

    public string NOMPROYECTO { get; set; }

    public string INVERSIONES { get; set; }

    public string FUENTE { get; set; }

    public string RESOLUCION { get; set; }

    public string PERMISO { get; set; }

    public Nullable<System.DateTime> EXPIRACION { get; set; }



    public virtual CONTACTO CONTACTO { get; set; }

}

}
