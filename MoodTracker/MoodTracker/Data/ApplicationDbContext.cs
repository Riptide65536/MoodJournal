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

        public DbSet<ChatRecord> ChatHistory { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MoodRecord> MoodRecords { get; set; }

        // ���ڱ����ͼƬ��������������ʱ��ȥ�������
        //public DbSet<Mood> Moods { get; set; }

        public DbSet<Tag> Tags { get; set; }
        public class ChatRecord
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();
            public string UserMessage { get; set; }
            public string AIMessage { get; set; }
            public string UserId { get; set; }  // ����û�ID�ֶ�
            public DateTime Timestamp { get; set; } = DateTime.Now;
        }

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
