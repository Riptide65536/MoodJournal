using Microsoft.EntityFrameworkCore;
using MoodTracker.Data;

namespace MoodTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;port=3307;database=JournalDatabase;user=root;password=password;");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<MoodRecord> MoodRecords { get; set; }
        public DbSet<Mood> Moods { get; set; }
    }
}
