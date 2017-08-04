using FluentNHibernate.Mapping;
using System;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Map
{
    public class CategoriaMap : ClassMap<Categoria>
    {
        public CategoriaMap()
        {
            Id(x => x.Id);

            Map(x => x.Valor);

            //HasManyToMany<Receita>(x => x.Receitas).LazyLoad()
            //    .NotFound.Ignore();
        }
    }
}
