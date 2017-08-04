using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCC.CL.Core.Entities;
using TCC.CL.Core.Interfaces;
using NHibernate.Linq;
using System.Transactions;

namespace TCC.CL.Core.Business
{
    public static class AutorBusiness
    {
        public static List<Autor> ObterLista()
        {
            return (from r in Session.Current.Query<Autor>()
                    select r).ToList();
        }

        public static Paginacao<Autor> ObterLista(int pagina)
        {
            return (from r in Session.Current.Query<Autor>()
                    select r).ToListPaginado(pagina);
        }

        public static Autor Obter(int Key)
        {
            return Session.Current.Query<Autor>().Where(x => x.Id == Key).Take(1).ToList().Single();
        }

        public static bool Add(Autor autor)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    //autor = Session.Current.Merge<Autor>(autor);
                    Session.Current.SaveOrUpdate(autor);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool Update(Autor autor)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    autor = Session.Current.Merge<Autor>(autor);
                    Session.Current.SaveOrUpdate(autor);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static Autor ObterPorUsuario(int idUsuario)
        {
            return Session.Current.Query<Autor>().Where(x => x.Usuario.Id == idUsuario).Take(1).ToList().SingleOrDefault();
        }

        public static IList<Autor> ObterLista(int[] Autores)
        {
            if (Autores != null)
                return ObterLista().Where(x => Autores.Contains(x.Id)).ToList();
            else
                return ObterLista();
        }
    }
}
