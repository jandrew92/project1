using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class StoreCommands
{

    #region USER ACTION PURCHASE LANGUAGE COURSE
    public static string userActionPurchaseLanguageCourse(string uEmail, int itemID)
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
        SqlCommand userActionPurchaseLanguageCourse = new SqlCommand("insert into purchaseHistory (itemID, userID, purchaseCost, purchaseDate) values (@itemID, @userID, @purchaseCost, @purchaseDate);", entry);
        userActionPurchaseLanguageCourse.Parameters.AddWithValue("@userID", OtherCommands.fetchUserID(uEmail));
        userActionPurchaseLanguageCourse.Parameters.AddWithValue("@itemID", itemID);
        userActionPurchaseLanguageCourse.Parameters.AddWithValue("@purchaseCost", OtherCommands.fetchPrice(itemID));
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

    #region FETCH STORE MENU
    public static List<string[]> fetchStoreMenu()
    {
        SqlConnection entry = new SqlConnection(Login.ConnectionString);
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


}