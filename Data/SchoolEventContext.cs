using Microsoft.EntityFrameworkCore;
using SchoolEventRegistration.Models;

namespace SchoolEventRegistration.Data;

public class SchoolEventContext : DbContext
{
    public SchoolEventContext(DbContextOptions<SchoolEventContext> options) : base(options)
    {
    }

    public DbSet<Event> Events => Set<Event>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Registration> Registrations => Set<Registration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>().ToTable("Events");
        modelBuilder.Entity<Student>().ToTable("Students");
        modelBuilder.Entity<Registration>().ToTable("Registrations");

        modelBuilder.Entity<Registration>()
            .HasIndex(registration => new { registration.EventId, registration.StudentId })
            .IsUnique();

        modelBuilder.Entity<Registration>()
            .HasOne(registration => registration.Event)
            .WithMany(schoolEvent => schoolEvent.Registrations)
            .HasForeignKey(registration => registration.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Registration>()
            .HasOne(registration => registration.Student)
            .WithMany(student => student.Registrations)
            .HasForeignKey(registration => registration.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
