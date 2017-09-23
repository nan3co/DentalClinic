using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DentalClinic.Models
{
    public class Treatment : BaseEntity
    {
        [Column("RecordId")]
        public int PatientRecordId { get; set; }        
        [Display(Name = "Nội dung điều trị")]
        public string Description { get; set; }
        public int? DoctorId { get; set; }
        [Display(Name ="Bác sĩ")]
        public Doctor Doctor { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Đã thu")]
        public decimal PaidAmount { get; set; }
        [DataType(DataType.Date)]        
        [Display(Name = "Ngày điều trị")]
        public DateTime? TreatmentDate { get; set; }
        [Display(Name ="Mã hồ sơ")]
        public PatientRecord Record { get; set; }
    }
}
