using SchoolVotingSystem.Models;

namespace SchoolVotingSystem.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Elections.Any())
        {
            return;
        }

        var election = new Election
        {
            Name = "2026 School Leadership Election",
            Description = "Vote for school representatives including President and School President.",
            IsActive = true,
            Positions =
            [
                new VotingPosition
                {
                    Name = "President",
                    CandidateOptions =
                    [
                        new CandidateOption { CandidateName = "Alex Rivera", Manifesto = "Student wellness and club funding" },
                        new CandidateOption { CandidateName = "Jordan Lee", Manifesto = "Digital classrooms and innovation" },
                        new CandidateOption { CandidateName = "Sam Patel", Manifesto = "Transparent student council budget" }
                    ]
                },
                new VotingPosition
                {
                    Name = "School President",
                    CandidateOptions =
                    [
                        new CandidateOption { CandidateName = "Taylor Morgan", Manifesto = "Campus safety and mentorship" },
                        new CandidateOption { CandidateName = "Jamie Chen", Manifesto = "Expanded sports and arts support" },
                        new CandidateOption { CandidateName = "Casey Brooks", Manifesto = "Community outreach and volunteering" }
                    ]
                },
                new VotingPosition
                {
                    Name = "Vice President",
                    CandidateOptions =
                    [
                        new CandidateOption { CandidateName = "Riley Kim", Manifesto = "Academic support programs" },
                        new CandidateOption { CandidateName = "Drew Singh", Manifesto = "Student event quality improvements" }
                    ]
                }
            ]
        };

        context.Elections.Add(election);
        context.SaveChanges();
    }
}
