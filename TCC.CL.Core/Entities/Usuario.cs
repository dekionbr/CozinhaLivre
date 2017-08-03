using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
//using FluentValidation.Attributes;
//using TCC.CL.Core.Validators;
using TCC.CL.Core.Enumeration;

namespace TCC.CL.Core.Entities
{
    [Serializable]
    public class Usuario : BaseDomain<Usuario, int>
    {
        public Usuario() {
            this.IsNovo = true;
        }

        /// <summary>
        /// Login de Entrada no Sistema
        /// </summary>
        public virtual string Login
        {
            get;
            set;
        }

        /// <summary>
        /// Senha de Entrada no Sistema
        /// </summary>
        public virtual string Senha
        {
            get;
            set;
        }

        /// <summary>
        /// Se é novo no Sistema
        /// </summary>
        public virtual bool IsNovo
        {
            get;
            set;
        }

        /// <summary>
        /// Data de Aprovação de Cadastro
        /// </summary>
        public virtual DateTime DataAprovacao
        {
            get;
            set;
        }

        /// <summary>
        /// Foto do Usuário
        /// </summary>
        public virtual byte[] FotoPerfil
        {
            get;
            set;
        }

        public virtual string EMail { get; set; }

        public virtual Grupo Grupo { get; set; }
    }

}
