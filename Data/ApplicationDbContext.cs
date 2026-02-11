using Microsoft.EntityFrameworkCore;
using SchoolVotingSystem.Models;

namespace SchoolVotingSystem.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Election> Elections => Set<Election>();
    public DbSet<VotingPosition> VotingPositions => Set<VotingPosition>();
    public DbSet<CandidateOption> CandidateOptions => Set<CandidateOption>();
    public DbSet<Vote> Votes => Set<Vote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Election>()
            .HasMany(e => e.Positions)
            .WithOne(p => p.Election)
            .HasForeignKey(p => p.ElectionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VotingPosition>()
            .HasMany(vp => vp.CandidateOptions)
            .WithOne(c => c.VotingPosition)
            .HasForeignKey(c => c.VotingPositionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CandidateOption>()
            .HasMany(c => c.Votes)
            .WithOne(v => v.CandidateOption)
            .HasForeignKey(v => v.CandidateOptionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Vote>()
            .HasIndex(v => new { v.VoterId, v.VotingPositionId })
            .IsUnique();

        modelBuilder.Entity<Vote>()
            .HasOne(v => v.VotingPosition)
            .WithMany()
            .HasForeignKey(v => v.VotingPositionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
