namespace SchoolVotingSystem.ViewModels;

public class CandidateResultViewModel
{
    public string CandidateName { get; set; } = string.Empty;
    public string? Manifesto { get; set; }
    public int VoteCount { get; set; }
}
