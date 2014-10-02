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
    public partial class Form1 : Form
    {
        private SerialPort port;
        private bool Flag = true;
        private bool Status = false;
        private Thread ChildThread = null;
        private bool SettingForm2=false;
        Form3 ObjForm3;
        public string DirName;
        public Form1()
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
                    Console.WriteLine(ex.Message);
                }
            }
            }
        }

        public void CheckSMS()
        {
            while (true)
            {
                Invoke(new CheckSMSDelegate(CheckAnyNewSMS));
                Thread.Sleep(600);
            }
        }

        private void CheckAnyNewSMS()
        {
            Class2 ObjectClass2 = new Class2();
            ObjectClass2.ReadMsg(port,this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            groupBox2.Hide();
            groupBox3.Hide();
            panel1.Hide();
            panel2.Hide();
            panel3.Hide();
            SetComPort();
            button1.Text = "Connect";
            this.WindowState =FormWindowState.Maximized;
            toolStripStatusLabel1.ForeColor = Color.Red;
            toolStripStatusLabel1.Text = "The SSCaT Server Not Connected.";

            FileInfo info = new FileInfo("SSCaTRegister.txt");
            DirName = info.DirectoryName;
            DirectoryInfo dir = new DirectoryInfo(@DirName);
            dir.CreateSubdirectory("SSCaTCustomers");

            
        }

        public void OpenComPort()
        {
            ChildThread = new Thread(new ThreadStart(CheckSMS));
            ChildThread.IsBackground = true;
            ChildThread.Start();
        }

        public void SetComPort()
        {
            /*port = new SerialPort();
            port.PortName = "COM3";
            port.BaudRate = 9600;
            port.DataBits = 8;
            port.StopBits = StopBits.One;
            port.Parity = Parity.None;
            port.ReadTimeout = 300;
            port.WriteTimeout = 300;
            port.Encoding = Encoding.GetEncoding("iso-8859-1");*/
            Form2 ObjectForm2 = new Form2();
            string Comport;
            int baudRate;
            int databit;
            int readtimeout;
            int writetimeout;
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                /*if ((ChildThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                    ChildThread.Abort();*/
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
            if ( ObjForm3!=null)
            {
                if (ObjForm3.IsDisposed == true)
                {
                   // MessageBox.Show("Not Runn..! true...");
                }
                else
                {
                    //MessageBox.Show("Still Runn..!");
                    ObjForm3.Dispose();
                    //MessageBox.Show("Now it Disposed....!");
                }
            }
            else
            {
                //MessageBox.Show("Not Runn..!");
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
            if (port.IsOpen)
            {
                MessageBox.Show("Please Disconnect or close the " + port.PortName + ". ");
            }
            else
            {
                SetComPort();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (port.IsOpen)
            {
                ChildThread.Abort();
                ChildThread = null;
                port.Close();
            }
            this.Dispose();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About ObjAbout = new About();
            ObjAbout.Show();
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
            /*ObjForm3 = new Form3();
            ObjForm3.Show();*/
            System.Diagnostics.Process.Start("Custmo_Information");
        }

        public void Form3SetToNull()
        {
            
        }

        private void createAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;
          /*  button1_Click(sender,e);
            Form4 obj = new Form4();
            obj.Show();*/
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
                    Console.WriteLine(ex.Message);
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
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Class3 Object3 = new Class3();
            string ph = textBox2.Text;
            string amount = textBox3.Text;
            string name = textBox1.Text;

                if(ph==" " || amount==" " || name==" ")
                {
                    MessageBox.Show("Fill all the information correctly");
                }
                else
                {
                    int resl=Object3.CreateReti(ph, amount, port, this);

                    switch (resl)
                    {
                        case 1: //MessageBox.Show("successfully created.....");
                            
                               break;
                        case 2: //MessageBox.Show("");
                              
                               break;
                        case 3://MessageBox.Show("Alredy exits..!");
                              
                               break;

                        default:
                               panel3.Show();
                               pictureBox3.Hide();
                               label9.Hide();
                               pictureBox4.Show();
                               label10.Text = "Error, Try again.....!";
                               label10.Show();
                               break;
                    }
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
                    Console.WriteLine(ex.Message);
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
                    Console.WriteLine(ex.Message);
                }
            }

        }

        private void deleteAdmToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            int count = 1;
            listView2.Items.Clear();
            if (File.Exists("Reti.txt"))
            {
                string line;
                StreamReader reader = new StreamReader("Reti.txt");
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

                MessageBox.Show("Null");

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string phoNo = textBox4.Text;

            Class3 ObjectClass3 = new Class3();
           bool Status= ObjectClass3.RetiSearch(phoNo);
           if (Status)
           {
               MessageBox.Show("Found.");
           }
           else
           {
               MessageBox.Show("Not Found.");
           }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string phoN = textBox5.Text;

            Class3 ObjectClass3 = new Class3();
            bool Statu = ObjectClass3.RetiSearch(phoN);
            if (Statu)
            {
                if (DialogResult.Yes == MessageBox.Show("Are you sure u want to delete " + phoN + " ..???", "Write mez", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    Statu = ObjectClass3.RetiSearchDelte(phoN,port,this);
                    if (Statu)
                    {
                        panel1.Show();
                       
                    }
                    else
                    {
                        panel2.Show();
                    }
                    

                }
                else
                {
                    panel2.Show();
                }
                
            }
            else
            {
               panel2.Show();
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


    }
        public delegate void CheckSMSDelegate();
}
