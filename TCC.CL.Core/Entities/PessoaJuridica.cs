using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.CL.Core.Entities
{
    [Serializable]
    public class PessoaJuridica : Pessoa
    {
        public PessoaJuridica() {
            DataCadastro = DateTime.Today;
            Autores = new List<Autor>();
        }

        /// <summary>
        /// Razao Social da empresa Patrocinadora
        /// </summary>
        public virtual string RazaoSocial { get; set; }

        /// <summary>
        /// CNPJ da empresa Patrocinadora
        /// </summary>
        public virtual string CNPJ { get; set; }

        /// <summary>
        /// Lista de Autores Autorizados a Incluir Receitas em nome da Empresa
        /// </summary>
        public virtual IList<Autor> Autores { get; set; }

        /// <summary>
        /// Nome Fantasia da Empresa
        /// </summary>
        public virtual string NomeFantasia { get { return Nome; } set { Nome = value; } }
    }
}
