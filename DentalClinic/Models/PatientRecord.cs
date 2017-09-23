using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DentalClinic.Models
{
    public class PatientRecord : BaseEntity
    {
        [Column("ProfileId")]
        public int PatientProfileId { get; set; }
        public int? DoctorId { get; set; }
        [DisplayName("Bác sĩ")]
        public Doctor Doctor { get; set; }
        [DisplayName("Nội dung điều trị")]
        [StringLength(500,MinimumLength = 5)]
        [Required]
        public string Description { get; set; }
        [DisplayName("Chi phí")]
        [DataType(DataType.Currency)]
        public decimal TotalCost { get; set; }
        [DataType(DataType.Date)]        
        [Display(Name = "Ngày điều trị")]
        public DateTime? TreatmentDate { get; set; }
        public ICollection<Treatment> Treatments { get; set; }
        [Display(Name = "Bệnh nhân")]
        public PatientProfile Profile { get; set; }
    }
}
