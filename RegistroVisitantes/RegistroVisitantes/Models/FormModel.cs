using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RegistroVisitantes.Models
{
    public class FormModel
    {
        [Required]
        public string document { get; set;}
        public string name { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string nationality { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string sex { get; set; }
    }
}