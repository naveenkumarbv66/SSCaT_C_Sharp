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
    class Class3
    {
        public int Identify(string Message, SSCaT form1)
        {
            string NewAcc = "^NEW ACC [+91]*[0]*\\d+ (([0-9]+.[0-9]*)|([0-9]*.[0-9]+)|([0-9]+))$";
            string Recharge = "^RCHRG ACC [A-Z][a-z0-9]+ (([0-9]+.[0-9]*)|([0-9]*.[0-9]+)|([0-9]+))$";
            string Transaction = "^[A-Z][a-z0-9]+ (([0-9]+.[0-9]*)|([0-9]*.[0-9]+)|([0-9]+)) [a-z0-9]+$";
            string Balance = "^BAL$";

            try
            {

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

                  form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[2].Text = "Invalied SSCaT Opreation";
                return 0;
            }
            catch (Exception ex)
            {
                 form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[2].Text = "Invalied SSCaT Opreation";
                return 0;
            }
        }

        public void MessageType(int Option, string Message, string CustomerNumber, SerialPort Port, SSCaT form1)
        {
            Class4 ObjectClass4 = new Class4();
            Class2 ObjectClass2 = new Class2();

            try
            {
                switch (Option)
                {
                    case 0:
                        form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[3].Text = "None";
                        form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[5].Text = "None";
                        ObjectClass2.sendMsg(Port, CustomerNumber, "Invalied SSCaT Opreation", form1, true);
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
                }
            }
            catch (Exception ex)
            {
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

        public bool RetailerAndCustomerSearchAndDelte(string PhoneNumber_UserID, string FileName, SerialPort Port, SSCaT form)
        {
            string NewCustomer=null;
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
                                

                                if (FileName.Equals("SSCaTRegister.txt"))
                                {
                                    NewCustomer = "SSCaT and Retailer account UserID " + Parts[0] + " with pnone number " + PhoneNumber91 + " successfully Deleted.";
                                    File.Delete(Parts[0] + ".txt");
                                    RetailerAndCustomerSearchAndDelte(PhoneNumber_UserID, "Retailer.txt", Port, form);

                                }
                                else
                                {
                                    if (NewCustomer != null)
                                    {
                                        NewCustomer = "Retailer account UserID " + Parts[0] + " with pnone number " + PhoneNumber91 + " successfully Deleted.";
                                        File.AppendAllText(Parts[0] + ".txt", Parts[1] + " " + "None" + " " + "Delete Retailer Account" + " " + Parts[2] + " " + DateTime.Now.ToString() + " " + "\n");
                                    }
                                }
                                Class2 ObjectClass2 = new Class2();
                                ObjectClass2.sendMsg(Port, Parts[1], NewCustomer, form, true);

                                return true;
                            }
                        }

                    }
                    Reader.Close();
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
