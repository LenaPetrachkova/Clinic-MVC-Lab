using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicDomain.Model;
using Microsoft.AspNetCore.Identity;

namespace ClinicInfrastructure.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ClinicContext _context;
        private readonly UserManager<User> _userManager;

        public AppointmentsController(ClinicContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Appointments
        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentClinicId = currentUser.ClinicId;

            // Получаем только аппоинтменты, у которых ClinicId совпадает с ClinicId текущего пользователя
            var clinicContext = _context.Appointments
                .Where(a => a.ClinicId == currentClinicId)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Procedures);

            return View(await clinicContext.ToListAsync());
        }


        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Procedures)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var doctors = _context.Doctors.Select(d => new {
                Id = d.Id,
                FullName = d.LastName + " " + d.FirstName + " " + d.FatherName
            }).ToList();
            ViewData["DoctorId"] = new SelectList(doctors, "Id", "FullName");

            var patients = _context.PatientCards.Select(p => new {
                Id = p.Id,
                FullName = p.LastName + " " + p.FirstName + " " + p.FatherName
            }).ToList();
            ViewData["PatientId"] = new SelectList(patients, "Id", "FullName");
            ViewData["ProceduresId"] = new SelectList(_context.Procedures, "Id", "Name");


            return View();
        }



        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,StartTime,EndTime,PatientId,ProceduresId,DoctorId")] Appointment appointment)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            appointment.ClinicId = currentUser.ClinicId;

            ModelState.Remove("Doctor");
            ModelState.Remove("Patient");
            ModelState.Remove("Procedures");            
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FullName", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.PatientCards, "Id", "FullName", appointment.PatientId);
            ViewData["ProceduresId"] = new SelectList(_context.Procedures, "Id", "Name", appointment.ProceduresId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            var doctors = _context.Doctors.Select(d => new {
                Id = d.Id,
                FullName = d.LastName + " " + d.FirstName + " " + d.FatherName
            }).ToList();
            ViewData["DoctorId"] = new SelectList(doctors, "Id", "FullName");

            var patients = _context.PatientCards.Select(p => new {
                Id = p.Id,
                FullName = p.LastName + " " + p.FirstName + " " + p.FatherName
            }).ToList();
            ViewData["PatientId"] = new SelectList(patients, "Id", "FullName");
            ViewData["ProceduresId"] = new SelectList(_context.Procedures, "Id", "Name", appointment.ProceduresId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,StartTime,EndTime,PatientId,ProceduresId,DoctorId")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            appointment.ClinicId = currentUser.ClinicId;

            ModelState.Remove("Doctor");
            ModelState.Remove("Patient");
            ModelState.Remove("Procedures");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FullName", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.PatientCards, "Id", "FullName", appointment.PatientId);
            ViewData["ProceduresId"] = new SelectList(_context.Procedures, "Id", "Name", appointment.ProceduresId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Procedures)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment); 
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
