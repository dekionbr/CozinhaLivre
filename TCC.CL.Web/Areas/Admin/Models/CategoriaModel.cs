using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCC.CL.Core.Entities;
using TCC.CL.Core.Enumeration;

namespace TCC.CL.Web.Areas.Admin.Models
{
    public class CategoriaModel
    {
        public IList<string> Buscas { get; set; }
        
        public ModeloDeCorteEnum Corte { get; set; }

        public bool InBuscas { get; set; }

        public DateTime DataInicial { get; set; }

        public DateTime DataFinal { get; set; }

        public int TopItens { get; set; }
    }
}