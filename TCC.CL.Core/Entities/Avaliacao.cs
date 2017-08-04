using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCC.CL.Core.Enumeration;

namespace TCC.CL.Core.Entities
{
    [Serializable]
    public class Avaliacao : BaseDomain<Avaliacao, int>
    {
        public virtual Nivel Nivel { get; set; }
        public virtual DateTime DataAvaliacao { get; set; }

        public virtual Usuario Avaliador { get; set; }
    }
}
