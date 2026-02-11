using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolVotingSystem.ViewModels;

public class VotePositionInputViewModel
{
    public int VotingPositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please choose a candidate.")]
    public int? SelectedCandidateOptionId { get; set; }

    public List<SelectListItem> CandidateOptions { get; set; } = [];
}
