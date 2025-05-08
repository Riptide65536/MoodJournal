using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Data
{
    public class UserService
    {
        // 添加用户
        public void AddUser(User user)
        {
            using var db = new ApplicationDbContext();
            db.Users.Add(user);
            db.SaveChanges();
        }

        // 删除用户（注销）
        public void DeleteUser(string userId)
        {
            using var db = new ApplicationDbContext();
            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return;
            
            // 删除用户
            db.Users.Remove(user);
            // 删除用户的所有记录
            var records = db.MoodRecords.Where(r => r.UserId == userId).ToList();
            foreach (var record in records)
            {
                db.MoodRecords.Remove(record);
            }
            db.SaveChanges();
        }

        // 更新用户信息
        public void UpdateUser(string id, User user)
        {
            using var db = new ApplicationDbContext();
            var existingUser = db.Users.FirstOrDefault(u => u.Id == id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Password = user.Password;
                existingUser.Avatar = user.Avatar;
                db.SaveChanges();
            }
        }
    }
}
