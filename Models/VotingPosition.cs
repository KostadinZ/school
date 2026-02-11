using System.ComponentModel.DataAnnotations;

namespace SchoolVotingSystem.Models;

public class VotingPosition
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public int ElectionId { get; set; }
    public Election? Election { get; set; }

    public ICollection<CandidateOption> CandidateOptions { get; set; } = new List<CandidateOption>();
}
