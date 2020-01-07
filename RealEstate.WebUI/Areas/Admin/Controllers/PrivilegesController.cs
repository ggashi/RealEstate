using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstate.WebUI.Areas.Admin.ViewModels;
using RealEstate.WebUI.Data;
using RealEstate.WebUI.Models;

namespace RealEstate.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class PrivilegesController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public PrivilegesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var model = new PrivilegesIndexViewModel();

            model.Property = _context.Properties.FirstOrDefault(p => p.Id == id);
            if (model.Property == null)
            {
                return NotFound();
            }

            model.Users = _context.Users.ToList();
            model.UserProperties = _context.UserProperties.Where(up => up.PropertyId == id).ToList();

            return View(model);
        }

        public async Task<IActionResult> Update(int propertyId, string userId)
        {
            var model = new PrivilegesUpdateViewModel();

            model.Property = await _context.Properties.FindAsync(propertyId);
            if (model.Property == null)
            {
                return NotFound();
            }
            model.User = await _context.Users.FindAsync(userId);
            model.UserProperty = _context.UserProperties.FirstOrDefault(up => up.UserId == userId && up.PropertyId == propertyId);
            if (model.UserProperty == null)
            {
                model.UserProperty = new Models.UserProperty()
                {
                    UserId = model.User.Id,
                    PropertyId = model.Property.Id
                };
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserProperty userProperty)
        {
            var existing = _context.UserProperties.FirstOrDefault(up => up.UserId == userProperty.UserId && up.PropertyId == userProperty.PropertyId);

            if (existing == null)
            {
                existing = userProperty;
                _context.UserProperties.Add(existing);
            }
            else
            {
                existing.HasWrite = userProperty.HasWrite;
                _context.Update(existing);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { id = existing.PropertyId });
        }

        public async Task<IActionResult> RevokePrivilege(int? id, int propertyId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProperty = await _context.UserProperties.FindAsync(id);
            if (userProperty != null)
            {
                _context.Remove(userProperty);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { id = propertyId });
        }
    }
}