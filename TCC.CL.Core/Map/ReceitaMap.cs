using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.CL.Core.Entities;
using NHibernate.Linq;

namespace TCC.CL.Core.Map
{
    public class ReceitaMap : ClassMap<Receita>
    {
        public ReceitaMap()
        {
            Id(x => x.Id);

            Map(x => x.Titulo).Length(60);
            Map(x => x.Conteudo).Length(5000);
            Map(x => x.SubTitulo).Length(180);
            Map(x => x.Data);
            Map(x => x.Audiencia);
            Map(x => x.Destaque);
            Map(x => x.Status).CustomType<int>();
            Map(x => x.ConteudoResumido);

            References<Autor>(x => x.Autor)
                .Not.Nullable();

            References<Receita>(x => x.Parente)
                .Nullable().NotFound.Ignore();

            HasMany<Avaliacao>(x => x.Avalicoes)
                .Not.LazyLoad()
                .Cascade.None()
                .NotFound.Ignore();

            HasManyToMany<Categoria>(x => x.Categorias)
                .Not.LazyLoad()
                .Cascade.AllDeleteOrphan()
                .NotFound.Ignore();

            HasMany<Comentario>(x => x.Comentarios)
                .Not.LazyLoad()
                .Cascade.None()
                .NotFound.Ignore();

            HasMany<Imagem>(x => x.Imagens)
                .Not.LazyLoad()
                .Cascade.AllDeleteOrphan()
                .NotFound.Ignore();

        }
    }
}
