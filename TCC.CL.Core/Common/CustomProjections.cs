using NHibernate;
using NHibernate.Criterion;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.CL.Core.Common
{
    public static class CustomProjections
    {
        public static IProjection Concat(params IProjection[] projections)
        {
            return Projections.SqlFunction(
                "concat",
                NHibernateUtil.String,
                projections);
        }

        public static IProjection SqlGroupAndConcatFunction(string sql, string groupby, string[] columnAliases, IType[] types, string constantConcat)
        {
            return Concat(Projections.SqlGroupProjection(sql,
                                                         groupby,
                                                         columnAliases,
                                                         types),
                          Projections.Constant(constantConcat, NHibernateUtil.String));
        }


        public static IProjection SqlGroupAndConcatFunction(string sql, string groupby, string[] columnAliases, IType[] types, string constantConcat, IProjection projconcat)
        {
            return Concat(SqlGroupAndConcatFunction(sql, groupby, columnAliases, types, constantConcat),
                          projconcat);
        }

        public static IProjection SqlGroupAndConcatFunction(string sql, string groupby, string[] columnAliases, IType[] types, string constantConcat, params IProjection[] projsconcats)
        {
            return Concat(SqlGroupAndConcatFunction(sql, groupby, columnAliases, types, constantConcat),
                        Concat(projsconcats));
        }

        //public static ICriterion Or(params ICriterion[] criterias) {
        //    return Restrictions.
        //}
    }
}
