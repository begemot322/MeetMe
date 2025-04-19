using MeetMe.Application.Common.Interfaces.Identity;
using MeetMe.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MeetMe.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<DbInitializer>();

        await initialiser.InitialiseAsync();

    }
}
public class DbInitializer
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public DbInitializer(ApplicationDbContext db, IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if ((await _db.Database.GetPendingMigrationsAsync()).Any())
            {
                await _db.Database.MigrateAsync();
            }
            
            var petyaUser = await _db.Users.FirstOrDefaultAsync(u => u.UserName == "petya");
            if (petyaUser == null)
            {
                petyaUser = new User
                {
                    UserName = "petya",
                    Email = "petya@example.com",
                    PasswordHash = _passwordHasher.Generate("petya123")
                };
                await _db.Users.AddAsync(petyaUser);
                await _db.SaveChangesAsync();
            }

            if (!_db.Events.Any(e => e.Title == "Сходить куда-нибудь"))
            {
                var testEvent = new Event
                {
                    Title = "Сходить куда-нибудь",
                    CreatorNickname = "Петя",
                    IsDateRange = true,
                    CreatedAt = DateTime.UtcNow,
                    Participants = new List<Participant>
                    {
                        new Participant
                        {
                            Nickname = "Петя",
                            IsCreator = true,
                            UserId = petyaUser.Id,
                            DateRanges = new List<DateRange>
                            {
                                new DateRange { StartDate = new DateTime(2023, 4, 10, 18, 0, 0), EndDate = new DateTime(2023, 4, 10, 22, 0, 0) },
                                new DateRange { StartDate = new DateTime(2023, 4, 11, 18, 0, 0), EndDate = new DateTime(2023, 4, 11, 22, 0, 0) },
                                new DateRange { StartDate = new DateTime(2023, 4, 17, 18, 0, 0), EndDate = new DateTime(2023, 4, 17, 22, 0, 0) },
                                new DateRange { StartDate = new DateTime(2023, 4, 18, 18, 0, 0), EndDate = new DateTime(2023, 4, 18, 22, 0, 0) }
                            }
                        },
                        new Participant
                        {
                            Nickname = "Вася",
                            DateRanges = new List<DateRange>
                            {
                                new DateRange { StartDate = new DateTime(2023, 4, 9, 20, 0, 0), EndDate = new DateTime(2023, 4, 9, 23, 0, 0) },
                                new DateRange { StartDate = new DateTime(2023, 4, 10, 19, 0, 0), EndDate = new DateTime(2023, 4, 10, 22, 0, 0) },
                                new DateRange { StartDate = new DateTime(2023, 4, 16, 21, 0, 0), EndDate = new DateTime(2023, 4, 16, 23, 0, 0) }
                            }
                        },
                        new Participant
                        {
                            Nickname = "Серега",
                            DateRanges = new List<DateRange>
                            {
                                new DateRange { StartDate = new DateTime(2023, 4, 8, 21, 0, 0), EndDate = new DateTime(2023, 4, 8, 23, 0, 0) },
                                new DateRange { StartDate = new DateTime(2023, 4, 9, 19, 0, 0), EndDate = new DateTime(2023, 4, 9, 22, 0, 0) },
                                new DateRange { StartDate = new DateTime(2023, 4, 10, 21, 0, 0), EndDate = new DateTime(2023, 4, 10, 23, 0, 0) },
                                new DateRange { StartDate = new DateTime(2023, 4, 11, 19, 0, 0), EndDate = new DateTime(2023, 4, 11, 23, 0, 0) }
                            }
                        }
                    }
                };

                await _db.Events.AddAsync(testEvent);
                await _db.SaveChangesAsync();
                
                await _db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при инициализации БД: {ex.Message}");
            throw;
        }
    }
}