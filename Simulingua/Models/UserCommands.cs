using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class UserCommands
{

    #region ADD NEW USER
    public static string addNewUser(string uEmail, string uName, DateTime bDay, string oLang, string tLang, int ptLang)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand addUser = new SqlCommand("insert into userInfo (uEmail, uName, uBirthday, uJoinDate, uOriginLanguage) values (@email, @name, @birthday, @joinDate, @originLanguage);", entry);
        addUser.Parameters.AddWithValue("@email", uEmail);
        addUser.Parameters.AddWithValue("@name", uName);
        addUser.Parameters.AddWithValue("@birthday", bDay);
        addUser.Parameters.AddWithValue("@joinDate", DateTime.Today);
        addUser.Parameters.AddWithValue("@originLanguage", oLang);
        try
        {
            entry.Open();
            addUser.ExecuteNonQuery();
            addNewTargetLang(uEmail, tLang, ptLang);
            entry.Close();
            ActionLogger.log("Add new user " + uEmail, "SUCCESS");
        }
        catch (SqlException e)
        {
            entry.Close();
            ActionLogger.log("Add new user " + uEmail, "FAILURE");
            return e.Message;
        }
        return "New user successfully added to the database.";
    }
    #endregion

    #region ADD NEW TARGET LANGUAGE
    public static string addNewTargetLang(string uEmail, string tLang, int ptLang)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand addTargetLanguage = new SqlCommand("insert into userTargetLang (userID, uTargetLanguage, uProficiency) values (@userID, @targetLanguage, @proficiency);", entry);
        addTargetLanguage.Parameters.AddWithValue("@email", uEmail);
        addTargetLanguage.Parameters.AddWithValue("@targetLanguage", tLang);
        addTargetLanguage.Parameters.AddWithValue("@proficiency", ptLang);
        addTargetLanguage.Parameters.AddWithValue("@userID", OtherCommands.fetchUserID(uEmail));

        try
        {
            entry.Open();
            addTargetLanguage.ExecuteNonQuery();
            entry.Close();
            ActionLogger.log("Add new user target language for " + uEmail, "SUCCESS");
        }
        catch (SqlException e)
        {
            entry.Close();
            ActionLogger.log("Add new user target language for " + uEmail, "FAILURE");
            return e.Message;
        }
        return "User's target language successfully added to the database.";
    }
    #endregion

    #region UPDATE USER
    public static string updateUser(string uEmail, string nuEmail, string nuName)
    {
        if (nuName == null) { nuName = OtherCommands.fetchUserName(uEmail); }
        if (nuEmail == null) { nuEmail = uEmail; }
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand updateUser = new SqlCommand("update userInfo set uName = @nuName, uEmail = @nuEmail where uEmail=@uEmail", entry);
        updateUser.Parameters.AddWithValue("@uEmail", uEmail);
        updateUser.Parameters.AddWithValue("@nuName", nuName);
        updateUser.Parameters.AddWithValue("@nuEmail", nuEmail);
        try
        {
            entry.Open();
            updateUser.ExecuteNonQuery();
            entry.Close();
            ActionLogger.log("Update user info for " + uEmail, "SUCCESS");
        }
        catch (SqlException e)
        {
            entry.Close();
            ActionLogger.log("Update user info for " + uEmail, "FAILURE");
            return e.Message;
        }
        return "User info updated successfully.";
    }
    #endregion

    #region UPDATE TARGET LANGUAGE PROFICIENCY
    public static string updateTargetLanguageProficiency(string uEmail, string uTargetLanguage, int uProficiency)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand updateProficiency = new SqlCommand("update userTargetLang set uProficiency = @uProficiency where userID=@userID and uTargetLanguage=@uTargetLanguage", entry);
        updateProficiency.Parameters.AddWithValue("@uTargetLanguage", uTargetLanguage);
        updateProficiency.Parameters.AddWithValue("@uProficiency", uProficiency);
        updateProficiency.Parameters.AddWithValue("@userID", OtherCommands.fetchUserID(uEmail));

        try
        {
            entry.Open();
            updateProficiency.ExecuteNonQuery();
            entry.Close();
            ActionLogger.log("Update language proficiency for " + uEmail, "SUCCESS");
        }
        catch (SqlException e)
        {
            entry.Close();
            ActionLogger.log("Update language proficiency for " + uEmail, "FAILURE");
            return e.Message;
        }
        return "User's language proficiency updated successfully.";
    }
    #endregion

    #region FETCH USER INFO
    public static string[] fetchUserInfo(string uEmail)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand readDB = new SqlCommand("select * from userInfo where uEmail=@uEmail", entry);
        readDB.Parameters.AddWithValue("@uEmail", uEmail);
        SqlDataReader reader = null;
        string[] info = new string[5];

        try
        {
            entry.Open();
            reader = readDB.ExecuteReader();
            while (reader.Read())
            {
                info[0] = "Email: " + reader[1].ToString();
                info[1] = "Name: " + reader[2].ToString();
                DateTime bDay = (DateTime)reader[3];
                info[2] = "Birthday: " + bDay.ToString("MMM d, yyyy");
                DateTime jDay = (DateTime)reader[4];
                info[3] = "Simulingua member since " + jDay.ToString("MMM d, yyyy");
                info[4] = "Native language: " + reader[5].ToString();
            }
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch user info for " + uEmail, "SUCCESS");
        }
        catch (SqlException e)
        {
            reader.Close();
            entry.Close();
            info[0] = e.Message;
            ActionLogger.log("Fetch user info for " + uEmail, "FAILURE");
        }
        if (info[0] == null) { string[] badEmail = new string[1] { "Invalid Email." }; return badEmail; }
        return info;
    }

    #endregion

    #region FETCH USER TARGET LANGUAGES
    public static List<string[]> fetchUserTargetLanguages(string uEmail)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand readDB = new SqlCommand("select * from userTargetLang where userID=@userID", entry);
        readDB.Parameters.AddWithValue("@userID", OtherCommands.fetchUserID(uEmail));
        SqlDataReader reader = null;
        string[] info = new string[2];
        var infoCom = new List<string[]>();

        try
        {
            entry.Open();
            reader = readDB.ExecuteReader();
            while (reader.Read())
            {
                info[0] = "Language: " + reader[1].ToString();
                info[1] = "Proficiency: " + reader[2].ToString();
                infoCom.Add(new string[2] { info[0], info[1] });
            }
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch all target languages for " + uEmail, "SUCCESS");
        }
        catch (SqlException e)
        {
            info[0] = e.Message;
            infoCom.Add(info);
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch all target languages for " + uEmail, "FAILURE");
        }
        return infoCom;
    }
    #endregion

    #region FETCH CUSTOMER RECIEPTS
    public static List<string[]> fetchCustomerReciepts(string uEmail)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand readDB = new SqlCommand("select * from purchaseHistory where userID=@userID", entry);
        readDB.Parameters.AddWithValue("@userID", OtherCommands.fetchUserID(uEmail));
        SqlDataReader reader = null;
        string[] info = new string[4];
        var infoCom = new List<string[]>();

        try
        {
            entry.Open();
            reader = readDB.ExecuteReader();
            while (reader.Read())
            {
                int x = (int)reader[1];
                info[0] = "Customer name: " + OtherCommands.fetchUserName(x);
                x = (int)reader[0];
                info[1] = OtherCommands.fetchCourseOriginLang(x) + " to " + OtherCommands.fetchCourseTargetLang(x) + " language course";
                info[2] = "Purchase Cost: $" + String.Format("{0:0.##}", reader[2]);
                DateTime pDay = (DateTime)reader[3];
                info[3] = "Purchase Date: " + pDay.ToString("MMM d, yyyy");
                infoCom.Add(new string[4] { info[0], info[1], info[2], info[3] });
            }
            ActionLogger.log("Fetch reciepts for " + uEmail, "SUCCESS");
        }
        catch (SqlException e)
        {
            info[0] = e.Message;
            infoCom.Add(info);
            ActionLogger.log("Fetch reciepts for " + uEmail, "FAILURE");
        }
        finally
        {
            reader.Close();
            entry.Close();
        }
        return infoCom;
    }
    #endregion

    #region DELETE USER TARGET LANGUAGE
    public static string deleteUserTargetLanguage(string uEmail, string uTargetLanguage)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand deleteUserTargetLanguage = new SqlCommand("delete from userTargetLang where userID=@userID and uTargetLanguage=@uTargetLanguage", entry);
        deleteUserTargetLanguage.Parameters.AddWithValue("@userID", OtherCommands.fetchUserID(uEmail));
        deleteUserTargetLanguage.Parameters.AddWithValue("@uTargetLanguage", uTargetLanguage);
        try
        {
            entry.Open();
            deleteUserTargetLanguage.ExecuteNonQuery();
            entry.Close();
            ActionLogger.log("Delete target language for " + uEmail, "SUCCESS");
        }
        catch (SqlException e)
        {
            entry.Close();
            ActionLogger.log("Delete target language for " + uEmail, "FAILURE");
            return e.Message;
        }
        return "User data deleted.";
    }
    #endregion

    #region DELETE USER INFO
    public static string deleteUserInfo(string uEmail)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand deleteUserInfo = new SqlCommand("delete from userInfo where uEmail=@uEmail", entry);
        deleteUserInfo.Parameters.AddWithValue("@uEmail", uEmail);
        try
        {
            entry.Open();
            OtherCommands.deleteAllUserTargetLanguages(uEmail);
            OtherCommands.deleteAllUserReciepts(uEmail);
            deleteUserInfo.ExecuteNonQuery();
            entry.Close();
            ActionLogger.log("Delete user info for " + uEmail, "SUCCESS");
        }
        catch (SqlException e)
        {
            entry.Close();
            ActionLogger.log("Delete user info for " + uEmail, "FAILURE");
            return e.Message;
        }
        return "User data deleted.";
    }
    #endregion


}