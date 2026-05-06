namespace SchoolEventRegistration.ViewModels;

public class EventListItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
    public int RegisteredCount { get; set; }
    public int SeatsLeft => Math.Max(0, MaxParticipants - RegisteredCount);
}
