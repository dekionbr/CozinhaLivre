using FluentNHibernate.Mapping;
using System;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Map
{
    public class PessoaJuridicaMap : SubclassMap<PessoaJuridica>
    {
        public PessoaJuridicaMap() {

            DiscriminatorValue(@"PessoaJuridica");

            Map(x => x.CNPJ);
            Map(x => x.RazaoSocial);

            HasMany(x => x.Autores)
                .Cascade.All();
        }
    }
}
