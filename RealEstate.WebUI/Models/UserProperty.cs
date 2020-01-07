using Microsoft.AspNetCore.Identity;

namespace RealEstate.WebUI.Models
{
    public class UserProperty
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PropertyId { get; set; }

        public IdentityUser User { get; set; }
        public Property Property { get; set; }

        public bool HasWrite { get; set; }
    }
}