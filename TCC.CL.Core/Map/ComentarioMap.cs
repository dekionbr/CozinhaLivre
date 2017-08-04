using FluentNHibernate.Mapping;
using System;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Map
{
    public class ComentarioMap : ClassMap<Comentario>
    {
        public ComentarioMap() {
            Id(x => x.Id);

            Map(x => x.Data);
            Map(x => x.Titulo).Length(60);
            Map(x => x.NomeAnonimo).Length(60);
            Map(x => x.Conteudo);
            Map(x => x.Ativo);

            References<Usuario>(x => x.Autor);
            References<Receita>(x => x.Receita)
                .Not.LazyLoad();
            References<Comentario>(x => x.Parente)
                .NotFound.Ignore();

            HasMany<Comentario>(x => x.Filhos).Cascade.All();
        }
    }
}
