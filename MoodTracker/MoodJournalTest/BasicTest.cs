using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoodTracker.Data;
using System.Linq;

// 注：用于测试dbContext是否拥有数据库的能力

[TestClass]
public class ApplicationDbContextTests
{
    [TestMethod]
    public void TestInsertAndRetrieveMoodRecord()
    {
        // Arrange
        /*
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;*/

        using var context = new ApplicationDbContext();
        context.Database.EnsureCreated();

        // 创建用户测试
        string UserId = Guid.NewGuid().ToString();
        var user = new User { Id = UserId, Name = "TestName" + DateTime.Now.ToString() };

        context.Users.Add(user);
        context.SaveChanges();

        // 创建心情记录到已知用户的测试
        var moodRecord = new MoodRecord
        {
            UserId = UserId,
            User = user,
            CurrentEmotion = EmotionType.Happy,
            Title = "Test Title",
            Content = "Test Content" + DateTime.Now.ToString()
        };

        context.MoodRecords.Add(moodRecord);
        context.SaveChanges();

        // 测试信息
        var retrievedRecord = context.MoodRecords.FirstOrDefault();
        Assert.IsNotNull(retrievedRecord);
        Assert.AreEqual("Test Title", retrievedRecord.Title);
    }
}
