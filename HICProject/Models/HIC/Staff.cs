using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Staff
    {
        [Key]
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

        [DataType(DataType.PhoneNumber)]
        [Column(name:"phone_number")]
        public int MobilePhone { get; set; }

        [Column(name: "gender")]
        public string Gender { get; set; }

        [Required]
        [Column(name:"qualifications")]
        public string Qualifications { get; set; }
    }
}
