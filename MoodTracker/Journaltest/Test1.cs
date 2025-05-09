using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoodTracker.Data;
using System.Linq;

[TestClass]
public class ApplicationDbContextTests
{
    [TestMethod]
    public void TestInsertAndRetrieveMoodRecord()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            context.Database.EnsureCreated();

            var user = new User { Id = "1", Name = "TestUser" };
            var moodRecord = new MoodRecord
            {
                UserId = "1",
                CurrentEmotion = EmotionType.Happy,
                Title = "Test Title",
                Content = "Test Content"
            };

            // Act
            context.Users.Add(user);
            context.MoodRecords.Add(moodRecord);
            context.SaveChanges();

            // Assert
            var retrievedRecord = context.MoodRecords.FirstOrDefault();
            Assert.IsNotNull(retrievedRecord);
            Assert.AreEqual("Test Title", retrievedRecord.Title);
        }
    }
}
