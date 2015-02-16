using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class ListOfApplication : Form
    {
        public ListOfApplication()
        {
            InitializeComponent();
            list();
        }

        public void list()
        {
            listBox1.Items.Clear();
            foreach (string item in Value.listOfApplication)
            {
                listBox1.Items.Add(item.ToString());
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Value.activeApplication = listBox1.SelectedItem.ToString();
            Console.WriteLine(listBox1.SelectedItem.ToString());
            this.Dispose();
        }
    }
}
