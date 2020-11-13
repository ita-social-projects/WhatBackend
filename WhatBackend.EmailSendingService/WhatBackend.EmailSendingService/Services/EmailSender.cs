using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WhatBackend.EmailSendingService.Services.Interfaces;
using CharlieBackend.Core.Entities;

namespace WhatBackend.EmailSendingService.Services
{
	public class EmailSender : IEmailSender
	{
		private readonly string _email;
		private readonly string _password;

		public EmailSender(string email, string password)
		{
			_email = email;
			_password = password;
		}
		public async Task SendMessageAsync(EmailData data)
		{
			MailMessage message = new MailMessage();

			message.From = new MailAddress(_email);

			message.To.Add(new MailAddress(data.RecipientMail));

			message.Subject = "WHAT Project approve";

			message.IsBodyHtml = true;

			message.Body = data.EmailBody;

			using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
			{
				smtp.UseDefaultCredentials = false;
				smtp.Credentials = new NetworkCredential(_email, _password);
				smtp.EnableSsl = true;
				smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
				await smtp.SendMailAsync(message);
			}
		}
	}
}
