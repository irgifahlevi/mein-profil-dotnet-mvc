using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> GetImage(string imageName)
        {
            try
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "Image/Profile", imageName);
                var fileStream = new FileStream(path, FileMode.Open);
                var fileExtension = Path.GetExtension(path);
                return File(fileStream, $"image/{fileExtension}");
            }
            catch (Exception ex)
            {
                // Handle the exception
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> View()
        {
            // get current user
            var user = await _userManager.GetUserAsync(HttpContext.User);

            // return view with user profile data
            return View(user);
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
    }
}
