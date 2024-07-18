using System.Net;
using System.Net.Mail;
using System.IO;

public class EmailService
{
    private readonly string _smtpServer = "smtp.office365.com";
    private readonly int _port = 587;
    private readonly bool _enableSsl = true;
    private readonly string _username = "proyectocdn2@outlook.com";
    private readonly string _password = "CDN123Proyecto";

    public void SendEmail(string toEmail, string subject, string body)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress("proyectocdn2@outlook.com", "Proyecto Ciudad De Los Niños"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        using (var smtpClient = new SmtpClient(_smtpServer, _port))
        {
            smtpClient.Credentials = new NetworkCredential(_username, _password);
            smtpClient.EnableSsl = _enableSsl;
            smtpClient.Send(mailMessage);
        }
    }

    public void SendEmailWithAttachment(string toEmail, string subject, string body, byte[] attachmentData, string attachmentFileName, string attachmentMimeType)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress("proyectocdn2@outlook.com", "Proyecto Ciudad De Los Niños"),
            Subject = subject,
            IsBodyHtml = true,
            Body = body
        };

        mailMessage.To.Add(toEmail);

        // Adjuntar archivo
        var attachment = new Attachment(new MemoryStream(attachmentData), attachmentFileName, attachmentMimeType);
        mailMessage.Attachments.Add(attachment);

        using (var smtpClient = new SmtpClient(_smtpServer, _port))
        {
            smtpClient.Credentials = new NetworkCredential(_username, _password);
            smtpClient.EnableSsl = _enableSsl;
            smtpClient.Send(mailMessage);
        }
    }
}
