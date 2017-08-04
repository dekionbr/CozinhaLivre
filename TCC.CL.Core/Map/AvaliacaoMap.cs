using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Map
{
    public class AvaliacaoMap : ClassMap<Avaliacao>
    {
        public AvaliacaoMap() {
            Id(x => x.Id);

            Map(x => x.DataAvaliacao);
            Map(x => x.Nivel).CustomType<int>();

            References<Usuario>(x => x.Avaliador);
        }
    }
}
