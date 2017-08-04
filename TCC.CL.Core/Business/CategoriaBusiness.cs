using NHibernate.Criterion;
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
    public static class CategoriaBusiness
    {
        public static List<Categoria> ObterLista(int[] ids)
        {
            if (ids != null)
                return Session.Current.Query<Categoria>()
                                      .Where(x => ids.Contains(x.Id))
                                      .ToList();
            else
                return Session.Current.Query<Categoria>().ToList();
        }

        public static List<Categoria> ObterLista()
        {
            return Session.Current.Query<Categoria>()
                .ToList();
        }

        public static bool Add(Categoria categoria)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    Session.Current.SaveOrUpdate(categoria);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static void AddCategorias(Categoria Categoria)
        {
            Add(Categoria);
        }

        public static bool Existe(string valor)
        {
            return Session.Current.Query<Categoria>().Any(x => x.Valor == valor);
        }

        public static void AddCategorias(string q)
        {
            List<string> palavras = SepararPalavras(q);

            IList<Categoria> Categorias = ObterLista();

            foreach (var cat in palavras)
            {
                if (!Existe(cat))
                    Add(new Categoria(cat));
            }

        }

        private static List<string> SepararPalavras(string q)
        {
            char[] separadores = new char[] { ' ', '-', '_', '+' };

            List<string> palavras = q.Split(separadores).ToList();

            if (palavras.Count > 1)
            {
                for (int i = 2; i < palavras.Count; i++)
                {
                    palavras.AddRange(q.Split(separadores, i).ToList());
                }
            }

            return palavras;
        }

        public static List<string> ObterListaDeValor()
        {
            return Session.Current.Query<Categoria>()
                .Select(x => x.Valor).Distinct()
                .ToList();

        }


        public static Paginacao<Categoria> ObterLista(int pagAtual)
        {
            return (from c in Session.Current.Query<Categoria>()
                       select c).ToListPaginado(pagAtual);
        }
    }
    
}
