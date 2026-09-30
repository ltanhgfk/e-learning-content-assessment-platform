using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
//using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
//using Microsoft.Office.Interop.Outlook;

namespace LmpSystem.Common
{
    public static class Function
    {
        public static string GetTeaserFromContent(string htmlString, int characterCount)
        {
            string htmlTagPattern = "<.*?>";
            var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            htmlString = regexCss.Replace(htmlString, string.Empty);
            htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
            htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            htmlString = htmlString.Replace("&nbsp;", string.Empty);
            if (htmlString.Length <= characterCount)
            {
                return htmlString;
            }
            return htmlString.Substring(0, characterCount);
        }
        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

        public static string GetEnumDisplayName(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DisplayAttribute[] attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Name;
            else
                return value.ToString();
        }

        //short value = (short)BookingStatus.Active;
        //var Status = Extensions.GetEnumDisplayName((BookingStatus)value);

        //var value = (short)BookingStatus.Active;
        //var description = Extensions.GetDescription((BookingStatus)value);       


        public static string GetEnumDescription(Enum value)
        {
            var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var descriptionAttribute =
                enumMember == null
                    ? default
                    : enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            return
                descriptionAttribute == null
                    ? value.ToString()
                    : descriptionAttribute.Description;
        }

        //public static string SendMail()
        //{
        //    MailMessage mail = new MailMessage();

        //    mail.To.Add("your-email@example.com");
        //    mail.From = new MailAddress("your-email@example.com");
        //    mail.Subject = "gui test choi!";
        //    mail.Body = "bạn dã nhạn được 1 công việc, hãy cập nhật trạng thái công việc!";


        //    SmtpClient smtp = new SmtpClient();
        //    smtp.Host = "smtp-mail.outlook.com";
        //    smtp.Port = 587;
        //    smtp.UseDefaultCredentials = false;
        //    smtp.Credentials = new System.Net.NetworkCredential("username", "password");
        //    smtp.EnableSsl = true;

        //    return "send ok!";
        //}

        //public static void SendMail(MailContent model)//async
        //{
        //    var body = "<p>Email From: {0} ({1})</p><p>{2}</p>";
        //    var message = new MailMessage();
        //    message.To.Add(new MailAddress(model.To));  // replace with valid value 
        //    message.From = new MailAddress(model.From);  // replace with valid value
        //    message.Subject = model.Subject;
        //    message.Body = string.Format(body, "Admin", model.From, model.Body);
        //    message.IsBodyHtml = true;

        //    using (var smtp = new SmtpClient())
        //    {
        //        smtp.Send(message);       //await           
        //    }
        //}

        ///****************backup code from Mailhelper*****************/
        //public static void SendEmailFromAccount(string subject, string body, string to, string smtpAddress)//("gui mail test","noi dung gui","your-email@example.com", "your-email@example.com");
        //{
        //    Application application = new Application();

        //    // Create a new MailItem and set the To, Subject, and Body properties.
        //    MailItem newMail = (MailItem)application.CreateItem(OlItemType.olMailItem);
        //    newMail.To = to;
        //    newMail.Subject = subject;
        //    newMail.Body = body;

        //    // Retrieve the account that has the specific SMTP address.
        //    Account account = GetAccountForEmailAddress(application, smtpAddress);// bị thiếu địa chỉ người gửi// cho này là địa chỉ người gửi
        //    // Use this account to send the email.
        //    newMail.SendUsingAccount = account;
        //    newMail.Send();
        //}

        //public static string Send_Email_Outlook(string _recipient, string _message, string _subject, string _cc, string _bcc, string accountname)
        //{
        //    try
        //    {
        //        Application oApp = new Application();

        //        // Get the NameSpace and Logon information.
        //        NameSpace oNS = oApp.GetNamespace("mapi");

        //        // Log on by using a dialog box to choose the profile.
        //        oNS.Logon(Missing.Value, Missing.Value, true, true);

        //        // Create a new mail item.
        //        MailItem oMsg = (MailItem)oApp.CreateItem(OlItemType.olMailItem);

        //        // Set the subject.
        //        oMsg.Subject = _subject;

        //        // Set HTMLBody.
        //        oMsg.HTMLBody = _message;

        //        oMsg.To = _recipient;
        //        oMsg.CC = _cc;
        //        oMsg.BCC = _bcc;

        //        #region Send via another account

        //        // Retrieve the account that has the specific SMTP address. 
        //        Account account = GetAccountForEmailAddress(oApp, "your-email@example.com");
        //        // Use this account to send the e-mail. 
        //        oMsg.SendUsingAccount = account;

        //        // Send.
        //        (oMsg as _MailItem).Send();

        //        // Log off.
        //        oNS.Logoff();

        //        // Clean up.
        //        //oRecip = null;
        //        //oRecips = null;
        //        oMsg = null;
        //        oNS = null;
        //        oApp = null;
        //    }
        //    // Return Error Message
        //    catch (System.Exception e)
        //    {
        //        return e.Message;
        //    }
        //    // Default return value.
        //    return "";
        //}

        //public static Account GetAccountForEmailAddress(Application application, string smtpAddress)
        //{

        //    // Loop over the Accounts collection of the current Outlook session.
        //    Accounts accounts = application.Session.Accounts;
        //    foreach (Account account in accounts)
        //    {
        //        // When the email address matches, return the account.
        //        if (account.SmtpAddress == smtpAddress)
        //        {
        //            return account;
        //        }
        //    }
        //    throw new System.Exception(string.Format("No Account with SmtpAddress: {0} exists!", smtpAddress));
        //}

        /****************backup code from Mailhelper*****************/
    }
}
//#endregion