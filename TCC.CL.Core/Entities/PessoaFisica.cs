using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;

namespace TCC.CL.Core.Entities
{
    [Serializable]
    public class PessoaFisica : Pessoa
    {
        public PessoaFisica()
            : base()
        {

        }

        public virtual DateTime DataNascimento
        {
            get;
            set;
        }
    }
}
