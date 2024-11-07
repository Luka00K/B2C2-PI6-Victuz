using System.ComponentModel.DataAnnotations;

namespace Victuz.Models
{
    public class Like
    {
        public int Id { get; set; }

        [Required]
        public int SuggestionId { get; set; }
        public Suggestion? Suggestion { get; set; } // Navigatie naar Suggestion

        [Required]
        public string? UserId { get; set; } // Unieke ID van de gebruiker die heeft geliket
    }
}
