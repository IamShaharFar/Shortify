using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http;
using System;
using LoginRagil.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using LoginRagil.NewFolder;
using LoginRagil.Servieces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.WebRequestMethods;

namespace UrlShortner.Controllers
{
    [Route("api")]
    [ApiController]
    public class ShortnerApiController : ControllerBase
    {
        private LoginDB _db;
        private readonly ILogger<LoginDB> _logger;
        private readonly SignInManager<LUser> _signInManager;
        private readonly ShortifyService _service;
        public ShortnerApiController(LoginDB db, ILogger<LoginDB> logger, SignInManager<LUser> signInManager)
        {
            _db = db;
            _logger = logger;
            _signInManager = signInManager;
            _service = new ShortifyService(_db);
        }
        [HttpPost("doshort")]
        public async Task<ActionResult<string>> GetShortUrl([FromQuery]string fullurl)
        {
            var user = HttpContext.User;
            var email = user.FindFirstValue(ClaimTypes.Email);
            var tempUser = _db.Users.FirstOrDefault(u => u.Email == email);
            if (tempUser != null)
            {
                var list = _db.UrlPairs.ToList();
                UrlPair? found = null;
                if (list.Count > 0) { found = list.FirstOrDefault(u => { return u.UrlUserEmail == tempUser.Email && u.FullUrl == fullurl; }); }
                if (found != null) { return found.ShortUrl!; }
            }
            else
            {
                var list = _db.UrlPairs.ToList();
                UrlPair? found = null;
                if (list.Count > 0) { found = _db.UrlPairs.ToList().FirstOrDefault(u => { return u.UrlUserEmail == "" && u.FullUrl == fullurl; }); }
                if (found != null) { return found.ShortUrl!; }
            }
            var s = await _service.Short(fullurl);
            var url = _db.UrlPairs.FirstOrDefault(u => u.ShortUrl == s);
            if (tempUser!=null)
            {
                url!.UrlUserEmail = tempUser.Email!;
                _db.SaveChanges();
            }
            return s != "" ? Ok(s) : BadRequest();
        }

        [HttpPost("custom/{custom}")]
        public ActionResult<string> GetCustomUrl([FromBody]string fullurl , string custom)
        {
            if (!User.Identity!.IsAuthenticated) { return BadRequest("You need to login to do this action!"); }
            var user = HttpContext.User;
            var email = user.FindFirstValue(ClaimTypes.Email);
            var tempUser = _db.Users.FirstOrDefault(u => u.Email == email);
            if(!_db.UrlPairs.Any(u => u.ShortUrl == "https://localhost:7207/s/" + custom))
            {
                _db.UrlPairs.Add(new UrlPair { ShortUrl = "https://localhost:7207/s/" + custom, FullUrl = fullurl, UrlUserEmail = email });
                _db.SaveChanges();
                return Ok("https://localhost:7207/s/" + custom);
            }
            return StatusCode(409, "custom already taken!");
        }

        [HttpGet("/s/{shorturl}")]
        public ActionResult Get(string shorturl)
        {
            var url = _db.UrlPairs.FirstOrDefault(u => u.ShortUrl == "https://localhost:7207/s/" + shorturl);
            if (url != null)
            { 
                url.Entries++;
                url.EntriesPc.Add(new Entry { Ip = Request.HttpContext.Connection.RemoteIpAddress!.ToString(), EntryTime = DateTime.UtcNow, ShortUrl = "https://localhost:7207/s/" + shorturl });

                _db.SaveChanges();
            }
            return Redirect(url.FullUrl);
        }
    }
}
