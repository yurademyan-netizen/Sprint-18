using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskAuthenticationAuthorization.Models;

namespace TaskAuthenticationAuthorization.Controllers
{
    public class AccountController : Controller
    {
        private readonly ShoppingContext context;

        public AccountController(ShoppingContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                Role role = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "buyer");

                if (user == null)
                {
                    context.Users.Add(new User
                    {
                        Email = model.Email,
                        Password = model.Password,
                        Role = role
                    });

                    await context.SaveChangesAsync();

                    await Authenticate(model.Email);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login and(or) password");
                }
            }

            return View(model);
        }

        private async Task Authenticate(string email)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.RoleName),
                    new Claim("buyerType", "Regular")
                };
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie"
                    , ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            }
        }
    }
}
