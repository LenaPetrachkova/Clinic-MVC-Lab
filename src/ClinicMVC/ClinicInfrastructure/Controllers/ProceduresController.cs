using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicInfrastructure;
using X.PagedList;
using ClinicDomain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ClinicInfrastructure.Controllers
{
    public class ProceduresController : Controller
    {
        private readonly ClinicContext _context;
        private readonly UserManager<User> _userManager;

        public ProceduresController(ClinicContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Procedures
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var currentClinicId = currentUser.ClinicId;

            var clinicContext = _context.Procedures
                .Where(p => p.ClinicId == currentClinicId);

            if (!string.IsNullOrEmpty(searchString))
            {
                
                clinicContext = clinicContext
                    .Where(p =>
                        p.Id.ToString().Contains(searchString) ||
                        p.Name.Contains(searchString) ||
                        p.Price.ToString().Contains(searchString)
                    );
            }
            
            var procedures = await clinicContext.ToListAsync();

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(procedures.ToPagedList(pageNumber, pageSize));
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

        // GET: Procedures/Create
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account"); 
            }

            ViewData["ClinicId"] = new SelectList(_context.Clinics, "Id", "Address", currentUser.ClinicId);
            return View();
        }

        // POST: Procedures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,ClinicId")] Procedure procedure)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account"); 
            }

            procedure.ClinicId = currentUser.ClinicId;

            ModelState.Remove("Clinic");
            ModelState.Remove("ClinicId");
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
        [Authorize(Roles = "Owner")]
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
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ClinicId")] Procedure procedure)
        {
            if (id != procedure.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Clinic");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(procedure);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClinicId"] = new SelectList(_context.Clinics, "Id", "Address", procedure.ClinicId);
            return View(procedure);
        }


        // GET: Procedures/Delete/5
        [Authorize(Roles = "Owner")]
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
        [Authorize(Roles = "Owner")]
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
