using System.ComponentModel.DataAnnotations;

namespace Victuz.Models
{
    public class Suggestion
    {
        public int Id { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public Member? Member { get; set; }
    }
}
