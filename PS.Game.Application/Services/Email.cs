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
using System.Web;
using System.Threading.Tasks;
using Domain.Entities;
using System.Linq;
using System.Net.Mime;

namespace Application.Services
{
    public class Email : IEmail
    {
        private readonly MySqlContext _sqlContext;
        private string virtualPath = "http://provisionfun.com.br/api/Resources/";

        public Email(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        private string ConfirmationTemplate(string name)
        {
            return string.Format(@"<p>Oi {0}, sua inscrição foi recebida por nossa equipe.</p>
                                   <br>
                                   <p>Estamos analisando seus dados para aprovação e em breve você receberá mais informações nesse e - mail.</p>
                                   <br>
                                   <p>Aguarde!</p>
                                   <p>Equipe Provision Fun</p>",
                                   name);
        }

        private string ChargeTemplate(string name, double valor)
        {
            return string.Format(@"<p>Oi {0}, parabéns!</p>
                                   <br>
                                   <p>Analisamos seus dados e sua inscrição foi aprovada com sucesso.</p>
                                   <p>Seu boleto de {1} já está disponível para pagamento, faça o download dele abaixo e pague através de qualquer agência bancária, casa lotérica ou através do seu internet banking.</p>
                                   <p>Lembre-se: sua inscrição só será confirmada mediante o pagamento desse boleto.</p>
                                   <br>
                                   <p>Te esperamos no pódio!</p>
                                   <p>Equipe Provision Fun</p>",
                                   name,
                                   Math.Round(valor, 2).ToString().Replace('.', ','));
        }

        private string FinishTemplate(string name)
        {
            return string.Format(@"<p>Olá {0}, sua inscrição foi confirmada!!!</p>
                                   <br>
                                   <p>A partir de agora, você está concorrendo a R$ 30.000,00 em prêmios no maior Campeonato de E-sports do norte do Brasil.</p>
                                   <br>
                                   <p>Continue treinando e se prepare que nós queremos te ver no pódio.</p>
                                   <p>Equipe Provision Fun</p>",
                                   name);
        }

        private string CancelledTemplate(string name, string motivo)
        {
            return string.Format(@"<p>Olá {0}, sua inscrição não pode ser confirmada pelo(s) seguinte(s) motivo(s) abaixo:</p>
                                   <br>
                                   <p><i>{1}</i></p>
                                   <br>
                                   <p>Um abraço!</p>
                                   <p>Equipe Provision Fun</p>",
                                   name,
                                   motivo);
        }

        private string EliminatedTemplate(string name)
        {
            return string.Format(@"<p>Olá {0}. Sentimos muito pela sua eliminação.</p>
                                   <br>
                                   <p>Fique ligado no nosso site para novas competições e boa sorte na próxima!</p>
                                   <br>
                                   <p>Até a próxima!</p>
                                   <p>Equipe Provision Fun</p>",
                                   name);
        }

        private string MatchInform(string name, Guid id, Match match, bool alter)
        {
            return string.Format(@"<p>Olá {0}. {1}</p>
                                   <br>
                                   <p><b>Data:</b> {2}</p>
                                   <p><b>Oponente:</b> {3}</p>
                                   <p><b>Auditor:</b> {4}</p>
                                   <br>
                                   <p>Deixe bem anotado e até lá!</p>
                                   <p>Equipe Provision Fun</p>",
                                   alter ? "Uma de suas partidas foi atualizada.</p><p>Confira:" : "Uma nova partida foi agendada:",
                                   name,
                                   string.Format("{0}/{1}/{2} {3}:{4}", match.Date.Value.Day, match.Date.Value.Month, match.Date.Value.Year, match.Date.Value.Hour, match.Date.Value.Minute),
                                   match.Player1ID == id ? match.Player2.Name : match.Player1.Name,
                                   match.Auditor.Name);
        }

        public async Task<bool> SendLog(string title, string message)
        {
            return await Send(new EmailVM("nathalialcoimbra@gmail.com", title, message));
        }

        public async Task<bool> SendEmail(Team team, eStatus status, string attach = null, Match match = null, bool? alter = null)
        {
            var _player = team.Players.Where(p => p.IsPrincipal).FirstOrDefault().Player;

            if (status == eStatus.Validation)
                return await Send(new EmailVM(_player.Email, "Provision Fun - Inscrição Recebida", ConfirmationTemplate(_player.Name)), "C:/inetpub/wwwroot/provisionfun_api/Resources/Heads-Emails1.png", true);
            else if (status == eStatus.Payment)
                return await Send(new EmailVM(_player.Email, "Provision Fun - Inscrição Aprovada", ChargeTemplate(_player.Name, team.Price)), attach);
            else if (status == eStatus.Finished)
                return await Send(new EmailVM(_player.Email, "Provision Fun - Inscrição Confirmada", FinishTemplate(_player.Name)));
            else if (status == eStatus.Eliminated)
                return await Send(new EmailVM(_player.Email, "Provision Fun - Eliminação", EliminatedTemplate(_player.Name)));
            else if (status == eStatus.Cancelled)
                return await Send(new EmailVM(_player.Email, "Provision Fun - Inscrição Cancelada", CancelledTemplate(_player.Name, team.CancellationComments)));
            else
                return await Send(new EmailVM(_player.Email, "Provision Fun - Informações de Partida", MatchInform(_player.Name, team.Id, match, alter.Value)));
        }

        private async Task<bool> Send(EmailVM request, string attach = null, bool inline = false)
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
                    MailMessage email = new MailMessage();
                    email.From = new MailAddress(request.EmailFrom.Trim());
                    email.To.Add(_email);
                    email.Subject = request.Subject.Trim();

                    email.IsBodyHtml = request.IsBodyHTML;

                    if (!string.IsNullOrEmpty(attach))
                    {
                        email.Body = request.Message.Trim();

                        if (!inline)
                        {
                            email.Body = request.Message.Trim();
                            MemoryStream ms = new MemoryStream();
                            PdfWriter writer = new PdfWriter(ms);
                            Document document = HtmlConverter.ConvertToDocument(attach, writer);
                            writer.SetCloseStream(false);
                            document.Close();
                            ms.Position = 0;
                            email.Attachments.Add(new Attachment(ms, "Boleto.pdf"));
                        }
                        /*else
                        {
                            LinkedResource res = new LinkedResource(attach);
                            res.ContentId = Guid.NewGuid().ToString();
                            var body = "<img src='cid:" + res.ContentId + "'/><br><br>" + request.Message.Trim();
                            AlternateView alt = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                            alt.LinkedResources.Add(res);
                            email.AlternateViews.Add(alt);
                        }*/
                    }
                    else
                        email.Body = request.Message.Trim();

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
                var _setup = _sqlContext.Set<Setup>()
                                    .Where(s => s.Key.Equals("Logo"))
                                    .FirstOrDefault();

                if (string.IsNullOrEmpty(ex.InnerException.Message))
                    _setup.Value = ex.Message;
                else
                    _setup.Value = ex.InnerException.Message;

                _sqlContext.Setups.Update(_setup);

                await _sqlContext.SaveChangesAsync();

                return false;
            }
        }
    }
}
