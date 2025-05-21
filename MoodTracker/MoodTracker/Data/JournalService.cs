using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Data
{
    public class JournalService
    {
        // 添加记录
        public void AddMoodRecord(MoodRecord record)
        {
            using var db = new ApplicationDbContext();
            db.MoodRecords.Add(record);
            db.SaveChanges();
        }

        public void AddRecordToExistingUser(string userId, MoodRecord newRecord)
        {
            using var db = new ApplicationDbContext();

            // 检查用户是否存在
            var existingUser = db.Users.FirstOrDefault(u => u.Id == userId);
            if (existingUser == null)
            {
                throw new Exception($"用户ID为 {userId} 的用户不存在。");
            }

            // 设置记录的外键和用户对象
            newRecord.UserId = existingUser.Id;
            newRecord.User = existingUser;

            // 添加记录
            db.MoodRecords.Add(newRecord);
            db.SaveChanges();
        }

        // 删除记录
        public void DeleteMoodRecord(string recordId)
        {
            using var db = new ApplicationDbContext();
            var record = db.MoodRecords.FirstOrDefault(r => r.RecordId == recordId);
            if (record != null)
            {
                db.MoodRecords.Remove(record);
                db.SaveChanges();
            }
        }

        // 更新记录
        public void UpdateMoodRecord(string recordId, MoodRecord new_record)
        {
            using var db = new ApplicationDbContext();

            var existing = db.MoodRecords
                .Include(r => r.Tags)
                .FirstOrDefault(r => r.RecordId == recordId);
            if (existing != null)
            {
                // 更新属性
                existing.Datetime = new_record.Datetime;
                existing.CurrentEmotion = new_record.CurrentEmotion;
                existing.SongName = new_record.SongName;
                existing.SongLink = new_record.SongLink;
                existing.Title = new_record.Title;
                existing.Content = new_record.Content;

                // 处理标签
                existing.Tags.Clear();
                foreach (var tag in new_record.Tags)
                {
                    var dbTag = db.Tags.FirstOrDefault(t => t.TagId == tag.TagId) ?? tag;
                    existing.Tags.Add(dbTag);
                }

                db.SaveChanges();
            }
        }

        // 获取用户的所有记录（按照时间倒序）
        public List<MoodRecord> GetRecordsByUserId(string userId)
        {
            using var db = new ApplicationDbContext();
            // db.EnsureDatabaseCreatedAndMigrated();

            return db.MoodRecords
                .Include(r => r.Tags)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Datetime)
                .ToList();
        }

        // 获取该用户的随机一条记录
        public MoodRecord? GetRandomRecordByUserId(string userId)
        {
            using var db = new ApplicationDbContext();
            var records = db.MoodRecords
                .Include(r => r.Tags)
                .Where(r => r.UserId == userId)
                .ToList();
            if (records.Count > 0)
            {
                Random random = new Random(DateTime.Now.Millisecond);
                int index = random.Next(records.Count);
                return records[index];
            }
            return null;
        }

        // 按照字符串获取当前用户的记录（还是按照时间倒序）
        public List<MoodRecord> GetRecordsByStringOfUserId(string userId, string content)
        {
            using var db = new ApplicationDbContext();
            // 包含标签信息，便于后续查询
            return db.MoodRecords
                .Include(r => r.Tags)
                .Where(r => r.UserId == userId)
                .Where(r =>
                    r.Content.Contains(content) ||
                    r.Title.Contains(content) ||
                    r.SongName.Contains(content) ||
                    r.Tags.Any(t => t.Name.Contains(content) || t.TagId.Contains(content))
                )
                .OrderByDescending(r => r.Datetime)
                .ToList();
        }

        // 按照字符串获取当前用户的记录时间
        public List<DateTime> GetAllRecordDates(string userId)
        {
            using var db = new ApplicationDbContext();
            return db.MoodRecords
                .Where(r => r.UserId == userId)
                .Select(r => r.Datetime.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToList();
        }
    }
}
