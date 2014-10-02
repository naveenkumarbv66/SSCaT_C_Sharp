using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace SSCaT._10.v
{
    class SendReadDeleteMessage
    {

        public void ReadMsg(SerialPort Port,SSCaTMainServer form1){
           try{
                ExecuteCommands ObjectClass1 = new ExecuteCommands();
                ReadSearchIdentify ObjectClass3 = new ReadSearchIdentify();

                ObjectClass1.receiveNow = new AutoResetEvent(false);
                Port.DataReceived += new SerialDataReceivedEventHandler(ObjectClass1.port_DataReceived);

                string RecievedData = ObjectClass1.ExecCommand(Port, "AT", 500);
                RecievedData = ObjectClass1.ExecCommand(Port, "AT+CMGF=1", 500);
                String Command = "AT+CMGL=\"REC UNREAD\"";
                RecievedData = ObjectClass1.ExecCommand(Port, Command, 500); 
                            

                Regex r = new Regex(@"\+CMGL: (\d+),""(.+)"",""(.+)"",(.*),""(.+)""\r\n(.+)\r\n");
                Match m = r.Match(RecievedData);

               while (m.Success){
                   
                    string IndexValue  = m.Groups[1].Value; //Index Value 
                    string Type = m.Groups[2].Value; //Type
                    string SenderNumber = m.Groups[3].Value; //Sender Number 
                    string SenderName = m.Groups[4].Value; //Sender Name
                    string DateTime = m.Groups[5].Value; //Date and time
                    string Message = m.Groups[6].Value; //Messg

                    string[] MyItems = new string[7];
                    MyItems[0] = "None";
                    MyItems[1] = "None";
                    MyItems[2] = "None";
                    MyItems[3] = "None";
                    MyItems[4] = "None";
                    MyItems[5] = "None";
                    MyItems[6] = "None";
                    ListViewItem lvi = new ListViewItem(MyItems);
                    form1.listView1.Items.Add(lvi);


                    form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[0].Text = SenderNumber;
                    form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[1].Text = Message;

                    int Option = ObjectClass3.Identify(Message, SenderNumber, form1);
                    ObjectClass3.MessageType(Option, Message, SenderNumber, Port, form1);
                    DeleteMsg(Port, form1);
                    
                    m = m.NextMatch();
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        public void sendMsg(SerialPort Port, string PhoneNo, string Message, SSCaTMainServer form1,bool Index,int Count){
           try{
                ExecuteCommands Object = new ExecuteCommands();

                if (PhoneNo.Length == 10) PhoneNo = PhoneNo.Insert(0, "+91");

                Object.receiveNow = new AutoResetEvent(false);
                Port.DataReceived += new SerialDataReceivedEventHandler(Object.port_DataReceived);

                string RecievedData = Object.ExecCommand(Port, "AT", 300);
                RecievedData = Object.ExecCommand(Port, "AT+CMGF=1", 300);
                String Command = "AT+CMGS=\"" + PhoneNo + "\"";
                RecievedData = Object.ExecCommand(Port, Command, 500); 
                Command = Message + char.ConvertFromUtf32(26) + "\r";
                RecievedData = Object.ExecCommand(Port, Command, 4000); 
               
               if (RecievedData.Contains("ERROR")){
                    if (Index==true) {
                        form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[4].Text = "Error while sending Message to : " + PhoneNo;
                      }
                    else{
                        form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[5].Text = "Error while sending Message to : " + PhoneNo;
                    }  
                }

               else if (RecievedData.Contains("OK")){
                   if (Index == true)
                   {
                       form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[4].Text = "Message has been sent to : " + PhoneNo;
                   }
                   else
                   {
                       form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[5].Text = "Message has been sent to : " + PhoneNo;
                   }
               }
               else if (Count==0)
               {
                   sendMsg(Port, PhoneNo, Message, form1, Index,1);
               }
            }
            catch (Exception ex) {
                if (Index == true)
                {
                    form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[4].Text = "Error while sending Message to : " + PhoneNo;
                }
                else
                {
                    form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[5].Text = "Error while sending Message to : " + PhoneNo;
                }  
            }
        }

        public void DeleteMsg(SerialPort Port, SSCaTMainServer form1)
        {

            string recievedData = "empty";
            try
            {
                ExecuteCommands ObjectClass1 = new ExecuteCommands();

                ObjectClass1.receiveNow = new AutoResetEvent(false);
                Port.DataReceived += new SerialDataReceivedEventHandler(ObjectClass1.port_DataReceived);

                recievedData = ObjectClass1.ExecCommand(Port, "AT", 300);
                recievedData = ObjectClass1.ExecCommand(Port, "AT+CMGD=1,3", 500);

                form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[6].Text = "Message has been deleted.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


    }
}
