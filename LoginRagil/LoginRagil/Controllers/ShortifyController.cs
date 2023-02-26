using LoginRagil.Models;
using LoginRagil.NewFolder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LoginRagil.Controllers
{
    [Route("shortify")]
    public class ShortifyController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private LoginDB _db;

        public ShortifyController(ILogger<HomeController> logger, LoginDB db)
        {
            _logger = logger;
            _db = db;
        }
        [HttpGet("links")]
        public IActionResult Links()
        {
            var user = HttpContext.User;
            var email = user.FindFirstValue(ClaimTypes.Email);
            var tempUser = _db.Users.FirstOrDefault(u => u.Email == email);
            var listforuser = _db.UrlPairs.ToList().Where(u => u.UrlUserEmail != null).ToList().FindAll(u => u.UrlUserEmail == email);
            if (listforuser == null) { return View(new List<UrlPair>()); }
            return View(listforuser);
        }
        [HttpGet("logger/{shorturl}")]
        public IActionResult Logger(string shorturl)
        {
            shorturl = System.Web.HttpUtility.UrlDecode(shorturl);
            var listloggers = _db.Entries.ToList().FindAll(e => e.ShortUrl == shorturl);
            if (listloggers.Count == 0) { return View(new List<Entry>()); }
            return View(listloggers);
        }
        [HttpGet("delete")]
        public async Task<IActionResult> Delete([FromQuery] string shorturl)
        {
            var urltodelete = await _db.UrlPairs.FirstAsync(u => u.ShortUrl == shorturl);
            if (urltodelete != null)
            {
                _db.UrlPairs.Remove(urltodelete);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Links");
        }
    }
}
