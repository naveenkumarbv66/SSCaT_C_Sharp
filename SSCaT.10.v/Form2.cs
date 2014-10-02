using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace SSCaT._10.v
{
    public partial class Form2 : Form
    {
        private string port;
        private int baudRate;
        private int databit;
        private int readtimeout;
        private int writetimeout;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string[] TotalPorts = SerialPort.GetPortNames();

            foreach (string port in TotalPorts)
            {
                this.comboBox1.Items.Add(port);
            }
            try
            {
                this.comboBox1.Text = TotalPorts[0];
            }
            catch
            {
                MessageBox.Show("No COM ports in the system");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.port = comboBox1.Text;
                this.baudRate = int.Parse(comboBox2.Text);
                this.databit = int.Parse(comboBox3.Text);
                this.readtimeout = int.Parse(comboBox4.Text);
                this.writetimeout = int.Parse(comboBox5.Text);
            }
            catch
            {
                MessageBox.Show("Select COM port");
            }
        }


        public void GetData(out string port, out int baudRate, out int databit, out int readtimeout, out int writetimeout)
        {
            port = this.port;
            baudRate = this.baudRate;
            databit = this.databit;
            readtimeout = this.readtimeout;
            writetimeout = this.writetimeout;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

    }
}
