using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebUygulamaProje1.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // e-mail gönderme işlemleri vs burada yapılabilir (opsiyonel)
            return Task.CompletedTask;
        }
    }
}
