using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.WebUI.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public String FullName { get; set; }
        [StringLength(100),  Required]
        public String Email { get; set; }
        [StringLength(100) , Required]
        public String Message { get; set; }
        [StringLength(150), Required]
        public String PhoneNumber { get; set; }
        public int PropertyId;

    }
}
