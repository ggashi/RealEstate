using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealEstate.WebUI.Areas.Admin.ViewModels;
using RealEstate.WebUI.Data;
using RealEstate.WebUI.Models;

namespace RealEstate.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PropertiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PropertiesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Properties
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var isAdmin = roles.Contains("Administrator");

            var model = new PropertiesIndexViewModel()
            {
                Properties = await _context.Properties.Include(p => p.Location).Include(p => p.Type).ToListAsync(),
                IsAdmin = isAdmin
            };

            return View(model);
        }

        // GET: Admin/Properties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var isAdmin = roles.Contains("Administrator");

            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .Include(p => p.Location)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (property == null)
            {
                return NotFound();
            }

            if (!isAdmin)
            {
                var userProperty = _context.UserProperties.Where(up => up.UserId == user.Id && up.PropertyId == property.Id).FirstOrDefault();
                if (userProperty == null)
                {
                    return NotFound();
                }
            }

            return View(property);
        }

        // GET: Admin/Properties/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Name");
            ViewData["TypeId"] = new SelectList(_context.Set<Type>(), "Id", "Name");
            return View();
        }

        // POST: Admin/Properties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,LocationId,Price,TypeId,IsRent,IsVisible")] Property property, IFormFile imageFile)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (ModelState.IsValid && imageFile != null)
            {
                var fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);
                var fileDirectory = "wwwroot/images";

                if (!Directory.Exists(fileDirectory))
                {
                    Directory.CreateDirectory(fileDirectory);
                }

                string filePath = fileDirectory + "/" + fileName;

                // Copy file to path...
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                property.Image = "/images/" + fileName;
            }

            if (ModelState.IsValid)
            {
                property.CreatedDate = System.DateTime.Now;
                _context.Add(property);
                _context.UserProperties.Add(new UserProperty()
                {
                    UserId = user.Id,
                    PropertyId = property.Id,
                    HasWrite = true
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Name", property.LocationId);
            ViewData["TypeId"] = new SelectList(_context.Set<Type>(), "Id", "Name", property.TypeId);
            return View(property);
        }

        // GET: Admin/Properties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var isAdmin = roles.Contains("Administrator");

            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties.FindAsync(id);

            if (property == null)
            {
                return NotFound();
            }
            if (!isAdmin)
            {
                var userProperty = _context.UserProperties.Where(up => up.UserId == user.Id && up.PropertyId == property.Id).FirstOrDefault();
                if (userProperty == null || !userProperty.HasWrite)
                {
                    return NotFound();
                }
            }

            ViewData["Image"] = property.Image;
            ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Name", property.LocationId);
            ViewData["TypeId"] = new SelectList(_context.Set<Type>(), "Id", "Name", property.TypeId);
            return View(property);
        }

        // POST: Admin/Properties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,LocationId,Price,TypeId,IsRent,IsVisible")] Property property, IFormFile imageFile)
        {
            if (id != property.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid && imageFile != null)
            {
                var fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);
                var fileDirectory = "wwwroot/images";

                if (!Directory.Exists(fileDirectory))
                {
                    Directory.CreateDirectory(fileDirectory);
                }

                string filePath = fileDirectory + "/" + fileName;

                // Copy file to path...
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                property.Image = "/images/" + fileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    property.UpdatedDate = System.DateTime.Now;
                    _context.Update(property);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(property.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Name", property.LocationId);
            ViewData["TypeId"] = new SelectList(_context.Set<Type>(), "Id", "Name", property.TypeId);
            return View(property);
        }

        // GET: Admin/Properties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var isAdmin = roles.Contains("Administrator");

            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .Include(p => p.Location)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (property == null)
            {
                return NotFound();
            }

            if (!isAdmin)
            {
                var userProperty = _context.UserProperties.Where(up => up.UserId == user.Id && up.PropertyId == property.Id).FirstOrDefault();
                if (userProperty == null)
                {
                    return NotFound();
                }
            }

            return View(property);
        }

        // POST: Admin/Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Deleted));
        }

        public IActionResult Deleted()
        {
            return View();
        }

        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.Id == id);
        }
    }
}
