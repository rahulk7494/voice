using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace WindowsFormsApplication1
{
    class Function
    {
        static string cs = @"server=localhost;userid=root;password=root;database=voice";
        static MySqlConnection conn = null;
        MySqlDataReader mdr=null;
        ProcessStartInfo pi;
        DirectoryInfo di;

        List<string> list1 = new List<string>();
        List<string> list2 = new List<string>();
        List<string> list3 = new List<string>();

        static string text1;

        public static void setText(string t)
        {
            text1 = t;
        }

        public void main()
        {
            Process[] pr = Process.GetProcesses();
            foreach (Process p in pr)
            {
                if (!(string.IsNullOrEmpty(p.MainWindowTitle)))
                {
                    list1.Add(p.MainWindowTitle);
                }
            }
            text1 = "a";
            caption();
            Console.WriteLine(text1);
            sendKey("select all");
        }

        public void openFile()
        {

        }
        
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public void Start(string name, string key)
        {
            Console.WriteLine("" + name + "  " + key);
            IntPtr zero = IntPtr.Zero;
            for (int i = 0; (i < 60) && (zero == IntPtr.Zero); i++)
            {
                Thread.Sleep(500);
                zero = FindWindow(null, name);
            }
            if (zero != IntPtr.Zero)
            {
                SetForegroundWindow(zero);
                SendKeys.SendWait(key);
                SendKeys.Flush();
            }
        }

        public static string[] GreedyTokenize(string text)
        {
            char[] delimiters = new char[] {

                  '{', '}', '(', ')', '[', ']', '>', '<','-', '_', '=', '+',
                  '|', '\\', ':', ';', ' ', '\'', ',', '.', '/', '?', '~', '!',
                  '@', '#', '$', '%', '^', '&', '*', ' ', '\r', '\n', '\t'
                  
                  };

            return text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        public void caption()
        {
            string[] tokens = GreedyTokenize(text1);
            foreach (string token in tokens)
            {
                int count = list1.Count;
                Console.WriteLine("*****" + token + " ****" + count);
                for (int i = 0; i <= count - 1; i++)
                {
                    Console.WriteLine(list1[i]);
                    tokenize(list1[i].ToLower(), token.ToLower(), i);
                }
                list1.Clear();
                list1.AddRange(list3);
                for (int j = 0; j <= list1.Count - 1; j++)
                    Console.WriteLine(list1[j]);
                Console.WriteLine("*****" + token + " ****" + list1.Count);
                list3.Clear();

            }
            text1 = list1[0];
        }
    
        public void tokenize(string text, string temp, int i)
        {
            string[] tokens = GreedyTokenize(text);
            list2.Clear();
            foreach (string token in tokens)
            {
                list2.Add(token);
            }
            int index;
            index = list2.IndexOf(temp);
            Console.WriteLine("" + index);
            if (index < 0)
            {
            }
            else
            {
                list3.Add(list1[i]);
                for (int j = 0; j < list3.Count; j++)
                    Console.WriteLine(list3[j]);
            }
        }

        

        public void sendKey(string key)
        {
            caption();
            string action = key;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string s = "SELECT * FROM command";
                MySqlCommand cmd = new MySqlCommand(s, conn);
                mdr = cmd.ExecuteReader();

                while (mdr.Read())
                {
                    if (mdr.GetString(1).Equals(key))
                    {
                        action = mdr.GetString(3);
                        break;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error : {0}", ex.ToString());
            }
            finally
            {
                if (mdr != null)
                    mdr.Close();
                if (conn != null)
                    conn.Close();
            }
            Start(text1, action);            
        }
    }
}
