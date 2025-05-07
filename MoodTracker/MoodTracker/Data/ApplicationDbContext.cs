using Microsoft.EntityFrameworkCore;
using MoodTracker.Models;

namespace MoodTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<MoodRecord> MoodRecords { get; set; }
        public DbSet<Mood> Moods { get; set; }
    }
}
