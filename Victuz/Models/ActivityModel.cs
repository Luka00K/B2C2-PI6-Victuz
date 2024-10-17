using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace Victuz.Models
{
    public class ActivityModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public int MaxParticipants { get; set; }
        [Required]
        public List<int> Categoryids { get; set; }
        public List<Category>? Categories { get; set; }
        [Required]
        public Location Location { get; set; }
        public ICollection<Registration>? Registrations { get; set; }

    }
}
