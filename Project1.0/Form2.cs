using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Recognition;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        SpeechRecognizer s = new SpeechRecognizer();

        private void Form2_Load(object sender, EventArgs e)
        {
  //          s.SpeechRecognized += s_SpeechRecognized;
        }

     /*   void s_SpeechRecognized(object sender, SpeechRecognitionEventArgs e)
        {
            MessageBox.Show(e.Result.Text);
        }
*/    }
}
