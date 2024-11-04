using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Victuz.Models
{
    public class ActivityModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public int MaxParticipants { get; set; }
        public List <int> CategoryIds { get; set; }
        public int LocationId { get; set; }
        public List<Category>? Categories { get; set; }
        public Location? Location { get; set; }
        // FreeForEverybody, FreeForMembers, Paid 
        public string PaymentType { get; set; }
        public ICollection<Registration>? Registrations { get; set; }
    }
}
