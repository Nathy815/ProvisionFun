using BoletoNetCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wkhtmltopdf.NetCore;
using Application.Services.Interfaces;

namespace Application.Services
{
    public class Boleto : IBoleto
    {
        private readonly IGeneratePdf _generatePdf;

        public Boleto(IGeneratePdf generatePdf)
        {
            _generatePdf = generatePdf;
        }

        public byte[] GeneratePayment(Team team)
        {
            var player = team.Players.Where(p => p.IsPrincipal).Select(t => t.Player).FirstOrDefault();

            var bancoEnum = (Bancos)short.Parse("33"); // Santander

            var contaBancaria = new ContaBancaria
            {
                Agencia = "3099",
                DigitoAgencia = "",
                Conta = "23031",
                DigitoConta = "4",
                CarteiraPadrao = "",
                TipoCarteiraPadrao = TipoCarteira.CarteiraCobrancaSimples,
                TipoFormaCadastramento = TipoFormaCadastramento.SemRegistro,
                TipoImpressaoBoleto = TipoImpressaoBoleto.Banco
            };

            var banco = Banco.Instancia(bancoEnum);

            banco.Beneficiario = new Beneficiario
            {
                CPFCNPJ = "",
                Nome = "Provision Service",
                Endereco = new Endereco
                {
                    LogradouroEndereco = "",
                    LogradouroNumero = "",
                    LogradouroComplemento = "",
                    Bairro = "",
                    Cidade = "",
                    UF = "UF",
                    CEP = "",
                },
                ContaBancaria = contaBancaria
            };

            // Apenas Santander
            banco.Beneficiario.Codigo = "";
            banco.Beneficiario.CodigoDV = "";

            banco.FormataBeneficiario();

            var boleto = new BoletoNetCore.Boleto(banco);

            boleto.Pagador = new Pagador
            {
                CPFCNPJ = player.CPF,
                Nome = player.Name,
                Endereco = new Endereco
                {
                    LogradouroEndereco = team.Condominium.Address,
                    LogradouroNumero = team.Condominium.Number,
                    Bairro = "",
                    Cidade = team.Condominium.City,
                    UF = team.Condominium.State,
                    CEP = team.Condominium.ZipCode.Replace("-", "")
                }
            };

            boleto.NumeroDocumento = "";
            boleto.NossoNumero = "";
            boleto.DataEmissao = DateTime.Today;
            boleto.DataVencimento = DateTime.Today.AddDays(3);

            boleto.ValorTitulo = (decimal)(team.Price); // custo do boleto
            boleto.EspecieDocumento = TipoEspecieDocumento.DM;

            boleto.MensagemInstrucoesCaixa = ""; // instruções

            boleto.ValidarDados();

            var boletoBancario = new BoletoBancario { Boleto = boleto };

            var html = boletoBancario.MontaHtmlEmbedded();
            return _generatePdf.GetPDF(html);
        }

    }
}
