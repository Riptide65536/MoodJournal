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
            // 根据实际情况选择数据库连接字符串
            optionsBuilder
                .UseMySql("server=localhost;port=3306;database=mooddb;user=root;password=2.71828182;",
                //.UseMySql("server=localhost;port=3306;database=mooddb;user=root;password=mrwuabc0750*;",
                //.UseMySql("server=localhost;port=3307;database=mooddb;user=root;password=password;",
                new MySqlServerVersion(new Version(8, 0, 41)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User-Tag关系
            modelBuilder.Entity<Tag>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tags)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade); // ����ɾ��[2,7](@ref)

            // User-MoodRecord关系
            modelBuilder.Entity<MoodRecord>()
                .HasOne(m => m.User)
                .WithMany(u => u.MoodRecords)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // MoodRecord-Tag关系
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
            // 确保已经拥有
            this.Database.EnsureCreated();

            // 确保已经迁移
            this.Database.Migrate();
        }
    }
}
