﻿using System;
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
using System.IO;

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

        static string[] line = System.IO.File.ReadAllLines(@"E:\words.txt");

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
            else if (e.Result.Text.IndexOf("sorry") >= 0)
            {
                Console.WriteLine(textBox1.Text);
                string arr = textBox1.Text.Remove(textBox1.Text.LastIndexOf(" "));
                Console.WriteLine(arr);
                textBox1.Text = arr;
            }
            else if (e.Result.Text.IndexOf("flush") >= 0)
            {
                Console.WriteLine(textBox1.Text);
                textBox1.Text = "";
            }
            else if (e.Result.Text.IndexOf("write into") >= 0)
            {
                string arr = textBox1.Text.Remove(0,textBox1.Text.Count()-textBox1.Text.LastIndexOf(" "));
                Console.WriteLine(arr);
                textBox1.Text = arr;
            }
            else
                textBox1.Text = textBox1.Text + " " + e.Result.Text;
            textBox2.Text = "Success";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartRecognition();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.IndexOf("write into") >= 0)
            {
                string arr = textBox1.Text.Remove(0,textBox1.Text.LastIndexOf(" ")+1);
                Console.WriteLine(arr);
                textBox1.Text = arr;
            }
            else
            {
                Console.WriteLine(textBox1.Text);
                string arr = textBox1.Text;
                System.IO.File.WriteAllText(@"E:\vesper.txt", arr);
                textBox1.Text = "";
                textBox2.Text = "Success";
                Morphological m = new Morphological();
                m.buttonClicked();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Speech_Enter(object sender, EventArgs e)
        {
            
        }
    }
    
}