using SchoolEventRegistration.Models;

namespace SchoolEventRegistration.Data;

public static class DbInitializer
{
    public static void Seed(SchoolEventContext context)
    {
        if (!context.Events.Any())
        {
            context.Events.AddRange(
                new Event
                {
                    Title = "Science Fair",
                    Description = "Students present science projects and experiments to classmates, teachers, and families.",
                    Date = DateTime.Today.AddDays(14).AddHours(15),
                    Location = "Main Hall",
                    MaxParticipants = 50
                },
                new Event
                {
                    Title = "Basketball Tournament",
                    Description = "Friendly after-school basketball games between class teams.",
                    Date = DateTime.Today.AddDays(21).AddHours(16),
                    Location = "Gymnasium",
                    MaxParticipants = 24
                },
                new Event
                {
                    Title = "Art Workshop",
                    Description = "A hands-on workshop for students who want to learn drawing and painting techniques.",
                    Date = DateTime.Today.AddDays(10).AddHours(13),
                    Location = "Art Room 2",
                    MaxParticipants = 20
                });
        }

        if (!context.Students.Any())
        {
            context.Students.AddRange(
                new Student { Name = "Ava Johnson", Email = "ava.johnson@student.school", ClassName = "10A" },
                new Student { Name = "Noah Smith", Email = "noah.smith@student.school", ClassName = "10B" },
                new Student { Name = "Mia Chen", Email = "mia.chen@student.school", ClassName = "11A" });
        }

        context.SaveChanges();
    }
}
