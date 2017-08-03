using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Map
{
    public class BuscaMap : ClassMap<Busca>
    {
        public BuscaMap()
        {
            Id(x => x.Id);
            Map(x => x.Termo);
            Map(x => x.Data);

            References<Receita>(x => x.Receita)
                .NotFound.Ignore()
                .Cascade.None();
        }
    }
}
