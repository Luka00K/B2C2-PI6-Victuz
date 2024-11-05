using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Victuz.Models
{
    public class Location
    {
        public int Id { get; set; }

        //[Required]
        [Display(Name = "Locatienaam")]
        public string? Name { get; set; }

        //[Required]
        [Display(Name = "Straat")]
        public string? Street { get; set; }

       // [Required]
        [Display(Name = "Huisnummer")]
        public int? HouseNumber { get; set; }

        //[Required]
        [Display(Name = "Stad")]
        public string? City { get; set; }

        //[Required]
        [Display(Name = "Maximale capaciteit")]
        public int? MaxCapacity { get; set; }

        public ICollection<ActivityModel>? Activities { get; set; }
    }
}
