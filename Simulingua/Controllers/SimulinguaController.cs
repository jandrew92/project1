using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simulingua.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminActionsController : ControllerBase
    {

        [HttpPost]
        [Route("add-new-language")]
        public IActionResult addNewLanguage(string aLanguage)
        {
            try
            {
                return Ok(AdminCommands.addNewLanguage(aLanguage));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("add-new-language-course")]
        public IActionResult addNewLanguageCourse(string originLanguage, string targetLanguage, int proficiencyLevel, double price)
        {
            if (proficiencyLevel < 1 || proficiencyLevel > 6) { return BadRequest("Invalid language proficiency. Simulingua ranks proficiency on a scale from 1 to 6."); }
            if (price < 0) { return BadRequest("Invalid price. Input a value higher than 0."); }
            try
            {
                return Ok(AdminCommands.addNewLanguageCourse(originLanguage, targetLanguage, proficiencyLevel, price));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("update-language-course-price")]
        public IActionResult updateLanguageCoursePrice(string itemID, double price)
        {
            if (price < 0) { return BadRequest("Invalid price. Input a value higher than 0."); }
            try
            {
                return Ok(AdminCommands.updateLanguageCoursePrice(itemID, price));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("delete-language-course")]
        public IActionResult deleteLanguageCourse(int itemID)
        {
            try
            {
                return Ok(AdminCommands.deleteLanguageCourse(itemID));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("fetch-all-user-data")]
        public IActionResult fetchAllUserInfo()
        {
            try
            {
                return Ok(AdminCommands.fetchAllUserInfo());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("fetch-all-user-receipts")]
        public IActionResult fetchAllCustomerReciepts()
        {
            try
            {
                return Ok(AdminCommands.fetchAllCustomerReciepts());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
    public class StoreActionsController : ControllerBase
    {

        [HttpPost]
        [Route("user-action-purchase-language-course")]
        public IActionResult userActionPurchaseLanguageCourse(string uEmail, int itemID)
        {
            try
            {
                return Ok(StoreCommands.userActionPurchaseLanguageCourse(uEmail, itemID));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("fetch-store-menu")]
        public IActionResult fetchStoreMenu()
        {
            try
            {
                return Ok(StoreCommands.fetchStoreMenu());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
    public class UserInfoController : ControllerBase
    {
        [HttpPost]
        [Route("new-user")]
        public IActionResult addNewUser(string uEmail, string uName, int bYear, int bMonth, int bDay, string oLang, string tLang, int ptLang)
        {
            try
            {
                if (!uEmail.Contains("@")) { return BadRequest("Invalid email."); }
                int n = int.Parse(DateTime.Today.ToString("yyyy"));
                if (bYear < 1900 || bYear > n) { return BadRequest("Invalid birth year. Select a year between 1900 and " + n + "."); }
                if (bMonth < 1 || bMonth > 12) { return BadRequest("Invalid birth month."); }
                if (bDay < 1 || bDay > 31) { return BadRequest("Invalid birth day."); }
                if (ptLang < 1 || ptLang > 6) { return BadRequest("Invalid language proficiency. Simulingua ranks proficiency on a scale from 1 to 6."); }
                DateTime bDate = new DateTime(bYear, bMonth, bDay);
                return Ok(UserCommands.addNewUser(uEmail, uName, bDate, oLang, tLang, ptLang));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("add-new-target-language")]
        public IActionResult addNewTargetLanguage(string uEmail, string tLang, int ptLang)
        {
            if (ptLang < 1 || ptLang > 6) { return BadRequest("Invalid language proficiency. Simulingua ranks proficiency on a scale from 1 to 6."); }
            try
            {
                return Ok(UserCommands.addNewTargetLang(uEmail, tLang, ptLang));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("update-user")]
        public IActionResult updateUser(string uEmail, string nuEmail, string nuName)
        {
            if (nuEmail != null)
            {
                if (!nuEmail.Contains("@")) { return BadRequest("Invalid email."); }
            }
            try
            {
                return Ok(UserCommands.updateUser(uEmail, nuEmail, nuName));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("update-target-language-proficiency")]
        public IActionResult updateTargetLanguageProficiency(string uEmail, string uTargetLanguage, int uProficiency)
        {
            if (uProficiency < 1 || uProficiency > 6) { return BadRequest("Invalid language proficiency. Simulingua ranks proficiency on a scale from 1 to 6."); }
            try
            {
                return Ok(UserCommands.updateTargetLanguageProficiency(uEmail, uTargetLanguage, uProficiency));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("fetch-user-info")]
        public IActionResult fetchUserInfo(string uEmail)
        {
            try
            {
                return Ok(UserCommands.fetchUserInfo(uEmail));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("fetch-user-target-languages")]
        public IActionResult fetchUserTargetLanguages(string uEmail)
        {
            try
            {
                return Ok(UserCommands.fetchUserTargetLanguages(uEmail));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("fetch-user-reciepts")]
        public IActionResult fetchCustomerReciepts(string uEmail)
        {
            try
            {
                return Ok(UserCommands.fetchCustomerReciepts(uEmail));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("delete-user-target-language")]
        public IActionResult deleteUserTargetLanguage(string uEmail, string uTargetLanguage)
        {
            try
            {
                return Ok(UserCommands.deleteUserTargetLanguage(uEmail, uTargetLanguage));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //cannot simply delete users anymore due to foreign key restraints
        //[HttpDelete]
        //[Route("delete-user-info")]
        //public IActionResult deleteUserInfo(string uEmail)
        //{
        //    try
        //    {
        //        return Ok(SqlTableOperations.deleteUserInfo(uEmail));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}
    }
}
