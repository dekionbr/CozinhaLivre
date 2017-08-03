using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.CL.Core.Entities
{
    [Serializable]
    public class Acesso : BaseDomain<Acesso, int>
    {
        public Acesso(string usuario)
        {
            this.Data = DateTime.Now;
            this.Usuario = usuario;
        }

        public Acesso() { }

        public virtual string IP { get; set; }
        public virtual string Navegador { get; set; }
        public virtual string Pagina { get; set; }
        public virtual string Origem { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual string Usuario { get; set; }
    }
}
