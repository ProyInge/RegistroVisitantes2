
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
    
public partial class INFOVISITA
{

    public string ID_RESERVACION { get; set; }

    public int CEDULA { get; set; }

    public string NOMBRE_EMERGENCIA { get; set; }

    public string TEL_EMERGENCIA { get; set; }

    public string EMAIL_EMERGENCIA { get; set; }

    public string REL_EMERGENCIA { get; set; }

    public string COMIDA { get; set; }

    public string DIETA { get; set; }

    public string OBSERVACIONES_DIETA { get; set; }

    public string ALERGIAS { get; set; }

    public string PROPOSITO { get; set; }

    public string OTRO_PROPOSITO { get; set; }

    public string COMO_ENTERO { get; set; }

    public Nullable<System.DateTime> FECHA { get; set; }

    public string NOMBRE_CURSO { get; set; }

    public string NUMERO_CURSO { get; set; }

    public string ROL_CURSO { get; set; }

    public string NOMBRE_PROYECTO { get; set; }

    public string INVERSIONES { get; set; }

    public string FUENTE { get; set; }

    public string RESOLUCION { get; set; }

    public string PERMISO { get; set; }

    public Nullable<System.DateTime> EXPIRACION { get; set; }

    public Nullable<bool> CARNE { get; set; }

    public Nullable<bool> POLLO { get; set; }

    public Nullable<bool> PESCADO { get; set; }

    public Nullable<bool> CERDO { get; set; }



    public virtual PERSONA PERSONA { get; set; }

    public virtual RESERVACION RESERVACION { get; set; }

}

}