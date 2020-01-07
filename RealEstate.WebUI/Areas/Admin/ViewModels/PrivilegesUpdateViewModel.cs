using Microsoft.AspNetCore.Identity;
using RealEstate.WebUI.Models;

namespace RealEstate.WebUI.Areas.Admin.ViewModels
{
    public class PrivilegesUpdateViewModel
    {
        public IdentityUser User { get; set; }
        public Property Property { get; set; }
        public UserProperty UserProperty { get; set; }
    }
}
