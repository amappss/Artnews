using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArtNews.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArtNews.Controllers
{
    public class LoginController : Controller
    {
        UserManager<ArtNewsUser> userManager;
        SignInManager<ArtNewsUser> signInManager;
        public LoginController(UserManager<ArtNewsUser> _userManager, SignInManager<ArtNewsUser> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }
        public IActionResult adminSignin()
        {
            return View();
        }
        public async Task<IActionResult> signInConfirm(string email,string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Json("Wrong username or password");
            var status = await signInManager.PasswordSignInAsync(user, password, true, false);
            if (status.Succeeded)
            {
                if(await userManager.IsInRoleAsync(user,"admin"))
                    return RedirectToAction("ArtPiecesList", "News");
                else
                    return Json("Not authorized");
            }

            return RedirectToAction("adminSignin", "Login");
        }

    }
}