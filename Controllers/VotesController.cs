using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolVotingSystem.Data;
using SchoolVotingSystem.Models;
using SchoolVotingSystem.ViewModels;

namespace SchoolVotingSystem.Controllers;

public class VotesController(ApplicationDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Cast()
    {
        var election = await context.Elections
            .AsNoTracking()
            .Include(e => e.Positions)
            .ThenInclude(p => p.CandidateOptions)
            .Where(e => e.IsActive)
            .OrderByDescending(e => e.Id)
            .FirstOrDefaultAsync();

        if (election is null)
        {
            return NotFound("No active election was found.");
        }

        var vm = new VoteInputViewModel
        {
            ElectionId = election.Id,
            ElectionName = election.Name,
            Positions = election.Positions
                .OrderBy(p => p.Name)
                .Select(p => new VotePositionInputViewModel
                {
                    VotingPositionId = p.Id,
                    PositionName = p.Name,
                    CandidateOptions = p.CandidateOptions
                        .OrderBy(c => c.CandidateName)
                        .Select(c => new SelectListItem
                        {
                            Value = c.Id.ToString(),
                            Text = $"{c.CandidateName} - {c.Manifesto}"
                        })
                        .ToList()
                })
                .ToList()
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cast(VoteInputViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await RehydratePositionOptions(model);
            return View(model);
        }

        var selectedPositions = model.Positions.Where(p => p.SelectedCandidateOptionId.HasValue).ToList();
        if (selectedPositions.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "Choose at least one position to vote for.");
            await RehydratePositionOptions(model);
            return View(model);
        }

        foreach (var position in selectedPositions)
        {
            var hasVoted = await context.Votes.AnyAsync(v =>
                v.VoterId == model.VoterId && v.VotingPositionId == position.VotingPositionId);

            if (hasVoted)
            {
                ModelState.AddModelError(string.Empty, $"You have already voted for {position.PositionName}.");
                await RehydratePositionOptions(model);
                return View(model);
            }
        }

        var votesToSave = selectedPositions.Select(position => new Vote
        {
            VoterId = model.VoterId.Trim(),
            VotingPositionId = position.VotingPositionId,
            CandidateOptionId = position.SelectedCandidateOptionId!.Value,
            VotedAtUtc = DateTime.UtcNow
        });

        await context.Votes.AddRangeAsync(votesToSave);
        await context.SaveChangesAsync();

        TempData["Success"] = "Your vote has been submitted successfully.";
        return RedirectToAction(nameof(Results));
    }

    [HttpGet]
    public async Task<IActionResult> Results()
    {
        var activeElection = await context.Elections
            .AsNoTracking()
            .Include(e => e.Positions)
            .ThenInclude(p => p.CandidateOptions)
            .Where(e => e.IsActive)
            .OrderByDescending(e => e.Id)
            .FirstOrDefaultAsync();

        if (activeElection is null)
        {
            return NotFound("No active election was found.");
        }

        var voteLookup = await context.Votes
            .AsNoTracking()
            .GroupBy(v => v.CandidateOptionId)
            .Select(g => new { CandidateOptionId = g.Key, VoteCount = g.Count() })
            .ToDictionaryAsync(x => x.CandidateOptionId, x => x.VoteCount);

        var results = activeElection.Positions
            .OrderBy(p => p.Name)
            .Select(position =>
            {
                var candidateResults = position.CandidateOptions
                    .OrderBy(c => c.CandidateName)
                    .Select(candidate => new CandidateResultViewModel
                    {
                        CandidateName = candidate.CandidateName,
                        Manifesto = candidate.Manifesto,
                        VoteCount = voteLookup.TryGetValue(candidate.Id, out var count) ? count : 0
                    })
                    .ToList();

                return new PositionResultViewModel
                {
                    PositionName = position.Name,
                    Candidates = candidateResults,
                    TotalVotes = candidateResults.Sum(c => c.VoteCount)
                };
            })
            .ToList();

        ViewBag.ElectionName = activeElection.Name;

        return View(results);
    }

    private async Task RehydratePositionOptions(VoteInputViewModel model)
    {
        var positions = await context.VotingPositions
            .AsNoTracking()
            .Include(p => p.CandidateOptions)
            .Where(p => model.Positions.Select(mp => mp.VotingPositionId).Contains(p.Id))
            .ToListAsync();

        model.Positions = model.Positions.Select(existing =>
        {
            var dbPosition = positions.FirstOrDefault(p => p.Id == existing.VotingPositionId);
            existing.CandidateOptions = dbPosition?.CandidateOptions
                .OrderBy(c => c.CandidateName)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.CandidateName} - {c.Manifesto}"
                })
                .ToList() ?? [];

            existing.PositionName = dbPosition?.Name ?? existing.PositionName;
            return existing;
        }).ToList();
    }
}
