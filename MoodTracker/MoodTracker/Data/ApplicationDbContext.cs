using Microsoft.EntityFrameworkCore;
using MoodTracker.Data;

namespace MoodTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ChatRecord> ChatHistory { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MoodRecord> MoodRecords { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public class ChatRecord
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();
            public string UserMessage { get; set; }
            public string AIMessage { get; set; }
            public string UserId { get; set; }  // �����û�ID�ֶ�
            public DateTime Timestamp { get; set; } = DateTime.Now;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseMySql("server=localhost;port=3307;database=Journaldb;user=root;password=password;",
                new MySqlServerVersion(new Version(8, 0, 41)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ����User-Tagһ�Զ��ϵ
            modelBuilder.Entity<Tag>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tags)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade); // ����ɾ��[2,7](@ref)

            // ����User-MoodRecordһ�Զ��ϵ
            modelBuilder.Entity<MoodRecord>()
                .HasOne(m => m.User)
                .WithMany(u => u.MoodRecords)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ����MoodRecord-Tag��Զ��ϵ��EF Core 5+��ʽ��
            modelBuilder.Entity<MoodRecord>()
                .HasMany(m => m.Tags)
                .WithMany(t => t.MoodRecords)
                .UsingEntity<Dictionary<string, object>>("MoodRecordTags",
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    j => j.HasOne<MoodRecord>().WithMany().HasForeignKey("MoodRecordId"),
                    j => j.ToTable("MoodRecordTags").HasKey("MoodRecordId", "TagId"));
        }

        public void EnsureDatabaseCreatedAndMigrated()
        {
            // ȷ�����ݿ����
            this.Database.EnsureCreated();

            // Ӧ�����й����Ǩ�����Զ�����
            this.Database.Migrate();
        }
    }
}
