using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCC.CL.Web.Models
{
    [Serializable]
    public class ListaReceitas
    {
        public string Nome { get; set; }
        public string Resumo { get; set; }
        public string UrlImagem { get; set; }
        public bool IsPatrocinado { get; set; }
        public string Link { get; set; }
        public string[] Categorias { get; set; }
    }
}