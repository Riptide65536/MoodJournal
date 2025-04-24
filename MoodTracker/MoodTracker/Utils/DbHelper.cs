using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
namespace MoodTracker.Utils
{
    public static class DbHelper
    {
        private static readonly string connstr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
        //获取数据库连接
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connstr);
        }
    }
}
