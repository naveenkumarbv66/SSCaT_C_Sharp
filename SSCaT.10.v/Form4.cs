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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string phno = textBox2.Text;
            string amount = textBox3.Text;
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
                string USERID = null;
                
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
                            USERID = parts[0];
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
                    Class2 ObjectClass2 = new Class2();
                    string UserID = ObjectClass3.generateRandomUserID(5);
                    string paswd = ObjectClass3.generateRandomString(5);
                    string NewRetilerData = UserID + " " + phno + " " + amount + " " + paswd + "\n";

                    string filename = UserID + ".txt";
                    File.AppendAllText(UserID + ".txt", NewRetilerData + " NewACC " + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString());

                    ObjectClass3.AppendDataToFile(NewRetilerData);
                    File.AppendAllText("Reti.txt", UserID+" " + phno + "\n");
                    string NewCustomer = "New SSCaT account successfully created. The UserID to your SSCaT account is " + UserID + ".The password to your SSCaT account is " + paswd + ". Your Current Balance is " + amount + "INR";
                    
                }
                else
                {
                    MessageBox.Show("Alredy have sccat account..!");
                    File.AppendAllText("Reti.txt", USERID+" " + phno + "\n");
                }
            }
            else
            {
                MessageBox.Show("Alredy exits..!");
            }

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 Object = new Form1();
            
        }
    }
}
