namespace Victuz.Models
{
    public class ActivityDetailsViewModel
    {
        public ActivityModel? Activity { get; set; }
        public bool IsRegistered { get; set; }
        public int AvailableSpots => (Activity.MaxParticipants ?? 0) - (Activity.Registrations?.Count ?? 0);
    }
}
