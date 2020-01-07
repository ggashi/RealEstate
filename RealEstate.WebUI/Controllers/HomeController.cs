using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RealEstate.WebUI.Data;
using RealEstate.WebUI.Models;
using RealEstate.WebUI.Models.Home;

namespace RealEstate.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;
        private ApplicationDbContext _context;

        public HomeController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<IActionResult> Index(
            string searchString,
            string currentFilter,
            string sortOrder,
            int? selectedTypeId,
            int? selectedLocationId,
            string isRent,
            int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var IsRent = isRent == "on";

            var types = new List<Models.Type>()
            {
                new Models.Type()
                {
                    Id = 0,
                    Name = "Select type"
                }
            };
            types = types.Concat(_context.Types.AsEnumerable()).ToList();

            var locations = new List<Location>()
            {
                new Location()
                {
                    Id = 0,
                    Name = "Select location"
                }
            };
            locations = locations.Concat(_context.Locations.AsEnumerable()).ToList();


            ViewData["sortOrder"] = sortOrder;
            ViewData["isRent"] = IsRent;
            ViewData["LocationId"] = new SelectList(locations, "Id", "Name", selectedLocationId);
            ViewData["TypeId"] = new SelectList(types, "Id", "Name", selectedTypeId);
            ViewData["CurrentFilter"] = searchString;

            var properties = from s in _context.Properties
                             where s.IsVisible
                             select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                properties = properties.Where(s => s.Title.Contains(searchString));
            }
            if (selectedTypeId != null && selectedTypeId != 0)
            {
                properties = properties.Where(p => p.TypeId == selectedTypeId);
            }
            if (selectedLocationId != null && selectedLocationId != 0)
            {
                properties = properties.Where(p => p.LocationId == selectedLocationId);
            }
            if (isRent != null)
            {
                properties = properties.Where(p => p.IsRent == IsRent);
            }

            switch (sortOrder)
            {
                case "title":
                    properties = properties.OrderBy(s => s.Title);
                    break;
                case "type":
                    properties = properties.OrderBy(s => s.TypeId);
                    break;
                case "location":
                    properties = properties.OrderBy(s => s.LocationId);
                    break;
                case "date":
                    properties = properties.OrderBy(s => s.CreatedDate);
                    break;
                case "price":
                    properties = properties.OrderBy(s => s.Price);
                    break;
                case "is_rent":
                    properties = properties.OrderByDescending(s => s.IsRent);
                    break;
            }


            int pageSize = Convert.ToInt32(_configuration.GetSection("PageSize").Value);
            var vm = new IndexViewModel()
            {
                Properties = await PaginatedList<Property>.CreateAsync(properties.AsNoTracking(), page ?? 1, pageSize)
            };
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var property = await _context.Properties
                .Include(p => p.Location)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
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


        public async Task<IActionResult> Main(
        string searchString,
        string currentFilter,
        string sortOrder,
        int? selectedTypeId,
        int? selectedLocationId,
        string isRent,
        int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var IsRent = isRent == "on";

            var types = new List<Models.Type>()
            {
                new Models.Type()
                {
                    Id = 0,
                    Name = "Select type"
                }
            };
            types = types.Concat(_context.Types.AsEnumerable()).ToList();

            var locations = new List<Location>()
            {
                new Location()
                {
                    Id = 0,
                    Name = "Select location"
                }
            };
            locations = locations.Concat(_context.Locations.AsEnumerable()).ToList();


            ViewData["sortOrder"] = sortOrder;
            ViewData["isRent"] = IsRent;
            ViewData["LocationId"] = new SelectList(locations, "Id", "Name", selectedLocationId);
            ViewData["TypeId"] = new SelectList(types, "Id", "Name", selectedTypeId);
            ViewData["CurrentFilter"] = searchString;

            var properties = from s in _context.Properties
                             where s.IsVisible
                             select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                properties = properties.Where(s => s.Title.Contains(searchString));
            }
            if (selectedTypeId != null && selectedTypeId != 0)
            {
                properties = properties.Where(p => p.TypeId == selectedTypeId);
            }
            if (selectedLocationId != null && selectedLocationId != 0)
            {
                properties = properties.Where(p => p.LocationId == selectedLocationId);
            }
            if (isRent != null)
            {
                properties = properties.Where(p => p.IsRent == IsRent);
            }

            switch (sortOrder)
            {
                case "title":
                    properties = properties.OrderBy(s => s.Title);
                    break;
                case "type":
                    properties = properties.OrderBy(s => s.TypeId);
                    break;
                case "location":
                    properties = properties.OrderBy(s => s.LocationId);
                    break;
                case "date":
                    properties = properties.OrderBy(s => s.CreatedDate);
                    break;
                case "price":
                    properties = properties.OrderBy(s => s.Price);
                    break;
                case "is_rent":
                    properties = properties.OrderByDescending(s => s.IsRent);
                    break;
            }


            int pageSize = Convert.ToInt32(_configuration.GetSection("PageSize").Value);
            var vm = new IndexViewModel()
            {
                Properties = await PaginatedList<Property>.CreateAsync(properties.AsNoTracking(), page ?? 1, pageSize)
            };
            return View(vm);
        }


        [HttpGet]
        public ActionResult Contact(int id)
        {
            return View(new Contact() { PropertyId = id });
        }

        // GET: Contacts/Create

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([Bind("FullName,Email,Message,PhoneNumber,PropertyId")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }
        
    }
}
