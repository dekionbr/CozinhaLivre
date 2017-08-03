using FluentNHibernate.Mapping;
using System;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Map
{
    public class PessoaFisicaMap : SubclassMap<PessoaFisica>
    {
        public PessoaFisicaMap() {

            DiscriminatorValue(@"PessoaFisica");

            Map(x => x.DataNascimento).Default("01/01/1900");

        }
    }
}
