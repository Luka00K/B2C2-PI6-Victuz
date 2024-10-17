namespace Victuz.Models
{
    public class Member : Person
    {
        ICollection<Suggestion>? Suggestions { get; set; }
        ICollection<Registration>? Registrations { get; set; }
    }
}
