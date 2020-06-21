using System;
using System.Net.Mail;

namespace RM_Messenger.Helpers
{
  public static class SendEmail
  {
    public static bool SendEmailExecute(string email, string emailSubject, string emailContent)
    {
      if (email == null)
      {
        return false;
      }
      try
      {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        mail.From = new MailAddress("RM.Messenger.App@gmail.com");
        mail.To.Add(email);
        mail.Subject = emailSubject;
        mail.Body = emailContent;
        SmtpServer.Port = 587;
        SmtpServer.Credentials = new System.Net.NetworkCredential("RM.Messenger.App@gmail.com", "RM_Messenger_App2@gmail.com");
        SmtpServer.EnableSsl = true;
        SmtpServer.Send(mail);
        return true;
      }
      catch (Exception e)
      {
        return false;
      }
    }
  }
}
