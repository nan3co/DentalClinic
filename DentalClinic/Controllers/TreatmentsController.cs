using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentalClinic.Data;
using DentalClinic.Models;

namespace DentalClinic.Controllers
{
    public class TreatmentsController : Controller
    {
        private readonly ClinicContext _context;

        public TreatmentsController(ClinicContext context)
        {
            _context = context;
        }

        // GET: Treatments
        public async Task<IActionResult> Index()
        {
            var clinicContext = _context.Treatments.Include(t => t.Doctor);
            return View(await clinicContext.ToListAsync());
        }

        // GET: Treatments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatments
                .Include(t => t.Doctor)
                .Include(t => t.Record).ThenInclude(r => r.Profile)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (treatment == null)
            {
                return NotFound();
            }

            return View(treatment);
        }

        // GET: Treatments/Create
        [HttpGet]
        public async Task<IActionResult> Create(int recordId)
        {
            if (!_context.PatientRecords.Any(t => t.Id == recordId))
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name");
            var record = await _context.PatientRecords.Include(r => r.Profile).SingleOrDefaultAsync(r => r.Id == recordId);
            var newTreatment = new Treatment
            {
                PatientRecordId = recordId,
                Record = record,
                DoctorId = record.DoctorId,
                Description = record.Description,
                TreatmentDate = DateTime.Now
            };
            return View(newTreatment);
        }

        // POST: Treatments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientRecordId,Description,DoctorId,PaidAmount,TreatmentDate")] Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                if (treatment.PatientRecordId <= 0 || !_context.PatientRecords.Any(t => t.Id == treatment.PatientRecordId))
                {
                    return View(treatment);
                }
                treatment.UpdatedOn = DateTime.Now;
                _context.Add(treatment);
                
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Records", new { id = treatment.PatientRecordId });
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", treatment.DoctorId);
            //var record = await _context.PatientRecords.SingleOrDefaultAsync(r => r.Id == treatment.PatientRecordId);
            //var newTreatment = new Treatment
            //{
            //    PatientRecordId = treatment.PatientRecordId,
            //    Record = record
            //};
            return View(treatment);
        }

        // GET: Treatments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id.Value <= 0)
            {
                return NotFound();
            }

            var treatment = await _context.Treatments.Include(t => t.Record).ThenInclude(r => r.Profile).SingleOrDefaultAsync(m => m.Id == id);
            if (treatment == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", treatment.DoctorId);
            return View(treatment);
        }

        // POST: Treatments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientRecordId,Description,DoctorId,PaidAmount,TreatmentDate,Id")] Treatment treatment)
        {
            if (id == treatment.Id && treatment.Id > 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        treatment.UpdatedOn = DateTime.Now;
                        _context.Update(treatment);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TreatmentExists(treatment.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Details", new { id = id });
                }

            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", treatment.DoctorId);

            return View(treatment);
        }

        // GET: Treatments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatments
                .Include(t => t.Doctor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (treatment == null)
            {
                return NotFound();
            }

            return View(treatment);
        }

        // POST: Treatments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treatment = await _context.Treatments.SingleOrDefaultAsync(m => m.Id == id);
            _context.Treatments.Remove(treatment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreatmentExists(int id)
        {
            return _context.Treatments.Any(e => e.Id == id);
        }
    }
}
