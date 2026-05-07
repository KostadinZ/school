using System.ComponentModel.DataAnnotations;

namespace SchoolEventRegistration.Models;

public class Student
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(40)]
    [Display(Name = "Class")]
    public string ClassName { get; set; } = string.Empty;

    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
