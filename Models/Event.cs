using System.ComponentModel.DataAnnotations;

namespace SchoolEventRegistration.Models;

public class Event
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required, DataType(DataType.DateTime)]
    public DateTime Date { get; set; } = DateTime.Today.AddDays(7);

    [Required, StringLength(150)]
    public string Location { get; set; } = string.Empty;

    [Range(1, 500)]
    [Display(Name = "Maximum Participants")]
    public int MaxParticipants { get; set; } = 30;

    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public int SeatsLeft => Math.Max(0, MaxParticipants - Registrations.Count);
}
