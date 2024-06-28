namespace Proyecto_Ciudad_De_Los_Ninos.Services
{
    public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body);
    }
}
