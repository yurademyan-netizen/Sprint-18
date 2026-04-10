using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskAuthenticationAuthorization.Models;

namespace TaskAuthenticationAuthorization.Controllers
{
    //[Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ShoppingContext _context;

        public AdminController(ShoppingContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.Include(u => u.Role).ToList();

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(int userId, string newRole , string newBuyerType)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            newRole = (newRole ?? string.Empty).Trim().ToLowerInvariant();
            var role = _context.Roles.FirstOrDefault(r => r.RoleName == newRole);

            if(role != null)
            {
                user.RoleId = role.Id;
            }

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
    } 
}
