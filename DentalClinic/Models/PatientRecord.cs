using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalClinic.Models
{
    public class PatientRecord: BaseEntity
    {
        public int ProfileId { get; set; }
        public int? DoctorId { get; set; }
        public string Description { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime? TreatmentDate { get; set; }
    }
}
