using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using TCC.CL.Core.Business;
using TCC.CL.Core.Entities;

namespace TCC.CL.Web.Helpers
{
    public static class Security
    {
        public static Usuario ObterUsuario(this IPrincipal principal)
        {
            return UsuarioBusiness.Obter(principal.Identity.Name);
        }

        public static Autor ObterAutor(this IPrincipal principal)
        {
            int id = principal.ObterUsuario().Id;
            return AutorBusiness.ObterPorUsuario(id) ?? null;
        }

        public static Funcionario ObterFuncionario(this IPrincipal principal) {
            int id = principal.ObterUsuario().Id;
            return FuncionarioBusiness.ObterPorUsuario(id) ?? null;
        }
    }
}