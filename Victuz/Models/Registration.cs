using System.ComponentModel.DataAnnotations;

namespace Victuz.Models
{
    public class Registration
    {
        public int Id { get; set; }
        [Required]
        public Member? Member { get; set; }
        [Required]
        public ActivityModel? Activity { get; set; }
        public bool? IsPresent { get; set; }

    }
}
