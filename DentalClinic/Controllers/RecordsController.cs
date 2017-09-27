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
    public class RecordsController : Controller
    {
        private readonly ClinicContext _context;

        public RecordsController(ClinicContext context)
        {
            _context = context;
        }


        // GET: PatientRecords
        public async Task<IActionResult> Index()
        {
            var clinicContext = _context.PatientRecords.Include(p => p.Doctor);
            return View(await clinicContext.ToListAsync());
        }

        public async Task<IActionResult> RecordList(int profileId)
        {
            var clinicContext = _context.PatientRecords.Include(p => p.Doctor).Where(r => r.PatientProfileId == profileId).OrderByDescending(r => r.Id);
            return PartialView("_RecordList", await clinicContext.ToListAsync());
        }
        // GET: PatientRecords/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientRecord = await _context.PatientRecords
                .Include(p => p.Doctor)
                .Include(r => r.Profile)
                .Include(p => p.Treatments).ThenInclude(t => t.Doctor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (patientRecord == null)
            {
                return NotFound();
            }

            return View(patientRecord);
        }

        // GET: PatientRecords/Create
        [HttpGet]
        public async Task<IActionResult> Create(int profileId)
        {
            if (!_context.PatientProfiles.Any(p => p.Id == profileId))
            {
                return NoContent();
            }
            var profile = await _context.PatientProfiles.SingleOrDefaultAsync(p => p.Id == profileId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name");
            var record = new PatientRecord
            {
                PatientProfileId = profileId,
                Profile = profile,
                TreatmentDate = DateTime.Now
            };

            return View(record);
        }

        // POST: PatientRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientProfileId,DoctorId,Description,TotalCost,TreatmentDate")] PatientRecord patientRecord)
        {
            if (ModelState.IsValid)
            {
                if (_context.PatientProfiles.Any(p => p.Id == patientRecord.PatientProfileId))
                {
                    //patientRecord.PatientProfileId = patientRecord.PatientProfileId;
                    _context.Add(patientRecord);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Profiles", new { id = patientRecord.PatientProfileId });
                }
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", patientRecord.DoctorId);
            var profile = await _context.PatientProfiles.SingleOrDefaultAsync(p => p.Id == patientRecord.PatientProfileId);
            var record = new PatientRecord
            {
                PatientProfileId = patientRecord.PatientProfileId,
                Profile = profile
            };
            return View(record);
        }

        // GET: PatientRecords/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientRecord = await _context.PatientRecords.Include(r => r.Profile).SingleOrDefaultAsync(m => m.Id == id);
            if (patientRecord == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", patientRecord.DoctorId);
            return View(patientRecord);
        }

        // POST: PatientRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var recordToUpdate = await _context.PatientRecords.Include(r => r.Profile).SingleOrDefaultAsync(r => r.Id == id);
            if (ModelState.IsValid)
            {

                if (await TryUpdateModelAsync<PatientRecord>(
                recordToUpdate,
                "", r => r.DoctorId, r => r.Description, r => r.TotalCost, r => r.TreatmentDate))
                {
                    if (id != recordToUpdate.Id)
                    {
                        return NotFound();
                    }
                    try
                    {
                        recordToUpdate.UpdatedOn = DateTime.Now;
                        _context.Update(recordToUpdate);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PatientRecordExists(recordToUpdate.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return RedirectToAction("Details", new { id = id });
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", recordToUpdate.DoctorId);
            return View(recordToUpdate);
        }

        // GET: PatientRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientRecord = await _context.PatientRecords
                .Include(p => p.Doctor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (patientRecord == null)
            {
                return NotFound();
            }

            return View(patientRecord);
        }

        // POST: PatientRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientRecord = await _context.PatientRecords.SingleOrDefaultAsync(m => m.Id == id);
            _context.PatientRecords.Remove(patientRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientRecordExists(int id)
        {
            return _context.PatientRecords.Any(e => e.Id == id);
        }
    }
}
