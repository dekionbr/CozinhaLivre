using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Test
{
    [TestClass]
    public class PessoaTestClass
    {
        [TestMethod]
        public void TesteEnderecoMetodo()
        {
            Pessoa Pessoa = new Pessoa();
            Pessoa.Bairro = "Bairro A";
            Pessoa.Cep = "12345678";
            Pessoa.Cidade = "Rio de Janeiro";
            Pessoa.Complemento = "Fundos";
            Pessoa.Numero = 342;
            Pessoa.Logradouro = "Rua Prof. Fulano";

            var mensagem = Pessoa.Endereco();
            Assert.IsNotNull(mensagem, mensagem);


        }
    }
}
