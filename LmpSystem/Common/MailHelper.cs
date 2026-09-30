//using System;
//using System.Reflection;
//using Microsoft.Office.Interop.Outlook;

//namespace Common
//{
//    public class MailHelper
//    {
//        //public  void SendMail(string toEmailAddress, string subject,string content)
//        //{
//        //    var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
//        //    var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
//        //    var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
//        //    var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
//        //    var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();

//        //    bool enabledSsl = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());

//        //    string body = content;
//        //    MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
//        //    message.Subject = subject;
//        //    message.IsBodyHtml = true;
//        //    message.Body = body;

//        //    var client = new SmtpClient();
//        //    client.UseDefaultCredentials = false;//
//        //    client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);

//        //    client.DeliveryMethod = SmtpDeliveryMethod.Network;//

//        //    client.Host = smtpHost;
//        //    client.EnableSsl = enabledSsl;
//        //    client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
//        //    client.Send(message);

//        //    //smtpClient.UseDefaultCredentials = false; 
//        //    //smtpClient.Credentials = new NetworkCredential(senderAddress, senderPassword); 
//        //    //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network; 
//        //    //smtpClient.Host = host; 
//        //    //smtpClient.Port = port; 
//        //    //smtpClient.EnableSsl = useSsl; 
//        //    //return smtpClient;
//        //}


//        //private void ThisAddIn_Startup(object sender, System.EventArgs e)
//        //{
//        //    SendEmailtoContacts();
//        //}

//        private void SendUsingAccountExample()
//        {
//            var application = new Application();

//            MailItem mail = application.CreateItem(OlItemType.olMailItem) as MailItem;
//            mail.Subject = "Our itinerary";
//            mail.Attachments.Add(@"c:\travel\itinerary.doc",
//                OlAttachmentType.olByValue,
//                Type.Missing, Type.Missing);
//            Account account = application.Session.Accounts["Hotmail"];
//            mail.SendUsingAccount = account;
//            mail.Send();
//        }

//        //---------------------------------------------------------------//        
//        //public void SendEmailtoContacts()
//        //{
//        //    var application = new Application();

//        //    string subjectEmail = "Meeting has been rescheduled.";
//        //    string bodyEmail = "Meeting is one hour later.";

//        //    MAPIFolder sentContacts = (MAPIFolder)
//        //                        application.ActiveExplorer().Session.GetDefaultFolder(OlDefaultFolders.olFolderContacts);
//        //    foreach (ContactItem contact in sentContacts.Items)
//        //    {
//        //        if (contact.Email1Address.Contains("gdt.gov.vn"))
//        //        {
//        //            this.CreateEmailItem(subjectEmail, contact
//        //                .Email1Address, bodyEmail);
//        //        }
//        //    }
//        //}

//        //public void CreateEmailItem(string subjectEmail, string toEmail, string bodyEmail)
//        //{
//        //    var application = new Application();

//        //    MailItem eMail = (MailItem)application.CreateItem(OlItemType.olMailItem);

//        //    eMail.Subject = subjectEmail;
//        //    eMail.To = toEmail;
//        //    eMail.Body = bodyEmail;
//        //    eMail.Importance = OlImportance.olImportanceLow;
//        //    ((_MailItem)eMail).Send();
//        //}

//        //--------------------------------------------------------------------------------------------------//

//        //private void SendSalesReport()
//        //{
//        //    var application = new Application();

//        //    MailItem mail = application.CreateItem(OlItemType.olMailItem) as MailItem;
//        //    mail.Subject = "Quarterly Sales Report FY06 Q4";
//        //    AddressEntry currentUser = application.Session.CurrentUser.AddressEntry;

//        //    if (currentUser.Type == "EX")
//        //    {
//        //        Outlook.ExchangeUser manager = currentUser.GetExchangeUser().GetExchangeUserManager();
//        //        // Add recipient using display name, alias, or smtp address
//        //        mail.Recipients.Add(manager.PrimarySmtpAddress);
//        //        mail.Recipients.ResolveAll();
//        //        mail.Attachments.Add(@"c:\sales reports\fy06q4.xlsx",
//        //            OlAttachmentType.olByValue, Type.Missing,
//        //            Type.Missing);
//        //        mail.Send();
//        //    }
//        //}

//        //--------------------------------------------------------------------------------------------------------------------------------//

//        public static void SendEmailFromAccount(string subject, string body, string to, string smtpAddress)//("gui mail test","noi dung gui","your-email@example.com", "your-email@example.com");
//        {
//            Application application = new Application();

//            // Create a new MailItem and set the To, Subject, and Body properties.
//            MailItem newMail = (MailItem)application.CreateItem(OlItemType.olMailItem);
//            newMail.To = to;
//            newMail.Subject = subject;
//            newMail.Body = body;

//            // Retrieve the account that has the specific SMTP address.
//            Account account = GetAccountForEmailAddress(application, smtpAddress);// bị thiếu địa chỉ người gửi// cho này là địa chỉ người gửi
//            // Use this account to send the email.
//            newMail.SendUsingAccount = account;
//            newMail.Send();
//        }

//        public static string Send_Email_Outlook(string _recipient, string _message, string _subject, string _cc, string _bcc, string accountname)
//        {
//            try
//            {
//                Application oApp = new Application();

//                // Get the NameSpace and Logon information.
//                NameSpace oNS = oApp.GetNamespace("mapi");

//                // Log on by using a dialog box to choose the profile.
//                oNS.Logon(Missing.Value, Missing.Value, true, true);

//                // Create a new mail item.
//                MailItem oMsg = (MailItem)oApp.CreateItem(OlItemType.olMailItem);

//                // Set the subject.
//                oMsg.Subject = _subject;

//                // Set HTMLBody.
//                oMsg.HTMLBody = _message;

//                oMsg.To = _recipient;
//                oMsg.CC = _cc;
//                oMsg.BCC = _bcc;

//                #region Send via another account

//                // Retrieve the account that has the specific SMTP address. 
//                Account account = GetAccountForEmailAddress(oApp, "your-email@example.com");
//                // Use this account to send the e-mail. 
//                oMsg.SendUsingAccount = account;

//                // Send.
//                (oMsg as _MailItem).Send();

//                // Log off.
//                oNS.Logoff();

//                // Clean up.
//                //oRecip = null;
//                //oRecips = null;
//                oMsg = null;
//                oNS = null;
//                oApp = null;
//            }
//            // Return Error Message
//            catch (System.Exception e)
//            {
//                return e.Message;
//            }
//            // Default return value.
//            return "";
//        }

//        public static Account GetAccountForEmailAddress(Application application, string smtpAddress)
//        {

//            // Loop over the Accounts collection of the current Outlook session.
//            Accounts accounts = application.Session.Accounts;
//            foreach (Account account in accounts)
//            {
//                // When the email address matches, return the account.
//                if (account.SmtpAddress == smtpAddress)
//                {
//                    return account;
//                }
//            }
//            throw new System.Exception(string.Format("No Account with SmtpAddress: {0} exists!", smtpAddress));
//        }
//    }
//}
//#endregion