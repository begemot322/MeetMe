using MeetMe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<DateRange> DateRanges { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}