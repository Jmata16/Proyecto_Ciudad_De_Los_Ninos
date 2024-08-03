using System;
using System.Net;
using System.Net.Mail;
using System.IO;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly bool _enableSsl;
    private readonly string _username;
    private readonly string _password;

    public EmailService(IConfiguration configuration)
    {
        _smtpServer = configuration["EmailSettings:SmtpServer"];
        _port = int.Parse(configuration["EmailSettings:Port"]);
        _enableSsl = bool.Parse(configuration["EmailSettings:EnableSsl"]);
        _username = configuration["EmailSettings:Username"];
        _password = configuration["EmailSettings:Password"];
    }

    public void SendEmail(string toEmail, string subject, string body)
    {
        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_username, "Proyecto Ciudad De Los Niños"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            using (var smtpClient = new SmtpClient(_smtpServer, _port))
            {
                smtpClient.Credentials = new NetworkCredential(_username, _password);
                smtpClient.UseDefaultCredentials = false; // Establece esta propiedad
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network; // Establece esta propiedad
                smtpClient.EnableSsl = _enableSsl;
                smtpClient.Send(mailMessage);
            }
        }
        catch (Exception ex)
        {
            // Registra el error en un archivo o en un sistema de logs
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }

    public void SendEmailWithAttachment(string toEmail, string subject, string body, byte[] attachmentData, string attachmentFileName, string attachmentMimeType)
    {
        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_username, "Proyecto Ciudad De Los Niños"),
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
                smtpClient.UseDefaultCredentials = false; // Establece esta propiedad
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network; // Establece esta propiedad
                smtpClient.EnableSsl = _enableSsl;
                smtpClient.Send(mailMessage);
            }
        }
        catch (Exception ex)
        {
            // Registra el error en un archivo o en un sistema de logs
            Console.WriteLine($"Error sending email with attachment: {ex.Message}");
        }
    }
}
