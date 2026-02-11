using System.ComponentModel.DataAnnotations;

namespace SchoolVotingSystem.Models;

public class Vote
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string VoterId { get; set; } = string.Empty;

    public int VotingPositionId { get; set; }
    public VotingPosition? VotingPosition { get; set; }

    public int CandidateOptionId { get; set; }
    public CandidateOption? CandidateOption { get; set; }

    public DateTime VotedAtUtc { get; set; } = DateTime.UtcNow;
}
