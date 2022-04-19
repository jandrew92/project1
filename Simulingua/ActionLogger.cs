using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class ActionLogger
{
    public static void log(string input, string output)
    {
        SqlConnection entry = new SqlConnection(@"server=DESKTOP-5M7ISTP\JUSTIN_INSTANCE;database=SimulinguaDB;User Id=sa;Password=Password123;");
        SqlCommand cmd = new SqlCommand("insert into actionLog (actionAttempted, actionOutcome, actionDate) values (@actionAttempted, @actionOutcome, @actionDate);", entry);
        cmd.Parameters.AddWithValue("@actionAttempted", input);
        cmd.Parameters.AddWithValue("@actionOutcome", output);
        cmd.Parameters.AddWithValue("@actionDate", DateTime.Now);
        try
        {
            entry.Open();
            cmd.ExecuteNonQuery();
            entry.Close();
        }
        catch (SqlException)
        {
            entry.Close();
        }
    }

}