using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SSCaT._10.v
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
               
        }

        private void button1_Click(object sender, EventArgs e)
        {

            int count = 1;

            if (File.Exists("SSCaTRegister.txt"))
            {
                string line;
                StreamReader reader = new StreamReader("SSCaTRegister.txt");
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');

                    string[] myItems = new string[4];
                    myItems[0] = "None";
                    myItems[1] = "None";
                    myItems[2] = "None";
                    myItems[3] = "None";
                    ListViewItem lvi = new ListViewItem(myItems);
                    listView1.Items.Add(lvi);

                    listView1.Items[listView1.Items.Count - 1].SubItems[0].Text = Convert.ToString(count);
                    listView1.Items[listView1.Items.Count - 1].SubItems[1].Text = parts[0];
                    listView1.Items[listView1.Items.Count - 1].SubItems[2].Text = parts[1];
                    listView1.Items[listView1.Items.Count - 1].SubItems[3].Text = parts[2];
                    count++;

                 
                }
                reader.Close();
            }
            else
            {

                MessageBox.Show("Null");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
                    

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            button4.Hide();
            label2.Hide();
            label3.Hide();
            label4.Hide();
            label5.Hide();
            label6.Hide();
            label7.Hide();
            listView2.Hide();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            label1.Hide();
            textBox1.Hide();
            button3.Hide();


            label2.Show();
            label3.Show();
            label4.Show();
            label5.Show();
            label6.Show();
            label7.Show();
            listView2.Show();
            button4.Show();


            string UserID = textBox1.Text;
            string filename = UserID + ".txt";


            FileInfo info = new FileInfo("SSCaTRegister.txt");
            string DirName = info.DirectoryName;
            DirectoryInfo dir = new DirectoryInfo(@DirName);
            dir.CreateSubdirectory("SSCaTCustomers");
            string path = DirName + "\\SSCaTFolder\\" + filename;

            if (File.Exists(path))
            {
                string line;


                StreamReader reader = new StreamReader(path);
                line = reader.ReadLine();
                string[] parts = line.Split(' ');
                label3.Text = UserID;
                label5.Text = parts[1];
                label7.Text = parts[3];
                label10.Text = parts[2];

                while ((line) != null)
                {

                    line = reader.ReadLine();
                    //parts = line.Split(' ');
                }
                reader.Close();
            }
            else
            {



            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Hide();
            label2.Hide();
            label3.Hide();
            label4.Hide();
            label5.Hide();
            label6.Hide();
            label7.Hide();
            listView2.Hide();

            label1.Show();
            textBox1.Clear();
            textBox1.Show();
            button3.Show();


            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string phno = textBox3.Text;
            string amount = textBox4.Text;
            bool flag = true;

            if (File.Exists("Reti.txt"))
            {
                string line;
                StreamReader reader = new StreamReader("Reti.txt");
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == phno)
                    {
                        reader.Close();
                        flag = false;
                    }
                }
                reader.Close();
            }
            else
            {
                flag = true;

            }

            if (flag)
            {
                File.AppendAllText("Reti.txt", phno+"\n");
                Class4 ObjectClass4 = new Class4();

                if (File.Exists("SSCaTRegister.txt"))
                {
                    flag = false;
                    string line;
                    StreamReader reader = new StreamReader("SSCaTRegister.txt");
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(' ');
                        if (phno == parts[1])
                        {
                            flag = false;
                        }
                    }
                
                }
                else
                {
                    flag = true;
                }



                if (flag)
                {

                    Class3 ObjectClass3 = new Class3();
                    string UserID = ObjectClass3.generateRandomUserID(5);
                    string paswd = ObjectClass3.generateRandomString(5);
                    string NewRetilerData = UserID + " " + phno + " " + amount + " " + paswd + "\n";

                    string filename = UserID + ".txt";
                    File.AppendAllText(UserID + ".txt", NewRetilerData + " NewACC " + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString());

                    ObjectClass3.AppendDataToFile(NewRetilerData);
                    string NewCustomer = "New SSCaT account successfully created. The UserID to your SSCaT account is " + UserID + ".The password to your SSCaT account is " + paswd + ". Your Current Balance is " + amount + "INR";
                }
                else
                {
                    MessageBox.Show("Alredy have sccat account..!");
                }
            }
            else
            {
                MessageBox.Show("Alredy exits..!");
            }


        }
    }
}
