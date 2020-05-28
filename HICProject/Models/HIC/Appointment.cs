using HICProject.Models.HIC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.HIC
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int patientId { get; set; } 

        [Required]
        public int doctorId { get; set; }

        public string Status { get; set; }

        [Required]
        public string Description { get; set; }//Shown on a subject line in a meeting request, or appointment list

        [Required]
        public string Comment { get; set; }//Additional comments

        [Required]
        public string Priority { get; set; }//Used to make informed decisions if needing to re-prioritize

        [Required]
        public DateTime Start { get; set; }//	When appointment is to take place

        [Required]
        public DateTime End { get; set; }//When appointment is to conclude

        [Required]
        public DateTime Created { get; set; } //The date that this appointment was initially created

    }
}
