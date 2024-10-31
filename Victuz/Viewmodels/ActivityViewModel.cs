using System;

namespace Victuz.Viewmodels
{
    public class ActivityViewModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateTime { get; set; }
        public int? MaxParticipants { get; set; }
        public string? CategoryName { get; set; } // Voor het weergeven van de categorie
        public string? LocationName { get; set; }
        public string? LocationStreet { get; set; }
        public int? LocationHouseNumber { get; set; }
        public string? LocationCity { get; set; }
    }
}
