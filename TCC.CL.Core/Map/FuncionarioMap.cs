using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Map
{
    public class FuncionarioMap : SubclassMap<Funcionario>
    {
        public FuncionarioMap() {

            DiscriminatorValue(@"Funcionario");

            Map(x => x.RG).Not.Nullable();
            Map(x => x.CPF).Not.Nullable();
            Map(x => x.TipoFuncionario).CustomType<int>();

            References<Usuario>(x => x.Usuario)
                .Cascade.All();
        }
    }
}
