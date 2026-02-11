using System.ComponentModel.DataAnnotations;

namespace SchoolVotingSystem.Models;

public class Election
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<VotingPosition> Positions { get; set; } = new List<VotingPosition>();
}
