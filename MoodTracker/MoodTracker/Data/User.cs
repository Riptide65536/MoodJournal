using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Data
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public byte[] Avatar { get; set; } = [];

        public List<MoodRecord> MoodRecords { get; set; } = [];
        public List<Tag> Tags { get; set; } = [];
    }
}
