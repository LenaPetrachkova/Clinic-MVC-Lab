using ClinicDomain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClinicInfrastructure.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ClinicContext _context;
        private readonly UserManager<User> _userManager;
        public ProfileController(ClinicContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var currentClinicId = currentUser.ClinicId;

            var clinic = _context.Clinics.FirstOrDefault(c => c.Id == currentClinicId);

            if (clinic == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(clinic);
        }

    }
}
