using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;


namespace WindowsFormsApplication1
{
    class Function
    {
        static string cs = @"server=localhost;userid=root;password=root;database=voice";
        static MySqlConnection conn = null;
        static MySqlDataReader mdr1=null , mdr2=null;
        static MySqlCommand cmd;
        ProcessStartInfo pi;
        DirectoryInfo di;

        List<string> list1 = new List<string>();
        List<string> list2 = new List<string>();
        List<string> list3 = new List<string>();

        static string text1;
        static string text2;
        static string methodName;
        static string command;
        static int appId = 0;

        int id = 0;
                
        public static void setText(string t)
        {
            text1 = t;
        }

        public static void setCommand(string c)
        {
            command = c;
        }

        public static void setApplication(string app)
        {
            // Open Database Connection 
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string s = "SELECT * FROM application";
                MySqlCommand cmd = new MySqlCommand(s, conn);
                mdr1 = cmd.ExecuteReader();

                while (mdr1.Read())
                {
                    string temp=mdr1.GetString(1);
                    temp=temp.Replace(" ", "");
                    Console.WriteLine(temp);
                    if (temp.Equals(app))
                    {
                        appId = mdr1.GetInt32(0);
                        setText(mdr1.GetString(1));
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
                if (mdr1 != null)
                    mdr1.Close();
                if (conn != null)
                    conn.Close();
            }
            // Close Database Connection

        }

        public static void setMethod(string f)
        {            
            // Open Database Connection 
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string s = "SELECT * FROM command";
                MySqlCommand cmd = new MySqlCommand(s, conn);
                mdr1 = cmd.ExecuteReader();

                while (mdr1.Read())
                {
                    if (mdr1.GetString(1).Equals(f))
                    {
                        methodName = mdr1.GetString(3);
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
                if (mdr1 != null)
                    mdr1.Close();
                if (conn != null)
                    conn.Close();
            }
            // Close Database Connection

            Type type = typeof(Function);
            MethodInfo method = type.GetMethod(methodName);         // using System.Reflection;
            Function c = new Function();
            method.Invoke(c, null);
            
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
            //text1 = "a";
            Console.WriteLine(text1);
            Console.WriteLine(command);
            caption();
            sendKey(command);
        //    Console.WriteLine(text1);
        //    sendKey("select all");
         //   sendKey("cut");
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
            for (int a = 0; a < list1.Count; a++)
                Console.WriteLine(" i=" + a + "  " + list1[a]);
            text1 = list1[0];
            text2 = (text1.Substring(text1.LastIndexOf("-") + 2, text1.Length - (text1.LastIndexOf("-") + 2))).ToLower();
            Console.WriteLine(text2);
            
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string s;
                s = "SELECT * FROM application";
                cmd = new MySqlCommand(s, conn);
                mdr1 = cmd.ExecuteReader();
                while (mdr1.Read())
                {
                    if (mdr1.GetString(1).Equals(text2))
                    {
                        id = mdr1.GetInt32(0);
                        Console.WriteLine("Match Found : Application Id=" + mdr1.GetInt32(0));
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
                if (mdr1 != null)
                    mdr1.Close();
                if (conn != null)
                    conn.Close();
            }
            
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
            //caption();
            string action = key;
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string s;
                s = "SELECT * FROM command";
                cmd = new MySqlCommand(s, conn);
                mdr2 = cmd.ExecuteReader();

                while (mdr2.Read())
                {
                    if (mdr2.GetInt32(2).Equals(id))
                    {
                        if (mdr2.GetString(1).Equals(key))
                        {
                            action = mdr2.GetString(3);
                            Console.WriteLine("Match Found : Command Id=" + mdr2.GetInt32(0));
                            break;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error : {0}", ex.ToString());
            }
            finally
            {
                if (mdr2 != null)
                    mdr2.Close();
                if (conn != null)
                    conn.Close();
            }
            Start(text1, action);            
        }

        public void shutdown()
        {
            Process.Start("shutdown", "/s /t 0");
        }
        
        public void restart()
        {
            Process.Start("shutdown", "/r /t 0");
        }

        // For Logoff
        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
        
        // For Hibernate , Sleep
        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        IntPtr hWnd;

        public void hibernate()
        {
            SetSuspendState(true, true, true);
        }

        public void sleep()
        {
            SetSuspendState(false, true, true);
        }

        public void logoff()
        {
            ExitWindowsEx(0, 0);
        }

        // For closing of window by window title
        static uint WM_CLOSE = 0x0010;
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lp2);
        
        public void closeWindow()
        {
            hWnd = FindWindowByCaption(IntPtr.Zero, text1);
            PostMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);            
        }

        // For Maximizing , minimizing and restore
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        // SW_SHOWMAXIMIZED to maximize the window
        // SW_SHOWMINIMIZED to minimize the window
        // SW_SHOWNORMAL to make the window be normal size

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;

        public void maximize()
        {
            hWnd = FindWindowByCaption(IntPtr.Zero, text1);
            if (!hWnd.Equals(IntPtr.Zero))
            {
                ShowWindowAsync(hWnd, SW_SHOWMAXIMIZED);
            }
        }

        public void minimize()
        {
            hWnd = FindWindowByCaption(IntPtr.Zero, text1);
            if (!hWnd.Equals(IntPtr.Zero))
            {
                ShowWindowAsync(hWnd, SW_SHOWMINIMIZED);
            }
        }

        public void restore()
        {
            hWnd = FindWindowByCaption(IntPtr.Zero, text1);
            if (!hWnd.Equals(IntPtr.Zero))
            {
                ShowWindowAsync(hWnd, SW_SHOWNORMAL);
            }
        }
    }
}
