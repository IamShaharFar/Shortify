using LoginRagil.Models;
using LoginRagil.NewFolder;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoginRagil.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> _logger;
        private LoginDB _db;
        private SignInManager<LUser> _SignInManager;

        public AccountsController(ILogger<AccountsController> logger, LoginDB db, SignInManager<LUser> signInManager)
        {
            _logger = logger;
            _db = db;
            _SignInManager = signInManager;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View(new RegisteUserModel { });
        }

        [HttpPost("register")]
        public IActionResult Register(RegisteUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new LUser { UserName = model.Name, Email = model.Email, Password = model.Password };
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View(new LoginUserModel { });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            if (_db.Users.Any(u => u.Email == model.Email))
            {
                var user = _db.Users.First(u => u.Email == model.Email);
                if (user.Password == model.Password)
                {
                    //todo: add to sign in manager
                    await SignInAsync(user);
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Home");
            }
            model.Error = "There is a incorrect field, please try again.";
            return View(model);
        }
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public async 
        Task
SignInAsync(LUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties { RedirectUri = "https://localhost:7207/GoogleResponse" };
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("GoogleResponse")]
        public async Task<IActionResult> GoogleResponse()
        {
            var res = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = res.Principal!.Identities.FirstOrDefault()!
                        .Claims.Select(c => new { c.Value, c.Type, c.Properties });
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            if (_db.Users.Any(u => u.Email == email))
            {
                var user = _db.Users.First(u => u.Email == email);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Register");
            }
        }

        [HttpGet("googleregister")]
        public IActionResult GoogleRegister()
        {
            var prop = new AuthenticationProperties { RedirectUri = "https://localhost:7207/googleregisterres" };
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet("googleregisterres")]
        public async Task<IActionResult> GoogleRegisterRes()
        {
            var res = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = res.Principal!.Identities.FirstOrDefault()!
                        .Claims.Select(c => new { c.Value, c.Type, c.Properties });
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            if (_db.Users.Any(u => u.Email == email))
            {
                return RedirectToAction("register");
            }
            var user = new LUser()
            {
                UserName = name,
                Email = email
            };
            _db.Users.Add(user);
            _db.SaveChanges();
            await SignInAsync(user);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("settings")]
        public IActionResult Settings()
        {
            var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (_db.Users.Any(u => u.Email == email))
            {
                var user = _db.Users.First(u => u.Email == email);
                return View(new UserProfileModel { Name = user.UserName });
            }
            //return StatusCode(401);
            return View("Register");
        }

        [HttpPost("settings")]
        public IActionResult Settings(UserProfileModel model)
        {
            var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var user = _db.Users.First(u => email == u.Email);
            if (ModelState.IsValid)
            {
                if (model.OldPassword == user.Password)
                {
                    user.UserName = model.Name;
                    user.Password = model.NewPassword;
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    model.Errors = "password isnt correct";
                }
            }
            return View(model);
        }


    }
}
