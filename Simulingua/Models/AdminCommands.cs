using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class AdminCommands
{

    #region ADD NEW LANGUAGE
    public static string addNewLanguage(string aLanguage)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand addNewLanguage = new SqlCommand("insert into languageList (aLanguage) values (@aLanguage);", entry);
        addNewLanguage.Parameters.AddWithValue("@aLanguage", aLanguage);
        try
        {
            entry.Open();
            addNewLanguage.ExecuteNonQuery();
            entry.Close();
            ActionLogger.log("Add new language", "SUCCESS");
        }
        catch (SqlException e)
        {
            entry.Close();
            ActionLogger.log("Add new language", "FAILURE");
            return e.Message;
        }
        return "Language successfully added to the database.";
    }
    #endregion

    #region ADD NEW LANGUAGE COURSE
    public static string addNewLanguageCourse(string originLanguage, string targetLanguage, int proficiencyLevel, double price)
    {
        if (price < 0) { return "Invalid input. Price must be higher than zero."; }
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand addNewLanguageCourse = new SqlCommand("insert into buyMenu (originLanguage, targetLanguage, proficiencyLevel, price) values (@originLanguage, @targetLanguage, @proficiencyLevel, @price);", entry);
        addNewLanguageCourse.Parameters.AddWithValue("@originLanguage", originLanguage);
        addNewLanguageCourse.Parameters.AddWithValue("@targetLanguage", targetLanguage);
        addNewLanguageCourse.Parameters.AddWithValue("@proficiencyLevel", proficiencyLevel);
        addNewLanguageCourse.Parameters.AddWithValue("@price", price);
        try
        {
            entry.Open();
            addNewLanguageCourse.ExecuteNonQuery();
            entry.Close();
            ActionLogger.log("Add new language course for " + targetLanguage, "SUCCESS");
        }
        catch (SqlException e)
        {
            entry.Close();
            ActionLogger.log("Add new language course for " + targetLanguage, "FAILURE");
            return e.Message;
        }
        return "Language course successfully added to the buy menu.";
    }
    #endregion

    #region UPDATE LANGUAGE COURSE PRICE
    public static string updateLanguageCoursePrice(string itemID, double price)
    {
        if (price < 0) { return "Invalid input. Price must be higher than zero."; }
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand updatePrice = new SqlCommand("update buyMenu set price=@price where itemID=@itemID", entry);
        updatePrice.Parameters.AddWithValue("@itemID", itemID);
        updatePrice.Parameters.AddWithValue("@price", price);
        try
        {
            entry.Open();
            updatePrice.ExecuteNonQuery();
            entry.Close();
            ActionLogger.log("Update language course price for item ID " + itemID, "SUCCESS");
        }
        catch (SqlException e)
        {
            entry.Close();
            ActionLogger.log("Update language course price for item ID " + itemID, "FAILURE");
            return e.Message;
        }
        return "Course price updated.";
    }
    #endregion

    #region DELETE LANGUAGE COURSE
    public static string deleteLanguageCourse(int itemID)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand deleteLanguageCourse = new SqlCommand("update buyMenu set price=@price where itemID=@itemID", entry);
        deleteLanguageCourse.Parameters.AddWithValue("@itemID", itemID);
        deleteLanguageCourse.Parameters.AddWithValue("@price", 0);
        try
        {
            entry.Open();
            deleteLanguageCourse.ExecuteNonQuery();
            entry.Close();
            ActionLogger.log("Remove language course for item ID " + itemID, "SUCCESS");
        }
        catch (SqlException e)
        {
            entry.Close();
            ActionLogger.log("Remove language course for item ID " + itemID, "FAILURE");
            return e.Message;
        }
        return "Language course removed from buy menu.";
    }
    #endregion

    #region FETCH ALL USER INFO
    public static List<string[]> fetchAllUserInfo()
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand readDB = new SqlCommand("select * from userInfo", entry);
        SqlDataReader reader = null;
        string[] info = new string[6];
        var infoCom = new List<string[]>();

        try
        {
            entry.Open();
            reader = readDB.ExecuteReader();
            while (reader.Read())
            {
                info[0] = "User ID: " + reader[0].ToString();
                info[1] = "Email: " + reader[1].ToString();
                info[2] = "Name: " + reader[2].ToString();
                DateTime bDay = (DateTime)reader[3];
                info[3] = "Birthday: " + bDay.ToString("MMM d, yyyy");
                DateTime jDay = (DateTime)reader[4];
                info[4] = "Simulingua member since " + jDay.ToString("MMM d, yyyy");
                info[5] = "Origin language: " + reader[5].ToString();
                infoCom.Add(new string[6] { info[0], info[1], info[2], info[3], info[4], info[5] });
            }
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch list of all users", "SUCCESS");
        }
        catch (SqlException e)
        {
            info[0] = e.Message;
            infoCom.Add(info);
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch list of all users", "FAILURE");
        }
        return infoCom;
    }
    #endregion

    #region FETCH ALL CUSTOMER RECIEPTS
    public static List<string[]> fetchAllCustomerReciepts()
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand readDB = new SqlCommand("select * from userInfo", entry);
        SqlDataReader reader = null;
        var info = new List<string[]>();
        var infoCom = new List<string[]>();

        try
        {
            entry.Open();
            reader = readDB.ExecuteReader();
            while (reader.Read())
            {
                info = UserCommands.fetchCustomerReciepts(reader[1].ToString());
                foreach (var i in info)
                {
                    infoCom.Add(i);
                }
            }
            ActionLogger.log("Fetch reciepts for all customers", "SUCCESS");
        }
        catch (SqlException e)
        {
            string[] error = new string[1] { e.Message };
            infoCom.Add(error);
            ActionLogger.log("Fetch reciepts for all customers", "FAILURE");
        }
        finally
        {
            reader.Close();
            entry.Close();
        }
        return infoCom;
    }
    #endregion

}