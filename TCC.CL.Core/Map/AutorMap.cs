using System;
using FluentNHibernate.Mapping;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Map
{
    public class AutorMap : SubclassMap<Autor>
    {
        public AutorMap()
        {
            DiscriminatorValue(@"Autor");

            References<PessoaJuridica>(x => x.Empresa)
                    .Not.LazyLoad()
                    .NotFound.Ignore();

            References<Usuario>(x => x.Usuario)
                .Cascade.All();
        }
    }
}
