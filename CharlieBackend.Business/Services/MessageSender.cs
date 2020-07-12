using System.Net;
using System.Net.Mail;

namespace CharlieBackend.Business.Services
{
    static class MessageSender
    {
        public static void SendMessage(string _login, string _password)
        {
            string header = "Asp.net core: CharlieBackend";

            // sender - set the address and name displayed in the letter
            MailAddress From = new MailAddress("softserveinc@yandex.ru", "Softserve");

            // to whom we send
            MailAddress To = new MailAddress("dmitriystroy18@gmail.com");

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
                                    "Вітаємо тебе з успішними результатами тестування та інтерв’ю. " +
                                    "Запрошуємо тебе на проходження практики у SoftServe за обраним напрямком. " +
                                    "Перша зустріч з ментором та іншими студентами-практикантами відбудеться у<a style='color: #71bc37; text-decoration: none;' href='#'> Zoom meeting</a> завтра 08.07 о 9:00." +
                                "</p>" +
                               "<p>" +
                                    "<ul>" +
                                        "Твої дані для входу в особистий кабінет:" +
                                        "<li>" +
                                            "Логін: " + _login +
                                        "</li>" +
                                        "<li>" +
                                            "Пароль: " + _password +
                                        "</li>" +
                                    "</ul>" +
                                "</p>" +
                                "<table style='width: 100% !important;border-collapse: collapse;'>" +
                                    "<tr>" +
                                        "<td align='center'>" +
                                            "<p>" +
                                                "<a href='#' style='display: inline-block;" +
                                                "color: white;background: #71bc37;" +
                                                "border: solid #71bc37;" +
                                                "border-width: 10px 20px 8px;" +
                                                "font-weight: bold;" +
                                                "border-radius: 4px;'>Детальніше</a>" +
                                            "</p>" +
                                        "</td>" +
                                    "</tr>" +
                                "</table>" +
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
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            smtp.Credentials = new NetworkCredential("softserveinc@yandex.ru", "ASDasd123");
            smtp.EnableSsl = true;
            smtp.Send(msg);
        }
    }
}
