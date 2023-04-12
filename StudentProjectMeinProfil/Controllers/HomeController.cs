using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentProjectMeinProfil.Models;
using System.Diagnostics;

namespace StudentProjectMeinProfil.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<AddUser> _userManager;
        private readonly SignInManager<AddUser> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, UserManager<AddUser> userManager, SignInManager<AddUser> signInManager, IWebHostEnvironment environment)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }

        //public IActionResult Index(string id)
        //{
        // get user information from session
        // var userId = HttpContext.Session.GetString("UserId");
        // var userName = HttpContext.Session.GetString("UserName");
        //  var user = _userManager.FindByIdAsync(id);
        //if (user == null)
        //{
        //  return NotFound();
        //}
        //else
        //{
        //  var viewUser = new UserProfile
        //{
        //  Id = user.Id
        //};
        //}


        //return View();
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(string id)
        {
            // find user by id
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // map user data to view model
            var viewModel = new UserProfile
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                Address = user.Address,
                BrithDate = user.BrithDate,
                ProfileUrl = user.ProfileUrl,
                PhoneNumber = user.PhoneNumber
            };

            // pass view model to view
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}