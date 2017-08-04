using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.CL.Core.Entities;
using NHibernate.Linq;
using System.Transactions;
using NHibernate;

namespace TCC.CL.Core.Business
{
    public static class ReceitaBusiness
    {
        public static List<Receita> ObterLista()
        {
            return (from r in Session.Current.Query<Receita>()
                    select r).ToList();
        }

        public static IQueryable<Receita> ByQuery()
        {
            return Session.Current.Query<Receita>();
        }

        //public static IList<Receita> ObterTodos(string q)
        //{
        //    return (from r in Session.Current.Query<Receita>()
        //            where r.Parente == null
        //            select r).ToList();
        //}

        public static List<Receita> ObterTodos(string q)
        {
            List<Receita> receitas = (from r in Session.Current.Query<Receita>()
                                      where r.Parente == null &&
                                            (r.Conteudo.Contains(q) ||
                                             r.Titulo.Contains(q))
                                      orderby r.Data descending
                                      select r).ToList();

            foreach (var rec in receitas)
            {
                BuscaBusiness.Add(new Busca(q, rec));
            }

            return receitas;
        }

        internal static IQueryable<Receita> ObterTodos()
        {
            var query = from r in Session.Current.Query<Receita>()
                        where r.Parente == null
                        orderby r.Data descending
                        select r;

            return query;
        }

        public static List<Receita> ObterReceitaPorLista(string[] itensbuscas)
        {
            var receitas = new HashSet<Receita>();

            foreach (var item in itensbuscas)
            {
                var receitasnovas = ObterTodos().Where(x => x.Titulo.Contains(item) || x.Conteudo.Contains(item)).ToList();

                foreach (var receita in receitasnovas)
                {
                    receitas.Add(receita);
                }
            }

            return receitas.OrderBy(x => x.IsPatrocinado).ToList();
        }

        public static Paginacao<Receita> ObterTodos(int pagAtual, string q)
        {
            if (!string.IsNullOrEmpty(q))
            {
                string[] itensbuscas = q.Split('+');

                if (itensbuscas.Length > 1)
                {
                    List<Receita> lista = ObterReceitaPorLista(itensbuscas);

                    return new Paginacao<Receita>(pagAtual, lista.Count(), lista);
                }
                else
                {

                    IList<Receita> receitas = ObterTodos()
                            .Where(r => r.Titulo.Contains(q) ||
                                        r.Conteudo.Contains(q))
                                        .ToList();

                    return new Paginacao<Receita>(pagAtual, receitas.Count(), receitas);
                }
            }
            else
            {
                return ObterTodos().ToListPaginado(pagAtual);
            }
        }

        #region Incluir, Deletar e Obter

        public static bool Add(Receita receita)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    //receita = Session.Current.Merge<Receita>(receita);

                    Session.Current.SaveOrUpdate(receita);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static Receita Obter(int id)
        {
            var receita = Session.Current.Get<Receita>(id);

            //if (NHibernateUtil.IsInitialized(receita.Categorias))
            //    NHibernateUtil.Initialize(receita.Categorias);

            //if (NHibernateUtil.IsInitialized(receita.Imagens))
            //    NHibernateUtil.Initialize(receita.Imagens);

            //if (NHibernateUtil.IsInitialized(receita.Avalicoes))
            //    NHibernateUtil.Initialize(receita.Avalicoes);

            //if (NHibernateUtil.IsInitialized(receita.Comentarios))
            //    NHibernateUtil.Initialize(receita.Comentarios);

            return receita;
        }

        public static bool AddParent(Receita receita, Receita OldReceita)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    receita = Session.Current.Merge<Receita>(receita);
                    Session.Current.Save(receita);
                    OldReceita.Parente = receita;
                    OldReceita = Session.Current.Merge<Receita>(OldReceita);
                    Session.Current.Update(OldReceita);
                    //Session.Current.Flush();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool Save(Receita receita)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    receita = Session.Current.Merge<Receita>(receita);
                    Session.Current.Save(receita);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private static bool Update(Receita receita)
        {

            try
            {
                using (var scope = new TransactionScope())
                {
                    receita = Session.Current.Merge<Receita>(receita);
                    Session.Current.Update(receita);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool Retornar(int id)
        {
            try
            {
                Receita receita = Session.Current.Get<Receita>(id);
                Receita RecuperaReceita = Session.Current.Query<Receita>().Where(x => x.Parente.Id == id).FirstOrDefault();

                using (var scope = new TransactionScope())
                {
                    Session.Current.Delete(receita);

                    if (RecuperaReceita != null)
                    {
                        RecuperaReceita.Parente = null;
                        Session.Current.Update(RecuperaReceita);
                    }

                    scope.Complete();
                }

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool Deletar(int id)
        {

            try
            {
                Receita receita = Session.Current.Get<Receita>(id);
                Receita OldReceita = Session.Current.Query<Receita>().Where(x => x.Parente.Id == id).FirstOrDefault();

                using (var scope = new TransactionScope())
                {
                    Session.Current.Delete(receita);

                    if (OldReceita != null)
                    {
                        Deletar(OldReceita.Id);
                    }

                    scope.Complete();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion Incluir, Deletar e Obter

        public static List<Receita> ObterListaPatrocinada(string q)
        {
            return ObterTodos(1, q).Where(x => x.IsPatrocinado).Take(3).ToList();
        }

        public static Paginacao<Receita> ObterTodos(int pagAtual)
        {
            return ObterTodos().ToListPaginado(pagAtual);
        }

        public static Paginacao<Receita> ObterTodos(int pagAtual, bool isPatrocinado)
        {
            if (!isPatrocinado)
                return ObterTodos().Where(x => x.Autor.Empresa != null).ToListPaginado(pagAtual);
            else
                return ObterTodos().ToListPaginado(pagAtual);
        }

        public static Paginacao<Receita> ObterTodos(int pagAtual, Autor autor)
        {
            return ObterTodos().Where(x => x.Autor == autor).ToListPaginado(pagAtual);
        }

        public static bool DeleteImagem(int idImagem)
        {
            try
            {
                Imagem imagem = Session.Current.Get<Imagem>(idImagem);
                using (var scope = new TransactionScope())
                {
                    Session.Current.Delete(imagem);
                    scope.Complete();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

    }
}
