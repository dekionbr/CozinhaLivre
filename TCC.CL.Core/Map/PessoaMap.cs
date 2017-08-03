using System;
using TCC.CL.Core.Entities;
using FluentNHibernate.Mapping;

namespace TCC.CL.Core.Map
{
    public class PessoaMap : ClassMap<Pessoa>
    {
        public PessoaMap() {

            Id(x => x.Id);

            Map(x => x.Nome);
            Map(x => x.DataCadastro);
            Map(x => x.Ativo);
            Map(x => x.UF).CustomType<int>();
            Map(x => x.Cidade);
            Map(x => x.Bairro);
            Map(x => x.Cep);
            Map(x => x.Logradouro);
            Map(x => x.Numero);
            Map(x => x.NumSuspensoes);
            Map(x => x.Complemento);            

            HasMany<Telefone>(x => x.Telefones)
                .Cascade.AllDeleteOrphan();
        }
    }
}
