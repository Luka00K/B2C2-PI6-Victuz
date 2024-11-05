using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Victuz.Models
{
    public class Suggestion
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        //[Required]
        public Member? Member { get; set; }

        // Verzameling van Likes
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
