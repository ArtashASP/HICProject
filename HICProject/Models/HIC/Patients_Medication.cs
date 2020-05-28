using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.HIC
{
    public class Patients_Medication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int medication_id { get; set; }

        [Required]
        public int patient_id { get; set; }

        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        public DateTime date_time_administrered { get; set; }

        [Required]
        public double Dosage { get; set; }

        [MinLength(20,ErrorMessage ="Please describe more about medication")]
        public string Comments { get; set; }
    }
}
