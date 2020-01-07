using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace RealEstate.WebUI.Models.Home
{
    public class IndexViewModel
    {
        public PaginatedList<Property> Properties { get; set; }
        public SelectList Locations { get; set; }
        public SelectList Types { get; set; }
    }
}
