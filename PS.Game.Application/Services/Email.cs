using Application.Services.Interfaces;
using PS.Game.Domain.Enums;
using Domain.ViewModels;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
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
            return "<p>Bem-vindo</p>";
        }

        private string ChargeTemplate()
        {
            return "<p>Inscrição validada. Segue o boleto</p>";
        }

        private string FinishTemplate()
        {
            return "<p>Pagamento confirmado. Você está no torneio.</p>";
        }

        private string CancelledTemplate()
        {
            return "<p>Sua inscrição foi cancelada.</p>";
        }

        public async Task<bool> SendLog(string title, string message)
        {
            return await Send(new EmailVM("nathalialcoimbra@gmail.com", title, message));
        }

        public async Task<bool> SendEmail(string email, eStatus status, string attach = null)
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

        private async Task<bool> Send(EmailVM request, string attach = null)
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

                    if (!string.IsNullOrEmpty(attach))
                    {
                        MemoryStream ms = new MemoryStream();
                        PdfWriter writer = new PdfWriter(ms);
                        Document document = HtmlConverter.ConvertToDocument(attach, writer);
                        writer.SetCloseStream(false);
                        document.Close();
                        ms.Position = 0;
                        email.Attachments.Add(new Attachment(ms, "Boleto.pdf"));
                    }

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
