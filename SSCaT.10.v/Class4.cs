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
    class Class4
    {

        public void NewAccount(string Message, string CustomerNumber, SerialPort Port, SSCaT form1)
        {
            string CustomerMessage;
            string RechargeOutletPersonMessage;
            string[] MessageSplit = Message.Split(' ');
            Class3 ObjectClass3 = new Class3();
            Class2 ObjectClass2 = new Class2();
            Class5 ObjectClass5 = new Class5();
            try
            {
                bool TempFlag = ObjectClass3.SearchCustomerRetailer(CustomerNumber, "Retailer.txt");
                if (TempFlag)
                {
                    if (MessageSplit[2].Length == 10)
                    {
                        string[] CustomerDetails = ObjectClass3.ReadDataFromCustomerRetailerFile(MessageSplit[2], "SSCaTRegister.txt");
                        string[] RetilerDetails = ObjectClass3.ReadDataFromCustomerRetailerFile(CustomerNumber, "SSCaTRegister.txt");
                        if (CustomerDetails == null)
                        {
                            double Amount = Convert.ToDouble(MessageSplit[3]);
                            if (Amount <= 0 || Amount >= 1000)
                            {
                                RechargeOutletPersonMessage =
                                     "Insufficient amount. Amount should be greater then 0.0Rs and less then 1000.0Rs.";
                            }
                            else
                            {
                                string UserID = ObjectClass5.GenerateRandomUserID(5);
                                string Password = ObjectClass5.GenerateRandomPassword(5);

                                string NewRetilerData = UserID + " " + MessageSplit[2] + " " + MessageSplit[3] + " " + Password + "\n";

                                File.AppendAllText(UserID + ".txt", NewRetilerData + " NewACC " + DateTime.Now.ToString() + " " + "\n");
                                File.AppendAllText(RetilerDetails[0] + ".txt", RetilerDetails[1] + " " + MessageSplit[2] + " " + "NewACC" + " " + MessageSplit[3] + " " + DateTime.Now.ToString() + " " + "\n");
                                File.AppendAllText("SSCaTRegister.txt", NewRetilerData);

                                CustomerMessage =
                                    "New SSCaT account successfully created. The UserID to your SSCaT account is " + UserID + ".The password to your SSCaT account is " + Password + ". Your Current Balance is " + MessageSplit[3] + "INR";
                                RechargeOutletPersonMessage =
                                    " New SSCaT account to the number  " + MessageSplit[2] + " successfully created ";

                                form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[3].Text = MessageSplit[2];
                                ObjectClass2.sendMsg(Port, MessageSplit[2], CustomerMessage, form1, true);                        
                            }
                        }
                        else
                        {
                            RechargeOutletPersonMessage =
                                "Already have SSCaT account to the number " + MessageSplit[2] + ".";
                        }
                    }
                    else
                    {
                        RechargeOutletPersonMessage =
                            "Inalid phone number. Creation of SSCaT account to the number " + MessageSplit[2] + " unsuccessful.";
                    }

                    ObjectClass2.sendMsg(Port, CustomerNumber, RechargeOutletPersonMessage, form1, false);
                }
                else
                {
                    RechargeOutletPersonMessage =
                        "You don't have Retailer account. Creation of SSCaT account to the number " + MessageSplit[2] + " unsuccessful.";
                    ObjectClass2.sendMsg(Port, CustomerNumber, RechargeOutletPersonMessage, form1, false);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void RechargeAccount(string Message, string RetailerNumber, SerialPort Port, SSCaT form1)
        {
            string CustomerMessage;
            string RechargeOutletPersonMessage;
            Class3 ObjectClass3 = new Class3();
            Class2 ObjectClass2 = new Class2();
            
            string[] MessageSplit = Message.Split(' ');

            try
            {
                bool TempFlag = ObjectClass3.SearchCustomerRetailer(RetailerNumber, "Retailer.txt");
                if (TempFlag)
                {
                    if (MessageSplit[2].Length == 5)
                    {
                        double Amount = Convert.ToDouble(MessageSplit[3]);
                        if (Amount <= 0.0 || Amount >= 1000.0)
                        {
                            RechargeOutletPersonMessage =
                                "Insufficient amount. Amount should be greater then 0.0Rs and less then 1000.0Rs.";
                        }
                        else
                        {
                            string[] CustomerInforamtion = ObjectClass3.ReadDataFromCustomerRetailerFile(MessageSplit[2], "SSCaTRegister.txt");
                            string[] RetailerInforamtion = ObjectClass3.ReadDataFromCustomerRetailerFile(RetailerNumber, "SSCaTRegister.txt");
                            
                            if (CustomerInforamtion == null)
                            {
                                RechargeOutletPersonMessage = "The customer " + MessageSplit[2] + " doesn’t have a SSCaT account. Recharge Terminated.";
                            }
                            else
                                if (RetailerInforamtion == null)
                                {
                                    RechargeOutletPersonMessage = "The customer " + RetailerInforamtion[0] + " doesn’t have a SSCaT account. Recharge Terminated.";
                                }
                                else
                                {
                                    if (CustomerInforamtion[0] != RetailerInforamtion[0])
                                    {
                                        double OriginalAmount = Convert.ToDouble(CustomerInforamtion[2]);
                                        double AdditionAmount = Convert.ToDouble(MessageSplit[3]);
                                        double RetailerAmount = Convert.ToDouble(RetailerInforamtion[2]);
                                        if (RetailerAmount >= AdditionAmount)
                                        {
                                            string CustomerOldValue = CustomerInforamtion[0] + " " + CustomerInforamtion[1] + " " + CustomerInforamtion[2] + " " + CustomerInforamtion[3];
                                            CustomerInforamtion[2] = Convert.ToString(OriginalAmount + AdditionAmount);
                                            string CustomerNewValue = CustomerInforamtion[0] + " " + CustomerInforamtion[1] + " " + CustomerInforamtion[2] + " " + CustomerInforamtion[3];

                                            string RetailerOldValue = RetailerInforamtion[0] + " " + RetailerInforamtion[1] + " " + RetailerInforamtion[2] + " " + RetailerInforamtion[3];
                                            RetailerInforamtion[2] = Convert.ToString(RetailerAmount - AdditionAmount);
                                            string RetailerNewValue = RetailerInforamtion[0] + " " + RetailerInforamtion[1] + " " + RetailerInforamtion[2] + " " + RetailerInforamtion[3];

                                            String strFile = File.ReadAllText("SSCaTRegister.txt");
                                            strFile = strFile.Replace(CustomerOldValue, CustomerNewValue);
                                            strFile = strFile.Replace(RetailerOldValue, RetailerNewValue);
                                            File.WriteAllText("SSCaTRegister.txt", strFile);


                                            CustomerMessage = "SSCaT account successfully recharged. Your Current Balance is " + CustomerInforamtion[2] + "INR.";
                                            RechargeOutletPersonMessage = " SSCaT account of  the UserID " + CustomerInforamtion[0] + " successfully recharged. Your Current Balance is " + RetailerInforamtion[2] + "INR.";

                                            File.AppendAllText(CustomerInforamtion[0] + ".txt", RetailerNumber + " " + CustomerInforamtion[1] + " " + "Recharge" + " " + MessageSplit[3] + " " + DateTime.Now.ToString() + " " + "\n");
                                            File.AppendAllText(RetailerInforamtion[0] + ".txt", RetailerNumber + " " + CustomerInforamtion[1] + " " + "Recharge" + " " + MessageSplit[3] + " " + DateTime.Now.ToString() + " " + "\n");


                                            form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[3].Text = CustomerInforamtion[0];
                                            ObjectClass2.sendMsg(Port, CustomerInforamtion[1], CustomerMessage, form1, true);
                                        }
                                        else
                                        {
                                            RechargeOutletPersonMessage = "You have insufficient balance in your account to complete this transaction. Please recharge your account  ";
                                        }
                                    }
                                    else
                                    {
                                        RechargeOutletPersonMessage = "You can't recharge your own account. Please contact adime";
                                    }

                                }

                        }
                    }
                    else
                    {
                        RechargeOutletPersonMessage = "Invalid password to the UserID " + MessageSplit[2] + " unsuccessful.";
                    }
                    ObjectClass2.sendMsg(Port, RetailerNumber, RechargeOutletPersonMessage, form1, false);  
                }
                else
                {
                    RechargeOutletPersonMessage = "You don't have Retailer account. Recharge to the number " + MessageSplit[2] + " unsuccessful.";
                    ObjectClass2.sendMsg(Port, RetailerNumber, RechargeOutletPersonMessage, form1, false);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Transaction(string Message, string CustomerNumber, SerialPort Port, SSCaT form1)
        {
            string[] Mg = Message.Split(' ');
            Class3 ObjectClass3 = new Class3();
            Class2 ObjectClass2 = new Class2();
           
            string[] CustomerDetails = ObjectClass3.ReadDataFromCustomerRetailerFile(CustomerNumber, "SSCaTRegister.txt");
            string[] RetailerDetails = ObjectClass3.ReadDataFromCustomerRetailerFile(Message, "SSCaTRegister.txt");
            string CustomerMessage;
            string RetilerMessage;

            try
            {
                if (CustomerDetails == null)
                {
                    CustomerMessage = "You don’t have a SSCaT account to Complete the transaction. Please Create a SSCaT account";
                    ObjectClass2.sendMsg(Port, CustomerDetails[1], CustomerMessage, form1, true);
                }
                if (RetailerDetails == null)
                {
                    RetilerMessage = "The customer with UserID " + Mg[0] + " doesn’t have a SSCaT account. Recharge Terminated.";
                    ObjectClass2.sendMsg(Port, RetailerDetails[1], RetilerMessage, form1, true);
                }

                if (RetailerDetails != null && CustomerDetails != null)
                {
                    if (CustomerDetails[3] == Mg[2])
                    {
                        double PrestAmount = Convert.ToDouble(CustomerDetails[2]);
                        double MegMmount = Convert.ToDouble(Mg[1]);

                        if ((MegMmount <= PrestAmount) && (PrestAmount >= 0))
                        {
                            double RetAm = Convert.ToDouble(RetailerDetails[2]);
                            RetAm += MegMmount;
                            PrestAmount -= MegMmount;

                            string OldCustomerData = CustomerDetails[0] + " " + CustomerDetails[1] + " " + CustomerDetails[2] + " " + CustomerDetails[3];
                            string OldRetilerData = RetailerDetails[0] + " " + RetailerDetails[1] + " " + RetailerDetails[2] + " " + RetailerDetails[3];

                            RetailerDetails[2] = Convert.ToString(RetAm);
                            CustomerDetails[2] = Convert.ToString(PrestAmount);

                            string NewCustomerData = CustomerDetails[0] + " " + CustomerDetails[1] + " " + CustomerDetails[2] + " " + CustomerDetails[3];
                            string NewRetilerData = RetailerDetails[0] + " " + RetailerDetails[1] + " " + RetailerDetails[2] + " " + RetailerDetails[3];

                            String strFile = File.ReadAllText("SSCaTRegister.txt");
                            strFile = strFile.Replace(OldCustomerData, NewCustomerData);
                            strFile = strFile.Replace(OldRetilerData, NewRetilerData);
                            File.WriteAllText("SSCaTRegister.txt", strFile);

                            CustomerMessage = "Transaction complete. INR " + MegMmount + " has been deducted from your account. Your currant balance is: " + CustomerDetails[2] + "INR.";
                            RetilerMessage = "Transaction complete. INR " + MegMmount + "  has been credited to your account from " + CustomerDetails[0] + ". Your currant balance is: " + RetailerDetails[2] + "INR.";

                            form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[3].Text = RetailerDetails[0];
                            ObjectClass2.sendMsg(Port, CustomerDetails[1], CustomerMessage, form1, true);
                            
                            ObjectClass2.sendMsg(Port, RetailerDetails[1], RetilerMessage, form1, false);
                            
                            File.AppendAllText(CustomerDetails[0] + ".txt", CustomerNumber + " " + RetailerDetails[1] + " " + "Transaction" + " " + Mg[1] + " " + DateTime.Now.ToString() + " " + "\n");
                            File.AppendAllText(RetailerDetails[0] + ".txt", CustomerNumber + " " + RetailerDetails[1] + " " + "Transaction" + " " + Mg[1] + " " + DateTime.Now.ToString() + " " + "\n");


                        }
                        else
                        {
                            CustomerMessage = "Transaction terminated. You have insufficient balance in your account to complete this transaction. Please recharge your account";
                            ObjectClass2.sendMsg(Port, CustomerDetails[1], CustomerMessage, form1, true);
                        }
                    }
                    else
                    {
                        CustomerMessage = "InValid password you entered..";
                        ObjectClass2.sendMsg(Port, CustomerDetails[1], CustomerMessage, form1, true);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Balance(string CustomerNumber, SerialPort Port, SSCaT form1)
        {
            Class3 ObjectClass3 = new Class3();
            Class2 ObjectClass2 = new Class2();
           
            try
            {
                string[] CustomerDetails = ObjectClass3.ReadDataFromCustomerRetailerFile(CustomerNumber, "SSCaTRegister.txt");
                string CustomerMessage;
                if (CustomerDetails == null)
                {
                    CustomerMessage = "You dont have SSCaT account";
                }
                else
                {
                    CustomerMessage = "Current balance in your SSCaT account is " + CustomerDetails[2] + "INR.";
                    File.AppendAllText(CustomerDetails[0] + ".txt", CustomerNumber + " " + "None" + " " + "Balance" + " " + CustomerDetails[2] + " " + DateTime.Now.ToString() + " " + "\n");
                }
                 form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[5].Text = "None";
                 form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[3].Text = CustomerNumber;
                 ObjectClass2.sendMsg(Port, CustomerNumber, CustomerMessage, form1, true);
               
            }
            catch (Exception ex)
            {
            }
        }

    }
}
