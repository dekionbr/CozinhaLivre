using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using TCC.CL.Core.Entities;
using System.Transactions;

namespace TCC.CL.Core.Business
{
    public static class FuncionarioBusiness
    {
        public static List<Funcionario> ObterLista()
        {
            return (from r in Session.Current.Query<Funcionario>()
                    select r).ToList();
        }

        public static Paginacao<Funcionario> ObterLista(int pagina)
        {
            return (from r in Session.Current.Query<Funcionario>()
                    select r).ToListPaginado(pagina);
        }

        public static Funcionario Obter(int Key)
        {
            return Session.Current.Query<Funcionario>()
                          .Where(x => x.Id == Key).Take(1).Single();
        }

        public static bool Add(Funcionario Funcionario)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    //autor = Session.Current.Merge<Autor>(autor);
                    Session.Current.SaveOrUpdate(Funcionario);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static Funcionario ObterPorUsuario(int id)
        {
            return Session.Current.Query<Funcionario>()
                .Where(x => x.Usuario.Id == id)
                .FirstOrDefault();
        }
    }
}
