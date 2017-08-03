using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Map
{
    public class AcessoMap : ClassMap<Acesso>
    {
        public AcessoMap()
        {
            Id(x => x.Id);

            Map(x => x.IP);
            Map(x => x.Data);
            Map(x => x.Navegador);
            Map(x => x.Origem);
            Map(x => x.Pagina);
            Map(x => x.Usuario);

        }
    }
}
