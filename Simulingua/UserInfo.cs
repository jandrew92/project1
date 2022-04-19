using System;

namespace Simulingua
{
    public class UserInfo
    {
        public string userEmail;
        public string userName;
        public DateTime birthday;
        public DateTime joinDay;
        public string OriginLanguage;
        public string TargetLanguage;
        public string TargetProficiency;

        public UserInfo(string uEmail, string uName, DateTime bDay, string oLang, string tLang, string ptLang)
        {
            this.userEmail = uEmail;
            this.userName = uName;
            this.birthday = bDay;
            this.joinDay = DateTime.Today;
            this.OriginLanguage = oLang;
            this.TargetLanguage = tLang;
            this.TargetProficiency = ptLang;
        }
    }
}
