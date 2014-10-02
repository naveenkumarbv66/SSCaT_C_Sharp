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
    class SSCaTOperations
    {

        public void NewAccount(string Message, string CustomerNumber, SerialPort Port, SSCaTMainServer form1)
        {
            string CustomerMessage;
            string RechargeOutletPersonMessage;
            string[] MessageSplit = Message.Split(' ');
            ReadSearchIdentify ObjectClass3 = new ReadSearchIdentify();
            SendReadDeleteMessage ObjectClass2 = new SendReadDeleteMessage();
            CreateUserIdPasswordRetailerAcc ObjectClass5 = new CreateUserIdPasswordRetailerAcc();
            try
            {
                bool TempFlag = ObjectClass3.SearchCustomerRetailer(CustomerNumber, "Retailer.txt");
                if (TempFlag)
                {
                    if (MessageSplit[1].Length == 10 || MessageSplit[1].StartsWith("0") && (MessageSplit[1].Length == 11) || MessageSplit[1].StartsWith("+91") && (MessageSplit[1].Length == 13))
                    {
                        string[] CustomerDetails = ObjectClass3.ReadDataFromCustomerRetailerFile(MessageSplit[1], "SSCaTRegister.txt");
                        string[] RetilerDetails = ObjectClass3.ReadDataFromCustomerRetailerFile(CustomerNumber, "SSCaTRegister.txt");
                        if (RetilerDetails[3] == MessageSplit[3])
                        {
                            if (CustomerDetails == null)
                            {
                                double Amount = Convert.ToDouble(MessageSplit[2]);
                                double PresentAmount = Convert.ToDouble(RetilerDetails[2]);
                                if (Amount <= 0 || Amount >= 1000)
                                {
                                    RechargeOutletPersonMessage =
                                         "Insufficient amount. Amount should be greater then 0.0Rs and less then 1000.0Rs.";
                                }
                                else
                                {
                                    string UserID = ObjectClass5.GenerateRandomUserID(5);
                                    string Password = ObjectClass5.GenerateRandomPassword(5);

                                    string NewRetilerData = UserID + " " + MessageSplit[1] + " " + MessageSplit[2] + " " + Password + "\n";

                                    string RetailerOldValue = RetilerDetails[0] + " " + RetilerDetails[1] + " " + RetilerDetails[2] + " " + RetilerDetails[3];
                                    RetilerDetails[2] = Convert.ToString(PresentAmount - Amount);
                                    string RetailerNewValue = RetilerDetails[0] + " " + RetilerDetails[1] + " " + RetilerDetails[2] + " " + RetilerDetails[3];
                                    String strFile = File.ReadAllText("SSCaTRegister.txt");
                                    strFile = strFile.Replace(RetailerOldValue, RetailerNewValue);
                                    File.WriteAllText("SSCaTRegister.txt", strFile);
                                    File.AppendAllText(UserID + ".txt", NewRetilerData + " NewACC " + DateTime.Now.ToString() + " " + "\n");
                                    File.AppendAllText(RetilerDetails[0] + ".txt", RetilerDetails[1] + " " + MessageSplit[1] + " " + "NewACC" + " " + MessageSplit[2] + " " + DateTime.Now.ToString() + " " + "\n");
                                    File.AppendAllText("SSCaTRegister.txt", NewRetilerData);

                                    CustomerMessage ="New SSCaT account successfully created.The UserID: " + UserID + ".Password: " + Password + ".Current Balance:" + MessageSplit[2] + "INR";
                                    RechargeOutletPersonMessage ="New SSCaT account to the number  " + MessageSplit[1] + " successfully created.Current Balance:" + RetilerDetails[2] + "INR";
                                    
                                    form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[3].Text = MessageSplit[1];
                                 
                                    ObjectClass2.sendMsg(Port, MessageSplit[1], CustomerMessage, form1, true, 0);
                                    ObjectClass2.sendMsg(Port, CustomerNumber, RechargeOutletPersonMessage, form1, false, 0);    
                                    
                                }
                            }
                            else
                            {
                                RechargeOutletPersonMessage =
                                    "SSCaT account already exists to the number " + MessageSplit[1] + ".";
                                ObjectClass2.sendMsg(Port, CustomerNumber, RechargeOutletPersonMessage, form1, false, 0);
                            }
                        }
                        else
                        {
                            RechargeOutletPersonMessage =
                                   "Invalid Password. Creation of SSCaT account to the number " + MessageSplit[1] + " unsuccessful.";
                            ObjectClass2.sendMsg(Port, CustomerNumber, RechargeOutletPersonMessage, form1, false, 0);
                        }
                    }
                    else
                    {
                        RechargeOutletPersonMessage =
                           "Invalid phone number. Creation of SSCaT account to the number " + MessageSplit[1] + " unsuccessful.";
                        ObjectClass2.sendMsg(Port, CustomerNumber, RechargeOutletPersonMessage, form1, false, 0);
                    }
                  
                }
                else
                {
                    RechargeOutletPersonMessage =
                        "You don't have ROP rights. Creation of SSCaT account to the number " + MessageSplit[1] + " unsuccessful.";
                    ObjectClass2.sendMsg(Port, CustomerNumber, RechargeOutletPersonMessage, form1, false, 0);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            
        }

        public void RechargeAccount(string Message, string RetailerNumber, SerialPort Port, SSCaTMainServer form1)
        {
            string CustomerMessage;
            string RechargeOutletPersonMessage;
            string[] CustomerInforamtion;
            string[] RetailerInforamtion;
            ReadSearchIdentify ObjectClass3 = new ReadSearchIdentify();
            SendReadDeleteMessage ObjectClass2 = new SendReadDeleteMessage();
            
            string[] MessageSplit = Message.Split(' ');
            try
            { 
                bool TempFlag = ObjectClass3.SearchCustomerRetailer(RetailerNumber, "Retailer.txt");
                if (TempFlag)
                {
                    if (MessageSplit[1].Length == 5)
                    {
                        CustomerInforamtion = ObjectClass3.ReadDataFromCustomerRetailerFile(MessageSplit[1], "SSCaTRegister.txt");
                        RetailerInforamtion = ObjectClass3.ReadDataFromCustomerRetailerFile(RetailerNumber, "SSCaTRegister.txt");
                        if (RetailerInforamtion[3] == MessageSplit[3])
                        {
                            double Amount = Convert.ToDouble(MessageSplit[2]);
                            if (Amount <= 0.0 || Amount >= 1000.0)
                            {
                                RechargeOutletPersonMessage =
                                    "Insufficient amount. Amount should be greater then 0.0Rs and less then 1000.0Rs.";
                            }
                            else
                            {
                                if (CustomerInforamtion == null)
                                {
                                    RechargeOutletPersonMessage = "The customer " + MessageSplit[1] + " doesn’t have a SSCaT account. Recharge Terminated.";
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
                                            double AdditionAmount = Convert.ToDouble(MessageSplit[2]);
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


                                                CustomerMessage = "SSCaT account successfully recharged. Current Balance: " + CustomerInforamtion[2] + "INR.";
                                                RechargeOutletPersonMessage = "SSCaT account of  the UserID " + CustomerInforamtion[0] + " successfully recharged. Current Balance: " + RetailerInforamtion[2] + "INR.";

                                                File.AppendAllText(CustomerInforamtion[0] + ".txt", RetailerNumber + " " + CustomerInforamtion[1] + " " + "Recharge" + " " + MessageSplit[2] + " " + DateTime.Now.ToString() + " " + "\n");
                                                File.AppendAllText(RetailerInforamtion[0] + ".txt", RetailerNumber + " " + CustomerInforamtion[1] + " " + "Recharge" + " " + MessageSplit[2] + " " + DateTime.Now.ToString() + " " + "\n");


                                                form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[3].Text = CustomerInforamtion[0];
                                                ObjectClass2.sendMsg(Port, CustomerInforamtion[1], CustomerMessage, form1, true, 0);
                                                ObjectClass2.sendMsg(Port, RetailerNumber, RechargeOutletPersonMessage, form1, false, 0);
                                            }
                                            else
                                            {
                                                RechargeOutletPersonMessage = "You have insufficient balance in your account to complete this transaction. Please recharge your account  ";
                                                ObjectClass2.sendMsg(Port, RetailerNumber, RechargeOutletPersonMessage, form1, false, 0);
                                            }
                                        }
                                        else
                                        {
                                            RechargeOutletPersonMessage = "You can't recharge your own account. Please contact admin";
                                            ObjectClass2.sendMsg(Port, RetailerNumber, RechargeOutletPersonMessage, form1, false, 0);
                                        }

                                    }

                            }
                        }
                        else
                        {
                            RechargeOutletPersonMessage =
                           "Invalid Password. " + MessageSplit[3] + ".Recharge unsuccessful.";
                            ObjectClass2.sendMsg(Port, RetailerNumber, RechargeOutletPersonMessage, form1, false, 0);
                        }
                    }
                    else
                    {
                        RechargeOutletPersonMessage = "Invalid UserID " + MessageSplit[1] + ". Recharge unsuccessful.";
                        ObjectClass2.sendMsg(Port, RetailerNumber, RechargeOutletPersonMessage, form1, false, 0);
                    }
                }
                else
                {
                    RechargeOutletPersonMessage = "You don't have ROP status. Recharge to the UserID " + MessageSplit[1] + " unsuccessful.";
                    ObjectClass2.sendMsg(Port, RetailerNumber, RechargeOutletPersonMessage, form1, false, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

         
        }

        public void Transaction(string Message, string CustomerNumber, SerialPort Port, SSCaTMainServer form1)
        {
            string[] Mg = Message.Split(' ');
            ReadSearchIdentify ObjectClass3 = new ReadSearchIdentify();
            SendReadDeleteMessage ObjectClass2 = new SendReadDeleteMessage();
           
            string[] CustomerDetails = ObjectClass3.ReadDataFromCustomerRetailerFile(CustomerNumber, "SSCaTRegister.txt");
            string[] RetailerDetails = ObjectClass3.ReadDataFromCustomerRetailerFile(Message, "SSCaTRegister.txt");
            string CustomerMessage;
            string RetilerMessage;

            try
            {
                if (CustomerDetails == null)
                {
                    CustomerMessage = "You don’t have a SSCaT account to complete the transaction. Please create a SSCaT account";
                    ObjectClass2.sendMsg(Port, CustomerDetails[1], CustomerMessage, form1, true, 0);
                }
                if (RetailerDetails == null)
                {
                    RetilerMessage = "The customer with UserID " + Mg[0] + " does not have a SSCaT account. Recharge Terminated.";
                    ObjectClass2.sendMsg(Port, RetailerDetails[1], RetilerMessage, form1, true, 0);
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

                            CustomerMessage = "Transaction complete. INR " + MegMmount + " has been deducted from your account. Current Balance: " + CustomerDetails[2] + "INR.";
                            RetilerMessage = "Transaction complete. INR " + MegMmount + "  has been credited to your account from " + CustomerDetails[0] + ". Current Balance: " + RetailerDetails[2] + "INR.";

                            form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[3].Text = RetailerDetails[0];
                            ObjectClass2.sendMsg(Port, CustomerDetails[1], CustomerMessage, form1, true, 0);

                            ObjectClass2.sendMsg(Port, RetailerDetails[1], RetilerMessage, form1, false, 0);
                            
                            File.AppendAllText(CustomerDetails[0] + ".txt", CustomerNumber + " " + RetailerDetails[1] + " " + "Transaction" + " " + Mg[1] + " " + DateTime.Now.ToString() + " " + "\n");
                            File.AppendAllText(RetailerDetails[0] + ".txt", CustomerNumber + " " + RetailerDetails[1] + " " + "Transaction" + " " + Mg[1] + " " + DateTime.Now.ToString() + " " + "\n");


                        }
                        else
                        {
                            CustomerMessage = "Transaction terminated due to insufficient balance. Please recharge your account";
                            ObjectClass2.sendMsg(Port, CustomerDetails[1], CustomerMessage, form1, true, 0);
                        }
                    }
                    else
                    {
                        CustomerMessage = "Invalid password entered.";
                        ObjectClass2.sendMsg(Port, CustomerDetails[1], CustomerMessage, form1, true, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void Balance(string CustomerNumber, SerialPort Port, SSCaTMainServer form1)
        {
            ReadSearchIdentify ObjectClass3 = new ReadSearchIdentify();
            SendReadDeleteMessage ObjectClass2 = new SendReadDeleteMessage();
           
            try
            {
                string[] CustomerDetails = ObjectClass3.ReadDataFromCustomerRetailerFile(CustomerNumber, "SSCaTRegister.txt");
                string CustomerMessage;
                if (CustomerDetails == null)
                {
                    CustomerMessage = "You dont have SSCaT account.";
                }
                else
                {
                    CustomerMessage = "Current balance in your SSCaT account is " + CustomerDetails[2] + "INR.";
                    File.AppendAllText(CustomerDetails[0] + ".txt", CustomerNumber + " " + "None" + " " + "Balance" + " " + CustomerDetails[2] + " " + DateTime.Now.ToString() + " " + "\n");
                }
                 form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[5].Text = "None";
                 form1.listView1.Items[form1.listView1.Items.Count - 1].SubItems[3].Text = CustomerNumber;
                 ObjectClass2.sendMsg(Port, CustomerNumber, CustomerMessage, form1, true, 0);
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void DeleteSSCaTAccountFromMessage(string Message, string CustomerNumber,SerialPort Port, SSCaTMainServer form)
        {
            try
            {
                ReadSearchIdentify ObjectClass3 = new ReadSearchIdentify();
                SendReadDeleteMessage ObjectClass2 = new SendReadDeleteMessage();
                string CustomerMessage;
                string[] MessageSplit = Message.Split(' ');
                string[] CustomerDetails = ObjectClass3.ReadDataFromCustomerRetailerFile(CustomerNumber, "SSCaTRegister.txt");

                if (CustomerDetails == null)
                {
                    CustomerMessage = "You don’t have a SSCaT account. Please create a SSCaT account";
                    ObjectClass2.sendMsg(Port, CustomerNumber, CustomerMessage, form, true, 0);
                }
                else
                    if (CustomerDetails[3] == MessageSplit[1])
                    {
                        bool TempFlag = ObjectClass3.SearchCustomerRetailer(CustomerNumber, "Retailer.txt");
                        if (TempFlag)
                        {
                            CustomerMessage = "You have a ROP account. Please Conntact to Admin.";
                            ObjectClass2.sendMsg(Port, CustomerNumber, CustomerMessage, form, true, 0);
                        }
                        else
                        {
                          int Temp=  ObjectClass3.RetailerAndCustomerSearchAndDelte(CustomerNumber, "SSCaTRegister.txt", form);
                          if (Temp == 1)
                          {
                              CustomerMessage = "SSCaT UserID " + form.TempMemory[0] + " with phone number " + form.TempMemory[1] + " Successfully Deleted.";
                              string FileDelete = form.TempMemory[0] + ".txt";
                              File.Delete(FileDelete);
                              ObjectClass2.sendMsg(Port, form.TempMemory[1], CustomerMessage, form, true, 0);
                          }
                          else
                          {
                              CustomerMessage = "Error while deleting SSCaT UserID " + form.TempMemory[0] + " with phone number " + form.TempMemory[1] + " Try later.";
                              string FileDelete = form.TempMemory[0] + ".txt";
                              File.Delete(FileDelete);
                              ObjectClass2.sendMsg(Port, form.TempMemory[1], CustomerMessage, form, true, 0);
                          }
                        }
                    }
                    else
                    {
                        CustomerMessage = "Invalid password";
                        ObjectClass2.sendMsg(Port, CustomerNumber, CustomerMessage, form, true, 0);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

    }
}
