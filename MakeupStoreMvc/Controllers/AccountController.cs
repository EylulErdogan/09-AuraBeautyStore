using MakeupStoreMvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace MakeupStoreMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
       UserManager<Users> userManager,
       SignInManager<Users> signInManager,
       RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new AccountViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Login.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                model.Login.Password,
                model.Login.RememberMe,
                false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Email or password is incorrect");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountViewModel model)
        {
            Users user = new Users
            {
                FullName = model.Register.FullName,
                UserName = model.Register.UserName,
                Email = model.Register.Email
            };

            var result = await _userManager.CreateAsync(user, model.Register.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }

                await _userManager.AddToRoleAsync(user, "User");

                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("Login", model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

    }
}