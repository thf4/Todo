using System.Net;
using System.Net.Mail;
using Todo.Shared;

namespace Todo.Services
{
  public class EmailService
  {
    public bool Send(
      string toName,
      string toEmail,
      string subject,
      string body
    )
    {
      var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);

      var fromEmail = Environment.GetEnvironmentVariable("CompanyEmail");
      var fromName = "Bem Vindo ao Todo";

      smtpClient.Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password);
      smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
      smtpClient.EnableSsl = true;
      var mail = new MailMessage();

      mail.From = new MailAddress(fromEmail, fromName);
      mail.To.Add(new MailAddress(toEmail, toName));
      mail.Subject = subject;
      mail.Body = body;
      mail.IsBodyHtml = true;

      try
      {
        smtpClient.Send(mail);
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }
  }
}