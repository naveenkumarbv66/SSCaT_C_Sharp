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
    public partial class SSCaTMainServer : Form
    {
        private SerialPort port;
        private bool Flag = true;
        private bool Status = false;
        private Thread ChildThread = null;
        private bool SettingForm2=false;
        private bool DeleteCustomerAccount = true;
        public string[] TempMemory=new string[4];

        public SSCaTMainServer()
        {
            InitializeComponent(); 
        }

        private void Form1_Load(object sender, EventArgs e) // Load initial required ments
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
                toolStripStatusLabel1.Text = "The SSCaT server not connected.";
            }
            catch (Exception ex)
            {
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) // Clean up all opreation before closing window
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
            catch (Exception ex)
            {   
            }
            string machinename = "";
            machinename = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            MessageBox.Show("Thanks " + machinename + " to using SSCaT server.\nYour feedback is important for me so keep in touch with me.\nEMail-ID: naveenkumarbv66@gmail.com", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void button1_Click(object sender, EventArgs e)// To connect or disconnect server
        {
            if (SettingForm2 == false) SetComPort();
            if (SettingForm2 == true)
            {
                if (Flag)
                {

                    try
                    {
                        port.Open();
                        toolStripStatusLabel1.ForeColor = Color.Green;
                        toolStripStatusLabel1.Text = "The SSCaT server connected to " + port.PortName;
                        button1.Text = "Disconnect";
                        Flag = false;
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
                        ChildThread.Abort();
                        ChildThread = null;
                        port.Close();
                        button1.Text = "Connect";
                        toolStripStatusLabel1.ForeColor = Color.Red;
                        toolStripStatusLabel1.Text = "The SSCaT server disconnected";
                        Flag = true;

                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private void CheckAnyNewSMS()// Read new message
        {
            try
            {
                SendReadDeleteMessage ObjectClass2 = new SendReadDeleteMessage();
                ObjectClass2.ReadMsg(port, this);
            }
            catch (Exception ex)
            {
            }
        }

        public void CheckSMS()// To invoke every 600 mil sc for new message
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

        public void OpenComPort() //create thread to read new mesg
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

        public void SetComPort() // Set com port values
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

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void portSettingsToolStripMenuItem_Click(object sender, EventArgs e) //Check whethere alredy connected or not before connecting com port
        {
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) // close window
        {
            
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) // Call About.exe
        {
            
        }

        private void fullSSCaTCustomerDetilesToolStripMenuItem_Click(object sender, EventArgs e) //Call Custmo_Information.exe
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

        private void createAdminToolStripMenuItem_Click(object sender, EventArgs e) // Display Group2 option tool
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
            catch (Exception ex) {    }
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

        private void button3_Click_2(object sender, EventArgs e) //Display Group1 tool
        {
            groupBox2.Hide();
            groupBox1.Show();
            if (Status)
            {
                try
                {
                    toolStripStatusLabel1.ForeColor = Color.Green;
                    toolStripStatusLabel1.Text = "The SSCaT server connected to " + port.PortName;
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
                    toolStripStatusLabel1.Text = "The SSCaT server is disconnected";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void button4_Click_1(object sender, EventArgs e) //Create Retailer account
        {
            CreateUserIdPasswordRetailerAcc Object5 = new CreateUserIdPasswordRetailerAcc();
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
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void button5_Click(object sender, EventArgs e) //Display Group1 tool
        {
            groupBox3.Hide();
            groupBox1.Show();
            if (Status)
            {
                try
                {
                    toolStripStatusLabel1.ForeColor = Color.Green;
                    toolStripStatusLabel1.Text = "The SSCaT server connected to " + port.PortName;
                    button1.Text = "Disconnect";
                    OpenComPort();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                try
                {
                    port.Close();
                    button1.Text = "Connect";
                    toolStripStatusLabel1.ForeColor = Color.Red;
                    toolStripStatusLabel1.Text = "The SSCaT server is disconnected";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

        }

        private void button7_Click(object sender, EventArgs e) // Diplay all customer inforamtion
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

                    MessageBox.Show("No retailer details found.","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void button6_Click(object sender, EventArgs e) // Search
        {
            ReadSearchIdentify ObjectClass3 = new ReadSearchIdentify();

            try
            {
                string PhoneNumber = textBox4.Text;
                String[] CustomerInforamtion = ObjectClass3.ReadDataFromCustomerRetailerFile(PhoneNumber, "Retailer.txt");
                if (CustomerInforamtion!=null)
                {
                    MessageBox.Show("The phone number " + CustomerInforamtion [1]+ " and UsrerID : "+CustomerInforamtion [0]+" Found.", "Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("The phone number " + PhoneNumber + " not Found.", "Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button8_Click(object sender, EventArgs e) // Delete customer
        {
            ReadSearchIdentify ObjectClass3 = new ReadSearchIdentify();
            string FileName = null;
            bool Temp;
            string NewCustomer;
            SendReadDeleteMessage ObjectClass2 = new SendReadDeleteMessage();
                               
            if (DeleteCustomerAccount == true)
            {
                FileName = "Retailer.txt";
                Temp = false;
            }
            else
            {
                FileName = "SSCaTRegister.txt";
                Temp = true;
            }
            try
            {
                string PhoneNumber = textBox5.Text;
                bool Statu = ObjectClass3.SearchCustomerRetailer(PhoneNumber, FileName);
                if (Statu)
                {
                    if (DialogResult.Yes == MessageBox.Show("Are you sure you want to delete " + PhoneNumber + " ..???", "Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        int Option = ObjectClass3.RetailerAndCustomerSearchAndDelte(PhoneNumber, FileName, this);
                        switch (Option)
                        {
                            case 1:
                                if (Temp)
                                {
                                    FileName = "Retailer.txt";
                                    Temp = false;
                                    Option = ObjectClass3.RetailerAndCustomerSearchAndDelte(PhoneNumber, FileName, this);
                                    switch (Option)
                                    {
                                        case 1://both
                                            NewCustomer = "SSCaT and ROP account UserID " + TempMemory[0] + " with phone number " + TempMemory[1] + " Successfully Deleted.";
                                            label7.Text = "Successfully Deleted.";
                                           
                                            panel1.Show();
                                            ObjectClass2.sendMsg(port, TempMemory[1], NewCustomer, this, true, 0);
                                            break;
                                        case 0:
                                            NewCustomer = "SSCaT UserID " + TempMemory[0] + " with phone number " + TempMemory[1] + " Successfully Deleted.";
                                            string FileDelete = TempMemory[0] + ".txt";
                                            File.Delete(FileDelete);
                                            label7.Text = "Successfully Deleted.";
                                            
                                            panel1.Show();
                                            ObjectClass2.sendMsg(port, TempMemory[1], NewCustomer, this, true, 0);
                                            break;
                                    }
                                }
                                else
                                {
                                    NewCustomer = "ROP account UserID " + TempMemory[0] + " with phone number " + TempMemory[1] + " Successfully Deleted.";
                                    label7.Text = "Successfully Deleted.";
                                    
                                    panel1.Show();
                                    ObjectClass2.sendMsg(port, TempMemory[1], NewCustomer, this, true, 0);
                                }
                                break;
                            case 0://error
                                label8.Text = "Invaild UserId/Phone number " + PhoneNumber + ".";
                                panel2.Show();
                                break;
                        }
                    }
                }
                else
                {
                    label8.Text = "Invaild UserId/Phone number " + PhoneNumber + ".";
                    panel2.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
                                     
        private void button9_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            button7_Click(sender, e);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            panel2.Hide();
            button7_Click(sender, e);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            panel3.Hide();
        }

        private void WindowForDeleteAccount() //Display Group3 tool
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

            groupBox2.Hide();
            groupBox1.Hide();
            groupBox3.Show();
        }

        private void retailerAccountToolStripMenuItem_Click(object sender, EventArgs e) //Option for Retailer account to delete
        {
            groupBox3.Text = "Delete Retailer Account";
            label4.Text = "Retailer Details";
            DeleteCustomerAccount = true;
            button7_Click(sender, e);
            WindowForDeleteAccount();
        }

        private void sSCaTAccountToolStripMenuItem_Click(object sender, EventArgs e) //Option for SSCaT account to delete
        {
            groupBox3.Text = "Delete SSCaT Subscriber Account";
            label4.Text = "SSCaT Subscriber Details";
            DeleteCustomerAccount = false;
            button7_Click(sender, e);
            WindowForDeleteAccount();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        public void Form3SetToNull()
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void deleteAdmToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {


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

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void naveenKumarBVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/Naveenkumar.BV");
        }

        private void vivekBGToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
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

        private void comPortSettingsToolStripMenuItem_Click(object sender, EventArgs e)
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
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
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

        private void runSSCaTServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void sumithaPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/sumi.tha.5");
        }

        private void umaRaniMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.facebook.com/umarani.m");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("Small_Scale_Cashless_Transaction.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Try later.", "Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void vivekBGToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            System.Diagnostics.Process.Start("https://facebook.com/vivekvjsd");
        }

        private void gitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/naveenkumarbv66");
        }

        private void bitbucketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://bitbucket.org/naveenkumarbv66");
        }

        private void linkedInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/Naveenkumar.BV");
        }

    }
        public delegate void CheckSMSDelegate();
}
