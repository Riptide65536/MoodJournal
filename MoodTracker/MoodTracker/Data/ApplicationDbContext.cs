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

        // ���ڱ����ͼƬ��������������ʱ��ȥ�������
        //public DbSet<Mood> Moods { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ���ö�Զ��ϵ
            modelBuilder.Entity<MoodRecord>()
                .HasMany(mr => mr.Tags)
                .WithMany(t => t.MoodRecords)
                .UsingEntity(j => j.ToTable("MoodRecordTags"));
        }
    }
}
