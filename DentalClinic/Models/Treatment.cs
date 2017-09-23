using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalClinic.Models
{
    public class Treatment:BaseEntity
    {
        public int RecordId { get; set; }
        public string Description { get; set; }
        public int DoctorId { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime? TreatmentDate { get; set; }
    }
}
