using System.ComponentModel.DataAnnotations;

namespace Victuz.Models
{
    public class Registration
    {
        public int Id { get; set; }
        //For if the member is logged in 
        public Member? Member { get; set; }
        //For if the member is not logged in
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public ActivityModel? Activity { get; set; }
        public bool? IsPresent { get; set; }

    }
}
