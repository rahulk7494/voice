using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Settings : Form
    {
        List<int> appId = new List<int>();
        List<int> cmdId = new List<int>();
        List<string> cmdName = new List<string>();
        int selectedId = 0;
        int index;

        public Settings()
        {
            InitializeComponent();
            panel1.Dock = DockStyle.Top;
            //panel2.Dock = DockStyle.Top;

            try
            {
                Value.conn = new MySqlConnection(Value.cs);
                Value.conn.Open();
                string s = "SELECT * FROM application";
                Value.cmd = new MySqlCommand(s, Value.conn);
                Value.mdr = Value.cmd.ExecuteReader();
                Value.listOfApplication.Clear();
                while (Value.mdr.Read())
                {
                    comboBox1.Items.Add(Value.mdr.GetString(1));
              //      comboBox3.Items.Add(Value.mdr.GetString(1));
                    Value.listOfApplication.Add(Value.mdr.GetString(1));
                    appId.Add(Value.mdr.GetInt32(0));
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
            comboBox1.SelectedIndex = 0;
            viewSetting(appId[0]);
        }

        public void viewSetting(int appId)
        {
            try
            {
                Value.conn = new MySqlConnection(Value.cs);
                Value.conn.Open();
                string s = "SELECT command_id AS CommandId , command_name AS CommandName , shortcut AS Shortcut FROM command where application_id=" + appId + " AND shortcut IS NOT NULL";
                Value.cmd = new MySqlCommand(s, Value.conn);
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = Value.cmd;
                DataTable db = new DataTable();
                sda.Fill(db);
                BindingSource bs = new BindingSource();
                bs.DataSource = db;
                dataGridView1.DataSource = bs;
                sda.Update(db);
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

        private void loadTable(object sender, EventArgs e)
        {
            selectedId = comboBox1.SelectedIndex;
            Console.WriteLine(selectedId);
            viewSetting(comboBox1.SelectedIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Visible = true;
            dataGridView1.ReadOnly = true;
            panel1.Visible = true;
            panel3.Visible = true;
            button6.Visible = false;
            button7.Visible = false;
            richTextBox1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            index = comboBox2.SelectedIndex;
            DialogResult result;
            if (index == 0)
                result = openFileDialog1.ShowDialog(); // Show the dialog.
            else
                result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                if (index == 0)
                    richTextBox1.Text = openFileDialog1.FileName;
                else
                    richTextBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void selectAction(object sender, EventArgs e)
        {
            panel1.Visible = true;
            label3.Visible = true;
            richTextBox1.Visible = true;
            comboBox3.Visible = false;
            switch (comboBox2.SelectedIndex)
            {
                case 0: label3.Text = "Select Program";
                    button2.Visible = true;
                    button2.Text = "Select Program";
                    break;
                case 1: label3.Text = "URL";
                    richTextBox1.Text = "http://";
                    button2.Visible = false;
                    break;
                case 2: label3.Text = "Select Directory";
                    button2.Visible = true;
                    button2.Text = "Select Directory";
                    break;
                case 3:
                    button2.Visible = false;
                    break;
                case 4: label3.Text = "Command Name";
                    richTextBox1.Visible = false;
                    button2.Visible = false;
                    comboBox3.Visible = true;
                    try
                    {
                        Value.conn = new MySqlConnection(Value.cs);
                        Value.conn.Open();
                        string s = "SELECT * FROM command where application_id=0";
                        Value.cmd = new MySqlCommand(s, Value.conn);
                        Value.mdr = Value.cmd.ExecuteReader();
                        while (Value.mdr.Read())
                        {
                            cmdId.Add(Value.mdr.GetInt32(0));
                            cmdName.Add(Value.mdr.GetString(1));
                            comboBox3.Items.Add(Value.mdr.GetString(1).ToString());
                        }                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    break;
            }
        }

        public void hide()
        {
            panel1.Visible = false;
            label3.Visible = false;
            richTextBox1.Visible = false;
            button2.Visible = false;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            hide();
            textBox1.Text = "";
            richTextBox1.Text = "";
            label3.Text = "";
            comboBox2.SelectedIndex = -1;
            flowLayoutPanel1.Visible = false; 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string commandName = textBox1.Text;
            string command="";
            command = command + richTextBox1.Text;
            command = command.Replace("\\", "\\\\");
            int id = 0;
            string s = "";
            try     {
                Value.conn = new MySqlConnection(Value.cs);
                Value.conn.Open();
                
                s = "Select MAX(command_id) FROM command";
                Value.cmd = new MySqlCommand(s, Value.conn);
                Value.mdr = Value.cmd.ExecuteReader();

                if (Value.mdr.Read())
                {
                    id = Value.mdr.GetInt32(0);
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.ToString());
            }
            finally {
                if (Value.mdr != null)
                    Value.mdr.Close();
            }

            try     {
                s = "INSERT INTO command VALUES (" + (id + 1) + ",'"+commandName+"',5,'"+command+"','"+command+"',0)";
                Value.cmd = new MySqlCommand(s, Value.conn);
                Value.mdr = Value.cmd.ExecuteReader();

                while (Value.mdr.Read())
                {
                    
                }
            }
            catch (MySqlException ex)   {
                Console.WriteLine("Error : {0}", ex.ToString());
            }
            finally {
                if (Value.mdr != null)
                    Value.mdr.Close();
                if (Value.conn != null)
                    Value.conn.Close();
            }
            MessageBox.Show("Command Added");
            hide();
            /*
                connection.Open();
                cmd.CommandText = ("INSERT INTO custom_command (command_id,command_name,action,command) VALUES (" + (id + 1) + ", @CommandName, @Action, @Command)");
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@CommandName", commandName);
                cmd.Parameters.AddWithValue("@Action", action);
                cmd.Parameters.AddWithValue("@Command", command);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            */
        }

        public int addApplication(string name)
        {
            return 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (selectedId == 5)
                dataGridView1.ReadOnly = false;
            else
                dataGridView1.ReadOnly = true;
            button6.Visible = true;
            button7.Visible = true;
            //panel3.Visible = true;
            panel1.Visible = false;
            try
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void tokenize(string text, string temp, int i)
        {
            string[] tokens = GreedyTokenize(text);
            foreach (string token in tokens)
            {
            }
        }

        public static string[] GreedyTokenize(string text)
        {
            char[] delimiters = new char[] {

                  '+',
                  '|', '\\', ':', ';', ' ', '\'', ',', '.', '/', '?', '~', '!',
                  '@', '#', '$', '%', '^', '&', '*', ' ', '\r', '\n', '\t'
                  
                  };

            return text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //int i=dataGridView1.NewRowIndex;
            //Console.WriteLine(i);
            Value.conn.Open();
            if (dataGridView1.RowCount >= 1)
            {
                Console.WriteLine(dataGridView1.RowCount);
                for (int x = 0; x < dataGridView1.RowCount ; x++)
                {
                    if (dataGridView1.Rows[x].Cells[1].Value.ToString() != "" && dataGridView1.Rows[x].Cells[1].Value != null)
                    {
                        //SqlCommand cmdSave = new SqlCommand("UPDATE command SET command_name=@cname,   shortcut=@shortcut, action=@action WHERE command_id=@cid ");
                        
                            //cmdSave.Parameters.Add("@cname", SqlDbType.VarChar).Value = dataGridView1.Rows[x].Cells[1].Value.ToString();
                            string skt = dataGridView1.Rows[x].Cells[2].Value.ToString();
                            string []s = skt.Split('+');
                            string temp = "";
                            for (int i = 0; i < s.Length; i++)
                            {
                                switch (s[i])
                                {
                                    case "CTRL": s[i] = "^"; break;
                                    case "ALT": s[i] = "%"; break;
                                    case "SHIFT": s[i] = "+"; break;
                                }
                                temp = temp + s[i];
                            }
                            //Console.WriteLine(temp);
                            //Console.WriteLine(skt);
                            skt = temp;
                            //Console.WriteLine(skt.ToLower());
                            //cmdSave.Parameters.Add("@shortcut", SqlDbType.VarChar).Value = dataGridView1.Rows[x].Cells[1].Value.ToString();
                            //cmdSave.Parameters.Add("@action", SqlDbType.VarChar).Value = skt.ToLower();
                            //cmdSave.Parameters.Add("@cid", SqlDbType.VarChar).Value = dataGridView1.Rows[x].Cells[3].Value.ToString();

                            String sql = "UPDATE command SET command_name='" + dataGridView1.Rows[x].Cells[1].Value.ToString() +
                                    "', shortcut='" + dataGridView1.Rows[x].Cells[2].Value.ToString() +
                                    "', action='" + skt.ToLower() +
                                    "' WHERE command_id= " + dataGridView1.Rows[x].Cells[0].Value;

                            Value.cmd = new MySqlCommand(sql, Value.conn);
                            Value.mdr = Value.cmd.ExecuteReader();
                            Value.mdr.Close();
                            
                        //MessageBox.Show("Record Updated!");
                    }
                }
            }
            Value.conn.Close();
            /*String s = "";
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                s = "Insert into command(date,column1,....) values ("
                            + System.DateTime.Now + ", "
                            + dataGridView1.Rows[i].Cells[1].Value + ".......);";
                Value.cmd = new MySqlCommand(s, Value.conn);
                Value.mdr = Value.cmd.ExecuteReader();
                cmd.CommandText = Strquery;
                cmd.ExecuteNonQuery();
            }*/
            MessageBox.Show("Record Updated!");
        }
        /*
        private void button3_Click_1(object sender, EventArgs e)
        {

        }*/

        private void button7_Click(object sender, EventArgs e)
        {
            button6.Visible = false;
            button7.Visible = false;
            dataGridView1.ReadOnly = true;
        }
        /*
        private void button4_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "";
            richTextBox1.Text = "";
            label3.Text = "";
            label3.Visible = false;
            richTextBox1.Visible = false;
            button2.Visible = false;
            comboBox2.SelectedIndex = -1;
            flowLayoutPanel1.Visible = false;
        }*/
    }
}
