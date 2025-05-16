using MoodTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 注：测试启动时为数据库添加初始化数据
// 通常只需要启动1次

[TestClass]
public class TestAccount
{
    [TestMethod]
    public void TestInsertAndRetrieveMoodRecord()
    {
        JournalService journalService = new JournalService();
        UserService userService = new UserService();

        // 添加一个id为0的用户
        User testUser = new User {Id = "0", Name = "Test", Password = "123456"};
        userService.UpdateUser("0", testUser);

        // 添加40项对应的记录
        for (int i = 0; i < 40; i++)
        {
            MoodRecord moodRecord = new MoodRecord
            {
                Datetime = DateTime.Now.AddDays(i-40),
                UserId = testUser.Id,
                User = testUser,
                CurrentEmotion = EmotionType.Happy,
                Title = "Test Title" + i,
                Content = "Test Content" + i
            };
            journalService.AddRecordToExistingUser("0", moodRecord);
        }
    }
}