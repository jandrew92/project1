using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class OtherCommands
{

    #region FETCH USER NAME
    public static string fetchUserName(string uEmail)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand readDB = new SqlCommand("select * from userInfo where uEmail=@uEmail", entry);
        readDB.Parameters.AddWithValue("@uEmail", uEmail);
        SqlDataReader reader = null;
        string uName = null;
        try
        {
            entry.Open();
            reader = readDB.ExecuteReader();
            while (reader.Read())
            {
                uName = reader[2].ToString();
            }
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch user name for " + uEmail, "SUCCESS");
            return uName;
        }
        catch (SqlException)
        {
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch user name for " + uEmail, "FAILURE");
            return uName;
        }
    }
    public static string fetchUserName(int userID)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand readDB = new SqlCommand("select * from userInfo where userID=@userID", entry);
        readDB.Parameters.AddWithValue("@userID", userID);
        SqlDataReader reader = null;
        string uName = null;
        try
        {
            entry.Open();
            reader = readDB.ExecuteReader();
            while (reader.Read())
            {
                uName = reader[2].ToString();
            }
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch user name for user ID " + userID, "SUCCESS");
            return uName;
        }
        catch (SqlException)
        {
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch user name for user ID " + userID, "FAILURE");
            return uName;
        }
    }
    #endregion

    #region FETCH USER ID
    public static int fetchUserID(string uEmail)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand readDB = new SqlCommand("select * from userInfo where uEmail=@uEmail", entry);
        readDB.Parameters.AddWithValue("@uEmail", uEmail);
        SqlDataReader reader = null;
        int userID = -1;
        try
        {
            entry.Open();
            reader = readDB.ExecuteReader();
            while (reader.Read())
            {
                userID = (int)reader[0];
            }
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch user ID for " + uEmail, "SUCCESS");
            return userID;
        }
        catch (SqlException)
        {
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch user ID for " + uEmail, "FAILURE");
            return userID;
        }
    }
    #endregion

    #region DELETE ALL USER TARGET LANGUAGES
    public static string deleteAllUserTargetLanguages(string uEmail)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand deleteUserTargetLanguage = new SqlCommand("delete from userTargetLang where userID=@userID", entry);
        deleteUserTargetLanguage.Parameters.AddWithValue("@userID", OtherCommands.fetchUserID(uEmail));
        try
        {
            entry.Open();
            deleteUserTargetLanguage.ExecuteNonQuery();
            entry.Close();
            ActionLogger.log("Delete all target languages for " + uEmail, "SUCCESS");
        }
        catch (SqlException e)
        {
            entry.Close();
            ActionLogger.log("Delete all target languages for " + uEmail, "FAILURE");
            return e.Message;
        }
        return "User data deleted.";
    }
    #endregion

    #region DELETE ALL USER RECIEPTS
    public static string deleteAllUserReciepts(string uEmail)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand deleteUserReciepts = new SqlCommand("delete from purchaseHistory where userID=@userID", entry);
        deleteUserReciepts.Parameters.AddWithValue("@userID", OtherCommands.fetchUserID(uEmail));
        try
        {
            entry.Open();
            deleteUserReciepts.ExecuteNonQuery();
            entry.Close();
            ActionLogger.log("Delete all reciepts for " + uEmail, "SUCCESS");
        }
        catch (SqlException e)
        {
            entry.Close();
            ActionLogger.log("Delete all reciepts for " + uEmail, "FAILURE");
            return e.Message;
        }
        return "User data deleted.";
    }
    #endregion

    #region FETCH PRICE
    public static double fetchPrice(int itemID)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand readDB = new SqlCommand("select * from buyMenu where itemID=@itemID", entry);
        readDB.Parameters.AddWithValue("@itemID", itemID);
        SqlDataReader reader = null;
        double price = -1;
        try
        {
            entry.Open();
            reader = readDB.ExecuteReader();
            while (reader.Read())
            {
                price = Convert.ToDouble(reader[4]);
            }
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch price for item ID " + itemID, "SUCCESS");
            return price;
        }
        catch (SqlException)
        {
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch price for item ID " + itemID, "FAILURE");
            return price;
        }
    }
    #endregion

    #region FETCH COURSE ORIGIN LANG
    public static string fetchCourseOriginLang(int itemID)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand readDB = new SqlCommand("select * from buyMenu where itemID=@itemID", entry);
        readDB.Parameters.AddWithValue("@itemID", itemID);
        SqlDataReader reader = null;
        string info = null;

        try
        {
            entry.Open();
            reader = readDB.ExecuteReader();
            while (reader.Read())
            {
                info = reader[1].ToString();
            }
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch origin lang for " + itemID, "SUCCESS");
        }
        catch (SqlException e)
        {
            reader.Close();
            entry.Close();
            info = e.Message;
            ActionLogger.log("Fetch origin lang for " + itemID, "FAILURE");
        }
        return info;
    }
    #endregion

    #region FETCH COURSE TARGET LANG
    public static string fetchCourseTargetLang(int itemID)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand readDB = new SqlCommand("select * from buyMenu where itemID=@itemID", entry);
        readDB.Parameters.AddWithValue("@itemID", itemID);
        SqlDataReader reader = null;
        string info = null;

        try
        {
            entry.Open();
            reader = readDB.ExecuteReader();
            while (reader.Read())
            {
                info = reader[2].ToString();
            }
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch target lang for " + itemID, "SUCCESS");
        }
        catch (SqlException e)
        {
            reader.Close();
            entry.Close();
            info = e.Message;
            ActionLogger.log("Fetch target lang for " + itemID, "FAILURE");
        }
        return info;
    }
    #endregion

}