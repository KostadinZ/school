namespace SchoolVotingSystem.ViewModels;

public class PositionResultViewModel
{
    public string PositionName { get; set; } = string.Empty;
    public int TotalVotes { get; set; }
    public List<CandidateResultViewModel> Candidates { get; set; } = [];
}
