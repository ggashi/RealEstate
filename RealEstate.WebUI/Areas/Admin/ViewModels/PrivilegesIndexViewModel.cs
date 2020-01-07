using Microsoft.AspNetCore.Identity;
using RealEstate.WebUI.Models;
using System.Collections.Generic;

namespace RealEstate.WebUI.Areas.Admin.ViewModels
{
    public class PrivilegesIndexViewModel
    {
        public IEnumerable<IdentityUser> Users { get; set; }
        public IEnumerable<UserProperty> UserProperties { get; set; }
        public Property Property { get; set; }
    }
}
