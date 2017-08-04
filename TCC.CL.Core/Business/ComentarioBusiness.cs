using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.CL.Core.Entities;
using NHibernate.Linq;
using System.Transactions;

namespace TCC.CL.Core.Business
{
    public static class ComentarioBusiness
    {
        public static Paginacao<Comentario> ObterTodos(int pagAtual)
        {
            return (from r in Session.Current.Query<Comentario>()
                    where r.Parente == null
                    select r).ToListPaginado(pagAtual);
        }

        public static bool Add(Comentario comentario)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    //autor = Session.Current.Merge<Autor>(autor);
                    Session.Current.SaveOrUpdate(comentario);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }


        public static bool Update(Comentario comentario)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    Session.Current.Update(comentario);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static void Ativar(int id)
        {
            var comentario = Session.Current.Get<Comentario>(id);

            comentario.Ativo = true;

            Update(comentario);
        }

        public static Comentario Obter(int id)
        {
            return Session.Current.Get<Comentario>(id);
        }

        public static void Deletar(int id)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var receita = Obter(id);
                    Session.Current.Delete(receita);
                    scope.Complete();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
