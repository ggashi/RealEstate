using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.WebUI.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int LocationId { get; set; }
        public decimal Price { get; set; }
        public int TypeId { get; set; }
        public string Image { get; set; }
        public bool IsRent { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Location Location { get; set; }

        [ForeignKey("TypeId")]
        public Type Type { get; set; }
    }
}
