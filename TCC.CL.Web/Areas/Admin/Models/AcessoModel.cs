using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCC.CL.Core.Entities;
using TCC.CL.Core.Enumeration;

namespace TCC.CL.Web.Areas.Admin.Models
{
    public class AcessoModel
    {
        public IList<Usuario> Usuarios { get; set; }
        public NavegadorEnum Navegadores { get; set; }
        public bool InAnonimos { get; set; }
        public ModeloDeCorteEnum Corte { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }

        public int TopItens { get; set; }
    }
}