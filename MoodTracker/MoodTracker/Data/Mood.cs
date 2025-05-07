using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Models
{
    public class Mood
    {
        public string MoodId { get; set; }
        public string MoodName { get; set; }

        public string UserId { get; set; }
    }
}
