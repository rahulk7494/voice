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
        List<string> action = new List<string>();
        List<string> shortcut = new List<string>();
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
            //Console.WriteLine(selectedId);
            viewSetting(comboBox1.SelectedIndex);
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
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
                    richTextBox1.Text = "";
                    button2.Text = "Select Program";
                    break;
                case 1: label3.Text = "URL";
                    richTextBox1.Text = "http://";
                    button2.Visible = false;
                    break;
                case 2: label3.Text = "Select Directory";
                    button2.Visible = true;
                    richTextBox1.Text = "";
                    button2.Text = "Select Directory";
                    break;
                case 3:
                    button2.Visible = false;
                    richTextBox1.Text = "";
                    break;
                case 4: label3.Text = "Command Name";
                    richTextBox1.Visible = false;
                    button2.Visible = false;
                    comboBox3.Visible = true;
                    try
                    {
                        Value.conn = new MySqlConnection(Value.cs);
                        Value.conn.Open();
                        string s = "SELECT * FROM command WHERE application_id=0 AND shortcut<>'0'";
                        Value.cmd = new MySqlCommand(s, Value.conn);
                        Value.mdr = Value.cmd.ExecuteReader();
                        while (Value.mdr.Read())
                        {
                            cmdId.Add(Value.mdr.GetInt32(0));
                            cmdName.Add(Value.mdr.GetString(1));
                            action.Add(Value.mdr.GetString(3));
                            shortcut.Add(Value.mdr.GetString(4));
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
            radioButton1.Checked = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string commandName = textBox1.Text;
            string command = "", act = "";
            command = command + richTextBox1.Text;
            command = command.Replace("\\", "\\\\");
            if (String.IsNullOrEmpty(command))
            {
                int index = comboBox3.SelectedIndex;
                Console.WriteLine("Command Id = " + cmdId[index] + " Command Name = " + cmdName[index]);
                command = cmdName[index];
                act = action[index];
                command = shortcut[index];
                /*
                string s = act;
                //string temp = "";
                for (int i = 0; i < s.Length; i++)
                {
                    //Console.WriteLine(s[i]);
                    if (Char.IsLower(s[i]))
                        s = s.Insert(i++, "+");
                    else
                    {
                        switch (s[i])
                        {
                            case '^': s = s.Replace(s[i].ToString(), "CTRL");
                                break;
                            case '%': s = s.Replace(s[i].ToString(), "ALT");
                                break;
                            case '+': s = s.Replace(s[i].ToString(), "SHIFT");
                                break;
                        }
                    }
                    //Console.WriteLine(s);
                    //temp = temp + s[i];
                }
                command = s.ToUpper();
                //Console.WriteLine(s);
                //Console.WriteLine(act);
                //command = temp;
                  */          
            }
            int id = 0;
            string s1 = "";
            try     {
                Value.conn = new MySqlConnection(Value.cs);
                Value.conn.Open();
                
                s1 = "Select MAX(command_id) FROM command";
                Value.cmd = new MySqlCommand(s1, Value.conn);
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
                if (comboBox3.SelectedIndex == 4)
                    s1 = "INSERT INTO command VALUES (" + (id + 1) + ",'" + commandName + "',5,'" + act + "','" + command + "',0)";
                else
                    s1 = "INSERT INTO command VALUES (" + (id + 1) + ",'" + commandName + "',5,'" + command + "','" + command + "',0)";
                Value.cmd = new MySqlCommand(s1, Value.conn);
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
            //MessageBox.Show("Command Added");
            hide();
            panel3.Visible = false;
            comboBox1.SelectedIndex = 5;
            loadTable(null, new EventArgs());
            radioButton1.Checked = false;
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

        bool delete = false;

        private void button6_Click(object sender, EventArgs e)
        {
            //int i=dataGridView1.NewRowIndex;
            //Console.WriteLine(i);
            if (delete == false)
            {
                Value.conn.Open();
                if (dataGridView1.RowCount >= 1)
                {
                    Console.WriteLine(dataGridView1.RowCount);
                    for (int x = 0; x < dataGridView1.RowCount; x++)
                    {
                        if (dataGridView1.Rows[x].Cells[1].Value.ToString() != "" && dataGridView1.Rows[x].Cells[1].Value != null)
                        {
                            string skt = dataGridView1.Rows[x].Cells[2].Value.ToString();
                            string[] s = skt.Split('+');
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
                            String sql = "UPDATE command SET command_name='" + dataGridView1.Rows[x].Cells[1].Value.ToString() +
                                    "', shortcut='" + dataGridView1.Rows[x].Cells[2].Value.ToString() +
                                    "', action='" + skt.ToLower() +
                                    "' WHERE command_id= " + dataGridView1.Rows[x].Cells[0].Value;

                            Value.cmd = new MySqlCommand(sql, Value.conn);
                            Value.mdr = Value.cmd.ExecuteReader();
                            Value.mdr.Close();
                            }
                    }
                }
                Value.conn.Close();
                loadTable(null, new EventArgs());
                //MessageBox.Show("Record Updated!");
            }
            else
            {
                Value.conn.Open();
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    int id = Int32.Parse(dataGridView1.SelectedRows[i].Cells[0].Value.ToString());
                    //MessageBox.Show("" + id);
                    String sql = "DELETE FROM command WHERE command_id= " + dataGridView1.SelectedRows[i].Cells[0].Value;// id;
                    Value.cmd = new MySqlCommand(sql, Value.conn);
                    Value.mdr = Value.cmd.ExecuteReader();
                    Value.mdr.Close();
                }
                button6.Visible = false;
                button7.Visible = false;
                loadTable(null, new EventArgs());
            }
        }
        
        private void button7_Click(object sender, EventArgs e)
        {
            button6.Visible = false;
            button7.Visible = false;
            dataGridView1.ReadOnly = true;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            loadTable(null,new EventArgs());
            delete = false;
        }
     
        public void addCommand()
        {
            flowLayoutPanel1.Visible = true;
            dataGridView1.ReadOnly = true;
            panel1.Visible = true;
            panel3.Visible = true;
            button6.Visible = false;
            button7.Visible = false;
            richTextBox1.Visible = false;
        }

        public void editCommand()
        {
            button6.Text = "Save"; 
            if (selectedId == 5)
                dataGridView1.ReadOnly = false;
            else
                dataGridView1.ReadOnly = true;
            panel1.Visible = false;
        }

        public void deleteCommand()
        {
            delete = true;
            button6.Text = "Delete";
            button6.Visible = true;
            button7.Visible = true;
        }

        private void cellChanged(object sender, DataGridViewCellEventArgs e)
        {
            button6.Visible = true;
            button7.Visible = true;
        }

        bool isChecked = false;

        private void radioButton1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked && !isChecked)
            {
                radioButton1.Checked = false;
                button4_Click(null, new EventArgs());
            }
            else
            {
                radioButton1.Checked = true;
                addCommand();
                isChecked = false;
            } 
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            button6.Visible = false;
            button7.Visible = false;
            if (radioButton2.Checked && !isChecked)
            {
                radioButton2.Checked = false;
                button7_Click(null, new EventArgs());
            }
            else
            {
                radioButton2.Checked = true;
                editCommand();
                isChecked = false;
            }
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            button6.Visible = false;
            button7.Visible = false;
            if (radioButton3.Checked && !isChecked)
            {
                radioButton3.Checked = false;
            }
            else
            {
                radioButton3.Checked = true;
                deleteCommand();
                isChecked = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            isChecked = radioButton1.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            isChecked = radioButton2.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            isChecked = radioButton3.Checked;
        }
    }
}
