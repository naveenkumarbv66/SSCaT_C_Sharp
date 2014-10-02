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
    class ReadSearchIdentify
    {
        public int Identify(string Message,string SenderNumber, SSCaTMainServer form1)
        {
            string NewAcc = "^NEW [+91]*[0]*\\d+ (([0-9]+.[0-9]*)|([0-9]*.[0-9]+)|([0-9]+)) [a-z0-9]+[ ]*$";
            string Recharge = "^RCHRG [A-Z][a-z0-9]+ (([0-9]+.[0-9]*)|([0-9]*.[0-9]+)|([0-9]+)) [a-z0-9]+[ ]*$";
            string Transaction = "^[A-Z][a-z0-9]+ (([0-9]+.[0-9]*)|([0-9]*.[0-9]+)|([0-9]+)) [a-z0-9]+[ ]*$";
            string Balance = "^BAL[ ]*$";
            string Delete = "^DEL [a-z0-9]+[ ]*$";

            try
            {
                ReadSearchIdentify ObjectClass3 = new ReadSearchIdentify();
                string[] CustomerDetails = ObjectClass3.ReadDataFromCustomerRetailerFile(SenderNumber, "SSCaTRegister.txt");
                if (CustomerDetails == null)
                {
                    form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[2].Text = "Invalid SSCaT Operation";
                    return 6;
                }


                Match m = Regex.Match(Message, NewAcc, RegexOptions.CultureInvariant);
                if (m.Success)
                {
                     form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[2].Text = "New Acc";
                    return 1;
                }

                m = Regex.Match(Message, Recharge, RegexOptions.CultureInvariant);
                if (m.Success)
                {
                     form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[2].Text = "Recharge";
                    return 2;
                }

                m = Regex.Match(Message, Transaction, RegexOptions.CultureInvariant);
                if (m.Success)
                {
                    form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[2].Text = "Transaction";
                    return 3;
                }

                m = Regex.Match(Message, Balance, RegexOptions.CultureInvariant);
                if (m.Success)
                {
                     form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[2].Text = "Balance";
                    return 4;
                }
                m = Regex.Match(Message, Delete, RegexOptions.CultureInvariant);
                if (m.Success)
                {
                    form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[2].Text = "Delete";
                    return 5;
                }

                  form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[2].Text = "Invalid SSCaT Operation";
                return 0;
            }
            catch (Exception ex)
            {
                 form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[2].Text = "Invalid SSCaT Operation";
                return 0;
            }
        }

        public void MessageType(int Option, string Message, string CustomerNumber, SerialPort Port, SSCaTMainServer form1)
        {
            SSCaTOperations ObjectClass4 = new SSCaTOperations();
            SendReadDeleteMessage ObjectClass2 = new SendReadDeleteMessage();

            try
            {
                switch (Option)
                {
                    case 0:
                        form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[3].Text = "None";
                        form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[5].Text = "None";
                        ObjectClass2.sendMsg(Port, CustomerNumber, "Invalid SSCaT Operation.", form1, true, 0);
                        break;
                    case 1:
                        ObjectClass4.NewAccount(Message, CustomerNumber, Port, form1);
                        break;
                    case 2:
                        ObjectClass4.RechargeAccount(Message, CustomerNumber, Port, form1);
                        break;
                    case 3:
                        CustomerNumber = CustomerNumber.Remove(0, 3);
                        ObjectClass4.Transaction(Message, CustomerNumber, Port, form1);
                        break;
                    case 4:
                        CustomerNumber = CustomerNumber.Remove(0, 3);
                        ObjectClass4.Balance(CustomerNumber, Port, form1);
                        break;
                    case 5:
                        ObjectClass4.DeleteSSCaTAccountFromMessage(Message, CustomerNumber, Port,form1 );
                        break;
                    case 6:
                        form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[3].Text = "None";
                        form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[5].Text = "None";
                        ObjectClass2.sendMsg(Port, CustomerNumber, "You don't have SSCaT Account to do this Operation.", form1, true, 0);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public string[] ReadDataFromCustomerRetailerFile(string PhoneNumber_UserID, string FileName)
        {
            try
            {
                if (File.Exists(FileName))
                {
                    string Line;
                    string[] UserID_PhoneNumber = PhoneNumber_UserID.Split(' ');
                    StreamReader Reader = new StreamReader(FileName);


                    while ((Line = Reader.ReadLine()) != null)
                    {
                        string[] Parts = Line.Split(' ');
                        if ((Parts.Length == 4) || (Parts.Length == 2))
                        {
                            string PhoneNumber91 = Parts[1].Insert(0, "+91");
                            string PhoneNumber0 = Parts[1].Insert(0, "0");
                            if ((UserID_PhoneNumber[0] == Parts[1]) || (PhoneNumber91 == UserID_PhoneNumber[0]) || (PhoneNumber0 == UserID_PhoneNumber[0]))
                            {
                                Reader.Close();
                                return Parts;
                            }
                            if (UserID_PhoneNumber[0] == Parts[0])
                            {
                                Reader.Close();
                                return Parts;
                            }
                        }
                    }

                    Reader.Close();
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool SearchCustomerRetailer(string PhoneNumber_UserID, String FileName)
        {
            if (File.Exists(FileName))
            {
                string Line;
                StreamReader Reader = new StreamReader(FileName);
                try
                {
                    while ((Line = Reader.ReadLine()) != null)
                    {
                        string[] Parts = Line.Split(' ');
                        if ((Parts.Length == 4) || (Parts.Length == 2))
                        {

                            string PhoneNumber91 = Parts[1].Insert(0, "+91");
                            string PhoneNumber0 = Parts[1].Insert(0, "0");
                            if ((Parts[1] == PhoneNumber_UserID) || (PhoneNumber91 == PhoneNumber_UserID) || (PhoneNumber0 == PhoneNumber_UserID))
                            {
                                Reader.Close();
                                return true;
                            }
                            if (PhoneNumber_UserID == Parts[0])
                            {
                                Reader.Close();
                                return true;
                            }
                        }
                    }
                    Reader.Close();
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
            else
            {
                return false;
            }

        }

        public int RetailerAndCustomerSearchAndDelte(string PhoneNumber_UserID, string FileName,  SSCaTMainServer form)
        {
             
            try
            {
                if (File.Exists(FileName))
                {
                    string Line;
                    StreamReader Reader = new StreamReader(FileName);
                    while ((Line = Reader.ReadLine()) != null)
                    {
                        string[] Parts = Line.Split(' ');
                        if ((Parts.Length == 2) || (Parts.Length == 4))
                        {
                            string PhoneNumber91 = Parts[1].Insert(0, "+91");
                            string PhoneNumber0 = Parts[1].Insert(0, "0");

                            if (((Parts[0] == PhoneNumber_UserID) || Parts[1] == PhoneNumber_UserID) || (PhoneNumber91 == PhoneNumber_UserID) || (PhoneNumber0 == PhoneNumber_UserID))
                            {
                                Reader.Close();

                                String StrFile = File.ReadAllText(FileName);
                                StrFile = StrFile.Replace(Line, "");
                                File.WriteAllText(FileName, StrFile);

                                form.TempMemory[0] = Parts[0];
                                form.TempMemory[1] = PhoneNumber91;
                                return 1;
                            }
                        }

                    }
                    Reader.Close();
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
