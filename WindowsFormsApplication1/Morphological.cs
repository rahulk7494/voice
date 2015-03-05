using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Data.Odbc;
using MySql.Data.MySqlClient;
using Microsoft.Win32;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    public partial class Morphological : Form
    {

        private string mModelPath = @"C:\Projects\DotNet\OpenNLP\OpenNLP\Models\";
        private OpenNLP.Tools.SentenceDetect.MaximumEntropySentenceDetector mSentenceDetector;
        private OpenNLP.Tools.Tokenize.EnglishMaximumEntropyTokenizer mTokenizer;
        private OpenNLP.Tools.PosTagger.EnglishMaximumEntropyPosTagger mPosTagger;
        SpeechSynthesizer reader1 =new SpeechSynthesizer();
        
        public Morphological()
        {
          //  InitializeComponent();
            
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            if (radioButton1.Checked == false)
                reader1.SpeakAsync("Welcome Sir. This is System Voice ready to assist you.hello innovators whats up ? i hope your project is going good ! all the best and keep the hard work on .");
            else
                reader1.SpeakAsync("Welcome Madam. This is System Voice ready to your assist you");
            */
        }
        private string[] SplitSentences(String str)
        {
            if (mSentenceDetector == null)
            {
                mSentenceDetector = new OpenNLP.Tools.SentenceDetect.EnglishMaximumEntropySentenceDetector(mModelPath + "EnglishSD.nbin");
            }
            
            return mSentenceDetector.SentenceDetect(str);
        }
        
        private string[] TokenizeSentence(string sentence)
        {
            if (mTokenizer == null)
            {
                mTokenizer = new OpenNLP.Tools.Tokenize.EnglishMaximumEntropyTokenizer(mModelPath + "EnglishTok.nbin");
            }
            return mTokenizer.Tokenize(sentence);
        }
        private string[] PosTagTokens(string[] tokens)
        {
            if (mPosTagger == null)
            {
                mPosTagger = new OpenNLP.Tools.PosTagger.EnglishMaximumEntropyPosTagger(mModelPath + "EnglishPOS.nbin", mModelPath + @"\Parser\tagdict");
            }
            return mPosTagger.Tag(tokens);
        }

        public int searchCommand()
        {
            String a = File.ReadAllText(@"E:\input.txt");
            try
            {
                Value.conn = new MySqlConnection(Value.cs);
                Value.conn.Open();

                string sq;
                sq = "SELECT * FROM command";// WHERE application_id=" + Value.appId + " OR application_id=5";
                Value.cmd = new MySqlCommand(sq, Value.conn);
                Value.mdr = Value.cmd.ExecuteReader();
                while (Value.mdr.Read())
                {
                    //Console.WriteLine(Value.mdr.GetString(1));
                    string temp=Value.mdr.GetString(1).ToLower();

                    if (temp.StartsWith(a[0].ToString().ToLower()) && temp.IndexOf(a.ToLower()) >= 0)
                    {
                        string action = Value.mdr.GetString(3);
                        Value.appId = Value.mdr.GetInt32(2);
                        Console.WriteLine("Match Found : Command Id=" + Value.mdr.GetInt32(0));
                       // if (action.Equals("rightclick"))
                          //  Value.rfound = true;
                        if (Value.mdr.GetInt32(0)==36)
                        {
                            KeyboardSend.KeyDown(Keys.LWin);
                            KeyboardSend.KeyUp(Keys.LWin);
                            return 1;
                        }
                        if (action.StartsWith("."))
                        {
                            action = action.Remove(0, 1);
                            //Function.setText(Value.activeApplication);
                            Function.callMethod(action);
                            return 1;
                        }
                        else if (Value.appId == 5)
                        {
                            action=action.Replace("\\","\\\\");
                            Process.Start(action);
                            return 1;
                        }
                        else
                        {
                            Function.setCommand(" " + Value.mdr.GetString(1));
                            Function.setText(Value.activeApplication);
                            Function f1 = new Function();
                            f1.sendCommand(Value.mdr.GetString(3));
                            return 1;
                        }
                    }
                }
                /*Value.mdr.Close();
                /*sq = "SELECT * FROM command where command_id=(select command_id from synonym where keyword='" + a.ToLower() + "')";
                //Console.WriteLine(sq); 
                Value.cmd = new MySqlCommand(sq, Value.conn);
                Value.mdr = Value.cmd.ExecuteReader();

                if (Value.mdr.Read())
                {
                    //Console.WriteLine(Value.mdr.GetString(1));
                    Console.WriteLine("Match Found : Command Id=" + Value.mdr.GetInt32(0));
                    Value.appId = Value.mdr.GetInt32(2);
                    //Console.WriteLine(Value.appId);
                    commandFound = true;
                    Function.setCommand(" " + Value.mdr.GetString(1));
                    //Console.WriteLine(Value.appId); 
                    Function.setText(Value.activeApplication);
                    //Console.WriteLine(Value.appId); 
                    Function f1 = new Function();
                    //Console.WriteLine(Value.appId); 
                    string action = Value.mdr.GetString(3);
                    action = action.Remove(0, 1);
                    Console.WriteLine(action);
                    f1.sendCommand(Value.mdr.GetString(3));
                    //Console.WriteLine("Match Found : Command Id=" + Value.mdr.GetInt32(0));
                }*/
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
            //if (commandFound == true)
            //    return 1;
            return 0;
        }

        public void buttonClicked()
        {
            if (searchCommand() == 1)
                return;
            //String sql= "Select * from Application";
            //MySqlConnection con = new MySqlConnection("host=localhost;user=root;password=root;database=voice;");
            //MySqlCommand cmd = new MySqlCommand(sql, con);
            //con.Open();
            
            String[] arr2 = TokenizeSentence("This too shall pass");
            String[] arr3 = TokenizeSentence(System.IO.File.ReadAllText(@"E:\input.txt"));
            int len = arr3.Length;
            int i,j,flag;
            String[] arr1 = PosTagTokens(arr3);
            int[] arr4= {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
            String[] arr5 = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            String[] arr6 = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" ,"",""};
            String str1, str2;
            //string rCommand = "";
            var emptyArr=new List<string>();
            List<string> application = new List<string>();
            List<string> commandList = new List<string>();

            /*if (Value.doMorpho = false && Value.rfound == true)
            {
                int c;
                for (c = 0; c < arr3.Length; c++)
                {
                    arr6[c] = arr3[c];
                    Console.WriteLine("-- " + arr6[c]);
                }
                foreach (var s in arr6)
                {
                    if (!string.IsNullOrEmpty(s))
                        emptyArr.Add(s);
                }
                arr6 = emptyArr.ToArray();
                rCommand = string.Join(" ", arr6);
                rCommand = "r" + rCommand;
                commandList.Add(rCommand);
                Function.setCommand(commandList[0]);
                application.Add(Value.activeApplication);
                Function.setText(application[0]);
                Function f2 = new Function();
                f2.main();
                Value.doMorpho = true;
                Value.rfound = false;
            }
            else
            {*/
                // reader.Dispose();

                if (File.Exists(@"E:\output.txt"))
                {
                    File.Delete(@"E:\output.txt");
                }
                if (File.Exists(@"E:\output2.txt"))
                {
                    File.Delete(@"E:\output2.txt");
                }
                if (File.Exists(@"E:\output3.txt"))
                {
                    File.Delete(@"E:\output3.txt");
                }
                if (File.Exists(@"E:\final_run.bat"))
                {
                    File.Delete(@"E:\final_run.bat");
                }
                if (File.Exists(@"E:\output4.txt"))
                {
                    File.Delete(@"E:\output4.txt");
                }
                //string s = "";
                for (i = 0; i < len; i++)
                {
                    //s = s + arr1[i] + "\r\n";
                    System.IO.File.AppendAllText((@"E:\output.txt"), arr1[i] + Environment.NewLine);
                }
                //System.IO.File.WriteAllText((@"E:\output.txt"), s);
                //String[] str = { "asdfg" };
                porter.Stemmer class1 = new porter.Stemmer();
                class1.morphine();
                j = 0;
                System.IO.StreamReader file = new System.IO.StreamReader(@"E:\output.txt");
                System.IO.StreamReader file2 = new System.IO.StreamReader(@"E:\output_new.txt");
                //MySqlDataReader reader = cmd.ExecuteReader();
                i = 0;
                Console.WriteLine("len :" + len);

                while (i < len)
                {
                    flag = 0;
                    str1 = file.ReadLine();
                    str2 = file2.ReadLine();
                    if (arr1[i] == "CC" || arr1[i] == "FW" || arr1[i] == "IN" || arr1[i] == "NN" || arr1[i] == "NNS" || arr1[i] == "NNP" || arr1[i] == "NNPS" || 
                        arr1[i] == "RB" || arr1[i] == "RBR" || arr1[i] == "RBS" || arr1[i] == "TO" || arr1[i] == "UH" || arr1[i] == "VB" || arr1[i] == "VBD" || 
                        arr1[i] == "VBG" || arr1[i] == "VBN" || arr1[i] == "VBP" || arr1[i] == "VBZ")
                    {
                        if (arr1[i] == "NNP" || arr1[i] == "FW" || arr1[i] == "NN" || arr1[i] == "NNS"|| arr1[i] == "RB" || arr1[i] == "RBR" || arr1[i] == "RBS")
                            while ((i <= (len - 1)) && (arr1[i] == "NNP" || arr1[i] == "FW" || arr1[i] == "NN" || arr1[i] == "NNS"|| arr1[i] == "RB" || arr1[i] == "RBR" || arr1[i] == "RBS"))
                            {
                                arr4[j] = 1;
                                if (flag != 0)
                                {
                                    arr5[j] = file.ReadLine();
                                    arr6[j] = file2.ReadLine();
                                    str1 = arr5[j];
                                    str2 = arr6[j];
                                    Console.WriteLine("here   " + str2);
                                }
                                arr5[j] = str1;
                                arr6[j] = str2;
                                flag++;
                                System.IO.File.AppendAllText(@"E:\output3.txt", arr1[i] + "      " + Environment.NewLine);
                                System.IO.File.AppendAllText(@"E:\output4.txt", arr6[j] + "      " + Environment.NewLine);
                                j++;
                                i++;
                            }
                        else if (arr1[i] == "VB" || arr1[i] == "VBD" || arr1[i] == "VBG" || arr1[i] == "VBN" || arr1[i] == "VBP" || arr1[i] == "VBZ")
                        {
                            if (j >= 1 && (arr5[j - 1] == "VB" || arr5[j - 1] == "VBD" || arr5[j - 1] == "VBG" || arr5[j - 1] == "VBN" || arr5[j - 1] == "VBP" || arr5[j - 1] == "VBZ"))
                            {
                                System.IO.File.AppendAllText(@"E:\output3.txt", "Replaced " + arr5[j - 1] + "  with   " + str1 + Environment.NewLine);
                                System.IO.File.AppendAllText(@"E:\output4.txt", "Replaced " + arr6[j - 1] + "  with   " + str2 + Environment.NewLine);
                                arr5[j - 1] = str1;
                                arr6[j - 1] = str2;
                                i++;
                            }
                            else
                            {
                                arr5[j] = str1;
                                arr6[j] = str2;
                                System.IO.File.AppendAllText(@"E:\output3.txt", arr1[i] + "      " + Environment.NewLine);
                                System.IO.File.AppendAllText(@"E:\output4.txt", arr6[j] + "      " + Environment.NewLine);
                                j++;
                                i++;
                            }
                        }
                        else if (arr1[i] == "IN" || arr1[i] == "TO")
                        {
                            arr5[j] = str1;
                            arr6[j] = str2;
                            System.IO.File.AppendAllText(@"E:\output3.txt", arr1[i] + "      " + Environment.NewLine);
                            System.IO.File.AppendAllText(@"E:\output4.txt", arr6[j] + "      " + Environment.NewLine);
                            j++;
                            arr4[j] = 2;
                            i++;
                        }
                        else if (arr1[i] == "CC")
                        {
                            arr5[j] = str1;
                            arr6[j] = str2;
                            System.IO.File.AppendAllText(@"E:\output3.txt", arr1[i] + "      " + Environment.NewLine);
                            System.IO.File.AppendAllText(@"E:\output4.txt", arr6[j] + "      " + Environment.NewLine);
                            j++;
                            arr4[j] = 3;
                            i++;
                        }
                        else if (arr1[i] == "DT")
                        {
                            arr5[j] = str1;
                            arr6[j] = str2;
                            System.IO.File.AppendAllText(@"E:\output3.txt", arr1[i] + "      " + Environment.NewLine);
                            System.IO.File.AppendAllText(@"E:\output4.txt", arr6[j] + "      " + Environment.NewLine);
                            j++;
                            arr4[j] = 4;
                            i++;
                        }
                        else i++;
                    }
                    else i++;

                }

                //Function f = new Function();
                //f.searchFile();

                len = j;
                i = 0;
                int p, k, z;
                p = k = z = 0;

                //List<string> applicationList = new List<string>();
                bool temp = false;              // check whether application is added or not
                string app = "";
                string cmd = "";
                bool noun = false;
                //Console.WriteLine(len);
                for (i = 0; i < len; i++)
                {
                    j = i;
                    Console.WriteLine("*****" + arr5[i] + "\t" + arr6[i]);
                    if (arr5[i].Equals("VBG") || arr5[i].Equals("VB") || arr5[i].Equals("VBN"))
                    {
                        k = i;
                        if (noun == true)
                        {
                            app = cmd;
                            cmd = "";
                        } 
                        cmd = cmd + " " + arr6[k]; //commandList.Add(arr6[k]);
                        //temp = true;
                      /*  if (arr6[k].Equals("rightclick"))
                        {
                            Value.rfound = true;
                            Value.doMorpho = false;
                        }*/
                        for (p = j + 1; p < len; p++)
                        {
                            if (arr5[p].Equals("NNS") || arr5[p].Equals("NN") || arr5[p].Equals("NNP") || arr5[p].Equals("RB") || arr5[p].Equals("RBR") || arr5[p].Equals("RBS"))
                            {
                                app = app + " " + arr6[p];    //application.Add(arr6[p]);
                                temp = true;
                            }
                            else
                            {
                                //application.Add("");
                                break;
                            }
                        }
                    }
                    else if (temp==false && (arr5[i].Equals("NN") || arr5[i].Equals("NNS")))             // for custom commands
                    {
                        cmd = cmd + " " + arr6[i];
                        noun = true;
                    }
                    
                    /*else if ((arr5[i].Equals("NN") || arr5[i].Equals("NNS")) && temp == false)
                    {
                        app = app + " " + arr6[i];
                    }*/
                    //application.Add(app);
                    //commandList.Add(cmd);
                    z++;
                    j = p;
                    //Console.WriteLine(j + "\t" + i);
                    /*
                    if (arr5[i].Equals("CC") || (arr5[i].Equals("IN")))
                    {
                        application.RemoveAt(application.Count - 1);
                        continue;
                    }
                    else
                    {
                        if (temp)
                            break;
                    }
                    */
                }
                commandList.Add(cmd);
                application.Add(app);
                //Function f = new Function();
                int flag1 = 0;

                Console.WriteLine("*********************");
                for (i = 0; i < commandList.Count(); i++)
                {
                    if (application == null || application.All(x => string.IsNullOrEmpty(x)))
                    {
                        Console.WriteLine(Value.activeApplication);
                        application.Add(Value.activeApplication);
                    }
                    //Console.WriteLine("{0} {1}", i, commandList.Count());
                    Console.WriteLine("Com[{0}] => {1} ", i, commandList[i]);
                    Console.WriteLine("App[{0}] => {1} ", i, application[application.Count - 1]);
                    Console.WriteLine("AppId => " + Value.appId);
                    if (commandList[i].ToLower().Equals(" open") || commandList[i].ToLower().Equals(" start") || commandList[i].ToLower().Equals(" launch"))
                    {
                      /*  if (Value.folder == true)
                        {
                            foreach(string f in Value.folderContents)
                            {
                                if (app.IndexOf(f) >= 0)
                                    Function.sendKey(Value.activeApplication, "{ENTER}");
                            }
                        }*/
                        if (flag1 == 0)
                        {
                            if (Function.searchFile(application[i]) == 1)
                                flag1 = 1;
                        }
                        else
                        {
                            System.Diagnostics.Process.Start(@"" + application[i]);
                        }
                    }
                    if (flag1 == 0)
                    {
                        Function.setCommand(commandList[i]);
                        Function.setText(application[application.Count - 1]);
                        Function f = new Function();
                        f.main();
                        flag1 = 0;
                    }
                    flag1 = 0;
                }
            //}
           //Application.Restart();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonClicked();
        }

/*
                Console.WriteLine(application);
                Function.setCommand(arr6[i]);
                Function.setText(application);
                    //Function.setApp(application);
                Function f = new Function();
                f.main();
                
        
             //   i = p;
        
            
                if (application == "browser")
                {
                    string name = string.Empty;
                    RegistryKey regKey = null;

                    try
                    {
                        //set the registry key we want to open
                        regKey = Registry.ClassesRoot.OpenSubKey("HTTP\\shell\\open\\command", false);

                        //get rid of the enclosing quotes
                        name = regKey.GetValue(null).ToString().ToLower().Replace("" + (char)34, "");

                        //check to see if the value ends with .exe (this way we can remove any command line arguments)
                        if (!name.EndsWith("exe"))
                            //get rid of all command line arguments (anything after the .exe must go)
                            name = name.Substring(0, name.LastIndexOf(".exe") + 4);

                    }
                    catch (Exception ex)
                    {
                        name = string.Format("ERROR: An exception of type: {0} occurred in method: {1} in the following module: {2}", ex.GetType(), ex.TargetSite, this.GetType());
                    }
                    finally
                    {
                        //check and see if the key is still open, if so
                        //then close it
                        if (regKey != null)
                            regKey.Close();
                        application = Path.GetFileName(name);
                        application = (application.Substring(0, application.LastIndexOf(".") + 1)).ToLower();
                    }
                }
                //Function f = new Function();
                //f.main();

                    /*if (arr4[i] == 1)
                    {
                        while (reader.Read())
                        {

                            textBox1.Text += reader.GetString("application_name") + "     ";
                            if (reader.GetString("application_name") == arr6[i])
                                textBox3.Text += arr6[i];
                        }

                    }
                    else if (arr4[i] == 0)
                        while (reader.Read())
                        {

                            textBox1.Text += reader.GetString("application_name") + "     ";
                            if (reader.GetString("application_name") == arr6[i])
                                textBox3.Text += arr6[i];
                        }
                    textBox1.Text += arr6[i];
                }

            
            for (i = 0; i < j; i++)
            {
                if (arr4[i]==1 && arr6[i] == "browser")
                    arr6[i] = "firefox ";
                else if (arr4[i] == 0 && arr6[i] == "open")
                    arr6[i] = "start ";
            }
            
            for (i = 0; i < j; i++)
            {
                System.IO.File.AppendAllText(@"E:\final_run.bat",arr6[i]);
            }

                if (radioButton1.Checked == true)
                    reader1.SpeakAsync("Alright Sir. Task performed.Bye");
                else
                    reader1.SpeakAsync("Alright Madam. Task performed.Bye");
            
        
*/

        private void button2_Click(object sender, EventArgs e)
        {
            String[] str = {"create"};
            porter.Stemmer class1 = new porter.Stemmer();
            class1.morphine();
            
        }

        
        private void button3_Click_1(object sender, EventArgs e)
        {
            /*
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
            Grammar dictationGrammar = new DictationGrammar();
            recognizer.LoadGrammar(dictationGrammar);
            try
            {
                button3.Text = "Speak Now";
                recognizer.SetInputToDefaultAudioDevice();
                RecognitionResult result = recognizer.Recognize();
                System.IO.File.AppendAllText(@"E:\speech_output.txt", result.Text);
                //textBox1.Text = textBox1.Text +result.Text+ Environment.NewLine;
            }
            catch (InvalidOperationException exception)
            {
                button3.Text = String.Format("Could not recognize input from default aduio device. Is a microphone or sound card available?\r\n{0} - {1}.", exception.Source, exception.Message);
            }
            finally
            {
                recognizer.UnloadAllGrammars();
            }
            */
        }
   }
    
}




namespace porter
{


    class Stemmer
    {
        private char[] b;
        private int i,     /* offset into b */
            i_end, /* offset to end of stemmed word */
            j, k;
        private static int INC = 50;
        /* unit of size whereby b is increased */
        int flag = 0;
        public Stemmer()
        {
            b = new char[INC];
            i = 0;
            i_end = 0;
        }

        /**
         * Add a character to the word being stemmed.  When you are finished
         * adding characters, you can call stem(void) to stem the word.
         */

        public void add(char ch)
        {
            if (i == b.Length)
            {
                char[] new_b = new char[i + INC];
                for (int c = 0; c < i; c++)
                    new_b[c] = b[c];
                b = new_b;
            }

            b[i++] = ch;
        }


        /** Adds wLen characters to the word being stemmed contained in a portion
         * of a char[] array. This is like repeated calls of add(char ch), but
         * faster.
         */

        public void add(char[] w, int wLen)
        {
            if (i + wLen >= b.Length)
            {
                char[] new_b = new char[i + wLen + INC];
                for (int c = 0; c < i; c++)
                    new_b[c] = b[c];
                b = new_b;
            }
            for (int c = 0; c < wLen; c++)
                b[i++] = w[c];
        }

        /**
         * After a word has been stemmed, it can be retrieved by toString(),
         * or a reference to the internal buffer can be retrieved by getResultBuffer
         * and getResultLength (which is generally more efficient.)
         */

        public string ConvertToString()
        {
            return new String(b, 0, i_end);
        }

        /**
         * Returns the length of the word resulting from the stemming process.
         */
        public int getResultLength()
        {
            return i_end;
        }

        /**
         * Returns a reference to a character buffer containing the results of
         * the stemming process.  You also need to consult getResultLength()
         * to determine the length of the result.
         */
        public char[] getResultBuffer()
        {
            return b;
        }

        /* cons(i) is true <=> b[i] is a consonant. */
        private bool cons(int i)
        {
            switch (b[i])
            {
                case 'a':
                case 'e':
                case 'i':
                case 'o':
                case 'u': return false;
                case 'y': return (i == 0) ? true : !cons(i - 1);
                default: return true;
            }
        }

        /* m() measures the number of consonant sequences between 0 and j. if c is
           a consonant sequence and v a vowel sequence, and <..> indicates arbitrary
           presence,

              <c><v>       gives 0
              <c>vc<v>     gives 1
              <c>vcvc<v>   gives 2
              <c>vcvcvc<v> gives 3
              ....
        */
        private int m()
        {
            int n = 0;
            int i = 0;
            while (true)
            {
                if (i > j) return n;
                if (!cons(i)) break; i++;
            }
            i++;
            System.IO.File.AppendAllText((@"E:\output_new2.txt"), "i === " + i + "n === " + n + Environment.NewLine);
            while (true)
            {
                while (true)
                {
                    if (i > j) return n;
                    if (cons(i)) break;
                    i++;
                }
                i++;
                n++;
                System.IO.File.AppendAllText((@"E:\output_new2.txt"), "i === " + i + "n === " + n + Environment.NewLine);
                while (true)
                {
                    if (i > j) return n;
                    if (!cons(i)) break;
                    i++;
                }
                i++;
                System.IO.File.AppendAllText((@"E:\output_new2.txt"), "i === " + i + "n === " + n + Environment.NewLine);
                return n;
            }
        }

        /* vowelinstem() is true <=> 0,...j contains a vowel */
        private bool vowelinstem()
        {
            int i;
            for (i = 0; i <= j; i++)
                if (!cons(i))
                    return true;
            return false;
        }

        /* doublec(j) is true <=> j,(j-1) contain a double consonant. */
        private bool doublec(int j)
        {
            if (j < 1)
                return false;
            if (b[j] != b[j - 1])
                return false;
            return cons(j);
        }

        /* cvc(i) is true <=> i-2,i-1,i has the form consonant - vowel - consonant
           and also if the second c is not w,x or y. this is used when trying to
           restore an e at the end of a short word. e.g.

              cav(e), lov(e), hop(e), crim(e), but
              snow, box, tray.

        */
        private bool cvc(int i)
        {
            char ch = b[i];
            if (ch == 'w' || ch == 'x' || ch == 'y')
                return false;
            if (cons(i) && !cons(i - 1) && cons(i - 2))
                return false;
            if (!cons(i - 2) && cons(i - 1) && cons(i) && b[i - 2] == 'o')
                return false;
            if (!cons(i - 2) && cons(i - 1) && cons(i) && b[i - 2] != 'o')
                return true;
            if (!cons(i - 2) && !cons(i - 1) && cons(i))
                return true;
            if (i < 2 || !cons(i) || cons(i - 1) || !cons(i - 2))
                return false;
            return true;
        }

        private bool ends(String s)
        {
            int l = s.Length;
            int o = k - l + 1;
            if (o < 0)
                return false;
            char[] sc = s.ToCharArray();
            for (int i = 0; i < l; i++)
                if (b[o + i] != sc[i])
                    return false;
            j = k - l;
            return true;
        }

        /* setto(s) sets (j+1),...k to the characters in the string s, readjusting
           k. */
        private void setto(String s)
        {
            int l = s.Length;
            int o = j + 1;
            char[] sc = s.ToCharArray();
            for (int i = 0; i < l; i++)
                b[o + i] = sc[i];
            k = j + l;
        }

        /* r(s) is used further down. */
        private void r(String s)
        {
            if (m() > 0)
                setto(s);
        }

        /* step1() gets rid of plurals and -ed or -ing. e.g.
               caresses  ->  caress
               ponies    ->  poni
               ties      ->  ti
               caress    ->  caress
               cats      ->  cat

               feed      ->  feed
               agreed    ->  agree
               disabled  ->  disable

               matting   ->  mat
               mating    ->  mate
               meeting   ->  meet
               milling   ->  mill
               messing   ->  mess

               meetings  ->  meet

        */

        private void step1()
        {

            if (b[k] == 's')
            {
                if (ends("sses"))
                    k -= 2;
                else if (ends("ies"))
                    setto("y");
                else if (b[k - 1] != 's')
                    k--;
            }
            if (b[k] == 'd')
            {
                if (ends("ied"))
                    setto("y");
            }
            if (ends("eed"))
            {
                if (m() > 0)
                    k--;
            }

            else if ((ends("ed") || ends("ing")) && vowelinstem())
            {
                System.IO.File.AppendAllText((@"E:\output_new2.txt"), "j === " + j + Environment.NewLine);
                System.IO.File.AppendAllText((@"E:\output_new2.txt"), "k === " + k + Environment.NewLine);
                k = j;
                System.IO.File.AppendAllText((@"E:\output_new2.txt"), "j === " + j + Environment.NewLine);
                System.IO.File.AppendAllText((@"E:\output_new2.txt"), "k === " + k + Environment.NewLine);
                if (ends("at"))
                    setto("ate");
                else if (ends("bl"))
                    setto("ble");
                else if (ends("iz"))
                    setto("ize");

                else if (doublec(k))
                {
                    k--;
                    int ch = b[k];
                    if (ch == 'l' || ch == 's' || ch == 'z')
                        k++;
                }
                else if (m() == 1 && cvc(k)) setto("e");
            }
        }

        /* step2() turns terminal y to i when there is another vowel in the stem. */
        private void step2()
        {
            if (ends("i") && vowelinstem())
                b[k] = 'y';
        }

        /* step3() maps double suffices to single ones. so -ization ( = -ize plus
           -ation) maps to -ize etc. note that the string before the suffix must give
           m() > 0. */
        private void step3()
        {
            if (k == 0)
                return;

            /* For Bug 1 */
            switch (b[k - 1])
            {
                case 'a':
                    if (ends("ational")) { r("ate"); break; }
                    if (ends("tional")) { r("tion"); break; }
                    break;
                case 'c':
                    if (ends("enci")) { r("ence"); break; }
                    if (ends("anci")) { r("ance"); break; }
                    break;
                case 'e':
                    if (ends("izer")) { r("ize"); break; }
                    break;
                case 'l':
                    if (ends("bli")) { r("ble"); break; }
                    if (ends("alli")) { r("al"); break; }
                    if (ends("entli")) { r("ent"); break; }
                    if (ends("eli")) { r("e"); break; }
                    if (ends("ousli")) { r("ous"); break; }
                    break;
                case 'o':
                    if (ends("ization")) { r("ize"); break; }
                    if (ends("ation")) { r("ate"); break; }
                    if (ends("ator")) { r("ate"); break; }
                    break;
                case 's':
                    if (ends("alism")) { r("al"); break; }
                    if (ends("iveness")) { r("ive"); break; }
                    if (ends("fulness")) { r("ful"); break; }
                    if (ends("ousness")) { r("ous"); break; }
                    break;
                case 't':
                    if (ends("aliti")) { r("al"); break; }
                    if (ends("iviti")) { r("ive"); break; }
                    if (ends("biliti")) { r("ble"); break; }
                    break;
                case 'g':
                    if (ends("logi")) { r("log"); break; }
                    break;
                default:
                    break;
            }
        }

        /* step4() deals with -ic-, -full, -ness etc. similar strategy to step3. */
        private void step4()
        {
            switch (b[k])
            {
                case 'e':
                    if (ends("icate")) { r("ic"); break; }
                    if (ends("ative")) { r(""); break; }
                    if (ends("alize")) { r("al"); break; }
                    break;
                case 'i':
                    if (ends("iciti")) { r("ic"); break; }
                    break;
                case 'l':
                    if (ends("ical")) { r("ic"); break; }
                    if (ends("ful")) { r(""); break; }
                    break;
                case 's':
                    if (ends("ness")) { r(""); break; }
                    break;
            }
        }

        /* step5() takes off -ant, -ence etc., in context <c>vcvc<v>. */
        private void step5()
        {
            if (k == 0)
                return;

            /* for Bug 1 */
            switch (b[k - 1])
            {
                case 'a':
                    if (ends("al")) break; return;
                case 'c':
                    if (ends("ance")) break;
                    if (ends("ence")) break; return;
                case 'e':
                    if (ends("er")) break; return;
                case 'i':
                    if (ends("ic")) break; return;
                case 'l':
                    if (ends("able")) break;
                    if (ends("ible")) break; return;
                case 'n':
                    if (ends("ant")) break;
                    if (ends("ement")) break;
                    if (ends("ment")) break;
                    /* element etc. not stripped before the m */
                    if (ends("ent")) break; return;
                case 'o':
                    if (ends("ion") && j >= 0 && (b[j] == 's' || b[j] == 't')) break;
                    /* j >= 0 fixes Bug 2 */
                    if (ends("ou")) break; return;
                /* takes care of -ous */
                case 's':
                    if (ends("ism")) break; return;
                case 't':
                    if (ends("ate")) break;
                    if (ends("iti")) break; return;
                case 'u':
                    if (ends("ous")) break; return;
                case 'v':
                    if (ends("ive")) break; return;
                case 'z':
                    if (ends("ize")) break; return;
                default:
                    return;
            }
            if (m() > 1)
                k = j;
        }

        /* step6() removes a final -e if m() > 1. */
        private void step6()
        {
            j = k;

            if (b[k] == 'e')
            {
                int a = m();
                if (a > 1 || a == 1 && !cvc(k - 1))
                    k--;
            }
            if (b[k] == 'l' && doublec(k) && m() > 1)
                k--;
        }


        public string stem()
        {
            k = i - 1;
            Char[] c = new char[k + 1];
            for (j = 0; j <= k; j++)
            {
                c[j] = b[j];
                //Console.WriteLine(b[j]);
            }
            string s = new string(c);
            //Console.WriteLine(s);
            if (k > 1)
            {
                step1();
                step2();
                step3();
                step4();
                step5();
                //step6();
            }
            i_end = k + 1;
            i = 0;
            return s;
        }


        public void morphine()
        {

            System.IO.StreamReader file = new System.IO.StreamReader(@"E:\output.txt");
            FileStream ipt = new FileStream(@"E:\input.txt", FileMode.Open, FileAccess.Read);
            char[] w = new char[501];
            Stemmer s = new Stemmer();

            if (File.Exists(@"E:\output_new.txt"))
            {
                File.Delete(@"E:\output_new.txt");
            }
            if (File.Exists(@"E:\output_new2.txt"))
            {
                File.Delete(@"E:\output_new2.txt");
            }

            try
            {
                FileStream _in = new FileStream(@"E:\input.txt", FileMode.Open, FileAccess.Read);
                try
                {
                    while (true)
                    {
                        int ch = _in.ReadByte();
                        if (Char.IsLetter((char)ch))
                        {
                            int j = 0;
                            while (true)
                            {
                                ch = Char.ToLower((char)ch);
                                w[j] = (char)ch;
                                if (j < 500)
                                    j++;

                                ch = _in.ReadByte();
                                if (!Char.IsLetter((char)ch))
                                {
                                    /* to test add(char ch) */
                                    for (int c = 0; c < j; c++)
                                        s.add(w[c]);
                                    /* or, to test add(char[] w, int j) */
                                    /* s.add(w, j); */
                                    String str = s.stem();

                                    String u;

                                    /* and now, to test toString() : */

                                    u = s.ConvertToString();
                                    String new_str = file.ReadLine();
                                    if (new_str != "VB" && new_str != "VBG" && new_str != "VBZ" && new_str != "VBN")
                                        u = str;
                                    /* to test getResultBuffer(), getResultLength() : */
                                    /* u = new String(s.getResultBuffer(), 0, s.getResultLength()); */
                                    System.IO.File.AppendAllText((@"E:\output_new.txt"), u + Environment.NewLine);
                                    //Console.Write(u);
                                    break;
                                }
                            }
                        }
                        if (ch < 0)
                            break;
                        //Console.Write((char)ch);
                    }
                    _in.Close();
                    file.Close();
                }
                catch (IOException)
                {
                    Console.WriteLine("error reading ");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("file  not found");
            }
            finally
            {
                ipt.Close();
            }
        }
    }
}