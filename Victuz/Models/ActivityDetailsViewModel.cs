namespace Victuz.Models
{
    public class ActivityDetailsViewModel
    {
        public ActivityModel? Activity { get; set; }
        public bool IsRegistered { get; set; }

        public int AvailableSpots => Activity.MaxParticipants - (Activity.Registrations?.Count ?? 0);

        // Locatie-informatie
        public string? LocationName => Activity.Location?.Name;
        public string? Street => Activity.Location?.Street;
        public int? HouseNumber => Activity.Location?.HouseNumber;
        public string? City => Activity.Location?.City;
    }
}
