using BoletoNetCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Wkhtmltopdf.NetCore;
using Application.Services.Interfaces;
using Persistence.Contexts;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;
using PS.Game.Application.Services.Interfaces;

namespace Application.Services
{
    public class Boleto : IBoleto
    {
        private static string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Resources");

        //private readonly IGeneratePdf _generatePdf;
        private readonly MySqlContext _sqlContext;
        private readonly IEmail _email;
        private readonly IUtil _util;
        private IBanco _banco { get; set; }
        private BoletoNetCore.Boleto _boleto { get; set; }

        public Boleto(/*IGeneratePdf generatePdf, */MySqlContext sqlContext, IEmail email, IUtil util)
        {
            //_generatePdf = generatePdf;
            _sqlContext = sqlContext;
            _email = email;
            _util = util;

            var bancoEnum = (Bancos)short.Parse("33"); // Santander

            var contaBancaria = new ContaBancaria
            {
                Agencia = "1590",
                DigitoAgencia = "",
                Conta = "13001027",
                DigitoConta = "3",
                CarteiraPadrao = "101",
                TipoCarteiraPadrao = TipoCarteira.CarteiraCobrancaSimples,
                TipoFormaCadastramento = TipoFormaCadastramento.SemRegistro,
                TipoImpressaoBoleto = TipoImpressaoBoleto.Banco
            };

            _banco = Banco.Instancia(bancoEnum);

            _banco.Beneficiario = new Beneficiario
            {
                CPFCNPJ = "30.850.441/0001-43",
                Nome = "C. R. L Ferreira e Cia Ltda",
                Endereco = new Endereco
                {
                    LogradouroEndereco = "Av. Dr. Eneas Pinheiro",
                    LogradouroNumero = "16",
                    LogradouroComplemento = "",
                    Bairro = "Pedreira",
                    Cidade = "Belém",
                    UF = "PA",
                    CEP = "66080-290",
                },
                ContaBancaria = contaBancaria
            };

            _banco.Beneficiario.Codigo = "";
            _banco.Beneficiario.CodigoDV = "";

            _banco.FormataBeneficiario();

            _boleto = new BoletoNetCore.Boleto(_banco);
        }

        public async Task<byte[]> GeneratePayment(Team team)
        {
            try
            {
                var player = team.Players.Where(p => p.IsPrincipal).Select(t => t.Player).FirstOrDefault();
                var nossoNumero = await _sqlContext.Set<Setup>().Where(s => s.Key.Equals("NossoNumero")).FirstOrDefaultAsync();

                _boleto.Pagador = new Pagador
                {
                    CPFCNPJ = player.CPF,
                    Nome = player.Name,
                    Endereco = new Endereco
                    {
                        LogradouroEndereco = team.Condominium.Address,
                        LogradouroNumero = team.Condominium.Number,
                        Cidade = team.Condominium.City,
                        UF = team.Condominium.State,
                        CEP = team.Condominium.ZipCode.Replace("-", "")
                    }
                };

                _boleto.NumeroDocumento = "";
                _boleto.NossoNumero = (Convert.ToInt32(nossoNumero) + 1).ToString();
                _boleto.NossoNumeroDV = "";
                _boleto.NossoNumeroFormatado = "";
                _boleto.DataEmissao = DateTime.Today;
                _boleto.DataVencimento = DateTime.Today.AddDays(3);

                _boleto.ValorTitulo = (decimal)(team.Price); // custo do boleto
                _boleto.EspecieDocumento = TipoEspecieDocumento.DM;

                _boleto.MensagemInstrucoesCaixa = ""; // instruções

                _boleto.ValidarDados();

                var boletoBancario = new BoletoBancario { Boleto = _boleto };

                var html = boletoBancario.MontaHtmlEmbedded();
                //var _pdf = _generatePdf.GetPDF(html);

                var _payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    DocumentNumber = _boleto.NumeroDocumento,
                    DueDate = _boleto.DataVencimento,
                    IssueDate = _boleto.DataEmissao,
                    Number = _boleto.NossoNumero,
                    NumberVD = _boleto.NossoNumeroDV,
                    FormatedNumber = _boleto.NossoNumeroFormatado,
                    Price = (double)_boleto.ValorTitulo,
                    Validated = false,
                    TeamID = team.Id
                };

                await _sqlContext.Payments.AddAsync(_payment);

                nossoNumero.Value = _boleto.NossoNumero;

                _sqlContext.Setups.Update(nossoNumero);

                await _sqlContext.SaveChangesAsync();

                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GenerateShipping(List<Team> teams, string virtualPath)
        {
            try
            {
                var _boletos = new Boletos();
                _boletos.Banco = _banco;

                foreach (var _team in teams)
                {
                    var _player = _team.Players.Where(p => p.Active && p.IsPrincipal).FirstOrDefault().Player;
                    var _payment = _team.Payments.OrderByDescending(p => p.DueDate).FirstOrDefault();

                    var boleto = new BoletoNetCore.Boleto(_banco);
                    boleto.Pagador = new Pagador
                    {
                        CPFCNPJ = _player.CPF,
                        Nome = _player.Name,
                        Endereco = new Endereco
                        {
                            LogradouroEndereco = _team.Condominium.Address,
                            LogradouroNumero = _team.Condominium.Number,
                            Cidade = _team.Condominium.City,
                            UF = _team.Condominium.State,
                            CEP = _team.Condominium.ZipCode.Replace("-", "")
                        }
                    };
                    boleto.NumeroDocumento = _payment.DocumentNumber;
                    boleto.NossoNumero = _payment.Number;
                    boleto.NossoNumeroDV = _payment.NumberVD;
                    boleto.NossoNumeroFormatado = _payment.FormatedNumber;
                    boleto.DataEmissao = _payment.IssueDate;
                    boleto.DataVencimento = _payment.DueDate;
                    boleto.ValorTitulo = (decimal)(_payment.Price);
                    boleto.EspecieDocumento = TipoEspecieDocumento.DM;
                    boleto.MensagemInstrucoesCaixa = ""; // instruções
                    boleto.ValidarDados();
                    _boletos.Add(boleto);
                }

                var _parameter = await _sqlContext.Set<Setup>().Where(s => s.Key.Equals("ShippingFile")).FirstOrDefaultAsync();

                var _shippingFile = new ArquivoRemessa(_banco, TipoArquivo.CNAB240, Convert.ToInt32(_parameter.Value));
                var _fileName = _util.GetFileName("Remessa");
                
                var fullPath = Path.Combine(pathToSave, _fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    _shippingFile.GerarArquivoRemessa(_boletos, stream);
                }

                _parameter.Value = (Convert.ToInt32(_parameter.Value) + 1).ToString();

                _sqlContext.Setups.Update(_parameter);

                return Path.Combine(virtualPath, _fileName);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> ImportReturn(IFormFile file, string virtualPath)
        {
            try
            {
                var _fileName = _util.GetFileName("Retorno");
                var _file = _util.UploadFile(file, _fileName, virtualPath);

                Boletos boletos = new Boletos();
                var arquivoRetorno = new ArquivoRetorno(_banco, TipoArquivo.CNAB240);
                using (var fileStream = new FileStream(_fileName, FileMode.Open))
                {
                    boletos = arquivoRetorno.LerArquivoRetorno(fileStream);
                }

                foreach (var _boleto in boletos)
                {
                    var _team = await _sqlContext.Set<Team>()
                                            .Include(t => t.Payments)
                                            .Include(t => t.Players)
                                                .ThenInclude(p => p.Player)
                                            .Where(t => t.Active &&
                                                        t.Status == Domain.Enums.eStatus.Payment &&
                                                        t.Payments.Any(p => p.DocumentNumber.Equals(_boleto.NumeroDocumento) &&
                                                                            p.Number.Equals(_boleto.NossoNumero)))
                                            .FirstOrDefaultAsync();

                    if (_team != null)
                    {
                        _team.Status = Domain.Enums.eStatus.Finished;

                        var _player = _team.Players.Where(p => p.Active && p.IsPrincipal).FirstOrDefault().Player;

                        _team.FinishedSent = await _email.SendEmail(_player.Email, Domain.Enums.eStatus.Finished);

                        var _payment = _team.Payments.Where(p => p.DocumentNumber.Equals(_boleto.NumeroDocumento) &&
                                                                 p.Number.Equals(_boleto.NossoNumero)).FirstOrDefault();

                        _payment.Validated = true;

                        _sqlContext.Teams.Update(_team);
                    }
                }

                await _sqlContext.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
