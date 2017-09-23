using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DentalClinic.Models
{
    //[Table("PatientProfile")]
    public class PatientProfile: BaseEntity
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public int Gender { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDay { get; set; }      
        public DateTime? TreatmentDate { get; set; }
    }
}
