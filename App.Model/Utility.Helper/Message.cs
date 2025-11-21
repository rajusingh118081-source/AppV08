using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model.Utility.Helper
{
    public class ControllerName
    {
        public const string EquatedMonthlyInstallment = "Ref_EquatedMonthlyInstallment";
        public const string LoanReferencesInfo = "Ref_LoanReferencesInfo";
    }
    public class Message
    {
        public const string Success = "Success";
        public const string CreateUser = "Register successfully";
        public const string UpdateUser = "Updated successfully";
        public const string DeleteUser = "Deleted successfully";
        public const string ServerError = "Server error";
        public const string DataNotFound = "Data not found";
        public const string EmailExist = "Email address is already exist";
        public const string ProductExist = "Product is already exist";
        public const string SKUExist = "SKU is already exist";
        public const string CategoryExist = "Category and Description is already exist";
        public const string LoginError = "Please enter a valid emailId and password";
        public const string EmailNotFound = "Email address not found";
        public const string Password = "Password";
        public const string Required = "Please check the required details.";
    }
    public class DefaultSetValue
    {
        public const int Zero = 0;
        public const int One = 1;
        public const int Two = 2;
    }
    public enum EntityModel
    {
        Contact, Activity, Document, Project, User, Pricing
    }
    public enum UserType
    {
        Admin, Dispatch, Manufuture
    }
    public static class UniqueNumber
    {
        static private int _InternalCounter = 0;
        public static string UniqueKey()
        {
            var now = DateTime.Now;
            var days = (int)(now - new DateTime(2000, 1, 1)).TotalDays;
            var seconds = (int)(now - DateTime.Today).TotalSeconds;
            var counter = _InternalCounter++ % 100;
            string value = days + seconds.ToString("d4") + counter.ToString("0");
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var numbers = "0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < 2; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            for (int i = 2; i < 6; i++)
            {
                stringChars[i] = numbers[random.Next(numbers.Length)];
            }
            for (int i = 6; i < 8; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            //return finalString;
            return (finalString + value).Substring(0, 10);
        }
    }
    public static class DecimalRounding
    {
        public static decimal ConvertDecimal(decimal val)
        {
            return decimal.Round(val, 2, MidpointRounding.AwayFromZero);
        }
    }
}
