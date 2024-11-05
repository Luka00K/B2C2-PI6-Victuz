using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Victuz.Models
{
    public class ActivityModel
    {
        public int? Id { get; set; }

        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

        [Display(Name = "Datum en Tijd")]
        public DateTime DateTime { get; set; }

        [Display(Name = "Maximaal aantal deelnemers")]
        public int MaxParticipants { get; set; }

        [Display(Name = "Categorie")]
        public List<int> CategoryIds { get; set; }

        [Display(Name = "Locatie")]
        public int LocationId { get; set; }

        public List<Category>? Categories { get; set; }
        public Location? Location { get; set; }
        // FreeForEverybody, FreeForMembers, Paid 
        public string PaymentType { get; set; }
        public string? OrganizerId { get; set; }
        public Organizer? Organizer { get; set; }
        public ICollection<Registration>? Registrations { get; set; }
    }
}
