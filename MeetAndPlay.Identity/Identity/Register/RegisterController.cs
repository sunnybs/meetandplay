using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using MeetAndPlay.Data.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerHost.Quickstart.UI.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly UserManager<User> _userManager;

        public RegisterController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid)
                return View("Index", registerModel);

            var existedUser = await _userManager.FindByNameAsync(registerModel.UserName);
            if (existedUser != null)
            {
                ModelState.AddModelError("UserName", "Пользователь с таким именем уже существует.");
                return View("Index", registerModel);
            }
            
            var newUser = new User
            {
                Email = registerModel.Email,
                EmailConfirmed = true,
                UserName = registerModel.UserName,
                PhoneNumber = registerModel.PhoneNumber,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Gender = registerModel.Gender
            };

            var result = await _userManager.CreateAsync(newUser, registerModel.Password);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Password", "Что-то пошло не так.");
                return View("Index", registerModel);
            }

            await _userManager.AddClaimsAsync(newUser, new []
            {
                new Claim(JwtClaimTypes.Name, newUser.UserName),
                new Claim(JwtClaimTypes.GivenName, newUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, newUser.LastName),
                new Claim(JwtClaimTypes.Email, newUser.Email),
            });

            return RedirectToAction("Login", "Account");
        }
    }
}