using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCC.CL.Core.Common;
using TCC.CL.Core.Entities;
using TCC.CL.Core.Enumeration;

namespace TCC.CL.Core.Servico
{
    public class SQLCorteService
    {
        public IProjection Executar(ModeloDeCorteEnum corte)
        {
            switch (corte)
            {
                case ModeloDeCorteEnum.Diario:
                    return GroupAndConcatDia();
                case ModeloDeCorteEnum.Semanal:
                    return GroupAndConcatSemanal();
                case ModeloDeCorteEnum.Mensal:
                    return GroupAndConcatMensal();
                case ModeloDeCorteEnum.Trimestral:
                    return GroupAndConcatTrimestral();
                case ModeloDeCorteEnum.Semestral:
                    return GroupAndConcatSemestral();
                case ModeloDeCorteEnum.Anual:
                    return GroupAndConcatAnual();
            }

            return GroupAndConcatDia();
        }

        private IProjection GroupAndConcatSemestral()
        {
            return CustomProjections.Concat(Projections.Conditional(Restrictions.Ge(Projections.SqlFunction("MONTH",
                                                                                   NHibernateUtil.Int16,
                                                                                   Projections.Property("Data")), 7),
                                           Projections.Constant("2º Semestre de ", NHibernateUtil.String),
                                           Projections.Constant("1º Semestre de ", NHibernateUtil.String)),
                                           GroupAndConcatAnual());
        }

        public IProjection GroupAndConcatSemanal()
        {
            return CustomProjections.SqlGroupAndConcatFunction("WEEK({alias}.Data) as SEMANA",
                                                               "WEEK({alias}.Data)",
                                                               new[] { "SEMANA" },
                                                               new[] { NHibernateUtil.String },
                                                               "ª Semana do Ano de ",
                                                               GroupAndConcatAnual());
        }

        public IProjection GroupAndConcatMensal()
        {
            return CustomProjections.SqlGroupAndConcatFunction("MONTH({alias}.Data) as MES",
                                                               "MONTH({alias}.Data)",
                                                               new[] { "MES" },
                                                               new[] { NHibernateUtil.String },
                                                               "/",
                                                               GroupAndConcatAnual());
        }

        public IProjection GroupAndConcatTrimestral()
        {
            return CustomProjections.SqlGroupAndConcatFunction("QUARTER({alias}.Data) as TRIMESTRE",
                                                               "QUARTER({alias}.Data)",
                                                               new[] { "TRIMESTRE" },
                                                               new[] { NHibernateUtil.String },
                                                               "º Trimestre de ",
                                                               GroupAndConcatAnual());
        }

        public IProjection GroupAndConcatDia()
        {
            return CustomProjections.SqlGroupAndConcatFunction("DAY({alias}.Data) as DIA",
                                                               "DAY({alias}.Data)",
                                                               new[] { "DIA" },
                                                               new[] { NHibernateUtil.String },
                                                               "/",
                                                               GroupAndConcatMensal());
        }

        public IProjection GroupAndConcatAnual()
        {

            return Projections.Cast(NHibernateUtil.String,
                                    Projections.SqlGroupProjection("YEAR({alias}.Data) as ANO",
                                                 "YEAR({alias}.Data)",
                                                  new[] { "ANO" },
                                                  new[] { NHibernateUtil.String }));
        }
    }
}
