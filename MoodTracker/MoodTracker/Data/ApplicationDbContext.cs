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

        // 由于表情的图片数量有限所以暂时不去建立表格
        //public DbSet<Mood> Moods { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置多对多关系
            modelBuilder.Entity<MoodRecord>()
                .HasMany(mr => mr.Tags)
                .WithMany(t => t.MoodRecords)
                .UsingEntity(j => j.ToTable("MoodRecordTags"));
        }
    }
}
