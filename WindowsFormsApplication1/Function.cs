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
        //static int Value.appId = 0;
        Boolean uninst;

        static SpeechSynthesizer synthesizer = new SpeechSynthesizer();
      
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

        /*
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
        */
        public int setApplicationName()
        {
            Console.WriteLine("------------------ setApplicationName() --------------------");
            try
            {
                Value.conn = new MySqlConnection(Value.cs);
                Value.conn.Open();

                string s;
                s = "SELECT * FROM application where application_id=" + Value.appId;
                //Console.WriteLine(s);
                Value.cmd = new MySqlCommand(s, Value.conn);
                Value.mdr = Value.cmd.ExecuteReader();
                if (Value.mdr.Read())
                {
                    text1 = Value.mdr.GetString(1);
                    Console.WriteLine("Match Found : Application Id=" + Value.mdr.GetInt32(0));
                    return 1;
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
            return 0;
        }

        public void main()
        {
            uninst = false;
            Console.WriteLine("---------------- main() -------------------");
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
                    Value.appId = 0;
                    findCommand(command);
                }
            }
            else
            {
                text1 = Value.activeApplication;//GetActiveWindow();
                if (setApplicationName() == 1)
                {
                    Console.WriteLine("555555555    "+Value.appId + "\t" + text1);
                    if (caption() == 1)
                        findCommand(command);
                    else
                    {
                        Value.appId = 0;
                        findCommand(command);
                    }
                }
                Console.WriteLine(Value.appId + "\t" + text1);
                //Value.appId = 0;
                //findCommand(command);   
             
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
            Console.WriteLine("---------------- searchFile() -------------------");
            Console.WriteLine("Input : " + text);
            text = text.Remove(0, 1);               // due to application name in morphological.cs
            Console.WriteLine(text);
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
                        Value.appId = Value.mdr.GetInt32(0);
                        Value.activeApplication = Value.mdr.GetString(1);
                        Console.WriteLine("Match Found : Application Id   = " + Value.appId);
                        Console.WriteLine("Match Found : Application Name = " + Value.activeApplication);
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
            try
            {
                Value.conn = new MySqlConnection(Value.cs);
                Value.conn.Open();

                string s;
                s = "SELECT * FROM command WHERE application_id=5";
                Value.cmd = new MySqlCommand(s, Value.conn);
                Value.mdr = Value.cmd.ExecuteReader();
                while (Value.mdr.Read())
                {
                    Console.WriteLine(Value.mdr.GetString(1).ToLower() + "\t^" + text.ToLower());
                    if ((Value.mdr.GetString(1)).ToLower().IndexOf(text.ToLower()) >= 0)
                    {
                        Process.Start(Value.mdr.GetString(3));
                        Value.appId = Value.mdr.GetInt32(2);
                        Console.WriteLine("Match Found : Command Id=" + Value.mdr.GetInt32(0));
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
            Value.activeApplication = name;
            Console.WriteLine("---------------- sendKey() -------------------");
            Console.WriteLine("Input text1=" + Value.activeApplication + " key=" + key);
            
            restore();
            //maximize();
            
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

        public delegate bool WindowEnumCallback(int hwnd, int lparam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(WindowEnumCallback lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        public static extern void GetWindowText(int h, StringBuilder s, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(int h);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("psapi.dll")]
        private static extern uint GetModuleFileNameEx(IntPtr hWnd, IntPtr hModule, StringBuilder lpFileName, int nSize);

        private List<string> Windows = new List<string>();

        private bool AddWnd(int hwnd, int lparam)
        {
            if (IsWindowVisible(hwnd))
            {
                StringBuilder sb = new StringBuilder(255);
                GetWindowText(hwnd, sb, sb.Capacity);
                Windows.Add(sb.ToString());
            }
            return true;
        }

        public static string GetTopWindowName(string s)
        {
            IntPtr hWnd = FindWindow(null, s);
            uint lpdwProcessId;
            GetWindowThreadProcessId(hWnd, out lpdwProcessId);
            IntPtr hProcess = OpenProcess(0x0410, false, lpdwProcessId);
            StringBuilder text = new StringBuilder(1000);
            GetModuleFileNameEx(hProcess, IntPtr.Zero, text, text.Capacity);
            CloseHandle(hProcess);
            return text.ToString();
        }

        public void getActiveWindows()
        {
            list1.Clear();
            int i = 0;
            EnumWindows(new WindowEnumCallback(this.AddWnd), 0);
            foreach (string s in Windows)
            {
                if (!String.IsNullOrEmpty(s) && !s.Equals("Start") && !s.Equals("Program Manager"))
                {
                    list1.Add(s);
                    Console.WriteLine("{0} {1} {2}", ++i, s, GetTopWindowName(s));
                }
            }
        
        }

        public int caption()
        {
            Console.WriteLine("---------------- caption() -------------------");
            Console.WriteLine("Input text1=" + text1);
            int flag = 0;
            string[] tokens = GreedyTokenize(text1);
            //getActiveWindows();
            
            Process[] pr = Process.GetProcesses();
            foreach (Process p in pr)
            {
                if (!(string.IsNullOrEmpty(p.MainWindowTitle)) || p.ProcessName.Equals("explorer"))
                {
                    list1.Add(p.MainWindowTitle);
                    //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", p.ProcessName, p.Id, p.MainWindowTitle);
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
                Console.WriteLine(Value.listOfApplication.Count);
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
                    Console.WriteLine(Value.activeApplication);
        //--------------------------------------------------------------------------------------------------------------------------------        
                    if (text1.LastIndexOf("-") >= 0)
                        text2 = (text1.Substring(text1.LastIndexOf("-") + 2, text1.Length - (text1.LastIndexOf("-") + 2))).ToLower();
                    else
                        text2 = (text1.Substring(text1.LastIndexOf("-") + 1, text1.Length - (text1.LastIndexOf("-") + 1))).ToLower();
        //--------------------------------------------------------------------------------------------------------------------------------
                    list1.Clear();
                    
                }
                return 1;
            }
            catch (Exception e)
            {
                Value.status = "Application Not Found";
                Console.WriteLine(e.ToString());
            }
           
            /*if (flag == 0)
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
                        //Console.WriteLine("{0} => {1}", (Value.mdr.GetString(1)).ToLower(), text2.ToLower());
                        if ((Value.mdr.GetString(1)).ToLower().Equals(text2.ToLower()))
                        {
                            Value.appId = Value.mdr.GetInt32(0);
                            //Console.WriteLine(Value.appId);
                            Console.WriteLine("Match Found : Application Id=" + Value.mdr.GetInt32(0));
                            return Value.appId;
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error : {0}", ex.ToString());
                }*/
                finally
                {
                    if (Value.mdr != null)
                        Value.mdr.Close();
                    if (Value.conn != null)
                        Value.conn.Close();
                }
                //Console.WriteLine("---------------------------  " + flag);
                //Console.WriteLine("Application  Name = " + text1);        // app name extracted from windows form title
     
            return 0;
        }

   /*     public static void dictate(string text)
        {
            text1 = Value.activeApplication;
            if (caption() == 1)
                sendKey(text1,text);
            else
                //  Console.WriteLine("Application not found");
                synthesizer.SpeakAsync("Application not found");
        }
        */
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
            Console.WriteLine("---------------- findCommand() -------------------");
            Console.WriteLine("Input text1=" + text1 + " key=" + key + " AppId=" + Value.appId);
            string action = key;
            string s;
            int flag = 0;                   // command found or not
            //Console.WriteLine(text1);
            try
            {
                Value.conn = new MySqlConnection(Value.cs);
                Value.conn.Open();
                s = "SELECT * FROM command where application_id=" + Value.appId + " OR application_id=5";
                Value.cmd = new MySqlCommand(s, Value.conn);
                Value.mdr = Value.cmd.ExecuteReader();
                //Console.WriteLine(action);
                if(!action.Equals(" "))
                    action = action.Remove(0, 1);
                
                while (Value.mdr.Read())
                {
                    //Console.WriteLine(Value.mdr.GetInt32(2) + " ^" + Value.mdr.GetString(1).ToLower() + "^ " + Value.appId + " ^" + action + "^");
                    if ((Value.mdr.GetString(1).ToLower().IndexOf(action.ToLower()) >= 0) && !(action.Equals("")))
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
                    if (Value.mdr.GetInt32(2).Equals(5))
                    {
                        //Console.WriteLine(Value.mdr.GetString(3));
                        if (Value.mdr.GetString(1).ToLower().Equals(action.ToLower()))
                        {
                            Process.Start(Value.mdr.GetString(3));
                            return;
                        }
                        else if ((" "+Value.mdr.GetString(1).ToLower()).Equals(text1.ToLower()))
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

        public void sendCommand(string key)
        {
            Console.WriteLine("---------------- sendCommand() -------------------");
            Console.WriteLine("Input text1=" + Value.activeApplication + " key=" + key);
            text1 = Value.activeApplication;
            caption();
            sendKey(text1, key);
            
        /*    if(!text1.Equals(""))
            {
                    sendKey(text1, key);                
            }
            else
            {
                if (setApplicationName() == 1)
                {
                    Console.WriteLine("888888888    " + Value.appId + "\t" + text1);
                    if (caption() == 1)
                        sendKey(text1, key);                
                }
                Console.WriteLine(Value.appId + "\t" + text1);            
            }*/
        }

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

        public void hidden()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced");
            if (key != null)
            {
                if (key.GetValue("Hidden").ToString() == "1")
                    key.SetValue("Hidden", 2);                  // Hide
                else
                    key.SetValue("Hidden", 1);                  // Show
            }

            Guid CLSID_ShellApplication = new Guid("13709620-C279-11CE-A49E-444553540000");
            Type shellApplicationType = Type.GetTypeFromCLSID(CLSID_ShellApplication, true);
            object shellApplication = Activator.CreateInstance(shellApplicationType);
            object windows = shellApplicationType.InvokeMember("Windows", System.Reflection.BindingFlags.InvokeMethod, null, shellApplication, new object[] { });
            Type windowsType = windows.GetType();
            object count = windowsType.InvokeMember("Count", System.Reflection.BindingFlags.GetProperty, null, windows, null);
            for (int i = 0; i < (int)count; i++)
            {
                object item = windowsType.InvokeMember("Item", System.Reflection.BindingFlags.InvokeMethod, null, windows, new object[] { i });
                Type itemType = item.GetType();

                string itemName = (string)itemType.InvokeMember("Name", System.Reflection.BindingFlags.GetProperty, null, item, null);
                if (itemName == "Windows Explorer")
                {
                    itemType.InvokeMember("Refresh", System.Reflection.BindingFlags.InvokeMethod, null, item, null);
                }
            }
        }

        //[DllImport("user32.dll")]
        //private static extern IntPtr GetForegroundWindow();

        //[DllImport("user32.dll")]
        //static extern int GetWindowTextLength(IntPtr hWnd);

        //[DllImport("user32.dll")]
        //private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /*
        public static string GetTopWindowText()
        {
            IntPtr hWnd = GetForegroundWindow();
            int length = GetWindowTextLength(hWnd);
            StringBuilder text = new StringBuilder(length + 1);
            GetWindowText(hWnd, text, text.Capacity);
            return text.ToString();
        }
        */

        enum RecycleFlag : int
        {
            SHERB_NOCONFIRMATION = 0x00000001, // No confirmation, when emptying
            SHERB_NOPROGRESSUI = 0x00000001, // No progress tracking window during the emptying of the recycle bin
            SHERB_NOSOUND = 0x00000004 // No sound when the emptying of the recycle bin is complete
        }

        [DllImport("Shell32.dll")]
        static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlag dwFlags);
        
        public void empty()
        {   
            //text1 = text1.Remove(0, 1);
            Console.WriteLine("text1 = " + text1);
            if (text1.IndexOf("temp") >= 0)
            {
                string s = Path.GetTempPath();
                Console.WriteLine(Path.GetTempFileName());
                Process.Start(s);
                Thread.Sleep(500);
                DirectoryInfo dir = new DirectoryInfo(@""+s);
                try
                {
                    foreach (FileInfo files in dir.GetFiles())
                        files.Delete();
                    foreach (DirectoryInfo dirs in dir.GetDirectories())
                        dirs.Delete(true);
                }
                catch (Exception e)
                { }
            }
            int i = caption();
            Console.WriteLine(i);
            if (text1.IndexOf("recycle bin") >= 0)
            {
                SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlag.SHERB_NOSOUND);
            }
            else
            {
                foreach (string s in list1)
                {
                    if (GetTopWindowName(s).IndexOf("explorer") >= 0)
                    {
                        Console.WriteLine("Hii");
                        sendKey(text1, "^a");
                        sendKey(text1, "{DEL}");
                        break;
                    }
                }
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

        public static void maximize()
        {
            hWnd = FindWindowByCaption(IntPtr.Zero, text1);
            if (!hWnd.Equals(IntPtr.Zero))
            {
                ShowWindowAsync(hWnd, SW_SHOWMAXIMIZED);
            }
        }

        public static void minimize()
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