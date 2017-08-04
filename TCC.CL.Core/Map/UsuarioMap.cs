using FluentNHibernate.Mapping;
using System;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Map
{
    public class UsuarioMap : ClassMap<Usuario>
    {
        public UsuarioMap() {

            Id(x => x.Id);

            Map(x => x.DataAprovacao);
            Map(x => x.IsNovo);
            Map(x => x.EMail);
            Map(x => x.Login);
            Map(x => x.Senha);
            Map(x => x.FotoPerfil);
            Map(x => x.Grupo).CustomType<int>();
            
        }
    }
}
