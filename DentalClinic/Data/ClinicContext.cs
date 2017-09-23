using DentalClinic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalClinic.Data
{
    public class ClinicContext : DbContext
    {
        public ClinicContext(DbContextOptions<ClinicContext> options) : base(options)
        { }
        public DbSet<PatientProfile> PatientProfiles { get; set; }
        public DbSet<PatientRecord> PatientRecords { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Treatment> Treatments { get;set;}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySQL("server=localhost;database=library;user=user;password=password");
        //}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PatientProfile>().ToTable("PatientProfile").HasKey(m => m.Id);
            builder.Entity<PatientRecord>().ToTable("PatientRecord").HasKey(r => r.Id);
            builder.Entity<Doctor>().ToTable("Doctor").HasKey(d => d.Id);
            builder.Entity<Treatment>().ToTable("Treatment").HasKey(t => t.Id);


            base.OnModelCreating(builder);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            return base.SaveChanges();
        }
    }
}
