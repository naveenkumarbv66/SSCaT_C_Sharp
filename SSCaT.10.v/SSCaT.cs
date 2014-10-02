using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace SSCaT._10.v
{
    public partial class SSCaT : Form
    {
        private SerialPort port;
        private bool Flag = true;
        private bool Status = false;
        private Thread ChildThread = null;
        private bool SettingForm2=false;
        private bool DeleteCustomerAccount = true;
        public SSCaT()
        {
            InitializeComponent();

           
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if (SettingForm2 == false) SetComPort();
            if(SettingForm2 ==true)
            {
            if (Flag )
            {
                
                try
                {
                     port.Open();
                     toolStripStatusLabel1.ForeColor = Color.Green;
                     toolStripStatusLabel1.Text = "The SSCaT Server Connected to " + port.PortName;
                    button1.Text = "Disconnect";
                    Flag = false;
                    OpenComPort();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    ChildThread.Abort();
                    ChildThread = null;
                    port.Close();
                    button1.Text = "Connect";
                    toolStripStatusLabel1.ForeColor = Color.Red;
                    toolStripStatusLabel1.Text = "The SSCaT Server Disconnected";
                    Flag = true;
                    
                }
                catch (Exception ex)
                {
                    
                }
            }
            }
        }

        public void CheckSMS()
        {
            try
            {
                while (true)
                {
                    Invoke(new CheckSMSDelegate(CheckAnyNewSMS));
                    Thread.Sleep(600);
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        private void CheckAnyNewSMS()
        {
            try
            {
                Class2 ObjectClass2 = new Class2();
                ObjectClass2.ReadMsg(port, this);
            }
            catch (Exception ex)
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                groupBox2.Hide();
                groupBox3.Hide();
                panel1.Hide();
                panel2.Hide();
                panel3.Hide();
                SetComPort();
                button1.Text = "Connect";
                this.WindowState = FormWindowState.Maximized;
                toolStripStatusLabel1.ForeColor = Color.Red;
                toolStripStatusLabel1.Text = "The SSCaT Server Not Connected.";
            }
            catch (Exception ex)
            {

            }
        }

        public void OpenComPort()
        {
            try
            {
                ChildThread = new Thread(new ThreadStart(CheckSMS));
                ChildThread.IsBackground = true;
                ChildThread.Start();
            }
            catch (Exception ex)
            {

            }
        }

        public void SetComPort()
        {
            SSCaTSettings ObjectForm2 = new SSCaTSettings();
            string Comport;
            int baudRate;
            int databit;
            int readtimeout;
            int writetimeout;
            try
            {
                if (ObjectForm2.ShowDialog(this) == DialogResult.OK)
                {
                    ObjectForm2.GetData(out Comport, out baudRate, out databit, out readtimeout, out writetimeout);
                    port = new SerialPort();
                    port.PortName = Comport;
                    port.BaudRate = baudRate;
                    port.DataBits = databit;
                    port.StopBits = StopBits.One;
                    port.Parity = Parity.None;
                    port.ReadTimeout = readtimeout;
                    port.WriteTimeout = writetimeout;
                    port.Encoding = Encoding.GetEncoding("iso-8859-1");
                    SettingForm2 = true;
                }
                else
                {
                    SettingForm2 = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (port.IsOpen)
                {
                    ChildThread.Abort();
                    ChildThread = null;
                    port.Close();
                }
                    
            }
            catch
            {
            }
            string machinename = "";
            machinename = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            MessageBox.Show("Thanks " + machinename + " to using SSCaT server.\nYour feedback is important for me so keep in touch with me.\nEMail-ID: naveenkumarbv66@gmail.com", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            
        }

        private void portSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (port.IsOpen)
                {
                    MessageBox.Show("Please Disconnect or close the " + port.PortName + ". ");
                }
                else
                {
                    SetComPort();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (port.IsOpen)
                {
                    ChildThread.Abort();
                    ChildThread = null;
                    port.Close();
                }

                this.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("AboutSSCaT");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Try later.", "Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void fullSSCaTCustomerDetilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("Custmo_Information");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Try later.", "Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void Form3SetToNull()
        {
            
        }

        private void createAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox3.Hide();
            Status = false;
            try
            {
                if (port.IsOpen)
                {
                    ChildThread.Abort();
                    ChildThread = null;
                    port.Close();
                    Status = true;
                }
                
            }
            catch (Exception ex)
            {
               
            }
            try
            {
                port.Open();
                toolStripStatusLabel1.ForeColor = Color.Green;
                toolStripStatusLabel1.Text = "The Port is connected to " + port.PortName;
            }
            catch (Exception ex)
            {
                
            }

            groupBox1.Hide();
            panel3.Hide();
            groupBox2.Show();

            
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            groupBox2.Hide();
            groupBox1.Show();
            if (Status)
            {
                try
                {
                    toolStripStatusLabel1.ForeColor = Color.Green;
                    toolStripStatusLabel1.Text = "The SSCaT Server Connected to " + port.PortName;
                    button1.Text = "Disconnect";
                    OpenComPort();
                }
                catch (Exception ex)
                {
                    
                }
            }
            else
            {
                try
                {
                    port.Close();
                    button1.Text = "Connect";
                    toolStripStatusLabel1.ForeColor = Color.Red;
                    toolStripStatusLabel1.Text = "The SSCaT Server Disconnected";
                }
                catch (Exception ex)
                {
                   
                }
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Class5 Object5 = new Class5();
            try
            {
                string PhoneNumber = textBox2.Text;
                string Amount = textBox3.Text;
                string Name = textBox1.Text;

                if (PhoneNumber == " " || Amount == " " || Name == " ")
                {
                    MessageBox.Show("Fill all the information correctly");
                }
                else
                {
                    int Option = Object5.CreateRetailerAccount(PhoneNumber, Amount, port, this);

                    switch (Option)
                    {
                        case 1:
                            break;
                        case 2: 
                            break;
                        case 3:
                            break;
                        default:
                            panel3.Show();
                            pictureBox3.Hide();
                            label9.Hide();
                            pictureBox4.Show();
                            label10.Text = "Error : Try again.....!";
                            label10.Show();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            groupBox3.Hide();
            groupBox1.Show();
            if (Status)
            {
                try
                {
                    toolStripStatusLabel1.ForeColor = Color.Green;
                    toolStripStatusLabel1.Text = "The SSCaT Server Connected to " + port.PortName;
                    button1.Text = "Disconnect";
                    OpenComPort();
                }
                catch (Exception ex)
                {
                    
                }
            }
            else
            {
                try
                {
                    port.Close();
                    button1.Text = "Connect";
                    toolStripStatusLabel1.ForeColor = Color.Red;
                    toolStripStatusLabel1.Text = "The SSCaT Server Disconnected";
                }
                catch (Exception ex)
                {
                    
                }
            }

        }

        private void deleteAdmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            int count = 1;
            string FileName = null;
            listView2.Items.Clear();
            if(DeleteCustomerAccount == true)
            {
                FileName = "Retailer.txt";
            }else
            {
                FileName = "SSCaTRegister.txt";
            }


            try
            {
                if (File.Exists(FileName))
                {
                    string line;
                    StreamReader reader = new StreamReader(FileName);
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(' ');
                        if (parts[0].Length == 5)
                        {
                            string[] myItems = new string[3];
                            myItems[0] = "None";
                            myItems[1] = "None";
                            myItems[2] = "None";

                            ListViewItem lvi = new ListViewItem(myItems);
                            listView2.Items.Add(lvi);

                            listView2.Items[listView2.Items.Count - 1].SubItems[0].Text = Convert.ToString(count);
                            listView2.Items[listView2.Items.Count - 1].SubItems[1].Text = parts[0];
                            listView2.Items[listView2.Items.Count - 1].SubItems[2].Text = parts[1];

                            count++;
                        }


                    }
                    reader.Close();
                }
                else
                {

                    MessageBox.Show("No retailer customer found.","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Class3 ObjectClass3 = new Class3();

            try
            {
                string PhoneNumber = textBox4.Text;
                String[] CustomerInforamtion = ObjectClass3.ReadDataFromCustomerRetailerFile(PhoneNumber, "Retailer.txt");
                if (CustomerInforamtion!=null)
                {
                    MessageBox.Show("The pnone number " + CustomerInforamtion [1]+ " and UsrerID : "+CustomerInforamtion [0]+" Found.", "Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("The pnone number " + PhoneNumber + " not Found.", "Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Class3 ObjectClass3 = new Class3();
            string FileName = null;
            if (DeleteCustomerAccount == true)
            {
                FileName = "Retailer.txt";
            }
            else
            {
                FileName = "SSCaTRegister.txt";
            }
            try
            {
                string PhoneNumber= textBox5.Text;
                bool Statu = ObjectClass3.SearchCustomerRetailer(PhoneNumber, FileName);
                if (Statu)
                {
                    if (DialogResult.Yes == MessageBox.Show("Are you sure you want to delete " + PhoneNumber + " ..???", "Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        Statu = ObjectClass3.RetailerAndCustomerSearchAndDelte(PhoneNumber, FileName, port, this);
                        if (Statu)
                        {
                            label7.Text = "successfully Deleted..!";
                            panel1.Show();

                        }
                        else
                        {
                            label8.Text = "Error, While deleting..!";
                            panel2.Show();
                        }


                    }
                    else
                    {
                        label8.Text = "Error, While deleting..!";
                        panel2.Show();
                    }

                }
                else
                {
                    label8.Text = "Error, While deleting..!";
                    panel2.Show();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel1.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            panel2.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            panel3.Hide();
        }

        private void WindowForDeleteAccount()
        {
            try
            {
                if (port.IsOpen)
                {
                    ChildThread.Abort();
                    ChildThread = null;
                    port.Close();
                    Status = true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                port.Open();
                toolStripStatusLabel1.ForeColor = Color.Green;
                toolStripStatusLabel1.Text = "The Port is connected to " + port.PortName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            groupBox2.Hide();
            groupBox1.Hide();
            groupBox3.Show();
        }

        private void retailerAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label4.Text = "Retailer Details";
            DeleteCustomerAccount = true;
            button7_Click(sender, e);
            WindowForDeleteAccount();
        }

        private void sSCaTAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label4.Text = "SSCaT Customers Details";
            DeleteCustomerAccount = false;
            button7_Click(sender, e);
            WindowForDeleteAccount();
        }


    }
        public delegate void CheckSMSDelegate();
}
