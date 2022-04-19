using System;
public class Login
{
    public static string ConnectionString;
    public static void GetConnectionStringFromXML()
    {
        var reader = new System.Xml.Serialization.XmlSerializer(typeof(String));
        var file = new System.IO.StreamReader(@".\ConnectionString.xml");
        Login.ConnectionString = (String)reader.Deserialize(file);
        file.Close();
    }
}