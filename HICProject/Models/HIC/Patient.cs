using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.HIC
{
    public class Patients
    {
        public int Id { get; set; }

        [Required]
        [Column(name: "first_name")]
        [MaxLength(10, ErrorMessage = "Cant be more then 10 letters")]
        public string FirstName { get; set; }

        [Required]
        [Column(name: "last_name")]
        [MaxLength(10, ErrorMessage = "Can't be more then 10 letters")]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        [Column(name: "email_address")]
        public string Email { get; set; }

        [Column(name: "phone_number")]
        [DataType(DataType.PhoneNumber)]
        public int MobilePhone { get; set; }

        [Column(name: "Gender")]
        public string Gender { get; set; }

        [Required]
        [Column(name: "Address")]
        public string Address { get; set; }
    }
}
