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
    public static class PessoaJuridicaBusiness
    {
        public static List<PessoaJuridica> ObterLista()
        {
            return (from r in Session.Current.Query<PessoaJuridica>()
                    select r).ToList();
        }

        public static Paginacao<PessoaJuridica> ObterLista(int pagina)
        {
            return (from r in Session.Current.Query<PessoaJuridica>()
                    select r).ToListPaginado(pagina);
        }

        public static PessoaJuridica Obter(int Key)
        {
            return Session.Current.Query<PessoaJuridica>().Where(x => x.Id == Key).Take(1).ToList().Single();
        }

        public static bool Add(PessoaJuridica empresa)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    empresa = Session.Current.Merge<PessoaJuridica>(empresa);
                    Session.Current.SaveOrUpdate(empresa);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool Update(PessoaJuridica empresa)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    empresa = Session.Current.Merge<PessoaJuridica>(empresa);
                    Session.Current.SaveOrUpdate(empresa);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static IList<PessoaJuridica> ObterLista(int[] Patrocinadores)
        {
            if (Patrocinadores != null)
                return ObterLista().Where(x => Patrocinadores.Contains(x.Id)).ToList();
            else
                return ObterLista();
        }
    }
}
