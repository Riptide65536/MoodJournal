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
        public void AddRecord(MoodRecord record)
        {
            using var db = new ApplicationDbContext();
            db.MoodRecords.Add(record);
            db.SaveChanges();
        }

        // 删除记录
        public void DeleteRecord(string recordId)
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
        public void UpdateRecord(string recordId, MoodRecord new_record)
        {
            DeleteRecord(recordId);
            AddRecord(new_record);
        }

        // 获取用户的所有记录（按照时间倒序）
        public List<MoodRecord> GetRecordsByUserId(string userId)
        {
            using var db = new ApplicationDbContext();
            return db.MoodRecords.Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Datetime)
                .ToList();
        }

        // 获取该用户的随机一条记录
        public MoodRecord? GetRandomRecordByUserId(string userId)
        {
            using var db = new ApplicationDbContext();
            var records = db.MoodRecords.Where(r => r.UserId == userId).ToList();
            if (records.Count > 0)
            {
                Random random = new Random();
                int index = random.Next(records.Count);
                return records[index];
            }
            return null;
        }

    }
}
