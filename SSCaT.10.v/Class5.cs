using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;

namespace SSCaT._10.v
{
    class Class5
    {
        public String GenerateRandomPassword(int Length)
        {

            String RandomString = "";
            int RandNumber;

            try
            {
                Random Random = new Random();
                for (int i = 0; i < Length; i++)
                {
                    if (Random.Next(1, 3) == 1)
                        RandNumber = Random.Next(97, 123); //char {a-z}
                    else
                        RandNumber = Random.Next(48, 58); //int {0-9}

                    RandomString = RandomString + (char)RandNumber;
                }
                return RandomString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool CheckUniqUserID(string UserID)
        {
            try
            {
                if (File.Exists("SSCaTRegister.txt"))
                {
                    string Line;
                    StreamReader reader = new StreamReader("SSCaTRegister.txt");
                    while ((Line = reader.ReadLine()) != null)
                    {
                        string[] parts = Line.Split(' ');
                        if (parts.Length == 4)
                        {
                            if (UserID == parts[0])
                            {
                                reader.Close();
                                return false;
                            }
                        }
                    }
                    reader.Close();
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public String GenerateRandomUserID(int Length)
        {
            String RandomString = "";
            int RandNumber;
            try
            {
                Random Random = new Random();
                RandNumber = Random.Next(65, 90); //char {A-Z}
                RandomString = RandomString + (char)RandNumber;

                RandomString = RandomString + GenerateRandomPassword(Length - 1);

                if (CheckUniqUserID(RandomString) == false)
                {
                    GenerateRandomUserID(Length);
                }
                return RandomString;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public int CreateRetailerAccount(string PhoneNumber, string Amount, SerialPort Port, SSCaT from)
        {
            bool Flag = true;
            Class3 ObjectClass3 = new Class3();
            Class2 ObjectClass2 = new Class2();
            //SSCaT ObjectForm1 = new SSCaT();
            Flag = ObjectClass3.SearchCustomerRetailer(PhoneNumber, "Retailer.txt");
            Flag = !Flag;

            if (Flag)
            {
                string USERID = null;
                Class4 ObjectClass4 = new Class4();
                try
                {
                    string[] CustomerData = ObjectClass3.ReadDataFromCustomerRetailerFile(PhoneNumber, "SSCaTRegister.txt");
                    if (CustomerData == null)
                    {
                        Flag = true;
                    }
                    else
                    {
                        Flag = false;
                        USERID = CustomerData[0];
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }

                if (Flag)
                {     
                    try
                    {
                        string UserID = GenerateRandomUserID(5);
                        string Password = GenerateRandomPassword(5);
                        string NewRetilerData = UserID + " " + PhoneNumber + " " + Amount + " " + Password + "\n";

                        File.AppendAllText(UserID + ".txt", NewRetilerData + " NewACC " + DateTime.Now.ToString() + " " + "\n");
                        File.AppendAllText("SSCaTRegister.txt", NewRetilerData);
                        File.AppendAllText("Retailer.txt", UserID + " " + PhoneNumber + "\n");
                        string NewCustomer = "New SSCaT and Retiler account successfully created. The UserID " + UserID + ".The password " + Password + ". Your Current Balance is " + Amount + "INR";

                        ObjectClass2.sendMsg(Port, PhoneNumber, NewCustomer, from, true);
                        from.panel3.Show();
                        from.pictureBox4.Hide();
                        from.label10.Hide();
                        from.label9.Text = "Successfully created retiler and SSCaT account.....!";
                        from.pictureBox3.Show();
                        from.label9.Show();
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        return 0;
                    }
                }
                else
                {
                    try
                    {
                        File.AppendAllText("Retailer.txt", USERID + " " + PhoneNumber + "\n");
                        string NewCustomer = "Successfully created retiler account to the userID " + USERID + " Phone Number " + PhoneNumber + ".....! ";
                        ObjectClass2.sendMsg(Port, PhoneNumber, NewCustomer, from, true);
                        from.panel3.Show();
                        from.pictureBox4.Hide();
                        from.label10.Hide();
                        from.label9.Text = "Alredy have SSCaT account..! but created Retiler account...! ";
                        from.pictureBox3.Show();
                        from.label9.Show();
                        return 2;
                    }
                    catch (Exception ex)
                    {
                        return 0;
                    }
                }
            }
            else
            {
                try
                {
                    string[] CustomerInforamtion = ObjectClass3.ReadDataFromCustomerRetailerFile(PhoneNumber, "SSCaTRegister.txt");
                    string NewCustomer = "Alredy both retiler and SSCaT account exits..! UserID : " + CustomerInforamtion[0] + " Pnone Number : " + CustomerInforamtion[1];
                    ObjectClass2.sendMsg(Port, PhoneNumber, NewCustomer, from, true);
                    from.panel3.Show();
                    from.pictureBox4.Hide();
                    from.label10.Hide();
                    from.label9.Text = "Alredy both retiler and SSCaT account exits..! ";
                    from.pictureBox3.Show();
                    from.label9.Show();
                    return 3;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

        }
    }
}
