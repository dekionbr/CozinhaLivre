using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using TCC.CL.Core.Enumeration;
using TCC.CL.Core.Helper;

namespace TCC.CL.Core.Entities
{
    [Serializable]
    public class Pessoa : BaseDomain<Pessoa, int>
    {

        public Pessoa()
        {
            this.DataCadastro = DateTime.Now;
            this.NumSuspensoes = 0;
            this.Telefones = new List<Telefone>();

        }

        /// <summary>
        /// Nome da Pessoa
        /// </summary>
        public virtual string Nome
        {
            get;
            set;
        }

        
        public virtual DateTime DataCadastro
        {
            get;
            set;
        }

        /// <summary>
        /// Determina se Pessoa se encontra ativo ou inativo
        /// </summary>
        public virtual bool Ativo
        {
            get;
            set;
        }

        /// <summary>
        /// Determina o numero que suspensões que a pessoa já levou
        /// </summary>
        public virtual int NumSuspensoes
        {
            get;
            set;
        }

        public virtual string Bairro
        {
            get;
            set;
        }

        public virtual string Cidade
        {
            get;
            set;
        }

        public virtual UF UF
        {
            get;
            set;
        }

        public virtual string Logradouro { get; set; }

        public virtual int Numero
        {
            get;
            set;
        }

        public virtual string Complemento
        {
            get;
            set;
        }

        public virtual string Cep
        {
            get;
            set;
        }

        public virtual IList<Telefone> Telefones
        {
            get;
            set;
        }

        public virtual string Endereco()
        {

            return string.Join(" - ", new string[] {
                    string.Format("{0}, {1}", Logradouro, Numero),
                    Complemento,
                    Bairro,
                    Cidade,
                    UF.GetDescription(),
                    string.Format("CEP {0:00000-000}", Cep)
                    }.Where(s => !string.IsNullOrEmpty(s)).ToArray());
        }
    }
}
