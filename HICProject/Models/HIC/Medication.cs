using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.HIC
{
    public class Medication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int medication_type_code { get; set; }

        [Required]
        public int medication_unit_code { get; set; }

        [Required]
        public string medication_name { get; set; }

        [Required]
        [MinLength(20,ErrorMessage ="Please tell more descriprion")]
        public string medication_description { get; set; }
    }
}
