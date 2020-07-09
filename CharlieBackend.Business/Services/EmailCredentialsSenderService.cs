using CharlieBackend.Business.Services.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class EmailCredentialsSenderService : ICredentialsSenderService
    {
        private readonly string _googleEmail;
        private readonly string _googlePassword;
        public EmailCredentialsSenderService(string googleEmail, string googlePassword)
        {
            _googleEmail = googleEmail;
            _googlePassword = googlePassword;
        }
        public Task SendCredentialsAsync(string email, string password)
        {
            string header = "CharlieBackend credentials";

            // sender - set the address and name displayed in the letter
            MailAddress From = new MailAddress(_googleEmail, "SoftServe");

            // to whom we send
            MailAddress To = new MailAddress(email);

            // create a message object
            MailMessage msg = new MailMessage(From, To);

            // letter subject           
            msg.Subject = header;

            // text of the letter
            string maintext =
                "<table style='width: 100% !important;border-collapse: collapse; width: 100% !important;height: 100%;background: #efefef;-webkit-font-smoothing: antialiased;-webkit-text-size-adjust: none;'>" +
                "<tr>" +
                    "<td style='display: block !important;clear: both !important;margin: 0 auto !important;max-width: 580px !important;'>" +
                    "<table style='width: 100% !important;border-collapse: collapse;'>" +
                        "<tr>" +
                            "<td style='padding: 45px 0; position:relative; background-repeat: no-repeat; background-size: cover;background-image: url(https://cdnssinc-dev.azureedge.net/img/home/gradient-map-1.jpg);' align='center'>" +
                                  "<h1 style='font-size: 32px; margin-bottom: 20px;line-height: 1.25;'>" + header + "</h1>" +
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td style='display: block !important;clear: both !important;margin: 0 auto !important;max-width: 580px !important;'>" +
                                "<p>" +
                                    "Добро пожаловать." +
                                "</p>" +
                               "<p>" +
                                    "<ul>" +
                                        "Ваши данные для входа:" +
                                        "<li>" +
                                            "Email: " + email +
                                        "</li>" +
                                        "<li>" +
                                            "Пароль: " + password +
                                        "</li>" +
                                    "</ul>" +
                                "</p>" +
                            "</td>" +
                        "</tr>" +
                    "</table>" +
                "</td>" +
            "</tr>" +
            "<tr>" +
                "<td class='container'>" +
                    "<table style='width: 100% !important;border-collapse: collapse;'>" +
                        "<tr>" +
                            "<td class='content footer' align='center'>" +
                                "<p>© Copyright 2020 SoftServe</p>" +
                            "</td>" +
                        "</tr>" +
                    "</table>" +
                "</td>" +
            "</tr>" +
        "</table>";

            msg.Body = maintext;
            msg.IsBodyHtml = true;

            // SMTP server address and port from which we will send the letter

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_googleEmail, _googlePassword);
            smtp.EnableSsl = true;

            return smtp.SendMailAsync(msg);
        }
    }
}
