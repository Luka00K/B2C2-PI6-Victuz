using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Victuz.Models
{
    public class ActivityModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateTime { get; set; }
        public int? MaxParticipants { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public Location? Location { get; set; }
        public ICollection<Registration>? Registrations { get; set; }
    }
}
