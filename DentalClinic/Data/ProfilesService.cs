using DentalClinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Data
{
    public class ProfilesService
    {
        private readonly ClinicContext _context;
        public ProfilesService(ClinicContext context)
        {
            _context = context;
        }

        public IQueryable<PatientProfile> Top(int top = 20)
        {   
            return _context.PatientProfiles.OrderByDescending(x=>x.UpdatedOn);
        }
        public IEnumerable<PatientProfile> Search(string name)
        {
            return _context.PatientProfiles.Where(x => x.Name.Contains(name));
        }
        public async Task<int> AddAsync(PatientProfile profile)
        {
            _context.Add(profile);
            return await _context.SaveChangesAsync();
        }
        public int Update(PatientProfile profile)
        {
            _context.Attach(profile);
            _context.SaveChanges();
            return profile.Id;
        }
        public PatientProfile GetDetail(int id)
        {
            return _context.PatientProfiles.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
