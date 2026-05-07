using Microsoft.EntityFrameworkCore;
using SchoolEventRegistration.Models;

namespace SchoolEventRegistration.Data;

public class SchoolEventContext : DbContext
{
    public SchoolEventContext(DbContextOptions<SchoolEventContext> options) : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Registration> Registrations { get; set; }
}
