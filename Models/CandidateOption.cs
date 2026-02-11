using System.ComponentModel.DataAnnotations;

namespace SchoolVotingSystem.Models;

public class CandidateOption
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string CandidateName { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Manifesto { get; set; }

    public int VotingPositionId { get; set; }
    public VotingPosition? VotingPosition { get; set; }

    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
