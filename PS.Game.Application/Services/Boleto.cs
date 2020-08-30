using BoletoNetCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Services.Interfaces;
using Persistence.Contexts;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;
using PS.Game.Application.Services.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Application.Services
{
    public class Boleto : IBoleto
    {
        private string pathToSave { get; set; }
        private readonly string virtualPath = "http://ec2-54-207-37-149.sa-east-1.compute.amazonaws.com/api/resources/";
        
        private readonly MySqlContext _sqlContext;
        private readonly IEmail _email;
        private readonly IUtil _util;
        private readonly IHostingEnvironment _env;
        private IBanco _banco { get; set; }

        private Beneficiario GerarBeneficiario()
        {
            return new Beneficiario
            {
                CPFCNPJ = "30.850.441/0001-43",
                Nome = "C R L Ferreira e Cia Ltda EPP",
                Codigo = "4592875",
                CodigoTransmissao = "159000004592875",
                Endereco = new Endereco
                {
                    LogradouroEndereco = "Av. Dr. Eneas Pinheiro",
                    LogradouroNumero = "16",
                    LogradouroComplemento = "",
                    Bairro = "Pedreira",
                    Cidade = "Belém",
                    UF = "PA",
                    CEP = "66080290",
                },
                ContaBancaria = new ContaBancaria
                {
                    Agencia = "1590",
                    DigitoAgencia = "0",
                    Conta = "13001027",
                    DigitoConta = "3",
                    CarteiraPadrao = "101",
                    TipoCarteiraPadrao = TipoCarteira.CarteiraCobrancaSimples,
                    TipoFormaCadastramento = TipoFormaCadastramento.ComRegistro,
                    TipoImpressaoBoleto = TipoImpressaoBoleto.Empresa
                }
            };
        }

        public Boleto(MySqlContext sqlContext, IEmail email, IUtil util, IHostingEnvironment env)
        {
            _sqlContext = sqlContext;
            _email = email;
            _util = util;
            _env = env;
            pathToSave = _env.ContentRootPath + "\\Resources\\";
        }

        private string GetDVNossoNumero(string nossoNumero)
        {
            var _char = nossoNumero.ToCharArray();
            var _count = 0;
            int dv = 0;

            for (var i = _char.Length - 1; i >= 0; i--)
            {
                var val = Convert.ToInt32(Char.GetNumericValue(_char[i]));

                if (_count == 0 || _count == 8)
                    dv += val * 2;
                else if (_count == 1 || _count == 9)
                    dv += val * 3;
                else if (_count == 2 || _count == 10)
                    dv += val * 4;
                else if (_count == 3 || _count == 11)
                    dv += val * 5;
                else if (_count == 4)
                    dv += val * 6;
                else if (_count == 5)
                    dv += val * 7;
                else if (_count == 6)
                    dv += val * 8;
                else
                    dv += val * 9;

                _count++;
            }

            var _mod = dv % 11;
            if (_mod == 10)
                return "1";
            else if (_mod == 1 || _mod == 0)
                return "0";
            else
                return (11 - _mod).ToString();
        }

        public async Task<string> GeneratePayment(Team team)
        {
            try
            {
                var player = team.Players.Where(p => p.IsPrincipal).Select(t => t.Player).FirstOrDefault();

                var _parameter = await _sqlContext.Set<Setup>()
                                            .Where(s => s.Key.Equals("NossoNumero"))
                                            .FirstOrDefaultAsync();
                var _novoNossoNumero = Convert.ToInt32(_parameter.Value) + 1;

                _banco = Banco.Instancia(Bancos.Santander);
                _banco.Beneficiario = GerarBeneficiario();
                _banco.FormataBeneficiario();

                var _pagador = new Pagador
                {
                    CPFCNPJ = player.CPF,
                    Nome = player.Name
                };

                if (!string.IsNullOrEmpty(team.Condominium.Address))
                {
                    _pagador.Endereco = new Endereco
                    {
                        LogradouroEndereco = team.Condominium.Address,
                        LogradouroNumero = team.Condominium.Number,
                        Cidade = team.Condominium.City,
                        UF = team.Condominium.State,
                        CEP = team.Condominium.ZipCode.Replace("-", "")
                    };
                }

                var _boleto = new BoletoNetCore.Boleto(_banco)
                {
                    DataVencimento = DateTime.Now.AddDays(10),
                    ValorTitulo = (decimal)team.Price,
                    NossoNumero = _novoNossoNumero.ToString(),
                    NumeroDocumento = "BB" + _novoNossoNumero.ToString("D6") + (char)(65),
                    EspecieDocumento = TipoEspecieDocumento.DM,
                    Pagador = _pagador
                };

                _boleto.ValidarDados();

                var boletoBancario = new BoletoBancario {
                    Boleto = _boleto,
                    OcultarInstrucoes = false,
                    MostrarComprovanteEntrega = false,
                    MostrarEnderecoBeneficiario = true,
                    ExibirDemonstrativo = true
                };

                var html = new StringBuilder();
                html.Append("<div style=\"page-break-after: always;\">");
                html.Append(boletoBancario.MontaHtmlEmbedded());
                html.Append("</div>");

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

                // Atualiza o último Nosso Número utilizado
                _parameter.Value = _novoNossoNumero.ToString();
                _sqlContext.Setups.Update(_parameter);

                await _sqlContext.SaveChangesAsync();

                return html.ToString().Replace("<td class=\"w150 cn8 Al Ab\"></td>", "").Replace("w450 Ar Ab ld", "Ar Ab ld").Replace("w450 Ar ld", "Ar ld").Replace("Pagadorr", "Pagador");
            }
            catch(Exception ex)
            {
                var _setup = _sqlContext.Set<Setup>()
                                        .Where(s => s.Key.Equals("Logo"))
                                        .FirstOrDefault();

                _setup.Value = "Path: " + pathToSave + " | Message: " + ex.Message;
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                    _setup.Value = _setup.Value + " | Trace: " + ex.InnerException.Message;
                _sqlContext.Setups.Update(_setup);

                _sqlContext.SaveChanges();

                return null;
            }
        }

        public async Task<string> GenerateShipping(List<Team> teams)
        {
            try
            {
                var _boletos = new Boletos();
                _banco = Banco.Instancia(Bancos.Santander);
                _banco.Beneficiario = GerarBeneficiario();
                _banco.FormataBeneficiario();
                
                foreach (var _team in teams)
                {
                    var _player = _team.Players.Where(p => p.Active && p.IsPrincipal).FirstOrDefault().Player;
                    var _payment = _team.Payments.OrderByDescending(p => p.DueDate).FirstOrDefault();

                    var _pagador = new Pagador
                    {
                        CPFCNPJ = _player.CPF,
                        Nome = _player.Name
                    };

                    if (!string.IsNullOrEmpty(_team.Condominium.Address))
                    {
                        _pagador.Endereco = new Endereco
                        {
                            LogradouroEndereco = _team.Condominium.Address,
                            LogradouroNumero = _team.Condominium.Number,
                            Cidade = _team.Condominium.City,
                            UF = _team.Condominium.State,
                            CEP = _team.Condominium.ZipCode,
                            Bairro = _team.Condominium.District
                        };
                    }

                    var _boleto = new BoletoNetCore.Boleto(_banco)
                    {
                        DataVencimento = _payment.DueDate,
                        ValorTitulo = (decimal)_payment.Price,
                        NossoNumero = Convert.ToInt32(_payment.Number).ToString(),
                        NossoNumeroDV = _payment.NumberVD,
                        NossoNumeroFormatado = _payment.FormatedNumber,
                        DataEmissao = _payment.IssueDate,
                        NumeroDocumento = _payment.DocumentNumber,
                        EspecieDocumento = TipoEspecieDocumento.DM,
                        Pagador = _pagador
                    };

                    _boleto.ValidarDados();
                    _boletos.Add(_boleto);
                }

                var _parameter = await _sqlContext.Set<Setup>().Where(s => s.Key.Equals("ShippingFile")).FirstOrDefaultAsync();
                var _remessaCount = Convert.ToInt32(_parameter.Value);

                var _shippingFile = new ArquivoRemessa(_banco, TipoArquivo.CNAB240, _remessaCount);
                var _fileName = _util.GetFileName("Remessa");
                
                var fullPath = Path.Combine(pathToSave, _fileName);
                var _base64 = string.Empty;

                // Salva arquivo localmente
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    _shippingFile.GerarArquivoRemessa(_boletos, stream);
                }

                // Converte arquivo para base64
                using (var stream = new FileStream(fullPath, FileMode.Open))
                {
                    var bytes = new Byte[(int)stream.Length];

                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Read(bytes, 0, (int)stream.Length);

                    _base64 = Convert.ToBase64String(bytes);
                }

                _parameter.Value = (_remessaCount + 1).ToString();
                _sqlContext.Setups.Update(_parameter);

                await _sqlContext.SaveChangesAsync();

                return _base64;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<int?> ImportReturn(IFormFile file)
        {
            try
            {
                _banco = Banco.Instancia(Bancos.Santander);
                _banco.Beneficiario = GerarBeneficiario();
                _banco.FormataBeneficiario();

                var arquivoRetorno = new ArquivoRetorno(_banco, TipoArquivo.CNAB240);
                var result = new StringBuilder();
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    arquivoRetorno.LerArquivoRetorno(reader.BaseStream);
                }

                var _total = 0;

                foreach (var _boleto in arquivoRetorno.Boletos)
                {
                    var _team = await _sqlContext.Set<Team>()
                                            .Include(t => t.Payments)
                                            .Include(t => t.Players)
                                                .ThenInclude(p => p.Player)
                                            .Where(t => t.Active &&
                                                        t.Status == PS.Game.Domain.Enums.eStatus.Payment &&
                                                        t.Payments.Any(p => p.DocumentNumber.Equals(_boleto.NumeroDocumento.Trim()) &&
                                                                            p.Number.Equals(_boleto.NossoNumero.Trim())))
                                            .FirstOrDefaultAsync();


                    if (_team != null &&
                        Convert.ToDouble(_boleto.ValorPago) == _team.Price)
                    {
                        _team.Status = PS.Game.Domain.Enums.eStatus.Finished;

                        var _player = _team.Players.Where(p => p.Active && p.IsPrincipal).FirstOrDefault().Player;

                        _team.FinishedSent = await _email.SendEmail(_team, PS.Game.Domain.Enums.eStatus.Finished);

                        var _payment = _team.Payments.Where(p => p.DocumentNumber.Equals(_boleto.NumeroDocumento.Trim()) &&
                                                                 p.Number.Equals(_boleto.NossoNumero)).FirstOrDefault();

                        _payment.Validated = true;
                        _team.PaymentDate = DateTime.Now;

                        _sqlContext.Teams.Update(_team);

                        _total += 1;
                    }
                }

                await _sqlContext.SaveChangesAsync();

                return _total;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
