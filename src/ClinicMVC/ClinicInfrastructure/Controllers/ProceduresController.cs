using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicDomain.Models;
using ClinicInfrastructure;

namespace ClinicInfrastructure.Controllers
{
    public class ProceduresController : Controller
    {
        private readonly ClinicContext _context;

        public ProceduresController(ClinicContext context)
        {
            _context = context;
        }

        // GET: Procedures
        public async Task<IActionResult> Index(string searchString)
        {
            var clinicContext = _context.Procedures.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                clinicContext = clinicContext
                    .Include(p => p.Clinic)
                    .Where(p =>
                        p.Id.Equals(searchString) ||
                        p.Name.Contains(searchString) ||
                        p.Price.Equals(searchString)
                    );
            }

            return View(await clinicContext.ToListAsync());
        }

        // GET: Procedures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procedure = await _context.Procedures
                .Include(p => p.Clinic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (procedure == null)
            {
                return NotFound();
            }

            return View(procedure);
        }
        private int GetCurrentClinicId()
        {
            return 1;
        }

        // GET: Procedures/Create
        public IActionResult Create()
        {   

            ViewData["ClinicId"] = new SelectList(_context.Clinics, "Id", "Address");
    
            int currentClinicId = GetCurrentClinicId();
            ViewBag.CurrentClinicId = currentClinicId;
            return View();
        }

        // POST: Procedures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,ClinicId")] Procedure procedure)
        {
            ModelState.Remove("Clinic");
            if (ModelState.IsValid)
            {
                _context.Add(procedure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClinicId"] = new SelectList(_context.Clinics, "Id", "Address", procedure.ClinicId);
            return View(procedure);
        }

        // GET: Procedures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procedure = await _context.Procedures.FindAsync(id);
            if (procedure == null)
            {
                return NotFound();
            }
            ViewData["ClinicId"] = new SelectList(_context.Clinics, "Id", "Address", procedure.ClinicId);
            return View(procedure);
        }

        // POST: Procedures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ClinicId")] Procedure procedure)
        {
            if (id != procedure.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Clinic");
            ModelState.Remove("ClinicId");

            if (ModelState.IsValid)
            {
                // Перевірка існування клініки з вказаним ClinicId
                if (!_context.Clinics.Any(c => c.Id == procedure.ClinicId))
                {
                    ModelState.AddModelError("ClinicId", "Клініка з вказаним ідентифікатором не існує.");
                    ViewData["ClinicId"] = new SelectList(_context.Clinics, "Id", "Address", procedure.ClinicId);
                    return View(procedure);
                }

                try
                {
                    _context.Update(procedure);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcedureExists(procedure.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["ClinicId"] = new SelectList(_context.Clinics, "Id", "Address", procedure.ClinicId);
            return View(procedure);
        }

        // GET: Procedures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procedure = await _context.Procedures
                .Include(p => p.Clinic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (procedure == null)
            {
                return NotFound();
            }

            return View(procedure);
        }

        // POST: Procedures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var procedure = await _context.Procedures.FindAsync(id);
            if (procedure != null)
            {
                _context.Procedures.Remove(procedure);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcedureExists(int id)
        {
            return _context.Procedures.Any(e => e.Id == id);
        }
    }
}
