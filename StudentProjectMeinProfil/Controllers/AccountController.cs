using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentProjectMeinProfil.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IO;
using NuGet.Protocol.Plugins;

namespace StudentProjectMeinProfil.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AddUser> _userManager;
        private readonly SignInManager<AddUser> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public AccountController(UserManager<AddUser> userManager, SignInManager<AddUser> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterView register, List<IFormFile> ImagePath)
        {
            var path = _environment.WebRootPath + "/Image/Profile/";

            if(ImagePath.Count > 0)
            {
                var fileTarget = path + ImagePath[0].FileName;
                using (var stream = new FileStream(fileTarget, FileMode.Create))
                {
                    await ImagePath[0].CopyToAsync(stream);
                }
            }         

            if (ModelState.IsValid)
            {
                // Check if email already exists in the database
                var userNameExists = await _userManager.FindByNameAsync(register.UserName);
                if (userNameExists != null)
                {
                    ModelState.AddModelError("UserName", "Username already exists.");
                    return View(register);
                }

                var user = new AddUser
                {
                    FullName = register.FullName,
                    Email = register.Email,
                    BrithDate = register.BrithDate,
                    Address = register.Address,
                    PhoneNumber = register.PhoneNumber,
                    UserName = register.UserName,
                    ProfileUrl = ImagePath[0].FileName
                };

               
                var result = await _userManager.CreateAsync(user, register.Password);

                if (result.Succeeded)
                {
                    var users = await _userManager.FindByNameAsync(register.UserName);
                    if (users != null)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Profile", "User", new { id = users.Id });
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(register);
        }


        public IActionResult Login(string? returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginView login, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(login.UserName);
                    if (user != null)
                    {
                        return Redirect(returnUrl ?? "/User/" + user.Id);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(login);
        }


        public async Task<IActionResult> Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction("Login", "Account");
        }
    }
}
