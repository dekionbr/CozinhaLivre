using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TCC.CL.Core.Entities
{
    [Serializable]
    public class Imagem : BaseDomain<Imagem, int>
    {
        public Imagem() { }

        public Imagem(string url)
        {
            this.Url = url;
        }

        public virtual string Url { get; set; }

        public virtual Receita Receita { get; set; }        
    }   
}
