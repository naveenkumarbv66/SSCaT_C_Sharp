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
    public partial class CreateRechargeOutletPerson : Form
    {
        public CreateRechargeOutletPerson()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string PhoneNumber = textBox2.Text;
                string Amount = textBox3.Text;
                bool flag = true;

                if (File.Exists("Reti.txt"))
                {
                    string line;
                    StreamReader reader = new StreamReader("Reti.txt");
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == PhoneNumber)
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
                            if (PhoneNumber == parts[1])
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
                        string UserID = ObjectClass3.GenerateRandomUserID(5);
                        string paswd = ObjectClass3.GenerateRandomString(5);
                        string NewRetilerData = UserID + " " + PhoneNumber + " " + Amount + " " + paswd + "\n";

                        string filename = UserID + ".txt";
                        File.AppendAllText(UserID + ".txt", NewRetilerData + " NewACC " + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString());

                        ObjectClass3.AppendDataToFile(NewRetilerData);
                        File.AppendAllText("Reti.txt", UserID + " " + PhoneNumber + "\n");
                        string NewCustomer = "New SSCaT account successfully created. The UserID to your SSCaT account is " + UserID + ".The password to your SSCaT account is " + paswd + ". Your Current Balance is " + Amount + "INR";

                    }
                    else
                    {
                        MessageBox.Show("Alredy have sccat account..!");
                        File.AppendAllText("Reti.txt", USERID + " " + PhoneNumber + "\n");
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
            catch (Exception ex)
            {
            }
        
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
            try
            {
                SSCaT Object = new SSCaT();
            }
            catch (Exception ex)
            {
            }
            
        }
    }
}
