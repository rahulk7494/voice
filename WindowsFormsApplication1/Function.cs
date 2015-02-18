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
using System.Text;
using System.Security.Principal;
using System.Security.AccessControl;
using Microsoft.Win32;
using System.Speech.Synthesis;
using System.Collections;

namespace WindowsFormsApplication1
{
    class Function
    {
        static List<string> list1 = new List<string>();
        static List<string> list2 = new List<string>();
        static List<string> list3 = new List<string>();

        static string text1 = "";
        static string text2 = "";
        //static string methodName;
        static string command;
        static int appId = 0;
        Boolean uninst;

        static SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        
       // int id = 0;

        public static void setText(string t)
        {
            text1 = t;
        }

        public static void setCommand(string c)
        {
            command = c;
        }

        public static void callMethod(string f)
        {
            Type type = typeof(Function);
            MethodInfo method = type.GetMethod(f);         // using System.Reflection;
            Function c = new Function();
            if (method != null)
                method.Invoke(c, null);
            else
                Console.WriteLine("method not");
        }

        [DllImport("user32.dll")]
        static extern int GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(int hWnd, StringBuilder text, int count);

        public string GetActiveWindow()
        {
            const int nChars = 256;
            int handle = 0;
            StringBuilder buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();
            if (GetWindowText(handle, buff, nChars) > 0)
            {
                text1 = buff.ToString();
                Value.activeApplication = text1;
                return text1;
            }
            return null;
        }

        public void main()
        {
            uninst = false;
            Console.WriteLine("application = "+text1);
            Console.WriteLine("command = "+command);
            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = -2;     // -10...10

            if (!text1.Equals(""))
            {
                if (caption() == 1)
                    findCommand(command);
                else
                {
                    appId = 0;
                    findCommand(command);
                }
                
            }
            else
            {
                text1 = GetActiveWindow();
                appId = 0;
                findCommand(command);   
             
            }
            Console.WriteLine("Active Window : " + text1);
            if (uninst == true)
                Value.activeApplication = "";
            else
                Value.activeApplication = text1;            
        }

        /*  Write something on VOICE
         * 
            IntPtr zero = FindWindow(null, "Form1");
            SetForegroundWindow(zero);
            SendKeys.SendWait("Opening .....");
            SendKeys.Flush();

         */
         
        public static int searchFile(string text)
        {
            text = text.Remove(0, 1);               // due to application name in morphological.cs
            try
            {
                Value.conn = new MySqlConnection(Value.cs);
                Value.conn.Open();

                string s;
                s = "SELECT * FROM application";
                Value.cmd = new MySqlCommand(s, Value.conn);
                Value.mdr = Value.cmd.ExecuteReader();
                while (Value.mdr.Read())
                {
                    if ((Value.mdr.GetString(1)).ToLower().IndexOf(text.ToLower()) >= 0)
                    {
                        Process.Start(Value.mdr.GetString(2));
                        //  Console.WriteLine("Match Found : Application Id=" + mdr1.GetInt32(0));
                        return 1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (Value.mdr != null)
                    Value.mdr = null;
                if (Value.conn != null)
                    Value.conn = null;
            }

            Console.WriteLine("Not Present in database");

            string[] name={"Program Files","Program Files (x86)","Windows","Windows\\System32"};
            DirectoryInfo dirInfo;

            foreach (string s in name)
            {
                try
                {
                    dirInfo = new DirectoryInfo(@"C:\" + s);
                    //SetPermissions(@"C:\"+s);
                    var exeFiles = dirInfo.EnumerateFiles("*.exe", SearchOption.AllDirectories);
                    //var exeFiles = Directory.GetFiles(@"C:\" + s, "*.exe", SearchOption.AllDirectories);
                    foreach (var exeFile in exeFiles)
                    {
                        //Console.WriteLine(exeFile);
                        if (exeFile.ToString().ToLower().IndexOf(text.ToLower()) >= 0)
                        {
                            Console.WriteLine("Present in " + s);
                            Process.Start("" + exeFile.FullName);
                            string str = exeFile.ToString().Remove(exeFile.ToString().IndexOf(".exe"));
                            Value.activeApplication = str;
                            synthesizer.SpeakAsync(text + " opened");
                            Console.WriteLine(exeFile.FullName);
                            return 1;
                        }
                    }
                    Console.WriteLine("Not Present in " + s);
                }
                catch (System.Exception excpt)
                {
                    Console.WriteLine(excpt.Message);
                }
            }
            Console.WriteLine("----------------ALL DONE------------------");
            return 0;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void sendKey(string name, string key)
        {
            Console.WriteLine("" + name + "  " + key);
            restore();
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

        public static int caption()
        {
            int flag = 0;
            string[] tokens = GreedyTokenize(text1);
            Process[] pr = Process.GetProcesses();
            foreach (Process p in pr)
            {
                if (!(string.IsNullOrEmpty(p.MainWindowTitle)))
                {
                    list1.Add(p.MainWindowTitle);
                }
            }
            foreach (string token in tokens)
            {
                int count = list1.Count;
                //  Console.WriteLine("token =" + token + " app count =" + count);
                //  Console.WriteLine("---------------------------");
                //  Console.WriteLine("List 1 Contents");
                for (int i = 0; i <= count - 1; i++)
                {
                    //Console.Write(list1[i]+"\t");
                    tokenize(list1[i].ToLower(), token.ToLower(), i);
                }
                list1.Clear();
                list1.AddRange(list3);          // Copy contents of list3 to list1
                //  Console.WriteLine("---------------------------");
                //  Console.WriteLine("List 3 contents (copied to list 1)");
                list3.Clear();                
            }
            /*for (int a = 0; a < list1.Count; a++)
                Console.WriteLine(" i=" + a + "  " + list1[a]);*/
            try
            {
                Value.listOfApplication.Clear();
                for (int j = 0; j <= list1.Count - 1; j++)
                {
                    Value.listOfApplication.Add(list1[j]);
                    flag = 1;
                    //Console.WriteLine(list1[j]);
                }
                //Console.WriteLine(Value.listOfApplication.Count);
                if (flag == 1)
                {
                    if (Value.listOfApplication.Count == 1)
                        text1 = list1[0];
                    else
                    {
                        ListOfApplication l = new ListOfApplication();
                        l.ShowDialog();
                        text1 = Value.activeApplication;
                    }
                    //  Console.WriteLine(Value.activeApplication);
                    text2 = (text1.Substring(text1.LastIndexOf("-") + 2, text1.Length - (text1.LastIndexOf("-") + 2))).ToLower();
                    list1.Clear();
                }
            }
            catch (Exception e)
            {
                Value.status = "Application Not Found";
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("---------------------------  " + flag);
            //  Console.WriteLine("Application  Name = " + text2);        // app name extracted from windows form title
            if (flag == 0)
            {
                try
                {
                    Value.conn = new MySqlConnection(Value.cs);
                    Value.conn.Open();

                    string s;
                    s = "SELECT * FROM application";
                    Value.cmd = new MySqlCommand(s, Value.conn);
                    Value.mdr = Value.cmd.ExecuteReader();
                    while (Value.mdr.Read())
                    {
                        if ((Value.mdr.GetString(1)).ToLower().Equals(text2.ToLower()))
                        {
                            appId = Value.mdr.GetInt32(0);
                            //Console.WriteLine(appId);
                            //  Console.WriteLine("Match Found : Application Id=" + mdr1.GetInt32(0));
                            return appId;
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error : {0}", ex.ToString());
                }
                finally
                {
                    if (Value.mdr != null)
                        Value.mdr.Close();
                    if (Value.conn != null)
                        Value.conn.Close();
                }
            }
            return 0;
        }

        public static void dictate(string text)
        {
            text1 = Value.activeApplication;
            if (caption() == 1)
                sendKey(text1,text);
            else
                //  Console.WriteLine("Application not found");
                synthesizer.SpeakAsync("Application not found");
        }

        public static void tokenize(string text, string temp, int i)
        {
            string[] tokens = GreedyTokenize(text);
            list2.Clear();
            foreach (string token in tokens)
            {
                list2.Add(token);
            }
            int index;
            index = list2.IndexOf(temp);
            //  Console.WriteLine(index);
            if (index >= 0)
               list3.Add(list1[i]);
        }

        public void findCommand(string key)
        {
            string action = key;
            string s;
            int flag = 0;                   // command found or not
            
            try
            {
                Value.conn = new MySqlConnection(Value.cs);
                Value.conn.Open();
                s = "SELECT * FROM command";
                Value.cmd = new MySqlCommand(s, Value.conn);
                Value.mdr = Value.cmd.ExecuteReader();

                while (Value.mdr.Read())
                {
                    if (Value.mdr.GetInt32(2).Equals(appId))
                    {
                        if (Value.mdr.GetString(1).ToLower().Equals(key.ToLower()))
                        {
                            action = Value.mdr.GetString(3);
                            Console.WriteLine("Match Found : Command Id=" + Value.mdr.GetInt32(0));
                            if (action.StartsWith("."))
                            {
                                action = action.Remove(0, 1);
                                callMethod(action);
                                return;
                            }
                            flag = 1;
                            break;
                        }
                    }
                    else if (Value.mdr.GetInt32(2).Equals(5))
                    {
                        //Console.WriteLine(Value.mdr.GetString(3));
                        if (Value.mdr.GetString(1).ToLower().Equals(key.ToLower()))
                        {
                            Process.Start(Value.mdr.GetString(3));
                            return;
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
                if (Value.mdr != null)
                    Value.mdr.Close();
                if (Value.conn != null)
                    Value.conn.Close();
            }
            try
            {    
                if (flag == 0)
                {
                    Value.conn = new MySqlConnection(Value.cs);
                    Value.conn.Open();
                    s = "SELECT * FROM command where command_id=(select command_id from synonym where keyword='" + action + "')";
                    Value.cmd = new MySqlCommand(s, Value.conn);
                    Value.mdr = Value.cmd.ExecuteReader();

                    if (Value.mdr.Read())
                    {
                        action = Value.mdr.GetString(3);
                        flag = 1;
                        //  Console.WriteLine("Match Found : Command Id=" + mdr3.GetInt32(0));
                    }
                }

                if (flag == 0)
                {   // Synchronous          synthesizer.Speak("Command not found");
                    // Asynchronous
                    synthesizer.SpeakAsync("Command not found");
                }
                
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error : {0}", ex.ToString());
            }
            finally
            {
                if (Value.mdr != null)
                    Value.mdr.Close();
                if (Value.conn != null)
                    Value.conn.Close();
            }
            command = action;
            sendKey(text1,command);
        }

        /*public static void sendKey(string key)
        {
            //  Console.WriteLine("Active : " + text1 + "\t Text : " + key);
            sendKey(text1, key);            
        }*/

        public void uninstall()
        {
            string appName = "";
            string uString = "";
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            try
            {
                using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
                {
                    foreach (string skName in rk.GetSubKeyNames())
                    {
                        if (!skName.StartsWith("{"))
                        {
                            using (RegistryKey sk = rk.OpenSubKey(skName))
                            {
                                try
                                {
                                    appName = sk.GetValue("DisplayName").ToString();
                                    uString = sk.GetValue("UninstallString").ToString();
                                    string t1 = text1.Replace(" ", "");
                                    string t2 = appName.Replace(" ", "");
                                    if (t2.ToLower().IndexOf(t1.ToLower()) >= 0)     //row4<row3)
                                    {
                                        Console.WriteLine(appName + "  " + uString);  // + " -------------------------- Out of Date");
                                        break;
                                    }
                                }
                                catch(Exception ex)
                                {
                                }
                            }
                        }
                    }
                }
                Process.Start(uString);
                uninst = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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

        static IntPtr hWnd;

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

        public static void restore()
        {
            hWnd = FindWindowByCaption(IntPtr.Zero, text1);
            if (!hWnd.Equals(IntPtr.Zero))
            {
                ShowWindowAsync(hWnd, SW_SHOWNORMAL);
            }
        }
    }
}

/*
if(caption()==1)
                    
Console.WriteLine(text1);
//sendKey("{F4}");
sendKey("^a");
sendKey("^c");
//sendKey("{F6}");
//text1 = "voice";
// Retrieves data
IDataObject iData = Clipboard.GetDataObject();
// Is Data Text?
string path="";
if (iData.GetDataPresent(DataFormats.Text))
    path=(String)iData.GetData(DataFormats.Text);
else
    Console.WriteLine("Data not found.");
Console.WriteLine(path);
Console.WriteLine(Path.GetFileName(path));
//string name = Path.GetFileName(path);
//text1 = name;
//caption();
//sendKey("^a");
//Process.Start("explorer.exe", @""+path);
//caption();
//sendKey("^v");
//sendKey(command);*/