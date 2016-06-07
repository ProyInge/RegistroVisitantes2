using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroVisitantes.Validacion
{
    public class Validaciones
    { }
}

/* 
 
using System.ComponentModel.DataAnnotations;

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

            [Required]
            public string CEDULA { get; set; }

            [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
            public string NOMBRE { get; set; }

            [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
            public string APELLIDO { get; set; }

            [EmailAddress()]
            public string EMAIL { get; set; }

            [RegularExpression("[0-9]")]
            public string TELEFONO { get; set; }

            public string NACIONALIDAD { get; set; }

            public string PAIS { get; set; }

            public string ESTADO { get; set; }

            public string CIUDAD { get; set; }

            [StringLength(100)]
            public string DIRECCION { get; set; }

            public string POSICION { get; set; }

            public Nullable<int> INSTITUCION { get; set; }

            public string TITULO { get; set; }

            public string ROL { get; set; }

            public string GENERO { get; set; }

            public string COD_POSTAL { get; set; }



            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

            public virtual ICollection<INFOVISITA> INFOVISITA { get; set; }

            public virtual V_INSTITUCION INSTITUCIONI { get; set; }

            public virtual V_PAISES NACIONALIDADI { get; set; }

            public virtual V_PAISES PAISI { get; set; }

        }

    }

} 

    

using System.ComponentModel.DataAnnotations;

namespace RegistroVisitantes.Models
{

using System;
    using System.Collections.Generic;
    
public partial class INFOVISITA
{

    public string ID_RESERVACION { get; set; }
    
    [Required]
    public string CEDULA { get; set; }

    [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
    public string NOMBRE_EMERGENCIA { get; set; }

    [RegularExpression("[0-9]")]
    public string TEL_EMERGENCIA { get; set; }

    [EmailAddress()]
    public string EMAIL_EMERGENCIA { get; set; }

    public string REL_EMERGENCIA { get; set; }

    public string COMIDA { get; set; }

    public bool CARNE { get; set; }

    public bool POLLO { get; set; }

    public bool PESCADO { get; set; }

    public bool CERDO { get; set; }

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

    public Nullable<bool> ESTADO { get; set; }



    public virtual PERSONA PERSONA { get; set; }

    public virtual V_RESERVACION RESERVACION { get; set; }

}

}



 */
