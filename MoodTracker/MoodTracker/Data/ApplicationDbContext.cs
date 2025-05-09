using Microsoft.EntityFrameworkCore;
using MoodTracker.Data;

namespace MoodTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<MoodRecord> MoodRecords { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseMySql("server=localhost;port=3307;database=Journaldb;user=root;password=password;",
                new MySqlServerVersion(new Version(8, 0, 41)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置User-Tag一对多关系
            modelBuilder.Entity<Tag>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tags)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade); // 级联删除[2,7](@ref)

            // 配置User-MoodRecord一对多关系
            modelBuilder.Entity<MoodRecord>()
                .HasOne(m => m.User)
                .WithMany(u => u.MoodRecords)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 配置MoodRecord-Tag多对多关系（EF Core 5+方式）
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
            // 确保数据库存在
            this.Database.EnsureCreated();

            // 应用所有挂起的迁移以自动建表
            this.Database.Migrate();
        }
    }
}
