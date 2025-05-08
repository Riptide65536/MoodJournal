using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Data
{
    public class TagService
    {
        public void AddTag(string moodRecordId, string tagName)
        {
            using var db = new ApplicationDbContext();
            var tag = db.Tags.FirstOrDefault(t => t.Name == tagName) ?? new Tag { Name = tagName };
            var moodRecord = db.MoodRecords.FirstOrDefault(mr => mr.RecordId == moodRecordId);

            if (moodRecord != null)
            {
                moodRecord.Tags.Add(tag);
                db.SaveChanges();
            }
        }

        public void RemoveTag(string moodRecordId, string tagName)
        {
            using var db = new ApplicationDbContext();
            var moodRecord = db.MoodRecords.Include(mr => mr.Tags)
                                           .FirstOrDefault(mr => mr.RecordId == moodRecordId);

            if (moodRecord != null)
            {
                var tag = moodRecord.Tags.FirstOrDefault(t => t.Name == tagName);
                if (tag != null)
                {
                    moodRecord.Tags.Remove(tag);
                    db.SaveChanges();
                }
            }
        }

        public List<Tag> GetTags(string moodRecordId)
        {
            using var db = new ApplicationDbContext();
            return db.MoodRecords.Include(mr => mr.Tags)
                                  .FirstOrDefault(mr => mr.RecordId == moodRecordId)?
                                  .Tags.ToList() ?? new List<Tag>();
        }
    }
}
