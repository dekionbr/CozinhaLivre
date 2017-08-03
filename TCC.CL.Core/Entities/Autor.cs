using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;

namespace TCC.CL.Core.Entities
{
    [Serializable]
    public class Autor : PessoaFisica
    {
        public Autor()
        {
            this.Usuario = new Usuario();
            this.Telefones = new List<Telefone>();
        }

        public Autor(Usuario usuario)
            : base()
        {
            this.Usuario = usuario;
            this.Empresa = null;
            this.Telefones = new List<Telefone>();
        }

        public Autor(Usuario usuario, PessoaJuridica empresa)
            : base()
        {
            this.Usuario = usuario;
            this.Empresa = empresa;
            this.Telefones = new List<Telefone>();
        }

        public Autor(PessoaJuridica Patrocinador)
        {
            // TODO: Complete member initialization
            this.Usuario = new Usuario();
            this.Empresa = Patrocinador;
            this.Telefones = new List<Telefone>();
        }

        /// <summary>
        /// Dados de Usuário do Sistema
        /// </summary>
        public virtual Usuario Usuario
        {
            get;
            set;
        }


        public virtual PessoaJuridica Empresa
        {
            get;
            set;
        }


        public virtual bool IsPatrocinador
        {
            get
            {
                return Empresa != null;
            }
        }


        public virtual bool isNew
        {
            get
            {
                return DataCadastro.Date > DateTime.Now.AddDays(-5);
            }
        }

        public override string ToString()
        {
            return base.Nome;
        }
    }
}
