using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.CL.Core.DTO;
using TCC.CL.Core.Common;
using TCC.CL.Core.Helper;
using TCC.CL.Core.Entities;
using TCC.CL.Core.Enumeration;
using TCC.CL.Core.Servico;
using System.Data;

namespace TCC.CL.Core.Business
{
    public static class RelatoriosBusiness
    {
        private static Random rnd = new Random();

        public static object ObterRelatorioReceitas(ModeloDeCorteEnum corte, int[] autoresId = null, string dataInicial = null, string dataFinal = null, int TopItens = 5)
        {
            DTOGrafico dtoGrafico = new DTOGrafico();

            try
            {

                Receita ReceitaAlias = null;
                Autor AutorAlias = null;

                var query = Session.Current.QueryOver<Receita>(() => ReceitaAlias);
                query = query.JoinAlias(() => ReceitaAlias.Autor, () => AutorAlias);

                query.Where(Restrictions.IsNull(Projections.Property(() => ReceitaAlias.Parente)));

                if (autoresId != null && autoresId.Count() > 0)
                    query.Where(Restrictions.On(() => AutorAlias.Id).IsIn(autoresId));

                if (dataInicial != null && dataFinal != null)
                {
                    DateTime DataInicial = DateTime.Parse(dataInicial).Date;
                    DateTime DataFinal = DateTime.Parse(dataFinal).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    query.Where(Restrictions.And(Restrictions.Ge(Projections.Property(() => ReceitaAlias.Data), DataInicial),
                                                 Restrictions.Le(Projections.Property(() => ReceitaAlias.Data), DataFinal)));
                }

                DTODado dto = new DTODado();

                var nomeAutorProperty = Projections.Property(() => AutorAlias.Nome);
                var nomeAutorGroup = Projections.GroupProperty(nomeAutorProperty);
                var nomeAlias = nomeAutorGroup.WithAlias(() => dto.Label);

                var quantAlias = Projections.RowCount().WithAlias(() => dto.Quantidade);

                var corteAlias = new SQLCorteService().Executar(corte).WithAlias(() => dto.Corte);


                var projections = Projections.ProjectionList()
                  .Add(nomeAlias)
                  .Add(quantAlias)
                  .Add(corteAlias)
                  ;

                var Dados = query.OrderBy(() => ReceitaAlias.Data).Asc()
                                     .ThenBy(() => AutorAlias.Nome).Asc()
                                     .Select(projections)
                                     .TransformUsing(Transformers.AliasToBean<DTODado>())
                                     .List<DTODado>();

                ObterGrafico(dtoGrafico, Dados, TopItens);

            }
            catch (Exception)
            {

                throw;
            }

            return dtoGrafico;
        }



        public static IList<DTOSelector> ObterAutores(int[] patrocinadores)
        {
            Autor AutorAlias = null;
            PessoaJuridica PatrocinadorAlias = null;

            var query = Session.Current.QueryOver<PessoaJuridica>(() => PatrocinadorAlias);
            query = query.JoinAlias(() => PatrocinadorAlias.Autores, () => AutorAlias, NHibernate.SqlCommand.JoinType.InnerJoin);

            query.Where(Restrictions.In(Projections.Property(() => PatrocinadorAlias.Id), patrocinadores));

            DTOSelector dto = null;

            var nomeAutorAlias = Projections.Property(() => AutorAlias.Nome)
                                                .WithAlias(() => dto.valor);

            var IDAutorAlias = Projections.Property(() => AutorAlias.Id)
                                                .WithAlias(() => dto.ID);

            var projections = Projections.ProjectionList()
                                .Add(nomeAutorAlias)
                                .Add(IDAutorAlias)
                                ;


            return query.OrderBy(() => AutorAlias.Nome).Asc()
                              .Select(projections)
                              .TransformUsing(Transformers.AliasToBean<DTOSelector>())
                              .List<DTOSelector>();
        }

        public static object ObterRelatorioReceitasPorBuscas(bool inBuscas, string[] buscas, ModeloDeCorteEnum corte, string dataInicial, string dataFinal, int TopItens = 5)
        {
            DTOGrafico dtoGrafico = new DTOGrafico();

            try
            {
                Busca BuscasAlias = null;
                Receita ReceitaAlias = null;

                var query = Session.Current.QueryOver<Busca>(() => BuscasAlias);
                query = query.JoinAlias(() => BuscasAlias.Receita, () => ReceitaAlias, NHibernate.SqlCommand.JoinType.LeftOuterJoin);

                query.Where(Restrictions.IsNull(Projections.Property(() => ReceitaAlias.Parente)));

                if (!inBuscas)
                    query.Where(Restrictions.IsNotNull(Projections.Property(() => BuscasAlias.Receita)));

                if (buscas != null && buscas.Count() > 0)
                    query.Where(Restrictions.On(() => BuscasAlias.Termo).IsIn(buscas));

                if (dataInicial != null && dataFinal != null)
                {
                    DateTime DataInicial = DateTime.Parse(dataInicial).Date;
                    DateTime DataFinal = DateTime.Parse(dataFinal).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    query.Where(Restrictions.And(Restrictions.Ge(Projections.Property(() => BuscasAlias.Data), DataInicial),
                                                 Restrictions.Le(Projections.Property(() => BuscasAlias.Data), DataFinal)));
                }


                DTODado dto = new DTODado();

                var nomeReceitaProperty = Projections.Conditional(Restrictions.IsNotNull(Projections.Property(() => BuscasAlias.Receita)),
                                                                  Projections.Cast(NHibernateUtil.String, Projections.Property(() => ReceitaAlias.Titulo)),
                                                                  Projections.Constant("Busca sem receita", NHibernateUtil.String));

                var nomeAlias = nomeReceitaProperty.WithAlias(() => dto.Label);

                var idReceitaGroup = Projections.GroupProperty(Projections.Property(() => BuscasAlias.Receita));

                var quantAlias = Projections.RowCount().WithAlias(() => dto.Quantidade);

                var corteAlias = new SQLCorteService().Executar(corte).WithAlias(() => dto.Corte);

                var projections = Projections.ProjectionList()
                  .Add(nomeAlias)
                  .Add(quantAlias)
                  .Add(corteAlias)
                  .Add(idReceitaGroup)
                  ;

                var Dados = query.OrderBy(() => BuscasAlias.Data).Asc()
                                .ThenBy(() => ReceitaAlias.Titulo).Asc()
                                .Select(projections)
                                .TransformUsing(Transformers.AliasToBean<DTODado>())
                                .List<DTODado>();

                ObterGrafico(dtoGrafico, Dados, TopItens);


            }
            catch (Exception)
            {

                throw;
            }

            return dtoGrafico;
        }

        public static object ObterRelatorioAcessos(bool inAnonimos, int[] navegadores, string[] usuariosId, string dataInicial, string dataFinal, ModeloDeCorteEnum corte, int TopItens = 5)
        {
            DTOGrafico dtoGrafico = new DTOGrafico();

            try
            {
                Acesso AcessoAlias = null;


                var query = Session.Current.QueryOver<Acesso>(() => AcessoAlias);

                if (!inAnonimos)
                    query.Where(Restrictions.IsNotNull(Projections.Property(() => AcessoAlias.Usuario)));

                if (navegadores != null && navegadores.Count() > 0)
                {
                    IList<string> NavegadoresList = new List<string>();
                    for (int i = 0; i < navegadores.Count(); i++)
                    {
                        NavegadoresList.Add(((NavegadorEnum)navegadores[i]).GetDescription());
                    }

                    query = query.Where(Restrictions.On(() => AcessoAlias.Navegador).IsIn(NavegadoresList.ToArray()));
                }

                if (dataInicial != null && dataFinal != null)
                {
                    DateTime DataInicial = DateTime.Parse(dataInicial).Date;
                    DateTime DataFinal = DateTime.Parse(dataFinal).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    query.Where(Restrictions.And(Restrictions.Ge(Projections.Property(() => AcessoAlias.Data), DataInicial),
                                                 Restrictions.Le(Projections.Property(() => AcessoAlias.Data), DataFinal)));
                }


                DTODado dto = new DTODado();


                var PaginaGroup = Projections.GroupProperty(Projections.Property(() => AcessoAlias.Pagina));
                var nomeAlias = PaginaGroup.WithAlias(() => dto.Label);

                var quantAlias = Projections.RowCount().WithAlias(() => dto.Quantidade);

                var corteAlias = new SQLCorteService().Executar(corte).WithAlias(() => dto.Corte);

                var projections = Projections.ProjectionList()
                  .Add(nomeAlias)
                  .Add(quantAlias)
                  .Add(corteAlias)
                  .Add(PaginaGroup)
                  ;

                var Dados = query.OrderBy(() => AcessoAlias.Data).Asc()
                                     .ThenBy(() => AcessoAlias.Navegador).Asc()
                                     .Select(projections)
                                     .TransformUsing(Transformers.AliasToBean<DTODado>())
                                     .List<DTODado>();

                ObterGrafico(dtoGrafico, Dados, TopItens);

            }
            catch (Exception)
            {

                throw;
            }

            return dtoGrafico;
        }

        private static void ObterGrafico(DTOGrafico dtoGrafico, IList<DTODado> dados, int topItens)
        {
            dtoGrafico.datasets = dados.GroupBy(x => new
            {
                label = x.Label
            })
            .Take(topItens)
            .Select(x =>
            new DTOItem()
            {
                label = x.Key.label,
                data = dados.Where(dado => dado.Label == x.Key.label).Select(item => item.Quantidade).ToList(),
                backgroundColor = "rgba(" + rnd.Next(0, 255) + "," + rnd.Next(0, 255) + "," + rnd.Next(0, 255) + ", 0.8)"

            }).ToList();

            dtoGrafico.labels = dados.Select(x => x.Corte).Distinct().ToList();
        }

        public static object ObterQuantidadeCategoria()
        {
            Categoria CategoriaAlias = null;
            Receita ReceitaAlias = null;

            var query = Session.Current.QueryOver<Receita>(() => ReceitaAlias);
            query = query.JoinAlias(() => ReceitaAlias.Categorias, () => CategoriaAlias, NHibernate.SqlCommand.JoinType.LeftOuterJoin);

            DTODado dto = new DTODado();

            var nomeAlias = Projections.Property(() => CategoriaAlias.Valor)
                                              .WithAlias(() => dto.Label);

            var categoriaGroup = Projections.GroupProperty(Projections.Property(() => CategoriaAlias.Valor));

            var quantAlias = Projections.RowCount().WithAlias(() => dto.Quantidade);

            var projections = Projections.ProjectionList()
                 .Add(nomeAlias)
                 .Add(quantAlias)
                 .Add(categoriaGroup)
                 ;

            var Dados = query.OrderBy(Projections.RowCount()).Asc()
                               .ThenBy(() => CategoriaAlias.Valor).Asc()
                               .Select(projections)
                               .TransformUsing(Transformers.AliasToBean<DTODado>())
                               .List<DTODado>();
            return Dados;
        }

        #region Excel

        public static DataTable ObterPatrocinadoresExcel()
        {
            var table = new System.Data.DataTable("Patrocinadores");
            table.Columns.Add("Razão Social", typeof(string));
            table.Columns.Add("Nome Fantasia", typeof(string));
            table.Columns.Add("CNPJ", typeof(string));

            IList<PessoaJuridica> patrocinadores = PessoaJuridicaBusiness.ObterLista();

            var contatos = patrocinadores.SelectMany(x => x.Telefones).Distinct().ToList();
            table = ObterColunasContato(table, contatos);


            foreach (var patro in patrocinadores)
            {
                IList<object> parametros = new List<object>() { patro.RazaoSocial, patro.NomeFantasia, patro.CNPJ };

                foreach (var contato in patro.Telefones)
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        if (contato.Key != null && table.Columns[i].ColumnName == contato.Key)
                        {
                            parametros.Add(contato.Value);
                            break;
                        }
                        else if (contato.Key == null && 
                                 table.Columns[i].ColumnName == "Telefone" && 
                                 parametros.Count < table.Columns.Count)
                        {
                            parametros.Add(contato.Value);
                            break;
                        }                            
                    }
                }

                table.Rows.Add(parametros.ToArray());
            }

            return table;
        }

        public static DataTable ObterFuncionariosExcel()
        {
            throw new NotImplementedException();
        }

        public static DataTable ObterAutoresExcel()
        {
            throw new NotImplementedException();
        }

        private static DataTable ObterColunasContato(DataTable table, IList<Telefone> telefones)
        {
            ISet<string> colunasContato = new HashSet<string>(telefones.Select(x => x.Key).Distinct());

            foreach (var contato in colunasContato)
            {
                table.Columns.Add(contato ?? "Telefone", typeof(string));
            }

            return table;
        }
        
        #endregion
    }
}
