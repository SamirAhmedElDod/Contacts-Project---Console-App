using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Net;
using System.Security.Policy;
using System.Collections;
using System.Data;

namespace ContactsDataAccessLayer
{
    public class clsContactsData
    {
        static SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
        public static bool GetContactsByID(int ContactID, ref string FirstName, ref string LastName,
            ref string Email, ref string Phone, ref string Address,
            ref DateTime DateOfBirth, ref int CountryID, ref string ImagePath)
        {

            bool IsFound = false;

            string GetContact_Query = "select * from Contacts Where ContactID = @ContactID";
            SqlCommand GetContact_Command = new SqlCommand(GetContact_Query, connection);

            GetContact_Command.Parameters.AddWithValue("@ContactID", ContactID);

            try
            {
                connection.Open();
                SqlDataReader reader = GetContact_Command.ExecuteReader();

                if (reader.Read()) 
                {
                    IsFound = true;

                    FirstName = (String)reader["FirstName"];
                    LastName = (string)reader["LastName"];
                    Email = (string)reader["Email"];
                    Phone = (string)reader["Phone"];
                    Address = (string)reader["Address"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    CountryID = (int)reader["CountryID"];

                    // We Have ImagePath Column In Database Allows NULL So We Need To Handle IT 

                    if (reader["ImagePath"] != DBNull.Value) 
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
                    }
                } else
                {
                    IsFound = false;
                }
                reader.Close();
            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }


            return IsFound;
        }
        

        public static int AddNewContact(string FirstName, string LastName,
            string Email, string Phone, string Address,
            DateTime DateOfBirth, int CountryID, string ImagePath)
        {
            int ContactId = -1;
            string InsertContact_Query = @"INSERT INTO Contacts (FirstName, LastName, Email, Phone, Address,DateOfBirth, CountryID,ImagePath)
                             VALUES (@FirstName, @LastName, @Email, @Phone, @Address,@DateOfBirth, @CountryID,@ImagePath);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand InsertContact_Command = new SqlCommand(InsertContact_Query, connection);


            InsertContact_Command.Parameters.AddWithValue("@FirstName", FirstName);
            InsertContact_Command.Parameters.AddWithValue("@LastName", LastName);
            InsertContact_Command.Parameters.AddWithValue("@Email", Email);
            InsertContact_Command.Parameters.AddWithValue("@Phone", Phone);
            InsertContact_Command.Parameters.AddWithValue("@Address", Address);
            InsertContact_Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            InsertContact_Command.Parameters.AddWithValue("@CountryID", CountryID);
            
            if(ImagePath != "")
            {
                InsertContact_Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
            {
                InsertContact_Command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            }

            try
            {
                connection.Open();
                Object result = InsertContact_Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID) )
                {
                    ContactId = InsertedID;
                    
                }
            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
            }
            finally
            {
                connection.Close();
            }
            return ContactId;
        }
        

        public static bool UpdateContact(int ContactID ,string FirstName, string LastName,
            string Email, string Phone, string Address,
            DateTime DateOfBirth, int CountryID, string ImagePath)
        {
            int rowsAffected = 0;

            string UpdateContact_query = @"Update  Contacts  
                            set FirstName = @FirstName, 
                                LastName = @LastName, 
                                Email = @Email, 
                                Phone = @Phone, 
                                Address = @Address, 
                                DateOfBirth = @DateOfBirth,
                                CountryID = @CountryID,
                                ImagePath =@ImagePath
                                where ContactID = @ContactID"
            ;

            SqlCommand UpdateContact_Command = new SqlCommand(UpdateContact_query, connection);

            UpdateContact_Command.Parameters.AddWithValue("@ContactID", ContactID);
            UpdateContact_Command.Parameters.AddWithValue("@FirstName", FirstName);
            UpdateContact_Command.Parameters.AddWithValue("@LastName", LastName);
            UpdateContact_Command.Parameters.AddWithValue("@Email", Email);
            UpdateContact_Command.Parameters.AddWithValue("@Phone", Phone);
            UpdateContact_Command.Parameters.AddWithValue("@Address", Address);
            UpdateContact_Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            UpdateContact_Command.Parameters.AddWithValue("@CountryID", CountryID);
            
            if (ImagePath != "")
            {

                UpdateContact_Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            } 
            else
            {
                UpdateContact_Command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            }

            try
            {
                connection.Open();
                rowsAffected = UpdateContact_Command.ExecuteNonQuery();

            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
                return false;
            }
            finally
            {             
                connection.Close();
            }
            return (rowsAffected > 0);
        }

        public static bool DeleteContact(int ContactID)
        {
            int rowsAffected = 0;

            string Delete_query = @"Delete Contacts 
                                where ContactID = @ContactID";
            SqlCommand Delete_Command = new SqlCommand(Delete_query, connection);

            Delete_Command.Parameters.AddWithValue("@ContactID", ContactID);

            try
            {
                connection.Open();

                rowsAffected = Delete_Command.ExecuteNonQuery();

            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }

        public static DataTable GetAllContacts()
        {
            DataTable dt = new DataTable();
            string SelectAll_Query = @"Select * From Contacts";

            SqlCommand SelectAll_Command = new SqlCommand(SelectAll_Query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = SelectAll_Command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();

            }

            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static bool IsContactExist(int ContactID)
        {
            bool isFound = false;

            string IsContactFound_Query = @"select Found= 1 From Contacts Where ContactID = @ContactID";
            
            SqlCommand IsContactFound_Command = new SqlCommand(IsContactFound_Query, connection);

            IsContactFound_Command.Parameters.AddWithValue("@ContactID", ContactID);

            try
            {
                connection.Open();
                SqlDataReader reader = IsContactFound_Command.ExecuteReader();

                isFound = reader.HasRows;
                reader.Close();
            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        // Country Code---
        public static bool FindCountryByName(ref int CountryID , ref string CountryName ,ref string CountryCode , ref string CountryPhoneCode )
        {
            bool isFound = false;

            string FindCountryByName_query = @"select * From Countries where CountryName = @CountryName ";

            SqlCommand FindCountryByName_Command = new SqlCommand(FindCountryByName_query, connection);

            FindCountryByName_Command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();
                SqlDataReader reader = FindCountryByName_Command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    CountryID = (int)reader["CountryID"];
                    CountryName = (string)reader["CountryName"];
                    
                    // We Can here Compelete All Data -> (Country Code , Country Phone Number )  <- But Check Null Values First

                }
                reader.Close();
            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsCountryExistByName(string CountryName)
        {
            bool isFound = false;

            string IsCountryExistByName_query = @"select Found= 1 From Countries Where CountryName = @CountryName";

            SqlCommand IsCountryExistByName_Command = new SqlCommand(IsCountryExistByName_query, connection);
            IsCountryExistByName_Command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();
                SqlDataReader reader = IsCountryExistByName_Command.ExecuteReader();
                isFound = reader.HasRows;
                reader.Close();
            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static int AddNewCountry(string CountryName, string Code , string PhoneCode)
        {
            int CountryID = 0;

            string AddNewCountry_query = @"insert into Countries values(@CountryName, @Code , @PhoneCode);
                                           SELECT SCOPE_IDENTITY();";

            SqlCommand AddNewCountry_Command = new SqlCommand(AddNewCountry_query, connection);

            AddNewCountry_Command.Parameters.AddWithValue("@CountryName", CountryName);
            AddNewCountry_Command.Parameters.AddWithValue("@Code", Code);
            AddNewCountry_Command.Parameters.AddWithValue("@PhoneCode", PhoneCode);

            try
            {
                connection.Open();

                object result = AddNewCountry_Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                {
                    CountryID = InsertedID;
                }

            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
            }
            finally
            {
                connection.Close();
            }
            return CountryID;
        }
        public static bool UpdateCountry(int CountryID ,string CountryName, string Code , string PhoneCode)
        {
            int RowsAffected = 0;

            string UpdateCountry_query = @"update Countries 
                                                set CountryName = @CountryName
	                                                , Code = @CountryCode
	                                                , PhoneCode = @CountryPhoneCode
	                                                where CountryID = @CountryID;";

            SqlCommand UpdateCountry_Command = new SqlCommand(UpdateCountry_query, connection);

            UpdateCountry_Command.Parameters.AddWithValue("@CountryID", CountryID);
            UpdateCountry_Command.Parameters.AddWithValue("@CountryName", CountryName);
            UpdateCountry_Command.Parameters.AddWithValue("@CountryCode", Code);
            UpdateCountry_Command.Parameters.AddWithValue("@CountryPhoneCode", PhoneCode);

            try
            {
                connection.Open();

                RowsAffected = UpdateCountry_Command.ExecuteNonQuery();

            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
            }
            finally
            {
                connection.Close();
            }
            return (RowsAffected > 0);
        }

        public static bool DeleteCountry(string CountryName)
        {
            int rowsAffected = 0;

            string Delete_query = @"Delete Countries 
                                where CountryName = @CountryName";
            SqlCommand Delete_Command = new SqlCommand(Delete_query, connection);

            Delete_Command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();

                rowsAffected = Delete_Command.ExecuteNonQuery();

            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);


        }

        public static DataTable GetAllCountries()
        {
            DataTable table = new DataTable();

            string GetAllCountries_query = @"select * From Countries";
            SqlCommand GetAllCountries_Command = new SqlCommand(GetAllCountries_query, connection);


            try
            {
                connection.Open();
                SqlDataReader reader = GetAllCountries_Command.ExecuteReader();

                table.Load(reader);

                reader.Close();
            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
            }
            finally
            {
                connection.Close();
            }
            return table;

        }



        public void haha()
        {
            try
            {
                connection.Open();
            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
