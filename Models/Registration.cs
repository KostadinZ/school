using System.ComponentModel.DataAnnotations;

namespace SchoolEventRegistration.Models;

public class Registration
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Event")]
    public int EventId { get; set; }

    [Required]
    [Display(Name = "Student")]
    public int StudentId { get; set; }

    [DataType(DataType.DateTime)]
    [Display(Name = "Registration Date")]
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    public Event? Event { get; set; }
    public Student? Student { get; set; }
}
