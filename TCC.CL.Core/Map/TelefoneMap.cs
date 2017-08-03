using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Map
{
    public class TelefoneMap : ClassMap<Telefone>
    {
        public TelefoneMap() {

            Id(x => x.Id);

            Map(x => x.Key);

            Map(x => x.Value);
        }
    }
}
