using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArtNews.Areas.Identity.Data;
using ArtNews.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArtNews.Api
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserManager<ArtNewsUser> _userManager;
        SignInManager<ArtNewsUser> _signInManager;
        public DBNews _dbNews { get; set; }


        public UserController(DBNews dbNews, UserManager<ArtNewsUser> userManager, SignInManager<ArtNewsUser> signInManager)
        {
            _dbNews = dbNews;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegInfo regInfo)
        {
            ArtNewsUser user = new ArtNewsUser
            {
                signalId = regInfo.signal,
                Email = regInfo.user,
                UserName = regInfo.user,

            };
            var status = await _userManager.CreateAsync(user, regInfo.pass);
            if (status.Succeeded)
                return Ok();
            return BadRequest("Username already exist");
        }
        [HttpPost]
        public async Task<IActionResult> Login(RegInfo regInfo)
        {
            var user = await _userManager.FindByEmailAsync(regInfo.user);
            if (user == null)
                return BadRequest("Wrong username or password");
            var status = await _signInManager.PasswordSignInAsync(user, regInfo.pass, false, false);
            if (status.Succeeded)
                return Ok();
            return BadRequest("Wrong username or password");
        }
    }
    public class RegInfo
    {
        public string signal { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
    }
}