using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class SqlTableOperations
{
    #region ADD NEW USER
    public static string addNewUser(string uEmail, string uName, DateTime bDay, string oLang, string tLang, int ptLang)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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

    #region UPDATE USER
    public static string updateUser(string uEmail, string nuEmail, string nuName)
    {
        if(nuName == null) { nuName = fetchUserName(uEmail); }
        if(nuEmail == null) { nuEmail = uEmail; }
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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

    #region FETCH USER INFO
    public static string[] fetchUserInfo(string uEmail)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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

    #region DELETE USER INFO
    public static string deleteUserInfo(string uEmail)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
        SqlCommand deleteUserInfo = new SqlCommand("delete from userInfo where uEmail=@uEmail", entry);
        deleteUserInfo.Parameters.AddWithValue("@uEmail", uEmail);
        try
        {
            entry.Open();
            deleteAllUserTargetLanguages(uEmail);
            deleteAllUserReciepts(uEmail);
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

    #region ADD NEW TARGET LANGUAGE
    public static string addNewTargetLang(string uEmail, string tLang, int ptLang)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
        SqlCommand addTargetLanguage = new SqlCommand("insert into userTargetLang (userID, uTargetLanguage, uProficiency) values (@userID, @targetLanguage, @proficiency);", entry);
        addTargetLanguage.Parameters.AddWithValue("@email", uEmail);
        addTargetLanguage.Parameters.AddWithValue("@targetLanguage", tLang);
        addTargetLanguage.Parameters.AddWithValue("@proficiency", ptLang);
        addTargetLanguage.Parameters.AddWithValue("@userID", SqlTableOperations.fetchUserID(uEmail));

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

    #region FETCH USER NAME
    public static string fetchUserName(string uEmail)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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

    #region UPDATE TARGET LANGUAGE PROFICIENCY
    public static string updateTargetLanguageProficiency(string uEmail, string uTargetLanguage, int uProficiency)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
        SqlCommand updateProficiency = new SqlCommand("update userTargetLang set uProficiency = @uProficiency where userID=@userID and uTargetLanguage=@uTargetLanguage", entry);
        updateProficiency.Parameters.AddWithValue("@uTargetLanguage", uTargetLanguage);
        updateProficiency.Parameters.AddWithValue("@uProficiency", uProficiency);
        updateProficiency.Parameters.AddWithValue("@userID", SqlTableOperations.fetchUserID(uEmail));

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

    #region FETCH USER TARGET LANGUAGES
    public static List<string[]> fetchUserTargetLanguages(string uEmail)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
        SqlCommand readDB = new SqlCommand("select * from userTargetLang where userID=@userID", entry);
        readDB.Parameters.AddWithValue("@userID", SqlTableOperations.fetchUserID(uEmail));
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

    #region DELETE USER TARGET LANGUAGE
    public static string deleteUserTargetLanguage(string uEmail, string uTargetLanguage)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
        SqlCommand deleteUserTargetLanguage = new SqlCommand("delete from userTargetLang where userID=@userID and uTargetLanguage=@uTargetLanguage", entry);
        deleteUserTargetLanguage.Parameters.AddWithValue("@userID", SqlTableOperations.fetchUserID(uEmail));
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

    #region DELETE ALL USER TARGET LANGUAGES
    public static string deleteAllUserTargetLanguages(string uEmail)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
        SqlCommand deleteUserTargetLanguage = new SqlCommand("delete from userTargetLang where userID=@userID", entry);
        deleteUserTargetLanguage.Parameters.AddWithValue("@userID", SqlTableOperations.fetchUserID(uEmail));
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
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
        SqlCommand deleteUserReciepts = new SqlCommand("delete from purchaseHistory where userID=@userID", entry);
        deleteUserReciepts.Parameters.AddWithValue("@userID", SqlTableOperations.fetchUserID(uEmail));
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

    #region ADD NEW LANGUAGE
    public static string addNewLanguage(string aLanguage)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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

    #region FETCH STORE MENU
    public static List<string[]> fetchStoreMenu()
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
        SqlCommand readDB = new SqlCommand("select * from buyMenu where price > 0", entry);
        SqlDataReader reader = null;
        string[] info = new string[5];
        var infoCom = new List<string[]>();

        try
        {
            entry.Open();
            reader = readDB.ExecuteReader();
            while (reader.Read())
            {
                info[0] = "Item ID: " + reader[0].ToString();
                info[1] = "Origin Language: " + reader[1].ToString();
                info[2] = "Target Language: " + reader[2].ToString();
                info[3] = "Proficiency Level: " + reader[3].ToString();
                info[4] = "Price: $" + String.Format("{0:0.##}", reader[4]);
                infoCom.Add(new string[5] { info[0], info[1], info[2], info[3], info[4] });
            }
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch store menu", "SUCCESS");
        }
        catch (SqlException e)
        {
            info[0] = e.Message;
            infoCom.Add(info);
            reader.Close();
            entry.Close();
            ActionLogger.log("Fetch store menu", "FAILURE");
        }
        return infoCom;
    }
    #endregion

    #region FETCH ALL USER INFO
    public static List<string[]> fetchAllUserInfo()
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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

    #region DELETE LANGUAGE COURSE
    public static string deleteLanguageCourse(int itemID)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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

    #region UPDATE LANGUAGE COURSE PRICE
    public static string updateLanguageCoursePrice(string itemID, double price)
    {
        if (price < 0) { return "Invalid input. Price must be higher than zero."; }
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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

    #region FETCH PRICE
    public static double fetchPrice(int itemID)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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

    #region USER ACTION PURCHASE LANGUAGE COURSE
    public static string userActionPurchaseLanguageCourse(string uEmail, int itemID)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
        SqlCommand userActionPurchaseLanguageCourse = new SqlCommand("insert into purchaseHistory (itemID, userID, purchaseCost, purchaseDate) values (@itemID, @userID, @purchaseCost, @purchaseDate);", entry);
        userActionPurchaseLanguageCourse.Parameters.AddWithValue("@userID", SqlTableOperations.fetchUserID(uEmail));
        userActionPurchaseLanguageCourse.Parameters.AddWithValue("@itemID", itemID);
        userActionPurchaseLanguageCourse.Parameters.AddWithValue("@purchaseCost", SqlTableOperations.fetchPrice(itemID));
        userActionPurchaseLanguageCourse.Parameters.AddWithValue("@purchaseDate", DateTime.Today);
        try
        {
            entry.Open();
            userActionPurchaseLanguageCourse.ExecuteNonQuery();
            entry.Close();
            ActionLogger.log("Purchase language course for " + uEmail, "SUCCESS");
            return "Language course purchased successfully.";
        }
        catch (SqlException e)
        {
            entry.Close();
            ActionLogger.log("Purchase language course for " + uEmail, "FAILURE");
            return e.Message;
        }
    }
    #endregion

    #region FETCH COURSE ORIGIN LANG
    public static string fetchCourseOriginLang(int itemID)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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

    #region FETCH CUSTOMER RECIEPTS
    public static List<string[]> fetchCustomerReciepts(string uEmail)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
        SqlCommand readDB = new SqlCommand("select * from purchaseHistory where userID=@userID", entry);
        readDB.Parameters.AddWithValue("@userID", SqlTableOperations.fetchUserID(uEmail));
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
                info[0] = "Customer name: " + fetchUserName(x);
                x = (int)reader[0];
                info[1] = fetchCourseOriginLang(x) + " to " + fetchCourseTargetLang(x) + " language course";
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

    #region FETCH ALL CUSTOMER RECIEPTS
    public static List<string[]> fetchAllCustomerReciepts()
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
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
                info = fetchCustomerReciepts(reader[1].ToString());
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