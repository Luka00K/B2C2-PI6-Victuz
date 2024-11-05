namespace Victuz.Models
{
    public class ActivityDetailsViewModel
    {
        public ActivityModel? Activity { get; set; }
        public bool IsRegistered { get; set; }
        public int AvailableSpots => (Activity.MaxParticipants ) - (Activity.Registrations?.Count ?? 0);

        public int ActivityId => Activity?.Id ?? 0;
    }
}
