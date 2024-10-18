using System.ComponentModel.DataAnnotations;

namespace Victuz.Models
{
    public class Location
    {
        public int Id { get; set; }
        //[Required]
        public string? Name { get; set; }
        //[Required]
        public string? Street { get; set; }
        //[Required]
        public int? HouseNumber { get; set; }
        //[Required]
        public string? City { get; set; }
        //[Required]
        public int? MaxCapacity { get; set; }
       // [Required]
        public ICollection<ActivityModel>? Activities { get; set; }
    }
}
