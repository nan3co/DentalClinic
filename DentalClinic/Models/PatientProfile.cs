using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DentalClinic.Models
{
    //[Table("PatientProfile")]
    public class PatientProfile : BaseEntity
    {
        [Display(Name = "Họ Tên")]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
        public string NameEn { get; set; }
        [Display(Name = "Giới Tính")]
        public Gender Gender { get; set; }
        public int GenderId { get; set; }

        public string Phone { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Ngày sinh")]
        public DateTime? BirthDay { get; set; }

        [DataType(DataType.Date)]        
        [Display(Name = "Ngày điều trị")]
        public DateTime? TreatmentDate { get; set; }
        public ICollection<PatientRecord> Records { get; set; }
    }
}
