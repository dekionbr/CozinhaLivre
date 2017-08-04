using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using TCC.CL.Core.Entities;
using NHibernate.Linq;

namespace TCC.CL.Core.Business
{
    public static class UsuarioBusiness
    {
        public static bool LogIn(string login, string senha)
        {
            Usuario usuario = null;

            using (var scope = new TransactionScope())
            {
                usuario = Session.Current.Query<Usuario>()
                                         .Where(x => x.Login.Contains(login) &&
                                                     x.Senha == senha)
                                         .Take(1)
                                         .FirstOrDefault();
            }

            return usuario != null;
        }



        public static Usuario Obter(string username)
        {
            Usuario usuario = null;

            using (var scope = new TransactionScope())
            {
                usuario = Session.Current.Query<Usuario>()
                                         .Where(x => x.Login.Contains(username))
                                         .Take(1)
                                         .ToList()
                                         .FirstOrDefault();
            }

            return usuario;
        }

        public static IList<Usuario> ObterLista(int[] usuarioIds)
        {
            return (from user in Session.Current.Query<Usuario>()
                    select user).ToList();
        }
    }
}
