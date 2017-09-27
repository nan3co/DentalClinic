using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DentalClinic.Models;
using DentalClinic.Data;
using DentalClinic.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalClinic.Controllers
{

    public class ProfilesController : Controller
    {
        private readonly ProfilesService _profileService;
        private readonly ClinicContext _context;
        public ProfilesController(ProfilesService profileService, ClinicContext context)
        {
            _profileService = profileService;
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page, int pageSize = 20)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var profiles = _context.PatientProfiles.Include(p => p.Gender).AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                profiles = profiles.Where(s => s.Name.Contains(searchString)
                                         || s.NameEn.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    profiles = profiles.OrderByDescending(p => p.Name);
                    break;
                case "date_desc":
                    profiles = profiles.OrderByDescending(p => p.TreatmentDate);
                    break;
                default:
                    profiles = profiles.OrderByDescending(p => p.Id);
                    break;
            }
            var model = await PaginatedList<PatientProfile>.CreateAsync(profiles.AsNoTracking(), page ?? 1, pageSize);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new PatientProfile
            {
                TreatmentDate = DateTime.Now
            };
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name", "GenderId", "Phone", "Address", "Email", "TreatmentDate", "BirthDay")] PatientProfile model)
        {
            if (ModelState.IsValid)
            {
                //model.CreatedOn = DateTime.Now;
                //model.UpdatedOn = DateTime.Now;
                model.NameEn = GeneralHelper.ToAscii(model.Name);
                ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", model.GenderId);
                if (model.TreatmentDate.HasValue)
                {
                    model.TreatmentDate = model.TreatmentDate.Value.Add(DateTime.Now.TimeOfDay);
                }
                var id = await _profileService.AddAsync(model);
                if (id > 0)
                {
                    return RedirectToAction("Details", new { id = model.Id });
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var profile = await _context.PatientProfiles.Include(p => p.Gender).Include(p => p.Records).ThenInclude(r => r.Doctor)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);

            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name");
            var profile = await _context.PatientProfiles.SingleOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EditPost(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var profileToUpdate = await _context.PatientProfiles.SingleOrDefaultAsync(s => s.Id == id);
            if (await TryUpdateModelAsync<PatientProfile>(
                profileToUpdate,
                "",
                s => s.Name, s => s.Gender, s => s.Address, s => s.Phone, s => s.Email, s => s.TreatmentDate, s => s.UpdatedOn, s => s.BirthDay))
            {
                try
                {
                    profileToUpdate.UpdatedOn = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = id });
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", profileToUpdate.GenderId);
            return View(profileToUpdate);
        }
    }
}