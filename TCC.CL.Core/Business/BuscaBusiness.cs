using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using NHibernate.Linq;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Business
{
    public class BuscaBusiness
    {
        public static IList<Busca> ObterLista()
        {
            return (from b in Session.Current.Query<Busca>()
                    select b).ToList();
        }

        public static IList<string> ObterLista(string[] term)
        {
            return (from b in Session.Current.Query<Busca>()
                    select b.Termo).ToList<string>()
                                   .Distinct()
                                   .ToList();
        }

        public static bool Add(Busca busca)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    //busca = Session.Current.Merge<Busca>(busca);
                    Session.Current.Save(busca);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static void AddRange(string q, List<Receita> list)
        {
            if (list.Count() > 0)
                foreach (var item in list)
                    Add(new Busca(q, item));
            else
                Add(new Busca(q));
        }
    }
}
