using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.CL.Core.Entities
{
    [Serializable]
    public class Categoria : BaseDomain<Categoria, int>
    {
        public Categoria() { }

        public Categoria(string Valor)
        {
            // TODO: Complete member initialization
            this.Valor = Valor;
        }
        
        public virtual string Valor { get; set; }

        //public virtual IList<Receita> Receitas { get; set; }
    }
}
