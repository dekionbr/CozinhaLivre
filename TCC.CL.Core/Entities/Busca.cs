using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.CL.Core.Entities
{
    [Serializable]
    public class Busca : BaseDomain<Busca, int>
    {
        public Busca(string termo, Receita receita)
        {
            // TODO: Complete member initialization
            this.Termo = termo;
            this.Receita = receita;
            this.Data = DateTime.Now;
        }

        public Busca(string termo)
        {
            // TODO: Complete member initialization
            this.Termo = termo;
            this.Data = DateTime.Now;
        }

        public Busca() {
            this.Data = DateTime.Now;
        }

        public virtual Receita Receita { get; set; }
        public virtual string Termo { get; set; }
        public virtual DateTime Data { get; set; }
    }
}
