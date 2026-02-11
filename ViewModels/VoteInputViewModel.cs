using System.ComponentModel.DataAnnotations;

namespace SchoolVotingSystem.ViewModels;

public class VoteInputViewModel
{
    [Required, StringLength(50)]
    [Display(Name = "Student ID / Voter ID")]
    public string VoterId { get; set; } = string.Empty;

    public int ElectionId { get; set; }
    public string ElectionName { get; set; } = string.Empty;
    public List<VotePositionInputViewModel> Positions { get; set; } = [];
}
