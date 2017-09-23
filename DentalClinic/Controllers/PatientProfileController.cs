using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DentalClinic.Models;
using DentalClinic.Data;
using DentalClinic.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Controllers
{
    public class PatientProfileController : Controller
    {
        private readonly ProfilesService _profileService;
        private readonly ClinicContext _context;
        public PatientProfileController(ProfilesService profileService, ClinicContext context)
        {
            _profileService = profileService;
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
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
            var profiles = from p in _context.PatientProfiles select p;
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
                    profiles = profiles.OrderBy(p => p.CreatedOn);
                    break;
            }

            int pageSize = 3;
            var model = await PaginatedList<PatientProfile>.CreateAsync(profiles.AsNoTracking(), page ?? 1, pageSize);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new PatientProfile();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name", "Gender", "Phone", "Address", "Email")] PatientProfile model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedOn = DateTime.Now;
                model.UpdatedOn = DateTime.Now;
                model.NameEn = GeneralHelper.TripNonAsciiString(model.Name);
                var id = await _profileService.AddAsync(model);
                if (id > 0)
                {
                    return RedirectToAction("Details", new { id = id });
                }
            }
            return View(model);
        }
        public IActionResult Details(int id)
        {
            var model = _profileService.GetDetail(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _profileService.GetDetail(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(PatientProfile model)
        {
            model.UpdatedOn = DateTime.Now;
            _profileService.Update(model);
            return RedirectToAction("Details", new { id = model.Id });
        }
    }
}