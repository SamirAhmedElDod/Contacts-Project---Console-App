using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ContactsBusinessLayer;
using System.Diagnostics.Contracts;

namespace ConsoleApp1
{
    internal class Program
    {
        public static void TestFindByID(int ID)
        {
            clsContacts Contact =  clsContacts.Find(ID);
            if(Contact != null)
            {
                Console.WriteLine(Contact.FirstName + " " + Contact.LastName);
                Console.WriteLine(Contact.Email);
                Console.WriteLine(Contact.Phone);
                Console.WriteLine(Contact.Address);
                Console.WriteLine(Contact.DateOfBirth);
                Console.WriteLine(Contact.CountryID);
                Console.WriteLine(Contact.ImagePath);
            }
            else
            {
                Console.WriteLine("Contact [" + ID + "] Not found!");
            }
        } 

        public static void TestAddNewContact()
        {
            clsContacts Contact = new clsContacts();

            Contact.FirstName = "Samir";
            Contact.LastName = "Ahmed";
            Contact.Email = "Basm.Fathy@gmail.com";
            Contact.Phone = "01277040276";
            Contact.Address = "20st | Alexadria";
            Contact.DateOfBirth = new DateTime(2003, 7, 24, 10,30, 0);
            Contact.CountryID = 1;
            Contact.ImagePath = "";

            if (Contact.Save())
            {
                Console.WriteLine("Record Inserted Successfully ");
            }

        }

        public static void TestUpdateContact(int ID)
        {
            clsContacts Contact = clsContacts.Find(ID);

            if (Contact != null)
            {
                Contact.FirstName = "Samir";
                Contact.LastName = "Ahmed";
                Contact.Email = "Basm.Fathy@gmail.com";
                Contact.Phone = "01277040276";
                Contact.Address = "20st | Alexadria";
                Contact.DateOfBirth = new DateTime(2003, 7, 24, 10, 30, 0);
                Contact.CountryID = 1;
                Contact.ImagePath = "";
            }

            if (Contact.Save())
            {
                Console.WriteLine("Record Updated Successfully ");
            }
        }
        public static void TestDeleteContact(int ID)
        {
            if(clsContacts.DeleteContact(ID))
            {
                Console.WriteLine("Contact Deleted Successfully");
            } 
            else
            {
                Console.WriteLine("Failed To Delete Contact");
            }
        } 
        public static void TestGetAllContacts()
        {
            DataTable dataTable = clsContacts.GetAllContacts();

            Console.WriteLine("All Contacts Data");

            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine($"{row["ContactID"]}, {row["FirstName"]}, {row["LastName"]}");
            }
        }

        public static void TestIsContactExist(int ID)
        {
            if (clsContacts.IsContactExist(ID))
            {
                Console.WriteLine("Yes , Contact Is There");
            }
            else
            {
                Console.WriteLine("No , Contact Is Not There");
            }
        }

        // Country Code -------

        public static void TestFindCountryByName(string CountryName)
        {
            clsContacts contacts = clsContacts.FindCountryByName(CountryName);
            
            if(contacts != null)
            {
                Console.WriteLine($"Country Found [ Country ID ] : {contacts.CountryID}");
                Console.WriteLine($"Country Found [ Country Name ] : {contacts.CountryName}");
            } 
            else
            {
                Console.WriteLine($"No Country Found With [ Country Name ] : {CountryName}");

            }
        }

        public static void TestIsCountryExistFoundByName(string CountryName) 
        {
            if (clsContacts.IsCountryExistByName(CountryName))
            {
                Console.WriteLine($"{CountryName} Exists");
            }
            else
            {
                Console.WriteLine($"{CountryName} Not Exists");
            }
        }

        public static void TestAddNewCountry()
        {
            clsContacts Country = new clsContacts();

            Country.CountryID = 7;
            Country.CountryName = "Alexandria";
            Country.CountryCode = "111";
            Country.CountryPhoneCode = "03";

            if (Country.SaveForCoutries())
            {
                Console.WriteLine("New Country Added Successfully");
            } 
            else
            {
                Console.WriteLine("No Country Added");
            }
        }
            
        public static void TestUpdateCountryByName(string CountryName)
        {
            clsContacts Country = clsContacts.FindCountryByName(CountryName);

            if (Country != null )
            {
                Country.CountryID = 7;
                Country.CountryName = "Alexandria";
                Country.CountryCode = "999";
                Country.CountryPhoneCode = "03";
                
                if(Country.SaveForCoutries())
                {
                    Console.WriteLine("Country Updated Successfully");
                }
                else
                {
                    Console.WriteLine("No Country Updated");
                }
            } 
            else
            {
                    Console.WriteLine("No Country Found");

            }

        }

        public static void TestDeleteCountryByName(string CountryName)
        {
            if(clsContacts.DeleteCountry(CountryName))
            {
                Console.WriteLine("Country Deleted Successfully");
            }
            else
            {
                Console.WriteLine("Failed To Delete Country");
            }
        }

        public static void GetAllCountries()
        {
            DataTable table = clsContacts.GetAllCountries();

            foreach(DataRow row in table.Rows)
            {
                Console.WriteLine($"{row["CountryID"]}, {row["CountryName"]}, {row["Code"]}, {row["PhoneCode"]}");
            }
        }
        static void Main(string[] args)
        {

            // To Find By ID 
            //TestFindByID(1);

            // To Add New Contact 
            //TestAddNewContact();

            // To Update Contact
            //TestUpdateContact(1);

            // To Delete Contact
            //TestDeleteContact(1);

            // TO Get All Contacts 
            ///TestGetAllContacts();

            // Check If Contact Exist
            //TestIsContactExist(1);


            //TestFindCountryByName("Canada");

            //TestIsCountryExistFoundByName("Germany");
            //TestIsCountryExistFoundByName("Egypt");

            //TestAddNewCountry();

            //TestDeleteCountryByName("canada");
            GetAllCountries();


            Console.ReadKey();
        }
    }
}
