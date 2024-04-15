
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicInfrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ClinicDomain;
using X.PagedList;
using ClinicDomain.Model;
using Microsoft.AspNetCore.Identity;

namespace ClinicInfrastructure.Controllers
{
    public class PatientCardsController : Controller
    {
        private readonly ClinicContext _context;
        private readonly UserManager<User> _userManager;

        public PatientCardsController(ClinicContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: PatientCards

        public async Task<IActionResult> Index(string searchString, int? page)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentClinicId = currentUser.ClinicId;
            var clinicContext = _context.PatientCards
               .Where(p => p.ClinicId == currentClinicId);

            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int id))
                {
                    clinicContext = clinicContext
                        .Include(p => p.Discount)
                        .Where(p =>
                            p.Id == id ||
                            p.FirstName.Contains(searchString) ||
                            p.LastName.Contains(searchString) ||
                            p.FatherName.Contains(searchString) ||
                            p.PhoneNumber.Contains(searchString)
                        );
                }
                else
                {
                    clinicContext = clinicContext
                        .Include(p => p.Discount)
                        .Where(p =>
                            p.FirstName.Contains(searchString) ||
                            p.LastName.Contains(searchString) ||
                            p.FatherName.Contains(searchString) ||
                            p.PhoneNumber.Contains(searchString)
                        );
                }
            }
            else
            {
                clinicContext = clinicContext.Include(p => p.Discount);
            }

            var patients = await clinicContext.ToListAsync();

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(patients.ToPagedList(pageNumber, pageSize));
        }


        // GET: PatientCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientCard = await _context.PatientCards
                .Include(p => p.Discount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientCard == null)
            {
                return NotFound();
            }

            return View(patientCard);
        }

        // GET: PatientCards/Create
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["DiscountId"] = new SelectList(_context.Discounts, "Id", "SocialGroup");
            return View();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,FatherName,PhoneNumber,DateOfBirth,AddInfo,Allergy,ChronicDisease,Diseases,DiscountId,Id")] PatientCard patientCard)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            patientCard.ClinicId = currentUser.ClinicId;

            ModelState.Remove("Discount");
            if (ModelState.IsValid)
            {
                _context.Add(patientCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "Id", "SocialGroup", patientCard.DiscountId);
            return View(patientCard);
        }

        // GET: PatientCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientCard = await _context.PatientCards.FindAsync(id);
            if (patientCard == null)
            {
                return NotFound();
            }
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "Id", "SocialGroup", patientCard.DiscountId);
            return View(patientCard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,FatherName,PhoneNumber,DateOfBirth,AddInfo,Allergy,ChronicDisease,Diseases,DiscountId,Id")] PatientCard patientCard)
        {
            if (id != patientCard.Id)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            patientCard.ClinicId = currentUser.ClinicId;

            ModelState.Remove("Discount");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientCardExists(patientCard.Id))
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
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "Id", "SocialGroup", patientCard.DiscountId);
            return View(patientCard);
        }


        // GET: PatientCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientCard = await _context.PatientCards
                .Include(p => p.Discount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientCard == null)
            {
                return NotFound();
            }

            return View(patientCard);
        }

        // POST: PatientCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientCard = await _context.PatientCards
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (patientCard == null)
            {
                return NotFound();
            }

            _context.Appointments.RemoveRange(patientCard.Appointments);
            _context.PatientCards.Remove(patientCard);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientCardExists(int id)
        {
            return _context.PatientCards.Any(e => e.Id == id);
        }
    }
}
