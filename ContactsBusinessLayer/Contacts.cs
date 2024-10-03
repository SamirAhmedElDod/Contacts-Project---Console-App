using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ContactsDataAccessLayer;
using System.Runtime.CompilerServices;

namespace ContactsBusinessLayer
{
    public class clsContacts
    {
        public enum enMode
        {
            Addnew = 0,
            Update = 1
        }
        public enMode Mode = enMode.Addnew;
        public int ID { get; set; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Address { set; get; }
        public DateTime DateOfBirth { set; get; }

        public string ImagePath { set; get; }

        public int CountryID { set; get; }

        public string CountryName {  set; get; }
        public string CountryCode {  set; get; }
        public string CountryPhoneCode {  set; get; }

        // Default Constructor
        // Used To add New 
        public clsContacts()
        {
            this.ID = -1;
            this.FirstName = "";
            this.LastName = "";
            this.Email = "";
            this.Phone = "";
            this.Address = "";
            this.DateOfBirth = DateTime.Now;
            this.CountryID = -1;
            this.ImagePath = "";

            Mode = enMode.Addnew;
        }


        // Constructor 
        // Used To Update
        private clsContacts (int ID, string FirstName, string LastName,
            string Email, string Phone, string Address, DateTime DateOfBirth, int CountryID, string ImagePath)
        {
            this.ID = ID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
            this.Address = Address;
            this.DateOfBirth = DateOfBirth;
            this.CountryID = CountryID;
            this.ImagePath = ImagePath;

            Mode = enMode.Update;

        }

        public static clsContacts Find(int ID) 
        {
            string FirstName = "", LastName = "", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int CountryID = -1;

            if (clsContactsData.GetContactsByID
                (ID, ref FirstName , ref LastName , ref Email , ref Phone, ref Address , ref DateOfBirth, ref CountryID , ref ImagePath))
            {
                return new clsContacts(ID, FirstName, LastName, Email, Phone, Address, DateOfBirth, CountryID, ImagePath);
            }
            else
            {
                return null;
            }

        }

        private bool _AddNewContact()
        {
            this.ID = clsContactsData.AddNewContact
                (this.FirstName, this.LastName, this.Email, this.Phone, this.Address, this.DateOfBirth, this.CountryID, this.ImagePath);
            return (this.ID != -1) ;
        }
        private bool _UpdateContact()
        {
            return clsContactsData.UpdateContact
                (this.ID, this.FirstName, this.LastName, this.Email, this.Phone,
                this.Address, this.DateOfBirth, this.CountryID, this.ImagePath);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.Addnew:
                    if(_AddNewContact())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    { 
                        return false;
                    }
                case enMode.Update:
                    return _UpdateContact();
            }
            return true;
        }

        public static bool DeleteContact(int ID)
        {
            return clsContactsData.DeleteContact(ID);
        }

        public static DataTable GetAllContacts()
        {
            return clsContactsData.GetAllContacts();
        }

        public static bool IsContactExist(int ID)
        {
            return clsContactsData.IsContactExist(ID);
        }

        // Country Code -----
        // Country Costructor Object That We Will Return 
        clsContacts (int CountryID , string CountryName , string CountryCode , string CountryPhoneCode)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
            this.CountryCode = CountryCode;
            this.CountryPhoneCode = CountryPhoneCode;

            Mode = enMode.Update;
        }

        public static clsContacts FindCountryByName(string CountryName)
        {
            int CountryID = 0;
            string CountryCode = "";
            string CountryPhoneCode = "";

            if (clsContactsData.FindCountryByName(ref CountryID, ref CountryName , ref CountryCode , ref CountryPhoneCode))
            {
                return new clsContacts(CountryID, CountryName, CountryCode , CountryPhoneCode);
            } 
            else 
            {
                return null;
            }
        }

        public static bool IsCountryExistByName(string CountryName) 
        {
            return clsContactsData.IsCountryExistByName(CountryName);
            
        }

        private bool _AddNewCountry()
        {
            this.CountryID = clsContactsData.AddNewCountry(this.CountryName, this.CountryCode, this.CountryPhoneCode);
            return (this.CountryID != -1);
        }

        private bool _UpdateCountry()
        {
            return clsContactsData.UpdateCountry(this.CountryID, this.CountryName, this.CountryCode, this.CountryPhoneCode);
        }

        public bool SaveForCoutries()
        {
            switch(Mode)
            {
                case enMode.Addnew:
                    if (_AddNewCountry())
                    {
                        Mode = enMode.Update;
                        return true;
                    } else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateCountry();
            }
            return true;
        }


        public static bool DeleteCountry(string CountryName)
        {
            return clsContactsData.DeleteCountry(CountryName);
        }

        public static DataTable GetAllCountries()
        {
            return clsContactsData.GetAllCountries();
        }
    }
}

