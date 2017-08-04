using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.CL.Core.Entities
{
    public class Comentario : BaseDomain<Int32>
    {
        public virtual Autor Autor { get; set; }
        public virtual Receita Receita { get; set; }
        public virtual string Conteudo { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual Comentario Parente { get; set; }
    }
}
