using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.CL.Core.Enumeration;

namespace TCC.CL.Core.Entities
{
    [Serializable]
    public class Funcionario : PessoaFisica
    {
        public Funcionario()
        {
            this.Usuario = new Usuario();
            this.Telefones = new List<Telefone>();
        }

        public virtual TipoFuncionarioEnum TipoFuncionario
        {
            get;
            set;
        }

        /// <summary>
        /// Usuario de Acesso ao Sistema
        /// </summary>
        public virtual Usuario Usuario
        {
            get;
            set;
        }

        public virtual string RG { get; set; }
        public virtual string CPF { get; set; }
    }
}
