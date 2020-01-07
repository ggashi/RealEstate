using RealEstate.WebUI.Models;
using System.Collections.Generic;

namespace RealEstate.WebUI.Areas.Admin.ViewModels
{
    public class PropertiesIndexViewModel
    {
        public IEnumerable<Property> Properties { get; set; }
        public bool IsAdmin { get; set; }
    }
}
