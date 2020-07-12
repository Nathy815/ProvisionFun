using Application.Services.Interfaces;
using Domain.Enums;
using Domain.ViewModels;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class Email : IEmail
    {
        private readonly MySqlContext _sqlContext;

        public Email(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        private string ConfirmationTemplate()
        {
            return null;
        }

        private string ChargeTemplate()
        {
            return null;
        }

        private string FinishTemplate()
        {
            return null;
        }

        private string CancelledTemplate()
        {
            return null;
        }

        public async Task<bool> SendEmail(string email, eStatus status, byte[] attach = null)
        {
            if (status == eStatus.Validation)
                return await Send(new EmailVM(email, "Bem-vindo ao campeonato!", ConfirmationTemplate()));
            else if (status == eStatus.Payment)
                return await Send(new EmailVM(email, "Envio de boleto", ChargeTemplate()), attach);
            else if (status == eStatus.Finished)
                return await Send(new EmailVM(email, "Sua inscrição está completa!", FinishTemplate()));
            else
                return await Send(new EmailVM(email, "Sua inscrição foi cancelada!", CancelledTemplate()));
        }

        private async Task<bool> Send(EmailVM request, byte[] attach = null)
        {
            try
            {
                string host = request.SMTP.Trim();

                string password = null;
                string username = null;
                if (request.User != null && !string.IsNullOrEmpty(request.User.Trim()))
                {
                    password = request.Password.Trim();
                    username = request.User.Trim();
                }

                int port = request.Port;

                foreach (var _email in request.EmailTo)
                {
                    MailMessage email = new MailMessage(request.EmailFrom.Trim(),
                                                        _email,
                                                        request.Subject.Trim(),
                                                        request.Message.Trim());

                    email.IsBodyHtml = request.IsBodyHTML;

                    if (attach != null)
                        email.Attachments.Add(new Attachment(new MemoryStream(attach), "BoletoBancario.pdf"));

                    SmtpClient client = new SmtpClient(host, port);
                    client.EnableSsl = request.UseSSL;

                    if (username != null && password != null)
                    {
                        client.UseDefaultCredentials = false;
                        NetworkCredential credential = new NetworkCredential(username, password);
                        client.Credentials = credential;
                    }
                    else
                        client.UseDefaultCredentials = true;

                    await client.SendMailAsync(email);
                }

                return true;
            }
            catch (Exception ex)
            {
                var _erro = ex.Message;
                return false;
            }
        }
    }
}
