using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;

namespace WindowsFormsApplication1
{
    public partial class Speech : Form
    {
        SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
        
        public Speech()
        {
            InitializeComponent();
            Rectangle r = Screen.PrimaryScreen.WorkingArea;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
        }

        public Speech(string text)
        {
            this.textBox2.Text = text;
        }

        static string[] line = System.IO.File.ReadAllLines(@"E:\word.txt");

        private static IEnumerable<string> Combinations(int start, int level)
        {
            for (int i = start; i < line.Length; i++)
                if (level == 1)
                    yield return line[i];
                else
                    foreach (string combination in Combinations(i + 1, level - 1))
                        yield return String.Format("{0} {1}", line[i], combination);

        }

        private void StartRecognition()
        {
            textBox2.Text = "Loading grammars ....";
            
            List<string> s = new List<string>();

            foreach (string r in line)
                s.Add(r);   
            
            var combinations = Combinations(0, 2);
            foreach (var item in combinations)
                s.Add(item.ToString());
            
            string []l = s.ToArray();

            textBox2.Text = "Starting recognition ....";
            
            textBox2.Text = "Load Complete";
            textBox3.Text = Value.mode;

            Console.WriteLine("------------------------------------");

            recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(l)))); //Loads a grammar choice into the speech recognition engine
            recognizer.RequestRecognizerUpdate();
            recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(recognizer_SpeechDetected);
            recognizer.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(recognizer_SpeechRecognitionRejected);
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            recognizer.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(recognizer_RecognizeCompleted);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void recognizer_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            textBox2.Text = "Recognizing voice command...";
        }

        private void recognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            textBox2.Text = "Failure.";
        }

        private void recognizer_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            textBox2.Text = "Success";
        }

        private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.IndexOf("ok")>=0)
            {
                Console.WriteLine(textBox1.Text);
                string arr = textBox1.Text;
                System.IO.File.WriteAllText(@"E:\vesper.txt", arr);
                textBox1.Text = "";
                Morphological m = new Morphological();
                m.buttonClicked();
            }
            else if (e.Result.Text.IndexOf("voice settings") >= 0)
            {
                Console.WriteLine(textBox1.Text);
                Settings s = new Settings();
                s.ShowDialog();
            }
            else if (e.Result.Text.IndexOf("backspace") >= 0)
            {
                Console.WriteLine(textBox1.Text);
                string arr = textBox1.Text.Remove(textBox1.Text.LastIndexOf(" "));
                Console.WriteLine(arr);
                textBox1.Text = arr;
            }
            else if (e.Result.Text.IndexOf("empty") >= 0)
            {
                Console.WriteLine(textBox1.Text);
                textBox1.Text = "";
            }
            else if (e.Result.Text.IndexOf("dictation mode") >= 0)
            {
                Value.mode="dictation";
                textBox3.Text = Value.mode;
            }
            else if (e.Result.Text.IndexOf("exit") >= 0)
            {
                if (Value.mode.Equals("dictation"))
                {
                    Value.mode = "normal";
                    textBox3.Text = Value.mode;
                }
            }
            else
            {
                if (Value.mode.Equals("dictation"))
                {
                    SendKeys.Send(e.Result.Text);
                    //Function.dictate(e.Result.Text + " ");    //Function.sendKey(e.Result.Text);
                }
                else
                    textBox1.Text = textBox1.Text + " " + e.Result.Text;
            }
            textBox2.Text = "Success";
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
                string text1 = buff.ToString();
                Value.activeApplication = text1;
                return text1;
            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            KeyboardSend.KeyDown(Keys.LWin);
            //KeyboardSend.KeyDown(Keys.E);
            //KeyboardSend.KeyUp(Keys.E);
            KeyboardSend.KeyUp(Keys.LWin);
            /*Thread.Sleep(500);
            string t = GetActiveWindow();
            Thread.Sleep(500);
            KeyboardSend.KeyDown(Keys.Right);
            KeyboardSend.KeyUp(Keys.Right);
            Thread.Sleep(500);
            SendKeys.SendWait("avg");
            SendKeys.SendWait("{ENTER}");

            //KeyboardSend.KeyDown(Keys.Shift);
            //KeyboardSend.KeyDown(Keys.F10);
            //KeyboardSend.KeyUp(Keys.F10);
            //KeyboardSend.KeyUp(Keys.Shift);
            //Console.WriteLine(t);
            
            //Shell32.Shell shell = new Shell32.Shell();  // Reference Added - Microsoft Shell Controls and Automation
            //shell.FileRun();


            //Process.Start("C:\\Program Files\\Microsoft Games\\Chess\\Chess.exe");
            //Process.Start("E:\\Music");
             * 
             */
            StartRecognition();
        }

        private void button2_Click(object sender, EventArgs e)
        {
      //      KeyboardSend.KeyDown(Keys.RButton);
        //    KeyboardSend.KeyUp(Keys.RButton);

            Console.WriteLine(textBox1.Text);
            string arr = textBox1.Text;
            System.IO.File.WriteAllText(@"E:\vesper.txt", arr);
            textBox1.Text = "";
            textBox2.Text = "Success";
            Morphological m = new Morphological();
            m.buttonClicked();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Speech_Enter(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Settings s=new Settings();
            s.ShowDialog();
        }
    }

    static class KeyboardSend
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private const int KEYEVENTF_EXTENDEDKEY = 1;
        private const int KEYEVENTF_KEYUP = 2;

        public static void KeyDown(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY, 0);
        }

        public static void KeyUp(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
    }
}
