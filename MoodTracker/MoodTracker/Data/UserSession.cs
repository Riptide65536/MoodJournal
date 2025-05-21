using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Data
{
    public static class UserSession
    {
        public static string CurrentUserId { get; set; } = "0";
        // 你也可以加更多用户信息，如CurrentUserName等
    }
}
