using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using TCC.CL.Core.Entities;


namespace TCC.CL.Core.Map
{
    public class ImagemMap : ClassMap<Imagem>
    {
        public ImagemMap() {
            Id(x => x.Id);
            
            Map(x => x.Url);

            References(x => x.Receita);
        }
    }
}