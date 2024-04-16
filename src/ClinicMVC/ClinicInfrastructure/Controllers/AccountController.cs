using ClinicDomain.Model;
using ClinicInfrastructure.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClinicInfrastructure.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register(int clinicId)
        {
            ViewBag.ClinicId = clinicId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Username, ClinicId = model.ClinicId };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Owner");

                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);

        }
        public async Task<IActionResult> RegisterAdmin()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {             
                ViewBag.ClinicId = currentUser.ClinicId;
                return View();
            }
            else
            {
               
                return View(new RegisterViewModel());
            }
        }


        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    
                    var adminUser = new User { UserName = model.Username, Email = model.Email, ClinicId = currentUser.ClinicId };
                    var result = await _userManager.CreateAsync(adminUser, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(adminUser, "Admin");

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                   
                    ModelState.AddModelError(string.Empty, "Поточний користувач не знайдений або не ввійшов до системи.");
                }
            }
            return View(model);
        }


        public async Task<IActionResult> RegisterDoctor()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                ViewBag.ClinicId = currentUser.ClinicId;
                return View();
            }
            else
            {

                return View(new RegisterViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterDoctor(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {

                    var doctorUser = new User { UserName = model.Username, Email = model.Email, ClinicId = currentUser.ClinicId };
                    var result = await _userManager.CreateAsync(doctorUser, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(doctorUser, "Doctor");

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Поточний користувач не знайдений або не ввійшов до системи.");
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ModelState.Remove("ReturnUrl");
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильний логін чи (та) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}