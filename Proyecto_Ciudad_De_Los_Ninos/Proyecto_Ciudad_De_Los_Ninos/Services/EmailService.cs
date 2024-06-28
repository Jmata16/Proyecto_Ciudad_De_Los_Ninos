using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _smtpServer = "smtp.office365.com";
    private readonly int _port = 587;
    private readonly bool _enableSsl = true;
    private readonly string _username = "proyectocdn1@outlook.com";
    private readonly string _password = "CDN123Proyecto";

    public void SendEmail(string toEmail, string subject, string body)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress("proyectocdn1@outlook.com", "Proyecto Ciudad De Los Niños"),
            Subject = subject,
            Body = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    width: 100%;
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    border-radius: 5px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    text-align: center;
                    padding: 3px 0;
                }}
                .header img {{
                    width: 300px;
                    height: 150px;
                }}
                .header h1 {{
                    margin-top: -20px;
                }}
                .content {{
                    padding: 3px;
                }}
                .footer {{
                    text-align: center;
                    padding: 10px 0;
                    font-size: 12px;
                    color: #888888;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <img src='https://www.ciudaddelosninoscr.org/pics/school-logo.png' alt='Logo' />
                    <h1>¡Te damos la bienvenida a Ciudad De Los Niños!</h1>
                </div>
                <div class='content'>
                    {body}
                </div>
                <div class='footer'>
                    <p>© 2024 Proyecto Ciudad De Los Niños. Todos los derechos reservados.</p>
                </div>
            </div>
        </body>
        </html>",
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        using (var smtpClient = new SmtpClient("smtp.office365.com", 587))
        {
            smtpClient.Credentials = new NetworkCredential("proyectocdn1@outlook.com", "CDN123Proyecto");
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }
    }
}
