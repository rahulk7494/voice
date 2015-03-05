using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public static class Value
    {
        public static string activeApplication = "";
        public static int appId = 0;
        public static string status = "";
        public static List<string> listOfApplication = new List<string>();
        public static string mode = "normal";
        public static string cs = @"server=localhost;userid=root;password=root;database=voice";
        public static MySqlConnection conn = null;
        public static MySqlDataReader mdr = null;
        public static MySqlCommand cmd;
        public static bool rfound = false;
        public static List<string> folderContents = new List<string>();
        public static bool folder = false;
        
    }
}
