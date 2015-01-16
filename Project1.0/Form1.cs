using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net.Mail;
using System.Net;
using Shell32;                      //Added Reference - Microsoft shell controls and automation
using IWshRuntimeLibrary;
using System.Threading;           //Added Reference - Windows script host object model
using MySql.Data.MySqlClient;
using System.Reflection;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        //static int stat = 0;

        List<string> list1 = new List<string>();
        List<string> list2 = new List<string>();
        List<string> list3 = new List<string>();

        string text1;

        ProcessStartInfo pi;
        DirectoryInfo di;

        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        public Form1()
        {
            InitializeComponent();
        //    list1.Add("rahul");
         //   string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
         //   textBox1.Text = path;
            Process[] pr = Process.GetProcesses();
            foreach (Process p in pr)
            {
                if (!(string.IsNullOrEmpty(p.MainWindowTitle)))
                {
                    listBox1.Items.Add(p.MainWindowTitle);
                    list1.Add(p.MainWindowTitle.ToString());
                }
            }
            /*int count = listBox2.Items.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (listBox2.Items[i].ToString().Length == 0)
                {
                    listBox2.Items.RemoveAt(i);
                }
            } */
        }

        static string cs = @"server=localhost;userid=root;password=root;database=voice";
        static MySqlConnection conn = null;

        public void executeCommand(String command,int status)
        {
            if (status == 0)
            {
                ProcessStartInfo ij1 = new ProcessStartInfo("cmd.exe", "/c" + command); //xcopy E:\a H:\a\ /s /e /h /-Y
                Process.Start(ij1);                                                     //   e - empty folder
            }
            else
            {
                if (textBox1.Text.Equals("all"))
                {
                    command.Replace('-', ' ');
                    ProcessStartInfo ij1 = new ProcessStartInfo("cmd.exe", "/c" + command); //xcopy E:\a H:\a\ /s /e /h /-Y
                    Process.Start(ij1);                                                     //   e - empty folder
                }
                else if (textBox1.Text.Equals("no"))
                {
                    ProcessStartInfo ij2 = new ProcessStartInfo("cmd.exe", "/c TASKKILL /IM cmd.exe");   //xcopy E:\a H:\a\ /s /e /h /-Y
                    Process.Start(ij2);                                                     //   e - empty folder
                }
                else
                {
                    ProcessStartInfo ij2 = new ProcessStartInfo("cmd.exe", "/c" + command);
                    Process.Start(ij2);                                                     //   e - empty folder
                }

            }//   s - copy subfolder
                                                                                               //   h - hidden
                                                                                   //   Y - Prompt user to overwrite     
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName;
            createDir(textBox2.Text);
            string source = @""+textBox1.Text;
            string dest = @""+textBox2.Text;
            if (System.IO.Directory.Exists(source))
            {
                string[] files = System.IO.Directory.GetFiles(source);
                foreach (string s in files)
                {
                    fileName = System.IO.Path.GetFileName(s);
                    string df = System.IO.Path.Combine(dest, fileName);
                    System.IO.File.Copy(s, df, true);
                }
            }
            //executeCommand(text,stat++);           
        }

        public void createDir(string path)
        {
            di = Directory.CreateDirectory(path);
        }

        public void deleteDir(string path)
        {
            if (System.IO.Directory.Exists(path))
            {
                // Delete a directory
                if (textBox1.Text.Equals(""))
                    System.IO.Directory.Delete(path, true);

                // Delete files
                else
                {
                    string[] files = Directory.GetFiles(path);
                    string msg=Directory.GetCurrentDirectory();
                    MessageBox.Show(msg);
                    foreach (string f in files)
                    {
                        string file = System.IO.Path.Combine(textBox2.Text, textBox1.Text);
                        if (f.Equals(file))
                            System.IO.File.Delete(f);
                        else
                            MessageBox.Show("File Not Found");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path=textBox2.Text;
            di = Directory.CreateDirectory(path);
        }
  
        /*int status1 = 0,status2=0;
            String receiver="";
            
            String[] filePaths = Directory.GetFiles("E:\\a\\");
            foreach (var f in filePaths)
            {
                String file = f.ToString();
                String str = "H:\\a\\" + file.ToString();
                if (!File.Exists(str))
                {
                    File.Copy(file, str);
                }
            }
            
            /*
            String[] words = text.Split(' ');
            /*foreach (string word in words)
            {
                if ((word.Equals("mail")) || (word.Equals("email")))
                    status1 = 1;
                else if (word.Equals("to") && status1==1)
                    status2 = 1;
                else if (word.Equals("google"))
                {
                    ProcessStartInfo ij = new ProcessStartInfo();
                    ij.FileName = "firefox.exe";
                    ij.Arguments = "http://www.google.com";
                    Process.Start(ij);
                }
                if (status2 == 1)
                    receiver = word;
            }
           sendEMailThroughGmail(receiver, "Innovators 2.0");
        }*/

        public void sendEMailThroughGmail(String text2, String subj)
        {
            try
            {
                //Mail Message
                MailMessage mM = new MailMessage();
                //Mail Address
                mM.From = new MailAddress("rahulk7494@gmail.com");
                //receiver email id
                mM.To.Add(""+text2+"@gmail.com");
                //subject of the email
                mM.Subject = subj;
                //deciding for the attachment
                //mM.Attachments.Add(new Attachment(@"C:\\attachedfile.jpg"));
                //add the body of the email
                mM.Body = "It is an auto-generated email from Rahul Kumar";
                mM.IsBodyHtml = true;
                //SMTP client
                SmtpClient sC = new SmtpClient("smtp.gmail.com");
                //port number for Gmail mail
                sC.Port = 587;
                //credentials to login in to Gmail account
                sC.Credentials = new NetworkCredential("rahulk7494@gmail.com", "687248service");
                //enabled SSL
                sC.EnableSsl = true;
                //Send an email
                sC.Send(mM);
            }//end of try block
            catch (Exception ex)
            {
                Console.WriteLine("" + ex);
            }//end of catch
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            deleteDir(textBox2.Text);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            createDir(textBox2.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pi = new ProcessStartInfo("taskmgr.exe");
            Process.Start(pi);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string dest = @""+textBox2.Text;
            string source = @""+textBox1.Text;
            //System.IO.File.Move(source,dest);
            System.IO.Directory.Move(source, dest);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //System.IO.Directory.Move(textBox1.Text, textBox2.Text);
            System.IO.File.Move(textBox1.Text, textBox2.Text);
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            if(System.IO.File.Exists(text))
                MessageBox.Show("File already exists , change file name");
            else
            {
                FileStream fs = new FileStream(@""+text, FileMode.CreateNew);
                System.Diagnostics.Process.Start(@"" + text);           // Process.Start("explorer.exe","E:");
                fs.Close();
            }
        }

        
        private void button9_Click(object sender, EventArgs e)
        {
           ExitWindowsEx(0,0);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Process.Start("shutdown","/s /t 0");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Process.Start("shutdown", "/r /t 0");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            SetSuspendState(false, true, true);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            SetSuspendState(true, true, true);
        }

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lp2);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
       
        private void button13_Click(object sender, EventArgs e)
        {
                // retrieve Notepad main window handle
                caption();
                string name = textBox1.Text;
                IntPtr hWnd = FindWindowByCaption(IntPtr.Zero,name);
                if (!hWnd.Equals(IntPtr.Zero))
                {
                    // SW_SHOWMAXIMIZED to maximize the window
                    // SW_SHOWMINIMIZED to minimize the window
                    // SW_SHOWNORMAL to make the window be normal size
                    ShowWindowAsync(hWnd, SW_SHOWMAXIMIZED);
                }
        }

        
        private void button16_Click(object sender, EventArgs e)
        {
            string file=@""+textBox1.Text;
            if (System.IO.Directory.Exists(file))
            {
                var wsh = new IWshShell_Class();
                string fileName = Path.GetFileName(file);
                IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fileName + ".lnk")
                    as IWshRuntimeLibrary.IWshShortcut;
                shortcut.TargetPath = file;
                shortcut.Save();
                popup("Shortcut created successfully");
            }
            else
                MessageBox.Show("File doesn't Exist");
        }

        private void popup(string msg)
        {

            Thread th = new Thread(() =>
            {
                try
                {
                    Open(msg);
                }
                catch (Exception)
                {

                }
            });
            th.Start();
            Thread.Sleep(50000);   //you can update this time as your requirement
            th.Abort();
        }

        private void Open(string msg)
        {
            Success frm = new Success(msg);
            frm.Show();   // frm.Show(); if MDI Parent form
        }

        private void button19_Click(object sender, EventArgs e)
        {
            caption();
            string name = textBox1.Text;
            IntPtr hWnd = FindWindowByCaption(IntPtr.Zero, name);
            if (!hWnd.Equals(IntPtr.Zero))
            {
                ShowWindowAsync(hWnd, SW_SHOWMINIMIZED);
            }
        }
        /*
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("User32.dll",CharSet=CharSet.Auto , ExactSpelling=true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("kernel32.dll")]
        static extern int GetProcessId(IntPtr handle);
        */

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

        
        public void caption()
        {
            string[] tokens = GreedyTokenize(textBox1.Text);
            foreach (string token in tokens)
            {
                int count = listBox1.Items.Count;
                Console.WriteLine("*****" + token + " ****" + count);
                for (int i = 0; i <= count - 1; i++)
                {
                    Console.WriteLine(listBox1.Items[i].ToString());
                    tokenize(listBox1.Items[i].ToString(), token, i);
                }
                listBox1.Items.Clear();
                object[] obj = new object[listBox3.Items.Count];
                listBox3.Items.CopyTo(obj, 0);
                listBox1.Items.AddRange(obj);
                for (int j = 0; j <= listBox1.Items.Count - 1; j++)
                    Console.WriteLine(listBox1.Items[j].ToString());
                Console.WriteLine("*****" + token + " ****" + listBox1.Items.Count);

                listBox3.Items.Clear();
               
            }
            textBox1.Text = listBox1.Items[0].ToString();
            
        }
        
        public void tokenize(string text, string temp, int i)
        {
            string[] tokens = GreedyTokenize(text);
            listBox2.Items.Clear();
            foreach (string token in tokens)
            {
                listBox2.Items.Add(token);
            }
            int index = listBox2.FindStringExact(temp);
            if (index < 0)
            {
            }
            else
            { 
                    listBox1.SelectedIndex = i;
                    listBox3.Items.Add(listBox1.SelectedItem.ToString());
                
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

        
        private void button17_Click(object sender, EventArgs e)
        {
            caption();
            string name = textBox1.Text;
            string key = textBox2.Text;
            string s1 = name.Substring(name.LastIndexOf("-") + 2, name.Length - (name.LastIndexOf("-") + 2));
            textBox3.Text = s1;
            text1 = textBox1.Text;
            string text2 = textBox3.Text.ToLower();

            MySqlDataReader mdr1=null , mdr2=null;
            string action = key;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
                MySqlCommand cmd;

                string s;
                int id=0;
                s = "SELECT * FROM application";
                cmd = new MySqlCommand(s, conn);
                mdr1 = cmd.ExecuteReader();
                while (mdr1.Read())
                {
                    if (mdr1.GetString(1).Equals(text2))
                    {
                        id = mdr1.GetInt32(0);
                        textBox3.Text = id.ToString();
                        break;
                    }
                }

                if (mdr1 != null)
                    mdr1.Close();
               
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
                            textBox3.Text = mdr2.GetString(3);
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
                {
                    mdr2.Close();
                }
                if(conn!=null)
                    conn.Close();
            }
            Start(name, action);            
        }

        [Serializable]
        public struct ShellExecuteInfo
        {
            public int Size;
            public uint Mask;
            public IntPtr hwnd;
            public string Verb;
            public string File;
            public string Parameters;
            public string Directory;
            public uint Show;
            public IntPtr InstApp;
            public IntPtr IDList;
            public string Class;
            public IntPtr hkeyClass;
            public uint HotKey;
            public IntPtr Icon;
            public IntPtr Monitor;
        }

        // Code For OpenWithDialog Box
        [DllImport("shell32.dll", SetLastError = true)]
        extern public static bool ShellExecuteEx(ref ShellExecuteInfo lpExecInfo);

        public const uint SW_NORMAL = 1;

        static void OpenAs(string file)
        {
            ShellExecuteInfo sei = new ShellExecuteInfo();
            sei.Size = Marshal.SizeOf(sei);
            sei.Verb = "openas";
            sei.File = file;
            sei.Show = SW_NORMAL;
            if (!ShellExecuteEx(ref sei))
                throw new System.ComponentModel.Win32Exception();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string file = textBox1.Text;
            OpenAs(file);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            IntPtr hWnd = FindWindowByCaption(IntPtr.Zero, name);
            if (!hWnd.Equals(IntPtr.Zero))
            {
                ShowWindowAsync(hWnd, SW_SHOWNORMAL);
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            string fileName;
            string source = @"" + textBox1.Text;
            string dest = @"" + textBox2.Text;
            fileName = System.IO.Path.GetFileName(source);
            string df = System.IO.Path.Combine(dest, fileName);
            System.IO.File.Copy(source, df, true);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        static uint WM_CLOSE = 0x0010;
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void button20_Click(object sender, EventArgs e)
        {
            caption();
            string name=textBox1.Text;
            IntPtr h = FindWindowByCaption(IntPtr.Zero,name);
            PostMessage(h, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);            
        }

        private void label4_Click(object sender, EventArgs e)
        {
            
        }

        private void button23_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            System.Diagnostics.Process.Start(@"" + text);           // Process.Start("explorer.exe","E:");
        }

        
        private void button24_Click(object sender, EventArgs e)
        {
            text1 = textBox1.Text;
            caption();
            //string s = textBox1.Text;
            //textBox3.Text = s;
           /* string s1 = s.Substring(s.LastIndexOf("-") + 2, s.Length - (s.LastIndexOf("-") + 2));
            textBox3.Text = s1;*/
        }

        private void button25_Click(object sender, EventArgs e)
        {
            Process[] pr = Process.GetProcesses();
            listBox1.Items.Clear();
            foreach (Process p in pr)
            {
                if (!(string.IsNullOrEmpty(p.MainWindowTitle)))
                    listBox1.Items.Add(p.MainWindowTitle);
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            listBox2.Items.Clear();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            Function f = new Function();
            //Function.setText(textBox1.Text);
            //Function.setMethod(textBox2.Text);

            //string methodName = textBox2.Text;

            //MethodInfo mi = this.GetType().GetMethod(methodName);           // using System.Reflection;
            //mi.Invoke((this, null);
/*
            Type type = typeof(Function);
            MethodInfo method = type.GetMethod(methodName);
            Function c = new Function();
            method.Invoke(c, null);*/
            //Console.WriteLine(result);

            f.main();
            //Process.Start("::{20d04fe0-3aea-1069-a2d8-08002b30309d}"); // Open My Computer
            //Process.Start("cmd.exe");
            //string myFavoritesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            //System.Diagnostics.Process.Start("explorer",myFavoritesPath);
        }
    }
}
