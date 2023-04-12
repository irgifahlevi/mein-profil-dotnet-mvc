using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentProjectMeinProfil.Models;

namespace StudentProjectMeinProfil.Controllers
{
    
    public class UserController : Controller
    {
        private readonly UserManager<AddUser> _userManager;
        private readonly AuthDbContext _authDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(UserManager<AddUser>userManager, AuthDbContext authDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _authDbContext = authDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        
        [Authorize]
        [Route("user/{id}")]
        public async Task<IActionResult> Profile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated || user.Id != _userManager.GetUserId(User))
            {
                var userProfile = new UserProfile
                {
                    FullName = user.FullName,
                };

                return View("ProfilePublic", userProfile);
            }

            var model = new UserProfile
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                Address = user.Address,
                BrithDate = user.BrithDate,
                PhoneNumber = user.PhoneNumber,
                ProfileUrl = user.ProfileUrl
            };

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            // get current user
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            // cek apakah user sedang login dan id yang diminta sama
            if (currentUser.Id != id)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            if (currentUser == null)
            {
                return NotFound();
            }

            // get user profile data
            var userProfile = new UserProfile
            {
                Id = currentUser.Id,
                FullName = currentUser.FullName,
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Address = currentUser.Address,
                BrithDate = currentUser.BrithDate,
                PhoneNumber = currentUser.PhoneNumber,
                ProfileUrl = currentUser.ProfileUrl
            };

            return View(userProfile);
        }


        [Authorize]
        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateProfile(string id, UserProfile model, List<IFormFile> profilePath)
        {
            var path = _webHostEnvironment.WebRootPath + "/Image/Profile/";

            if (profilePath.Count > 0)
            {
                var fileTarget = path + profilePath[0].FileName;
                using (var stream = new FileStream(fileTarget, FileMode.Create))
                {
                    await profilePath[0].CopyToAsync(stream);
                }
            }

            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                if (currentUser.Id != id)
                {
                    return RedirectToAction("AccessDenied", "Account");
                }

                currentUser.FullName = model.FullName;
                currentUser.UserName = model.UserName;
                currentUser.Email = model.Email;
                currentUser.Address = model.Address;
                currentUser.BrithDate = model.BrithDate;
                currentUser.PhoneNumber = model.PhoneNumber;
                currentUser.ProfileUrl = profilePath[0].FileName;

                var result = await _userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", "User", new { id = currentUser.Id });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);

        }
    }
}
